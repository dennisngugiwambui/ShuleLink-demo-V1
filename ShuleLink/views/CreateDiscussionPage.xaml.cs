using ShuleLink.Models;
using ShuleLink.Services;

namespace ShuleLink.Views;

public partial class CreateDiscussionPage : ContentPage
{
    private readonly DiscussionService _discussionService;
    private User? _currentUser;

    public CreateDiscussionPage()
    {
        InitializeComponent();
        _discussionService = new DiscussionService();
        LoadUserData();
    }

    private async void LoadUserData()
    {
        try
        {
            var userId = Preferences.Get("UserId", 0);
            if (userId > 0)
            {
                var databaseService = new DatabaseService();
                _currentUser = await databaseService.GetUserAsync(userId);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading user data: {ex.Message}");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnPostClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await Services.ToastService.ShowToast("Please enter a discussion title", Services.ToastType.Error);
            return;
        }

        if (string.IsNullOrWhiteSpace(ContentEditor.Text))
        {
            await Services.ToastService.ShowToast("Please enter discussion content", Services.ToastType.Error);
            return;
        }

        if (_currentUser == null)
        {
            await Services.ToastService.ShowToast("User not found. Please log in again.", Services.ToastType.Error);
            return;
        }

        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            PostButton.IsEnabled = false;

            await _discussionService.CreateDiscussionAsync(
                TitleEntry.Text.Trim(),
                ContentEditor.Text.Trim(),
                _currentUser.Name,
                SubjectPicker.SelectedItem?.ToString() ?? "General",
                GradePicker.SelectedItem?.ToString()?.Replace("Grade ", "") ?? "",
                TopicEntry.Text?.Trim() ?? ""
            );
            
            await Services.ToastService.ShowToast("Discussion created successfully!", Services.ToastType.Success);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Services.ToastService.ShowToast($"Error creating discussion: {ex.Message}", Services.ToastType.Error);
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            PostButton.IsEnabled = true;
        }
    }

    private async void OnAttachPhotoClicked(object sender, EventArgs e)
    {
        await Services.ToastService.ShowToast("Photo attachment coming soon!", Services.ToastType.Info);
    }

    private async void OnAttachDocumentClicked(object sender, EventArgs e)
    {
        await Services.ToastService.ShowToast("Document attachment coming soon!", Services.ToastType.Info);
    }

    private async void OnAttachLinkClicked(object sender, EventArgs e)
    {
        var result = await DisplayPromptAsync("Add Link", "Enter URL:", "Add", "Cancel", "https://");
        if (!string.IsNullOrWhiteSpace(result))
        {
            ContentEditor.Text += $"\n\n🔗 {result}";
            await Services.ToastService.ShowToast("Link added to content", Services.ToastType.Success);
        }
    }
}
