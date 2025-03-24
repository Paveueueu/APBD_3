namespace APBD_3.Container;


public abstract class CargoContainer
{
    public string SerialNumber { get; }
    public double OwnMassKg { get; }
    private double HeightCm { get; }
    private double DepthCm { get; }
    public double CargoMassKg { get; protected set; }
    protected double MaxCargoMassKg { get; private set; }

    protected CargoContainer(string serialNumber, double ownMassKg, double heightCm, double depthCm, double maxCargoMassKg)
    {
        if (ownMassKg <= 0 || heightCm <= 0 || depthCm <= 0 || maxCargoMassKg <= 0)
            throw new ArgumentException("Niewłaściwe parametry kontenera!");
        
        SerialNumber = serialNumber;
        OwnMassKg = ownMassKg;
        HeightCm = heightCm;
        DepthCm = depthCm;
        CargoMassKg = 0;
        MaxCargoMassKg = maxCargoMassKg;
    }

    public virtual void LoadCargo(double mass)
    {
        if (mass > MaxCargoMassKg)
        {
            throw new OverfillException("Przekroczono dopuszczalny limit masy!");
        }
        if (mass < 0)
        {
            throw new ArgumentException("Niewłaściwa masa!");
        }
        CargoMassKg = mass;
    }

    public virtual void UnloadCargo()
    {
        CargoMassKg = MaxCargoMassKg = 0;
    }
    
    public override string ToString()
    {
        return $"{{" +
               $"  Serial: {SerialNumber}" +
               $"  Wysokość: {HeightCm} cm" +
               $"  Głębokość: {DepthCm} cm" +
               $"  Masa własna: {OwnMassKg} kg" +
               $"  Pojemność: {CargoMassKg} kg / {MaxCargoMassKg} kg" +
               $"  Masa całkowita: {OwnMassKg + CargoMassKg} kg";
    }
}