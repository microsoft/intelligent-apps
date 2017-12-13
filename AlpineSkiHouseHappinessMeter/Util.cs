﻿// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: http://www.microsoft.com/cognitive
// 
// Microsoft Cognitive Services Github:
// https://github.com/Microsoft/Cognitive
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using ServiceHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Rest;
using Microsoft.ProjectOxford.Face;

namespace IntelligentKioskSample
{
    internal static class Util
    {
        internal static async Task GenericApiCallExceptionHandler(Exception ex, string errorTitle)
        {
            string errorDetails = GetMessageFromException(ex);

            await new MessageDialog(errorDetails, errorTitle).ShowAsync();
        }
        internal static string GetMessageFromException(Exception ex)
        {
            string errorDetails = ex.Message;

            FaceAPIException faceApiException = ex as FaceAPIException;
            if (faceApiException?.ErrorMessage != null)
            {
                errorDetails = faceApiException.ErrorMessage;
            }

            Microsoft.ProjectOxford.Common.ClientException commonException = ex as Microsoft.ProjectOxford.Common.ClientException;
            if (commonException?.Error?.Message != null)
            {
                errorDetails = commonException.Error.Message;
            }

            HttpOperationException httpException = ex as HttpOperationException;
            if (httpException?.Response?.ReasonPhrase != null)
            {
                errorDetails = string.Format("{0}. The error message was \"{1}\".", ex.Message, httpException?.Response?.ReasonPhrase);
            }

            return errorDetails;
        }
        public static async Task<IEnumerable<string>> GetAvailableCameraNamesAsync()
        {
            DeviceInformationCollection deviceInfo = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            return deviceInfo.OrderBy(d => d.Name).Select(d => d.Name);
        }
		internal static async Task<byte[]> GetPixelBytesFromSoftwareBitmapAsync(SoftwareBitmap softwareBitmap)
		{
			using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
			{
				BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
				encoder.SetSoftwareBitmap(softwareBitmap);
				await encoder.FlushAsync();

				// Read the pixel bytes from the memory stream
				using (var reader = new DataReader(stream.GetInputStreamAt(0)))
				{
					var bytes = new byte[stream.Size];
					await reader.LoadAsync((uint)stream.Size);
					reader.ReadBytes(bytes);
					return bytes;
				}
			}
		}
    }
}