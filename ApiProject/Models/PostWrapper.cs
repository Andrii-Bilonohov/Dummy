using System.Text.Json.Serialization;

namespace ApiProject.Models
{
    public class PostWrapper
    {
        [JsonPropertyName("data")]
        public List<Post> Posts { get; set; }

        public override string ToString()
        {
            return string.Join("\n", Posts.Select(post => post.ToString()));
        }
    }
}
