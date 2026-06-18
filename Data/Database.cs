namespace Blood_Pressure_Tracker.Data;

public class Database
{
    //CSV elérési útvonala
    private string path;

    public Database(string path)
    {
        this.path = path;
    }

    //Adatok betöltése a CSV fájlból
    public async Task<List<Measurement>> LoadData()
    {
        List<Measurement> measurements = new List<Measurement>();
        if(File.Exists(path))
        {
            string[] file = await File.ReadAllLinesAsync(path);
            foreach (string sor in file)
            {
                string[] columns = sor.Split(',');
                if (columns.Length == 4) 
                    measurements.Add(new Measurement(int.Parse(columns[0]), int.Parse(columns[1]), int.Parse(columns[2]), DateTime.Parse(columns[3])));
            }
        }
        return measurements;
    }
    
    //Adatok írása CSV fájlba
    public async Task WriteData(List<Measurement> measurements)
    {
        foreach (Measurement measurement in measurements)
        {
            //Ezt fogjuk a fájlba írni
            string toWrite = "";
            
            //Átalakítjuk stringé a kiírandó adatokat
            string[] columns =
            [
                measurement.Systolic.ToString(),measurement.Diastolic.ToString(), measurement.Pulse.ToString(),
                measurement.TimeOfRecording.ToString()
            ];
            
            //hozzáadjuk a kiírandó szöveghez
            for (int i = 0; i < columns.Length; i++)
            {
                if (i == columns.Length - 1)
                {
                    toWrite += columns[i];
                }
                else 
                {
                    toWrite += columns[i] + ",";
                }
            }
            await File.AppendAllLinesAsync(path, new []{toWrite});
        }
    }

}