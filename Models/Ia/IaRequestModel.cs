using System.Collections.Generic;

namespace IAChatMVC.Models.Ia
{
    public class IaRequestModel
    {
        public string Input { get; set; }

        public List<ChatMessage> Messages { get; set; } = new();
    }

    public class ChatMessage
    {
        public string Role { get; set; } 
        public string Message { get; set; }
    }
}
