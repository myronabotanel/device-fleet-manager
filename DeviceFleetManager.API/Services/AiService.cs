using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace DeviceFleetManager.API.Services
{
    public class AiService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private const string Model = "gemini-2.5-flash";

        public AiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // pt a lua cheia
            _apiKey = configuration["Gemini:ApiKey"] ?? throw new ArgumentNullException("Gemini API Key is missing");
        }

        public async Task<string> GenerateDescriptionAsync(
            string name, string manufacturer, string type,
            string os, int ram, string processor)
        {
            var prompt = $"""
                Generate a short, professional description (max 2 sentences) for a device:
                Name: {name}, Manufacturer: {manufacturer}, Type: {type},
                OS: {os}, RAM: {ram}GB, Processor: {processor}.
                Respond with ONLY the description, no extra text.
                """;

            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{Model}:generateContent?key={_apiKey}";
            
            //in caz de 429
            var response = await _httpClient.PostAsync(url, content);

            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);

            return doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "No description generated.";
        }
    }
}