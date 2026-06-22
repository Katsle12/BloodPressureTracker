using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Android.OS;
using Blood_Pressure_Tracker.Data;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kotlin.Properties;
namespace Blood_Pressure_Tracker.ViewModel;

public partial class MainPageViewModel : ObservableObject
{
    //adattároló file
    private string path;
    
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

    //Egy mérés számainak átlaga formázva
    public string AvgText => $"Systolic: {AvgSys}, Diastolic: {AvgDia} Pulse: {AvgPul}";

    //ctor
    public MainPageViewModel()
    {
        _measurement = new Measurement(0, 0, 0,DateTime.Now); 
        _measurements = new ObservableCollection<Measurement>();
        _currentMeasurements = new ObservableCollection<Measurement>();
        
    }
    
    
    
    public async Task<string?> PickCsvFileAsync()
    {
        try
        {
            // Egyedi fájltípus definiálása a CSV-hez platformonként
            var csvFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    // Androidon a MIME-típusok határozzák meg a szűrést
                    { DevicePlatform.Android, new[] { "text/csv", "text/comma-separated-values", "application/csv" } },
                    // Ha később más platformra is portolnád (opcionális, de jó gyakorlat):
                    { DevicePlatform.WinUI, new[] { ".csv" } },
                    { DevicePlatform.iOS, new[] { "public.comma-separated-values-text" } },
                    { DevicePlatform.MacCatalyst, new[] { "public.comma-separated-values-text" } }
                });

            var options = new PickOptions
            {
                PickerTitle = "Válassz ki egy CSV fájlt",
                FileTypes = csvFileType
            };

            // Fájlválasztó megnyitása
            FileResult? result = await FilePicker.Default.PickAsync(options);

            if (result != null)
            {
                // Ha a felhasználó választott fájlt, visszatérünk az elérési úttal
                return result.FullPath;
            }
        }
        catch (Exception ex)
        {
            // Hibakezelés (pl. ha a felhasználó mégsem adott engedélyt a tárhelyhez)
            System.Diagnostics.Debug.WriteLine($"Hiba a fájlválasztás során: {ex.Message}");
        }

        // Ha megszakította a választást vagy hiba történt
        return null;
    }
    
    //Betöltés fájlból
    [RelayCommand]
    private async Task LoadFile()
    {
        string path = await PickCsvFileAsync();
        if (path != null)
        {
            var m = await Database.LoadData(path);
            foreach (var measurement in m)
                if (measurement != null)
                    _measurements.Add(measurement);
        }
        else Shell.Current.DisplayAlert("Hiba", "Fájl nem lett kiválasztva", "Ok");
    }
    
    //Adatok elmentése
    [RelayCommand]
    private async Task SaveFile()
    {
        if (path != null)
        {
            Database.WriteData(new List<Measurement>(Measurements), path);
        }
        else
        {
            // 1. Generáld le a CSV tartalmát egy stringbe/streambe
            StringBuilder csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Datum,Sistolic,Diastolic,Pulse");
            foreach (var m in Measurements)
            {
                csvBuilder.AppendLine($"{m.TimeOfRecording:yyyy-MM-dd HH:mm},{m.Systolic},{m.Diastolic},{m.Pulse}");
            }

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvBuilder.ToString()));

            // 2. Mentés ablak megnyitása
            var fileSaveResult = await FileSaver.Default.SaveAsync("vernyomas_export.csv", stream, CancellationToken.None);

            if (fileSaveResult.IsSuccessful)
            {
                 path = fileSaveResult.FilePath;
            }
        }
    }


    //Új mérés rögzítése
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