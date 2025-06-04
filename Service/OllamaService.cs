using System.Text;
using System.Text.Json;


namespace Service
{
    public class OllamaService
    {
        private readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(5)
        };

        private readonly string apiUrl = "http://localhost:11434/api/generate";

        public async Task<string> EnviarPromptAsync(string prompt)
        {
            var requestBody = new
            {
                model = "llama3",
                prompt = prompt,
                stream = false
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiUrl, content);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty("response", out var resposta))
                return resposta.GetString();
            else
                return "Erro: resposta não encontrada.";
        }
    }

}
