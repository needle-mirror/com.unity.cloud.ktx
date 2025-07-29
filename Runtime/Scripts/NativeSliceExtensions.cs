// SPDX-FileCopyrightText: 2025 Unity Technologies and the KTX for Unity authors
// SPDX-License-Identifier: Apache-2.0

using System;
using System.IO;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Assertions;

namespace KtxUnity
{
    static class NativeSliceExtensions
    {
        [Obsolete("Use the overload that accepts a NativeArray<byte>.ReadOnly data")]
        public static unsafe NativeArray<byte> AsNativeArray(this NativeSlice<byte> src)
        {
            if (src.Stride != 1)
            {
                throw new InvalidDataException("Only NativeSlice<byte> with a stride of 1 is supported!");
            }
            var array = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(
                src.GetUnsafeReadOnlyPtr(),
                src.Length,
                Allocator.None
                );
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var safetyHandle = AtomicSafetyHandle.Create();
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(array: ref array, safetyHandle);
#endif
            return array;
        }
    }
}
