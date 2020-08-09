using System.Collections.Generic;
using System.Threading.Tasks;
using AGL.CodeExercise.Common;
using AGL.CodeExercise.Common.Models;
using AGL.CodeExercise.Common.ViewModels;

namespace AGL.CodeExercise.ServiceManagers
{
    public interface IOwnerPetsServiceManager
    {
        Task<List<OwnerDetails>> GetAllOwnerWithAllPetsAsync();
        Task<List<OwnerPetViewModel>> GetAllOwnerWithPetTypeAsync(PetType petType);

    }
}
