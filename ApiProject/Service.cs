using System.Text;
using System.Text.Json;

namespace ApiProject
{
    public class Service
    {
        private readonly HttpClient _httpClient;

        public Service(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("https://dummyapi.io/data/v1/");
            _httpClient.DefaultRequestHeaders.Add("app-id", "6112dc7c3f812e0d9b6679dd");
        }

        public async Task<string> GetDataAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching data from {endpoint}: {response.ReasonPhrase}");
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostDataAsync(string endpoint, object data)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error posting data to {endpoint}: {response.ReasonPhrase}");
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PutDataAsync(string endpoint, object data)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
               Console.WriteLine($"Error updating data at {endpoint}: {response.ReasonPhrase}");
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string?> DeleteDataAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error deleting data at {endpoint}: {response.ReasonPhrase}");
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task DownloadPostImageAsync(string imageUrl, string savePath)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("Image URL cannot be null or empty.", nameof(imageUrl));

            if (string.IsNullOrWhiteSpace(savePath))
                throw new ArgumentException("Save path cannot be null or empty.", nameof(savePath));

            try
            {
                using var response = await _httpClient.GetAsync(imageUrl);
                
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error downloading image: {response.ReasonPhrase}");
                    return;
                }

                var imageBytes = await response.Content.ReadAsByteArrayAsync();

                await File.WriteAllBytesAsync(savePath, imageBytes);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading image: {ex.Message}");
            }
        }

    }
}
