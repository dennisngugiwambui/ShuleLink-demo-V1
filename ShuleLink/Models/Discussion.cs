using System.ComponentModel.DataAnnotations;

namespace ShuleLink.Models
{
    public class Discussion
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public string? ImageUrl { get; set; } // For image posts
        
        [Required]
        public int AuthorId { get; set; }
        
        public string AuthorName { get; set; } = string.Empty;
        
        public string Subject { get; set; } = string.Empty;
        
        public string Grade { get; set; } = string.Empty;
        
        public string Topic { get; set; } = string.Empty;
        
        public int Upvotes { get; set; } = 0;
        
        public int Downvotes { get; set; } = 0;
        
        public int ReplyCount { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        public bool IsResolved { get; set; } = false;
        
        public bool IsPinned { get; set; } = false;
        
        // Navigation properties
        public List<DiscussionReply> Replies { get; set; } = new();
        public List<DiscussionVote> Votes { get; set; } = new();
    }
    
    public class DiscussionReply
    {
        public int Id { get; set; }
        
        [Required]
        public int DiscussionId { get; set; }
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public string? ImageUrl { get; set; } // For image replies
        
        [Required]
        public int AuthorId { get; set; }
        
        public string AuthorName { get; set; } = string.Empty;
        
        public int Upvotes { get; set; } = 0;
        
        public int Downvotes { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public bool IsAcceptedAnswer { get; set; } = false;
        
        // Navigation properties
        public Discussion Discussion { get; set; } = null!;
        public List<DiscussionVote> Votes { get; set; } = new();
    }
    
    public class DiscussionVote
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        public int? DiscussionId { get; set; } // For discussion votes
        
        public int? ReplyId { get; set; } // For reply votes
        
        [Required]
        public VoteType VoteType { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public Discussion? Discussion { get; set; }
        public DiscussionReply? Reply { get; set; }
    }
    
    public enum VoteType
    {
        Upvote = 1,
        Downvote = -1
    }
}
