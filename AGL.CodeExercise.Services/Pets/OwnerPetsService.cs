using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AGL.CodeExercise.Common;
using AGL.CodeExercise.Common.ApplicationConfiguration;
using AGL.CodeExercise.Common.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AGL.CodeExercise.Services.Pets
{
    public class OwnerPetsService : HttpOperations, IOwnerPetsService
    {
        #region Private variables
        private readonly ILogger<OwnerPetsService> _logger;
        private readonly AppSettings _appSettings;
        #endregion

        #region Constructor
        public OwnerPetsService(HttpClient httpClient, ILogger<OwnerPetsService> logger, AppSettings appSettings) : base(httpClient, logger)
        {
            _logger = logger;
            _appSettings = appSettings;

            //set the Base address for HttpClient
            httpClient.BaseAddress = new Uri(_appSettings.DeveloperTest.Url);
        }
        #endregion

        #region IPetService Implementation

        /// <summary>
        /// Make a call to API and get Owners and Pets raw data
        /// </summary>
        /// <returns>Returns OwnerDetails model</returns>
        public async Task<List<OwnerDetails>> GetOwnersAndPetsAsync()
        {
            List<OwnerDetails> ownerDetails = null;
            //Get pets service endpoint from API
            var apiServiceEndPoint = _appSettings.DeveloperTest.PetsServiceEndpoint;

            //Get data using pets service endpoint
            var httpResponse = await GetDataAsync(apiServiceEndPoint);
            _logger.LogInformation($"Call to {apiServiceEndPoint} returned with status code {httpResponse.StatusCode}");

            //Deserialize Json data if response is successful.
            if (!httpResponse.IsSuccessStatusCode)
                throw new HttpResponseException(Error.APIReponseError, httpResponse.StatusCode.ToString());

            try
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                ownerDetails = JsonConvert.DeserializeObject<List<OwnerDetails>>(content);
                _logger.LogInformation("Owner Pets data received!");
                return ownerDetails;
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError(Error.DataDeserializeError + ":" + ex.Message, ex.InnerException);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException);
                throw;
            }
            
        }

        #endregion


    }
}
