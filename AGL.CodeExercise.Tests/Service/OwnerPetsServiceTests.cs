using AGL.CodeExercise.Common.ApplicationConfiguration;
using AGL.CodeExercise.Services.Pets;
using Microsoft.Extensions.Logging;
using Moq;
using AGL.CodeExercise.Tests.MockExtensions;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Newtonsoft.Json;
using AGL.CodeExercise.Common;

namespace AGL.CodeExercise.Tests.Service
{
    public class OwnerPetsServiceTests
    {
        private IOwnerPetsService _ownerPetsService;
        private readonly Mock<ILogger<OwnerPetsService>> _mockLogger;
        private readonly AppSettings _appSettings;
        private HttpClient _httpClient = new HttpClient();


        public OwnerPetsServiceTests()
        {
            _mockLogger = new Mock<ILogger<OwnerPetsService>>();
            _appSettings = ApplicationSettings();
            _httpClient = _httpClient.MockObject(HttpStatusCode.OK, JsonTestData());
            _ownerPetsService = new OwnerPetsService(_httpClient, _mockLogger.Object, _appSettings);
        }

        #region Private Test Data Setup

        private string IncorrectJsonData()
        {
            return @"[{""name"":""Owner1"",""gender"":""Male"",""age"":23,""pets"":
                                                                [{""name"":""Cat1"",""type"":""Cat""},
                                                                 {""name"":""Dog1"",""type"":""Dog""},
                      {""name"":""Owner2"",""gender"":""Female"",""age"":18,""pets"":
                                                                [{""name"":""Cat2"",""type"":""Cat""}]}]";
        }

        private string JsonTestData()
        {
            return @"[{""name"":""Owner1"",""gender"":""Male"",""age"":23,""pets"":
                                                                [{""name"":""Cat1"",""type"":""Cat""},
                                                                 {""name"":""Dog1"",""type"":""Dog""}]},
                      {""name"":""Owner2"",""gender"":""Female"",""age"":18,""pets"":
                                                                [{""name"":""Cat2"",""type"":""Cat""}]},
                      {""name"":""Owner3"",""gender"":""Male"",""age"":45,""pets"":
                                                                null},
                      {""name"":""Owner4"",""gender"":""Male"",""age"":40,""pets"":
                                                                [{""name"":""Cat3"",""type"":""Cat""},
                                                                 {""name"":""Cat4"",""type"":""Cat""},
                                                                 {""name"":""Dog2"",""type"":""Dog""},
                                                                 {""name"":""Cat5"",""type"":""Cat""}]},
                      {""name"":""Owner5"",""gender"":""Female"",""age"":40,""pets"":
                                                                [{""name"":""Cat6"",""type"":""Cat""}]},
                      {""name"":""Owner6"",""gender"":""Female"",""age"":64,""pets"":
                                                                [{""name"":""Cat7"",""type"":""Cat""},
                                                                {""name"":""Fish1"",""type"":""Fish""}]},
                      {""name"":""Owner7"",""gender"":""Male"",""age"":23,""pets"":
                                                                [{""name"":""Cat8"",""type"":""Cat""},
                                                                 {""name"":""Dog3"",""type"":""Dog""}]},
                      {""name"":""Owner8"",""gender"":""Female"",""age"":18,""pets"":
                                                                null},
                      {""name"":""Owner9"",""gender"":""Male"",""age"":45,""pets"":
                                                                null},
                      {""name"":""Owner10"",""gender"":""Male"",""age"":40,""pets"":
                                                                [{""name"":""Dog4"",""type"":""Dog""},
                                                                 {""name"":""Cat9"",""type"":""Cat""}]},
                      {""name"":""Owner11"",""gender"":""Female"",""age"":40,""pets"":
                                                                [{""name"":""Cat10"",""type"":""Cat""}]},
                      {""name"":""Owner12"",""gender"":""Female"",""age"":64,""pets"":
                                                                [{""name"":""Fish2"",""type"":""Fish""}]}]";
        }

        private AppSettings ApplicationSettings()
        {
            return new AppSettings() { DeveloperTest = new DeveloperTest() { Url = "http://agl-api-demo.com" } };
        }

        #endregion

        #region Facts
        /// <summary>
        /// Tests the GetOwnersAndPetsAsync method with correct Json Data.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetOwnersAndPetsAsync_Successful_Test()
        {
            var ownerDetails = await _ownerPetsService.GetOwnersAndPetsAsync();
            Assert.NotEmpty(ownerDetails);
            Assert.Equal(12, ownerDetails.Count);
            Assert.Equal(6, ownerDetails.Count(detail => detail.OwnerGender == "Male"));
            Assert.Equal(6, ownerDetails.Count(detail => detail.OwnerGender == "Female"));
            Assert.Equal(16, (int)ownerDetails.Sum(detail => detail.Pets?.Count));
            
        }

        /// <summary>
        /// Test GetOwnersAndPetsAsync method for JsonSerializationException when json data is not correct.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetOwnersAndPetsAsync_UnSuccessful_With_JsonSerializationException()
        {
            _httpClient = _httpClient.MockObject(HttpStatusCode.OK, IncorrectJsonData());
            _ownerPetsService = new OwnerPetsService(_httpClient, _mockLogger.Object, _appSettings);
            await Assert.ThrowsAsync<JsonSerializationException>(async () => await _ownerPetsService.GetOwnersAndPetsAsync());
        }

        /// <summary>
        /// Test GetOwnersAndPetsAsync method for HttpResponseException when http response is not OK.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetOwnersAndPetsAsync_UnSuccessful_With_HttpResponseException()
        {
            _httpClient = _httpClient.MockObject(HttpStatusCode.BadRequest, IncorrectJsonData());
            _ownerPetsService = new OwnerPetsService(_httpClient, _mockLogger.Object, _appSettings);
            var exception = await Assert.ThrowsAsync<HttpResponseException>(async () => await _ownerPetsService.GetOwnersAndPetsAsync());
            Assert.Equal(HttpStatusCode.BadRequest.ToString().Trim(), exception.ResponseErrorCode.Trim());
            Assert.Equal(Error.APIReponseError.Trim(), exception.Message.Trim());
        }
        #endregion
    }
}
