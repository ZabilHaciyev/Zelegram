using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zelegram.Models;

namespace Zelegram.Services
{
    public static class ObjectSender
    {
        public static async Task<HttpResponseMessage> Request(HttpMethod method, SendType type, string jsonContent,HttpClient client)
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = method;
            httpRequestMessage.RequestUri = new Uri($"http://localhost:8888/{Enum.GetName(typeof(SendType), type)}/");

            switch (method.Method)
            {
                case "POST":
                    HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    httpRequestMessage.Content = httpContent;
                    break;

            }

            return await client.SendAsync(httpRequestMessage);
        }
        public static async Task<string> GetResponse(SendType type, string jsonContent)
        {
            using (var client = new HttpClient())
            {
                var uri =($"http://localhost:8888/{Enum.GetName(typeof(SendType), type)}/");
                HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await client.PostAsync(uri, httpContent);
                if (result.IsSuccessStatusCode)
                {
                    return await Task.Run(async () =>
                    {
                        return await result.Content.ReadAsStringAsync();
                    });
                }
                else return null;
            }
        }
    }
}
