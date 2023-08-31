// SPDX-FileCopyrightText: 2023 Unity Technologies and the KTX for Unity authors
// SPDX-License-Identifier: Apache-2.0


using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace KtxUnity
{
    abstract class TextureImporter : ScriptedImporter
    {

        /// <summary>
        /// Texture array layer to import.
        /// </summary>
        public uint layer;

        /// <summary>
        /// Cubemap face or 3D/volume texture slice to import.
        /// </summary>
        public uint faceSlice;

        /// <summary>
        /// Lowest mipmap level to import (where 0 is the highest resolution).
        /// Lower mipmap levels (of higher resolution) are being discarded.
        /// Useful to limit texture resolution.
        /// </summary>
        public uint levelLowerLimit;

        /// <summary>
        /// If true, a mipmap chain (if present) is imported.
        /// </summary>
        public bool importLevelChain = true;

        /// <summary>
        /// If true, texture will be sampled
        /// in linear color space (sRGB otherwise)
        /// </summary>
        public bool linear;

        // ReSharper disable once NotAccessedField.Local
        [SerializeField]
        [HideInInspector]
        string[] reportItems;

        public override void OnImportAsset(AssetImportContext ctx)
        {
            Profiler.BeginSample("Import Texture");
            var texture = CreateTextureBase();
            Profiler.BeginSample("Load Texture");
            var result = AsyncHelpers.RunSync(() =>
            {
                using (var alloc = new ManagedNativeArray(File.ReadAllBytes(assetPath)))
                {
                    return texture.LoadFromBytes(
                        alloc.nativeArray,
                        linear,
                        layer,
                        faceSlice,
                        levelLowerLimit,
                        importLevelChain
                        );
                }
            });
            Profiler.EndSample();

            if (result.errorCode == ErrorCode.Success)
            {
                result.texture.name = name;
                result.texture.alphaIsTransparency = true;
                ctx.AddObjectToAsset("result", result.texture);
                ctx.SetMainObject(result.texture);
                reportItems = new string[] { };
            }
            else
            {
                var errorMessage = ErrorMessage.GetErrorMessage(result.errorCode);
                reportItems = new[] { errorMessage };
                Debug.LogError($"Could not load texture file at {assetPath}: {errorMessage}", this);
            }

            Profiler.EndSample();
        }

        protected abstract TextureBase CreateTextureBase();

        // from glTFast : AsyncHelpers
        static class AsyncHelpers
        {
            /// <summary>
            /// Executes an async Task&lt;T&gt; method which has a T return type synchronously
            /// </summary>
            /// <typeparam name="T">Return Type</typeparam>
            /// <param name="task">Task&lt;T&gt; method to execute</param>
            /// <returns></returns>
            public static T RunSync<T>(Func<Task<T>> task)
            {
                var oldContext = SynchronizationContext.Current;
                var sync = new ExclusiveSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(sync);
                var ret = default(T);
                // ReSharper disable once AsyncVoidLambda
                sync.Post(async _ =>
                {
                    try
                    {
                        ret = await task();
                    }
                    catch (Exception e)
                    {
                        sync.InnerException = e;
                        throw;
                    }
                    finally
                    {
                        sync.EndMessageLoop();
                    }
                }, null);
                sync.BeginMessageLoop();
                SynchronizationContext.SetSynchronizationContext(oldContext);
                return ret;
            }

            class ExclusiveSynchronizationContext : SynchronizationContext
            {
                bool m_Done;
                public Exception InnerException { get; set; }
                readonly AutoResetEvent m_WorkItemsWaiting = new AutoResetEvent(false);
                readonly Queue<Tuple<SendOrPostCallback, object>> m_Items =
                    new Queue<Tuple<SendOrPostCallback, object>>();

                public override void Send(SendOrPostCallback d, object state)
                {
                    throw new NotSupportedException("We cannot send to our same thread");
                }

                public override void Post(SendOrPostCallback d, object state)
                {
                    lock (m_Items)
                    {
                        m_Items.Enqueue(Tuple.Create(d, state));
                    }
                    m_WorkItemsWaiting.Set();
                }

                public void EndMessageLoop()
                {
                    Post(_ => m_Done = true, null);
                }

                public void BeginMessageLoop()
                {
                    while (!m_Done)
                    {
                        Tuple<SendOrPostCallback, object> task = null;
                        lock (m_Items)
                        {
                            if (m_Items.Count > 0)
                            {
                                task = m_Items.Dequeue();
                            }
                        }
                        if (task != null)
                        {
                            task.Item1(task.Item2);
                            if (InnerException != null) // the method threw an exception
                            {
                                throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
                            }
                        }
                        else
                        {
                            m_WorkItemsWaiting.WaitOne();
                        }
                    }
                }

                public override SynchronizationContext CreateCopy()
                {
                    return this;
                }
            }
        }
    }
}
