using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AGL.CodeExercise.Common;
using AGL.CodeExercise.ServiceManagers;
using Microsoft.Extensions.Logging;

namespace AGL.CodeExercise.App
{
    public class PetsController: IPetsController
    {
        #region Private Variables
        private readonly ILogger<PetsController> _logger;
        private readonly TextReader _input;
        private readonly TextWriter _output;
        private readonly IOwnerPetsServiceManager _ownerPetsServiceManager;

        #endregion

        #region Constructor
        public PetsController(IOwnerPetsServiceManager ownerPetsServiceManager, ILogger<PetsController> logger, TextReader input, TextWriter output)
        {
            _ownerPetsServiceManager = ownerPetsServiceManager;
            _logger = logger;
            _input = input;
            _output = output;
        }
        #endregion


        #region Interface Implementation
        /// <summary>
        /// Get list of cats in alphabetical order and grouped with owner's gender
        /// </summary>
        /// <returns></returns>
        public async Task GetCatsPerOwnerGender()
        {
            try
            {
                // Get list of all cats grouped by owner gender
                var catsPerOwner = await _ownerPetsServiceManager.GetAllOwnerWithPetTypeAsync(PetType.Cat);

                //Print the data in terminal window
                catsPerOwner.ForEach(ownerGen =>
                {
                    _output.WriteLine(ownerGen.OwnerGender);
                    ownerGen.PetNames.ToList().ForEach(catName => _output.WriteLine($" -> {catName}"));
                    _output.WriteLine(_output.NewLine);
                });
            }
            catch (HttpResponseException ex)
            {
                _logger.LogError(ex.Message + ex.ResponseErrorCode, ex.InnerException?.InnerException);
                _output.WriteLine(ex.Message + ex.ResponseErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Error.APICallFailed);
                _output.WriteLine(Error.APICallFailed);
            }

            _input.ReadLine();
        }
        #endregion
    }
}
