using System;

namespace ShuleLink.ViewModels
{
    public class ChatMessageViewModel
    {
        public string Message { get; set; } = string.Empty;
        
        public bool IsFromCurrentUser { get; set; } = false;
        
        public string SenderName { get; set; } = string.Empty;
        
        public DateTime Timestamp { get; set; } = DateTime.Now;
        
        public string BackgroundColor { get; set; } = "#FFFFFF";
        
        public bool IsSentByUser { get; set; } = false;
        
        public DateTime SentAt { get; set; } = DateTime.Now;
        
        // Additional properties for WhatsApp-style UI
        public bool IsReceivedMessage => !IsSentByUser;
        
        public bool ShowSenderName => !IsSentByUser && !string.IsNullOrEmpty(SenderName);
        
        // Constructor
        public ChatMessageViewModel()
        {
        }
        
        // Constructor with parameters
        public ChatMessageViewModel(string message, bool isFromCurrentUser, string senderName)
        {
            Message = message;
            IsFromCurrentUser = isFromCurrentUser;
            SenderName = senderName;
            Timestamp = DateTime.Now;
            SentAt = DateTime.Now;
            BackgroundColor = isFromCurrentUser ? "#E3F2FD" : "#F8F9FA";
        }
    }
}
