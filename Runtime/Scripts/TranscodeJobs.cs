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

using System.Runtime.InteropServices;
using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using IntPtr = System.IntPtr;

namespace KtxUnity
{
    unsafe struct BasisUniversalJob : IJob
    {
        [WriteOnly]
        public NativeArray<bool> result;

        [ReadOnly]
        public TranscodeFormat format;

        [ReadOnly]
        public uint mipLevel;

        [ReadOnly]
        [NativeDisableUnsafePtrRestriction]
        public IntPtr nativeReference;

        [ReadOnly]
        public NativeArray<uint> sizes;

        [ReadOnly]
        public NativeArray<uint> offsets;

        [ReadOnly]
        public uint layer;

        [WriteOnly]
        public NativeArray<byte> textureData;

        public void Execute()
        {
            var success = ktx_basisu_startTranscoding(nativeReference);
            var textureDataPtr = textureData.GetUnsafePtr();
            for (uint i = 0; i < offsets.Length; i++)
            {
                success = success &&
                ktx_basisu_transcodeImage(
                    nativeReference,
                    (byte*)textureDataPtr + offsets[(int)i],
                    sizes[(int)i],
                    layer,
                    mipLevel + i,
                    (uint)format,
                    0,
                    0
                    );
                if (!success) break;
            }
            result[0] = success;
        }

        [DllImport(KtxNativeInstance.interfaceDLL)]
        static extern bool ktx_basisu_startTranscoding(IntPtr basis);

        [DllImport(KtxNativeInstance.interfaceDLL)]
        static extern bool ktx_basisu_transcodeImage(
            IntPtr basis,
            void* dst,
            uint dstSize,
            uint imageIndex,
            uint levelIndex,
            uint format,
            uint pvrtcWrapAddressing,
            uint getAlphaForOpaqueFormats
            );
    }

    struct KtxTranscodeJob : IJob
    {

        [WriteOnly]
        public NativeArray<KtxErrorCode> result;

        [ReadOnly]
        [NativeDisableUnsafePtrRestriction]
        public IntPtr nativeReference;

        [ReadOnly]
        public TranscodeFormat outputFormat;

        public void Execute()
        {
            result[0] = KtxNativeInstance.ktxTexture2_TranscodeBasis(
                nativeReference,
                outputFormat,
                0 // transcodeFlags
                );
        }
    }
}
