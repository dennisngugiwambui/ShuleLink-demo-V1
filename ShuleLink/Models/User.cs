using SQLite;

namespace ShuleLink.Models
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20), Unique]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(20)]
        public string AdmissionNumber { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Grade { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Class { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ParentName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string HomeAddress { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Status { get; set; } = "active";

        [MaxLength(20)]
        public string UserType { get; set; } = "student";

        public bool Graduated { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
