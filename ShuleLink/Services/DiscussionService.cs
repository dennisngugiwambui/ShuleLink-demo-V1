using ShuleLink.Models;
using ShuleLink.ViewModels;
using System.Collections.ObjectModel;

namespace ShuleLink.Services
{
    public class DiscussionService
    {
        private readonly DatabaseService _databaseService;
        private readonly List<Discussion> _discussions = new();
        private readonly List<DiscussionReply> _replies = new();
        private readonly List<DiscussionVote> _votes = new();

        public DiscussionService()
        {
            _databaseService = new DatabaseService();
            InitializeSampleData();
        }

        public DiscussionService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            // Add some sample discussions for demo purposes
            _discussions.AddRange(new[]
            {
                new Discussion
                {
                    Id = 1,
                    Title = "How do I solve quadratic equations?",
                    Content = "I'm struggling with quadratic equations in my math class. Can someone explain the quadratic formula step by step? I understand the basics but get confused with the discriminant part.",
                    AuthorId = 1,
                    AuthorName = "Sarah M.",
                    Subject = "Mathematics",
                    Grade = "8",
                    Topic = "Quadratic Equations",
                    Upvotes = 15,
                    Downvotes = 2,
                    ReplyCount = 8,
                    CreatedAt = DateTime.Now.AddHours(-3),
                    IsResolved = false
                },
                new Discussion
                {
                    Id = 2,
                    Title = "Physics: Understanding Newton's Laws",
                    Content = "Can someone help me understand the difference between Newton's three laws of motion? I have a test tomorrow and I'm really confused about when to apply each law.",
                    AuthorId = 2,
                    AuthorName = "John D.",
                    Subject = "Physics",
                    Grade = "9",
                    Topic = "Newton's Laws",
                    Upvotes = 23,
                    Downvotes = 1,
                    ReplyCount = 12,
                    CreatedAt = DateTime.Now.AddHours(-5),
                    IsResolved = true
                },
                new Discussion
                {
                    Id = 3,
                    Title = "Chemistry: Balancing Chemical Equations",
                    Content = "I need help balancing this equation: H₂ + O₂ → H₂O. I know the answer but I don't understand the process. Can someone show me the steps?",
                    AuthorId = 3,
                    AuthorName = "Emma K.",
                    Subject = "Chemistry",
                    Grade = "10",
                    Topic = "Chemical Equations",
                    Upvotes = 18,
                    Downvotes = 0,
                    ReplyCount = 6,
                    CreatedAt = DateTime.Now.AddHours(-1),
                    IsResolved = false
                },
                new Discussion
                {
                    Id = 4,
                    Title = "English: Essay Writing Tips",
                    Content = "What are some good strategies for writing a persuasive essay? I have to write one about environmental protection and I want to make it really compelling.",
                    AuthorId = 4,
                    AuthorName = "Alex R.",
                    Subject = "English",
                    Grade = "7",
                    Topic = "Essay Writing",
                    Upvotes = 12,
                    Downvotes = 1,
                    ReplyCount = 9,
                    CreatedAt = DateTime.Now.AddMinutes(-45),
                    IsResolved = false
                }
            });

            // Add sample replies
            _replies.AddRange(new[]
            {
                new DiscussionReply
                {
                    Id = 1,
                    DiscussionId = 1,
                    Content = "The quadratic formula is x = (-b ± √(b²-4ac)) / 2a. The discriminant is b²-4ac. If it's positive, you get 2 real solutions. If it's zero, you get 1 solution. If it's negative, you get complex solutions.",
                    AuthorId = 5,
                    AuthorName = "Math Teacher",
                    Upvotes = 25,
                    Downvotes = 0,
                    CreatedAt = DateTime.Now.AddHours(-2),
                    IsAcceptedAnswer = true
                },
                new DiscussionReply
                {
                    Id = 2,
                    DiscussionId = 2,
                    Content = "Newton's First Law: Objects at rest stay at rest, objects in motion stay in motion (unless acted upon by a force). Newton's Second Law: F = ma (force equals mass times acceleration). Newton's Third Law: For every action, there's an equal and opposite reaction.",
                    AuthorId = 6,
                    AuthorName = "Physics Pro",
                    Upvotes = 30,
                    Downvotes = 1,
                    CreatedAt = DateTime.Now.AddHours(-4),
                    IsAcceptedAnswer = true
                }
            });
        }

        public async Task<List<DiscussionViewModel>> GetDiscussionsAsync(string? subject = null, string? grade = null, string? sortBy = "recent")
        {
            var discussions = _discussions.AsQueryable();

            // Filter by subject
            if (!string.IsNullOrEmpty(subject) && subject != "All")
            {
                discussions = discussions.Where(d => d.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by grade
            if (!string.IsNullOrEmpty(grade) && grade != "All")
            {
                discussions = discussions.Where(d => d.Grade == grade);
            }

            // Sort discussions
            discussions = sortBy?.ToLower() switch
            {
                "popular" => discussions.OrderByDescending(d => d.Upvotes - d.Downvotes),
                "replies" => discussions.OrderByDescending(d => d.ReplyCount),
                "resolved" => discussions.OrderByDescending(d => d.IsResolved).ThenByDescending(d => d.CreatedAt),
                _ => discussions.OrderByDescending(d => d.CreatedAt) // recent (default)
            };

            var currentUserId = Preferences.Get("UserId", 0);
            
            return discussions.Select(d => new DiscussionViewModel
            {
                Id = d.Id,
                Title = d.Title,
                Content = d.Content,
                ImageUrl = d.ImageUrl,
                AuthorId = d.AuthorId,
                AuthorName = d.AuthorName,
                Subject = d.Subject,
                Grade = d.Grade,
                Topic = d.Topic,
                Upvotes = d.Upvotes,
                Downvotes = d.Downvotes,
                ReplyCount = d.ReplyCount,
                CreatedAt = d.CreatedAt,
                IsResolved = d.IsResolved,
                IsPinned = d.IsPinned,
                HasUserUpvoted = _votes.Any(v => v.UserId == currentUserId && v.DiscussionId == d.Id && v.VoteType == VoteType.Upvote),
                HasUserDownvoted = _votes.Any(v => v.UserId == currentUserId && v.DiscussionId == d.Id && v.VoteType == VoteType.Downvote)
            }).ToList();
        }

        public async Task<DiscussionViewModel?> GetDiscussionAsync(int discussionId)
        {
            var discussion = _discussions.FirstOrDefault(d => d.Id == discussionId);
            if (discussion == null) return null;

            var currentUserId = Preferences.Get("UserId", 0);

            return new DiscussionViewModel
            {
                Id = discussion.Id,
                Title = discussion.Title,
                Content = discussion.Content,
                ImageUrl = discussion.ImageUrl,
                AuthorId = discussion.AuthorId,
                AuthorName = discussion.AuthorName,
                Subject = discussion.Subject,
                Grade = discussion.Grade,
                Topic = discussion.Topic,
                Upvotes = discussion.Upvotes,
                Downvotes = discussion.Downvotes,
                ReplyCount = discussion.ReplyCount,
                CreatedAt = discussion.CreatedAt,
                IsResolved = discussion.IsResolved,
                IsPinned = discussion.IsPinned,
                HasUserUpvoted = _votes.Any(v => v.UserId == currentUserId && v.DiscussionId == discussion.Id && v.VoteType == VoteType.Upvote),
                HasUserDownvoted = _votes.Any(v => v.UserId == currentUserId && v.DiscussionId == discussion.Id && v.VoteType == VoteType.Downvote)
            };
        }

        public async Task<List<DiscussionReplyViewModel>> GetRepliesAsync(int discussionId)
        {
            var replies = _replies.Where(r => r.DiscussionId == discussionId)
                                 .OrderByDescending(r => r.IsAcceptedAnswer)
                                 .ThenByDescending(r => r.Upvotes - r.Downvotes)
                                 .ThenBy(r => r.CreatedAt);

            var currentUserId = Preferences.Get("UserId", 0);

            return replies.Select(r => new DiscussionReplyViewModel
            {
                Id = r.Id,
                DiscussionId = r.DiscussionId,
                Content = r.Content,
                ImageUrl = r.ImageUrl,
                AuthorId = r.AuthorId,
                AuthorName = r.AuthorName,
                Upvotes = r.Upvotes,
                Downvotes = r.Downvotes,
                CreatedAt = r.CreatedAt,
                IsAcceptedAnswer = r.IsAcceptedAnswer,
                HasUserUpvoted = _votes.Any(v => v.UserId == currentUserId && v.ReplyId == r.Id && v.VoteType == VoteType.Upvote),
                HasUserDownvoted = _votes.Any(v => v.UserId == currentUserId && v.ReplyId == r.Id && v.VoteType == VoteType.Downvote)
            }).ToList();
        }

        public async Task<int> CreateDiscussionAsync(string title, string content, string? imageUrl, string subject, string grade, string topic)
        {
            var currentUserId = Preferences.Get("UserId", 0);
            var currentUser = await _databaseService.GetUserAsync(currentUserId);
            
            var discussion = new Discussion
            {
                Id = _discussions.Count + 1,
                Title = title,
                Content = content,
                ImageUrl = imageUrl,
                AuthorId = currentUserId,
                AuthorName = currentUser?.Name ?? "Anonymous",
                Subject = subject,
                Grade = grade,
                Topic = topic,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _discussions.Add(discussion);
            return discussion.Id;
        }

        public async Task<int> CreateReplyAsync(int discussionId, string content, string? imageUrl)
        {
            var currentUserId = Preferences.Get("UserId", 0);
            var currentUser = await _databaseService.GetUserAsync(currentUserId);
            
            var reply = new DiscussionReply
            {
                Id = _replies.Count + 1,
                DiscussionId = discussionId,
                Content = content,
                ImageUrl = imageUrl,
                AuthorId = currentUserId,
                AuthorName = currentUser?.Name ?? "Anonymous",
                CreatedAt = DateTime.Now
            };

            _replies.Add(reply);

            // Update reply count
            var discussion = _discussions.FirstOrDefault(d => d.Id == discussionId);
            if (discussion != null)
            {
                discussion.ReplyCount++;
                discussion.UpdatedAt = DateTime.Now;
            }

            return reply.Id;
        }

        public async Task<bool> VoteDiscussionAsync(int discussionId, VoteType voteType)
        {
            var currentUserId = Preferences.Get("UserId", 0);
            if (currentUserId == 0) return false;

            // Remove existing vote if any
            var existingVote = _votes.FirstOrDefault(v => v.UserId == currentUserId && v.DiscussionId == discussionId);
            if (existingVote != null)
            {
                _votes.Remove(existingVote);
                
                // Update vote counts
                var discussion = _discussions.FirstOrDefault(d => d.Id == discussionId);
                if (discussion != null)
                {
                    if (existingVote.VoteType == VoteType.Upvote)
                        discussion.Upvotes--;
                    else
                        discussion.Downvotes--;
                }

                // If same vote type, just remove (toggle off)
                if (existingVote.VoteType == voteType)
                    return true;
            }

            // Add new vote
            var newVote = new DiscussionVote
            {
                Id = _votes.Count + 1,
                UserId = currentUserId,
                DiscussionId = discussionId,
                VoteType = voteType,
                CreatedAt = DateTime.Now
            };

            _votes.Add(newVote);

            // Update vote counts
            var targetDiscussion = _discussions.FirstOrDefault(d => d.Id == discussionId);
            if (targetDiscussion != null)
            {
                if (voteType == VoteType.Upvote)
                    targetDiscussion.Upvotes++;
                else
                    targetDiscussion.Downvotes++;
            }

            return true;
        }

        public async Task<bool> VoteReplyAsync(int replyId, VoteType voteType)
        {
            var currentUserId = Preferences.Get("UserId", 0);
            if (currentUserId == 0) return false;

            // Remove existing vote if any
            var existingVote = _votes.FirstOrDefault(v => v.UserId == currentUserId && v.ReplyId == replyId);
            if (existingVote != null)
            {
                _votes.Remove(existingVote);
                
                // Update vote counts
                var reply = _replies.FirstOrDefault(r => r.Id == replyId);
                if (reply != null)
                {
                    if (existingVote.VoteType == VoteType.Upvote)
                        reply.Upvotes--;
                    else
                        reply.Downvotes--;
                }

                // If same vote type, just remove (toggle off)
                if (existingVote.VoteType == voteType)
                    return true;
            }

            // Add new vote
            var newVote = new DiscussionVote
            {
                Id = _votes.Count + 1,
                UserId = currentUserId,
                ReplyId = replyId,
                VoteType = voteType,
                CreatedAt = DateTime.Now
            };

            _votes.Add(newVote);

            // Update vote counts
            var targetReply = _replies.FirstOrDefault(r => r.Id == replyId);
            if (targetReply != null)
            {
                if (voteType == VoteType.Upvote)
                    targetReply.Upvotes++;
                else
                    targetReply.Downvotes++;
            }

            return true;
        }

        public async Task<bool> MarkReplyAsAcceptedAsync(int replyId, int discussionId)
        {
            var currentUserId = Preferences.Get("UserId", 0);
            var discussion = _discussions.FirstOrDefault(d => d.Id == discussionId);
            
            // Only discussion author can mark answers as accepted
            if (discussion == null || discussion.AuthorId != currentUserId)
                return false;

            // Remove accepted status from other replies
            foreach (var reply in _replies.Where(r => r.DiscussionId == discussionId))
            {
                reply.IsAcceptedAnswer = false;
            }

            // Mark this reply as accepted
            var targetReply = _replies.FirstOrDefault(r => r.Id == replyId);
            if (targetReply != null)
            {
                targetReply.IsAcceptedAnswer = true;
                discussion.IsResolved = true;
                return true;
            }

            return false;
        }

        public async Task<List<string>> GetSubjectsAsync()
        {
            var subjects = _discussions.Select(d => d.Subject).Distinct().Where(s => !string.IsNullOrEmpty(s)).ToList();
            subjects.Insert(0, "All");
            return subjects;
        }

        public async Task<List<string>> GetGradesAsync()
        {
            var grades = _discussions.Select(d => d.Grade).Distinct().Where(g => !string.IsNullOrEmpty(g)).OrderBy(g => g).ToList();
            grades.Insert(0, "All");
            return grades;
        }
    }
}
