using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WMP.Core;

namespace WMP.Api.Controllers
{
    public class WMPUTController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> WMPUT_0006()
        {
            string tokenCheckUrl = ConfigurationManager.AppSettings["TOKEN_CHECK_URL"];
            var parameters = HttpUtil.QueryStringToDictionary();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(tokenCheckUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["BUSAN_BANK_TOKEN"] = parameters["TOKEN"];
                string queryString = query.ToString();
                var response = await httpClient.GetAsync(queryString);

                response.EnsureSuccessStatusCode();
                return Ok(response.IsSuccessStatusCode);
            }
            
        }
    }
}
