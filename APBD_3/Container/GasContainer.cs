namespace APBD_3.Container;

public class GasContainer(
        double ownMassKg, double heightCm, double depthCm, double maxCargoMassKg
    ) : CargoContainer($"KON-G-{_nextSerialNumber++}", ownMassKg, heightCm, depthCm, maxCargoMassKg), IHazardNotifier
{
    private static int _nextSerialNumber = 1;
    
    public void Notify()
    {
        Console.WriteLine($"[HAZARD] container {SerialNumber}");
    }
    
    public override void LoadCargo(double mass)
    {
        if (mass > MaxCargoMassKg)
        {
            Notify();
            throw new OverfillException("Cargo exceeds maximum capacity.");
        }
        base.LoadCargo(mass);
    }

    public override void UnloadCargo()
    {
        CargoMassKg *= 0.05;
    }
}