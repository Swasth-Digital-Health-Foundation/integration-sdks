using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Io.HcxProtocol.Utils
{
    /**
     * Library  : Io.Hcx.Protocol
     * Author   : WalkingTree Technologies
     * Date     : 15-Mar-2023
     * All Rights Reserved. WalkingTree Technologies.
     **/

    /// <summary>
    /// The REST API Utility to make http request.
    /// </summary>
    public class HttpUtils
    {
        public static Dto.HttpResponse Post(string url, Dictionary<string, string> headers, string requestBody)
        {
            using (var client = new HttpClient())
            {
                foreach (var header in headers)
                {
                    if (header.Key == "Authorization")
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header.Value.Replace("Bearer ", ""));
                    else if (header.Key == "Content-Type")
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(header.Value));
                    else
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                var request = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, request).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                return new Dto.HttpResponse((int)response.StatusCode, responseString);
            }
        }

        public static Dto.HttpResponse Post(string url, Dictionary<string, string> headers, Dictionary<string, string> fields)
        {
            using (var client = new HttpClient())
            {
                foreach (var header in headers)
                {
                    if (header.Key == "Authorization")
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header.Value.Replace("Bearer ", ""));
                    else if (header.Key == "Content-Type")
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(header.Value));
                    else
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                client.DefaultRequestHeaders.Clear();                
                FormUrlEncodedContent formData = new FormUrlEncodedContent(fields);//.Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value.ToString())));
                var response = client.PostAsync(url, formData).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                return new Dto.HttpResponse((int)response.StatusCode, responseString);
            }
        }

        public static void DownloadFile(string uri, string outputFilePath)
        {
            Uri uriResult;
            if (!Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
                throw new InvalidOperationException("URI is invalid.");

            if (File.Exists(outputFilePath))
                File.Delete(outputFilePath);

            using (var client = new HttpClient())
            {
                byte[] fileBytes = client.GetByteArrayAsync(uri).Result;
                File.WriteAllBytes(outputFilePath, fileBytes);
            }
        }

    }
}
