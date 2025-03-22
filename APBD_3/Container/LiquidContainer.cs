namespace APBD_3.Container;

public class LiquidContainer(
        double ownMassKg, double heightCm, double depthCm, double maxCargoMassKg,
        bool isHazardous
    ) : CargoContainer($"KON-L-{_nextSerialNumber++}", ownMassKg, heightCm, depthCm, maxCargoMassKg), IHazardNotifier
{
    private static int _nextSerialNumber = 1;

    private bool IsHazardous { get; set; } = isHazardous;

    public void Notify()
    {
        Console.WriteLine($"[HAZARD] container {SerialNumber}");
    }

    public override void LoadCargo(int mass)
    {
        if (IsHazardous && mass > MaxCargoMassKg / 2)
        {
            Notify();
            throw new OverfillException("Trying to load hazardous liquid cargo exceeding allowed limit!");
        }
        if (mass * 10 > MaxCargoMassKg * 9)
        {
            Notify();
            throw new OverfillException("Trying to load non-hazardous liquid cargo exceeding allowed limit!");
        }

        base.LoadCargo(mass);
    }
}