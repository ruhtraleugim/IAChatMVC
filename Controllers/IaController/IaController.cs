using IAChatMVC.Models.Ia;
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
public async Task<IActionResult> Index(IaRequestModel model)
{
    if (string.IsNullOrEmpty(model.TipoAcao))
    {
        ModelState.AddModelError("", "Selecione uma ação.");
        return View(model);
    }

    string prompt = "";

    switch (model.TipoAcao)
    {
        case "explicacao":
            if (string.IsNullOrWhiteSpace(model.Input))
            {
                ModelState.AddModelError("", "Digite uma pergunta.");
                return View(model);
            }

            prompt = $"""
Explique o seguinte conteúdo ou pergunta de forma clara e objetiva, em até 100 palavras:

{model.Input}
""";
            break;

        case "resumo":
            if (model.File == null || model.File.Length == 0)
            {
                ModelState.AddModelError("", "Envie um arquivo para resumir.");
                return View(model);
            }

            using (var reader = new StreamReader(model.File.OpenReadStream()))
            {
                var textoArquivo = await reader.ReadToEndAsync();
                prompt = $"""
Resuma o seguinte conteúdo de forma clara e em português do Brasil:

{textoArquivo}
""";
            }
            break;

        case "emocao":
            if (string.IsNullOrWhiteSpace(model.Input))
            {
                ModelState.AddModelError("", "Digite o texto para análise de sentimento.");
                return View(model);
            }

            prompt = $"""
Analise a emoção ou sentimento no seguinte texto. Retorne se é positivo, negativo ou neutro, e justifique:

{model.Input}
""";
            break;
    }

    model.Response = await _ollamaService.EnviarPromptAsync(prompt);
    return View(model);
}

    }
}
