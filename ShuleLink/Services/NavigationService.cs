namespace ShuleLink.Services
{
    public class NavigationService
    {
        public static bool IsUserLoggedIn()
        {
            var isLoggedIn = Preferences.Get("IsLoggedIn", false);
            var userId = Preferences.Get("UserId", 0);
            var userName = Preferences.Get("UserName", "");
            
            // Validate that we have complete session data
            return isLoggedIn && userId > 0 && !string.IsNullOrEmpty(userName);
        }

        public static async Task<bool> ValidateUserSession()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("NavigationService.ValidateUserSession called");
                
                // Simplified validation - only check preferences to avoid database crashes
                var isLoggedIn = IsUserLoggedIn();
                System.Diagnostics.Debug.WriteLine($"IsUserLoggedIn: {isLoggedIn}");
                
                if (!isLoggedIn) return false;

                var userId = Preferences.Get("UserId", 0);
                var sessionTime = Preferences.Get("SessionTime", DateTime.MinValue);
                
                if (userId <= 0 || sessionTime == DateTime.MinValue) 
                {
                    System.Diagnostics.Debug.WriteLine("Invalid session data");
                    return false;
                }

                // Check session expiry (24 hours)
                var sessionExpiry = sessionTime.AddHours(24);
                if (DateTime.Now > sessionExpiry)
                {
                    System.Diagnostics.Debug.WriteLine("Session expired");
                    Preferences.Clear();
                    return false;
                }

                System.Diagnostics.Debug.WriteLine("Session valid");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ValidateUserSession error: {ex.Message}");
                return false;
            }
        }

        public static async Task NavigateToAuthenticatedArea()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("NavigateToAuthenticatedArea called");
                
                if (IsUserLoggedIn())
                {
                    System.Diagnostics.Debug.WriteLine("User logged in - navigating to MainTabs");
                    await Shell.Current.GoToAsync("//MainTabs");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("User not logged in - navigating to WelcomePage");
                    await Shell.Current.GoToAsync("//WelcomePage");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"NavigateToAuthenticatedArea error: {ex.Message}");
                // Fallback navigation
                try
                {
                    await Shell.Current.GoToAsync("//WelcomePage");
                }
                catch (Exception fallbackEx)
                {
                    System.Diagnostics.Debug.WriteLine($"Fallback navigation error: {fallbackEx.Message}");
                }
            }
        }

        public static async Task NavigateToUnauthenticatedArea()
        {
            await Shell.Current.GoToAsync("//WelcomePage");
        }

        public static async Task LogoutAndNavigate()
        {
            // Clear all user session data
            Preferences.Clear();
            
            // Navigate to welcome page and clear navigation stack
            await Shell.Current.GoToAsync("//WelcomePage");
        }

        public static bool CanNavigateBack(string currentRoute)
        {
            var isLoggedIn = IsUserLoggedIn();
            
            // Define routes that don't allow back navigation
            var noBackRoutes = new[] { "WelcomePage", "LoginPage", "RegisterPage" };
            
            if (!isLoggedIn)
            {
                // Unauthenticated users can only navigate within auth pages
                return noBackRoutes.Contains(currentRoute);
            }
            else
            {
                // Authenticated users cannot go back to auth pages
                return !noBackRoutes.Contains(currentRoute);
            }
        }

        public static async Task<bool> HandleBackNavigation(string currentRoute)
        {
            var isLoggedIn = IsUserLoggedIn();
            
            if (!isLoggedIn)
            {
                // For unauthenticated users, prevent going back from auth pages
                if (currentRoute == "WelcomePage")
                {
                    // Exit app or stay on welcome page
                    return false; // Don't allow back navigation
                }
                else if (currentRoute == "LoginPage" || currentRoute == "RegisterPage")
                {
                    await Shell.Current.GoToAsync("//WelcomePage");
                    return true; // We handled the navigation
                }
            }
            else
            {
                // For authenticated users, prevent going back to auth pages
                var authPages = new[] { "WelcomePage", "LoginPage", "RegisterPage" };
                if (authPages.Contains(currentRoute))
                {
                    await Shell.Current.GoToAsync("//MainTabs");
                    return true; // We handled the navigation
                }
            }
            
            return false; // Allow default back navigation
        }
    }
}
