namespace APBD_3.Container;

public class GasContainer(double ownMassKg, double heightCm, double depthCm, double maxCargoMassKg)
    : CargoContainer($"KON-G-{_nextSerialNumber++}", ownMassKg, heightCm, depthCm, maxCargoMassKg), IHazardNotifier
{
    private static int _nextSerialNumber = 1;
    
    public void Notify()
    {
        Console.WriteLine($"[UWAGA] kontener {SerialNumber}");
    }
    
    public override void LoadCargo(double mass)
    {
        if (mass > MaxCargoMassKg)
        {
            Notify();
            throw new OverfillException("Przekroczono dopuszczalny limit masy!");
        }
        base.LoadCargo(mass);
    }

    public override void UnloadCargo()
    {
        CargoMassKg *= 0.05;
    }
}