using System.Text;
using System.Text.Json;
using ShuleLink.Models;

namespace ShuleLink.Services
{
    public class GeminiAIService
    {
        private readonly HttpClient _httpClient;
        private readonly HuggingFaceAIService _huggingFaceService;
        private readonly OfflineAIService _offlineService;
        private readonly string _apiKey = "AIzaSyDu3TiD8PEli4Snfx4UlH04PNfMKCtsVds";
        private readonly string _baseUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";

        public GeminiAIService(HttpClient httpClient)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("GeminiAIService constructor started...");
                _httpClient = httpClient;
                
                System.Diagnostics.Debug.WriteLine("Creating HuggingFaceAIService...");
                _huggingFaceService = new HuggingFaceAIService(httpClient); // Use same HttpClient
                System.Diagnostics.Debug.WriteLine("HuggingFaceAIService created successfully!");
                
                System.Diagnostics.Debug.WriteLine("Creating OfflineAIService...");
                _offlineService = new OfflineAIService();
                System.Diagnostics.Debug.WriteLine("OfflineAIService created successfully!");
                
                System.Diagnostics.Debug.WriteLine("GeminiAIService constructor completed successfully!");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GeminiAIService constructor error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<string> GenerateDailyQuoteAsync()
        {
            try
            {
                var prompt = @"Generate an inspiring and educational quote suitable for primary school students (grades 1-7). 
                The quote should be motivational, age-appropriate, and encourage learning, curiosity, or personal growth. 
                Format: 'Quote text' - Author Name
                Keep it simple and positive.";

                var response = await CallGeminiAPI(prompt);
                return response ?? "Education is the most powerful weapon which you can use to change the world. - Nelson Mandela";
            }
            catch
            {
                // Fallback quotes for offline mode
                var fallbackQuotes = new[]
                {
                    "The more that you read, the more things you will know. The more that you learn, the more places you'll go. - Dr. Seuss",
                    "Education is the most powerful weapon which you can use to change the world. - Nelson Mandela",
                    "Learning never exhausts the mind. - Leonardo da Vinci",
                    "The beautiful thing about learning is that no one can take it away from you. - B.B. King"
                };
                
                var random = new Random();
                return fallbackQuotes[random.Next(fallbackQuotes.Length)];
            }
        }

        public async Task<string> GenerateChatResponseAsync(string userMessage, string context = "")
        {
            try
            {
                var prompt = $@"You are ShuleLink AI, an intelligent educational assistant for primary school students (grades 1-7). 

                IMPORTANT: You must NEVER repeat or echo the user's message. Always provide a unique, helpful response.
                
                User message: '{userMessage}'
                Previous context: {context}
                
                Provide a helpful, engaging, and age-appropriate response that:
                1. NEVER echoes or repeats the user's exact words
                2. Directly addresses the user's message/question with NEW information
                3. Is educational and encouraging
                4. Uses simple language suitable for young learners
                5. Includes relevant emojis to make it engaging
                6. Asks follow-up questions to encourage learning
                7. Provides specific examples, explanations, or guidance
                
                Response Guidelines:
                - If user says 'Hi/Hello': Greet warmly and ask about their learning goals
                - If user asks questions: Provide clear explanations with examples
                - If user needs help: Break down concepts into simple steps
                - If user shares work: Give constructive feedback and encouragement
                - Always add educational value beyond just acknowledging their message
                
                Keep responses conversational, supportive, and informative. Maximum 200 words.
                NEVER just repeat what they said - always add new value and information.";

                var response = await CallGeminiAPI(prompt);
                
                if (!string.IsNullOrEmpty(response) && response.Length > 15 && !response.ToLower().Contains(userMessage.ToLower()))
                {
                    return response;
                }

                // Fallback to HuggingFace
                var huggingFaceResponse = await _huggingFaceService.GenerateChatResponseAsync(userMessage);
                if (!string.IsNullOrEmpty(huggingFaceResponse) && !huggingFaceResponse.ToLower().Contains(userMessage.ToLower()))
                {
                    return huggingFaceResponse;
                }

                // Final fallback to offline service
                return _offlineService.GenerateChatResponse(userMessage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Chat response error: {ex.Message}");
                // Fallback to offline service
                return _offlineService.GenerateChatResponse(userMessage);
            }
        }

        public async Task<string> GenerateComprehensiveTopicNotesAsync(string subject, string grade, string topic)
        {
            try
            {
                // Enhanced prompt for comprehensive content
                var prompt = $@"Generate comprehensive educational content for Grade {grade} students on '{topic}' in {subject}.

                Create detailed, scrollable content that covers the topic from 0-100% understanding:

                📚 INTRODUCTION & OVERVIEW
                - What is {topic}? (Simple definition)
                - Why is it important to learn about {topic}?
                - How does {topic} connect to other subjects?

                🔍 KEY CONCEPTS & DEFINITIONS
                - Essential vocabulary and terms
                - Core principles and laws
                - Important formulas or rules (if applicable)

                📖 DETAILED EXPLANATION
                - Step-by-step breakdown of how {topic} works
                - Multiple examples with explanations
                - Common misconceptions and clarifications
                - Visual descriptions (describe diagrams/charts)

                🌍 REAL-WORLD APPLICATIONS
                - Where we encounter {topic} in daily life
                - Career connections and practical uses
                - Historical context and discoveries
                - Current research and developments

                🧪 ACTIVITIES & EXPERIMENTS
                - Simple experiments students can try
                - Observation activities
                - Discussion questions
                - Problem-solving exercises

                💡 FASCINATING FACTS & DISCOVERIES
                - Amazing facts that will surprise students
                - Important scientists/mathematicians involved
                - Recent discoveries or innovations
                - Fun trivia and interesting statistics

                📊 EXAMPLES & CASE STUDIES
                - Detailed worked examples
                - Step-by-step problem solving
                - Different scenarios and applications
                - Comparison with similar concepts

                🎯 SUMMARY & KEY TAKEAWAYS
                - Essential points to remember
                - How this connects to future learning
                - Review questions for self-assessment

                📚 ADDITIONAL RESOURCES
                - Suggest related topics to explore
                - Recommend age-appropriate books/websites
                - Educational videos or documentaries
                - Interactive online tools

                Make this comprehensive enough that a student could learn the entire topic from this content alone. Use engaging language, emojis, and clear formatting. Include plenty of examples and make it interactive where possible.";

                var response = await CallGeminiAPI(prompt);
                
                if (!string.IsNullOrEmpty(response) && response.Length > 500)
                {
                    return response;
                }

                // Fallback to HuggingFace API
                var huggingFaceResponse = await _huggingFaceService.GenerateEducationalContentAsync(subject, grade, topic);
                
                if (!string.IsNullOrEmpty(huggingFaceResponse) && huggingFaceResponse.Length > 500)
                {
                    return huggingFaceResponse;
                }

                // Fallback to offline comprehensive content
                return _offlineService.GenerateComprehensiveContent(subject, grade, topic);
            }
            catch
            {
                return _offlineService.GenerateComprehensiveContent(subject, grade, topic);
            }
        }

        public async Task<string> GenerateTopicNotesAsync(string subject, string grade, string topic)
        {
            try
            {
                // First try Gemini API
                var prompt = $@"Generate comprehensive educational notes for Grade {grade} students on the topic '{topic}' in {subject} subject.

                The notes should include:
                
                📚 INTRODUCTION
                - What is {topic}?
                - Why is it important?
                
                🔍 KEY CONCEPTS
                - Main definitions and terms
                - Important principles
                
                📖 DETAILED EXPLANATION
                - How {topic} works
                - Step-by-step breakdown
                - Important facts and details
                
                🌍 REAL-WORLD EXAMPLES
                - Where we see {topic} in daily life
                - Practical applications
                
                💡 INTERESTING FACTS
                - Fun facts about {topic}
                - Amazing discoveries
                
                🎯 SUMMARY
                - Key takeaways
                - What students should remember
                
                Make it engaging, educational, and age-appropriate for Grade {grade} students. Use emojis and clear formatting.";

                var response = await CallGeminiAPI(prompt);
                
                if (!string.IsNullOrEmpty(response) && response.Length > 200)
                {
                    return response;
                }

                // Fallback to HuggingFace API
                var huggingFaceResponse = await _huggingFaceService.GenerateEducationalContentAsync(subject, grade, topic);
                
                if (!string.IsNullOrEmpty(huggingFaceResponse) && huggingFaceResponse.Length > 200)
                {
                    return huggingFaceResponse;
                }

                // Final fallback to offline service
                return _offlineService.GenerateEducationalContent(subject, grade, topic);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Topic notes generation error: {ex.Message}");
                return GetFallbackNotes(subject, grade, topic);
            }
        }

        private string GetFallbackNotes(string subject, string grade, string topic)
        {
            return $@"📚 {topic}

Welcome to your {subject} lesson on {topic}!

🎯 Learning Objectives:
• Understand the key concepts of {topic}
• Learn how {topic} applies to everyday life
• Develop critical thinking about {subject}

📖 Introduction:
{topic} is an important topic in {subject} that helps us understand the world around us. This topic is designed for Grade {grade} students to explore and learn.

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

        public async Task<List<QuizQuestion>> GenerateQuizQuestionsAsync(string subject, string grade, string topic, int count = 30)
        {
            try
            {
                var allQuestions = new List<QuizQuestion>();
                
                // Generate questions in batches to ensure variety and avoid API limits
                var batchSize = 10;
                var batches = (int)Math.Ceiling((double)count / batchSize);

                for (int batch = 0; batch < batches; batch++)
                {
                    var questionsInBatch = Math.Min(batchSize, count - allQuestions.Count);
                    var batchQuestions = await GenerateQuestionBatch(subject, grade, topic, questionsInBatch, batch + 1);
                    allQuestions.AddRange(batchQuestions);
                    
                    if (allQuestions.Count >= count) break;
                }

                // If we don't have enough questions, try HuggingFace as backup
                if (allQuestions.Count < count)
                {
                    var remainingCount = count - allQuestions.Count;
                    var backupQuestions = await _huggingFaceService.GenerateQuizQuestionsAsync(subject, grade, topic, remainingCount);
                    allQuestions.AddRange(backupQuestions);
                }

                // If still not enough, use offline service
                if (allQuestions.Count < count)
                {
                    var offlineQuestions = _offlineService.GenerateQuizQuestions(subject, grade, topic, count - allQuestions.Count);
                    allQuestions.AddRange(offlineQuestions);
                }

                return allQuestions.Take(count).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Quiz generation error: {ex.Message}");
                return GetFallbackQuestions(subject, grade, topic, count);
            }
        }

        private async Task<List<QuizQuestion>> GenerateQuestionBatch(string subject, string grade, string topic, int count, int batchNumber)
        {
            var questionTypes = new[] { "multiple_choice", "fill_blank", "true_false", "calculation", "short_answer" };
            var selectedType = questionTypes[new Random().Next(questionTypes.Length)];

            // For Math, Physics, Chemistry - prioritize calculation questions
            if ((subject.ToLower().Contains("math") || subject.ToLower().Contains("physics") || 
                 subject.ToLower().Contains("chemistry") || subject.ToLower().Contains("science")) && 
                new Random().Next(100) < 60) // 60% chance for calculation questions in these subjects
            {
                selectedType = "calculation";
            }

            string prompt;

            if (selectedType == "calculation")
            {
                prompt = $@"Generate {count} practical calculation questions about {topic} for Grade {grade} {subject} students.

                Create questions that require mathematical calculations with step-by-step solutions.
                Include real-world scenarios and practical applications.
                Batch {batchNumber} - ensure questions test different calculation skills.

                For Math: Include arithmetic, geometry, fractions, decimals, word problems
                For Physics: Include motion, forces, energy, simple machines
                For Chemistry: Include basic reactions, measurements, mixtures
                For Science: Include measurements, data analysis, simple experiments

                Format as JSON array:
                [
                    {{
                        ""question"": ""A rectangle has a length of 8 meters and width of 5 meters. What is its area?"",
                        ""optionA"": ""40 square meters"",
                        ""optionB"": ""26 square meters"",
                        ""optionC"": ""13 square meters"",
                        ""optionD"": ""45 square meters"",
                        ""correctAnswer"": ""A"",
                        ""explanation"": ""To find the area of a rectangle, multiply length × width."",
                        ""calculationSteps"": ""Step 1: Identify the formula: Area = length × width\nStep 2: Substitute values: Area = 8m × 5m\nStep 3: Calculate: Area = 40 square meters"",
                        ""formula"": ""Area = length × width"",
                        ""units"": ""square meters"",
                        ""questionType"": ""calculation"",
                        ""showWorkRequired"": true
                    }}
                ]

                Include detailed step-by-step solutions in calculationSteps field.
                Make questions age-appropriate for Grade {grade} level.";
            }
            else if (selectedType == "fill_blank")
            {
                prompt = $@"Generate {count} fill-in-the-blank questions about {topic} for Grade {grade} {subject} students.

                Create questions where students need to fill in missing words, numbers, or formulas.
                Batch {batchNumber} - ensure questions are unique and educational.

                Format as JSON array:
                [
                    {{
                        ""question"": ""The formula for area of a rectangle is length × ___"",
                        ""optionA"": ""width"",
                        ""optionB"": ""height"",
                        ""optionC"": ""perimeter"",
                        ""optionD"": ""diagonal"",
                        ""correctAnswer"": ""A"",
                        ""explanation"": ""Area = length × width. Width is the correct term for the second dimension."",
                        ""blankAnswer"": ""width"",
                        ""questionType"": ""fill_blank""
                    }}
                ]

                Make the blanks test key concepts, formulas, or vocabulary about {topic}.";
            }
            else if (selectedType == "true_false")
            {
                prompt = $@"Generate {count} true/false questions about {topic} for Grade {grade} {subject} students.

                Create clear statements that are either true or false.
                Include both obviously true/false and more challenging statements.
                Batch {batchNumber} - ensure questions test different concepts.

                Format as JSON array:
                [
                    {{
                        ""question"": ""The sun is a star. True or False?"",
                        ""optionA"": ""True"",
                        ""optionB"": ""False"",
                        ""optionC"": """",
                        ""optionD"": """",
                        ""correctAnswer"": ""A"",
                        ""explanation"": ""The sun is indeed a star - it's our closest star and provides light and heat through nuclear fusion."",
                        ""questionType"": ""true_false""
                    }}
                ]

                Provide detailed explanations for why the statement is true or false.";
            }
            else
            {
                // Multiple choice with randomized correct answers
                var correctAnswers = new[] { "A", "B", "C", "D" };
                var randomCorrect = correctAnswers[new Random().Next(correctAnswers.Length)];

                prompt = $@"Generate {count} multiple choice questions about {topic} for Grade {grade} {subject} students.

                IMPORTANT: Mix up the correct answers - don't make them all 'A'. 
                Make sure the correct answer for each question is randomly distributed among A, B, C, D.
                Batch {batchNumber} - ensure questions are unique and cover different aspects of {topic}.

                For practical subjects (Math, Physics, Chemistry), include:
                - Real-world applications
                - Problem-solving scenarios
                - Conceptual understanding questions
                - Formula and calculation-based questions

                Format as JSON array:
                [
                    {{
                        ""question"": ""What is the main characteristic of {topic}?"",
                        ""optionA"": ""First option"",
                        ""optionB"": ""Second option"",
                        ""optionC"": ""Third option"",
                        ""optionD"": ""Fourth option"",
                        ""correctAnswer"": ""{randomCorrect}"",
                        ""explanation"": ""Detailed explanation of why this is correct and why other options are wrong"",
                        ""questionType"": ""multiple_choice""
                    }}
                ]

                Ensure correct answers are distributed across A, B, C, D options randomly.
                Make distractors (wrong answers) plausible but clearly incorrect.
                Topic: {topic}
                Subject: {subject}
                Grade: {grade}";
            }

            var response = await CallGeminiAPI(prompt);
            return ParseQuizQuestions(response, subject, grade, topic);
        }

        public async Task<bool> CheckAnswerAsync(string question, string userAnswer, string correctAnswer)
        {
            try
            {
                if (userAnswer.Trim().ToUpper() == correctAnswer.Trim().ToUpper())
                    return true;

                // For essay/short answer questions, use AI to check
                var prompt = $@"Question: {question}
                Correct Answer: {correctAnswer}
                User Answer: {userAnswer}
                
                Is the user's answer correct or acceptable? Consider partial credit for close answers.
                Respond with only 'TRUE' or 'FALSE'.";

                var response = await CallGeminiAPI(prompt);
                return response?.ToUpper().Contains("TRUE") == true;
            }
            catch
            {
                return userAnswer.Trim().ToUpper() == correctAnswer.Trim().ToUpper();
            }
        }

        public async Task<string?> CallGeminiAPI(string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}?key={_apiKey}", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
                
                if (result.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                {
                    var firstCandidate = candidates[0];
                    if (firstCandidate.TryGetProperty("content", out var contentObj) &&
                        contentObj.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                    {
                        var firstPart = parts[0];
                        if (firstPart.TryGetProperty("text", out var text))
                        {
                            return text.GetString();
                        }
                    }
                }
            }

            return null;
        }

        private List<QuizQuestion> ParseQuizQuestions(string? response, string subject, string grade, string topic)
        {
            var questions = new List<QuizQuestion>();
            
            if (string.IsNullOrEmpty(response))
                return GetFallbackQuestions(subject, grade, topic);

            try
            {
                // Try to parse JSON response
                var jsonQuestions = JsonSerializer.Deserialize<JsonElement[]>(response);
                
                foreach (var q in jsonQuestions)
                {
                    var question = new QuizQuestion
                    {
                        Question = q.GetProperty("question").GetString() ?? "",
                        Subject = subject,
                        Grade = grade,
                        Topic = topic,
                        OptionA = q.GetProperty("optionA").GetString() ?? "",
                        OptionB = q.GetProperty("optionB").GetString() ?? "",
                        OptionC = q.GetProperty("optionC").GetString() ?? "",
                        OptionD = q.GetProperty("optionD").GetString() ?? "",
                        CorrectAnswer = q.GetProperty("correctAnswer").GetString() ?? "",
                        Explanation = q.GetProperty("explanation").GetString() ?? ""
                    };

                    // Parse optional fields for enhanced question types
                    if (q.TryGetProperty("calculationSteps", out var calcSteps))
                        question.CalculationSteps = calcSteps.GetString() ?? "";
                    
                    if (q.TryGetProperty("blankAnswer", out var blankAns))
                        question.BlankAnswer = blankAns.GetString() ?? "";
                    
                    if (q.TryGetProperty("formula", out var formula))
                        question.Formula = formula.GetString() ?? "";
                    
                    if (q.TryGetProperty("units", out var units))
                        question.Units = units.GetString() ?? "";
                    
                    if (q.TryGetProperty("showWorkRequired", out var showWork))
                        question.ShowWorkRequired = showWork.GetBoolean();

                    // Set question type based on questionType field
                    if (q.TryGetProperty("questionType", out var qType))
                    {
                        var typeString = qType.GetString()?.ToLower();
                        question.Type = typeString switch
                        {
                            "calculation" => QuestionType.Calculation,
                            "fill_blank" => QuestionType.FillInTheBlank,
                            "true_false" => QuestionType.TrueFalse,
                            "short_answer" => QuestionType.ShortAnswer,
                            _ => QuestionType.MultipleChoice
                        };
                    }

                    questions.Add(question);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"JSON parsing error: {ex.Message}");
                return GetFallbackQuestions(subject, grade, topic);
            }

            return questions.Any() ? questions : GetFallbackQuestions(subject, grade, topic);
        }

        private List<QuizQuestion> GetFallbackQuestions(string subject, string grade, string topic, int count = 30)
        {
            var questions = new List<QuizQuestion>();
            
            var questionTemplates = new[]
            {
                $"What is the main concept of {topic}?",
                $"How does {topic} relate to {subject}?",
                $"Which of the following best describes {topic}?",
                $"What is an important characteristic of {topic}?",
                $"Why is {topic} significant in {subject}?",
                $"What can we learn from studying {topic}?",
                $"How is {topic} used in real life?",
                $"What should Grade {grade} students know about {topic}?",
                $"Which statement about {topic} is most accurate?",
                $"What is the relationship between {topic} and other concepts in {subject}?",
                $"How can understanding {topic} help students?",
                $"What makes {topic} important for Grade {grade} learning?",
                $"Which example best illustrates {topic}?",
                $"What is a key feature of {topic}?",
                $"How does {topic} connect to everyday experiences?"
            };

            var optionSets = new[]
            {
                new[] { "A fundamental concept that builds understanding", "An outdated idea with no relevance", "A complex theory beyond grade level", "A simple fact to memorize" },
                new[] { "It provides essential foundational knowledge", "It has no connection to the subject", "It only appears in advanced studies", "It contradicts other concepts" },
                new[] { "An important topic for student learning", "A minor detail in the curriculum", "An optional concept to skip", "A confusing idea to avoid" },
                new[] { "It helps students understand the world", "It creates unnecessary confusion", "It wastes valuable class time", "It has no practical application" },
                new[] { "It builds critical thinking skills", "It prevents student progress", "It complicates simple ideas", "It serves no educational purpose" },
                new[] { "Practical applications in daily life", "Only theoretical with no real use", "Limited to laboratory settings", "Relevant only to experts" },
                new[] { "Clear examples and demonstrations", "Abstract concepts without examples", "Complex formulas and equations", "Memorization of facts only" },
                new[] { "Interactive learning and exploration", "Passive listening without engagement", "Rote memorization techniques", "Avoiding hands-on activities" }
            };

            for (int i = 0; i < count; i++)
            {
                var questionIndex = i % questionTemplates.Length;
                var optionIndex = i % optionSets.Length;
                
                // Add some variation to make questions unique
                var questionVariation = i / questionTemplates.Length;
                var questionText = questionTemplates[questionIndex];
                
                if (questionVariation > 0)
                {
                    questionText = questionText.Replace($"{topic}", $"{topic} (Part {questionVariation + 1})");
                }
                
                questions.Add(new QuizQuestion
                {
                    Question = questionText,
                    Subject = subject,
                    Grade = grade,
                    Topic = topic,
                    OptionA = optionSets[optionIndex][0],
                    OptionB = optionSets[optionIndex][1],
                    OptionC = optionSets[optionIndex][2],
                    OptionD = optionSets[optionIndex][3],
                    CorrectAnswer = "A",
                    Explanation = $"This answer correctly identifies the key aspects of {topic} that are important for Grade {grade} students to understand in {subject}."
                });
            }

            return questions;
        }
    }
}
