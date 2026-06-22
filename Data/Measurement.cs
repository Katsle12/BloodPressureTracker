

namespace Blood_Pressure_Tracker.Data;

public class Measurement
{
    
    
    public int Systolic { get; set; }
    public int Diastolic { get; set; }
    public int Pulse { get; set; }
    public DateTime TimeOfRecording { get; set; }

    public Measurement(int systolic, int diastolic, int pulse, DateTime timeOfRecording)
    {
        Systolic = systolic;
        Diastolic = diastolic;
        Pulse = pulse;
        TimeOfRecording = timeOfRecording;
    }

    public Measurement()
    {
        TimeOfRecording = DateTime.Now;
    }
}