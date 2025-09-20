using ShuleLink.Models;
using ShuleLink.Services;

namespace ShuleLink.Views;

public partial class AcademicRecordsPage : ContentPage
{
    private readonly AcademicService _academicService;

    public AcademicRecordsPage()
    {
        InitializeComponent();
        _academicService = new AcademicService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAcademicRecordsAsync();
    }

    private async Task LoadAcademicRecordsAsync()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var studentId = Preferences.Get("UserId", 1);
            var records = await _academicService.GetAcademicRecordsAsync(studentId);
            var progress = await _academicService.GetStudentProgressAsync(studentId);

            // Update summary
            CurrentGPALabel.Text = progress.CurrentGPA.ToString("F1");
            ClassPositionLabel.Text = $"{GetOrdinalNumber(progress.CurrentClassPosition)}";
            StreamPositionLabel.Text = $"{GetOrdinalNumber(progress.CurrentStreamPosition)}";
            TrendLabel.Text = $"Performance Trend: {progress.PerformanceTrend}";

            // Clear existing records
            var summaryFrame = RecordsContainer.Children.FirstOrDefault();
            RecordsContainer.Children.Clear();
            if (summaryFrame != null)
                RecordsContainer.Children.Add(summaryFrame);

            // Add grade records
            foreach (var record in records)
            {
                var gradeCard = CreateGradeCard(record);
                RecordsContainer.Children.Add(gradeCard);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load academic records: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private Frame CreateGradeCard(AcademicRecord record)
    {
        var gradeCard = new Frame
        {
            BackgroundColor = Colors.White,
            HasShadow = true,
            CornerRadius = 15,
            Padding = new Thickness(20),
            Margin = new Thickness(0, 10)
        };

        var mainLayout = new StackLayout { Spacing = 15 };

        // Grade Header
        var headerLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = 10
        };

        headerLayout.Children.Add(new Label
        {
            Text = "🎓",
            FontSize = 24,
            VerticalOptions = LayoutOptions.Center
        });

        headerLayout.Children.Add(new Label
        {
            Text = $"Grade {record.Grade} - {record.Year}",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#2C3E50"),
            VerticalOptions = LayoutOptions.Center
        });

        headerLayout.Children.Add(new Frame
        {
            BackgroundColor = GetGradeColor(record.OverallAverage),
            CornerRadius = 15,
            Padding = new Thickness(10, 5),
            HasShadow = false,
            HorizontalOptions = LayoutOptions.End,
            Content = new Label
            {
                Text = $"{record.OverallAverage:F1}%",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White
            }
        });

        mainLayout.Children.Add(headerLayout);

        // Subjects Grid
        var subjectsGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            RowSpacing = 8,
            ColumnSpacing = 5
        };

        // Headers
        subjectsGrid.Add(CreateHeaderLabel("Subject"), 0, 0);
        subjectsGrid.Add(CreateHeaderLabel("Term 1"), 1, 0);
        subjectsGrid.Add(CreateHeaderLabel("Term 2"), 2, 0);
        subjectsGrid.Add(CreateHeaderLabel("Term 3"), 3, 0);
        subjectsGrid.Add(CreateHeaderLabel("Average"), 4, 0);

        int row = 1;
        foreach (var subject in record.SubjectGrades)
        {
            subjectsGrid.Add(CreateSubjectLabel(subject.SubjectName), 0, row);
            subjectsGrid.Add(CreateMarkLabel(subject.Term1Mark), 1, row);
            subjectsGrid.Add(CreateMarkLabel(subject.Term2Mark), 2, row);
            subjectsGrid.Add(CreateMarkLabel(subject.Term3Mark), 3, row);
            subjectsGrid.Add(CreateAverageLabel(subject.AverageMark, subject.Grade), 4, row);
            row++;
        }

        mainLayout.Children.Add(subjectsGrid);

        // Position Information
        var positionLayout = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            ColumnSpacing = 10
        };

        var classPositionFrame = new Frame
        {
            BackgroundColor = Color.FromArgb("#E8F5E8"),
            CornerRadius = 10,
            Padding = new Thickness(15, 10),
            HasShadow = false
        };

        var classPositionLayout = new StackLayout
        {
            Spacing = 5,
            Children =
            {
                new Label
                {
                    Text = $"{GetOrdinalNumber(record.ClassPosition)}",
                    FontSize = 18,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#27AE60"),
                    HorizontalOptions = LayoutOptions.Center
                },
                new Label
                {
                    Text = $"Class Position",
                    FontSize = 12,
                    TextColor = Color.FromArgb("#27AE60"),
                    HorizontalOptions = LayoutOptions.Center
                },
                new Label
                {
                    Text = $"out of {record.TotalStudentsInClass}",
                    FontSize = 10,
                    TextColor = Color.FromArgb("#7F8C8D"),
                    HorizontalOptions = LayoutOptions.Center
                }
            }
        };

        classPositionFrame.Content = classPositionLayout;

        var streamPositionFrame = new Frame
        {
            BackgroundColor = Color.FromArgb("#E3F2FD"),
            CornerRadius = 10,
            Padding = new Thickness(15, 10),
            HasShadow = false
        };

        var streamPositionLayout = new StackLayout
        {
            Spacing = 5,
            Children =
            {
                new Label
                {
                    Text = $"{GetOrdinalNumber(record.StreamPosition)}",
                    FontSize = 18,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#2196F3"),
                    HorizontalOptions = LayoutOptions.Center
                },
                new Label
                {
                    Text = $"Stream Position",
                    FontSize = 12,
                    TextColor = Color.FromArgb("#2196F3"),
                    HorizontalOptions = LayoutOptions.Center
                },
                new Label
                {
                    Text = $"out of {record.TotalStudentsInStream}",
                    FontSize = 10,
                    TextColor = Color.FromArgb("#7F8C8D"),
                    HorizontalOptions = LayoutOptions.Center
                }
            }
        };

        streamPositionFrame.Content = streamPositionLayout;

        positionLayout.Add(classPositionFrame, 0, 0);
        positionLayout.Add(streamPositionFrame, 1, 0);

        mainLayout.Children.Add(positionLayout);

        // Comments
        if (!string.IsNullOrEmpty(record.Comments))
        {
            var commentsFrame = new Frame
            {
                BackgroundColor = Color.FromArgb("#FFF8E1"),
                CornerRadius = 10,
                Padding = new Thickness(15),
                HasShadow = false
            };

            var commentsLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Children =
                {
                    new Label
                    {
                        Text = "💬",
                        FontSize = 16,
                        VerticalOptions = LayoutOptions.Start
                    },
                    new Label
                    {
                        Text = record.Comments,
                        FontSize = 14,
                        TextColor = Color.FromArgb("#F57F17"),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    }
                }
            };

            commentsFrame.Content = commentsLayout;
            mainLayout.Children.Add(commentsFrame);
        }

        gradeCard.Content = mainLayout;
        return gradeCard;
    }

    private Label CreateHeaderLabel(string text)
    {
        return new Label
        {
            Text = text,
            FontSize = 12,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#34495E"),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
    }

    private Label CreateSubjectLabel(string text)
    {
        return new Label
        {
            Text = text,
            FontSize = 13,
            TextColor = Color.FromArgb("#2C3E50"),
            VerticalOptions = LayoutOptions.Center
        };
    }

    private Label CreateMarkLabel(double mark)
    {
        return new Label
        {
            Text = mark.ToString("F0"),
            FontSize = 13,
            TextColor = GetMarkColor(mark),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
    }

    private Frame CreateAverageLabel(double average, string grade)
    {
        return new Frame
        {
            BackgroundColor = GetGradeColor(average),
            CornerRadius = 8,
            Padding = new Thickness(8, 4),
            HasShadow = false,
            HorizontalOptions = LayoutOptions.Center,
            Content = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 3,
                Children =
                {
                    new Label
                    {
                        Text = average.ToString("F0"),
                        FontSize = 12,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Colors.White
                    },
                    new Label
                    {
                        Text = grade,
                        FontSize = 10,
                        TextColor = Colors.White,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            }
        };
    }

    private Color GetGradeColor(double average)
    {
        return average switch
        {
            >= 90 => Color.FromArgb("#27AE60"), // A+ - Green
            >= 80 => Color.FromArgb("#2ECC71"), // A - Light Green
            >= 70 => Color.FromArgb("#3498DB"), // B+ - Blue
            >= 60 => Color.FromArgb("#9B59B6"), // B - Purple
            >= 50 => Color.FromArgb("#F39C12"), // C+ - Orange
            >= 40 => Color.FromArgb("#E67E22"), // C - Dark Orange
            >= 30 => Color.FromArgb("#E74C3C"), // D+ - Red
            _ => Color.FromArgb("#95A5A6") // D/E - Gray
        };
    }

    private Color GetMarkColor(double mark)
    {
        return mark switch
        {
            >= 80 => Color.FromArgb("#27AE60"),
            >= 60 => Color.FromArgb("#3498DB"),
            >= 40 => Color.FromArgb("#F39C12"),
            _ => Color.FromArgb("#E74C3C")
        };
    }

    private string GetOrdinalNumber(int number)
    {
        if (number <= 0) return number.ToString();

        return (number % 100) switch
        {
            11 or 12 or 13 => $"{number}th",
            _ => (number % 10) switch
            {
                1 => $"{number}st",
                2 => $"{number}nd",
                3 => $"{number}rd",
                _ => $"{number}th"
            }
        };
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnAnalyticsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Analytics", "Performance analytics feature coming soon!", "OK");
    }
}
