using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace ShuleLink.Services
{
    public static class ToastService
    {
        public static async Task ShowToast(string message, ToastType type = ToastType.Info, int durationMs = 3000)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"ToastService: Showing toast - {message}");
                
                // Simple DisplayAlert-based toast for better compatibility
                var title = GetTitle(type);
                var icon = GetIcon(type);
                var displayMessage = $"{icon} {message}";
                
                // Show alert on UI thread - NO Task.Run to avoid threading issues
                if (Application.Current?.MainPage != null)
                {
                    // Use MainThread.BeginInvokeOnMainThread for thread safety
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            await Application.Current.MainPage.DisplayAlert(title, displayMessage, "OK");
                        }
                        catch (Exception alertEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"DisplayAlert error: {alertEx.Message}");
                        }
                    });
                }
                
                System.Diagnostics.Debug.WriteLine($"ToastService: Toast displayed successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ToastService error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        public static async Task ShowAdvancedToast(string message, ToastType type = ToastType.Info, int durationMs = 3000)
        {
            try
            {
                var mainPage = Application.Current?.MainPage;
                if (mainPage == null) return;

                // Create toast container
                var toastContainer = new Frame
                {
                    BackgroundColor = GetBackgroundColor(type),
                    CornerRadius = 25,
                    Padding = new Thickness(20, 15),
                    Margin = new Thickness(20, 0, 20, 50),
                    HasShadow = true,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.End,
                    ZIndex = 1000,
                    Opacity = 0,
                    TranslationY = 100
                };

                toastContainer.Shadow = new Shadow
                {
                    Brush = Colors.Black,
                    Offset = new Point(0, 5),
                    Radius = 15,
                    Opacity = 0.3f
                };

                // Create content
                var contentStack = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 15,
                    VerticalOptions = LayoutOptions.Center
                };

                // Add icon
                var icon = new Label
                {
                    Text = GetIcon(type),
                    FontSize = 20,
                    TextColor = Colors.White,
                    VerticalOptions = LayoutOptions.Center
                };

                // Add message
                var messageLabel = new Label
                {
                    Text = message,
                    TextColor = Colors.White,
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                contentStack.Children.Add(icon);
                contentStack.Children.Add(messageLabel);
                toastContainer.Content = contentStack;

                // Add to main page - handle different page types
                if (mainPage is ContentPage contentPage)
                {
                    if (contentPage.Content is Grid mainGrid)
                    {
                        mainGrid.Children.Add(toastContainer);
                    }
                    else if (contentPage.Content is StackLayout mainStack)
                    {
                        mainStack.Children.Add(toastContainer);
                    }
                    else
                    {
                        // Create overlay grid
                        var overlay = new Grid();
                        if (contentPage.Content != null)
                        {
                            overlay.Children.Add(contentPage.Content);
                        }
                        overlay.Children.Add(toastContainer);
                        contentPage.Content = overlay;
                    }
                }
                else if (mainPage is Shell shell)
                {
                    // For Shell-based apps, we need to add to the current page
                    var currentPage = shell.CurrentPage;
                    if (currentPage is ContentPage currentContentPage && currentContentPage.Content is Layout currentLayout)
                    {
                        if (currentLayout is Grid currentGrid)
                        {
                            currentGrid.Children.Add(toastContainer);
                        }
                        else
                        {
                            // Create overlay for non-grid layouts
                            var overlay = new Grid();
                            overlay.Children.Add(currentContentPage.Content);
                            overlay.Children.Add(toastContainer);
                            currentContentPage.Content = overlay;
                        }
                    }
                }

                // Animate in
                await Task.WhenAll(
                    toastContainer.FadeTo(1, 300, Easing.CubicOut),
                    toastContainer.TranslateTo(0, 0, 300, Easing.CubicOut)
                );

                // Wait for duration
                await Task.Delay(durationMs);

                // Animate out
                await Task.WhenAll(
                    toastContainer.FadeTo(0, 300, Easing.CubicIn),
                    toastContainer.TranslateTo(0, 100, 300, Easing.CubicIn)
                );

                // Remove from parent
                if (toastContainer.Parent is Layout parent)
                {
                    parent.Children.Remove(toastContainer);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Toast error: {ex.Message}");
            }
        }

        private static string GetTitle(ToastType type)
        {
            return type switch
            {
                ToastType.Success => "Success",
                ToastType.Error => "Error",
                ToastType.Warning => "Warning",
                ToastType.Info => "Info",
                _ => "Notification"
            };
        }

        private static Color GetBackgroundColor(ToastType type)
        {
            return type switch
            {
                ToastType.Success => Color.FromArgb("#27AE60"),
                ToastType.Error => Color.FromArgb("#E74C3C"),
                ToastType.Warning => Color.FromArgb("#F39C12"),
                ToastType.Info => Color.FromArgb("#3498DB"),
                _ => Color.FromArgb("#3498DB")
            };
        }

        private static string GetIcon(ToastType type)
        {
            return type switch
            {
                ToastType.Success => "✅",
                ToastType.Error => "❌",
                ToastType.Warning => "⚠️",
                ToastType.Info => "ℹ️",
                _ => "ℹ️"
            };
        }
    }

    public enum ToastType
    {
        Success,
        Error,
        Warning,
        Info
    }
}
