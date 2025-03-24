namespace APBD_3.Container;

public class LiquidContainer : CargoContainer
{
    private static int _nextSerialNumber = 1;
    private bool IsHazardous { get; }

    public LiquidContainer(double ownMassKg, double heightCm, double depthCm, double maxCargoMassKg, bool isHazardous)
        : base($"KON-L-{_nextSerialNumber++}", ownMassKg, heightCm, depthCm, maxCargoMassKg)
    {
        IsHazardous = isHazardous;
    }

    private void Notify()
    {
        Console.WriteLine($"[UWAGA] kontener {SerialNumber}");
    }

    public override void LoadCargo(double mass)
    {
        var exp1 = IsHazardous && mass > MaxCargoMassKg / 2;
        var exp2 = mass * 10 > MaxCargoMassKg * 9;
        if (exp1 || exp2)
        {
            Notify();
            throw new OverfillException("Przekroczono dopuszczalny limit masy!");
        }
        if (mass < 0)
        {
            throw new ArgumentException("Niewłaściwa masa!");
        }
        base.LoadCargo(mass);
    }
    
    public override string ToString()
    {
        return base.ToString() + $"\n  Klasyfikacja: {(IsHazardous ? "niebezpieczny" : "bezpieczny")}}}";
    }
}