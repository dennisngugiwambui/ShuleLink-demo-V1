using SQLite;

namespace ShuleLink.Models
{
    [Table("DailyQuotes")]
    public class DailyQuote
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(500)]
        public string Quote { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Author { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        [MaxLength(20)]
        public string BackgroundColor { get; set; } = "#4A90E2";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
