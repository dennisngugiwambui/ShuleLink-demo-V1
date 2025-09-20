using ShuleLink.Services;

namespace ShuleLink.Views;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== 🎨 SPLASHPAGE CONSTRUCTOR START ===");
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("✅ SplashPage InitializeComponent completed");
            
            // Ensure spinner is running
            if (LoadingSpinner != null)
            {
                LoadingSpinner.IsRunning = true;
                LoadingSpinner.IsVisible = true;
                System.Diagnostics.Debug.WriteLine("✅ Loading spinner activated");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("❌ LoadingSpinner not found!");
            }
            
            StartSplashSequence();
            System.Diagnostics.Debug.WriteLine("=== ✅ SPLASHPAGE CONSTRUCTOR COMPLETED ===");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("=== ❌ SPLASHPAGE CONSTRUCTOR ERROR ===");
            System.Diagnostics.Debug.WriteLine($"❌ SplashPage constructor error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"❌ Stack trace: {ex.StackTrace}");
            
            // If splash page fails, navigate directly to welcome
            _ = Task.Run(async () =>
            {
                await Task.Delay(500);
                Application.Current.MainPage = new AppShell();
                await Task.Delay(100);
                await Shell.Current.GoToAsync("//WelcomePage");
            });
        }
    }

    private async void StartSplashSequence()
    {
        try
        {
            // Animate progress dots
            _ = AnimateProgressDots();
            
            await InitializeAppAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Splash initialization error: {ex.Message}");
            // Navigate to welcome page on any error
            try
            {
                if (Application.Current.MainPage is not AppShell)
                {
                    Application.Current.MainPage = new AppShell();
                    await Task.Delay(100);
                }
                await Shell.Current.GoToAsync("//WelcomePage");
            }
            catch (Exception navEx)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {navEx.Message}");
                // Fallback: directly set welcome page
                Application.Current.MainPage = new Views.WelcomePage();
            }
        }
    }

    private async Task InitializeAppAsync()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== 🚀 SPLASH INITIALIZATION START ===");
            
            // Show splash for 2 seconds to reduce memory pressure
            await Task.Delay(2000);
            
            // Force garbage collection to free memory before navigation
            System.Diagnostics.Debug.WriteLine("🧹 Running garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            System.Diagnostics.Debug.WriteLine("🔄 Transitioning to AppShell...");
            
            // Transition to AppShell first
            Application.Current.MainPage = new AppShell();
            await Task.Delay(500); // Give time for shell to initialize
            
            System.Diagnostics.Debug.WriteLine("✅ AppShell set as MainPage");
            
            // Check authentication status (using simple preferences check)
            var isLoggedIn = Preferences.Get("IsLoggedIn", false);
            var userId = Preferences.Get("UserId", 0);
            var userName = Preferences.Get("UserName", "");
            
            System.Diagnostics.Debug.WriteLine($"📋 Auth check: isLoggedIn={isLoggedIn}, userId={userId}, userName={userName}");
            
            // Simplified navigation - avoid database calls that might crash
            if (isLoggedIn && userId > 0 && !string.IsNullOrEmpty(userName))
            {
                // Check session expiry without database
                var sessionTime = Preferences.Get("SessionTime", DateTime.MinValue);
                var sessionExpiry = sessionTime.AddHours(24); // 24 hour session
                
                if (sessionTime == DateTime.MinValue || DateTime.Now > sessionExpiry)
                {
                    // Session expired or invalid - clear and redirect to welcome
                    System.Diagnostics.Debug.WriteLine("⏰ Session expired or invalid");
                    Preferences.Clear();
                    await Shell.Current.GoToAsync("//WelcomePage");
                }
                else
                {
                    // Valid session - proceed to main app
                    System.Diagnostics.Debug.WriteLine("🏠 Valid session found, navigating to MainTabs");
                    try
                    {
                        await Shell.Current.GoToAsync("//MainTabs");
                        System.Diagnostics.Debug.WriteLine("✅ Navigation to MainTabs successful!");
                    }
                    catch (Exception navEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Navigation to MainTabs failed: {navEx.Message}");
                        System.Diagnostics.Debug.WriteLine($"❌ Stack trace: {navEx.StackTrace}");
                        await Shell.Current.GoToAsync("//WelcomePage");
                    }
                }
            }
            else
            {
                // No valid session - go to welcome
                System.Diagnostics.Debug.WriteLine("👋 No valid session, going to WelcomePage");
                await Shell.Current.GoToAsync("//WelcomePage");
            }
            
            System.Diagnostics.Debug.WriteLine("=== ✅ SPLASH INITIALIZATION COMPLETED ===");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("=== ❌ SPLASH INITIALIZATION ERROR ===");
            System.Diagnostics.Debug.WriteLine($"❌ Splash initialization error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"❌ Stack trace: {ex.StackTrace}");
            
            // Navigate to welcome page on any error
            try
            {
                if (Application.Current.MainPage is not AppShell)
                {
                    System.Diagnostics.Debug.WriteLine("🔄 Creating AppShell as fallback...");
                    Application.Current.MainPage = new AppShell();
                    await Task.Delay(100);
                }
                await Shell.Current.GoToAsync("//WelcomePage");
                System.Diagnostics.Debug.WriteLine("✅ Fallback navigation to WelcomePage successful");
            }
            catch (Exception navEx)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Fallback navigation error: {navEx.Message}");
                // Ultimate fallback: directly set welcome page
                Application.Current.MainPage = new Views.WelcomePage();
                System.Diagnostics.Debug.WriteLine("🚨 Ultimate fallback: Direct WelcomePage set");
            }
        }
    }

    private async Task AnimateProgressDots()
    {
        try
        {
            // Check if dots exist before animating
            if (Dot1 == null || Dot2 == null || Dot3 == null)
            {
                System.Diagnostics.Debug.WriteLine("Progress dots not found, skipping animation");
                return;
            }

            while (true)
            {
                // Animate dot 1
                await Dot1.FadeTo(1, 300);
                await Dot2.FadeTo(0.5, 300);
                await Dot3.FadeTo(0.5, 300);
                await Task.Delay(400);

                // Animate dot 2
                await Dot1.FadeTo(0.5, 300);
                await Dot2.FadeTo(1, 300);
                await Dot3.FadeTo(0.5, 300);
                await Task.Delay(400);

                // Animate dot 3
                await Dot1.FadeTo(0.5, 300);
                await Dot2.FadeTo(0.5, 300);
                await Dot3.FadeTo(1, 300);
                await Task.Delay(400);

                // Reset
                await Dot1.FadeTo(0.5, 300);
                await Dot2.FadeTo(0.5, 300);
                await Dot3.FadeTo(0.5, 300);
                await Task.Delay(200);
            }
        }
        catch (Exception ex)
        {
            // Animation stopped, likely due to page navigation
            System.Diagnostics.Debug.WriteLine($"Animation error: {ex.Message}");
        }
    }

    protected override bool OnBackButtonPressed()
    {
        // Prevent back button during splash
        return true;
    }
}
