using Blood_Pressure_Tracker.ViewModel;

namespace Blood_Pressure_Tracker;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}