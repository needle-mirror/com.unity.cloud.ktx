// SPDX-FileCopyrightText: 2023 Unity Technologies and the KTX for Unity authors
// SPDX-License-Identifier: Apache-2.0


using System;
using UnityEngine;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace KtxUnity
{
    [ScriptedImporter(0, new[] { ".ktx2" })]
    class KtxImporter : TextureImporter
    {

        protected override TextureBase CreateTextureBase()
        {
            return new KtxTexture();
        }
    }
}
