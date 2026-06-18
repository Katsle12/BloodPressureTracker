using System.Collections.ObjectModel;
using System.Windows.Input;
using Android.OS;
using Blood_Pressure_Tracker.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kotlin.Properties;
namespace Blood_Pressure_Tracker.ViewModel;

public partial class MainPageViewModel : ObservableObject
{
    //database funckiók
    private Database db;
    
    //Összes mérés
    [ObservableProperty]
    private ObservableCollection<Measurement> _measurements;
    

    //Jelenlegi mérés
    [ObservableProperty]
    private Measurement _measurement;
    
    
    //Jelenlegi mérések
    [ObservableProperty]
    private ObservableCollection<Measurement> _currentMeasurements;

    
    //Átlagos értékek számítása
    public int AvgSys => CurrentMeasurements?.Any() == true 
        ? (int)CurrentMeasurements.Average(m => m.Systolic) 
        : 0;

    public int AvgDia => CurrentMeasurements?.Any() == true 
        ? (int)CurrentMeasurements.Average(m => m.Diastolic) 
        : 0;

    public int AvgPul => CurrentMeasurements?.Any() == true 
        ? (int)CurrentMeasurements.Average(m => m.Pulse) 
        : 0;

    public string AvgText => $"Systolic: {AvgSys}, Diastolic: {AvgDia} Pulse: {AvgPul}";

    protected MainPageViewModel()
    {
        //////////////////////////////
        /// Old meg az adattárolást
        /// //////////////////////////
        //db = new Database();
        _measurement = new Measurement(0, 0, 0,DateTime.Now);
        _measurements = new ObservableCollection<Measurement>(db.LoadData().Result);
    }

    [RelayCommand]
    private async Task RecordMeasurement()
    {
       //Ha minden 0
        if (Measurement.Systolic == 0 && Measurement.Diastolic == 0 && Measurement.Pulse == 0)
        {
            await Shell.Current.DisplayAlert("Hiba", "Az értékeknek nagyobbnak kell lennie 0-nál", "Ok");
            return; // Stops execution right here
        }

        // Ha valami 0
        if (Measurement.Systolic <= 0)
        {
            await Shell.Current.DisplayAlert("Hiba", "Systolic értéknek nagyobbnak kell lennie 0-nál", "Ok");
            return;
        }

        if (Measurement.Diastolic <= 0)
        {
            await Shell.Current.DisplayAlert("Hiba", "Diastolic értéknek nagyobbnak kell lennie 0-nál", "Ok");
            return;
        }

        if (Measurement.Pulse <= 0)
        {
            await Shell.Current.DisplayAlert("Hiba", "Pulse értéknek nagyobbnak kell lennie 0-nál", "Ok");
            return;
        }

        //Listához adás
        CurrentMeasurements.Add(Measurement);
        
        //Tulajdonságok frissítése
        OnPropertyChanged(nameof(AvgSys));
        OnPropertyChanged(nameof(AvgDia));
        OnPropertyChanged(nameof(AvgPul));
        OnPropertyChanged(nameof(AvgText));

    }

    


}