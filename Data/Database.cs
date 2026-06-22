namespace Blood_Pressure_Tracker.Data;

public static class Database
{
   

    //Adatok betöltése a CSV fájlból
    public static async Task<List<Measurement>> LoadData(string path)
    {
        List<Measurement> measurements = new List<Measurement>();
        if(File.Exists(path))
        {
            string[] file = await File.ReadAllLinesAsync(path);
            foreach (string sor in file)
            {
                string[] columns = sor.Split(',');
                if (columns.Length == 4)
                    try
                    {
                        measurements.Add(new Measurement(int.Parse(columns[0]), int.Parse(columns[1]), int.Parse(columns[2]), DateTime.Parse(columns[3])));
                    }
                    catch (Exception e)
                    {
                        break;
                    }
            }
        }
        return measurements;
    }
    
    //Adatok írása CSV fájlba
    public static async Task WriteData(List<Measurement> measurements, string path)
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