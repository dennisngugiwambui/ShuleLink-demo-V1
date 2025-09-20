using ShuleLink.Models;
using ShuleLink.Services;
using System.Collections.ObjectModel;

namespace ShuleLink.Views;

public partial class TimetablePage : ContentPage
{
    private readonly AcademicService _academicService;
    private ObservableCollection<DayViewModel> _days = new();
    private string _selectedDay = "Monday";

    public TimetablePage()
    {
        InitializeComponent();
        _academicService = new AcademicService();
        InitializeDays();
        DaySelector.ItemsSource = _days;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTimetableAsync();
    }

    private void InitializeDays()
    {
        var days = new[]
        {
            new { Name = "Monday", Short = "MON" },
            new { Name = "Tuesday", Short = "TUE" },
            new { Name = "Wednesday", Short = "WED" },
            new { Name = "Thursday", Short = "THU" },
            new { Name = "Friday", Short = "FRI" }
        };

        var today = DateTime.Now.DayOfWeek.ToString();
        
        foreach (var day in days)
        {
            _days.Add(new DayViewModel
            {
                DayName = day.Name,
                DayShort = day.Short,
                IsSelected = day.Name.Equals(today, StringComparison.OrdinalIgnoreCase) || 
                           (today == "Saturday" || today == "Sunday" ? day.Name == "Monday" : false)
            });
        }

        // Set selected day
        var selectedDayItem = _days.FirstOrDefault(d => d.IsSelected);
        if (selectedDayItem != null)
        {
            _selectedDay = selectedDayItem.DayName;
            DaySelector.SelectedItem = selectedDayItem;
        }
        else
        {
            _selectedDay = "Monday";
            _days[0].IsSelected = true;
            DaySelector.SelectedItem = _days[0];
        }
    }

    private async Task LoadTimetableAsync()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var currentGrade = 5; // This would come from user profile
            var timetableEntries = await _academicService.GetTimetableForDayAsync(currentGrade, _selectedDay);

            CurrentDayLabel.Text = $"{_selectedDay} Schedule";
            
            var regularPeriods = timetableEntries.Where(t => !t.IsBreak).Count();
            TotalPeriodsLabel.Text = $"{regularPeriods} Periods";

            // Clear existing periods (keep the info card)
            var infoCard = TimetableContainer.Children.FirstOrDefault();
            TimetableContainer.Children.Clear();
            if (infoCard != null)
                TimetableContainer.Children.Add(infoCard);

            // Add periods
            foreach (var entry in timetableEntries)
            {
                var periodCard = CreatePeriodCard(entry);
                TimetableContainer.Children.Add(periodCard);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load timetable: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private Frame CreatePeriodCard(TimetableEntry entry)
    {
        var periodCard = new Frame
        {
            BackgroundColor = Colors.White,
            HasShadow = true,
            CornerRadius = 12,
            Padding = new Thickness(0),
            Margin = new Thickness(0, 5)
        };

        var mainGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = new GridLength(6) },
                new ColumnDefinition { Width = GridLength.Star }
            }
        };

        // Color indicator
        var colorIndicator = new BoxView
        {
            BackgroundColor = Color.FromArgb(entry.SubjectColor),
            CornerRadius = new CornerRadius(6, 0, 0, 6)
        };
        mainGrid.Add(colorIndicator, 0, 0);

        // Content
        var contentLayout = new StackLayout
        {
            Padding = new Thickness(15),
            Spacing = 8
        };

        if (entry.IsBreak)
        {
            // Break layout
            var breakLayout = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            var breakInfo = new StackLayout
            {
                Spacing = 3,
                Children =
                {
                    new Label
                    {
                        Text = entry.SubjectName,
                        FontSize = 16,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.FromArgb("#7F8C8D")
                    },
                    new Label
                    {
                        Text = entry.TimeSlot,
                        FontSize = 12,
                        TextColor = Color.FromArgb("#95A5A6")
                    }
                }
            };

            var breakIcon = new Label
            {
                Text = entry.SubjectName.Contains("Lunch") ? "🍽️" : "☕",
                FontSize = 24,
                VerticalOptions = LayoutOptions.Center
            };

            breakLayout.Add(breakInfo, 0, 0);
            breakLayout.Add(breakIcon, 1, 0);
            contentLayout.Children.Add(breakLayout);
        }
        else
        {
            // Regular period layout
            var headerLayout = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            var subjectInfo = new StackLayout
            {
                Spacing = 3,
                Children =
                {
                    new Label
                    {
                        Text = entry.SubjectName,
                        FontSize = 16,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.FromArgb("#2C3E50")
                    },
                    new Label
                    {
                        Text = entry.TeacherName,
                        FontSize = 13,
                        TextColor = Color.FromArgb("#3498DB")
                    }
                }
            };

            var periodBadge = new Frame
            {
                BackgroundColor = Color.FromArgb(entry.SubjectColor),
                CornerRadius = 12,
                Padding = new Thickness(10, 5),
                HasShadow = false,
                Content = new Label
                {
                    Text = entry.Period,
                    FontSize = 11,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Colors.White
                }
            };

            headerLayout.Add(subjectInfo, 0, 0);
            headerLayout.Add(periodBadge, 1, 0);
            contentLayout.Children.Add(headerLayout);

            // Time and classroom info
            var detailsLayout = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };

            var timeInfo = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 5,
                Children =
                {
                    new Label
                    {
                        Text = "🕐",
                        FontSize = 14,
                        VerticalOptions = LayoutOptions.Center
                    },
                    new Label
                    {
                        Text = entry.TimeSlot,
                        FontSize = 12,
                        TextColor = Color.FromArgb("#7F8C8D"),
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };

            var classroomInfo = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 5,
                HorizontalOptions = LayoutOptions.End,
                Children =
                {
                    new Label
                    {
                        Text = "🏫",
                        FontSize = 14,
                        VerticalOptions = LayoutOptions.Center
                    },
                    new Label
                    {
                        Text = entry.Classroom,
                        FontSize = 12,
                        TextColor = Color.FromArgb("#7F8C8D"),
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };

            detailsLayout.Add(timeInfo, 0, 0);
            detailsLayout.Add(classroomInfo, 1, 0);
            contentLayout.Children.Add(detailsLayout);
        }

        mainGrid.Add(contentLayout, 1, 0);
        periodCard.Content = mainGrid;

        return periodCard;
    }

    private void OnDaySelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is DayViewModel selectedDay)
        {
            // Update selection state
            foreach (var day in _days)
            {
                day.IsSelected = day == selectedDay;
            }

            _selectedDay = selectedDay.DayName;
            _ = LoadTimetableAsync();
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnTodayClicked(object sender, EventArgs e)
    {
        var today = DateTime.Now.DayOfWeek.ToString();
        var todayItem = _days.FirstOrDefault(d => d.DayName.Equals(today, StringComparison.OrdinalIgnoreCase));
        
        if (todayItem != null)
        {
            DaySelector.SelectedItem = todayItem;
        }
        else
        {
            // Weekend - show Monday
            DaySelector.SelectedItem = _days.FirstOrDefault(d => d.DayName == "Monday");
        }
    }

    // ViewModels for the page
    public class DayViewModel
    {
        private bool _isSelected;
        
        public string DayName { get; set; } = string.Empty;
        public string DayShort { get; set; } = string.Empty;
        
        public bool IsSelected 
        { 
            get => _isSelected;
            set
            {
                _isSelected = value;
                BackgroundColor = value ? Color.FromArgb("#4A90E2") : Color.FromArgb("#F8F9FA");
                TextColor = value ? Colors.White : Color.FromArgb("#7F8C8D");
            }
        }
        
        public Color BackgroundColor { get; private set; } = Color.FromArgb("#F8F9FA");
        public Color TextColor { get; private set; } = Color.FromArgb("#7F8C8D");
    }

    // Value Converters
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Color.FromArgb("#4A90E2") : Color.FromArgb("#F8F9FA");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Colors.White : Color.FromArgb("#7F8C8D");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
