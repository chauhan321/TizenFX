/*
* Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
*
* Licensed under the Apache License, Version 2.0 (the License);
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an AS IS BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Native = Interop.ThumbnailExtractor;

namespace Tizen.Multimedia
{
    static internal class ThumbnailExtractorLog
    {
        internal const string LogTag = "Tizen.Multimedia.ThumbnailExtractor";
    }

    /// <summary>
    /// The Thumbnail extractor class provides a set of functions to extract the thumbnail data of the input media file
    /// </summary>
    public class ThumbnailExtractor : IDisposable
    {
        private bool _disposed = false;
        internal IntPtr _handle = IntPtr.Zero;

        /// <summary>
        /// Thumbnail extractor constructor
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <remarks>
        /// If you need the thumbnail of the specified size, use ThumbnailExtractor(path, width, height) or ThumbnailExtractor(path, size).
        /// </remarks>
        /// <param name="path"> The path of the media file to extract the thumbnail </param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        public ThumbnailExtractor(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            ThumbnailExtractorError ret = Native.Create(out _handle);
            ThumbnailExtractorErrorFactory.ThrowIfError(ret, "Failed to create constructor");

            try
            {
                ThumbnailExtractorErrorFactory.ThrowIfError(
                    Native.SetPath(_handle, path), "Failed to set the path");
            }
            catch (Exception)
            {
                Native.Destroy(_handle);
                _handle = IntPtr.Zero;
                throw;
            }
        }

        private void Create(String path, int width, int height)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "The width must be greater than zero:[" + width + "]");
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), "The height must be greater than zero:[" + height + "]");
            }

            ThumbnailExtractorError ret = Native.Create(out _handle);
            ThumbnailExtractorErrorFactory.ThrowIfError(ret, "Failed to create constructor");

            try
            {
                ThumbnailExtractorErrorFactory.ThrowIfError(
                    Native.SetPath(_handle, path), "Failed to set the path");

                ThumbnailExtractorErrorFactory.ThrowIfError(
                    Native.SetSize(_handle, width, height), "Failed to set the size");
            }
            catch (Exception)
            {
                Native.Destroy(_handle);
                _handle = IntPtr.Zero;
                throw;
            }
        }

        /// <summary>
        /// Thumbnail extractor constructor
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <remarks>
        /// If you need the thumbnail of the default size, use ThumbnailExtractor(path). The default size is 320x240. \n
        /// If the set width is not a multiple of 8, it can be changed by inner process. \n
        /// The width will be a multiple of 8 greater than the set value.
        /// </remarks>
        /// <param name="path"> The path of the media file to extract the thumbnail </param>
        /// <param name="width"> The width of the thumbnail </param>
        /// <param name="height"> The height of the thumbnail </param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="width"/> or <paramref name="height"/> is less than zero.
        /// </exception>
        public ThumbnailExtractor(string path, int width, int height)
        {
            Create(path, width, height);
        }

        /// <summary>
        /// Thumbnail extractor constructor
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <remarks>
        /// If you need the thumbnail of the default size, use ThumbnailExtractor(path). The default size is 320x240. \n
        /// If the set width is not a multiple of 8, it can be changed by inner process. \n
        /// The width will be a multiple of 8 greater than the set value.
        /// </remarks>
        /// <param name="path"> The path of the media file to extract the thumbnail </param>
        /// <param name="size"> The size to extract the thumbnail </param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// A value of <paramref name="size"/> is less than zero.
        /// </exception>
        public ThumbnailExtractor(string path, Size size)
        {
            Create(path, size.Width, size.Height);
        }

        /// <summary>
        /// Extracts the thumbnail for the given media, asynchronously
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <returns>
        /// Task for creation of Thumbnail. See <see cref="ThumbnailData"/> details.
        /// </returns>
        /// <exception cref="ArgumentException">Requested <paramref name="path"/> does not exist.</exception>
        /// <exception cref="OutOfMemoryException">Memory allocation failed.</exception>
        /// <exception cref="InvalidOperationException">Internal processing failed.</exception>
        /// <exception cref="UnauthorizedAccessException">Inaccessible for the requested <paramref name="path"/>.</exception>
        public Task<ThumbnailData> Extract()
        {
            if (_handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException(nameof(ThumbnailExtractor), "Failed to extract thumbnail");
            }

            var task = new TaskCompletionSource<ThumbnailData>();

            Native.ThumbnailExtractCallback extractCallback = (ThumbnailExtractorError error,
                                                                                string requestId,
                                                                                int thumbWidth,
                                                                                int thumbHeight,
                                                                                IntPtr thumbData,
                                                                                int thumbSize,
                                                                                IntPtr userData) =>
            {
                if (error == ThumbnailExtractorError.None)
                {
                    try
                    {
                        byte[] tmpBuf = new byte[thumbSize];
                        Marshal.Copy(thumbData, tmpBuf, 0, thumbSize);

                        task.SetResult(new ThumbnailData(tmpBuf, thumbWidth, thumbHeight));
                    }
                    catch (Exception)
                    {
                        task.SetException(new InvalidOperationException("[" + error + "] Fail to copy data"));
                    }
                    finally
                    {
                        LibcSupport.Free(thumbData);
                    }
                }
                else
                {
                    task.SetException(new InvalidOperationException("[" + error + "] Fail to create thumbnail"));
                }
            };

            IntPtr id = IntPtr.Zero;
            ThumbnailExtractorError res = Native.Extract(_handle, extractCallback, IntPtr.Zero, out id);
            if (id != IntPtr.Zero)
            {
                LibcSupport.Free(id);
                id = IntPtr.Zero;
            }

            ThumbnailExtractorErrorFactory.ThrowIfError(res, "Failed to extract thumbnail");

            return task.Task;
        }

        /// <summary>
        /// Thumbnail Utility destructor
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        ~ThumbnailExtractor()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // To be used if there are any other disposable objects
                }

                if (_handle != IntPtr.Zero)
                {
                    Native.Destroy(_handle);
                    _handle = IntPtr.Zero;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
