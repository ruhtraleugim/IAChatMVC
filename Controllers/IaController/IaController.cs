using Microsoft.AspNetCore.Mvc;
using IAChatMVC.Models.Ia;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IAChatMVC.Controllers
{
    public class IAController : Controller
    {
        private readonly HttpClient _httpClient;

        public IAController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new IaRequestModel();
            return View(model);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Index(IaRequestModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Input))
            {
                ModelState.AddModelError("", "Digite uma pergunta.");
                return View(model);
            }

            model.Messages.Add(new ChatMessage
            {
                Role = "Usuário",
                Message = model.Input
            });

            var prompt = "Você é um assistente simpático. Pergunta: " + model.Input;

            var requestBody = new
            {
                model = "llama3",
                prompt = prompt,
                stream = false
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:11434/api/generate", content);
            var resultJson = await response.Content.ReadAsStringAsync();

            Console.WriteLine(resultJson);

            var jsonDoc = JsonDocument.Parse(resultJson);
            string resposta = "Erro: resposta não encontrada.";

            if (jsonDoc.RootElement.TryGetProperty("response", out var responseElement))
            {
                resposta = responseElement.GetString();
            }

            ViewBag.Resposta = resposta; // <-- ESSA LINHA ADICIONADA!

            model.Input = "";

            return View(model);
        }

    }
}
