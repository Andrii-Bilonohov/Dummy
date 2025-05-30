using System.Text.Json.Serialization;

namespace ApiProject.Models
{
    public class Owner
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
    }
}