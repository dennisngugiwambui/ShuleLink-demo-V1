using System.ComponentModel.DataAnnotations;

namespace ShuleLink.Models
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        
        [Required]
        public string Question { get; set; } = string.Empty;
        
        [Required]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        public string Grade { get; set; } = string.Empty;
        
        public string Topic { get; set; } = string.Empty;
        
        public QuestionType Type { get; set; } = QuestionType.MultipleChoice;
        
        // For multiple choice questions
        public string OptionA { get; set; } = string.Empty;
        public string OptionB { get; set; } = string.Empty;
        public string OptionC { get; set; } = string.Empty;
        public string OptionD { get; set; } = string.Empty;
        
        [Required]
        public string CorrectAnswer { get; set; } = string.Empty;
        
        public string Explanation { get; set; } = string.Empty;
        
        // For calculation questions - step-by-step solution
        public string CalculationSteps { get; set; } = string.Empty;
        
        // For fill-in-the-blank questions
        public string BlankAnswer { get; set; } = string.Empty;
        
        public string? DiagramUrl { get; set; } // For science diagrams
        
        public int DifficultyLevel { get; set; } = 1; // 1-5
        
        // Additional properties for enhanced questions
        public string Formula { get; set; } = string.Empty; // For math/science formulas
        public string Units { get; set; } = string.Empty; // For measurement units
        public bool ShowWorkRequired { get; set; } = false; // Whether to show calculation steps
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
    
    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse,
        ShortAnswer,
        Essay,
        FillInTheBlank,
        Calculation,
        Matching
    }
}
