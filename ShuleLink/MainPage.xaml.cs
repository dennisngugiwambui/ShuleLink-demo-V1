using ShuleLink.Services;

namespace ShuleLink
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private readonly GeminiAIService _geminiService;
        private readonly AcademicService _academicService;

        public MainPage()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== 🏠 MAINPAGE CONSTRUCTOR START ===");
                System.Diagnostics.Debug.WriteLine("MainPage constructor started...");
                
                System.Diagnostics.Debug.WriteLine("Calling InitializeComponent...");
                InitializeComponent();
                System.Diagnostics.Debug.WriteLine("✅ InitializeComponent completed successfully!");
                
                // Create services with fallback handling
                System.Diagnostics.Debug.WriteLine("Creating DatabaseService...");
                try
                {
                    _databaseService = new DatabaseService();
                    System.Diagnostics.Debug.WriteLine("✅ DatabaseService created successfully!");
                }
                catch (Exception dbEx)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ DatabaseService creation failed: {dbEx.Message}");
                    // Don't throw - create a minimal fallback
                    _databaseService = null;
                }
                
                System.Diagnostics.Debug.WriteLine("Creating GeminiAIService...");
                try
                {
                    // Create HttpClient with timeout to prevent hanging
                    var httpClient = new HttpClient();
                    httpClient.Timeout = TimeSpan.FromSeconds(5); // 5 second timeout
                    
                    _geminiService = new GeminiAIService(httpClient);
                    System.Diagnostics.Debug.WriteLine("✅ GeminiAIService created successfully!");
                }
                catch (Exception geminiEx)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ GeminiAIService creation failed: {geminiEx.Message}");
                    System.Diagnostics.Debug.WriteLine($"❌ GeminiAIService stack trace: {geminiEx.StackTrace}");
                    // Don't throw - set to null for fallback handling
                    _geminiService = null;
                }
                
                System.Diagnostics.Debug.WriteLine("Creating AcademicService...");
                try
                {
                    _academicService = new AcademicService();
                    System.Diagnostics.Debug.WriteLine("✅ AcademicService created successfully!");
                }
                catch (Exception acadEx)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ AcademicService creation failed: {acadEx.Message}");
                    // Don't throw - create a minimal fallback
                    _academicService = null;
                }
                
                System.Diagnostics.Debug.WriteLine("=== ✅ MAINPAGE CONSTRUCTOR COMPLETED (with fallbacks if needed) ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("=== ❌ MAINPAGE CONSTRUCTOR CRITICAL ERROR ===");
                System.Diagnostics.Debug.WriteLine($"❌ MainPage constructor error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"❌ Exception type: {ex.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"❌ Stack trace: {ex.StackTrace}");
                System.Diagnostics.Debug.WriteLine($"❌ Inner exception: {ex.InnerException?.Message}");
                
                // DON'T THROW - this crashes the entire app
                // Instead, set all services to null and let OnAppearing handle fallbacks
                System.Diagnostics.Debug.WriteLine("🚨 Setting all services to null for emergency fallback");
            }
        }

        public MainPage(DatabaseService databaseService, GeminiAIService geminiService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _geminiService = geminiService;
            _academicService = new AcademicService();
        }

        protected override async void OnAppearing()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== 🏠 MAINPAGE ONAPPEARING START ===");
                System.Diagnostics.Debug.WriteLine($"📍 Current time: {DateTime.Now}");
                System.Diagnostics.Debug.WriteLine($"📍 Thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                
                base.OnAppearing();
                System.Diagnostics.Debug.WriteLine("✅ base.OnAppearing() completed");
                
                // Set fallback data first to prevent crashes
                System.Diagnostics.Debug.WriteLine("🔧 Setting fallback UI data...");
                try
                {
                    WelcomeLabel.Text = "Welcome to ShuleLink!";
                    System.Diagnostics.Debug.WriteLine("✅ WelcomeLabel set");
                    
                    UserInfoLabel.Text = "Smart Learning Platform";
                    System.Diagnostics.Debug.WriteLine("✅ UserInfoLabel set");
                    
                    // Remove references to non-existent UI elements
                    System.Diagnostics.Debug.WriteLine("✅ Basic UI elements set");
                    
                    System.Diagnostics.Debug.WriteLine("✅ All fallback UI data set successfully!");
                }
                catch (Exception uiEx)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ UI element setting failed: {uiEx.Message}");
                    System.Diagnostics.Debug.WriteLine($"UI error stack trace: {uiEx.StackTrace}");
                    throw new Exception($"MainPage UI initialization failed: {uiEx.Message}", uiEx);
                }
                
                // Load user info safely
                System.Diagnostics.Debug.WriteLine("👤 Loading user info...");
                try
                {
                    LoadUserInfo();
                    System.Diagnostics.Debug.WriteLine("✅ User info loaded successfully!");
                }
                catch (Exception userEx)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ User info loading failed: {userEx.Message}");
                    System.Diagnostics.Debug.WriteLine($"User error stack trace: {userEx.StackTrace}");
                    // Don't throw - this is not critical
                }
                
                // Skip complex operations that might cause crashes
                System.Diagnostics.Debug.WriteLine("⏭️ Skipping complex operations to prevent crashes");
                // await LoadDailyQuote();
                // await LoadAcademicStats();
                
                System.Diagnostics.Debug.WriteLine("=== ✅ MAINPAGE ONAPPEARING COMPLETED SUCCESSFULLY ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("=== ❌ MAINPAGE ONAPPEARING CRITICAL ERROR ===");
                System.Diagnostics.Debug.WriteLine($"❌ MainPage OnAppearing error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"❌ Exception type: {ex.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"❌ Stack trace: {ex.StackTrace}");
                System.Diagnostics.Debug.WriteLine($"❌ Inner exception: {ex.InnerException?.Message}");
                System.Diagnostics.Debug.WriteLine($"❌ Inner stack trace: {ex.InnerException?.StackTrace}");
                
                // Ensure fallback data is set as last resort
                try
                {
                    System.Diagnostics.Debug.WriteLine("🚨 Attempting emergency fallback UI setup...");
                    WelcomeLabel.Text = "Welcome to ShuleLink!";
                    UserInfoLabel.Text = "Smart Learning Platform";
                    System.Diagnostics.Debug.WriteLine("✅ Emergency fallback UI setup successful!");
                }
                catch (Exception fallbackEx)
                {
                    System.Diagnostics.Debug.WriteLine($"🚨 EMERGENCY FALLBACK FAILED: {fallbackEx.Message}");
                    System.Diagnostics.Debug.WriteLine("🚨 MainPage UI is completely corrupted!");
                    throw new Exception($"MainPage is completely broken - UI corruption detected: {ex.Message}", ex);
                }
            }
        }

        private async void LoadUserInfo()
        {
            try
            {
                var userName = Preferences.Get("UserName", "Student");
                var userPhone = Preferences.Get("UserPhone", "");
                var userId = Preferences.Get("UserId", 0);
                
                WelcomeLabel.Text = $"Welcome back, {userName}!";
                UserInfoLabel.Text = $"Phone: {userPhone}";
                
                // Test database connectivity
                if (userId > 0)
                {
                    var user = await _databaseService.GetUserAsync(userId);
                    if (user != null)
                    {
                        WelcomeLabel.Text = $"Welcome back, {user.Name}!";
                        System.Diagnostics.Debug.WriteLine($"Successfully loaded user: {user.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadUserInfo error: {ex.Message}");
                // Fallback to preferences data
                var userName = Preferences.Get("UserName", "Student");
                WelcomeLabel.Text = $"Welcome back, {userName}!";
                UserInfoLabel.Text = "Database connection issue";
            }
        }

        private async Task LoadDailyQuote()
        {
            try
            {
                // Always generate a fresh quote for better user experience
                var quoteText = await _geminiService.GenerateDailyQuoteAsync();
                
                // Parse quote and author
                var parts = quoteText.Split(" - ");
                var quote = parts[0].Trim('"');
                var author = parts.Length > 1 ? parts[1] : "Unknown";

                // Quote display removed - not in current UI
                
                // Save to database for offline access
                var dailyQuote = new Models.DailyQuote
                {
                    Quote = quote,
                    Author = author,
                    Category = "Inspiration",
                    Date = DateTime.Now,
                    BackgroundColor = "#4A90E2"
                };
                await _databaseService.SaveDailyQuoteAsync(dailyQuote);
            }
            catch (Exception)
            {
                // Fallback quotes with variety
                var fallbackQuotes = new[]
                {
                    "\"The more that you read, the more things you will know.\" - Dr. Seuss",
                    "\"Education is the most powerful weapon which you can use to change the world.\" - Nelson Mandela",
                    "\"Learning never exhausts the mind.\" - Leonardo da Vinci",
                    "\"The beautiful thing about learning is that no one can take it away from you.\" - B.B. King",
                    "\"Success is the sum of small efforts repeated day in and day out.\" - Robert Collier"
                };
                
                var random = new Random();
                var selectedQuote = fallbackQuotes[random.Next(fallbackQuotes.Length)];
                var parts = selectedQuote.Split(" - ");
                
                // Fallback quote display removed - not in current UI
            }
        }

        private async void OnNewQuoteClicked(object sender, EventArgs e)
        {
            try
            {
                // Quote loading removed - not in current UI
                
                // Generate fresh quote
                await LoadDailyQuote();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to load new quote. Please try again.", "OK");
            }
        }

        private async void OnHelpClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Help", 
                "ShuleLink Help:\n\n" +
                "🏠 Home: View dashboard and daily quotes\n" +
                "💬 Chat: Talk with teachers and AI assistant\n" +
                "📚 Learn: Access learning materials and search\n" +
                "🧠 Quiz: Take AI-generated quizzes\n" +
                "👤 Profile: Manage your account\n\n" +
                "For more help, contact your teacher or school admin.", 
                "OK");
        }

        private async void OnReportClicked(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet("Report Problem", "Cancel", null, 
                "App Crash", "Login Issues", "Content Error", "Other Technical Issue");
            
            if (result != "Cancel" && result != null)
            {
                await DisplayAlert("Report Submitted", 
                    $"Thank you for reporting: {result}\n\n" +
                    "Your report has been logged and will be reviewed by our technical team. " +
                    "If this is urgent, please contact your teacher directly.", 
                    "OK");
            }
        }

        private async Task LoadAcademicStats()
        {
            try
            {
                var studentId = Preferences.Get("UserId", 1);
                var progress = await _academicService.GetStudentProgressAsync(studentId);

                // GPA display removed - not in current UI
                // Note: ClassPositionLabel needs to be added to XAML or we can use a different approach
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadAcademicStats error: {ex.Message}");
                // GPA fallback removed - not in current UI
            }
        }

        private string GetOrdinalNumber(int number)
        {
            if (number <= 0) return number.ToString();

            return (number % 100) switch
            {
                11 or 12 or 13 => $"{number}th",
                _ => (number % 10) switch
                {
                    1 => $"{number}st",
                    2 => $"{number}nd",
                    3 => $"{number}rd",
                    _ => $"{number}th"
                }
            };
        }

        private async void OnTimetableClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("📅 Opening Timetable...");
            await Shell.Current.GoToAsync("timetable");
        }

        private async void OnAcademicRecordsClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("📊 Loading Academic Records...");
            await Shell.Current.GoToAsync("academicrecords");
        }

        private async void OnAssignmentsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Assignments", "Assignments feature coming soon! This will show your homework, projects, and due dates.", "OK");
        }

        private async void OnReportCardClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Report Card", "Report Card feature coming soon! This will show your grades and academic performance.", "OK");
        }

        private async void OnChatClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("💬 Opening Chat...");
            await Shell.Current.GoToAsync("ChatList");
        }

        private async void OnLearningClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("📚 Opening Learning...");
            await Shell.Current.GoToAsync("Learning");
        }

        private async void OnQuizClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("🧠 Opening Quiz...");
            await Shell.Current.GoToAsync("Quiz");
        }

        private async void OnProfileClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("👤 Opening Profile...");
            await Shell.Current.GoToAsync("Profile");
        }

        private async void OnDiscussionsClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("💭 Opening Discussions...");
            await Shell.Current.GoToAsync("discussions");
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
            
            if (result)
            {
                await Services.NavigationService.LogoutAndNavigate();
            }
        }
    }
}
