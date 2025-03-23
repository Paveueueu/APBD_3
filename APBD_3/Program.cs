
using APBD_3.Container;

namespace APBD_3;

public static class Program
{
    private static readonly List<CargoShip> Ships = [];
    private static readonly List<CargoContainer> Containers = [];

    private static void PrintMenu()
    {
        Console.Clear();
        Console.WriteLine("Lista kontenerowców: ");
        if (Ships.Count == 0)
        {
            Console.WriteLine("Brak");
        }
        else
        {
            for (var i=0; i<Ships.Count; i++)
            {
                Console.WriteLine($"  {i+1}. kontenerowiec {Ships[i]}");
            }   
        }
        
        Console.WriteLine();
        Console.WriteLine("Lista kontenerów: ");
        if (Containers.Count == 0)
        {
            Console.WriteLine("Brak");
        }
        else
        {
            for (var i=0; i<Containers.Count; i++)
            {
                Console.WriteLine($"  {i+1}. kontener {Containers[i]}");
            }  
        }


        Console.WriteLine();
        Console.WriteLine("=== System ===");
        Console.WriteLine("1. Dodaj kontenerowiec");
        Console.WriteLine("2. Usuń kontenerowiec");
        Console.WriteLine("3. Dodaj kontener");
        Console.WriteLine("4. Usuń kontener");
        Console.WriteLine("5. Załaduj kontener/y na statek");
        Console.WriteLine("6. Rozładuj kontener/y ze statku");
        Console.WriteLine("7. Załaduj ładunek do kontenera");
        Console.WriteLine("8. Rozładuj ładunek z kontenera");
        Console.WriteLine("9. Zastąp kontener na statku innym kontenerem");
        Console.WriteLine("10. Przenieś kontener na inny statek");
        Console.WriteLine("11. Wypisz informacje o kontenerze");
        Console.WriteLine("12. Wypisz informacje o statku");
        Console.WriteLine("20. Wyjdź");
    }
    

    public static void Main()
    {
        var lastMsg = "Brak";
        
        while (true)
        {
            PrintMenu();
            
            
            Console.WriteLine("\nRezultat ostatniej operacji: ");
            Console.WriteLine($"  {lastMsg}\n");
            
            Console.Write("> ");
            var choice = Console.ReadLine();
        
            switch (choice)
            {
                case "1":
                    lastMsg = AddCargoShip();
                    break;
                case "2":
                    lastMsg = RemoveCargoShip();
                    break;
                case "3":
                    lastMsg = AddCargoContainer();
                    break;
                case "4":
                    lastMsg = RemoveCargoContainer();
                    break;
                case "5":
                    lastMsg = LoadContainerOnShip();
                    break;
                case "6":
                    lastMsg = UnloadContainerFromShip();
                    break;
                case "7":
                    lastMsg = LoadCargoToContainer();
                    break;
                case "8":
                    lastMsg = UnloadCargoFromContainer();
                    break;
                case "9":
                    lastMsg = ReplaceContainerOnShip();
                    break;
                case "10":
                    lastMsg = MoveContainerToOtherShip();
                    break;
                case "11":
                    lastMsg = PrintInfoOnContainer();
                    break;
                case "12":
                    lastMsg = PrintInfoOnShip();
                    break;
                case "20":
                    return;
                default:
                    lastMsg = "Nieprawidłowy wybór operacji!";
                    break;
            }
        }
    }
    
    private static string AddCargoShip()
    {
        Console.Clear();
        Console.WriteLine("Dodaj kontenerowiec");
        Console.Write("maxSpeedKnots(double): ");
        var maxSpeedKnots = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine();
        
        Console.Write("maxContainers(int): ");
        var maxContainers = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine();
        
        Console.Write("maxCargoMassT(double): ");
        var maxCargoMassT = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine();
        
        Ships.Add(new CargoShip(maxSpeedKnots, maxContainers, maxCargoMassT));
        return $"Dodano statek {Ships.Count}";
    }
    
    private static string RemoveCargoShip()
    {
        Console.Clear();
        Console.WriteLine("=== Usuwanie kontenerowca ===");
        Console.Write("numer(int): ");
        var index = Convert.ToInt32(Console.ReadLine());
        
        if (index < 0 || index > Ships.Count) return "Nieistniejący numer statku!";
        Ships.RemoveAt(index - 1);
        return $"Usunięto statek {index}";
    }
    
    private static string AddCargoContainer()
    {
        Console.Clear();
        Console.WriteLine("=== Dodawanie kontenera ===");
        
        Console.Write("Masa własna [kg] (double): ");
        var ownMass = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine();
        
        Console.Write("Wysokość [cm] (double): ");
        var height = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine();
        
        Console.Write("Głębokość [cm] (double): ");
        var depth = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine();
        
        Console.Write("Maksymalna masa ładunku [kg] (double): ");
        var maxCargoMass = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine();
        
        
        Console.Write("typ kontenera(L/G/C): ");
        var type = Console.ReadLine();
        Console.WriteLine();
        
        CargoContainer container;

        switch (type)
        {
            case "L" or "l":
                Console.Write("Ładunek niebezpieczny (default: NIE) (TAK/..): ");
                var isHazardous = Console.ReadLine() == "TAK";
                Console.WriteLine();
                container = new LiquidContainer(ownMass, height, depth, maxCargoMass, isHazardous);
                break;
            case "G" or "g":
                container = new GasContainer(ownMass, height, depth, maxCargoMass);
                break;
            case "C" or "c":
                Console.Write("Typ ładunku (string): ");
                var cargoType = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();

                Console.Write("Temperatura [degC] (double): ");
                var temperature = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine();

                container = new CoolingContainer(ownMass, height, depth, maxCargoMass, cargoType, temperature);
                break;
            default:
                return $"Nieistniejący typ kontenera: {type}";
        }

        Containers.Add(container);
        return $"Dodano kontener {container.SerialNumber}";
    }
    
    private static string RemoveCargoContainer()
    {
        Console.Clear();
        Console.WriteLine("=== Usuwanie kontenera ===");
        Console.Write("Numer seryjny: ");
        var serial = Console.ReadLine();
        
        if (serial == null) return "Nie znaleziono kontenera!";
        
        var found = Containers.Find( con => con.SerialNumber == serial);
        if (found == null) return "Nie znaleziono kontenera!";
        
        Containers.Remove(found);

        foreach (var ship in Ships.Where(ship => ship.IsContainerLoaded(serial)))
        {
            ship.Unload(serial);
            return $"Usunięto kontener {serial}";
        }
        
        return $"Usunięto kontener {serial}";
    }
    
    private static string LoadContainerOnShip()
    {
        Console.Clear();
        Console.WriteLine("=== Załadunek kontenera na kontenerowiec ===");
        Console.Write("Numer seryjny: ");
        var serial = Console.ReadLine();
        var found = Containers.Find( con => con.SerialNumber == serial);

        if (found == null) {
            return "Nie znaleziono kontenera!";
        }
        
        Console.Write("Statek: ");
        var index = Convert.ToInt32(Console.ReadLine());
        if (index > Ships.Count || index < 1) {
            return "Nieistniejący numer statku!";
        }

        try
        {
            Ships[index - 1].Load(found);
        }
        catch (OverfillException e)
        {
            return $"Nie załadowano kontenera {serial} na statek {index}. Zbyt duża masa kontenera";
        }
        
        return $"Załadowano kontener {serial} na statek {index}";
    } 
    
    private static string UnloadContainerFromShip()
    {
        Console.Clear();
        Console.WriteLine("=== Rozładunek kontenera z kontenerowca ===");
        Console.Write("Numer seryjny: ");
        var serial = Console.ReadLine();

        if (serial == null) {
            return "Nie znaleziono kontenera!";
        }
        
        Console.Write("Statek: ");
        var index = Convert.ToInt32(Console.ReadLine());
        if (index > Ships.Count || index < 1) {
            return "Nieistniejący numer statku!";
        }

        try
        {
            Ships[index - 1].Unload(serial);
        }
        catch (NullReferenceException e)
        {
            return $"Nie rozładowano kontenera {serial} ze statku {index}. Nie ma go na tym statku";
        }
        
        return $"Załadowano kontener {serial} na statek {index}";
    }
    
    private static string LoadCargoToContainer()
    {
        Console.Clear();
        Console.WriteLine("=== Załadunek ładunku do kontenera ===");
        Console.Write("Numer seryjny: ");
        var serial = Console.ReadLine();
        var found = Containers.Find( con => con.SerialNumber == serial);

        if (found == null) {
            return "Nie znaleziono kontenera!";
        }
        
        Console.Write("Masa ładunku [kg](double): ");
        var mass = Convert.ToDouble(Console.ReadLine());
        if (mass < 0)
        {
            return "Nie załadowano ładunku! Niewłaściwa masa";
        }
        
        try
        {
            found.LoadCargo(mass);
        }
        catch (OverfillException e)
        {
            return $"Nie załadowano ładunku do kontenera {serial}. Zbyt duża masa ładunku";
        }
        
        return $"Załadowano ładunek do kontenera {serial}";
    }
    
    private static string UnloadCargoFromContainer()
    {
        Console.Clear();
        Console.WriteLine("=== Rozładunek ładunku z kontenera ===");
        Console.Write("Numer seryjny: ");
        var serial = Console.ReadLine();
        var found = Containers.Find( con => con.SerialNumber == serial);

        if (found == null) {
            return "Nie znaleziono kontenera!";
        }
        
        found.UnloadCargo();
        
        return $"Rozładowano ładunek z kontenera {serial}";
    }
    
    private static string ReplaceContainerOnShip()
    {
        Console.Clear();
        Console.WriteLine("=== Zamiana kontenera na kontenerowcu ===");
        Console.Write("Numer seryjny: ");
        var serial = Console.ReadLine();
        
        if (serial == null)
        {
            return "Nie znaleziono kontenera!";
        }
        
        var found = Containers.Find( con => con.SerialNumber == serial);

        if (found == null)
        {
            return "Nie znaleziono kontenera!";
        }
        
        Console.Write("Numer seryjny nowego kontenera: ");
        var serialNew = Console.ReadLine();
        var foundNew = Containers.Find( con => con.SerialNumber == serialNew);
        
        if (foundNew == null) {
            return "Nie znaleziono kontenera!";
        }
        
        foreach (var ship in Ships.Where(ship => ship.IsContainerLoaded(serial)))
        {
            if (!ship.IsContainerLoaded(serial)) continue;
            ship.Replace(serial, foundNew);
            return $"Zastąpiono kontener {serial}, kontenerem {serialNew}";
        }
        
        return $"Nie zastąpiono kontenera {serial}! Nie ma go na żadnym statku";
    }
    
    private static string MoveContainerToOtherShip()
    {
        Console.Clear();
        Console.WriteLine("=== Przeniesienie kontenera na inny kontenerowiec ===");
        Console.Write("Numer seryjny: ");
        
        var serial = Console.ReadLine();
        if (serial == null)
        {
            return "Nie znaleziono kontenera!";
        }

        var found = Containers.Find( con => con.SerialNumber == serial);
        if (found == null)
        {
            return "Nie znaleziono kontenera!";
        }
        
        Console.Write("Nowy statek: ");
        var index = Convert.ToInt32(Console.ReadLine());
        if (index > Ships.Count || index < 1) {
            return "Nieistniejący numer statku!";
        }

        var old = 0;
        foreach (var ship in Ships.Where(ship => ship.IsContainerLoaded(serial)))
        {
            old++;
            ship.MoveContainer(serial, Ships[index - 1]);
            return $"Przeniesiono kontener {serial} ze statku {old} na statek {index}";
        }
        return "Nie przeniesiono kontenera!";
    }
    
    private static string PrintInfoOnContainer()
    {
        Console.Clear();
        Console.WriteLine("=== Informacja o kontenerze ===");
        Console.Write("Numer seryjny: ");
        var serial = Console.ReadLine();
        
        if (serial == null) return "Nie znaleziono kontenera!";
        var found = Containers.Find( con => con.SerialNumber == serial);
        if (found == null) return "Nie znaleziono kontenera!";

        Console.WriteLine($"  {found}");
        
        
        foreach (var ship in Ships.Where(ship => ship.IsContainerLoaded(serial)))
        {
            Console.WriteLine($"  Znajduje się na statku {ship}");
            return $"Wypisano info o kontenerze {serial}";
        }
        
        Console.WriteLine("  Nie Znajduje się na żadnym statku");
        return $"Wypisano info o kontenerze {serial}";
    }
    
    private static string PrintInfoOnShip()
    {
        Console.Clear();
        Console.WriteLine("=== Informacja o kontenerowcu ===");
        Console.Write("numer(int): ");
        var index = Convert.ToInt32(Console.ReadLine());
        
        if (index < 0 || index > Ships.Count) return "Nieistniejący numer statku!";
        
        Console.WriteLine(Ships[index]);
        
        return $"Wypisano info o statku {index}";
    }
}

