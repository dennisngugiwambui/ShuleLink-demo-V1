using SQLite;
using ShuleLink.Models;
using System.Security.Cryptography;
using System.Text;

namespace ShuleLink.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

        public async Task<SQLiteAsyncConnection> GetDatabaseAsync()
        {
            if (_database is not null)
                return _database;

            try
            {
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "ShuleLink.db3");
                System.Diagnostics.Debug.WriteLine($"Database path: {databasePath}");
                
                _database = new SQLiteAsyncConnection(databasePath);
                
                // Create tables with error handling
                await _database.CreateTableAsync<User>();
                System.Diagnostics.Debug.WriteLine("User table created successfully");
                
                await _database.CreateTableAsync<Teacher>();
                System.Diagnostics.Debug.WriteLine("Teacher table created successfully");
                
                await _database.CreateTableAsync<ChatMessage>();
                System.Diagnostics.Debug.WriteLine("ChatMessage table created successfully");
                
                await _database.CreateTableAsync<ChatConversation>();
                System.Diagnostics.Debug.WriteLine("ChatConversation table created successfully");
                
                await _database.CreateTableAsync<QuizQuestion>();
                System.Diagnostics.Debug.WriteLine("QuizQuestion table created successfully");
                
                await _database.CreateTableAsync<QuizAttempt>();
                System.Diagnostics.Debug.WriteLine("QuizAttempt table created successfully");
                
                await _database.CreateTableAsync<DailyQuote>();
                System.Diagnostics.Debug.WriteLine("DailyQuote table created successfully");
                
                await InitializeDefaultUsersAsync();
                await InitializeDefaultTeachersAsync();
                
                // Test database connectivity
                var userCount = await _database.Table<User>().CountAsync();
                System.Diagnostics.Debug.WriteLine($"Database test: Found {userCount} users in database");
                
                System.Diagnostics.Debug.WriteLine("Database initialization completed successfully");
                return _database;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw new Exception($"Failed to initialize database: {ex.Message}", ex);
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var database = await GetDatabaseAsync();
            return await database.Table<User>().ToListAsync();
        }

        public async Task<User?> GetUserAsync(int userId)
        {
            try
            {
                var database = await GetDatabaseAsync();
                var user = await database.Table<User>().Where(u => u.Id == userId).FirstOrDefaultAsync();
                System.Diagnostics.Debug.WriteLine($"GetUserAsync: Found user with ID {userId}: {user?.Name ?? "null"}");
                return user;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetUserAsync error: {ex.Message}");
                throw new Exception($"Failed to get user with ID {userId}: {ex.Message}", ex);
            }
        }

        public async Task<User?> GetUserByPhoneAsync(string phoneNumber)
        {
            var database = await GetDatabaseAsync();
            return await database.Table<User>().Where(u => u.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
        }

        public async Task<int> SaveUserAsync(User user)
        {
            var database = await GetDatabaseAsync();
            user.UpdatedAt = DateTime.Now;
            
            if (user.Id != 0)
                return await database.UpdateAsync(user);
            else
            {
                user.CreatedAt = DateTime.Now;
                user.Password = HashPassword(user.Password);
                return await database.InsertAsync(user);
            }
        }

        public async Task<int> DeleteUserAsync(User user)
        {
            var database = await GetDatabaseAsync();
            return await database.DeleteAsync(user);
        }

        public async Task<(bool IsValid, string Message, User? User)> ValidateUserAsync(string phoneNumber, string password)
        {
            var user = await GetUserByPhoneAsync(phoneNumber);
            if (user == null) 
                return (false, "Invalid phone number or password.", null);
            
            if (!VerifyPassword(password, user.Password))
                return (false, "Invalid phone number or password.", null);

            // Check if user is a student
            if (user.UserType.ToLower() != "student")
                return (false, "Access denied. This app is only for students.", null);

            // Check if user has graduated
            if (user.Graduated)
                return (false, "You have already graduated so you are no longer part of us.", null);

            // Check if user status is active
            if (user.Status.ToLower() != "active")
                return (false, "Your account is not active. Please contact the school administration.", null);

            return (true, "Login successful.", user);
        }

        private async Task InitializeDefaultUsersAsync()
        {
            var database = await GetDatabaseAsync();
            var existingUsers = await database.Table<User>().CountAsync();
            
            if (existingUsers == 0)
            {
                var defaultUsers = new List<User>
                {
                    new User
                    {
                        Name = "Shule Student 1",
                        PhoneNumber = "254758024400",
                        Password = HashPassword("password123"),
                        AdmissionNumber = "12345",
                        Grade = "5",
                        Class = "5 East",
                        ParentName = "Shule Parent 1",
                        HomeAddress = "Olkalou, Salient",
                        Status = "active",
                        UserType = "student",
                        Graduated = false
                    },
                    new User
                    {
                        Name = "Shule Student 2",
                        PhoneNumber = "254758024401",
                        Password = HashPassword("password123"),
                        AdmissionNumber = "12346",
                        Grade = "6",
                        Class = "6 West",
                        ParentName = "Shule Parent 2",
                        HomeAddress = "Nakuru, Kenya",
                        Status = "active",
                        UserType = "student",
                        Graduated = false
                    },
                    new User
                    {
                        Name = "Shule Student 3",
                        PhoneNumber = "254758024402",
                        Password = HashPassword("password123"),
                        AdmissionNumber = "12347",
                        Grade = "7",
                        Class = "7 North",
                        ParentName = "Shule Parent 3",
                        HomeAddress = "Eldoret, Kenya",
                        Status = "active",
                        UserType = "student",
                        Graduated = false
                    },
                    new User
                    {
                        Name = "Shule Student 4",
                        PhoneNumber = "254758024403",
                        Password = HashPassword("password123"),
                        AdmissionNumber = "12348",
                        Grade = "8",
                        Class = "8 South",
                        ParentName = "Shule Parent 4",
                        HomeAddress = "Kisumu, Kenya",
                        Status = "active",
                        UserType = "student",
                        Graduated = false
                    },
                    new User
                    {
                        Name = "Shule Student 5",
                        PhoneNumber = "254758024404",
                        Password = HashPassword("password123"),
                        AdmissionNumber = "12349",
                        Grade = "4",
                        Class = "4 Central",
                        ParentName = "Shule Parent 5",
                        HomeAddress = "Mombasa, Kenya",
                        Status = "active",
                        UserType = "student",
                        Graduated = false
                    }
                };

                foreach (var user in defaultUsers)
                {
                    await database.InsertAsync(user);
                }
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "ShuleLink_Salt"));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hashedPassword;
        }

        // Teacher methods
        public async Task<List<Teacher>> GetTeachersAsync()
        {
            var database = await GetDatabaseAsync();
            return await database.Table<Teacher>().ToListAsync();
        }

        public async Task<Teacher?> GetClassTeacherAsync(string grade)
        {
            var database = await GetDatabaseAsync();
            return await database.Table<Teacher>()
                .Where(t => t.IsClassTeacher && t.ClassAssigned.Contains(grade))
                .FirstOrDefaultAsync();
        }

        public async Task<List<Teacher>> GetSubjectTeachersAsync(string grade)
        {
            var database = await GetDatabaseAsync();
            return await database.Table<Teacher>()
                .Where(t => !t.IsClassTeacher && (t.Grade.Contains(grade) || t.Grade == "All"))
                .ToListAsync();
        }

        // Chat methods
        public async Task<List<ChatConversation>> GetUserConversationsAsync(int userId)
        {
            var database = await GetDatabaseAsync();
            return await database.Table<ChatConversation>()
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.LastMessageAt)
                .ToListAsync();
        }

        public async Task<List<ChatMessage>> GetChatMessagesAsync(int userId, int teacherId)
        {
            var database = await GetDatabaseAsync();
            return await database.Table<ChatMessage>()
                .Where(m => (m.SenderId == userId && m.ReceiverId == teacherId) || 
                           (m.SenderId == teacherId && m.ReceiverId == userId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<int> SaveChatMessageAsync(ChatMessage message)
        {
            var database = await GetDatabaseAsync();
            return await database.InsertAsync(message);
        }

        // Quiz methods
        public async Task<List<QuizQuestion>> GetQuizQuestionsAsync(string subject, string grade)
        {
            var database = await GetDatabaseAsync();
            return await database.Table<QuizQuestion>()
                .Where(q => q.Subject == subject && q.Grade == grade)
                .ToListAsync();
        }

        public async Task<int> SaveQuizQuestionAsync(QuizQuestion question)
        {
            var database = await GetDatabaseAsync();
            return await database.InsertAsync(question);
        }

        public async Task<int> SaveQuizAttemptAsync(QuizAttempt attempt)
        {
            var database = await GetDatabaseAsync();
            return await database.InsertAsync(attempt);
        }

        // Daily Quote methods
        public async Task<DailyQuote?> GetTodayQuoteAsync()
        {
            var database = await GetDatabaseAsync();
            return await database.Table<DailyQuote>()
                .Where(q => q.Date.Date == DateTime.Today)
                .FirstOrDefaultAsync();
        }

        public async Task<int> SaveDailyQuoteAsync(DailyQuote quote)
        {
            var database = await GetDatabaseAsync();
            return await database.InsertAsync(quote);
        }

        private async Task InitializeDefaultTeachersAsync()
        {
            var database = await GetDatabaseAsync();
            var existingTeachers = await database.Table<Teacher>().CountAsync();
            
            if (existingTeachers == 0)
            {
                var defaultTeachers = new List<Teacher>
                {
                    // Head Teacher
                    new Teacher
                    {
                        Name = "Dr. Sarah Wanjiku",
                        PhoneNumber = "254712345678",
                        Email = "headteacher@shulelink.ac.ke",
                        Subject = "Administration",
                        Grade = "All",
                        Role = TeacherRole.HeadTeacher,
                        IsClassTeacher = false,
                        Bio = "Experienced educator with 20+ years in primary education. Passionate about student development and academic excellence.",
                        IsOnline = true
                    },
                    
                    // Deputy Head Teacher
                    new Teacher
                    {
                        Name = "Mr. John Kiprotich",
                        PhoneNumber = "254723456789",
                        Email = "deputy@shulelink.ac.ke",
                        Subject = "Mathematics",
                        Grade = "5-7",
                        Role = TeacherRole.DeputyHeadTeacher,
                        IsClassTeacher = false,
                        Bio = "Mathematics specialist and deputy head teacher. Loves making math fun and accessible for all students.",
                        IsOnline = true
                    },
                    
                    // Secretary
                    new Teacher
                    {
                        Name = "Ms. Grace Akinyi",
                        PhoneNumber = "254734567890",
                        Email = "secretary@shulelink.ac.ke",
                        Subject = "Administration",
                        Grade = "All",
                        Role = TeacherRole.Secretary,
                        IsClassTeacher = false,
                        Bio = "School secretary handling administrative duties and student records.",
                        IsOnline = false
                    },
                    
                    // Class Teachers
                    new Teacher
                    {
                        Name = "Mrs. Mary Njeri",
                        PhoneNumber = "254745678901",
                        Email = "grade1@shulelink.ac.ke",
                        Subject = "General Studies",
                        Grade = "1",
                        Role = TeacherRole.ClassTeacher,
                        IsClassTeacher = true,
                        ClassAssigned = "Grade 1A",
                        Bio = "Dedicated Grade 1 teacher with expertise in early childhood education.",
                        IsOnline = true
                    },
                    
                    new Teacher
                    {
                        Name = "Mr. Peter Mwangi",
                        PhoneNumber = "254756789012",
                        Email = "grade2@shulelink.ac.ke",
                        Subject = "General Studies",
                        Grade = "2",
                        Role = TeacherRole.ClassTeacher,
                        IsClassTeacher = true,
                        ClassAssigned = "Grade 2B",
                        Bio = "Creative Grade 2 teacher who loves storytelling and interactive learning.",
                        IsOnline = true
                    },
                    
                    new Teacher
                    {
                        Name = "Ms. Faith Wambui",
                        PhoneNumber = "254767890123",
                        Email = "grade3@shulelink.ac.ke",
                        Subject = "English & Literature",
                        Grade = "3",
                        Role = TeacherRole.ClassTeacher,
                        IsClassTeacher = true,
                        ClassAssigned = "Grade 3C",
                        Bio = "English specialist and Grade 3 class teacher. Passionate about reading and writing.",
                        IsOnline = false
                    },
                    
                    new Teacher
                    {
                        Name = "Mr. David Ochieng",
                        PhoneNumber = "254778901234",
                        Email = "grade4@shulelink.ac.ke",
                        Subject = "Science & Mathematics",
                        Grade = "4",
                        Role = TeacherRole.ClassTeacher,
                        IsClassTeacher = true,
                        ClassAssigned = "Grade 4 Central",
                        Bio = "Science and math teacher for Grade 4. Makes learning fun with experiments and practical examples.",
                        IsOnline = true
                    },
                    
                    new Teacher
                    {
                        Name = "Mrs. Agnes Mutindi",
                        PhoneNumber = "254789012345",
                        Email = "grade5@shulelink.ac.ke",
                        Subject = "Social Studies",
                        Grade = "5",
                        Role = TeacherRole.ClassTeacher,
                        IsClassTeacher = true,
                        ClassAssigned = "Grade 5 East",
                        Bio = "Social studies expert and Grade 5 class teacher. Loves teaching about history and geography.",
                        IsOnline = true
                    },
                    
                    new Teacher
                    {
                        Name = "Mr. Samuel Kiplagat",
                        PhoneNumber = "254790123456",
                        Email = "grade6@shulelink.ac.ke",
                        Subject = "Science",
                        Grade = "6",
                        Role = TeacherRole.ClassTeacher,
                        IsClassTeacher = true,
                        ClassAssigned = "Grade 6 West",
                        Bio = "Science teacher specializing in biology and environmental studies for Grade 6.",
                        IsOnline = false
                    },
                    
                    new Teacher
                    {
                        Name = "Ms. Rebecca Chebet",
                        PhoneNumber = "254701234567",
                        Email = "grade7@shulelink.ac.ke",
                        Subject = "Mathematics & Science",
                        Grade = "7",
                        Role = TeacherRole.ClassTeacher,
                        IsClassTeacher = true,
                        ClassAssigned = "Grade 7 North",
                        Bio = "Mathematics and science teacher for Grade 7. Prepares students for secondary school.",
                        IsOnline = true
                    },
                    
                    // Subject Teachers
                    new Teacher
                    {
                        Name = "Mr. James Otieno",
                        PhoneNumber = "254712345679",
                        Email = "pe@shulelink.ac.ke",
                        Subject = "Physical Education",
                        Grade = "All",
                        Role = TeacherRole.SubjectTeacher,
                        IsClassTeacher = false,
                        Bio = "Physical education teacher promoting fitness and sports across all grades.",
                        IsOnline = true
                    },
                    
                    new Teacher
                    {
                        Name = "Mrs. Catherine Wanjala",
                        PhoneNumber = "254723456780",
                        Email = "music@shulelink.ac.ke",
                        Subject = "Music & Arts",
                        Grade = "All",
                        Role = TeacherRole.SubjectTeacher,
                        IsClassTeacher = false,
                        Bio = "Music and arts teacher inspiring creativity in students of all ages.",
                        IsOnline = false
                    },
                    
                    new Teacher
                    {
                        Name = "Mr. Francis Kamau",
                        PhoneNumber = "254734567881",
                        Email = "cre@shulelink.ac.ke",
                        Subject = "Christian Religious Education",
                        Grade = "All",
                        Role = TeacherRole.SubjectTeacher,
                        IsClassTeacher = false,
                        Bio = "CRE teacher focusing on moral and spiritual development of students.",
                        IsOnline = true
                    }
                };

                foreach (var teacher in defaultTeachers)
                {
                    await database.InsertAsync(teacher);
                }
            }
        }
    }
}
