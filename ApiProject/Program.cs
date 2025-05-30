using ApiProject.Models;
using System.Text.Json;

namespace ApiProject
{
    internal class Program
    {
        private static readonly Service _service = new Service(new HttpClient());

        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1. Get");
                Console.WriteLine("2. Post");
                Console.WriteLine("3. Put");
                Console.WriteLine("4. Delete");

                var input = ValidString("Enter the variant: ");
                if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int choice) || choice < 1 || choice > 4)
                {
                    Console.WriteLine("Invalid input. Please enter a number between 0 and 4.");
                    return;
                }

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        await PrintPostAsync();
                        break;
                    case "2":
                        Console.Clear();
                        await AddPostAsync();
                        break;
                    case "3":
                        Console.Clear();
                        await UpdatePostAsync();
                        break;
                    case "4":
                        Console.Clear();
                        await DeletePostAsync();
                        break;
                }
            }
        }


        private static async Task PrintPostAsync()
        {
            var data = await _service.GetDataAsync("post");

            if (string.IsNullOrEmpty(data))
            {
                Console.WriteLine("No data received from GET request.");
                return;
            }

            Console.Write("Data received from GET request: ");
            var deserializedData = JsonSerializer.Deserialize<PostWrapper>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (deserializedData == null)
            {
                Console.WriteLine("No posts found in the response.");
                return;
            }

            Console.WriteLine(deserializedData);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }


        private static async Task AddPostAsync()
        {
            var text = ValidString("Enter text: ");
            var image = ValidString("Enter image URL: ");
            var tags = new List<string>();
            while (true)
            {
                var tag = ValidString("Enter new tag (or 0 for to finish): ");
                if (tag == "0") break;
                tags.Add(tag);
            }
            var ownerId = ValidString("Enter owner ID: ");

            var postData = new
            {
                text = text,
                image = image,
                tags = tags,
                owner = ownerId
            };

            var postResponse = await _service.PostDataAsync("post/create", postData);

            if (string.IsNullOrEmpty(postResponse))
            {
                Console.WriteLine("No data received from POST request.");
                return;
            }

            Console.Write("Data received from POST request: ");

            var postResponseDeserialized = JsonSerializer.Deserialize<PostCreate>(postResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (postResponseDeserialized == null)
            {
                Console.WriteLine("Failed to deserialize the response from POST request.");
                return;
            }

            Console.WriteLine(postResponseDeserialized);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }


        private static async Task UpdatePostAsync()
        {
            var text = ValidString("Enter new text: ");
            var image = ValidString("Enter new image URL: ");
            var tags = new List<string>();
            while (true)
            {
                var tag = ValidString("Enter new tag (or 0 for to finish): ");
                if (tag == "0") break;
                tags.Add(tag);
            }

            var putData = new
            {
                text = text,
                image = image,
                tags = tags
            };

            var putResponse = await _service.PutDataAsync("post/6839e2888932b72aae2f5079", putData);

            if (string.IsNullOrEmpty(putResponse))
            {
                Console.WriteLine("No data received from PUT request.");
                return;
            }

            Console.Write("Data received from PUT request: ");
            var putResponseDeserialized = JsonSerializer.Deserialize<Post>(putResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (putResponseDeserialized == null)
            {
                Console.WriteLine("Failed to deserialize the response from PUT request.");
                return;
            }

            Console.WriteLine(putResponseDeserialized);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }


        private static async Task DeletePostAsync()
        {
            var id = ValidString("Enter the ID of the post to delete: ");

            var deleteResponse = await _service.DeleteDataAsync($"post/{id}");

            if (string.IsNullOrEmpty(deleteResponse))
            {
                Console.WriteLine("No data received from DELETE request.");
                return;
            }

            Console.Write("Data received from DELETE request: ");
            var deleteResponseDeserialized = JsonSerializer.Deserialize<Post>(deleteResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (deleteResponseDeserialized == null)
            {
                Console.WriteLine("Failed to deserialize the response from DELETE request.");
                return;
            }

            Console.WriteLine(deleteResponseDeserialized);

            Console.WriteLine("Post deleted successfully.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }


        private static string ValidString(string message)
        {
            while (true)
            {
                Console.Write(message);

                string? result = Console.ReadLine();
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }

                Console.WriteLine("Invalid input.");
            }
        }
    }
}
