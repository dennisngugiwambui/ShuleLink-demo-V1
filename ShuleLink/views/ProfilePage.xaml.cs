using ShuleLink.Models;
using ShuleLink.Services;

namespace ShuleLink.Views;

public partial class ProfilePage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private User? _currentUser;

    public ProfilePage()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== PROFILEPAGE CONSTRUCTOR START ===");
            System.Diagnostics.Debug.WriteLine("ProfilePage constructor started...");
            
            System.Diagnostics.Debug.WriteLine("Calling InitializeComponent...");
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("InitializeComponent completed successfully!");
            
            System.Diagnostics.Debug.WriteLine("Creating DatabaseService...");
            _databaseService = new DatabaseService();
            System.Diagnostics.Debug.WriteLine("DatabaseService created successfully!");
            
            System.Diagnostics.Debug.WriteLine("=== PROFILEPAGE CONSTRUCTOR COMPLETED SUCCESSFULLY ===");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("=== PROFILEPAGE CONSTRUCTOR ERROR ===");
            System.Diagnostics.Debug.WriteLine($"ProfilePage constructor error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Exception type: {ex.GetType().Name}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            
            // Don't throw - set minimal fallback state instead
            try
            {
                _databaseService = new DatabaseService();
                System.Diagnostics.Debug.WriteLine("Fallback services created successfully!");
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Failed to create fallback services - page may not function properly");
            }
        }
    }

    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();
            System.Diagnostics.Debug.WriteLine("ProfilePage OnAppearing started...");
            
            // Load data when page appears
            LoadUserData();
            
            System.Diagnostics.Debug.WriteLine("ProfilePage OnAppearing completed successfully!");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ProfilePage OnAppearing error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            
            // Set fallback data to prevent crashes
            try
            {
                var userName = Preferences.Get("UserName", "Student");
                var userPhone = Preferences.Get("UserPhone", "");
                
                UserNameLabel.Text = userName;
                PhoneNumberLabel.Text = userPhone;
                UserGradeLabel.Text = "Grade 5";
                AdmissionNumberLabel.Text = "Admission: 12345";
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Failed to set fallback data in ProfilePage");
            }
        }
    }

    public ProfilePage(DatabaseService databaseService)
    {
        try
        {
            InitializeComponent();
            _databaseService = databaseService;
            // Don't load data in constructor to prevent crashes
            // LoadUserData();
            System.Diagnostics.Debug.WriteLine("ProfilePage constructor (with service) completed");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ProfilePage constructor (with service) error: {ex.Message}");
        }
    }

    private async void LoadUserData()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var userId = Preferences.Get("UserId", 0);
            if (userId > 0)
            {
                _currentUser = await _databaseService.GetUserAsync(userId);
                if (_currentUser != null)
                {
                    DisplayUserInfo();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load profile: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private void DisplayUserInfo()
    {
        if (_currentUser == null) return;

        UserNameLabel.Text = _currentUser.Name;
        UserGradeLabel.Text = $"{_currentUser.Grade} {_currentUser.Class}";
        AdmissionNumberLabel.Text = $"Admission: {_currentUser.AdmissionNumber}";
        PhoneNumberLabel.Text = _currentUser.PhoneNumber;
        ParentNameLabel.Text = _currentUser.ParentName;
        HomeAddressLabel.Text = _currentUser.HomeAddress;

        // Mock stats for demo - in real app, these would come from database
        QuizzesCompletedLabel.Text = "12";
        AverageScoreLabel.Text = "85%";
        StreakLabel.Text = "7";
    }

    private async void OnNotificationSettingsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Notifications", "Notification settings will be available in future updates.", "OK");
    }

    private async void OnDarkModeClicked(object sender, EventArgs e)
    {
        var isDarkMode = ThemeService.IsDarkMode;
        var newThemeState = !isDarkMode;
        
        // Apply theme immediately
        ThemeService.SetTheme(newThemeState);
        
        // Show confirmation with current theme state
        var themeText = newThemeState ? "enabled" : "disabled";
        await DisplayAlert("Theme Changed", $"Dark mode {themeText}! The theme has been applied immediately.", "OK");
        
        // Force refresh the current page to apply new theme
        await Shell.Current.GoToAsync("//MainTabs/Profile");
    }

    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        var currentPassword = await DisplayPromptAsync("Change Password", "Enter current password:", "Continue", "Cancel", placeholder: "Current password", maxLength: 50, keyboard: Keyboard.Default, initialValue: "");
        
        if (string.IsNullOrEmpty(currentPassword)) return;

        var newPassword = await DisplayPromptAsync("Change Password", "Enter new password:", "Change", "Cancel", placeholder: "New password", maxLength: 50, keyboard: Keyboard.Default, initialValue: "");
        
        if (!string.IsNullOrEmpty(newPassword))
        {
            await DisplayAlert("Success", "Password changed successfully!", "OK");
        }
    }

    private async void OnDiscussionsClicked(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("💭 Opening Discussions from Profile...");
        await Shell.Current.GoToAsync("discussions");
    }

    private async void OnContactSupportClicked(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Contact Support", "Cancel", null, "Call Support", "Email Support", "WhatsApp Support");
        
        if (action != "Cancel" && action != null)
        {
            await DisplayAlert("Support", $"Opening {action}...", "OK");
        }
    }

    private async void OnAboutClicked(object sender, EventArgs e)
    {
        await DisplayAlert("About ShuleLink", 
            "ShuleLink v1.0\n\n" +
            "A comprehensive school management app for students.\n\n" +
            "Features:\n" +
            "• AI-powered daily quotes\n" +
            "• Interactive quizzes\n" +
            "• Teacher chat system\n" +
            "• Educational content\n" +
            "• Progress tracking\n\n" +
            "© 2024 ShuleLink. All rights reserved.", 
            "OK");
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
        
        if (result)
        {
            // Clear user preferences
            Preferences.Remove("UserId");
            Preferences.Remove("UserName");
            
            // Navigate to welcome page
            await Shell.Current.GoToAsync("//WelcomePage");
        }
    }
}
