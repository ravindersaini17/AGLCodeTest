using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AGL.CodeExercise.Services
{
    public abstract class HttpOperations
    {
        #region Private variables

        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        #endregion

        #region Constructor
        protected HttpOperations(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;

        }
        #endregion

        #region HttpOperations
        protected async Task<HttpResponseMessage> GetDataAsync(string uri)
        {
            try
            {
                var httpResponse = await this._httpClient.GetAsync(uri);
                return httpResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("API: '{RequestUri}' Error: ", uri, ex.InnerException?.Message);
                throw;
            }
        }
        #endregion
    }
}
