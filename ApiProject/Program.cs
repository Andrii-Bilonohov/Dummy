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
                Console.WriteLine("5. Download Post Image");
                Console.WriteLine("0. Exit");

                var input = await ValidString("Enter the variant: ");
                if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int choice) || choice < 0 || choice > 5)
                {
                    Console.WriteLine("Invalid input. Please enter a number between 0 and 5.");
                    return;
                }

                switch (input)
                {
                    case "0":
                        Console.Clear();
                        Console.WriteLine("Exiting the application...");
                        return;
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
                    case "5":
                        Console.Clear();
                        await DownloadPostImageAsync();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
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
            var postData = DeserializeResponse<PostWrapper>(data);

            if (postData == null)
            {
                Console.WriteLine("No posts found in the response.");
                return;
            }

            Console.WriteLine(postData);

            ContinuePrompt();
        }


        private static async Task AddPostAsync()
        {
            try
            {
                var text = await ValidString("Enter text: ");
                var image = await ValidString("Enter image URL: ");
                var tagsInput = await ValidString("Enter tags separated by commas: ");
                var ownerId = await ValidString("Enter owner ID: ");

                var tags = tagsInput.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();

                var postData = new { text, image, tags, owner = ownerId };

                var response = await _service.PostDataAsync("post/create", postData);
                if (string.IsNullOrEmpty(response))
                {
                    Console.WriteLine("No data received from POST request.");
                    return;
                }

                var postCreate = DeserializeResponse<PostCreate>(response);
                if (postCreate == null) return;

                Console.WriteLine(postCreate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                ContinuePrompt();
            }
        }


        private static async Task UpdatePostAsync()
        {
            try
            {
                var id = await ValidString("Enter the ID of the post to update: ");
                var response = await _service.GetDataAsync($"post/{id}");

                if (string.IsNullOrEmpty(response))
                {
                    Console.WriteLine("No data received from GET request.");
                    return;
                }

                var post = DeserializeResponse<Post>(response);
                if (post == null) return;

                Console.WriteLine("Current post data:");
                Console.WriteLine(post);

                var text = await ValidString("Enter new text: ");
                var image = await ValidString("Enter new image URL: ");
                var tagsInput = await ValidString("Enter new tags separated by commas: ");

                var putData = new { text, image, tags = tagsInput.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList() };
                var putResponse = await _service.PutDataAsync($"post/{id}", putData);

                if (string.IsNullOrEmpty(putResponse))
                {
                    Console.WriteLine("No data received from PUT request.");
                    return;
                }

                var updatedPost = DeserializeResponse<Post>(putResponse);
                if (updatedPost == null) return;

                Console.WriteLine("Updated post data:");
                Console.WriteLine(updatedPost);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                ContinuePrompt();
            }
        }


        private static async Task DeletePostAsync()
        {
            var id = ValidString("Enter the ID of the post to delete: ");

            var response = await _service.DeleteDataAsync($"post/{id}");

            if (string.IsNullOrEmpty(response))
            {
                Console.WriteLine("No data received from DELETE request.");
                return;
            }

            Console.Write("Data received from DELETE request: ");
            var post = DeserializeResponse<Post>(response);

            if (post == null)
            {
                Console.WriteLine("Failed to deserialize the response from DELETE request.");
                return;
            }

            Console.WriteLine(post);

            Console.WriteLine("Post deleted successfully.");
            ContinuePrompt();
        }


        private static async Task DownloadPostImageAsync()
        {
            try
            {
                var postId = await ValidString("Enter the ID of the post to download the image: ");



                var postData = await _service.GetDataAsync($"post/{postId}");
                if (string.IsNullOrEmpty(postData))
                {
                    Console.WriteLine("No data received from GET request.");
                    return;
                }

                var post = DeserializeResponse<Post>(postData);
                if (post == null || post.Image == null)
                {
                    Console.WriteLine("Error");
                    return;
                }

                var ImageUrl = post.Image;

                if (string.IsNullOrWhiteSpace(ImageUrl))
                {
                    Console.WriteLine("No image URL found for the specified post.");
                    return;
                }

                var savePath = Path.Combine("C:\\WebApi\\ApiProject\\ApiProject\\Images", $"{postId}.jpg");
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                await _service.DownloadPostImageAsync(ImageUrl, savePath);

                Console.WriteLine($"Image downloaded successfully to {savePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
               ContinuePrompt();
            }
        }


        private static async Task<string> ValidString(string message)
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

                await Task.Delay(500);
            }
        }


        private static T DeserializeResponse<T>(string response) where T : class
        {
            try
            {
                return JsonSerializer.Deserialize<T>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                Console.WriteLine("Failed to deserialize response.");
                return null;
            }
        }


        private static void ContinuePrompt()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
