namespace ShuleLink.Views;

[QueryProperty(nameof(DiagramType), "type")]
public partial class DiagramPage : ContentPage
{
    public string DiagramType { get; set; } = "";

    public DiagramPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadDiagramContent();
    }

    private void LoadDiagramContent()
    {
        DiagramTitleLabel.Text = DiagramType;
        
        switch (DiagramType.ToLower())
        {
            case "🫁 respiratory system":
                DiagramIconLabel.Text = "🫁";
                DiagramDescriptionLabel.Text = "The respiratory system helps us breathe. It includes the nose, trachea, lungs, and diaphragm.";
                AddKeyPoints(new[]
                {
                    "Nose filters and warms air",
                    "Trachea carries air to lungs",
                    "Lungs exchange oxygen and carbon dioxide",
                    "Diaphragm helps with breathing"
                });
                break;
                
            case "🍎 digestive system":
                DiagramIconLabel.Text = "🍎";
                DiagramDescriptionLabel.Text = "The digestive system breaks down food into nutrients our body can use.";
                AddKeyPoints(new[]
                {
                    "Mouth begins digestion with saliva",
                    "Stomach breaks down food with acid",
                    "Small intestine absorbs nutrients",
                    "Large intestine removes waste"
                });
                break;
                
            case "❤️ circulatory system":
                DiagramIconLabel.Text = "❤️";
                DiagramDescriptionLabel.Text = "The circulatory system pumps blood throughout the body, carrying oxygen and nutrients.";
                AddKeyPoints(new[]
                {
                    "Heart pumps blood through the body",
                    "Arteries carry blood away from heart",
                    "Veins return blood to the heart",
                    "Blood carries oxygen and nutrients"
                });
                break;
                
            case "🧠 nervous system":
                DiagramIconLabel.Text = "🧠";
                DiagramDescriptionLabel.Text = "The nervous system controls all body functions and helps us think and feel.";
                AddKeyPoints(new[]
                {
                    "Brain controls thinking and memory",
                    "Spinal cord carries messages",
                    "Nerves connect to all body parts",
                    "Reflexes protect us from danger"
                });
                break;
                
            case "🌱 plant parts":
                DiagramIconLabel.Text = "🌱";
                DiagramDescriptionLabel.Text = "Plants have different parts that help them grow, make food, and reproduce.";
                AddKeyPoints(new[]
                {
                    "Roots absorb water and nutrients",
                    "Stem supports the plant",
                    "Leaves make food through photosynthesis",
                    "Flowers help plants reproduce"
                });
                break;
                
            case "🌍 solar system":
                DiagramIconLabel.Text = "🌍";
                DiagramDescriptionLabel.Text = "Our solar system has the Sun at the center with eight planets orbiting around it.";
                AddKeyPoints(new[]
                {
                    "Sun is the center of our solar system",
                    "Eight planets orbit the Sun",
                    "Earth is the third planet from the Sun",
                    "Planets have different sizes and features"
                });
                break;
                
            default:
                DiagramIconLabel.Text = "🔬";
                DiagramDescriptionLabel.Text = "Educational diagram content would be displayed here.";
                break;
        }
    }

    private void AddKeyPoints(string[] points)
    {
        KeyPointsContainer.Children.Clear();
        
        foreach (var point in points)
        {
            var pointLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10
            };
            
            pointLayout.Children.Add(new Label
            {
                Text = "•",
                FontSize = 16,
                TextColor = Color.FromArgb("#E74C3C"),
                VerticalOptions = LayoutOptions.Start
            });
            
            pointLayout.Children.Add(new Label
            {
                Text = point,
                FontSize = 14,
                TextColor = Color.FromArgb("#2C3E50"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand
            });
            
            KeyPointsContainer.Children.Add(pointLayout);
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnTakeQuizClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainTabs");
        await Task.Delay(100);
        await Shell.Current.GoToAsync("Quiz");
    }
}
