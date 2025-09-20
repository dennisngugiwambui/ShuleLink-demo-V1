using ShuleLink.Services;

namespace ShuleLink.Views;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("WelcomePage constructor started...");
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("WelcomePage InitializeComponent completed...");
            
            // Disable automatic navigation to prevent crashes
            // User must manually click Login/Register buttons
            System.Diagnostics.Debug.WriteLine("WelcomePage constructor completed - no auto navigation");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"WelcomePage constructor error: {ex.Message}");
        }
    }

    private async Task NavigateToMainApp()
    {
        await Task.Delay(100); // Small delay to ensure page is loaded
        await NavigationService.NavigateToAuthenticatedArea();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//RegisterPage");
    }

    private async void OnDemoClicked(object sender, EventArgs e)
    {
        // Demo mode redirects to register page
        await Shell.Current.GoToAsync("//RegisterPage");
    }

    protected override bool OnBackButtonPressed()
    {
        // Exit app when back button is pressed on welcome page
        Application.Current?.Quit();
        return true;
    }
}
