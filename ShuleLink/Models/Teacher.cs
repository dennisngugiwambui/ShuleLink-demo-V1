using System.ComponentModel.DataAnnotations;

namespace ShuleLink.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Subject { get; set; } = string.Empty;
        
        public string Grade { get; set; } = string.Empty; // e.g., "1-3", "4-7", "All"
        
        [Required]
        public TeacherRole Role { get; set; }
        
        public bool IsClassTeacher { get; set; }
        
        public string? ClassAssigned { get; set; } // e.g., "Grade 5A"
        
        public string ProfileImage { get; set; } = "dotnet_bot.png";
        
        public string Bio { get; set; } = string.Empty;
        
        public bool IsOnline { get; set; }
        
        public DateTime LastSeen { get; set; } = DateTime.Now;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
    
    public enum TeacherRole
    {
        HeadTeacher,
        DeputyHeadTeacher,
        Secretary,
        ClassTeacher,
        SubjectTeacher
    }
}
