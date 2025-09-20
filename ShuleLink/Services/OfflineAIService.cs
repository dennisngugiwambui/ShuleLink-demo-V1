using ShuleLink.Models;

namespace ShuleLink.Services
{
    public class OfflineAIService
    {
        private readonly Random _random = new Random();

        public string GenerateEducationalContent(string subject, string grade, string topic)
        {
            return $@"📚 {topic} - {subject} (Grade {grade})

🎯 INTRODUCTION
{topic} is a fascinating topic in {subject} that Grade {grade} students need to understand. This concept helps build a strong foundation for future learning and connects to many real-world applications.

🔍 KEY CONCEPTS
• {topic} involves understanding fundamental principles and their applications
• It connects to other important concepts in {subject}
• Students learn through observation, practice, and exploration
• The topic builds upon previous knowledge and prepares for advanced learning

📖 DETAILED EXPLANATION
{topic} works through a series of interconnected processes and principles. When we study {topic}, we discover how different elements work together to create the phenomena we observe.

Key points to understand:
• The basic structure and components involved
• How different parts interact with each other
• The role of {topic} in the broader context of {subject}
• Step-by-step processes that occur

🌍 REAL-WORLD EXAMPLES
We can see {topic} in action in our daily lives:
• Examples from nature and our environment
• Applications in technology and everyday objects
• How {topic} affects our daily experiences
• Connections to other subjects and areas of learning

💡 INTERESTING FACTS
• {topic} has been studied for many years by scientists and researchers
• New discoveries about {topic} continue to be made
• {topic} plays an important role in many modern technologies
• Understanding {topic} helps us make sense of the world around us

🎯 SUMMARY
{topic} is an essential concept in {subject} that helps Grade {grade} students understand how the world works. By studying {topic}, students develop critical thinking skills and build knowledge that will serve them throughout their educational journey.

Remember: Learning is a process, and every question you ask helps you understand better!";
        }

        public string GenerateComprehensiveContent(string subject, string grade, string topic)
        {
            return $@"📚 **{topic} - Complete Learning Guide for Grade {grade}**

🌟 **INTRODUCTION & OVERVIEW**
Welcome to your comprehensive guide on {topic}! This topic is an important part of {subject} that will help you understand the world around you better.

What is {topic}?
{topic} is a fundamental concept in {subject} that students your age should understand. It connects to many things you see and experience every day.

Why is it important?
Learning about {topic} helps you:
• Understand how things work in the real world
• Solve problems more effectively
• Connect ideas across different subjects
• Prepare for more advanced learning

🔍 **KEY CONCEPTS & DEFINITIONS**
Here are the most important terms you need to know:

• **Main Concept**: The central idea behind {topic}
• **Key Principles**: The basic rules that govern how {topic} works
• **Important Terms**: Vocabulary words that are essential for understanding

📖 **DETAILED EXPLANATION**
Let's break down {topic} step by step:

Step 1: Understanding the Basics
{topic} works by following certain patterns and rules. Think of it like a recipe - there are specific steps that always work.

Step 2: How It Works
The process involves several important parts working together. Each part has a specific job to do.

Step 3: Why It Matters
This concept appears in many different situations, making it very useful to understand.

🌍 **REAL-WORLD APPLICATIONS**
You can see {topic} in action in these everyday situations:

• At Home: Examples of how {topic} affects your daily life
• In Nature: How {topic} appears in the natural world
• In Technology: Ways that {topic} is used in modern devices
• In Other Subjects: Connections to math, science, history, and more

🧪 **ACTIVITIES & EXPERIMENTS**
Try these fun activities to explore {topic}:

Activity 1: Observation Exercise
Look around your environment and identify examples of {topic}. Keep a journal of what you find.

Activity 2: Simple Experiment
With adult supervision, try this safe experiment to see {topic} in action.

Activity 3: Discussion Questions
• What would happen if {topic} didn't exist?
• How does {topic} affect your daily life?
• Can you think of new ways to use {topic}?

💡 **FASCINATING FACTS & DISCOVERIES**
Here are some amazing facts about {topic}:

• Did you know that {topic} was first discovered/invented by...
• The most surprising thing about {topic} is...
• Recent scientists have found that {topic} can...
• In the future, {topic} might be used to...

📊 **EXAMPLES & CASE STUDIES**
Let's look at some detailed examples:

Example 1: Simple Scenario
Imagine you're [relevant scenario]. Here's how {topic} would work in this situation...

Example 2: More Complex Application
In a more advanced case, {topic} can be used to solve bigger problems...

🎯 **SUMMARY & KEY TAKEAWAYS**
The most important things to remember about {topic}:

✓ {topic} is essential for understanding {subject}
✓ It appears in many real-world situations
✓ Understanding {topic} helps you solve problems
✓ This knowledge will help you in future learning

📚 **ADDITIONAL RESOURCES**
Want to learn more? Check out these resources:

• Books: Look for age-appropriate books about {topic}
• Websites: Educational websites with interactive content
• Videos: Educational videos that explain {topic} visually
• Games: Online games that make learning {topic} fun

🤔 **REVIEW QUESTIONS**
Test your understanding:

1. What is the main idea behind {topic}?
2. Can you give three examples of {topic} in real life?
3. Why is {topic} important to learn about?
4. How does {topic} connect to other subjects?
5. What's the most interesting thing you learned about {topic}?

Remember: Learning is a journey, and every question you ask helps you understand better! Keep exploring and stay curious about {topic} and how it connects to everything around you.

🌟 Great job learning about {topic}! You're well on your way to mastering this important concept in {subject}.";
        }

        public string GenerateChatResponse(string userMessage)
        {
            var message = userMessage.ToLower().Trim();
            
            // Greeting responses
            if (message.Contains("hi") || message.Contains("hello") || message.Contains("hey") || message.Contains("good morning") || message.Contains("good afternoon"))
            {
                var greetingResponses = new[]
                {
                    "🌟 Hello there! I'm so excited to help you learn today! What subject would you like to explore? I can help with Math, Science, English, or any other topic you're curious about!",
                    "👋 Hi! Welcome to your learning adventure! I'm here to make learning fun and easy. What questions do you have for me today?",
                    "🎓 Hello, brilliant student! I'm your AI learning companion, ready to help you discover amazing things. What would you like to learn about?",
                    "✨ Hey there, future genius! I'm thrilled to be your study buddy today. What homework or topic can I help you with?"
                };
                return greetingResponses[_random.Next(greetingResponses.Length)];
            }
            
            // Math-related responses
            if (message.Contains("math") || message.Contains("number") || message.Contains("calculate") || message.Contains("add") || message.Contains("subtract") || message.Contains("multiply") || message.Contains("divide"))
            {
                var mathResponses = new[]
                {
                    "🔢 Awesome! Math is like a superpower that helps us solve real-world problems! Whether it's addition, subtraction, multiplication, or division, I'll help you master it step by step. What specific math concept are you working on?",
                    "📊 Math time! I love helping students discover the patterns and logic in numbers. Math is everywhere - from counting your allowance to measuring ingredients for cookies! What math problem can I help you solve?",
                    "🧮 Perfect! Mathematics is the language of the universe! Let's break down any math problem into simple, easy steps. Remember, every math wizard started with basic counting. What would you like to practice?",
                    "➕ Great choice! Math builds your brain like exercise builds your muscles. Whether you're learning basic operations or word problems, I'll make sure you understand every step. What's your math challenge today?"
                };
                return mathResponses[_random.Next(mathResponses.Length)];
            }
            
            // Science-related responses
            if (message.Contains("science") || message.Contains("experiment") || message.Contains("why") || message.Contains("how") || message.Contains("plant") || message.Contains("animal") || message.Contains("space"))
            {
                var scienceResponses = new[]
                {
                    "🔬 Science is AMAZING! It's all about discovering how our incredible world works! From tiny atoms to massive planets, from growing plants to flying rockets - science explains it all! What scientific mystery would you like to solve today?",
                    "🌟 I love your scientific curiosity! Science helps us understand everything around us - why the sky is blue, how plants grow, what makes rain fall, and so much more! What natural phenomenon are you curious about?",
                    "🧪 Welcome to the wonderful world of science! Every question you ask is like a scientist making a discovery. Science is everywhere - in your kitchen, backyard, and even in your own body! What would you like to explore?",
                    "🌍 Science rocks! It's like being a detective, but instead of solving crimes, we're solving the mysteries of nature! From dinosaurs to space exploration, science has all the coolest stories. What scientific adventure shall we go on?"
                };
                return scienceResponses[_random.Next(scienceResponses.Length)];
            }
            
            // Language/Reading/Writing responses
            if (message.Contains("read") || message.Contains("write") || message.Contains("story") || message.Contains("english") || message.Contains("word") || message.Contains("book"))
            {
                var languageResponses = new[]
                {
                    "📚 Reading and writing are like magic spells that let you travel to any world and share your amazing ideas! Every great author started just like you - with curiosity and practice. What story or writing project are you working on?",
                    "✍️ Words are powerful tools that can make people laugh, cry, learn, and dream! Whether you're reading an exciting adventure or writing your own story, I'm here to help you become a word wizard! What can I help you with?",
                    "📖 Literature opens doors to infinite worlds! From fairy tales to mysteries, from poetry to reports - every type of writing teaches us something new. Reading makes you smarter and writing makes you creative! What are you reading or writing?",
                    "🎭 Language arts is like being an actor, director, and audience all at once! You get to create characters, build worlds, and tell stories that matter. Every word you write and read makes you a better communicator! How can I help with your language journey?"
                };
                return languageResponses[_random.Next(languageResponses.Length)];
            }
            
            // Homework help responses
            if (message.Contains("homework") || message.Contains("assignment") || message.Contains("help") || message.Contains("stuck") || message.Contains("don't understand"))
            {
                var homeworkResponses = new[]
                {
                    "📝 Homework time! Don't worry - I'm here to make it easier and more fun! Remember, homework isn't just busy work - it's like training for your brain to become stronger and smarter. What subject are you working on?",
                    "🎯 I'm your homework helper! Every assignment is a chance to learn something cool and useful. Let's break down your homework into small, manageable pieces so it feels less overwhelming. What do you need help with?",
                    "💪 Homework can be challenging, but that's what makes your brain grow! I'll help you understand each step so you feel confident and proud of your work. What assignment is giving you trouble?",
                    "🌟 Great job asking for help! That's what smart students do. Homework helps you practice what you learned in class so it sticks in your memory. Let's tackle this together - what subject needs attention?"
                };
                return homeworkResponses[_random.Next(homeworkResponses.Length)];
            }
            
            // Question-asking responses
            if (message.Contains("what") || message.Contains("how") || message.Contains("why") || message.Contains("when") || message.Contains("where") || message.Contains("?"))
            {
                var questionResponses = new[]
                {
                    "🤔 What an excellent question! Asking questions is how we learn and discover new things. Scientists, inventors, and explorers all started with questions just like yours. Let me help you find the answer!",
                    "💡 I love curious minds like yours! Questions are the keys that unlock knowledge. Every 'what', 'how', and 'why' leads to amazing discoveries. Let's explore this together!",
                    "🔍 That's a fantastic question! Questions show that you're thinking deeply and want to understand the world better. That's exactly how great learners think! Let me help you discover the answer.",
                    "🎓 Brilliant question! You know what? The smartest people in history were the ones who asked the most questions. Your curiosity will take you far! Let's find the answer together."
                };
                return questionResponses[_random.Next(questionResponses.Length)];
            }
            
            // General encouraging responses for any other input
            var generalResponses = new[]
            {
                "🌟 I'm here to help you learn and grow! Whether you need help with schoolwork, want to explore a new topic, or just have questions about the world, I'm your learning buddy. What's on your mind today?",
                "💡 Learning is an adventure, and I'm excited to be your guide! Every day is a chance to discover something new and amazing. What would you like to explore or learn about?",
                "🎓 You're in the right place for learning! I love helping students like you understand new concepts and solve problems. What subject or topic interests you most right now?",
                "✨ Welcome to your personal learning space! I'm here to make education fun, engaging, and easy to understand. What can I help you learn or figure out today?",
                "🚀 Ready for a learning adventure? I can help with any subject - Math, Science, English, History, or anything else you're curious about! What shall we explore together?"
            };
            
            return generalResponses[_random.Next(generalResponses.Length)];
        }

        public List<QuizQuestion> GenerateQuizQuestions(string subject, string grade, string topic, int count = 30)
        {
            var questions = new List<QuizQuestion>();
            
            // Generate different types of questions
            var questionTypes = new[] { QuestionType.MultipleChoice, QuestionType.TrueFalse, QuestionType.FillInTheBlank };
            
            // For Math, Physics, Chemistry - add calculation questions
            if (subject.ToLower().Contains("math") || subject.ToLower().Contains("physics") || 
                subject.ToLower().Contains("chemistry") || subject.ToLower().Contains("science"))
            {
                questionTypes = new[] { QuestionType.MultipleChoice, QuestionType.TrueFalse, QuestionType.FillInTheBlank, QuestionType.Calculation };
            }
            
            for (int i = 0; i < count; i++)
            {
                var questionType = questionTypes[i % questionTypes.Length];
                
                if (questionType == QuestionType.Calculation && 
                    (subject.ToLower().Contains("math") || subject.ToLower().Contains("physics") || 
                     subject.ToLower().Contains("chemistry") || subject.ToLower().Contains("science")))
                {
                    questions.Add(GenerateCalculationQuestion(subject, grade, topic, i));
                }
                else if (questionType == QuestionType.FillInTheBlank)
                {
                    questions.Add(GenerateFillInBlankQuestion(subject, grade, topic, i));
                }
                else if (questionType == QuestionType.TrueFalse)
                {
                    questions.Add(GenerateTrueFalseQuestion(subject, grade, topic, i));
                }
                else
                {
                    questions.Add(GenerateMultipleChoiceQuestion(subject, grade, topic, i));
                }
            }
            
            return questions;
        }

        private QuizQuestion GenerateCalculationQuestion(string subject, string grade, string topic, int index)
        {
            var calculations = GetCalculationQuestions(subject, topic, grade);
            var calc = calculations[index % calculations.Length];
            
            return new QuizQuestion
            {
                Question = calc.Question,
                Subject = subject,
                Grade = grade,
                Topic = topic,
                Type = QuestionType.Calculation,
                OptionA = calc.CorrectAnswer,
                OptionB = calc.WrongAnswer1,
                OptionC = calc.WrongAnswer2,
                OptionD = calc.WrongAnswer3,
                CorrectAnswer = "A",
                Explanation = calc.Explanation,
                CalculationSteps = calc.Steps,
                Formula = calc.Formula,
                Units = calc.Units,
                ShowWorkRequired = true
            };
        }

        private QuizQuestion GenerateFillInBlankQuestion(string subject, string grade, string topic, int index)
        {
            var blanks = GetFillInBlankQuestions(subject, topic);
            var blank = blanks[index % blanks.Length];
            
            return new QuizQuestion
            {
                Question = blank.Question,
                Subject = subject,
                Grade = grade,
                Topic = topic,
                Type = QuestionType.FillInTheBlank,
                OptionA = blank.CorrectAnswer,
                OptionB = blank.WrongAnswer1,
                OptionC = blank.WrongAnswer2,
                OptionD = blank.WrongAnswer3,
                CorrectAnswer = "A",
                BlankAnswer = blank.CorrectAnswer,
                Explanation = blank.Explanation
            };
        }

        private QuizQuestion GenerateTrueFalseQuestion(string subject, string grade, string topic, int index)
        {
            var statements = GetTrueFalseStatements(subject, topic);
            var statement = statements[index % statements.Length];
            
            return new QuizQuestion
            {
                Question = $"{statement.Statement} True or False?",
                Subject = subject,
                Grade = grade,
                Topic = topic,
                Type = QuestionType.TrueFalse,
                OptionA = statement.IsTrue ? "True" : "False",
                OptionB = statement.IsTrue ? "False" : "True",
                OptionC = "",
                OptionD = "",
                CorrectAnswer = "A",
                Explanation = statement.Explanation
            };
        }

        private QuizQuestion GenerateMultipleChoiceQuestion(string subject, string grade, string topic, int index)
        {
            var questionTemplates = GetQuestionTemplates(subject, topic);
            var optionSets = GetOptionSets(subject, topic);
            
            var templateIndex = index % questionTemplates.Length;
            var optionIndex = index % optionSets.Length;
            
            var questionText = questionTemplates[templateIndex];
            var options = optionSets[optionIndex];
            
            // Add variation to questions
            if (index >= questionTemplates.Length)
            {
                var variation = (index / questionTemplates.Length) + 1;
                questionText = questionText.Replace("?", $" (Part {variation})?");
            }
            
            // Randomize correct answer position
            var correctAnswers = new[] { "A", "B", "C", "D" };
            var correctPos = _random.Next(correctAnswers.Length);
            var shuffledOptions = new string[4];
            
            shuffledOptions[correctPos] = options[0]; // Correct answer
            var wrongIndex = 1;
            for (int i = 0; i < 4; i++)
            {
                if (i != correctPos)
                {
                    shuffledOptions[i] = options[wrongIndex++];
                }
            }
            
            return new QuizQuestion
            {
                Question = questionText,
                Subject = subject,
                Grade = grade,
                Topic = topic,
                Type = QuestionType.MultipleChoice,
                OptionA = shuffledOptions[0],
                OptionB = shuffledOptions[1],
                OptionC = shuffledOptions[2],
                OptionD = shuffledOptions[3],
                CorrectAnswer = correctAnswers[correctPos],
                Explanation = $"This answer correctly relates to the key concepts of {topic} in {subject} for Grade {grade} students."
            };
        }

        private string[] GetQuestionTemplates(string subject, string topic)
        {
            return new[]
            {
                $"What is the main concept of {topic}?",
                $"How does {topic} relate to {subject}?",
                $"Which of the following best describes {topic}?",
                $"What is an important characteristic of {topic}?",
                $"Why is {topic} significant in {subject}?",
                $"What can we learn from studying {topic}?",
                $"How is {topic} used in real life?",
                $"What should students know about {topic}?",
                $"Which statement about {topic} is most accurate?",
                $"What is the relationship between {topic} and other concepts?",
                $"How can understanding {topic} help students?",
                $"What makes {topic} important for learning?",
                $"Which example best illustrates {topic}?",
                $"What is a key feature of {topic}?",
                $"How does {topic} connect to everyday experiences?"
            };
        }

        private string[][] GetOptionSets(string subject, string topic)
        {
            return new[]
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
        }

        private CalculationQuestion[] GetCalculationQuestions(string subject, string topic, string grade)
        {
            if (subject.ToLower().Contains("math"))
            {
                return new[]
                {
                    new CalculationQuestion
                    {
                        Question = "A rectangle has a length of 12 meters and width of 8 meters. What is its area?",
                        CorrectAnswer = "96 square meters",
                        WrongAnswer1 = "40 square meters",
                        WrongAnswer2 = "20 square meters", 
                        WrongAnswer3 = "104 square meters",
                        Steps = "Step 1: Use the formula Area = length × width\nStep 2: Substitute values: Area = 12m × 8m\nStep 3: Calculate: Area = 96 square meters",
                        Formula = "Area = length × width",
                        Units = "square meters",
                        Explanation = "To find the area of a rectangle, multiply the length by the width."
                    },
                    new CalculationQuestion
                    {
                        Question = "Sarah has 24 apples. She wants to share them equally among 6 friends. How many apples will each friend get?",
                        CorrectAnswer = "4 apples",
                        WrongAnswer1 = "6 apples",
                        WrongAnswer2 = "3 apples",
                        WrongAnswer3 = "5 apples",
                        Steps = "Step 1: Identify the operation: Division (sharing equally)\nStep 2: Set up the division: 24 ÷ 6\nStep 3: Calculate: 24 ÷ 6 = 4 apples per friend",
                        Formula = "Total items ÷ Number of groups = Items per group",
                        Units = "apples",
                        Explanation = "When sharing items equally, we use division to find how many each person gets."
                    },
                    new CalculationQuestion
                    {
                        Question = "A circle has a radius of 5 centimeters. What is its circumference? (Use π = 3.14)",
                        CorrectAnswer = "31.4 centimeters",
                        WrongAnswer1 = "15.7 centimeters",
                        WrongAnswer2 = "78.5 centimeters",
                        WrongAnswer3 = "25 centimeters",
                        Steps = "Step 1: Use the formula C = 2πr\nStep 2: Substitute values: C = 2 × 3.14 × 5\nStep 3: Calculate: C = 31.4 centimeters",
                        Formula = "C = 2πr",
                        Units = "centimeters",
                        Explanation = "The circumference of a circle is found using the formula C = 2πr, where r is the radius."
                    }
                };
            }
            else if (subject.ToLower().Contains("physics") || subject.ToLower().Contains("science"))
            {
                return new[]
                {
                    new CalculationQuestion
                    {
                        Question = "A car travels 120 kilometers in 2 hours. What is its average speed?",
                        CorrectAnswer = "60 km/h",
                        WrongAnswer1 = "240 km/h",
                        WrongAnswer2 = "122 km/h",
                        WrongAnswer3 = "30 km/h",
                        Steps = "Step 1: Use the formula Speed = Distance ÷ Time\nStep 2: Substitute values: Speed = 120 km ÷ 2 hours\nStep 3: Calculate: Speed = 60 km/h",
                        Formula = "Speed = Distance ÷ Time",
                        Units = "km/h",
                        Explanation = "Average speed is calculated by dividing the total distance by the total time taken."
                    },
                    new CalculationQuestion
                    {
                        Question = "A force of 20 Newtons is applied to move an object 5 meters. How much work is done?",
                        CorrectAnswer = "100 Joules",
                        WrongAnswer1 = "25 Joules",
                        WrongAnswer2 = "4 Joules",
                        WrongAnswer3 = "15 Joules",
                        Steps = "Step 1: Use the formula Work = Force × Distance\nStep 2: Substitute values: Work = 20 N × 5 m\nStep 3: Calculate: Work = 100 Joules",
                        Formula = "Work = Force × Distance",
                        Units = "Joules",
                        Explanation = "Work is calculated by multiplying the force applied by the distance moved in the direction of the force."
                    }
                };
            }
            else if (subject.ToLower().Contains("chemistry"))
            {
                return new[]
                {
                    new CalculationQuestion
                    {
                        Question = "How many grams are in 2.5 kilograms?",
                        CorrectAnswer = "2500 grams",
                        WrongAnswer1 = "250 grams",
                        WrongAnswer2 = "25 grams",
                        WrongAnswer3 = "0.25 grams",
                        Steps = "Step 1: Remember that 1 kg = 1000 g\nStep 2: Multiply: 2.5 kg × 1000 g/kg\nStep 3: Calculate: 2.5 × 1000 = 2500 grams",
                        Formula = "kg × 1000 = grams",
                        Units = "grams",
                        Explanation = "To convert kilograms to grams, multiply by 1000 since there are 1000 grams in 1 kilogram."
                    }
                };
            }

            // Default math questions if subject doesn't match
            return new[]
            {
                new CalculationQuestion
                {
                    Question = "What is 15 + 27?",
                    CorrectAnswer = "42",
                    WrongAnswer1 = "32",
                    WrongAnswer2 = "52",
                    WrongAnswer3 = "41",
                    Steps = "Step 1: Line up the numbers\nStep 2: Add ones place: 5 + 7 = 12 (write 2, carry 1)\nStep 3: Add tens place: 1 + 2 + 1 = 4\nStep 4: Answer: 42",
                    Formula = "Addition",
                    Units = "",
                    Explanation = "When adding two-digit numbers, add the ones place first, then the tens place, carrying over when needed."
                }
            };
        }

        private FillInBlankQuestion[] GetFillInBlankQuestions(string subject, string topic)
        {
            return new[]
            {
                new FillInBlankQuestion
                {
                    Question = $"The main purpose of studying {topic} is to _____ our understanding.",
                    CorrectAnswer = "improve",
                    WrongAnswer1 = "decrease",
                    WrongAnswer2 = "ignore",
                    WrongAnswer3 = "complicate",
                    Explanation = "Studying any topic helps improve our understanding and knowledge."
                },
                new FillInBlankQuestion
                {
                    Question = $"In {subject}, {topic} is considered a _____ concept.",
                    CorrectAnswer = "fundamental",
                    WrongAnswer1 = "useless",
                    WrongAnswer2 = "optional",
                    WrongAnswer3 = "confusing",
                    Explanation = "Most topics in academic subjects are fundamental concepts that build understanding."
                },
                new FillInBlankQuestion
                {
                    Question = $"Students learn about {topic} to develop their _____ skills.",
                    CorrectAnswer = "thinking",
                    WrongAnswer1 = "sleeping",
                    WrongAnswer2 = "eating",
                    WrongAnswer3 = "playing",
                    Explanation = "Academic topics help develop critical thinking and analytical skills."
                }
            };
        }

        private TrueFalseStatement[] GetTrueFalseStatements(string subject, string topic)
        {
            return new[]
            {
                new TrueFalseStatement
                {
                    Statement = $"{topic} is an important concept in {subject}",
                    IsTrue = true,
                    Explanation = $"Yes, {topic} is indeed an important concept that helps students understand {subject} better."
                },
                new TrueFalseStatement
                {
                    Statement = $"Learning about {topic} has no practical applications",
                    IsTrue = false,
                    Explanation = $"This is false. {topic} has many practical applications that help us understand the world around us."
                },
                new TrueFalseStatement
                {
                    Statement = $"Students should skip studying {topic} because it's too difficult",
                    IsTrue = false,
                    Explanation = $"This is false. While {topic} may be challenging, it's important for building a strong foundation in {subject}."
                }
            };
        }

        // Helper classes for structured question data
        private class CalculationQuestion
        {
            public string Question { get; set; } = "";
            public string CorrectAnswer { get; set; } = "";
            public string WrongAnswer1 { get; set; } = "";
            public string WrongAnswer2 { get; set; } = "";
            public string WrongAnswer3 { get; set; } = "";
            public string Steps { get; set; } = "";
            public string Formula { get; set; } = "";
            public string Units { get; set; } = "";
            public string Explanation { get; set; } = "";
        }

        private class FillInBlankQuestion
        {
            public string Question { get; set; } = "";
            public string CorrectAnswer { get; set; } = "";
            public string WrongAnswer1 { get; set; } = "";
            public string WrongAnswer2 { get; set; } = "";
            public string WrongAnswer3 { get; set; } = "";
            public string Explanation { get; set; } = "";
        }

        private class TrueFalseStatement
        {
            public string Statement { get; set; } = "";
            public bool IsTrue { get; set; }
            public string Explanation { get; set; } = "";
        }
    }
}
