using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WMP.Core {
    public class ErrorResult : IHttpActionResult
    {
        Error error;
        HttpRequestMessage requestMessage;

        public ErrorResult(Error error, HttpRequestMessage requestMessage)
        {
            this.error = error;
            this.requestMessage = requestMessage;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new ObjectContent<Error>(this.error, new JsonMediaTypeFormatter()),
                RequestMessage = this.requestMessage
            };

            return Task.FromResult(response);
        }
    }
}
