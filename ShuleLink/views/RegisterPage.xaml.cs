using ShuleLink.Services;
using ShuleLink.Models;
using System.Text.RegularExpressions;

namespace ShuleLink.Views;

public partial class RegisterPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private bool _isPasswordVisible = false;

    public RegisterPage()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("RegisterPage constructor started...");
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("RegisterPage InitializeComponent completed...");
            
            _databaseService = new DatabaseService();
            System.Diagnostics.Debug.WriteLine("RegisterPage DatabaseService created...");
            
            // Remove automatic navigation to prevent crashes
            System.Diagnostics.Debug.WriteLine("RegisterPage constructor completed - no auto navigation");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"RegisterPage constructor error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }

    public RegisterPage(DatabaseService databaseService)
    {
        try
        {
            InitializeComponent();
            _databaseService = databaseService;
            System.Diagnostics.Debug.WriteLine("RegisterPage constructor (with service) completed");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"RegisterPage constructor (with service) error: {ex.Message}");
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

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        if (!ValidateForm())
            return;

        ShowLoading(true);
        HideError();

        try
        {
            // Check if user already exists
            var existingUser = await _databaseService.GetUserByPhoneAsync(PhoneEntry.Text.Trim());
            if (existingUser != null)
            {
                await ShowError("A user with this phone number already exists.");
                return;
            }

            // Create new user with simplified form
            var newUser = new User
            {
                Name = NameEntry.Text.Trim(),
                PhoneNumber = PhoneEntry.Text.Trim(),
                Password = PasswordEntry.Text, // Will be hashed by DatabaseService
                AdmissionNumber = $"ADM{DateTime.Now.Year}{new Random().Next(1000, 9999)}", // Auto-generate
                Grade = GradeEntry.Text.Trim(),
                Class = ClassEntry.Text.Trim(),
                ParentName = "Parent/Guardian", // Default value
                HomeAddress = "Not specified", // Default value
                Status = "active",
                UserType = "student",
                Graduated = false
            };

            var userId = await _databaseService.SaveUserAsync(newUser);
            newUser.Id = userId;

            // Show success toast
            await ToastService.ShowToast($"✅ Account created successfully! Welcome {newUser.Name}!", ToastType.Success, 3000);
            
            // Store credentials for auto-login on login page
            Preferences.Set("RegisteredPhone", newUser.PhoneNumber);
            Preferences.Set("RegisteredPassword", PasswordEntry.Text);
            Preferences.Set("JustRegistered", true);
            
            // Navigate to login page with credentials
            await Task.Delay(1000); // Allow toast to show
            await Shell.Current.GoToAsync("//LoginPage");
        }
        catch (Exception ex)
        {
            await ShowError($"Registration failed: {ex.Message}");
        }
        finally
        {
            ShowLoading(false);
        }
    }

    private async void OnSignInClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }

    private bool ValidateForm()
    {
        // Name validation
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            _ = ShowError("Please enter your full name.");
            return false;
        }

        // Phone validation
        if (string.IsNullOrWhiteSpace(PhoneEntry.Text))
        {
            _ = ShowError("Please enter your phone number.");
            return false;
        }

        if (!IsValidPhoneNumber(PhoneEntry.Text.Trim()))
        {
            _ = ShowError("Please enter a valid Kenyan phone number (254XXXXXXXXX).");
            return false;
        }

        // Password validation
        if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            _ = ShowError("Please enter a password.");
            return false;
        }

        if (PasswordEntry.Text.Length < 6)
        {
            _ = ShowError("Password must be at least 6 characters long.");
            return false;
        }

        // Grade validation
        if (string.IsNullOrWhiteSpace(GradeEntry.Text))
        {
            _ = ShowError("Please enter your grade (1-8).");
            return false;
        }

        // Class validation
        if (string.IsNullOrWhiteSpace(ClassEntry.Text))
        {
            _ = ShowError("Please enter your class (e.g., East, West).");
            return false;
        }

        return true;
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        // Kenyan phone number format: 254XXXXXXXXX (12 digits total)
        var phoneRegex = new Regex(@"^254[0-9]{9}$");
        return phoneRegex.IsMatch(phoneNumber);
    }

    private void ShowLoading(bool isLoading)
    {
        LoadingIndicator.IsVisible = isLoading;
        LoadingIndicator.IsRunning = isLoading;
        RegisterBtn.IsEnabled = !isLoading;
        RegisterBtn.Text = isLoading ? "Creating Account..." : "🚀 Create Account";
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
        
        // Clear form on each appearance for security
        NameEntry.Text = string.Empty;
        PhoneEntry.Text = string.Empty;
        PasswordEntry.Text = string.Empty;
        GradeEntry.Text = string.Empty;
        ClassEntry.Text = string.Empty;
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
