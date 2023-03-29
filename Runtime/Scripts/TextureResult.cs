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


using UnityEngine;

namespace KtxUnity
{

    /// <summary>
    /// TextureResult encapsulates result of texture loading. The texture itself and its orientation.
    /// </summary>
    public class TextureResult
    {
        /// <summary>
        /// The successfully imported <see cref="Texture2D"/>.
        /// </summary>
        public Texture2D texture;
        /// <summary>
        /// The <see cref="TextureOrientation"/> of the imported texture.
        /// </summary>
        public TextureOrientation orientation;
        /// <summary>
        /// The <see cref="ErrorCode"/> from the failed texture import.
        /// </summary>
        public ErrorCode errorCode = ErrorCode.Success;

        /// <summary>
        /// Creates an empty <see cref="TextureResult"/>.
        /// </summary>
        public TextureResult() { }

        /// <summary>
        /// Creates an invalid <see cref="TextureResult"/> with an <see cref="ErrorCode"/>.
        /// </summary>
        /// <param name="errorCode">The <see cref="ErrorCode"/> from the failed texture import.</param>
        public TextureResult(ErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }

        /// <summary>
        /// Creates a successful <see cref="TextureResult"/> with a <see cref="Texture2D"/>
        /// and a <see cref="TextureOrientation"/>.
        /// </summary>
        /// <param name="texture">The successfully imported <see cref="Texture2D"/>.</param>
        /// <param name="orientation">The <see cref="TextureOrientation"/> of the imported texture.</param>
        public TextureResult(Texture2D texture, TextureOrientation orientation)
        {
            this.texture = texture;
            this.orientation = orientation;
        }
    }
}
