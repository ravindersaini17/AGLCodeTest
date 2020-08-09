using System.Collections.Generic;
using Newtonsoft.Json;

namespace AGL.CodeExercise.Common.Models
{
    public class OwnerDetails
    {
        [JsonProperty("name")]
        public string OwnerName { get; set; }

        [JsonProperty("gender")]
        public string OwnerGender { get; set; }

        [JsonProperty("age")]
        public int OwnerAge { get; set; }

        [JsonProperty("pets")]
        public List<Pet> Pets { get; set; }
    }
}
