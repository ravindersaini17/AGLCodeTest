using Newtonsoft.Json;
namespace AGL.CodeExercise.Common.Models
{
    public class Pet
    {
        [JsonProperty("name")]
        public string PetName { get; set; }

        [JsonProperty("type")]
        public string PetType { get; set; }
    }
}
