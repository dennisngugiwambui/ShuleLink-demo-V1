using ShuleLink.Services;

namespace ShuleLink
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            
            // Handle navigation events
            this.Navigating += OnNavigating;
        }

        private void RegisterRoutes()
        {
            // Authentication routes
            Routing.RegisterRoute("WelcomePage", typeof(Views.WelcomePage));
            Routing.RegisterRoute("LoginPage", typeof(Views.LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(Views.RegisterPage));
            
            // Main app routes
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            
            // Sub-page routes
            // Register all page routes for navigation (since we removed TabBar)
            Routing.RegisterRoute("ChatList", typeof(Views.ChatListPage));
            Routing.RegisterRoute("Learning", typeof(Views.LearningPage));
            Routing.RegisterRoute("Quiz", typeof(Views.QuizPage));
            Routing.RegisterRoute("Profile", typeof(Views.ProfilePage));
            // Routing.RegisterRoute("DiscussionForum", typeof(Views.DiscussionForumPage)); // Removed - using 'discussions' instead
            
            // Register additional routes for navigation
            Routing.RegisterRoute("ChatPage", typeof(Views.ChatPage));
            Routing.RegisterRoute("ReadingDetailPage", typeof(Views.ReadingDetailPage));
            Routing.RegisterRoute("DiagramPage", typeof(Views.DiagramPage));
            Routing.RegisterRoute("SplashPage", typeof(Views.SplashPage));
            
            // Academic and timetable routes
            Routing.RegisterRoute("academicrecords", typeof(Views.AcademicRecordsPage));
            Routing.RegisterRoute("timetable", typeof(Views.TimetablePage));
            Routing.RegisterRoute("discussions", typeof(Views.DiscussionsPage));
            Routing.RegisterRoute("creatediscussion", typeof(Views.CreateDiscussionPage));
        }

        private async void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            try
            {
                var targetRoute = e.Target.Location.OriginalString;
                System.Diagnostics.Debug.WriteLine($"=== APPSHELL NAVIGATION EVENT ===");
                System.Diagnostics.Debug.WriteLine($"📍 Navigation to: {targetRoute}");
                System.Diagnostics.Debug.WriteLine($"📍 Navigation source: {e.Source}");
                System.Diagnostics.Debug.WriteLine($"📍 Can cancel: {e.CanCancel}");
                System.Diagnostics.Debug.WriteLine($"📍 Current time: {DateTime.Now}");
                
                // Check if this is the problematic MainTabs navigation
                if (targetRoute.Contains("MainTabs"))
                {
                    System.Diagnostics.Debug.WriteLine("=== 🚨 MAINTABS NAVIGATION DETECTED 🚨 ===");
                    System.Diagnostics.Debug.WriteLine("This is the critical navigation that might be causing crashes!");
                    System.Diagnostics.Debug.WriteLine("Monitoring tab page creation...");
                    
                    // Test if we can access the tab pages safely
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("🔍 Testing tab page accessibility...");
                        
                        // Check if MainPage can be created
                        System.Diagnostics.Debug.WriteLine("Testing MainPage access...");
                        var testMain = typeof(MainPage);
                        System.Diagnostics.Debug.WriteLine($"✅ MainPage type accessible: {testMain.Name}");
                        
                        // Check if ChatListPage can be created
                        System.Diagnostics.Debug.WriteLine("Testing ChatListPage access...");
                        var testChat = typeof(Views.ChatListPage);
                        System.Diagnostics.Debug.WriteLine($"✅ ChatListPage type accessible: {testChat.Name}");
                        
                        // Check if LearningPage can be created
                        System.Diagnostics.Debug.WriteLine("Testing LearningPage access...");
                        var testLearning = typeof(Views.LearningPage);
                        System.Diagnostics.Debug.WriteLine($"✅ LearningPage type accessible: {testLearning.Name}");
                        
                        System.Diagnostics.Debug.WriteLine("✅ All tab page types are accessible!");
                    }
                    catch (Exception typeEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Tab page type access failed: {typeEx.Message}");
                        System.Diagnostics.Debug.WriteLine($"Type error stack trace: {typeEx.StackTrace}");
                    }
                }
                
                System.Diagnostics.Debug.WriteLine("✅ Navigation guard allowing navigation");
                System.Diagnostics.Debug.WriteLine("=== APPSHELL NAVIGATION PROCEEDING ===");
                return;
                
                /*
                var routeName = ExtractRouteName(targetRoute);
                
                // Define protected routes
                var authenticatedRoutes = new[] { "MainTabs", "MainPage", "ChatList", "Learning", "Quiz", "Profile", "DiscussionForum", "ChatPage", "ReadingDetailPage", "DiagramPage", "DiscussionForumPage", "academicrecords", "timetable", "discussions", "creatediscussion" };
                var unauthenticatedRoutes = new[] { "WelcomePage", "LoginPage", "RegisterPage" };
                
                // Check if user is trying to access authenticated routes
                if (authenticatedRoutes.Any(route => targetRoute.Contains(route)))
                {
                    var isValidSession = await NavigationService.ValidateUserSession();
                    if (!isValidSession)
                    {
                        e.Cancel();
                        await DisplayAlert("Session Expired", "Your session has expired. Please log in again.", "OK");
                        await Shell.Current.GoToAsync("//WelcomePage");
                        return;
                    }
                }
                */
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AppShell navigation error: {ex.Message}");
                // Don't cancel navigation on error - let it proceed
            }
            
            /*
            // Check if logged-in user is trying to access authentication pages
            var isLoggedIn = NavigationService.IsUserLoggedIn();
            if (isLoggedIn && unauthenticatedRoutes.Any(route => targetRoute.Contains(route)))
            {
                // Only allow navigation to WelcomePage for logout
                if (routeName != "WelcomePage")
                {
                    e.Cancel();
                    await Shell.Current.GoToAsync("//MainTabs");
                    return;
                }
            }
            */
        }

        private string ExtractRouteName(string fullRoute)
        {
            if (fullRoute.Contains("//"))
            {
                return fullRoute.Split("//").LastOrDefault() ?? "";
            }
            return fullRoute.Split('/').LastOrDefault() ?? "";
        }

        protected override bool OnBackButtonPressed()
        {
            try
            {
                var currentRoute = Shell.Current.CurrentState.Location.OriginalString;
                var routeName = ExtractRouteName(currentRoute);
                
                System.Diagnostics.Debug.WriteLine($"Back button pressed on route: {routeName}");
                
                // Handle specific cases where we want custom back behavior
                switch (routeName)
                {
                    case "MainPage":
                    case "MainTabs":
                        // On main dashboard, exit the app
                        System.Diagnostics.Debug.WriteLine("On main page - exiting app");
                        Application.Current?.Quit();
                        return true;
                        
                    case "WelcomePage":
                        // On welcome page, exit the app
                        System.Diagnostics.Debug.WriteLine("On welcome page - exiting app");
                        Application.Current?.Quit();
                        return true;
                        
                    case "LoginPage":
                    case "RegisterPage":
                        // From login/register, go back to welcome
                        System.Diagnostics.Debug.WriteLine("From auth page - going to welcome");
                        Shell.Current.GoToAsync("//WelcomePage");
                        return true;
                        
                    default:
                        // For all other pages, allow default back navigation
                        System.Diagnostics.Debug.WriteLine("Default back navigation");
                        return false; // Let Shell handle the navigation
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Back button error: {ex.Message}");
                // On error, allow default back navigation
                return false;
            }
        }
    }
}
