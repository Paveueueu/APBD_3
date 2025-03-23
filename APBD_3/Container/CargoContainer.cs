namespace APBD_3.Container;

public abstract class CargoContainer(
        string serialNumber, double ownMassKg, double heightCm, double depthCm, double maxCargoMassKg
    )
{
    public string SerialNumber { get; set; } = serialNumber;

    public double OwnMassKg { get; set; } = ownMassKg;
    private double HeightCm { get; set; } = heightCm;
    private double DepthCm { get; set; } = depthCm;

    public double CargoMassKg { get; set; }
    protected double MaxCargoMassKg { get; set; } = maxCargoMassKg;


    public virtual void LoadCargo(double mass)
    {
        if (mass > MaxCargoMassKg) 
            throw new OverfillException("Trying to load cargo exceeding maximum cargo mass!");
        
        CargoMassKg = mass;
    }
    
    public virtual void UnloadCargo() => CargoMassKg = MaxCargoMassKg = 0;

    public override string ToString()
    {
        return $"Container {{ \n" +
               $"  Serial number: {SerialNumber} \n" +
               $"  Height: {HeightCm} cm\n" +
               $"  Depth: {DepthCm} \n\n cm" +
               $"  Own mass: {OwnMassKg} kg\n" +
               $"  Cargo capacity: {CargoMassKg} kg / {MaxCargoMassKg} kg \n" +
               $"  Total mass: {OwnMassKg + CargoMassKg} kg \n" +
               $"}}";
    }
}