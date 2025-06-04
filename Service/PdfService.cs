using System.Text;
using UglyToad.PdfPig;

namespace IAChatMVC.Services
{
    public class PdfService
    {
        public async Task<string> LerArquivoAsync(IFormFile arquivo)
        {
            if (arquivo.FileName.EndsWith(".txt"))
            {
                using var reader = new StreamReader(arquivo.OpenReadStream());
                return await reader.ReadToEndAsync();
            }
            else if (arquivo.FileName.EndsWith(".pdf"))
            {
                var builder = new StringBuilder();
                using var stream = arquivo.OpenReadStream();
                using var pdf = PdfDocument.Open(stream);
                foreach (var page in pdf.GetPages())
                    builder.AppendLine(page.Text);
                return builder.ToString();
            }
            return "";
        }
    }
}