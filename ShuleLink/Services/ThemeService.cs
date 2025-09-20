using Microsoft.Maui.Controls;

namespace ShuleLink.Services;

public static class ThemeService
{
    public static bool IsDarkMode => Preferences.Get("DarkMode", false);

    public static void SetTheme(bool isDarkMode)
    {
        Preferences.Set("DarkMode", isDarkMode);
        ApplyTheme(isDarkMode);
    }

    public static void ApplyTheme(bool isDarkMode)
    {
        var app = Application.Current;
        if (app?.Resources == null) return;

        // Clear existing theme resources
        app.Resources.Clear();

        // Apply theme-specific colors
        if (isDarkMode)
        {
            ApplyDarkTheme(app.Resources);
        }
        else
        {
            ApplyLightTheme(app.Resources);
        }
    }

    private static void ApplyDarkTheme(ResourceDictionary resources)
    {
        // Dark Theme Colors
        resources.Add("Primary", Color.FromArgb("#4A90E2"));
        resources.Add("Secondary", Color.FromArgb("#27AE60"));
        resources.Add("Tertiary", Color.FromArgb("#9B59B6"));
        resources.Add("AccentRed", Color.FromArgb("#E74C3C"));
        resources.Add("AccentOrange", Color.FromArgb("#E67E22"));
        
        // Dark Background Colors
        resources.Add("BackgroundLight", Color.FromArgb("#1E1E1E"));
        resources.Add("BackgroundCard", Color.FromArgb("#2D2D2D"));
        resources.Add("BackgroundHeader", Color.FromArgb("#1A1A1A"));
        
        // Dark Text Colors
        resources.Add("TextPrimary", Color.FromArgb("#FFFFFF"));
        resources.Add("TextSecondary", Color.FromArgb("#B0B0B0"));
        resources.Add("TextLight", Color.FromArgb("#808080"));
        
        // Chat Colors (Dark)
        resources.Add("ChatBackground", Color.FromArgb("#0B141A"));
        resources.Add("ChatSentBubble", Color.FromArgb("#005C4B"));
        resources.Add("ChatReceivedBubble", Color.FromArgb("#1F2C34"));
        resources.Add("ChatHeaderBackground", Color.FromArgb("#2A2F32"));
        resources.Add("ChatInputBackground", Color.FromArgb("#2A2F32"));
        resources.Add("ChatInputFieldBackground", Color.FromArgb("#3C4043"));
        
        // Tab Bar Colors (Dark)
        resources.Add("TabBarBackground", Color.FromArgb("#1E1E1E"));
        resources.Add("TabBarSelected", Color.FromArgb("#4A90E2"));
        resources.Add("TabBarUnselected", Color.FromArgb("#808080"));

        ApplyCommonStyles(resources, true);
    }

    private static void ApplyLightTheme(ResourceDictionary resources)
    {
        // Light Theme Colors
        resources.Add("Primary", Color.FromArgb("#4A90E2"));
        resources.Add("Secondary", Color.FromArgb("#27AE60"));
        resources.Add("Tertiary", Color.FromArgb("#9B59B6"));
        resources.Add("AccentRed", Color.FromArgb("#E74C3C"));
        resources.Add("AccentOrange", Color.FromArgb("#E67E22"));
        
        // Light Background Colors
        resources.Add("BackgroundLight", Color.FromArgb("#F8F9FA"));
        resources.Add("BackgroundCard", Color.FromArgb("#FFFFFF"));
        resources.Add("BackgroundHeader", Color.FromArgb("#34495E"));
        
        // Light Text Colors
        resources.Add("TextPrimary", Color.FromArgb("#2C3E50"));
        resources.Add("TextSecondary", Color.FromArgb("#7F8C8D"));
        resources.Add("TextLight", Color.FromArgb("#95A5A6"));
        
        // Chat Colors (Light)
        resources.Add("ChatBackground", Color.FromArgb("#ECE5DD"));
        resources.Add("ChatSentBubble", Color.FromArgb("#DCF8C6"));
        resources.Add("ChatReceivedBubble", Color.FromArgb("#FFFFFF"));
        resources.Add("ChatHeaderBackground", Color.FromArgb("#075E54"));
        resources.Add("ChatInputBackground", Color.FromArgb("#F0F0F0"));
        resources.Add("ChatInputFieldBackground", Color.FromArgb("#FFFFFF"));
        
        // Tab Bar Colors (Light)
        resources.Add("TabBarBackground", Color.FromArgb("#FFFFFF"));
        resources.Add("TabBarSelected", Color.FromArgb("#4A90E2"));
        resources.Add("TabBarUnselected", Color.FromArgb("#BDC3C7"));

        ApplyCommonStyles(resources, false);
    }

    private static void ApplyCommonStyles(ResourceDictionary resources, bool isDarkMode)
    {
        // Shell Styling
        var shellStyle = new Style(typeof(Shell));
        shellStyle.Setters.Add(new Setter { Property = Shell.BackgroundColorProperty, Value = resources["BackgroundLight"] });
        shellStyle.Setters.Add(new Setter { Property = Shell.ForegroundColorProperty, Value = resources["TextPrimary"] });
        shellStyle.Setters.Add(new Setter { Property = Shell.TitleColorProperty, Value = resources["TextPrimary"] });
        shellStyle.Setters.Add(new Setter { Property = Shell.DisabledColorProperty, Value = resources["TextLight"] });
        shellStyle.Setters.Add(new Setter { Property = Shell.UnselectedColorProperty, Value = resources["TabBarUnselected"] });
        shellStyle.Setters.Add(new Setter { Property = Shell.TabBarBackgroundColorProperty, Value = resources["TabBarBackground"] });
        shellStyle.Setters.Add(new Setter { Property = Shell.TabBarForegroundColorProperty, Value = resources["TabBarSelected"] });
        shellStyle.Setters.Add(new Setter { Property = Shell.TabBarUnselectedColorProperty, Value = resources["TabBarUnselected"] });
        shellStyle.Setters.Add(new Setter { Property = Shell.TabBarTitleColorProperty, Value = resources["TextPrimary"] });
        resources.Add(shellStyle);

        // Button Styling
        var buttonStyle = new Style(typeof(Button));
        buttonStyle.Setters.Add(new Setter { Property = Button.BackgroundColorProperty, Value = resources["Primary"] });
        buttonStyle.Setters.Add(new Setter { Property = Button.TextColorProperty, Value = Colors.White });
        buttonStyle.Setters.Add(new Setter { Property = Button.CornerRadiusProperty, Value = 12 });
        buttonStyle.Setters.Add(new Setter { Property = Button.FontSizeProperty, Value = 16 });
        buttonStyle.Setters.Add(new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold });
        buttonStyle.Setters.Add(new Setter { Property = Button.PaddingProperty, Value = new Thickness(20, 12) });
        buttonStyle.Setters.Add(new Setter { Property = Button.MarginProperty, Value = new Thickness(5) });
        resources.Add(buttonStyle);

        // Frame Styling (No shadows to prevent crashes)
        var frameStyle = new Style(typeof(Frame));
        frameStyle.Setters.Add(new Setter { Property = Frame.BackgroundColorProperty, Value = resources["BackgroundCard"] });
        frameStyle.Setters.Add(new Setter { Property = Frame.HasShadowProperty, Value = false });
        frameStyle.Setters.Add(new Setter { Property = Frame.CornerRadiusProperty, Value = 15 });
        frameStyle.Setters.Add(new Setter { Property = Frame.PaddingProperty, Value = new Thickness(15) });
        frameStyle.Setters.Add(new Setter { Property = Frame.MarginProperty, Value = new Thickness(10, 5) });
        resources.Add(frameStyle);

        // Entry Styling
        var entryStyle = new Style(typeof(Entry));
        entryStyle.Setters.Add(new Setter { Property = Entry.BackgroundColorProperty, Value = resources["BackgroundCard"] });
        entryStyle.Setters.Add(new Setter { Property = Entry.TextColorProperty, Value = resources["TextPrimary"] });
        entryStyle.Setters.Add(new Setter { Property = Entry.PlaceholderColorProperty, Value = resources["TextLight"] });
        entryStyle.Setters.Add(new Setter { Property = Entry.FontSizeProperty, Value = 16 });
        entryStyle.Setters.Add(new Setter { Property = Entry.MarginProperty, Value = new Thickness(0, 5) });
        entryStyle.Setters.Add(new Setter { Property = Entry.HeightRequestProperty, Value = 50 });
        resources.Add(entryStyle);

        // Label Styling
        var labelStyle = new Style(typeof(Label));
        labelStyle.Setters.Add(new Setter { Property = Label.TextColorProperty, Value = resources["TextPrimary"] });
        labelStyle.Setters.Add(new Setter { Property = Label.FontSizeProperty, Value = 14 });
        resources.Add(labelStyle);

        // ContentPage Styling
        var contentPageStyle = new Style(typeof(ContentPage));
        contentPageStyle.Setters.Add(new Setter { Property = ContentPage.BackgroundColorProperty, Value = resources["BackgroundLight"] });
        resources.Add(contentPageStyle);
    }

    public static void InitializeTheme()
    {
        var isDarkMode = IsDarkMode;
        ApplyTheme(isDarkMode);
    }
}
