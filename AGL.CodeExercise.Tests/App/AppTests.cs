using System;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using AGL.CodeExercise.ServiceManagers;
using System.IO;
using System.Collections.Generic;
using AGL.CodeExercise.Common.ViewModels;
using System.Threading.Tasks;
using AGL.CodeExercise.Common;
using AGL.CodeExercise.App;

namespace AGL.CodeExercise.Tests.App
{
    public class AppTests
    {
        private readonly IPetsController _petsController;
        private readonly Mock<ILogger<PetsController>> _mockLogger;
        private readonly Mock<IOwnerPetsServiceManager> _mockPetsServiceManager;
        private readonly StringWriter _output;
        private readonly StringReader _input;

        public AppTests()
        {
            _mockLogger = new Mock<ILogger<PetsController>>();
            _mockPetsServiceManager = new Mock<IOwnerPetsServiceManager>();
            _output = new StringWriter();
            _input = new StringReader("init");
            _petsController = new PetsController(_mockPetsServiceManager.Object, _mockLogger.Object, _input, _output);
        }

        /// <summary>
        /// Test the data output format produced by GetCatsPerOwnerGender method
        /// </summary>
        [Fact]
        public async Task Check_Output_Data_format_with_SuccessfulData()
        {
            var ownerPetViewModel = new List<OwnerPetViewModel>
            {
                new OwnerPetViewModel() {OwnerGender = "Male", PetNames = new List<string>{"Cat1", "Cat2", "Cat3"} },
                new OwnerPetViewModel() {OwnerGender = "Female", PetNames = new List<string>{"Cat1", "Cat4"} }
            };

            var expectedOutputResult = "Male" + Environment.NewLine
                + " -> " + "Cat1" + Environment.NewLine + " -> " + "Cat2" + Environment.NewLine
                + " -> " + "Cat3" + Environment.NewLine
                + Environment.NewLine + Environment.NewLine
                + "Female" + Environment.NewLine
                + " -> " + "Cat1" + Environment.NewLine + " -> " + "Cat4" + Environment.NewLine
                + Environment.NewLine + Environment.NewLine;

            

            _mockPetsServiceManager.Setup(obj => obj.GetAllOwnerWithPetTypeAsync(Common.PetType.Cat)).Returns(Task.FromResult(ownerPetViewModel));

            await _petsController.GetCatsPerOwnerGender();

            var petsControllerActualResults = _output.ToString();

            Assert.Equal(expectedOutputResult, petsControllerActualResults);

        }

        /// <summary>
        /// Test if API failure exceptions are handled as expected.
        /// </summary>
        [Fact]
        public async Task Check_If_Exception_Handled_And_Logged()
        {
            _mockPetsServiceManager.Setup(obj => obj.GetAllOwnerWithPetTypeAsync(Common.PetType.Cat))
                .Throws(new Exception("Error 1"));
            
            await _petsController.GetCatsPerOwnerGender();
            var exceptionString = _output.ToString();
            Assert.Equal(Error.APICallFailed.Trim(), exceptionString.Trim());
        }

        /// <summary>
        /// Test if API error response exceptions are handled as expected.
        /// </summary>
        [Fact]
        public async Task Check_If_HttpResponseException_Handled_And_Logged()
        {
            _mockPetsServiceManager.Setup(obj => obj.GetAllOwnerWithPetTypeAsync(Common.PetType.Cat))
                .Throws(new HttpResponseException(Error.APIReponseError, System.Net.HttpStatusCode.BadRequest.ToString()));

            var expectedOutput = Error.APIReponseError + System.Net.HttpStatusCode.BadRequest.ToString();

            await _petsController.GetCatsPerOwnerGender();
            var exceptionString = _output.ToString();
            Assert.Equal(expectedOutput.Trim(), exceptionString.Trim());
        }
    }
}
