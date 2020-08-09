using System.Collections.Generic;
using System.Threading.Tasks;
using AGL.CodeExercise.Common.Models;

namespace AGL.CodeExercise.Services.Pets
{
    public interface IOwnerPetsService
    {
        Task<List<OwnerDetails>> GetOwnersAndPetsAsync();
    }
}
