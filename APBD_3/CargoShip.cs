using APBD_3.Container;

namespace APBD_3;

public class CargoShip
{
    private readonly List<CargoContainer> _containers = [];
    private double MaxSpeedKnots { get; }
    private int MaxContainers { get; }
    private double MaxCargoMassT { get; }

    public CargoShip(double maxSpeedKnots, int maxContainers, double maxCargoMassT)
    {
        if (maxSpeedKnots <= 0 || maxContainers <= 0 || maxCargoMassT <= 0)
            throw new ArgumentException("Niewłaściwe parametry statku!");
        
        MaxSpeedKnots = maxSpeedKnots;
        MaxContainers = maxContainers;
        MaxCargoMassT = maxCargoMassT;
    }

    private double GetCargoMass()
    {
        return _containers.Sum(container => container.CargoMassKg + container.OwnMassKg);
    }
    
    public void Load(CargoContainer? cargoContainer)
    {
        if (cargoContainer == null)
            throw new ArgumentException("Nieistniejący kontener!");
        if (cargoContainer.OwnMassKg + cargoContainer.CargoMassKg > MaxCargoMassT + GetCargoMass())
            throw new OverfillException("Kontener przewyższa dozwoloną masę ładunku statku!");
        if (_containers.Count > MaxContainers)
            throw new OverfillException("Kontener przewyższa dozwoloną liczbę kontenerów na statku!");
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
    
    public void Unload(string serialNumber)
    {
        var toRemove = _containers.Find(container => container.SerialNumber == serialNumber);
        if (toRemove == null)
            throw new NullReferenceException("Na tym statku nie ma tego kontenera!");
        
        _containers.Remove(toRemove);
    }
    
    public void Replace(string serialNumber, CargoContainer? newContainer)
    {
        var toReplace = _containers.Find(container => container.SerialNumber == serialNumber);
        if (toReplace == null)
            throw new NullReferenceException("Na tym statku nie ma tego kontenera!");
        if (newContainer == null)
            throw new NullReferenceException("Nieistniejący kontener!");
        
        if (GetCargoMass() - toReplace.CargoMassKg + newContainer.CargoMassKg > MaxCargoMassT * 1000)
            throw new OverfillException("Kontener przewyższa dozwoloną masę ładunku nowego statku!");
        
        _containers.Remove(toReplace);
        _containers.Add(newContainer);
    }

    public void MoveContainer(string serialNumber, CargoShip newShip)
    {
        var cargoContainer = _containers.Find(container => container.SerialNumber == serialNumber);
        
        if (cargoContainer == null)
            throw new NullReferenceException("Na tym statku nie ma tego kontenera!");
        
        _containers.Remove(cargoContainer);
        newShip.Load(cargoContainer);
    }

    
    public override string ToString()
    {
        return $"{{ " + 
               $"  Pojemność: {_containers.Count}/{MaxContainers} kontenerów" +
               $"  Max prędkość: {MaxSpeedKnots} kn" +
               $"  Masa ładunku: {GetCargoMass()/1000} t / {MaxCargoMassT} t" +
               $" }}";
    }
}