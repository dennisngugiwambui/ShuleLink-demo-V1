using SQLite;

namespace ShuleLink.Models
{
    [Table("ChatConversations")]
    public class ChatConversation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int StudentId { get; set; }
        
        // Alias for compatibility
        [Ignore]
        public int UserId 
        { 
            get => StudentId; 
            set => StudentId = value; 
        }

        public int TeacherId { get; set; }

        public DateTime LastMessageAt { get; set; }

        [MaxLength(500)]
        public string LastMessage { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
