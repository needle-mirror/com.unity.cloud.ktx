// SPDX-FileCopyrightText: 2023 Unity Technologies and the KTX for Unity authors
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Profiling;

namespace KtxUnity
{
    static class BasisUniversal
    {
#pragma warning disable UDR0001
        // s_NativeLibraryInitialized is intentionally not reset in `ResetStaticsOnLoad`
        // as the native library should only be initialized once.
        static bool s_NativeLibraryInitialized;
#pragma warning restore UDR0001
        static int s_TranscoderCountAvailable = SystemInfo.processorCount;

        public static BasisUniversalTranscoderInstance GetTranscoderInstance()
        {
            TranscodeFormatHelper.Init();
            if (!s_NativeLibraryInitialized)
            {
                s_NativeLibraryInitialized = true;
                ktx_basisu_basis_init();
            }

            // TODO: Pool transcoder instances instead of just counting available ones.

            if (s_TranscoderCountAvailable > 0)
            {
                s_TranscoderCountAvailable--;
                return new BasisUniversalTranscoderInstance(ktx_basisu_create_basis());
            }

            return null;
        }

        public static void ReturnTranscoderInstance(BasisUniversalTranscoderInstance transcoder)
        {
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

        [DllImport(KtxNativeInstance.ktxLibrary)]
        static extern void ktx_basisu_basis_init();

        [DllImport(KtxNativeInstance.ktxLibrary)]
        static extern IntPtr ktx_basisu_create_basis();

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStaticsOnLoad()
        {
            // Reset static state
            s_TranscoderCountAvailable = SystemInfo.processorCount;
        }
#endif
    }
}
