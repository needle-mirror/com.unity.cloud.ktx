// Copyright (c) 2019-2022 Andreas Atteneder, All Rights Reserved.

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//    http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// TODO: Re-using transcoders does not work consistently. Fix and enable!
// #define POOL_TRANSCODERS

using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Profiling;
using Unity.Jobs;
using Unity.Collections;

namespace KtxUnity
{
    static class BasisUniversal
    {
        static bool s_Initialized;
        static int s_TranscoderCountAvailable = 8;

#if POOL_TRANSCODERS
        static Stack<TranscoderInstance> transcoderPool;
#endif

        static void InitInternal()
        {
            s_Initialized = true;
            TranscodeFormatHelper.Init();
            ktx_basisu_basis_init();
            s_TranscoderCountAvailable = SystemInfo.processorCount;
        }

        public static BasisUniversalTranscoderInstance GetTranscoderInstance()
        {
            if (!s_Initialized)
            {
                InitInternal();
            }
#if POOL_TRANSCODERS
            if(transcoderPool!=null) {
                return transcoderPool.Pop();
            }
#endif
            if (s_TranscoderCountAvailable > 0)
            {
                s_TranscoderCountAvailable--;
                return new BasisUniversalTranscoderInstance(ktx_basisu_create_basis());
            }
            else
            {
                return null;
            }
        }

        public static void ReturnTranscoderInstance(BasisUniversalTranscoderInstance transcoder)
        {
#if POOL_TRANSCODERS
            if(transcoderPool==null) {
                transcoderPool = new Stack<TranscoderInstance>();
            }
            transcoderPool.Push(transcoder);
#endif
            s_TranscoderCountAvailable++;
        }

        internal static JobHandle LoadBytesJob(
            ref BasisUniversalJob job,
            BasisUniversalTranscoderInstance basis,
            TranscodeFormat transF,
            bool mipChain = true
        )
        {

            Profiler.BeginSample("BasisU.LoadBytesJob");

            var numLevels = basis.GetLevelCount(job.layer);
            var levelsNeeded = mipChain ? numLevels - job.mipLevel : 1;
            var sizes = new NativeArray<uint>((int)levelsNeeded, KtxNativeInstance.defaultAllocator);
            var offsets = new NativeArray<uint>((int)levelsNeeded, KtxNativeInstance.defaultAllocator);
            uint totalSize = 0;
            for (var i = 0u; i < levelsNeeded; i++)
            {
                var level = job.mipLevel + i;
                offsets[(int)i] = totalSize;
                var size = basis.GetImageTranscodedSize(job.layer, level, transF);
                sizes[(int)i] = size;
                totalSize += size;
            }

            job.format = transF;
            job.sizes = sizes;
            job.offsets = offsets;
            job.nativeReference = basis.nativeReference;

            job.textureData = new NativeArray<byte>((int)totalSize, KtxNativeInstance.defaultAllocator);

            var jobHandle = job.Schedule();

            Profiler.EndSample();
            return jobHandle;
        }

        [DllImport(KtxNativeInstance.interfaceDLL)]
        static extern void ktx_basisu_basis_init();

        [DllImport(KtxNativeInstance.interfaceDLL)]
        static extern IntPtr ktx_basisu_create_basis();
    }
}
