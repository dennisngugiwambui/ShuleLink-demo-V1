using ShuleLink.Models;
using ShuleLink.Services;
using ShuleLink.ViewModels;
using System.Collections.ObjectModel;

namespace ShuleLink.Views;

public partial class LearningPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private readonly GeminiAIService _geminiService;
    private readonly LearningContentService _contentService;
    private User? _currentUser;
    private ObservableCollection<ReadingTopicViewModel> _readingTopics = new();
    private ObservableCollection<LearningTopic> _allTopics = new();
    private ObservableCollection<LearningTopic> _filteredTopics = new();

    public LearningPage()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== LEARNINGPAGE CONSTRUCTOR START ===");
            System.Diagnostics.Debug.WriteLine("LearningPage constructor started...");
            
            System.Diagnostics.Debug.WriteLine("Calling InitializeComponent...");
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("InitializeComponent completed successfully!");
            
            System.Diagnostics.Debug.WriteLine("Creating DatabaseService...");
            _databaseService = new DatabaseService();
            System.Diagnostics.Debug.WriteLine("DatabaseService created successfully!");
            
            System.Diagnostics.Debug.WriteLine("Creating GeminiAIService...");
            _geminiService = new GeminiAIService(new HttpClient());
            System.Diagnostics.Debug.WriteLine("GeminiAIService created successfully!");
            
            System.Diagnostics.Debug.WriteLine("Creating LearningContentService...");
            _contentService = new LearningContentService();
            System.Diagnostics.Debug.WriteLine("LearningContentService created successfully!");
            
            System.Diagnostics.Debug.WriteLine("Setting ItemsSource for CollectionViews...");
            // Set ItemsSource safely
            if (ReadingTopicsCollectionView != null)
            {
                ReadingTopicsCollectionView.ItemsSource = _readingTopics;
                System.Diagnostics.Debug.WriteLine("ReadingTopicsCollectionView ItemsSource set successfully!");
            }
            if (SearchResultsCollectionView != null)
            {
                SearchResultsCollectionView.ItemsSource = _filteredTopics;
                System.Diagnostics.Debug.WriteLine("SearchResultsCollectionView ItemsSource set successfully!");
            }
            
            System.Diagnostics.Debug.WriteLine("=== LEARNINGPAGE CONSTRUCTOR COMPLETED SUCCESSFULLY ===");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("=== LEARNINGPAGE CONSTRUCTOR ERROR ===");
            System.Diagnostics.Debug.WriteLine($"LearningPage constructor error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Exception type: {ex.GetType().Name}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            
            // Don't throw - set minimal fallback state instead
            try
            {
                _databaseService = new DatabaseService();
                _geminiService = new GeminiAIService(new HttpClient());
                _contentService = new LearningContentService();
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
            System.Diagnostics.Debug.WriteLine("LearningPage OnAppearing started...");
            
            // Load data when page appears
            LoadData();
            
            System.Diagnostics.Debug.WriteLine("LearningPage OnAppearing completed successfully!");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LearningPage OnAppearing error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            
            // Set fallback data to prevent crashes
            try
            {
                QuoteText.Text = "\"Education is the most powerful weapon which you can use to change the world.\"";
                QuoteAuthor.Text = "- Nelson Mandela";
                ReadingTopicsCollectionView.ItemsSource = new List<ReadingTopicViewModel>();
                SearchResultsCollectionView.ItemsSource = new List<LearningTopic>();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Failed to set fallback data in LearningPage");
            }
        }
    }

    public LearningPage(DatabaseService databaseService, GeminiAIService geminiService, LearningContentService contentService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        _geminiService = geminiService;
        _contentService = contentService;
        ReadingTopicsCollectionView.ItemsSource = _readingTopics;
        SearchResultsCollectionView.ItemsSource = _filteredTopics;
        LoadData();
    }

    private async void LoadData()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            // Get current user
            var userId = Preferences.Get("UserId", 0);
            if (userId > 0)
            {
                _currentUser = await _databaseService.GetUserAsync(userId);
            }

            await LoadDailyQuote();
            await LoadReadingTopics();
            LoadComprehensiveTopics();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load learning content: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private async Task LoadDailyQuote()
    {
        try
        {
            // Check if we have today's quote
            var todayQuote = await _databaseService.GetTodayQuoteAsync();
            
            if (todayQuote == null)
            {
                // Generate new quote using AI
                var quoteText = await _geminiService.GenerateDailyQuoteAsync();
                
                // Parse quote and author
                var parts = quoteText.Split(" - ");
                var quote = parts[0].Trim('"');
                var author = parts.Length > 1 ? parts[1] : "Unknown";

                // Save to database
                todayQuote = new DailyQuote
                {
                    Quote = quote,
                    Author = author,
                    Category = "Inspiration",
                    Date = DateTime.Today,
                    BackgroundColor = "#4A90E2"
                };

                await _databaseService.SaveDailyQuoteAsync(todayQuote);
            }

            // Display quote
            QuoteText.Text = $"\"{todayQuote.Quote}\"";
            QuoteAuthor.Text = $"- {todayQuote.Author}";
        }
        catch (Exception)
        {
            // Fallback quote
            QuoteText.Text = "\"Education is the most powerful weapon which you can use to change the world.\"";
            QuoteAuthor.Text = "- Nelson Mandela";
        }
    }

    private async Task LoadReadingTopics()
    {
        if (_currentUser == null) return;

        _readingTopics.Clear();

        // Generate reading topics based on user's grade
        var topics = GetReadingTopicsForGrade(_currentUser.Grade);
        
        foreach (var topic in topics)
        {
            _readingTopics.Add(topic);
        }
    }

    private List<ReadingTopicViewModel> GetReadingTopicsForGrade(string grade)
    {
        var topics = new List<ReadingTopicViewModel>();

        switch (grade)
        {
            case "1":
            case "2":
            case "3":
                topics.AddRange(new[]
                {
                    new ReadingTopicViewModel
                    {
                        Title = "Animals and Their Homes",
                        Subject = "Science",
                        Description = "Learn about different animals and where they live",
                        SubjectIcon = "🐾",
                        SubjectColor = "#27AE60"
                    },
                    new ReadingTopicViewModel
                    {
                        Title = "My Family",
                        Subject = "Social Studies",
                        Description = "Understanding family relationships and roles",
                        SubjectIcon = "👨‍👩‍👧‍👦",
                        SubjectColor = "#3498DB"
                    },
                    new ReadingTopicViewModel
                    {
                        Title = "Numbers and Counting",
                        Subject = "Mathematics",
                        Description = "Basic counting and number recognition",
                        SubjectIcon = "🔢",
                        SubjectColor = "#E74C3C"
                    }
                });
                break;

            case "4":
            case "5":
                topics.AddRange(new[]
                {
                    new ReadingTopicViewModel
                    {
                        Title = "The Human Body - Complete Guide",
                        Subject = "Science",
                        Description = "Learn about body systems: respiratory, digestive, circulatory. Includes fun facts and health tips!",
                        SubjectIcon = "🫀",
                        SubjectColor = "#E74C3C"
                    },
                    new ReadingTopicViewModel
                    {
                        Title = "Kenya's Geography & Culture",
                        Subject = "Social Studies", 
                        Description = "Explore Kenya's mountains, lakes, tribes, and wildlife. Discover our beautiful country!",
                        SubjectIcon = "🗺️",
                        SubjectColor = "#27AE60"
                    },
                    new ReadingTopicViewModel
                    {
                        Title = "Fractions Made Easy",
                        Subject = "Mathematics",
                        Description = "Master fractions with pizza slices, cake pieces, and real-life examples. Practice included!",
                        SubjectIcon = "🍰",
                        SubjectColor = "#F39C12"
                    }
                });
                break;

            case "6":
            case "7":
                topics.AddRange(new[]
                {
                    new ReadingTopicViewModel
                    {
                        Title = "Digestive System - Journey of Food",
                        Subject = "Science",
                        Description = "Follow food's amazing journey: mouth → stomach → intestines. Learn digestion secrets!",
                        SubjectIcon = "🍎",
                        SubjectColor = "#E67E22"
                    },
                    new ReadingTopicViewModel
                    {
                        Title = "Respiratory System - Breathing Life",
                        Subject = "Science",
                        Description = "Discover how we breathe: nose → lungs → blood. Includes breathing exercises!",
                        SubjectIcon = "🫁",
                        SubjectColor = "#3498DB"
                    },
                    new ReadingTopicViewModel
                    {
                        Title = "African Kingdoms & Heroes",
                        Subject = "Social Studies",
                        Description = "Explore great African empires, brave leaders, and our proud heritage!",
                        SubjectIcon = "🏛️",
                        SubjectColor = "#8E44AD"
                    },
                    new ReadingTopicViewModel
                    {
                        Title = "Algebra Adventure",
                        Subject = "Mathematics",
                        Description = "Solve mysteries with X and Y! Learn variables, equations, and problem-solving tricks.",
                        SubjectIcon = "📐",
                        SubjectColor = "#2C3E50"
                    }
                });
                break;
        }

        return topics;
    }

    private async void OnNewQuoteClicked(object sender, EventArgs e)
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var newQuote = await _geminiService.GenerateDailyQuoteAsync();
            var parts = newQuote.Split(" - ");
            var quote = parts[0].Trim('"');
            var author = parts.Length > 1 ? parts[1] : "Unknown";

            QuoteText.Text = $"\"{quote}\"";
            QuoteAuthor.Text = $"- {author}";
        }
        catch (Exception)
        {
            await DisplayAlert("Error", "Failed to generate new quote. Please try again.", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private void LoadComprehensiveTopics()
    {
        if (_currentUser == null) return;
        
        var topics = _contentService.GetComprehensiveTopics(_currentUser.Grade);
        _allTopics.Clear();
        foreach (var topic in topics)
        {
            _allTopics.Add(topic);
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue?.ToLower() ?? "";
        
        _filteredTopics.Clear();
        
        if (string.IsNullOrWhiteSpace(searchText))
        {
            SearchResultsFrame.IsVisible = false;
            return;
        }

        var results = _allTopics.Where(t => 
            t.Title.ToLower().Contains(searchText) ||
            t.Subject.ToLower().Contains(searchText) ||
            t.Content.ToLower().Contains(searchText)
        ).Take(5);

        foreach (var result in results)
        {
            _filteredTopics.Add(result);
        }

        SearchResultsFrame.IsVisible = _filteredTopics.Count > 0;
    }

    private async void OnSearchResultTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is LearningTopic topic)
        {
            SearchEntry.Text = "";
            SearchResultsFrame.IsVisible = false;
            await Shell.Current.GoToAsync($"ReadingDetailPage?title={Uri.EscapeDataString(topic.Title)}&subject={Uri.EscapeDataString(topic.Subject)}&grade={Uri.EscapeDataString(topic.Grade)}");
        }
    }

    private async void OnReadingTopicTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is ReadingTopicViewModel topic)
        {
            await Shell.Current.GoToAsync($"ReadingDetailPage?title={Uri.EscapeDataString(topic.Title)}&subject={Uri.EscapeDataString(topic.Subject)}&grade={Uri.EscapeDataString(_currentUser?.Grade ?? "5")}");
        }
    }

    private async void OnDiagramClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            var diagramType = button.Text;
            await Shell.Current.GoToAsync($"DiagramPage?type={diagramType}");
        }
    }
}
