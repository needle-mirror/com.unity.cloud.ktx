// SPDX-FileCopyrightText: 2025 Unity Technologies and the KTX for Unity authors
// SPDX-License-Identifier: Apache-2.0

using System;

namespace KtxUnity
{
    static class KtxErrorCodeExtensions
    {
        public static ErrorCode ToErrorCode(this KtxErrorCode code)
        {
            switch (code)
            {
                case KtxErrorCode.Success:
                    return ErrorCode.Success;
                case KtxErrorCode.TranscodeFailed:
                    return ErrorCode.TranscodeFailed;
                case KtxErrorCode.UnknownFileFormat:
                case KtxErrorCode.UnsupportedTextureType:
                    return ErrorCode.UnsupportedFormat;
                case KtxErrorCode.FileDataError:
                case KtxErrorCode.FileIsPipe:
                case KtxErrorCode.FileOpenFailed:
                case KtxErrorCode.FileOverflow:
                case KtxErrorCode.FileReadError:
                case KtxErrorCode.FileSeekError:
                case KtxErrorCode.FileUnexpectedEof:
                case KtxErrorCode.FileWriteError:
                case KtxErrorCode.GLError:
                case KtxErrorCode.InvalidOperation:
                case KtxErrorCode.InvalidValue:
                case KtxErrorCode.NotFound:
                case KtxErrorCode.OutOfMemory:
                case KtxErrorCode.UnsupportedFeature:
                case KtxErrorCode.LibraryNotLinked:
                default:
                    return ErrorCode.LoadingFailed;
            }
        }
    }
}
