using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;

namespace WMP.Core
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            return base.HandleAsync(context, cancellationToken);
        }

        public override async void Handle(ExceptionHandlerContext context)
        {
            var ctx = context.RequestContext;
            string action = ctx.RouteData.Values["action"].ToString();
            var requestType = HttpContext.Current.Request.RequestType;
            var method = context.Request.Method;
            var bp = new Dictionary<string, string>();
            var qs = HttpUtil.QueryStringToDictionary();

            if ( method == HttpMethod.Get ) { }
            else if (method == HttpMethod.Post) {
                var rawBody = await context.Request.Content.ReadAsJsonAsync<Dictionary<string, object>>();
                bp = rawBody.ToStringDictionary();
            }

            base.Handle(context);
        }

        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }
    }
}
