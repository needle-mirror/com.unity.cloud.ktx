// Copyright (c) 2020-2022 Andreas Atteneder, All Rights Reserved.

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//    http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


namespace KtxUnity
{

    /// <summary>
    /// See Section 5.2 in https://registry.khronos.org/KTX/specs/2.0/ktxspec.v2.html#_ktxorientation
    /// </summary>
    [System.Flags]
    public enum TextureOrientation
    {
        /// <summary>
        /// KTX defaults to X=right Y=down Z=out
        /// </summary>
        KtxDefault = 0x0,
        /// <summary>
        /// If present X=left, else X=right
        /// </summary>
        XLeft = 0x1,
        /// <summary>
        /// If present Y=up, else Y=down
        /// </summary>
        YUp = 0x2,
        /// <summary>
        /// If present Z=in, else Z=out
        /// </summary>
        ZIn = 0x4, // Not used at the moment
        /// <summary>
        /// Unity expects GPU textures to be X=right Y=up
        /// </summary>
        UnityDefault = YUp,
    }

    /// <summary>
    /// Extensions to check if a texture's orientation conforms to Unity's default.
    /// </summary>
    public static class TextureOrientationExtension
    {

        /// <summary>
        /// Evaluates if the texture's horizontal orientation conforms to Unity's default.
        /// If it's not aligned (=true; =flipped), the texture has to be applied mirrored horizontally.
        /// </summary>
        /// <param name="to"></param>
        /// <returns>True if the horizontal orientation is flipped, false otherwise</returns>
        public static bool IsXFlipped(this TextureOrientation to)
        {
            // Unity default == X_RIGHT
            return (to & TextureOrientation.XLeft) != 0;
        }

        /// <summary>
        /// Evaluates if the texture's vertical orientation conforms to Unity's default.
        /// If it's not aligned (=true; =flipped), the texture has to be applied mirrored vertically.
        /// </summary>
        /// <param name="to"></param>
        /// <returns>True if the vertical orientation is flipped, false otherwise</returns>
        public static bool IsYFlipped(this TextureOrientation to)
        {
            // Unity default == Y_UP
            return (to & TextureOrientation.YUp) == 0;
        }
    }
}
