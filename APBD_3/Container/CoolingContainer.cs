namespace APBD_3.Container;

public class CoolingContainer(
        double ownMassKg, double heightCm, double depthCm, double maxCargoMassKg,
        string cargoType, double temperatureDegC
    ) : CargoContainer($"KON-C-{_nextSerialNumber++}", ownMassKg, heightCm, depthCm, maxCargoMassKg)
{
    private static int _nextSerialNumber = 1;
    
    public string CargoType { get; set; } = cargoType;
    public double TemperatureDegC { get; set; } = temperatureDegC;
}