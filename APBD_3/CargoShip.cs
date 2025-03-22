using APBD_3.Container;

namespace APBD_3;

public class CargoShip (double maxSpeedKnots, int maxContainers, double maxCargoMassT)
{
    private readonly List<CargoContainer> _containers = [];
    private double MaxSpeedKnots { get; } = maxSpeedKnots;
    private int MaxContainers { get; } = maxContainers;
    private double MaxCargoMassT { get; } = maxCargoMassT;

    private double GetCargoMass()
    {
        return _containers.Sum(container => container.CargoMassKg + container.OwnMassKg);
    }

    public void Load(CargoContainer cargoContainer)
    {
        ArgumentNullException.ThrowIfNull("container null");
        if (cargoContainer.OwnMassKg + cargoContainer.CargoMassKg > MaxCargoMassT + GetCargoMass())
            throw new OverfillException("Trying to load container exceeding maximum cargo mass of the ship!");
        if (_containers.Count > MaxContainers)
            throw new OverfillException("Trying to load container exceeding maximum numbers of containers the ship!");
        
        _containers.Add(cargoContainer);
    }
    
    public void Load(List<CargoContainer> containers)
    {
        foreach (var container in containers)
            Load(container);
    }

    public bool IsContainerLoaded(string? serialNumber)
    {
        return serialNumber != null && _containers.Any(container => container.SerialNumber == serialNumber);
    }
    
    public CargoContainer Unload(string serialNumber)
    {
        var toRemove = _containers.Find(container => container.SerialNumber == serialNumber);
        if (toRemove == null)
            throw new NullReferenceException("No container found");
        
        _containers.Remove(toRemove);
        return toRemove;
    }
    
    public CargoContainer Replace(string serialNumber, CargoContainer newContainer)
    {
        ArgumentNullException.ThrowIfNull("serialNumber");
        ArgumentNullException.ThrowIfNull("newContainer");
        
        var toReplace = _containers.Find(container => container.SerialNumber == serialNumber);
        if (toReplace == null)
            throw new NullReferenceException("No container found");
        
        if (GetCargoMass() - toReplace.CargoMassKg + newContainer.CargoMassKg > MaxCargoMassT * 1000)
            throw new OverfillException("Trying to replace container exceeding maximum cargo mass of the ship!");
        
        _containers.Remove(toReplace);
        return toReplace;
    }

    public void MoveContainer(string serialNumber, CargoShip newShip)
    {
        ArgumentNullException.ThrowIfNull("serialNumber");
        ArgumentNullException.ThrowIfNull("newShip");
        
        var cargoContainer = _containers.Find(container => container.SerialNumber == serialNumber);
        
        if (cargoContainer == null)
            throw new NullReferenceException("No container found");
        
        _containers.Remove(cargoContainer);
        newShip.Load(cargoContainer);
    }

    
    public override string ToString()
    {
        return $"{{ " + 
               $"  Capacity: {_containers.Count}/{MaxContainers} " +
               $"  Max speed: {MaxSpeedKnots} kn" +
               $"  Cargo: {GetCargoMass()/1000} t / {MaxCargoMassT} t" +
               $" }}";
    }
}