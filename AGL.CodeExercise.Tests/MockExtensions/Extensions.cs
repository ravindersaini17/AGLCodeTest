using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace AGL.CodeExercise.Tests.MockExtensions
{
    public static class Extensions
    {
        public static HttpClient MockObject(this HttpClient httpClient,HttpStatusCode httpStatusResponseCode, string responseContent)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
               //protected method mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               //Setup expected response for mock object
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = httpStatusResponseCode,
                   Content = new StringContent(responseContent),
               })
               .Verifiable();

            //use real http client with mocked handler here
            httpClient = new HttpClient(mockHandler.Object);

            return httpClient;

        }
    }
}
