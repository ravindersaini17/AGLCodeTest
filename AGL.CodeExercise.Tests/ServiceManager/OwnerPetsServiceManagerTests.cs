using System.Collections.Generic;
using System.Threading.Tasks;
using AGL.CodeExercise.Common.Models;
using AGL.CodeExercise.ServiceManagers;
using AGL.CodeExercise.ServiceManagers.ServiceManagers;
using AGL.CodeExercise.Services.Pets;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Linq;
using AGL.CodeExercise.Common;

namespace AGL.CodeExercise.Tests.ServiceManager
{
    public class OwnerPetsServiceManagerTests
    {
        #region Private fields

        private readonly IOwnerPetsServiceManager _ownerPetsServiceManager;
        private readonly Mock<ILogger<OwnerPetsServiceManager>> _mockLogger;
        private readonly Mock<IOwnerPetsService> _mockPetsService;
        private List<OwnerDetails> _ownerDetails;

        #endregion

        #region Constructor
        public OwnerPetsServiceManagerTests()
        {
            InitiateData();
            _mockPetsService = new Mock<IOwnerPetsService>();
            _mockLogger = new Mock<ILogger<OwnerPetsServiceManager>>();
            _mockPetsService.Setup(obj => obj.GetOwnersAndPetsAsync()).Returns(Task.FromResult(_ownerDetails));
            _ownerPetsServiceManager = new OwnerPetsServiceManager(_mockPetsService.Object, _mockLogger.Object);
        }
        #endregion

        #region Private Methods
        private void InitiateData()
        {
            _ownerDetails = new List<OwnerDetails>
            {
                new OwnerDetails(){ OwnerName = "Owner1", OwnerAge = 30, OwnerGender = "Male",
                    Pets = new List<Pet>(){
                    new Pet(){ PetName = "Cat1", PetType = "Cat" },
                    new Pet(){ PetName = "Dog1", PetType = "Dog" },
                    new Pet(){ PetName = "Cat2", PetType = "Cat" }
                    }},
                new OwnerDetails(){ OwnerName = "Owner2", OwnerAge = 32, OwnerGender = "Female",
                    Pets = new List<Pet>(){
                    new Pet(){ PetName = "Cat3", PetType = "Cat" },
                    new Pet(){ PetName = "Dog2", PetType = "Dog" },
                    new Pet(){ PetName = "Fish1", PetType = "Fish" }
                    }},
                new OwnerDetails(){ OwnerName = "Owner3", OwnerAge = 33, OwnerGender = "Female",
                    Pets = null},
                new OwnerDetails(){ OwnerName = "Owner4", OwnerAge = 34, OwnerGender = "Male",
                    Pets = new List<Pet>(){
                    new Pet(){ PetName = "Cat4", PetType = "Cat" },
                    new Pet(){ PetName = "Dog3", PetType = "Dog" }
                    }},
                new OwnerDetails(){ OwnerName = "Owner2", OwnerAge = 32, OwnerGender = "Female",
                    Pets = new List<Pet>(){
                    new Pet(){ PetName = "Cat5", PetType = "Cat" },
                    new Pet(){ PetName = "Dog4", PetType = "Dog" },
                    new Pet(){ PetName = "Fish2", PetType = "Fish" }
                    }}

                
            };
        }
        #endregion

        #region Facts

        /// <summary>
        /// Test if the data returned and converted to model is as expected and is raw.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Test_GetAllOwnerWithAllPetsAsync_When_Success()
        {
            var results = await _ownerPetsServiceManager.GetAllOwnerWithAllPetsAsync();
            Assert.NotEmpty(results);
            Assert.Equal(5, (int)results.Count());
        }

        /// <summary>
        /// Test if data returned for a specific pet type is as expected and filtered correctly.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Test_GetAllOwnerWithPetTypeAsync_When_Success()
        {
            var resultsCats = await _ownerPetsServiceManager.GetAllOwnerWithPetTypeAsync(PetType.Cat);
            var resultsDogs = await _ownerPetsServiceManager.GetAllOwnerWithPetTypeAsync(PetType.Dog);
            var resultsFish = await _ownerPetsServiceManager.GetAllOwnerWithPetTypeAsync(PetType.Fish);

            Assert.NotEmpty(resultsCats);
            Assert.Equal(2, resultsCats.Count());
            Assert.Equal(3, resultsCats.First(owner => owner.OwnerGender == "Male").PetNames.Count());
            Assert.Equal(2, resultsCats.First(owner => owner.OwnerGender == "Female").PetNames.Count());

            Assert.NotEmpty(resultsDogs);
            Assert.Equal(2, resultsDogs.Count());
            Assert.Equal(2, resultsDogs.First(owner => owner.OwnerGender == "Male").PetNames.Count());
            Assert.Equal(2, resultsDogs.First(owner => owner.OwnerGender == "Female").PetNames.Count());

            Assert.NotEmpty(resultsFish);
            Assert.Equal(2, resultsFish.Count());
            Assert.Equal(0, (int)(resultsFish.First(owner => owner.OwnerGender == "Male").PetNames.Count()));
            Assert.Equal(2, resultsFish.First(owner => owner.OwnerGender == "Female").PetNames.Count());


        }
        #endregion
    }
}
