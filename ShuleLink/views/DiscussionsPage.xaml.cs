using ShuleLink.Services;
using ShuleLink.ViewModels;
using System.Collections.ObjectModel;

namespace ShuleLink.Views;

public partial class DiscussionsPage : ContentPage
{
    private readonly DiscussionService _discussionService;
    private ObservableCollection<DiscussionViewModel> _discussions = new();
    private string _currentSubjectFilter = "All";
    private string _currentGradeFilter = "All";
    private string _currentSortBy = "Recent";

    public DiscussionsPage()
    {
        InitializeComponent();
        _discussionService = new DiscussionService();
        DiscussionsCollectionView.ItemsSource = _discussions;
        
        // Set default sort selection
        SortPicker.SelectedIndex = 0; // Recent
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadFiltersAsync();
        await LoadDiscussionsAsync();
    }

    private async Task LoadFiltersAsync()
    {
        try
        {
            // Load subjects
            var subjects = await _discussionService.GetSubjectsAsync();
            SubjectPicker.ItemsSource = subjects;
            SubjectPicker.SelectedIndex = 0; // "All"

            // Load grades
            var grades = await _discussionService.GetGradesAsync();
            GradePicker.ItemsSource = grades;
            GradePicker.SelectedIndex = 0; // "All"
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load filters: {ex.Message}", "OK");
        }
    }

    private async Task LoadDiscussionsAsync()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var discussions = await _discussionService.GetDiscussionsAsync(
                _currentSubjectFilter, 
                _currentGradeFilter, 
                _currentSortBy.ToLower());

            _discussions.Clear();
            foreach (var discussion in discussions)
            {
                _discussions.Add(discussion);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load discussions: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            RefreshView.IsRefreshing = false;
        }
    }

    private async void OnFilterChanged(object sender, EventArgs e)
    {
        if (sender == SubjectPicker && SubjectPicker.SelectedItem != null)
        {
            _currentSubjectFilter = SubjectPicker.SelectedItem.ToString() ?? "All";
        }
        else if (sender == GradePicker && GradePicker.SelectedItem != null)
        {
            _currentGradeFilter = GradePicker.SelectedItem.ToString() ?? "All";
        }
        else if (sender == SortPicker && SortPicker.SelectedItem != null)
        {
            _currentSortBy = SortPicker.SelectedItem.ToString() ?? "Recent";
        }

        await LoadDiscussionsAsync();
    }

    private async void OnSearchPressed(object sender, EventArgs e)
    {
        var searchTerm = SearchBar.Text?.Trim();
        if (string.IsNullOrEmpty(searchTerm))
        {
            await LoadDiscussionsAsync();
            return;
        }

        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var allDiscussions = await _discussionService.GetDiscussionsAsync(
                _currentSubjectFilter, 
                _currentGradeFilter, 
                _currentSortBy.ToLower());

            var filteredDiscussions = allDiscussions.Where(d => 
                d.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                d.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                d.Topic.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            _discussions.Clear();
            foreach (var discussion in filteredDiscussions)
            {
                _discussions.Add(discussion);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Search failed: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private async void OnRefreshing(object sender, EventArgs e)
    {
        await LoadDiscussionsAsync();
    }

    private async void OnDiscussionSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is DiscussionViewModel selectedDiscussion)
        {
            // Navigate to discussion detail page
            await Shell.Current.GoToAsync($"discussiondetail?discussionId={selectedDiscussion.Id}");
            
            // Clear selection
            DiscussionsCollectionView.SelectedItem = null;
        }
    }

    private async void OnUpvoteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int discussionId)
        {
            try
            {
                await _discussionService.VoteDiscussionAsync(discussionId, Models.VoteType.Upvote);
                
                // Update the discussion in the collection
                var discussion = _discussions.FirstOrDefault(d => d.Id == discussionId);
                if (discussion != null)
                {
                    if (discussion.HasUserUpvoted)
                    {
                        discussion.Upvotes--;
                        discussion.HasUserUpvoted = false;
                    }
                    else
                    {
                        if (discussion.HasUserDownvoted)
                        {
                            discussion.Downvotes--;
                            discussion.HasUserDownvoted = false;
                        }
                        discussion.Upvotes++;
                        discussion.HasUserUpvoted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to vote: {ex.Message}", "OK");
            }
        }
    }

    private async void OnDownvoteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int discussionId)
        {
            try
            {
                await _discussionService.VoteDiscussionAsync(discussionId, Models.VoteType.Downvote);
                
                // Update the discussion in the collection
                var discussion = _discussions.FirstOrDefault(d => d.Id == discussionId);
                if (discussion != null)
                {
                    if (discussion.HasUserDownvoted)
                    {
                        discussion.Downvotes--;
                        discussion.HasUserDownvoted = false;
                    }
                    else
                    {
                        if (discussion.HasUserUpvoted)
                        {
                            discussion.Upvotes--;
                            discussion.HasUserUpvoted = false;
                        }
                        discussion.Downvotes++;
                        discussion.HasUserDownvoted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to vote: {ex.Message}", "OK");
            }
        }
    }

    private async void OnNewPostClicked(object sender, EventArgs e)
    {
        // Navigate to create new discussion page
        await Shell.Current.GoToAsync("creatediscussion");
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    // Value Converters for XAML
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !string.IsNullOrEmpty(value?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToUpvoteColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Color.FromArgb("#FF6B35") : Color.FromArgb("#95A5A6");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToDownvoteColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Color.FromArgb("#6C5CE7") : Color.FromArgb("#95A5A6");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
