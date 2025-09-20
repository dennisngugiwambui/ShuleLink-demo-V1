using SQLite;

namespace ShuleLink.Models
{
    [Table("QuizAttempts")]
    public class QuizAttempt
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }

        [MaxLength(50)]
        public string Subject { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Grade { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Topic { get; set; } = string.Empty;

        public int Score { get; set; }

        public int TotalQuestions { get; set; }

        public double Percentage { get; set; }

        public DateTime AttemptedAt { get; set; } = DateTime.Now;

        public TimeSpan TimeTaken { get; set; }
    }
}
