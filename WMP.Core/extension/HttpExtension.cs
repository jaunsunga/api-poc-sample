using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace WMP.Core
{
    public static class HttpExtension
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent httpContent)
        {
            var stream = await httpContent.ReadAsStreamAsync();
            if (stream.CanSeek) stream.Position = 0;
            var jsonReader = new JsonTextReader(new StreamReader(stream));
            var json = jsonReader.ReadAsString();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
