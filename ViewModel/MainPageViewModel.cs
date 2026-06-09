using Blood_Pressure_Tracker.Data;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Blood_Pressure_Tracker.ViewModel;

public abstract class MainPageViewModel : ObservableObject
{
    public List<Measurement> Measurements => _measurements;

    public Measurement Measurement { get => _measurement;
        set { _measurement = value; OnPropertyChanged(); }
    }

    protected MainPageViewModel()
    {
        _measurement = new Measurement(0, 0, 0);
        _measurements = new List<Measurement>();
    }

    private List<Measurement> _measurements;
    private Measurement _measurement;

}