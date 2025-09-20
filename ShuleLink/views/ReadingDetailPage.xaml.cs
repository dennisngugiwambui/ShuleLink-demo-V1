using ShuleLink.Services;

namespace ShuleLink.Views;

[QueryProperty(nameof(Title), "title")]
[QueryProperty(nameof(Subject), "subject")]
[QueryProperty(nameof(Grade), "grade")]
public partial class ReadingDetailPage : ContentPage
{
    private readonly LearningContentService _contentService;
    private readonly GeminiAIService _geminiService;
    private bool _isLoadingContent = false;
    
    public string Title { get; set; } = "";
    public string Subject { get; set; } = "";
    public string Grade { get; set; } = "";

    public ReadingDetailPage()
    {
        InitializeComponent();
        _contentService = new LearningContentService();
        _geminiService = new GeminiAIService(new HttpClient());
    }

    public ReadingDetailPage(LearningContentService contentService)
    {
        InitializeComponent();
        _contentService = contentService;
        _geminiService = new GeminiAIService(new HttpClient());
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        TitleLabel.Text = Title;
        SubjectLabel.Text = Subject;
        LoadContent();
    }
    
    private async void LoadContent()
    {
        if (_isLoadingContent) return;
        _isLoadingContent = true;

        try
        {
            // First try to get content from local service
            var topics = _contentService.GetComprehensiveTopics(Grade);
            var topic = topics.FirstOrDefault(t => t.Title == Title);
            
            if (topic != null && !string.IsNullOrEmpty(topic.Content))
            {
                ContentLabel.Text = topic.Content;
            }
            else
            {
                // Show loading message
                ContentLabel.Text = "🔄 Generating comprehensive notes for this topic...";
                
                // Generate content using AI
                await GenerateContentWithAI();
            }
        }
        catch (Exception ex)
        {
            ContentLabel.Text = $"Error loading content: {ex.Message}";
        }
        finally
        {
            _isLoadingContent = false;
        }
    }

    private async Task GenerateContentWithAI()
    {
        try
        {
            // Use the new comprehensive content generation
            var generatedContent = await _geminiService.GenerateComprehensiveTopicNotesAsync(Subject, Grade, Title);
            
            // If comprehensive content is too short, fallback to regular notes
            if (string.IsNullOrEmpty(generatedContent) || generatedContent.Length < 500)
            {
                generatedContent = await _geminiService.GenerateTopicNotesAsync(Subject, Grade, Title);
            }
            
            ContentLabel.Text = generatedContent;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"AI content generation error: {ex.Message}");
            ContentLabel.Text = GetFallbackContent();
        }
    }

    private string GetAgeRange(string grade)
    {
        return grade switch
        {
            "1" => "6-7 years",
            "2" => "7-8 years", 
            "3" => "8-9 years",
            "4" => "9-10 years",
            "5" => "10-11 years",
            "6" => "11-12 years",
            "7" => "12-13 years",
            _ => "6-13 years"
        };
    }

    private string GetFallbackContent()
    {
        return $@"📚 {Title}

Welcome to your {Subject} lesson on {Title}!

🎯 Learning Objectives:
• Understand the key concepts of {Title}
• Learn how {Title} applies to everyday life
• Develop critical thinking about {Subject}

📖 Introduction:
{Title} is an important topic in {Subject} that helps us understand the world around us. This topic is designed for Grade {Grade} students to explore and learn.

🔍 Key Points to Remember:
• Pay attention to the main concepts
• Ask questions when you don't understand
• Practice what you learn
• Connect this topic to what you already know

💡 Study Tips:
• Read through the material carefully
• Take notes of important points
• Discuss with your classmates and teachers
• Practice with the quiz to test your understanding

🎓 Ready to Learn?
Take your time to understand this topic. Remember, learning is a journey, and every step counts!

Click 'Take Quiz on This Topic' below when you're ready to test your knowledge!";
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnTakeQuizClicked(object sender, EventArgs e)
    {
        // Navigate to quiz page with topic information
        var navigationParameter = $"Quiz?subject={Uri.EscapeDataString(Subject)}&grade={Uri.EscapeDataString(Grade)}&topic={Uri.EscapeDataString(Title)}";
        await Shell.Current.GoToAsync($"//{navigationParameter}");
    }
}
