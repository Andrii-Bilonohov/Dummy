using System.Text.Json.Serialization;

namespace ApiProject.Models
{
    public class Post
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("image")]
        public string Image { get; set; }
        
        [JsonPropertyName("likes")]
        public int Like { get; set; }
        
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }
        
        [JsonPropertyName("text")]
        public string Text { get; set; }
        
        [JsonPropertyName("publishDate")]
        public DateTime PublishDate { get; set; }
        
        [JsonPropertyName("updateDate")]
        public DateTime UpdateDate { get; set; }
        
        [JsonPropertyName("owner")]
        public Owner Owner { get; set; }
        public override string ToString()
        {
            return $"\nPost ID: {Id}\nText: {Text}\nImage: {Image}\nTags: {string.Join(", ", Tags)}\nOwner: {Owner.Id}";
        }
    }
}
