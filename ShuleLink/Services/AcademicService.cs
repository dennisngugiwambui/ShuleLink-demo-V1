using ShuleLink.Models;

namespace ShuleLink.Services
{
    public class AcademicService
    {
        private readonly List<AcademicRecord> _academicRecords = new();
        private readonly List<TimetableEntry> _timetableEntries = new();
        private readonly Random _random = new();

        public AcademicService()
        {
            InitializeDummyData();
        }

        private void InitializeDummyData()
        {
            GenerateDummyAcademicRecords();
            GenerateDummyTimetable();
        }

        private void GenerateDummyAcademicRecords()
        {
            var subjects = new[]
            {
                "Mathematics", "English", "Science", "Social Studies", "Kiswahili",
                "Art & Craft", "Physical Education", "Music", "Computer Studies", "Religious Education"
            };

            var currentYear = DateTime.Now.Year;
            var studentId = Preferences.Get("UserId", 1);

            // Generate records for grades 1-5
            for (int grade = 1; grade <= 5; grade++)
            {
                var academicRecord = new AcademicRecord
                {
                    Id = grade,
                    StudentId = studentId,
                    Grade = grade,
                    Year = currentYear - (5 - grade), // Past years for lower grades
                    ClassPosition = _random.Next(1, 31), // Position out of 30 students
                    TotalStudentsInClass = 30,
                    StreamPosition = _random.Next(1, 121), // Position out of 120 students (4 streams)
                    TotalStudentsInStream = 120
                };

                var subjectGrades = new List<SubjectGrade>();
                var totalMarks = 0.0;

                foreach (var subject in subjects)
                {
                    var basePerformance = GetBasePerformanceForGrade(grade);
                    var subjectGrade = new SubjectGrade
                    {
                        Id = subjectGrades.Count + 1,
                        AcademicRecordId = academicRecord.Id,
                        SubjectName = subject,
                        Term1Mark = GenerateRealisticMark(basePerformance),
                        Term2Mark = GenerateRealisticMark(basePerformance),
                        Term3Mark = GenerateRealisticMark(basePerformance),
                        Comments = GenerateSubjectComment(subject)
                    };

                    subjectGrades.Add(subjectGrade);
                    totalMarks += subjectGrade.AverageMark;
                }

                academicRecord.SubjectGrades = subjectGrades;
                academicRecord.OverallAverage = totalMarks / subjects.Length;
                academicRecord.Comments = GenerateOverallComment(academicRecord.OverallAverage, grade);

                _academicRecords.Add(academicRecord);
            }
        }

        private double GetBasePerformanceForGrade(int grade)
        {
            // Simulate improving performance as student progresses
            return grade switch
            {
                1 => 65, // Grade 1: Average performance
                2 => 68, // Grade 2: Slight improvement
                3 => 72, // Grade 3: Good progress
                4 => 75, // Grade 4: Strong performance
                5 => 78, // Grade 5: Excellent performance
                _ => 70
            };
        }

        private double GenerateRealisticMark(double basePerformance)
        {
            // Add some variation around the base performance
            var variation = _random.NextDouble() * 20 - 10; // ±10 points variation
            var mark = basePerformance + variation;
            return Math.Max(0, Math.Min(100, mark)); // Ensure mark is between 0-100
        }

        private string GenerateSubjectComment(string subject)
        {
            var comments = subject.ToLower() switch
            {
                "mathematics" => new[] { "Shows strong problem-solving skills", "Needs more practice with calculations", "Excellent understanding of concepts", "Good progress in numerical work" },
                "english" => new[] { "Excellent reading comprehension", "Good creative writing skills", "Needs improvement in grammar", "Strong vocabulary development" },
                "science" => new[] { "Shows great curiosity in experiments", "Good understanding of scientific concepts", "Excellent observation skills", "Needs more practice in scientific method" },
                "social studies" => new[] { "Good knowledge of historical events", "Shows interest in geography", "Excellent map reading skills", "Good understanding of civic duties" },
                "kiswahili" => new[] { "Fluent in conversation", "Good writing skills", "Excellent pronunciation", "Needs more vocabulary practice" },
                _ => new[] { "Good performance", "Shows improvement", "Excellent effort", "Consistent work" }
            };

            return comments[_random.Next(comments.Length)];
        }

        private string GenerateOverallComment(double average, int grade)
        {
            var performance = average switch
            {
                >= 85 => "Excellent",
                >= 75 => "Very Good",
                >= 65 => "Good",
                >= 55 => "Satisfactory",
                _ => "Needs Improvement"
            };

            var comments = new[]
            {
                $"{performance} performance in Grade {grade}. Keep up the good work!",
                $"Shows {performance.ToLower()} progress. Continue working hard.",
                $"{performance} academic achievement. Well done!",
                $"Demonstrates {performance.ToLower()} understanding across subjects."
            };

            return comments[_random.Next(comments.Length)];
        }

        private void GenerateDummyTimetable()
        {
            var subjects = new[]
            {
                new { Name = "Mathematics", Teacher = "Mr. Johnson", Color = "#E74C3C" },
                new { Name = "English", Teacher = "Ms. Smith", Color = "#3498DB" },
                new { Name = "Science", Teacher = "Dr. Brown", Color = "#27AE60" },
                new { Name = "Social Studies", Teacher = "Mrs. Davis", Color = "#F39C12" },
                new { Name = "Kiswahili", Teacher = "Mwalimu Mwangi", Color = "#9B59B6" },
                new { Name = "Art & Craft", Teacher = "Ms. Wilson", Color = "#E67E22" },
                new { Name = "Physical Education", Teacher = "Coach Miller", Color = "#1ABC9C" },
                new { Name = "Music", Teacher = "Mr. Anderson", Color = "#34495E" },
                new { Name = "Computer Studies", Teacher = "Ms. Taylor", Color = "#8E44AD" }
            };

            var days = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            var periods = new[]
            {
                new { Period = "Period 1", Start = new TimeSpan(8, 0, 0), End = new TimeSpan(8, 40, 0) },
                new { Period = "Period 2", Start = new TimeSpan(8, 40, 0), End = new TimeSpan(9, 20, 0) },
                new { Period = "Break", Start = new TimeSpan(9, 20, 0), End = new TimeSpan(9, 40, 0) },
                new { Period = "Period 3", Start = new TimeSpan(9, 40, 0), End = new TimeSpan(10, 20, 0) },
                new { Period = "Period 4", Start = new TimeSpan(10, 20, 0), End = new TimeSpan(11, 0, 0) },
                new { Period = "Period 5", Start = new TimeSpan(11, 0, 0), End = new TimeSpan(11, 40, 0) },
                new { Period = "Lunch", Start = new TimeSpan(11, 40, 0), End = new TimeSpan(12, 40, 0) },
                new { Period = "Period 6", Start = new TimeSpan(12, 40, 0), End = new TimeSpan(1, 20, 0) },
                new { Period = "Period 7", Start = new TimeSpan(1, 20, 0), End = new TimeSpan(2, 0, 0) },
                new { Period = "Period 8", Start = new TimeSpan(2, 0, 0), End = new TimeSpan(2, 40, 0) }
            };

            int entryId = 1;

            foreach (var day in days)
            {
                foreach (var period in periods)
                {
                    var entry = new TimetableEntry
                    {
                        Id = entryId++,
                        DayOfWeek = day,
                        StartTime = period.Start,
                        EndTime = period.End,
                        Period = period.Period,
                        Grade = 5 // Current grade
                    };

                    if (period.Period == "Break")
                    {
                        entry.SubjectName = "Break Time";
                        entry.TeacherName = "";
                        entry.Classroom = "Playground";
                        entry.SubjectColor = "#95A5A6";
                        entry.IsBreak = true;
                    }
                    else if (period.Period == "Lunch")
                    {
                        entry.SubjectName = "Lunch Break";
                        entry.TeacherName = "";
                        entry.Classroom = "Dining Hall";
                        entry.SubjectColor = "#95A5A6";
                        entry.IsBreak = true;
                    }
                    else
                    {
                        var subject = subjects[_random.Next(subjects.Length)];
                        entry.SubjectName = subject.Name;
                        entry.TeacherName = subject.Teacher;
                        entry.Classroom = $"Room {_random.Next(101, 120)}";
                        entry.SubjectColor = subject.Color;
                        entry.IsBreak = false;
                    }

                    _timetableEntries.Add(entry);
                }
            }
        }

        public async Task<List<AcademicRecord>> GetAcademicRecordsAsync(int studentId)
        {
            return _academicRecords.Where(r => r.StudentId == studentId)
                                  .OrderBy(r => r.Grade)
                                  .ToList();
        }

        public async Task<AcademicRecord?> GetAcademicRecordByGradeAsync(int studentId, int grade)
        {
            return _academicRecords.FirstOrDefault(r => r.StudentId == studentId && r.Grade == grade);
        }

        public async Task<List<TimetableEntry>> GetTimetableAsync(int grade)
        {
            return _timetableEntries.Where(t => t.Grade == grade)
                                   .OrderBy(t => GetDayOrder(t.DayOfWeek))
                                   .ThenBy(t => t.StartTime)
                                   .ToList();
        }

        public async Task<List<TimetableEntry>> GetTimetableForDayAsync(int grade, string dayOfWeek)
        {
            return _timetableEntries.Where(t => t.Grade == grade && t.DayOfWeek.Equals(dayOfWeek, StringComparison.OrdinalIgnoreCase))
                                   .OrderBy(t => t.StartTime)
                                   .ToList();
        }

        public async Task<StudentProgress> GetStudentProgressAsync(int studentId)
        {
            var records = await GetAcademicRecordsAsync(studentId);
            var currentRecord = records.LastOrDefault();

            var progress = new StudentProgress
            {
                StudentId = studentId,
                StudentName = "Student Name", // This would come from user data
                CurrentGrade = currentRecord?.Grade ?? 1,
                AcademicHistory = records,
                CurrentGPA = currentRecord?.OverallAverage ?? 0,
                CurrentClassPosition = currentRecord?.ClassPosition ?? 0,
                CurrentStreamPosition = currentRecord?.StreamPosition ?? 0,
                PerformanceTrend = CalculatePerformanceTrend(records),
                Strengths = CalculateStrengths(records),
                AreasForImprovement = CalculateAreasForImprovement(records)
            };

            return progress;
        }

        private string CalculatePerformanceTrend(List<AcademicRecord> records)
        {
            if (records.Count < 2) return "Stable";

            var recent = records.TakeLast(2).ToList();
            var improvement = recent[1].OverallAverage - recent[0].OverallAverage;

            return improvement switch
            {
                > 5 => "Improving",
                < -5 => "Declining",
                _ => "Stable"
            };
        }

        private List<string> CalculateStrengths(List<AcademicRecord> records)
        {
            var strengths = new List<string>();
            var latestRecord = records.LastOrDefault();

            if (latestRecord != null)
            {
                var topSubjects = latestRecord.SubjectGrades
                    .OrderByDescending(s => s.AverageMark)
                    .Take(3)
                    .Select(s => s.SubjectName)
                    .ToList();

                strengths.AddRange(topSubjects);
            }

            return strengths;
        }

        private List<string> CalculateAreasForImprovement(List<AcademicRecord> records)
        {
            var improvements = new List<string>();
            var latestRecord = records.LastOrDefault();

            if (latestRecord != null)
            {
                var weakSubjects = latestRecord.SubjectGrades
                    .Where(s => s.AverageMark < 60)
                    .OrderBy(s => s.AverageMark)
                    .Take(2)
                    .Select(s => s.SubjectName)
                    .ToList();

                improvements.AddRange(weakSubjects);
            }

            return improvements;
        }

        private int GetDayOrder(string dayOfWeek)
        {
            return dayOfWeek.ToLower() switch
            {
                "monday" => 1,
                "tuesday" => 2,
                "wednesday" => 3,
                "thursday" => 4,
                "friday" => 5,
                _ => 6
            };
        }
    }
}
