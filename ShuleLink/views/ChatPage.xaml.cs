using ShuleLink.Models;
using ShuleLink.Services;
using ShuleLink.ViewModels;
using System.Collections.ObjectModel;

namespace ShuleLink.Views;

[QueryProperty(nameof(TeacherId), "teacherId")]
[QueryProperty(nameof(TeacherName), "teacherName")]
public partial class ChatPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private readonly GeminiAIService _geminiService;
    private readonly DiscussionService _discussionService;
    private User? _currentUser;
    private Teacher? _teacher;
    private ObservableCollection<ChatMessageViewModel> _messages = new();
    private ObservableCollection<DiscussionViewModel> _discussions = new();
    private bool _isAIChat = false;
    private bool _isDiscussionMode = false;

    public string TeacherId { get; set; } = "";
    public string TeacherName { get; set; } = "";

    public ChatPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        _geminiService = new GeminiAIService(new HttpClient());
        _discussionService = new DiscussionService();
        MessagesCollectionView.ItemsSource = _messages;
    }

    public ChatPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        _geminiService = new GeminiAIService(new HttpClient());
        _discussionService = new DiscussionService();
        MessagesCollectionView.ItemsSource = _messages;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadData();
    }

    private async Task LoadData()
    {
        try
        {
            // Get current user
            var userId = Preferences.Get("UserId", 0);
            if (userId > 0)
            {
                _currentUser = await _databaseService.GetUserAsync(userId);
            }

            // Check if this is AI chat
            if (TeacherId == "AI")
            {
                _isAIChat = true;
                TeacherNameLabel.Text = "🤖 AI Learning Assistant";
                // Note: Online status is now hardcoded in XAML as "🟢 Online"
                
                // Add welcome message if no previous messages
                if (_messages.Count == 0)
                {
                    _messages.Add(new ChatMessageViewModel
                    {
                        Message = "Hello! I'm your AI Learning Assistant. I can help you with:\n\n" +
                                "📚 Homework questions\n" +
                                "🧮 Math problems\n" +
                                "🔬 Science concepts\n" +
                                "📖 Reading comprehension\n" +
                                "✍️ Writing tips\n\n" +
                                "What would you like to learn about today?",
                        IsFromCurrentUser = false,
                        SenderName = "AI Assistant",
                        Timestamp = DateTime.Now,
                        BackgroundColor = "#F8F9FA"
                    });
                }
            }
            else if (int.TryParse(TeacherId, out var teacherIdInt))
            {
                var teachers = await _databaseService.GetTeachersAsync();
                _teacher = teachers.FirstOrDefault(t => t.Id == teacherIdInt);
                
                if (_teacher != null)
                {
                    TeacherNameLabel.Text = _teacher.Name;
                    // Note: Online status is now hardcoded in XAML as "🟢 Online"
                }
            }
            else
            {
                TeacherNameLabel.Text = TeacherName;
            }

            await LoadMessages();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load chat: {ex.Message}", "OK");
        }
    }

    private async Task LoadMessages()
    {
        // Skip loading messages for AI chat as they are handled differently
        if (_isAIChat || _currentUser == null) return;

        // Only load messages if we have a valid teacher
        if (_teacher == null) return;

        try
        {
            var messages = await _databaseService.GetChatMessagesAsync(_currentUser.Id, _teacher.Id);
            _messages.Clear();

            foreach (var message in messages)
            {
                _messages.Add(new ChatMessageViewModel
                {
                    Message = message.Message,
                    SentAt = message.SentAt,
                    Timestamp = message.SentAt,
                    IsFromCurrentUser = message.SenderId == _currentUser.Id,
                    IsSentByUser = message.SenderId == _currentUser.Id,
                    SenderName = message.SenderId == _currentUser.Id ? _currentUser.Name : _teacher.Name,
                    BackgroundColor = message.SenderId == _currentUser.Id ? "#E3F2FD" : "#F8F9FA"
                });
            }

            // Scroll to bottom
            if (_messages.Any())
            {
                MessagesCollectionView.ScrollTo(_messages.Last(), position: ScrollToPosition.End);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load messages: {ex.Message}", "OK");
        }
    }

    private async void OnSendClicked(object sender, EventArgs e)
    {
        var messageText = MessageEntry.Text?.Trim();
        if (string.IsNullOrEmpty(messageText) || _currentUser == null)
            return;

        // For AI chat, we don't need a teacher object
        if (!_isAIChat && _teacher == null)
            return;

        try
        {
            // Add message to UI immediately
            var messageViewModel = new ChatMessageViewModel
            {
                Message = messageText,
                SentAt = DateTime.Now,
                Timestamp = DateTime.Now,
                IsFromCurrentUser = true,
                IsSentByUser = true,
                SenderName = _currentUser.Name,
                BackgroundColor = "#E3F2FD"
            };
            _messages.Add(messageViewModel);

            // Clear input
            MessageEntry.Text = "";

            // Save to database (only for real teachers, not AI)
            if (!_isAIChat && _teacher != null)
            {
                var chatMessage = new ChatMessage
                {
                    SenderId = _currentUser.Id,
                    ReceiverId = _teacher.Id,
                    Message = messageText,
                    SentAt = DateTime.Now,
                    IsRead = false
                };

                await _databaseService.SaveChatMessageAsync(chatMessage);
            }

            // Scroll to bottom
            MessagesCollectionView.ScrollTo(messageViewModel, position: ScrollToPosition.End);

            // Handle response
            if (_isAIChat)
            {
                await HandleAIResponse(messageText);
            }
            else
            {
                await SimulateTeacherResponse();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to send message: {ex.Message}", "OK");
        }
    }

    private async Task HandleAIResponse(string userMessage)
    {
        try
        {
            // Show enhanced typing indicator
            await ShowEnhancedTypingIndicator("AI Assistant");
            
            // Use the new intelligent chat response system
            var aiResponse = await _geminiService.GenerateChatResponseAsync(userMessage, GetChatContext());
            
            await Task.Delay(1500); // Simulate thinking time
            await HideEnhancedTypingIndicator();

            // Generate contextual response if AI APIs fail
            if (string.IsNullOrEmpty(aiResponse) || aiResponse.Length < 10)
            {
                aiResponse = GenerateContextualResponse(userMessage);
            }

            // Add AI response
            var responseMessage = new ChatMessageViewModel
            {
                Message = aiResponse,
                IsFromCurrentUser = false,
                IsSentByUser = false,
                SenderName = "AI Assistant",
                Timestamp = DateTime.Now,
                SentAt = DateTime.Now,
                BackgroundColor = "#F8F9FA"
            };

            _messages.Add(responseMessage);
            MessagesCollectionView.ScrollTo(responseMessage, position: ScrollToPosition.End);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"AI Response Error: {ex.Message}");
            TypingIndicator.IsVisible = false;
            
            // Generate contextual fallback response
            var response = GenerateContextualResponse(userMessage);
            
            var responseMessage = new ChatMessageViewModel
            {
                Message = response,
                IsFromCurrentUser = false,
                IsSentByUser = false,
                SenderName = "AI Assistant",
                Timestamp = DateTime.Now,
                SentAt = DateTime.Now,
                BackgroundColor = "#F8F9FA"
            };

            _messages.Add(responseMessage);
            MessagesCollectionView.ScrollTo(responseMessage, position: ScrollToPosition.End);
        }
    }

    private string GetChatContext()
    {
        // Get last few messages for context
        var recentMessages = _messages.TakeLast(3).ToList();
        var context = string.Join("\n", recentMessages.Select(m => $"{m.SenderName}: {m.Message}"));
        return context;
    }

    private string GenerateContextualResponse(string userMessage)
    {
        var message = userMessage.ToLower();
        
        // Math-related responses
        if (message.Contains("math") || message.Contains("add") || message.Contains("subtract") || 
            message.Contains("multiply") || message.Contains("divide") || message.Contains("number"))
        {
            return "🔢 Great math question! Let me help you think through this step by step. Remember, practice makes perfect in mathematics. What specific part would you like me to explain more?";
        }
        
        // Science-related responses
        if (message.Contains("science") || message.Contains("experiment") || message.Contains("plant") || 
            message.Contains("animal") || message.Contains("water") || message.Contains("earth"))
        {
            return "🔬 Excellent science question! Science is all about discovering how things work. Let me help you understand this concept better. Have you tried observing this in real life?";
        }
        
        // English/Language responses
        if (message.Contains("english") || message.Contains("read") || message.Contains("write") || 
            message.Contains("story") || message.Contains("word"))
        {
            return "📚 Wonderful language question! Reading and writing help us express our thoughts clearly. Let me guide you through this. What part would you like to focus on first?";
        }
        
        // Social Studies responses
        if (message.Contains("history") || message.Contains("geography") || message.Contains("country") || 
            message.Contains("culture") || message.Contains("people"))
        {
            return "🌍 Interesting social studies question! Learning about our world and its people is fascinating. Let me help you explore this topic. What would you like to know more about?";
        }
        
        // Homework help
        if (message.Contains("homework") || message.Contains("assignment") || message.Contains("help"))
        {
            return "📝 I'm here to help with your homework! Remember, the goal is to learn and understand. Let me guide you through the problem step by step. What subject is this for?";
        }
        
        // General encouraging responses
        var generalResponses = new[]
        {
            "🌟 That's a thoughtful question! I love helping curious students like you. Let me break this down in a way that's easy to understand.",
            "💡 Great question! Learning happens when we ask questions like this. Let me help you discover the answer together.",
            "🎓 I can see you're thinking hard about this! That's exactly what good students do. Let me guide you through this step by step.",
            "🤔 Interesting question! Let's explore this together. I'll help you understand the concept so you can apply it to similar problems.",
            "✨ You're asking exactly the right kind of question! Let me help you understand this concept clearly."
        };
        
        var random = new Random();
        return generalResponses[random.Next(generalResponses.Length)];
    }

    private async Task SimulateTeacherResponse()
    {
        if (_teacher == null || _currentUser == null) return;

        try
        {
            // Show typing indicator
            TypingIndicator.IsVisible = true;
            await Task.Delay(2000);
            TypingIndicator.IsVisible = false;

            // Add teacher response
            var responses = new[]
            {
                "Thank you for your message. I'll get back to you soon.",
                "That's a great question! Let me think about it.",
                "I understand your concern. Let's discuss this further.",
                "Good work! Keep it up.",
                "I'll help you with that during our next class.",
                "Please see me after class to discuss this.",
                "That's correct! Well done.",
                "Let me explain this concept in more detail tomorrow."
            };

            var random = new Random();
            var response = responses[random.Next(responses.Length)];

            var teacherMessage = new ChatMessageViewModel
            {
                Message = response,
                SentAt = DateTime.Now,
                Timestamp = DateTime.Now,
                IsFromCurrentUser = false,
                IsSentByUser = false,
                SenderName = _teacher.Name,
                BackgroundColor = "#F8F9FA"
            };

            _messages.Add(teacherMessage);

            // Save teacher response to database with proper teacher ID
            var chatMessage = new ChatMessage
            {
                SenderId = _teacher.Id,
                ReceiverId = _currentUser.Id,
                Message = response,
                SentAt = DateTime.Now,
                IsRead = false
            };

            await _databaseService.SaveChatMessageAsync(chatMessage);

            MessagesCollectionView.ScrollTo(teacherMessage, position: ScrollToPosition.End);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Teacher response error: {ex.Message}");
            TypingIndicator.IsVisible = false;
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCallClicked(object sender, EventArgs e)
    {
        if (_teacher != null)
        {
            var result = await DisplayActionSheet($"Contact {_teacher.Name}", "Cancel", null, 
                "Call Phone", "Send Email", "Video Call");
            
            if (result != "Cancel" && result != null)
            {
                await DisplayAlert("Contact", $"{result} to {_teacher.Name}", "OK");
            }
        }
    }

    private async void OnEmojiClicked(object sender, EventArgs e)
    {
        var emojis = new[] { "😊", "😂", "❤️", "👍", "👎", "😮", "😢", "😡", "🎉", "🔥" };
        var result = await DisplayActionSheet("Choose Emoji", "Cancel", null, emojis);
        
        if (result != "Cancel" && result != null)
        {
            MessageEntry.Text += result;
            MessageEntry.Focus();
        }
    }

    private async void OnAttachClicked(object sender, EventArgs e)
    {
        var result = await DisplayActionSheet("Attach", "Cancel", null, 
            "📷 Photo", "📄 Document", "🎤 Voice Message", "📍 Location");
        
        if (result != "Cancel" && result != null)
        {
            await Services.ToastService.ShowToast($"Attaching {result.Substring(2)}...", Services.ToastType.Info);
        }
    }

    private async void OnChatModeClicked(object sender, EventArgs e)
    {
        if (!_isDiscussionMode) return; // Already in chat mode

        _isDiscussionMode = false;

        // Animate toggle switch
        await AnimateToggleToChat();

        // Update header for chat mode
        HeaderIcon.Text = _isAIChat ? "🤖" : "👨‍🏫";
        TeacherNameLabel.Text = _isAIChat ? "AI Assistant" : TeacherName;
        // Note: Online status is now hardcoded in XAML
        // Note: Call buttons are always visible in new design

        // Show notification
        await Services.ToastService.ShowToast("💬 Back to Chat", Services.ToastType.Info);
    }

    private async Task AnimateToggleToChat()
    {
        // Animation removed - new WhatsApp design doesn't have toggle buttons
        await Task.CompletedTask;
    }

    private async void OnDiscussionModeClicked(object sender, EventArgs e)
    {
        if (_isDiscussionMode) return; // Already in discussion mode

        _isDiscussionMode = true;

        // Animate toggle switch
        await AnimateToggleToDiscussions();

        // Update header for discussions mode
        HeaderIcon.Text = "💭";
        TeacherNameLabel.Text = "Study Discussions";
        // Note: In new design, we navigate to separate discussions page
        await Shell.Current.GoToAsync("discussions");
        return;

        // Load discussions data
        await LoadDiscussionsData();

        // Show notification
        await Services.ToastService.ShowToast("💭 Switched to Discussions", Services.ToastType.Info);
    }

    private async Task AnimateToggleToDiscussions()
    {
        // Animation removed - new WhatsApp design doesn't have toggle buttons
        await Task.CompletedTask;
    }

    private async Task LoadDiscussionsData()
    {
        try
        {
            var discussions = await _discussionService.GetDiscussionsAsync();
            var discussionViewModels = discussions.Select(d => new DiscussionViewModel
            {
                Id = d.Id,
                Title = d.Title,
                Content = d.Content,
                AuthorName = d.AuthorName,
                Subject = d.Subject,
                Grade = d.Grade,
                Topic = d.Topic,
                Upvotes = d.Upvotes,
                Downvotes = d.Downvotes,
                ReplyCount = d.ReplyCount,
                CreatedAt = d.CreatedAt,
                IsResolved = d.IsResolved,
                IsPinned = d.IsPinned
            }).ToList();

            // DiscussionsCollectionView removed in new WhatsApp design
            // Discussions are now handled in separate DiscussionsPage
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading discussions: {ex.Message}");
        }
    }

    private async void OnVideoCallClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Video Call", "Video call feature will be available in future updates.", "OK");
    }

    // Enhanced typing indicator with animations and auto-scroll
    private async Task ShowEnhancedTypingIndicator(string senderName = "Teacher")
    {
        try
        {
            // Update typing text
            TypingText.Text = $"{senderName} is typing...";
            
            // Show typing indicator
            TypingIndicator.IsVisible = true;
            
            // Auto-scroll to show typing indicator
            await ScrollToBottom();
            
            // Start typing dot animation
            _ = AnimateTypingDots();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error showing typing indicator: {ex.Message}");
        }
    }

    private async Task HideEnhancedTypingIndicator()
    {
        try
        {
            TypingIndicator.IsVisible = false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error hiding typing indicator: {ex.Message}");
        }
    }

    private async Task AnimateTypingDots()
    {
        try
        {
            while (TypingIndicator.IsVisible)
            {
                // Animate dot 1
                await TypingDot1.ScaleTo(1.3, 300);
                await TypingDot1.ScaleTo(1.0, 300);
                
                if (!TypingIndicator.IsVisible) break;
                
                // Animate dot 2
                await TypingDot2.ScaleTo(1.3, 300);
                await TypingDot2.ScaleTo(1.0, 300);
                
                if (!TypingIndicator.IsVisible) break;
                
                // Animate dot 3
                await TypingDot3.ScaleTo(1.3, 300);
                await TypingDot3.ScaleTo(1.0, 300);
                
                if (!TypingIndicator.IsVisible) break;
                
                // Small pause before repeating
                await Task.Delay(500);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error animating typing dots: {ex.Message}");
        }
    }

    private async Task ScrollToBottom()
    {
        try
        {
            await Task.Delay(100); // Small delay to ensure UI is updated
            
            if (_messages.Count > 0)
            {
                var lastMessage = _messages.Last();
                MessagesCollectionView.ScrollTo(lastMessage, position: ScrollToPosition.End, animate: true);
            }
            else
            {
                // If no messages, scroll to show typing indicator
                await MessagesScrollView.ScrollToAsync(0, MessagesScrollView.ContentSize.Height, true);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error scrolling to bottom: {ex.Message}");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Hide typing indicator when leaving page
        TypingIndicator.IsVisible = false;
    }

}
