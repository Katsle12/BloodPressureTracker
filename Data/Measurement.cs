

namespace Blood_Pressure_Tracker.Data;

public class Measurement
{
    
    
    private int _systolic;
    private int _diastolic;
    private int _pulse;
    private DateTime timeOfRecording;

    public int Systolic => _systolic;

    public int Diastolic => _diastolic;

    public int Pulse => _pulse;
    
    public DateTime TimeOfRecording => timeOfRecording;

    public Measurement(int systolic, int diastolic, int pulse, DateTime timeOfRecording)
    {
        this._systolic = systolic;
        this._diastolic = diastolic;
        this._pulse = pulse;
        this.timeOfRecording = DateTime.Now;
    }

    public Measurement()
    {
        
    }
}