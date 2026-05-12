using System;

namespace ChatBot.UI.Models
{
    public class ChatMessage
    {
        public string Sender { get; set; } 
        public string Text { get; set; }
        public string Time { get; set; } = DateTime.Now.ToString("HH:mm");
        public bool IsUser { get; set; } 
    }
}