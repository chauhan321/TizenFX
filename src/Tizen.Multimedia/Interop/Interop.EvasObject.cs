﻿/*
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
using System.Runtime.InteropServices;

namespace Tizen.Multimedia
{
    internal static partial class Interop
    {
        internal static partial class EvasObject
        {
            [DllImport("libevas.so.1")]
            internal static extern IntPtr evas_object_image_add(IntPtr parent);
            [DllImport("libevas.so.1")]
            internal static extern IntPtr evas_object_evas_get(IntPtr obj);
            [DllImport("libevas.so.1")]
            internal static extern void evas_object_show(IntPtr obj);
        }
    }
}