using ShuleLink.Models;
using ShuleLink.Services;
using System.Collections.ObjectModel;

namespace ShuleLink.Views;

public partial class DiscussionForumPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private ObservableCollection<DiscussionPost> _posts = new();
    private string _selectedCategory = "All";
    private User? _currentUser;

    public DiscussionForumPage()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("DiscussionForumPage constructor started...");
            InitializeComponent();
            _databaseService = new DatabaseService();
            
            // Set ItemsSource safely
            if (PostsCollectionView != null)
                PostsCollectionView.ItemsSource = _posts;
            
            System.Diagnostics.Debug.WriteLine("DiscussionForumPage constructor completed");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DiscussionForumPage constructor error: {ex.Message}");
        }
    }

    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();
            System.Diagnostics.Debug.WriteLine("DiscussionForumPage OnAppearing started");
            
            // Don't load data automatically to prevent crashes
            // await LoadCurrentUser();
            // await LoadPosts();
            
            System.Diagnostics.Debug.WriteLine("DiscussionForumPage OnAppearing completed");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DiscussionForumPage OnAppearing error: {ex.Message}");
        }
    }

    private async Task LoadCurrentUser()
    {
        try
        {
            var userId = Preferences.Get("UserId", 0);
            if (userId > 0)
            {
                _currentUser = await _databaseService.GetUserAsync(userId);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading current user: {ex.Message}");
        }
    }

    private async Task LoadPosts()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            // Initialize discussion tables if they don't exist
            await InitializeDiscussionTables();

            var allPosts = await GetDiscussionPostsAsync();

            // Filter by category if not "All"
            if (_selectedCategory != "All")
            {
                allPosts = allPosts.Where(p => p.Category == _selectedCategory).ToList();
            }

            // Sort by sticky first, then by creation date
            allPosts = allPosts.OrderByDescending(p => p.IsSticky)
                              .ThenByDescending(p => p.CreatedAt)
                              .ToList();

            _posts.Clear();
            foreach (var post in allPosts)
            {
                _posts.Add(post);
            }

            // If no posts exist, create some sample posts
            if (_posts.Count == 0)
            {
                await CreateSamplePosts();
                await LoadPosts(); // Reload after creating samples
                return;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load posts: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private async Task InitializeDiscussionTables()
    {
        try
        {
            var db = await _databaseService.GetDatabaseAsync();
            await db.CreateTableAsync<DiscussionPost>();
            await db.CreateTableAsync<DiscussionReply>();
            await db.CreateTableAsync<DiscussionLike>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error initializing discussion tables: {ex.Message}");
        }
    }

    private async Task<List<DiscussionPost>> GetDiscussionPostsAsync()
    {
        try
        {
            var db = await _databaseService.GetDatabaseAsync();
            return await db.Table<DiscussionPost>().ToListAsync();
        }
        catch
        {
            return new List<DiscussionPost>();
        }
    }

    private async Task CreateSamplePosts()
    {
        try
        {
            var db = await _databaseService.GetDatabaseAsync();

            var samplePosts = new List<DiscussionPost>
            {
                new DiscussionPost
                {
                    Title = "🤔 What's your favorite math trick?",
                    Content = "I just learned that you can multiply by 9 using your fingers! Hold up 10 fingers, fold down the finger for the number you're multiplying (like 9×3, fold the 3rd finger), and count the fingers on each side. Mind blown! 🤯\n\nWhat other cool math tricks do you know?",
                    AuthorId = 1,
                    AuthorName = "Sarah Johnson",
                    AuthorType = "Student",
                    Category = "Math",
                    CreatedAt = DateTime.Now.AddHours(-2),
                    Likes = 15,
                    Replies = 8,
                    Tags = "math,tricks,multiplication"
                },
                new DiscussionPost
                {
                    Title = "🔬 Science Fair Project Ideas?",
                    Content = "Our science fair is coming up next month and I need ideas! I'm really interested in chemistry and biology. Has anyone done a cool experiment that was fun and educational?\n\nI was thinking about growing crystals or testing which liquids clean pennies best. What do you think?",
                    AuthorId = 2,
                    AuthorName = "Mike Chen",
                    AuthorType = "Student",
                    Category = "Science",
                    CreatedAt = DateTime.Now.AddHours(-4),
                    Likes = 12,
                    Replies = 6,
                    Tags = "science,fair,experiments,chemistry"
                },
                new DiscussionPost
                {
                    Title = "📚 Book Recommendations for Grade 5",
                    Content = "Hi everyone! I'm looking for some good books to read during the holidays. I love adventure stories and mysteries. I've already read all the Magic Tree House books and the first few Harry Potter books.\n\nAny suggestions for what I should read next?",
                    AuthorId = 3,
                    AuthorName = "Emma Wilson",
                    AuthorType = "Student",
                    Category = "English",
                    CreatedAt = DateTime.Now.AddHours(-6),
                    Likes = 9,
                    Replies = 12,
                    Tags = "books,reading,adventure,mystery"
                },
                new DiscussionPost
                {
                    Title = "💡 Study Tips That Actually Work",
                    Content = "As a teacher, I wanted to share some study techniques that I've seen work really well with students:\n\n1. 🎵 Turn facts into songs or rhymes\n2. 📝 Teach someone else what you learned\n3. 🎨 Use colors and drawings in your notes\n4. ⏰ Take breaks every 25 minutes\n5. 🏃‍♂️ Study while walking around\n\nWhat study methods work best for you?",
                    AuthorId = 101,
                    AuthorName = "Mrs. Rodriguez",
                    AuthorType = "Teacher",
                    Category = "General",
                    CreatedAt = DateTime.Now.AddHours(-8),
                    Likes = 25,
                    Replies = 15,
                    IsSticky = true,
                    Tags = "study,tips,learning,teacher"
                },
                new DiscussionPost
                {
                    Title = "🌍 Climate Change Discussion",
                    Content = "We're learning about climate change in science class and I'm curious about what we as students can do to help. I've started recycling more and turning off lights, but what else can we do?\n\nAlso, does anyone know good websites with kid-friendly information about environmental issues?",
                    AuthorId = 4,
                    AuthorName = "Alex Thompson",
                    AuthorType = "Student",
                    Category = "Science",
                    CreatedAt = DateTime.Now.AddHours(-12),
                    Likes = 18,
                    Replies = 10,
                    Tags = "environment,climate,recycling,science"
                }
            };

            foreach (var post in samplePosts)
            {
                await db.InsertAsync(post);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error creating sample posts: {ex.Message}");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnNewPostClicked(object sender, EventArgs e)
    {
        if (_currentUser == null)
        {
            await DisplayAlert("Login Required", "Please log in to create a new post.", "OK");
            return;
        }

        await Shell.Current.GoToAsync("NewDiscussionPostPage");
    }

    private async void OnCategorySelected(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            // Reset all category buttons
            ResetCategoryButtons();

            // Highlight selected category
            button.BackgroundColor = Color.FromArgb("#4A90E2");
            button.TextColor = Colors.White;

            // Get category from button
            _selectedCategory = button.Text.Contains("Math") ? "Math" :
                               button.Text.Contains("Science") ? "Science" :
                               button.Text.Contains("English") ? "English" :
                               button.Text.Contains("General") ? "General" :
                               button.Text.Contains("Homework") ? "Homework Help" :
                               "All";

            await LoadPosts();
        }
    }

    private void ResetCategoryButtons()
    {
        var buttons = new[] { AllCategoryBtn, MathCategoryBtn, ScienceCategoryBtn, 
                             EnglishCategoryBtn, GeneralCategoryBtn, HomeworkCategoryBtn };

        foreach (var btn in buttons)
        {
            btn.BackgroundColor = Color.FromArgb("#E8F4FD");
            btn.TextColor = Color.FromArgb("#4A90E2");
        }
    }

    private async void OnPostSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is DiscussionPost selectedPost)
        {
            await Shell.Current.GoToAsync($"DiscussionPostDetailPage?postId={selectedPost.Id}");
            
            // Clear selection
            PostsCollectionView.SelectedItem = null;
        }
    }

    private async void OnRefresh(object sender, EventArgs e)
    {
        await LoadPosts();
        PostsRefreshView.IsRefreshing = false;
    }
}
