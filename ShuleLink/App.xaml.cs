using ShuleLink.Services;

namespace ShuleLink
{
    public partial class App : Application
    {
        public App()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== 🚀 APP CONSTRUCTOR START ===");
                System.Diagnostics.Debug.WriteLine($"📍 Current time: {DateTime.Now}");
                
                // Set up global exception handling
                SetupGlobalExceptionHandling();
                
                System.Diagnostics.Debug.WriteLine("App constructor started...");
                InitializeComponent();
                System.Diagnostics.Debug.WriteLine("InitializeComponent completed...");

                // Initialize theme system
                System.Diagnostics.Debug.WriteLine("🎨 Initializing theme system...");
                ThemeService.InitializeTheme();
                System.Diagnostics.Debug.WriteLine("✅ Theme system initialized");

                // Skip splash to avoid bitmap allocation issues - go directly to AppShell
                MainPage = new AppShell();
                System.Diagnostics.Debug.WriteLine("AppShell set as MainPage...");
                
                // Don't auto-navigate - let the shell handle routing
                System.Diagnostics.Debug.WriteLine("=== ✅ APP CONSTRUCTOR COMPLETED ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("=== ❌ APP CONSTRUCTOR ERROR ===");
                System.Diagnostics.Debug.WriteLine($"❌ App constructor error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"❌ Exception type: {ex.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"❌ Stack trace: {ex.StackTrace}");
                System.Diagnostics.Debug.WriteLine($"❌ Inner exception: {ex.InnerException?.Message}");
                
                // Fallback to simple shell
                try
                {
                    MainPage = new AppShell();
                    System.Diagnostics.Debug.WriteLine("✅ Fallback AppShell created successfully");
                }
                catch (Exception shellEx)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ AppShell fallback failed: {shellEx.Message}");
                    // Ultimate fallback - create a simple content page
                    MainPage = new ContentPage
                    {
                        Content = new Label
                        {
                            Text = "ShuleLink is starting...",
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        }
                    };
                    System.Diagnostics.Debug.WriteLine("✅ Ultimate fallback ContentPage created");
                }
            }
        }

        private void SetupGlobalExceptionHandling()
        {
            System.Diagnostics.Debug.WriteLine("🛡️ Setting up global exception handling...");
            
            // Handle unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                System.Diagnostics.Debug.WriteLine("=== 🚨 UNHANDLED EXCEPTION CAUGHT ===");
                System.Diagnostics.Debug.WriteLine($"🚨 Exception: {ex?.Message}");
                System.Diagnostics.Debug.WriteLine($"🚨 Exception type: {ex?.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"🚨 Stack trace: {ex?.StackTrace}");
                System.Diagnostics.Debug.WriteLine($"🚨 Inner exception: {ex?.InnerException?.Message}");
                System.Diagnostics.Debug.WriteLine($"🚨 Is terminating: {e.IsTerminating}");
                System.Diagnostics.Debug.WriteLine("=== 🚨 END UNHANDLED EXCEPTION ===");
            };

            // Handle task exceptions
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("=== 🚨 UNOBSERVED TASK EXCEPTION CAUGHT ===");
                System.Diagnostics.Debug.WriteLine($"🚨 Exception: {e.Exception?.Message}");
                System.Diagnostics.Debug.WriteLine($"🚨 Exception type: {e.Exception?.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"🚨 Stack trace: {e.Exception?.StackTrace}");
                System.Diagnostics.Debug.WriteLine($"🚨 Inner exception: {e.Exception?.InnerException?.Message}");
                System.Diagnostics.Debug.WriteLine("=== 🚨 END UNOBSERVED TASK EXCEPTION ===");
                e.SetObserved(); // Prevent app termination
            };
            
            System.Diagnostics.Debug.WriteLine("✅ Global exception handling setup complete");
        }

        private async Task NavigateToSplashAsync()
        {
            try
            {
                await Task.Delay(100); // Small delay to ensure shell is ready
                await Shell.Current.GoToAsync("//WelcomePage");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation to splash failed: {ex.Message}");
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            
            // MainPage already set in constructor
        }
    }
}
