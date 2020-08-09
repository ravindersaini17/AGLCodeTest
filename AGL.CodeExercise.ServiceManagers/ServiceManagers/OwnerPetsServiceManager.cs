using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AGL.CodeExercise.Common;
using AGL.CodeExercise.Common.Models;
using AGL.CodeExercise.Common.ViewModels;
using AGL.CodeExercise.Services.Pets;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace AGL.CodeExercise.ServiceManagers.ServiceManagers
{
    public class OwnerPetsServiceManager : IOwnerPetsServiceManager
    {
        #region Private fields
        private readonly IOwnerPetsService _ownerPetsService;
        private readonly ILogger<OwnerPetsServiceManager> _logger;
        #endregion

        #region Constructor
        public OwnerPetsServiceManager(IOwnerPetsService ownerPetsService, ILogger<OwnerPetsServiceManager> logger)
        {
            _logger = logger;
            _ownerPetsService = ownerPetsService;
        }
        #endregion

        #region Interface Implementation

        /// <summary>
        /// Get raw data for all owners and pets
        /// </summary>
        /// <returns>List of OwnerDetails (Complete Model)</returns>
        public async Task<List<OwnerDetails>> GetAllOwnerWithAllPetsAsync()
        {
            return await _ownerPetsService.GetOwnersAndPetsAsync();
        }


        /// <summary>
        /// Get Owner and pets data grouped by owner gender.
        /// </summary>
        /// <param name="petType">Filter pets based on pet type</param>
        /// <returns>List of OwnerPetViewModel</returns>
        public async Task<List<OwnerPetViewModel>> GetAllOwnerWithPetTypeAsync(PetType petType)
        {
            //Get raw data with all owners and pets
            var ownerPetDetails = await GetAllOwnerWithAllPetsAsync();

            try
            {
                //Group data by Owner gender and filter using Pet Type.
                var petsTypeGroupedByOwnerGender = ownerPetDetails
                    .Where(p => p.Pets != null && p.Pets.Any())
                    .GroupBy(g => g.OwnerGender) //Group data with Gender
                    .Select(
                        group => new OwnerPetViewModel
                        {
                            OwnerGender = group.Key,
                            PetNames = group
                                .SelectMany(p => p.Pets)
                                .Where(p => p.PetType.Equals(petType.ToString(),
                                StringComparison.OrdinalIgnoreCase)) // Filter data with pet type
                                .Select(pet => pet.PetName)
                                .OrderBy(name => name)
                        });

                return petsTypeGroupedByOwnerGender.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.InnerException?.InnerException);
                throw;
            }
        }

        #endregion
    }
}
