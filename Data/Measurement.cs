namespace Blood_Pressure_Tracker.Data;

public class Measurement
{
    public Measurement(int systole, int diastole, int pulse)
    {
        this.systole = systole;
        this.diastole = diastole;
        this.pulse = pulse;
    }

    public int Systole => systole;

    public int Diastole => diastole;

    public int Pulse => pulse;

    private int systole;
    private int diastole;
    private int pulse;
}