using ShuleLink.Models;
using ShuleLink.Services;
using System.Collections.ObjectModel;

namespace ShuleLink.Views;

public partial class ChatListPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private User? _currentUser;
    private Teacher? _classTeacher;
    private List<Teacher> _subjectTeachers = new();

    public ChatListPage()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== CHATLISTPAGE CONSTRUCTOR START ===");
            System.Diagnostics.Debug.WriteLine("ChatListPage constructor started...");
            
            System.Diagnostics.Debug.WriteLine("Calling InitializeComponent...");
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("InitializeComponent completed successfully!");
            
            System.Diagnostics.Debug.WriteLine("Creating DatabaseService...");
            _databaseService = new DatabaseService();
            System.Diagnostics.Debug.WriteLine("DatabaseService created successfully!");
            
            System.Diagnostics.Debug.WriteLine("=== CHATLISTPAGE CONSTRUCTOR COMPLETED SUCCESSFULLY ===");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("=== CHATLISTPAGE CONSTRUCTOR ERROR ===");
            System.Diagnostics.Debug.WriteLine($"ChatListPage constructor error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Exception type: {ex.GetType().Name}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            throw; // Re-throw to see the actual error
        }
    }

    public ChatListPage(DatabaseService databaseService)
    {
        try
        {
            InitializeComponent();
            _databaseService = databaseService;
            // Don't load data in constructor to prevent crashes
            // LoadData();
            System.Diagnostics.Debug.WriteLine("ChatListPage constructor (with service) completed");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ChatListPage constructor (with service) error: {ex.Message}");
        }
    }

    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();
            System.Diagnostics.Debug.WriteLine("ChatListPage OnAppearing started...");
            
            // Load data when page appears
            LoadData();
            
            System.Diagnostics.Debug.WriteLine("ChatListPage OnAppearing completed successfully!");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ChatListPage OnAppearing error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            
            // Set fallback data to prevent crashes
            try
            {
                ClassTeacherName.Text = "Loading...";
                ClassTeacherSubject.Text = "Please wait";
                TeachersCollectionView.ItemsSource = new List<Teacher>();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Failed to set fallback data in ChatListPage");
            }
        }
    }

    private async void LoadData()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            // Get current user from preferences
            var userId = Preferences.Get("UserId", 0);
            if (userId > 0)
            {
                _currentUser = await _databaseService.GetUserAsync(userId);
                if (_currentUser != null)
                {
                    await LoadClassTeacher();
                    await LoadSubjectTeachers();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load teachers: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private async Task LoadClassTeacher()
    {
        if (_currentUser == null) return;

        _classTeacher = await _databaseService.GetClassTeacherAsync(_currentUser.Grade);
        if (_classTeacher != null)
        {
            ClassTeacherName.Text = _classTeacher.Name;
            ClassTeacherSubject.Text = $"{_classTeacher.Subject} • {_classTeacher.ClassAssigned}";
            
            // Update online status
            if (_classTeacher.IsOnline)
            {
                ClassTeacherStatus.Fill = Color.FromArgb("#27AE60");
            }
            else
            {
                ClassTeacherStatus.Fill = Color.FromArgb("#95A5A6");
            }
        }
        else
        {
            ClassTeacherName.Text = "No Class Teacher Assigned";
            ClassTeacherSubject.Text = "Contact administration";
        }
    }

    private async Task LoadSubjectTeachers()
    {
        if (_currentUser == null) return;

        _subjectTeachers = await _databaseService.GetSubjectTeachersAsync(_currentUser.Grade);
        TeachersCollectionView.ItemsSource = _subjectTeachers;
    }

    private async void OnClassTeacherTapped(object sender, EventArgs e)
    {
        if (_classTeacher != null && _currentUser != null)
        {
            await Shell.Current.GoToAsync($"ChatPage?teacherId={_classTeacher.Id}&teacherName={_classTeacher.Name}");
        }
    }

    private async void OnAIAssistantTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"ChatPage?teacherId=AI&teacherName=AI Learning Assistant");
    }

    private async void OnTeacherTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Teacher teacher)
        {
            await Shell.Current.GoToAsync($"ChatPage?teacherId={teacher.Id}&teacherName={Uri.EscapeDataString(teacher.Name)}");
        }
    }

    private async void OnTeacherChatButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Teacher teacher)
        {
            await Shell.Current.GoToAsync($"ChatPage?teacherId={teacher.Id}&teacherName={Uri.EscapeDataString(teacher.Name)}");
        }
    }

    private async void OnEmergencyContactClicked(object sender, EventArgs e)
    {
        var result = await DisplayActionSheet("Emergency Contact", "Cancel", null, 
            "Call Head Teacher", "Call Deputy Head", "Call School Office");
        
        if (result != "Cancel" && result != null)
        {
            await DisplayAlert("Emergency", $"Calling {result}...", "OK");
            // In a real app, you would make the actual phone call
        }
    }

    private async void OnReportIssueClicked(object sender, EventArgs e)
    {
        var issue = await DisplayPromptAsync("Report Issue", "Describe the issue:", 
            "Submit", "Cancel", "Type your issue here...");
        
        if (!string.IsNullOrEmpty(issue))
        {
            await DisplayAlert("Issue Reported", "Your issue has been reported to the administration.", "OK");
            // In a real app, you would save this to the database
        }
    }

    private async void OnAskForHelpClicked(object sender, EventArgs e)
    {
        var subjects = new[] { "Mathematics", "English", "Science", "Social Studies", "Other" };
        var subject = await DisplayActionSheet("Which subject do you need help with?", "Cancel", null, subjects);
        
        if (subject != "Cancel" && subject != null)
        {
            await DisplayAlert("Help Request", $"A help request for {subject} has been sent to your teachers.", "OK");
        }
    }

    private async void OnHomeworkHelpClicked(object sender, EventArgs e)
    {
        var homework = await DisplayPromptAsync("Homework Help", "What homework do you need help with?", 
            "Send", "Cancel", "Describe your homework...");
        
        if (!string.IsNullOrEmpty(homework))
        {
            await DisplayAlert("Homework Help", "Your homework help request has been sent to your class teacher.", "OK");
        }
    }

    private async void OnDiscussionsClicked(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("💭 Opening Discussions...");
        await Shell.Current.GoToAsync("discussions");
    }
}
