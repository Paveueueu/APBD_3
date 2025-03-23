namespace APBD_3.Container;

public class CoolingContainer : CargoContainer
{
    private static int _nextSerialNumber = 1;
    private string CargoType { get; }
    private double TemperatureDegC { get; }
    
    public CoolingContainer(double ownMassKg, double heightCm, double depthCm, double maxCargoMassKg, string cargoType, double temperatureDegC)
        : base($"KON-C-{_nextSerialNumber++}", ownMassKg, heightCm, depthCm, maxCargoMassKg)
    {
        CargoType = cargoType;
        TemperatureDegC = temperatureDegC;
    }

    public override string ToString()
    {
        return base.ToString() + $"\n  Typ ładunku: {CargoType}  Temperatura: {TemperatureDegC} stC}}";
    }
}