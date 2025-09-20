using SQLite;

namespace ShuleLink.Models
{
    [Table("ChatMessages")]
    public class ChatMessage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public int SenderId { get; set; } // User ID
        
        public int ReceiverId { get; set; } // Teacher ID
        
        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;
        
        public DateTime SentAt { get; set; } = DateTime.Now;
        
        public bool IsRead { get; set; }
        
        public MessageType Type { get; set; } = MessageType.Text;
        
        [MaxLength(500)]
        public string? AttachmentUrl { get; set; }
    }
    
    public enum MessageType
    {
        Text,
        Image,
        File,
        Voice
    }
}
