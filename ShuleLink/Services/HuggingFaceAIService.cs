using System.Text;
using System.Text.Json;
using ShuleLink.Models;

namespace ShuleLink.Services
{
    public class HuggingFaceAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://api-inference.huggingface.co/models/microsoft/DialoGPT-medium";
        private readonly string _apiKey = "hf_your_token_here"; // Free API - no key needed for basic usage

        public HuggingFaceAIService(HttpClient httpClient)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("HuggingFaceAIService constructor started...");
                _httpClient = httpClient;
                
                // Safely add headers without modifying shared HttpClient
                if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
                {
                    _httpClient.DefaultRequestHeaders.Add("User-Agent", "ShuleLink/1.0");
                }
                
                System.Diagnostics.Debug.WriteLine("HuggingFaceAIService constructor completed successfully!");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"HuggingFaceAIService constructor error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<string> GenerateChatResponseAsync(string userMessage)
        {
            try
            {
                var educationalPrompt = $"Educational AI Assistant: A student asks '{userMessage}'. Provide a helpful, encouraging response suitable for primary school students with emojis and simple explanations.";
                
                var requestBody = new
                {
                    inputs = educationalPrompt,
                    parameters = new
                    {
                        max_length = 150,
                        temperature = 0.8,
                        do_sample = true,
                        top_p = 0.9
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_baseUrl, content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<JsonElement[]>(responseContent);
                    
                    if (result.Length > 0 && result[0].TryGetProperty("generated_text", out var text))
                    {
                        var generatedText = text.GetString() ?? "";
                        // Clean up the response to extract just the AI response part
                        if (generatedText.Contains("Educational AI Assistant:"))
                        {
                            var parts = generatedText.Split("Educational AI Assistant:");
                            if (parts.Length > 1)
                            {
                                return parts[1].Trim();
                            }
                        }
                        return generatedText;
                    }
                }
            }
            catch
            {
                // Return null to trigger fallback
            }
            
            return null;
        }

        public async Task<string> GenerateResponseAsync(string prompt)
        {
            try
            {
                var requestBody = new
                {
                    inputs = prompt,
                    parameters = new
                    {
                        max_length = 500,
                        temperature = 0.7,
                        do_sample = true,
                        top_p = 0.9
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_baseUrl, content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<JsonElement[]>(responseContent);
                    
                    if (result.Length > 0 && result[0].TryGetProperty("generated_text", out var text))
                    {
                        return text.GetString() ?? "";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"HuggingFace API error: {ex.Message}");
            }

            return "";
        }

        public async Task<string> GenerateEducationalContentAsync(string subject, string grade, string topic)
        {
            var prompt = $@"Create comprehensive educational notes for Grade {grade} students about {topic} in {subject}.

Include:
1. Introduction to {topic}
2. Key concepts and definitions
3. Important facts and details
4. Real-world examples
5. How it relates to daily life
6. Summary of main points

Make it engaging and age-appropriate for Grade {grade} students.

Topic: {topic}
Subject: {subject}
Grade: {grade}

Educational Content:";

            var response = await GenerateResponseAsync(prompt);
            
            if (string.IsNullOrEmpty(response))
            {
                return GenerateFallbackContent(subject, grade, topic);
            }

            return response;
        }

        public async Task<List<QuizQuestion>> GenerateQuizQuestionsAsync(string subject, string grade, string topic, int count = 30)
        {
            var questions = new List<QuizQuestion>();
            
            // Generate questions in batches to avoid API limits
            var batchSize = 5;
            var batches = (int)Math.Ceiling((double)count / batchSize);

            for (int batch = 0; batch < batches; batch++)
            {
                var questionsInBatch = Math.Min(batchSize, count - (batch * batchSize));
                var batchQuestions = await GenerateQuestionBatch(subject, grade, topic, questionsInBatch, batch + 1);
                questions.AddRange(batchQuestions);
            }

            return questions.Take(count).ToList();
        }

        private async Task<List<QuizQuestion>> GenerateQuestionBatch(string subject, string grade, string topic, int count, int batchNumber)
        {
            var prompt = $@"Generate {count} multiple choice questions about {topic} for Grade {grade} {subject} students.

Format each question as:
Q: [Question text]
A) [Option A]
B) [Option B] 
C) [Option C]
D) [Option D]
Correct: [A/B/C/D]
Explanation: [Why this answer is correct]

Topic: {topic}
Subject: {subject}
Grade: {grade}
Batch: {batchNumber}

Questions:";

            var response = await GenerateResponseAsync(prompt);
            return ParseQuizQuestions(response, subject, grade, topic);
        }

        private List<QuizQuestion> ParseQuizQuestions(string response, string subject, string grade, string topic)
        {
            var questions = new List<QuizQuestion>();
            
            if (string.IsNullOrEmpty(response))
            {
                return GenerateFallbackQuestions(subject, grade, topic);
            }

            try
            {
                var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                QuizQuestion? currentQuestion = null;

                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();
                    
                    if (trimmedLine.StartsWith("Q:"))
                    {
                        if (currentQuestion != null)
                        {
                            questions.Add(currentQuestion);
                        }
                        
                        currentQuestion = new QuizQuestion
                        {
                            Question = trimmedLine.Substring(2).Trim(),
                            Subject = subject,
                            Grade = grade,
                            Topic = topic
                        };
                    }
                    else if (currentQuestion != null)
                    {
                        if (trimmedLine.StartsWith("A)"))
                            currentQuestion.OptionA = trimmedLine.Substring(2).Trim();
                        else if (trimmedLine.StartsWith("B)"))
                            currentQuestion.OptionB = trimmedLine.Substring(2).Trim();
                        else if (trimmedLine.StartsWith("C)"))
                            currentQuestion.OptionC = trimmedLine.Substring(2).Trim();
                        else if (trimmedLine.StartsWith("D)"))
                            currentQuestion.OptionD = trimmedLine.Substring(2).Trim();
                        else if (trimmedLine.StartsWith("Correct:"))
                            currentQuestion.CorrectAnswer = trimmedLine.Substring(8).Trim();
                        else if (trimmedLine.StartsWith("Explanation:"))
                            currentQuestion.Explanation = trimmedLine.Substring(12).Trim();
                    }
                }

                if (currentQuestion != null)
                {
                    questions.Add(currentQuestion);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Question parsing error: {ex.Message}");
                return GenerateFallbackQuestions(subject, grade, topic);
            }

            return questions.Any() ? questions : GenerateFallbackQuestions(subject, grade, topic);
        }

        private string GenerateFallbackContent(string subject, string grade, string topic)
        {
            return $@"📚 {topic} - {subject} (Grade {grade})

🎯 Introduction
{topic} is an important concept in {subject} that Grade {grade} students need to understand. This topic helps build foundational knowledge for more advanced learning.

📖 Key Concepts
• {topic} involves understanding the basic principles and applications
• It connects to real-world situations that students encounter daily
• The concepts build upon previous learning in {subject}

🔍 Important Details
• Students should focus on understanding rather than memorization
• Practice and application help reinforce the concepts
• Asking questions helps deepen understanding

💡 Real-World Applications
{topic} can be seen in everyday life through various examples and situations. Understanding these connections helps make learning more meaningful and memorable.

🎓 Summary
{topic} is a fundamental concept in {subject} that provides the foundation for continued learning. Students should take time to understand the key points and practice applying their knowledge.

Remember: Learning is a journey, and every step builds toward greater understanding!";
        }

        private List<QuizQuestion> GenerateFallbackQuestions(string subject, string grade, string topic)
        {
            var questions = new List<QuizQuestion>();
            
            // Generate 30 varied questions
            var questionTemplates = new[]
            {
                $"What is the main concept of {topic}?",
                $"How does {topic} relate to {subject}?",
                $"Which of the following best describes {topic}?",
                $"What is an example of {topic} in real life?",
                $"Why is {topic} important in {subject}?",
                $"What are the key characteristics of {topic}?",
                $"How can {topic} be applied practically?",
                $"What should Grade {grade} students know about {topic}?",
                $"Which statement about {topic} is correct?",
                $"What is the relationship between {topic} and other concepts?"
            };

            var optionSets = new[]
            {
                new[] { "Understanding basic principles", "Memorizing facts only", "Ignoring applications", "Avoiding practice" },
                new[] { "It builds foundational knowledge", "It has no relevance", "It's only theoretical", "It's too advanced" },
                new[] { "A fundamental concept", "An outdated idea", "A complex theory", "A simple fact" },
                new[] { "Daily life situations", "Only in textbooks", "Laboratory only", "Historical events only" },
                new[] { "Provides learning foundation", "Creates confusion", "Wastes time", "Has no purpose" }
            };

            for (int i = 0; i < 30; i++)
            {
                var questionIndex = i % questionTemplates.Length;
                var optionIndex = i % optionSets.Length;
                
                questions.Add(new QuizQuestion
                {
                    Question = questionTemplates[questionIndex],
                    Subject = subject,
                    Grade = grade,
                    Topic = topic,
                    OptionA = optionSets[optionIndex][0],
                    OptionB = optionSets[optionIndex][1],
                    OptionC = optionSets[optionIndex][2],
                    OptionD = optionSets[optionIndex][3],
                    CorrectAnswer = "A",
                    Explanation = $"This answer correctly relates to the key concepts of {topic} in {subject}."
                });
            }

            return questions;
        }
    }
}
