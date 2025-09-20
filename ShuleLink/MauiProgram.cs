using Microsoft.Extensions.Logging;
using ShuleLink.Services;
using ShuleLink.Views;

namespace ShuleLink
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register services
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<GeminiAIService>();
            builder.Services.AddSingleton<LearningContentService>();
            builder.Services.AddSingleton<NavigationService>();
            builder.Services.AddSingleton<HttpClient>();

            // Register pages
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<SplashPage>();
            builder.Services.AddTransient<QuizPage>();
            builder.Services.AddTransient<LearningPage>();
            builder.Services.AddTransient<ChatListPage>();
            builder.Services.AddTransient<ChatPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<DiagramPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<WelcomePage>();
            builder.Services.AddTransient<ReadingDetailPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
