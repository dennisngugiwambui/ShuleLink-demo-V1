using SQLite;

namespace ShuleLink.Models
{
    [Table("DiscussionPosts")]
    public class DiscussionPost
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Title { get; set; } = "";

        [NotNull]
        public string Content { get; set; } = "";

        [NotNull]
        public int AuthorId { get; set; }

        [NotNull]
        public string AuthorName { get; set; } = "";

        [NotNull]
        public string AuthorType { get; set; } = "Student"; // Student, Teacher

        [NotNull]
        public string Category { get; set; } = "General"; // General, Math, Science, English, etc.

        [NotNull]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public int Likes { get; set; } = 0;

        public int Replies { get; set; } = 0;

        public bool IsSticky { get; set; } = false;

        public bool IsLocked { get; set; } = false;

        [NotNull]
        public string Tags { get; set; } = ""; // Comma-separated tags
    }

    // DiscussionReply moved to Discussion.cs to avoid conflicts

    [Table("DiscussionLikes")]
    public class DiscussionLike
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int UserId { get; set; }

        [NotNull]
        public string ItemType { get; set; } = ""; // Post, Reply

        [NotNull]
        public int ItemId { get; set; }

        [NotNull]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
