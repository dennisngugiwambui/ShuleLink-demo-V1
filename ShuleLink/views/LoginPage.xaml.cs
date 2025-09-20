using ShuleLink.Services;

namespace ShuleLink.Views;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private bool _isPasswordVisible = false;

    public LoginPage()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("LoginPage constructor started...");
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("LoginPage InitializeComponent completed...");
            
            _databaseService = new DatabaseService();
            System.Diagnostics.Debug.WriteLine("LoginPage DatabaseService created...");
            
            // Remove automatic navigation to prevent crashes
            // User must manually login
            System.Diagnostics.Debug.WriteLine("LoginPage constructor completed - no auto navigation");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoginPage constructor error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }

    public LoginPage(DatabaseService databaseService)
    {
        try
        {
            InitializeComponent();
            _databaseService = databaseService;
            System.Diagnostics.Debug.WriteLine("LoginPage constructor (with service) completed");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoginPage constructor (with service) error: {ex.Message}");
        }
    }

    private async Task NavigateToMainApp()
    {
        await Task.Delay(100);
        await NavigationService.NavigateToAuthenticatedArea();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await NavigationService.NavigateToUnauthenticatedArea();
    }

    private void OnTogglePasswordClicked(object sender, EventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;
        PasswordEntry.IsPassword = !_isPasswordVisible;
        TogglePasswordBtn.Text = _isPasswordVisible ? "🙈" : "👁️";
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PhoneEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await ShowError("Please enter both phone number and password.");
            return;
        }

        ShowLoading(true);
        HideError();

        try
        {
            var validationResult = await _databaseService.ValidateUserAsync(PhoneEntry.Text.Trim(), PasswordEntry.Text);
            
            if (validationResult.IsValid && validationResult.User != null)
            {
                // Store user session with timestamp
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("UserId", validationResult.User.Id);
                Preferences.Set("UserName", validationResult.User.Name);
                Preferences.Set("UserPhone", validationResult.User.PhoneNumber);
                Preferences.Set("SessionTime", DateTime.Now);
                
                // NO TOAST - Direct navigation to prevent app exit
                System.Diagnostics.Debug.WriteLine($"Login successful for user: {validationResult.User.Name}");
                
                // COMPREHENSIVE DEBUGGING FOR LOGIN CRASH
                try
                {
                    System.Diagnostics.Debug.WriteLine("=== COMPREHENSIVE LOGIN NAVIGATION DEBUG ===");
                    System.Diagnostics.Debug.WriteLine($"Current time: {DateTime.Now}");
                    System.Diagnostics.Debug.WriteLine($"User ID: {validationResult.User.Id}");
                    System.Diagnostics.Debug.WriteLine($"User Name: {validationResult.User.Name}");
                    
                    // Check Shell state before navigation
                    System.Diagnostics.Debug.WriteLine($"Shell.Current is null: {Shell.Current == null}");
                    if (Shell.Current != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Current route: {Shell.Current.CurrentState?.Location?.OriginalString}");
                        System.Diagnostics.Debug.WriteLine($"Shell type: {Shell.Current.GetType().Name}");
                    }
                    
                    // Test individual page construction to identify the problematic page
                    System.Diagnostics.Debug.WriteLine("=== TESTING PAGE CONSTRUCTION INDIVIDUALLY ===");
                    
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Testing MainPage construction...");
                        var mainPage = new MainPage();
                        System.Diagnostics.Debug.WriteLine("✅ MainPage construction successful!");
                        mainPage = null; // Release immediately
                    }
                    catch (Exception mainEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ MainPage construction FAILED: {mainEx.Message}");
                        System.Diagnostics.Debug.WriteLine($"MainPage stack trace: {mainEx.StackTrace}");
                        throw new Exception($"MainPage construction failed: {mainEx.Message}", mainEx);
                    }
                    
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Testing ChatListPage construction...");
                        var chatPage = new Views.ChatListPage();
                        System.Diagnostics.Debug.WriteLine("✅ ChatListPage construction successful!");
                        chatPage = null; // Release immediately
                    }
                    catch (Exception chatEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ ChatListPage construction FAILED: {chatEx.Message}");
                        System.Diagnostics.Debug.WriteLine($"ChatListPage stack trace: {chatEx.StackTrace}");
                        throw new Exception($"ChatListPage construction failed: {chatEx.Message}", chatEx);
                    }
                    
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Testing LearningPage construction...");
                        var learningPage = new Views.LearningPage();
                        System.Diagnostics.Debug.WriteLine("✅ LearningPage construction successful!");
                        learningPage = null; // Release immediately
                    }
                    catch (Exception learningEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ LearningPage construction FAILED: {learningEx.Message}");
                        System.Diagnostics.Debug.WriteLine($"LearningPage stack trace: {learningEx.StackTrace}");
                        throw new Exception($"LearningPage construction failed: {learningEx.Message}", learningEx);
                    }
                    
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Testing QuizPage construction...");
                        var quizPage = new Views.QuizPage();
                        System.Diagnostics.Debug.WriteLine("✅ QuizPage construction successful!");
                        quizPage = null; // Release immediately
                    }
                    catch (Exception quizEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ QuizPage construction FAILED: {quizEx.Message}");
                        System.Diagnostics.Debug.WriteLine($"QuizPage stack trace: {quizEx.StackTrace}");
                        throw new Exception($"QuizPage construction failed: {quizEx.Message}", quizEx);
                    }
                    
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Testing ProfilePage construction...");
                        var profilePage = new Views.ProfilePage();
                        System.Diagnostics.Debug.WriteLine("✅ ProfilePage construction successful!");
                        profilePage = null; // Release immediately
                    }
                    catch (Exception profileEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ ProfilePage construction FAILED: {profileEx.Message}");
                        System.Diagnostics.Debug.WriteLine($"ProfilePage stack trace: {profileEx.StackTrace}");
                        throw new Exception($"ProfilePage construction failed: {profileEx.Message}", profileEx);
                    }
                    
                    System.Diagnostics.Debug.WriteLine("✅ ALL PAGE CONSTRUCTIONS SUCCESSFUL!");
                    
                    // Now attempt navigation
                    System.Diagnostics.Debug.WriteLine("=== ATTEMPTING NAVIGATION ===");
                    System.Diagnostics.Debug.WriteLine("Calling Shell.Current.GoToAsync('//MainTabs')...");
                    
                    await Shell.Current.GoToAsync("//MainTabs");
                    
                    System.Diagnostics.Debug.WriteLine("✅ Navigation call completed!");
                    
                    // Wait and check if we actually navigated
                    await Task.Delay(1000);
                    System.Diagnostics.Debug.WriteLine($"Post-navigation route: {Shell.Current.CurrentState?.Location?.OriginalString}");
                    System.Diagnostics.Debug.WriteLine("=== LOGIN NAVIGATION COMPLETED SUCCESSFULLY ===");
                }
                catch (Exception navEx)
                {
                    System.Diagnostics.Debug.WriteLine("=== CRITICAL NAVIGATION ERROR ===");
                    System.Diagnostics.Debug.WriteLine($"❌ Navigation failed: {navEx.Message}");
                    System.Diagnostics.Debug.WriteLine($"Exception type: {navEx.GetType().Name}");
                    System.Diagnostics.Debug.WriteLine($"Stack trace: {navEx.StackTrace}");
                    System.Diagnostics.Debug.WriteLine($"Inner exception: {navEx.InnerException?.Message}");
                    System.Diagnostics.Debug.WriteLine($"Inner stack trace: {navEx.InnerException?.StackTrace}");
                    
                    // Show detailed error to user
                    await DisplayAlert("Navigation Error Details", 
                        $"Navigation failed with error:\n\n{navEx.Message}\n\nType: {navEx.GetType().Name}\n\nThis error has been logged for debugging.", 
                        "OK");
                }
            }
            else
            {
                // NO TOAST - Direct error display to prevent app exit
                System.Diagnostics.Debug.WriteLine($"Login failed: {validationResult.Message}");
                await ShowError(validationResult.Message);
            }
        }
        catch (Exception ex)
        {
            // NO TOAST - Direct error display to prevent app exit
            System.Diagnostics.Debug.WriteLine($"Login exception: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            await ShowError($"Login failed: {ex.Message}");
        }
        finally
        {
            ShowLoading(false);
        }
    }

    private async void OnForgotPasswordClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Forgot Password", "Please contact your school administrator to reset your password.", "OK");
    }

    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//RegisterPage");
    }

    private void ShowLoading(bool isLoading)
    {
        LoadingIndicator.IsVisible = isLoading;
        LoadingIndicator.IsRunning = isLoading;
        LoginBtn.IsEnabled = !isLoading;
        LoginBtn.Text = isLoading ? "Signing In..." : "Sign In";
    }

    private async Task ShowError(string message)
    {
        ErrorLabel.Text = message;
        ErrorLabel.IsVisible = true;
        
        // Auto-hide error after 5 seconds
        await Task.Delay(5000);
        HideError();
    }

    private void HideError()
    {
        ErrorLabel.IsVisible = false;
        ErrorLabel.Text = string.Empty;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Check if user just registered and auto-fill credentials
        if (Preferences.Get("JustRegistered", false))
        {
            PhoneEntry.Text = Preferences.Get("RegisteredPhone", "");
            PasswordEntry.Text = Preferences.Get("RegisteredPassword", "");
            
            // Clear the registration flags
            Preferences.Remove("JustRegistered");
            Preferences.Remove("RegisteredPhone");
            Preferences.Remove("RegisteredPassword");
            
            // Show welcome toast
            _ = ToastService.ShowToast("🎉 Ready to sign in with your new account!", ToastType.Info, 2000);
        }
        else
        {
            // Clear form on each appearance for security
            PhoneEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
        }
        
        HideError();
    }

    protected override bool OnBackButtonPressed()
    {
        // Navigate back to welcome page
        Task.Run(async () =>
        {
            await NavigationService.NavigateToUnauthenticatedArea();
        });
        return true;
    }
}
