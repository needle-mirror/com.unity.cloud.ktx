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

namespace KtxUnity
{
    interface IMetaData
    {
        bool hasAlpha { get; }
    }

    interface ILevelInfo
    {
        bool isPowerOfTwo { get; }
        bool isMultipleOfFour { get; }
        bool isSquare { get; }
    }

    class MetaData : IMetaData
    {
        public bool hasAlpha { get; set; }

        public ImageInfo[] images;

        public void GetSize(out uint width, out uint height, uint imageIndex = 0, uint levelIndex = 0)
        {
            var level = images[imageIndex].levels[levelIndex];
            width = level.width;
            height = level.height;
        }

        public override string ToString()
        {
            return $"BU images:{images.Length} A:{hasAlpha}";
        }
    }

    class ImageInfo
    {
        public LevelInfo[] levels;
        public override string ToString()
        {
            return $"Image levels:{levels.Length}";
        }
    }

    class LevelInfo : ILevelInfo
    {
        public uint width;
        public uint height;

        public static bool IsPowerOfTwo(uint i)
        {
            return (i & (i - 1)) == 0;
        }

        public static bool IsMultipleOfFour(uint i)
        {
            return (i & 0x3) == 0;
        }

        public bool isPowerOfTwo => IsPowerOfTwo(width) && IsPowerOfTwo(height);

        public bool isMultipleOfFour => IsMultipleOfFour(width) && IsMultipleOfFour(height);

        public bool isSquare => width == height;

        public override string ToString()
        {
            return $"Level size {width} x {height}";
        }
    }
}
