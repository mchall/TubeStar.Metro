using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.ApplicationModel;

namespace TubeStarMetro
{
    public static class WebClientHelpers
    {
        public static async Task<T> Download<T>(Uri uri)
        {
            try
            {
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                var responseMsg = await client.GetAsync(uri);
                var result = await responseMsg.Content.ReadAsStringAsync();

                var item = JsonConvert.DeserializeObject<T>(result);
                return item;
            }
            catch
            {
                return default(T);
            }
        }

        public static async Task<Stream> DownloadImage(Uri imageUri)
        {
            try
            {
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                var responseMsg = await client.GetAsync(imageUri);
                var result = await responseMsg.Content.ReadAsStreamAsync();
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}