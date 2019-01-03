using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Sockets;

namespace WMP.Core
{
    public class HttpUtil
    {
        public static Dictionary<string, string> QueryStringToDictionary()
        {
            var qs = HttpContext.Current.Request.QueryString;
            var parameters = new Dictionary<string, string>();
            parameters = qs.Keys.Cast<string>().Where(x => !string.IsNullOrEmpty(x))
                .ToDictionary(k => k, v => qs[v].Replace("'", string.Empty), StringComparer.InvariantCultureIgnoreCase);

            return parameters;
        }

        public static string GetClientIp()
        {
            string ipv4 = IPAddress.Loopback.ToString();

            var context = HttpContext.Current;

            if (context == null) return ipv4;

            var ipaddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipaddress))
            {
                var addresses = ipaddress.Split(',');
                if (addresses.Length != 0) return addresses[0];
            }

            foreach (var ip in Dns.GetHostAddresses(context.Request.UserHostAddress))
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            if (ipv4 != IPAddress.Loopback.ToString()) return ipv4;

            return context.Request.UserHostAddress;
        }

        public static string GetIp()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList[0].ToString();
        }

        public static bool IsLocal()
        {
            var context = HttpContext.Current;
            return (context != null) ? context.Request.IsLocal : false;
        }
    }
}
