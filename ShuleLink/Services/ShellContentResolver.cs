using Microsoft.Extensions.DependencyInjection;

namespace ShuleLink.Services
{
    public static class ShellContentResolver
    {
        public static void RegisterRoutes(IServiceProvider serviceProvider)
        {
            // Register routes with dependency injection support
            Routing.RegisterRoute("WelcomePage", typeof(Views.WelcomePage));
            Routing.RegisterRoute("LoginPage", typeof(Views.LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(Views.RegisterPage));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("ChatList", typeof(Views.ChatListPage));
            Routing.RegisterRoute("Learning", typeof(Views.LearningPage));
            Routing.RegisterRoute("Quiz", typeof(Views.QuizPage));
            Routing.RegisterRoute("Profile", typeof(Views.ProfilePage));
            Routing.RegisterRoute("ChatPage", typeof(Views.ChatPage));
            Routing.RegisterRoute("ReadingDetailPage", typeof(Views.ReadingDetailPage));
            Routing.RegisterRoute("DiagramPage", typeof(Views.DiagramPage));
        }
    }
}
