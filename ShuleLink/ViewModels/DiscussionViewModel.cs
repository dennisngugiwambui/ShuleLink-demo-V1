using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShuleLink.ViewModels
{
    public class DiscussionViewModel : INotifyPropertyChanged
    {
        private int _id;
        private string _title = string.Empty;
        private string _content = string.Empty;
        private string? _imageUrl;
        private int _authorId;
        private string _authorName = string.Empty;
        private string _subject = string.Empty;
        private string _grade = string.Empty;
        private string _topic = string.Empty;
        private int _upvotes;
        private int _downvotes;
        private int _replyCount;
        private DateTime _createdAt;
        private bool _isResolved;
        private bool _isPinned;
        private bool _hasUserUpvoted;
        private bool _hasUserDownvoted;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public string? ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        public int AuthorId
        {
            get => _authorId;
            set => SetProperty(ref _authorId, value);
        }

        public string AuthorName
        {
            get => _authorName;
            set => SetProperty(ref _authorName, value);
        }

        public string Subject
        {
            get => _subject;
            set => SetProperty(ref _subject, value);
        }

        public string Grade
        {
            get => _grade;
            set => SetProperty(ref _grade, value);
        }

        public string Topic
        {
            get => _topic;
            set => SetProperty(ref _topic, value);
        }

        public int Upvotes
        {
            get => _upvotes;
            set
            {
                SetProperty(ref _upvotes, value);
                OnPropertyChanged(nameof(NetVotes));
                OnPropertyChanged(nameof(VoteDisplay));
            }
        }

        public int Downvotes
        {
            get => _downvotes;
            set
            {
                SetProperty(ref _downvotes, value);
                OnPropertyChanged(nameof(NetVotes));
                OnPropertyChanged(nameof(VoteDisplay));
            }
        }

        public int ReplyCount
        {
            get => _replyCount;
            set
            {
                SetProperty(ref _replyCount, value);
                OnPropertyChanged(nameof(ReplyCountDisplay));
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                SetProperty(ref _createdAt, value);
                OnPropertyChanged(nameof(TimeAgo));
            }
        }

        public bool IsResolved
        {
            get => _isResolved;
            set => SetProperty(ref _isResolved, value);
        }

        public bool IsPinned
        {
            get => _isPinned;
            set => SetProperty(ref _isPinned, value);
        }

        public bool HasUserUpvoted
        {
            get => _hasUserUpvoted;
            set
            {
                SetProperty(ref _hasUserUpvoted, value);
                OnPropertyChanged(nameof(UpvoteColor));
            }
        }

        public bool HasUserDownvoted
        {
            get => _hasUserDownvoted;
            set
            {
                SetProperty(ref _hasUserDownvoted, value);
                OnPropertyChanged(nameof(DownvoteColor));
            }
        }

        // Computed properties
        public int NetVotes => Upvotes - Downvotes;

        public string VoteDisplay
        {
            get
            {
                var net = NetVotes;
                if (net > 0) return $"+{net}";
                if (net < 0) return net.ToString();
                return "0";
            }
        }

        public string ReplyCountDisplay => ReplyCount == 1 ? "1 reply" : $"{ReplyCount} replies";

        public string TimeAgo
        {
            get
            {
                var timeSpan = DateTime.Now - CreatedAt;
                
                if (timeSpan.TotalMinutes < 1)
                    return "Just now";
                if (timeSpan.TotalMinutes < 60)
                    return $"{(int)timeSpan.TotalMinutes}m ago";
                if (timeSpan.TotalHours < 24)
                    return $"{(int)timeSpan.TotalHours}h ago";
                if (timeSpan.TotalDays < 7)
                    return $"{(int)timeSpan.TotalDays}d ago";
                
                return CreatedAt.ToString("MMM dd");
            }
        }

        public string SubjectTag => string.IsNullOrEmpty(Subject) ? "" : Subject;
        public string GradeTag => string.IsNullOrEmpty(Grade) ? "" : $"Grade {Grade}";
        public string TopicTag => string.IsNullOrEmpty(Topic) ? "" : Topic;

        public bool HasSubjectTag => !string.IsNullOrEmpty(Subject);
        public bool HasGradeTag => !string.IsNullOrEmpty(Grade);
        public bool HasTopicTag => !string.IsNullOrEmpty(Topic);

        public bool HasImage => !string.IsNullOrEmpty(ImageUrl);
        public bool IsQuestion => Content.Contains("?") || Title.Contains("?");
        public string PostTypeIcon => IsQuestion ? "❓" : "💬";

        public Color UpvoteColor => HasUserUpvoted ? Color.FromArgb("#FF6B35") : Color.FromArgb("#95A5A6");
        public Color DownvoteColor => HasUserDownvoted ? Color.FromArgb("#6C5CE7") : Color.FromArgb("#95A5A6");

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public class DiscussionReplyViewModel : INotifyPropertyChanged
    {
        private int _id;
        private int _discussionId;
        private string _content = string.Empty;
        private string? _imageUrl;
        private int _authorId;
        private string _authorName = string.Empty;
        private int _upvotes;
        private int _downvotes;
        private DateTime _createdAt;
        private bool _isAcceptedAnswer;
        private bool _hasUserUpvoted;
        private bool _hasUserDownvoted;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public int DiscussionId
        {
            get => _discussionId;
            set => SetProperty(ref _discussionId, value);
        }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public string? ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        public int AuthorId
        {
            get => _authorId;
            set => SetProperty(ref _authorId, value);
        }

        public string AuthorName
        {
            get => _authorName;
            set => SetProperty(ref _authorName, value);
        }

        public int Upvotes
        {
            get => _upvotes;
            set
            {
                SetProperty(ref _upvotes, value);
                OnPropertyChanged(nameof(NetVotes));
                OnPropertyChanged(nameof(VoteDisplay));
            }
        }

        public int Downvotes
        {
            get => _downvotes;
            set
            {
                SetProperty(ref _downvotes, value);
                OnPropertyChanged(nameof(NetVotes));
                OnPropertyChanged(nameof(VoteDisplay));
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                SetProperty(ref _createdAt, value);
                OnPropertyChanged(nameof(TimeAgo));
            }
        }

        public bool IsAcceptedAnswer
        {
            get => _isAcceptedAnswer;
            set => SetProperty(ref _isAcceptedAnswer, value);
        }

        public bool HasUserUpvoted
        {
            get => _hasUserUpvoted;
            set => SetProperty(ref _hasUserUpvoted, value);
        }

        public bool HasUserDownvoted
        {
            get => _hasUserDownvoted;
            set => SetProperty(ref _hasUserDownvoted, value);
        }

        // Computed properties
        public int NetVotes => Upvotes - Downvotes;

        public string VoteDisplay
        {
            get
            {
                var net = NetVotes;
                if (net > 0) return $"+{net}";
                if (net < 0) return net.ToString();
                return "0";
            }
        }

        public string TimeAgo
        {
            get
            {
                var timeSpan = DateTime.Now - CreatedAt;
                
                if (timeSpan.TotalMinutes < 1)
                    return "Just now";
                if (timeSpan.TotalMinutes < 60)
                    return $"{(int)timeSpan.TotalMinutes}m ago";
                if (timeSpan.TotalHours < 24)
                    return $"{(int)timeSpan.TotalHours}h ago";
                if (timeSpan.TotalDays < 7)
                    return $"{(int)timeSpan.TotalDays}d ago";
                
                return CreatedAt.ToString("MMM dd");
            }
        }

        public bool HasImage => !string.IsNullOrEmpty(ImageUrl);
        public string AcceptedAnswerIcon => IsAcceptedAnswer ? "✅" : "";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
