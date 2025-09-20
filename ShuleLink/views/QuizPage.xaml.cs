using ShuleLink.Models;
using ShuleLink.Services;
using System.Collections.ObjectModel;

namespace ShuleLink.Views;

[QueryProperty(nameof(Subject), "subject")]
[QueryProperty(nameof(Grade), "grade")]
[QueryProperty(nameof(Topic), "topic")]
public partial class QuizPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private readonly GeminiAIService _geminiService;
    private User? _currentUser;
    private List<QuizQuestion> _currentQuestions = new();
    private int _currentQuestionIndex = 0;
    private int _score = 0;
    private System.Timers.Timer? _timer;
    private int _timeLeft = 30;

    public string Subject { get; set; } = "";
    public string Grade { get; set; } = "";
    public string Topic { get; set; } = "";

    public QuizPage()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== QUIZPAGE CONSTRUCTOR START ===");
            System.Diagnostics.Debug.WriteLine("QuizPage constructor started...");
            
            System.Diagnostics.Debug.WriteLine("Calling InitializeComponent...");
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("InitializeComponent completed successfully!");
            
            System.Diagnostics.Debug.WriteLine("Creating DatabaseService...");
            _databaseService = new DatabaseService();
            System.Diagnostics.Debug.WriteLine("DatabaseService created successfully!");
            
            System.Diagnostics.Debug.WriteLine("Creating GeminiAIService...");
            _geminiService = new GeminiAIService(new HttpClient());
            System.Diagnostics.Debug.WriteLine("GeminiAIService created successfully!");
            
            System.Diagnostics.Debug.WriteLine("=== QUIZPAGE CONSTRUCTOR COMPLETED SUCCESSFULLY ===");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("=== QUIZPAGE CONSTRUCTOR ERROR ===");
            System.Diagnostics.Debug.WriteLine($"QuizPage constructor error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Exception type: {ex.GetType().Name}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException?.Message}");
            
            // Don't throw - set minimal fallback state instead
            try
            {
                _databaseService = new DatabaseService();
                _geminiService = new GeminiAIService(new HttpClient());
                System.Diagnostics.Debug.WriteLine("Fallback services created successfully!");
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Failed to create fallback services - page may not function properly");
            }
        }
    }

    public QuizPage(DatabaseService databaseService, GeminiAIService geminiService)
    {
        try
        {
            InitializeComponent();
            _databaseService = databaseService;
            _geminiService = geminiService;
            // Don't load data in constructor to prevent crashes
            // _ = LoadUserData();
            System.Diagnostics.Debug.WriteLine("QuizPage constructor (with services) completed");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"QuizPage constructor (with services) error: {ex.Message}");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Check if we have query parameters for direct quiz start
        if (!string.IsNullOrEmpty(Subject) && !string.IsNullOrEmpty(Topic))
        {
            // Auto-start quiz with provided parameters
            await Task.Delay(500); // Give UI time to load
            await StartQuiz(Subject, Topic);
        }
    }

    private async Task LoadUserData()
    {
        var userId = Preferences.Get("UserId", 0);
        if (userId > 0)
        {
            _currentUser = await _databaseService.GetUserAsync(userId);
        }
    }

    private void OnSubjectChanged(object sender, EventArgs e)
    {
        var subject = SubjectPicker.SelectedItem?.ToString();
        LoadTopicsForSubject(subject);
    }

    private void LoadTopicsForSubject(string? subject)
    {
        TopicPicker.Items.Clear();
        
        // Always add custom topic option first
        TopicPicker.Items.Add("Custom Topic (Enter Any Topic)");
        
        switch (subject)
        {
            case "Science":
                TopicPicker.Items.Add("Digestive System");
                TopicPicker.Items.Add("Respiratory System");
                TopicPicker.Items.Add("Plant Parts");
                TopicPicker.Items.Add("Animals");
                TopicPicker.Items.Add("Solar System");
                TopicPicker.Items.Add("Weather");
                TopicPicker.Items.Add("Human Body");
                TopicPicker.Items.Add("Matter and Energy");
                break;
            case "Mathematics":
                TopicPicker.Items.Add("Fractions");
                TopicPicker.Items.Add("Multiplication");
                TopicPicker.Items.Add("Division");
                TopicPicker.Items.Add("Geometry");
                TopicPicker.Items.Add("Measurement");
                TopicPicker.Items.Add("Decimals");
                TopicPicker.Items.Add("Word Problems");
                break;
            case "English":
                TopicPicker.Items.Add("Grammar");
                TopicPicker.Items.Add("Vocabulary");
                TopicPicker.Items.Add("Reading Comprehension");
                TopicPicker.Items.Add("Writing");
                TopicPicker.Items.Add("Spelling");
                TopicPicker.Items.Add("Poetry");
                break;
            case "Social Studies":
                TopicPicker.Items.Add("Geography");
                TopicPicker.Items.Add("History");
                TopicPicker.Items.Add("Culture");
                TopicPicker.Items.Add("Government");
                TopicPicker.Items.Add("Community");
                break;
            default:
                TopicPicker.Items.Add("General Knowledge");
                break;
        }
    }

    private async void OnStartQuizClicked(object sender, EventArgs e)
    {
        var subject = SubjectPicker.SelectedItem?.ToString();
        var topic = TopicPicker.SelectedItem?.ToString();
        
        // Check if user wants to enter a custom topic
        if (string.IsNullOrEmpty(topic) || topic.Contains("Custom Topic"))
        {
            var customTopic = await DisplayPromptAsync("Custom Topic", 
                "Enter any topic you'd like to be quizzed on:", 
                "Start Quiz", "Cancel", 
                "e.g., Solar System, Fractions, Animals, etc.");
            
            if (!string.IsNullOrEmpty(customTopic))
            {
                topic = customTopic;
            }
            else
            {
                return; // User cancelled
            }
        }
        
        if (string.IsNullOrEmpty(subject))
        {
            await DisplayAlert("Error", "Please select a subject", "OK");
            return;
        }

        await StartQuiz(subject, topic);
    }

    private async Task StartQuiz(string subject, string topic)
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            StartQuizBtn.IsEnabled = false;

            // Ensure user is loaded
            if (_currentUser == null)
            {
                await LoadUserData();
                if (_currentUser == null)
                {
                    await DisplayAlert("Error", "Unable to load user data. Please try logging in again.", "OK");
                    return;
                }
            }

            // Generate 30+ questions for comprehensive quiz
            _currentQuestions = await _geminiService.GenerateQuizQuestionsAsync(subject, _currentUser.Grade ?? "5", topic, 30);
            
            if (_currentQuestions != null && _currentQuestions.Any())
            {
                _currentQuestionIndex = 0;
                _score = 0;
                
                QuizSelectionFrame.IsVisible = false;
                QuizContentFrame.IsVisible = true;
                
                ShowCurrentQuestion();
                StartTimer();
            }
            else
            {
                await DisplayAlert("Error", "Failed to generate quiz questions. Please check your internet connection and try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to generate quiz: {ex.Message}\n\nPlease check your internet connection and try again.", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            StartQuizBtn.IsEnabled = true;
        }
    }

    private void ShowCurrentQuestion()
    {
        if (_currentQuestionIndex >= _currentQuestions.Count) return;

        var question = _currentQuestions[_currentQuestionIndex];
        
        QuestionCounterLabel.Text = $"Question {_currentQuestionIndex + 1} of {_currentQuestions.Count}";
        QuestionLabel.Text = question.Question;
        
        OptionA.Text = $"A. {question.OptionA}";
        OptionB.Text = $"B. {question.OptionB}";
        OptionC.Text = $"C. {question.OptionC}";
        OptionD.Text = $"D. {question.OptionD}";
        
        // Reset button colors
        ResetOptionColors();
        
        ExplanationFrame.IsVisible = false;
        NextButton.IsVisible = false;
        
        _timeLeft = 30;
        StartTimer();
    }

    private void ResetOptionColors()
    {
        OptionA.BackgroundColor = Color.FromArgb("#ECF0F1");
        OptionB.BackgroundColor = Color.FromArgb("#ECF0F1");
        OptionC.BackgroundColor = Color.FromArgb("#ECF0F1");
        OptionD.BackgroundColor = Color.FromArgb("#ECF0F1");
    }

    private async void OnOptionClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            _timer?.Stop();
            
            var selectedOption = button.Text.Substring(0, 1); // Get A, B, C, or D
            var question = _currentQuestions[_currentQuestionIndex];
            
            var isCorrect = selectedOption == question.CorrectAnswer;
            
            if (isCorrect)
            {
                button.BackgroundColor = Color.FromArgb("#27AE60");
                _score++;
            }
            else
            {
                button.BackgroundColor = Color.FromArgb("#E74C3C");
                // Highlight correct answer
                var correctButton = question.CorrectAnswer switch
                {
                    "A" => OptionA,
                    "B" => OptionB,
                    "C" => OptionC,
                    _ => OptionD
                };
                correctButton.BackgroundColor = Color.FromArgb("#27AE60");
            }
            
            ExplanationLabel.Text = question.Explanation;
            ExplanationFrame.IsVisible = true;
            NextButton.IsVisible = true;
        }
    }

    private void OnNextQuestionClicked(object sender, EventArgs e)
    {
        _currentQuestionIndex++;
        
        if (_currentQuestionIndex >= _currentQuestions.Count)
        {
            ShowResults();
        }
        else
        {
            ShowCurrentQuestion();
        }
    }

    private void ShowResults()
    {
        QuizContentFrame.IsVisible = false;
        ResultsFrame.IsVisible = true;
        
        ScoreLabel.Text = $"{_score}/{_currentQuestions.Count}";
        
        var percentage = (double)_score / _currentQuestions.Count * 100;
        PerformanceLabel.Text = percentage switch
        {
            >= 80 => "🌟 Excellent! Outstanding work!",
            >= 60 => "👍 Good job! Keep it up!",
            >= 40 => "📚 Not bad! Study more to improve!",
            _ => "💪 Keep practicing! You'll get better!"
        };
    }

    private void StartTimer()
    {
        _timer?.Stop();
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += (s, e) =>
        {
            _timeLeft--;
            Device.BeginInvokeOnMainThread(() =>
            {
                TimerLabel.Text = $"{_timeLeft}s";
                if (_timeLeft <= 0)
                {
                    _timer.Stop();
                    OnNextQuestionClicked(this, EventArgs.Empty);
                }
            });
        };
        _timer.Start();
    }

    private void OnTryAgainClicked(object sender, EventArgs e)
    {
        ResultsFrame.IsVisible = false;
        QuizSelectionFrame.IsVisible = true;
    }

    private async void OnViewDetailsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Quiz Details", $"Score: {_score}/{_currentQuestions.Count}\nPercentage: {(double)_score / _currentQuestions.Count * 100:F1}%", "OK");
    }
}
