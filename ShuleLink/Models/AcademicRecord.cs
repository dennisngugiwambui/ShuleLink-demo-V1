using System.ComponentModel.DataAnnotations;

namespace ShuleLink.Models
{
    public class AcademicRecord
    {
        public int Id { get; set; }
        
        [Required]
        public int StudentId { get; set; }
        
        [Required]
        public int Grade { get; set; } // 1, 2, 3, 4, 5, etc.
        
        [Required]
        public int Year { get; set; } // Academic year
        
        public List<SubjectGrade> SubjectGrades { get; set; } = new();
        
        public double OverallAverage { get; set; }
        
        public int ClassPosition { get; set; }
        
        public int TotalStudentsInClass { get; set; }
        
        public int StreamPosition { get; set; }
        
        public int TotalStudentsInStream { get; set; }
        
        public string Comments { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
    
    public class SubjectGrade
    {
        public int Id { get; set; }
        
        [Required]
        public int AcademicRecordId { get; set; }
        
        [Required]
        public string SubjectName { get; set; } = string.Empty;
        
        public double Term1Mark { get; set; }
        
        public double Term2Mark { get; set; }
        
        public double Term3Mark { get; set; }
        
        public double AverageMark => (Term1Mark + Term2Mark + Term3Mark) / 3;
        
        public string Grade => CalculateGrade(AverageMark);
        
        public string Comments { get; set; } = string.Empty;
        
        // Navigation property
        public AcademicRecord AcademicRecord { get; set; } = null!;
        
        private string CalculateGrade(double average)
        {
            return average switch
            {
                >= 90 => "A+",
                >= 80 => "A",
                >= 70 => "B+",
                >= 60 => "B",
                >= 50 => "C+",
                >= 40 => "C",
                >= 30 => "D+",
                >= 20 => "D",
                _ => "E"
            };
        }
    }
    
    public class TimetableEntry
    {
        public int Id { get; set; }
        
        [Required]
        public string DayOfWeek { get; set; } = string.Empty; // Monday, Tuesday, etc.
        
        [Required]
        public TimeSpan StartTime { get; set; }
        
        [Required]
        public TimeSpan EndTime { get; set; }
        
        [Required]
        public string SubjectName { get; set; } = string.Empty;
        
        public string TeacherName { get; set; } = string.Empty;
        
        public string Classroom { get; set; } = string.Empty;
        
        public string SubjectColor { get; set; } = "#4A90E2"; // Default blue
        
        public int Grade { get; set; }
        
        public string Period { get; set; } = string.Empty; // Period 1, Period 2, etc.
        
        public bool IsBreak { get; set; } = false;
        
        public string TimeSlot => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
    }
    
    public class StudentProgress
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int CurrentGrade { get; set; }
        public List<AcademicRecord> AcademicHistory { get; set; } = new();
        public double CurrentGPA { get; set; }
        public int CurrentClassPosition { get; set; }
        public int CurrentStreamPosition { get; set; }
        public string PerformanceTrend { get; set; } = "Stable"; // Improving, Declining, Stable
        public List<string> Strengths { get; set; } = new();
        public List<string> AreasForImprovement { get; set; } = new();
    }
}
