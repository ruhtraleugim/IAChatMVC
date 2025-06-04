using IAChatMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace IAChatMVC.Controllers
{
    public class IAController : Controller
    {
        private readonly OllamaService _ollamaService;
        private readonly PdfService _pdfService;

        public IAController()
        {
            _ollamaService = new OllamaService();
            _pdfService = new PdfService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string modo, string input, IFormFile arquivo)
        {
            string prompt;
            string resposta;
            string padrão = "Você é um assistente simpático e deve responder sempre em Em Pt-br. " ;

            Console.WriteLine(input);
            if (modo == "Resumo" && arquivo != null && arquivo.Length > 0)
            {
                string texto = await _pdfService.LerArquivoAsync(arquivo);
                prompt = padrão + "Resuma o texto abaixo em até 100 palavras:\n\n{texto} ";
            }
            else if (modo == "Explicar")
            {
                prompt = $"Explique o seguinte termo em até 100 palavras: {input}" + padrão;
            }
            else if (modo == "Sentimento")
            {
                prompt = $"Classifique o sentimento da seguinte frase como 'positivo', 'negativo' ou 'neutro'. Frase: {input}. Responda apenas com a classificação." + padrão;
            }
            else
            {
                prompt = padrão + input;
            }

            resposta = await _ollamaService.EnviarPromptAsync(prompt);
            ViewBag.Resposta = resposta;
            Console.WriteLine(resposta);

            return View();
        }
    }
}
