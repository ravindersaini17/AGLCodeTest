using System.Collections.Generic;

namespace AGL.CodeExercise.Common.ViewModels
{
    public class OwnerPetViewModel
    {
        public string OwnerGender { get; set; }
        public IEnumerable<string> PetNames { get; set; }
    }
}
