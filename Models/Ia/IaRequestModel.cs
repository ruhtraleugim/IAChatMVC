using System.Collections.Generic;

namespace IAChatMVC.Models.Ia
{
    public class IaRequestModel
    {
        public string Input { get; set; }

        // Lista para manter histórico de mensagens
        public List<ChatMessage> Messages { get; set; } = new();
    }

    public class ChatMessage
    {
        public string Role { get; set; } // "Usuário" ou "IA"
        public string Message { get; set; }
    }
}
