
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

    private static string InputString(string msg)
    {
        Console.Write(msg);
        var result = Console.ReadLine() ?? string.Empty;
        Console.WriteLine();
        return result;
    }

    private static double InputDouble(string msg)
    {
        Console.Write(msg);
        var result = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine();
        return result;
    }

    private static int InputInt(string msg)
    {
        Console.Write(msg);
        var result = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine();
        return result;
    }
    
    private static CargoContainer? FindContainer(string serial)
    {
        return Containers.Find(con => con.SerialNumber == serial);
    }

    private static CargoShip? FindShip(int index)
    {
        return index > 0 && index <= Ships.Count ? Ships[index - 1] : null;
    }
    
    private static CargoShip? FindShipWithContainer(string? serial)
    {
        return serial == null ? null : Ships.FirstOrDefault(ship => ship.IsContainerLoaded(serial));
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
            Console.Clear();
            
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
        Console.WriteLine("=== Dodawanie kontenerowca ===");
        Ships.Add(new CargoShip(
            InputDouble("Max prędkość [kn] (double): "), 
            InputInt("Max kontenerów (int): "), 
            InputDouble("Max masa ładunku [kg] (double): "))
        );
        return $"Dodano statek {Ships.Count}";
    }
    
    private static string RemoveCargoShip()
    {
        Console.WriteLine("=== Usuwanie kontenerowca ===");
        var index = InputInt("Numer statku (int): ");
        
        var ship = FindShip(index);
        if (ship != null) Ships.Remove(ship);
        
        return $"Usunięto statek {index}";
    }
    
    private static string AddCargoContainer()
    {
        Console.WriteLine("=== Dodawanie kontenera ===");
        
        var ownMass = InputDouble("Masa własna [kg] (double): ");
        var height = InputDouble("Wysokość [cm] (double): ");
        var depth = InputDouble("Głębokość [cm] (double): ");
        var maxCargoMass = InputInt("Maksymalna masa ładunku [kg] (double): ");
        var type = InputString("typ kontenera(L/G/C): ");
        
        CargoContainer container;
        switch (type)
        {
            case "L" or "l":
                var isHazardous = InputString("Ładunek niebezpieczny (default: NIE) (TAK/..): ") == "TAK";
                container = new LiquidContainer(ownMass, height, depth, maxCargoMass, isHazardous);
                break;
            case "G" or "g":
                container = new GasContainer(ownMass, height, depth, maxCargoMass);
                break;
            case "C" or "c":
                var cargoType = InputString("Typ ładunku (string): ");
                var temperature = InputDouble("Temperatura [degC] (double): ");
                container = new CoolingContainer(ownMass, height, depth, maxCargoMass, cargoType, temperature);
                break;
            default:
                return $"Nieistniejący typ kontenera: {type}";
        }

        try {
            Containers.Add(container);
        }
        catch (Exception e) {
            return $"Nie dodano kontenera. {e.Message}";
        }

        return $"Dodano kontener {container.SerialNumber}";
    }
    
    private static string RemoveCargoContainer()
    {
        Console.WriteLine("=== Usuwanie kontenera ===");
        var serial = InputString("Numer seryjny: ");
        
        var container = FindContainer(serial);
        if (container != null) Containers.Remove(container);

        var ship = FindShipWithContainer(serial);
        if (ship == null) return $"Usunięto kontener {serial}";
        
        ship.Unload(serial);
        return $"Usunięto kontener {serial}";
    }
    
    private static string LoadContainerOnShip()
    {
        Console.WriteLine("=== Załadunek kontenera na kontenerowiec ===");
        var container = FindContainer(InputString("Numer seryjny: "));
        var ship = FindShip(InputInt("Statek: "));

        try {
            ship?.Load(container);
        } catch (Exception e) {
            return $"Nie załadowano kontenera {container?.SerialNumber} na statek. {e.Message}";
        }
        return $"Załadowano kontener {container?.SerialNumber} na statek";
    } 
    
    private static string UnloadContainerFromShip()
    {
        Console.WriteLine("=== Rozładunek kontenera z kontenerowca ===");
        var serial = InputString("Numer seryjny: ");
        var ship = FindShipWithContainer(serial);

        try {
            ship?.Unload(serial);
        } catch (Exception e) {
            return $"Nie rozładowano kontenera {serial} ze statku. {e.Message}";
        }
        return $"Rozładowano kontener {serial} ze statku";
    }
    
    private static string LoadCargoToContainer()
    {
        Console.WriteLine("=== Załadunek ładunku do kontenera ===");
        var container = FindContainer(InputString("Numer seryjny: "));
        
        var mass = InputDouble("Masa ładunku [kg](double): ");
        try {
            container?.LoadCargo(mass);
        } catch (Exception e) {
            return $"Nie załadowano ładunku do kontenera {container?.SerialNumber}. {e.Message}";
        }
        return $"Załadowano ładunek do kontenera {container?.SerialNumber}";
    }
    
    private static string UnloadCargoFromContainer()
    {
        Console.WriteLine("=== Rozładunek ładunku z kontenera ===");
        var container = FindContainer(InputString("Numer seryjny: "));
        container?.UnloadCargo();
        return $"Rozładowano ładunek z kontenera {container?.SerialNumber}";
    }
    
    private static string ReplaceContainerOnShip()
    {
        Console.WriteLine("=== Zamiana kontenera na kontenerowcu ===");
        var serial = InputString("Numer seryjny: ");
        var container = FindContainer(serial);
        var newContainer = FindContainer(InputString("Numer seryjny nowego kontenera: "));
        
        var ship = FindShipWithContainer(container?.SerialNumber);
        if (ship == null) return "Kontenera nie ma na statku!";

        try
        {
            ship.Replace(serial, newContainer);
            return $"Zastąpiono kontener {serial}, kontenerem {newContainer?.SerialNumber}";
        }
        catch (Exception e)
        {
            return $"Nie zastąpiono kontenera {serial}. {e.Message}";
        }
    }
    
    private static string MoveContainerToOtherShip()
    {
        Console.WriteLine("=== Przeniesienie kontenera na inny kontenerowiec ===");
        var container = FindContainer(InputString("Numer seryjny: "));
        var ship = FindShipWithContainer(container?.SerialNumber);
        var newShip = FindShip(InputInt("Numer nowego statku: "));

        try
        {
            if (container == null) throw new ArgumentException("Nie znaleziono kontenera!");
            if (ship == null) throw new ArgumentException("Nie znaleziono statku!");
            if (newShip == null) throw new ArgumentException("Nie znaleziono statku!");
            
            ship.MoveContainer(container.SerialNumber, newShip);
            return $"Przeniesiono kontener {container.SerialNumber}.";
        }
        catch (Exception e)
        {
            return $"Nie przeniesiono kontenera! {e.Message}";
        }
    }
    
    private static string PrintInfoOnContainer()
    {
        Console.WriteLine("=== Informacja o kontenerze ===");
        var container = FindContainer(InputString("Numer seryjny: "));
        if (container == null) return "Nie znaleziono kontenera!";
        
        Console.WriteLine($"  {container}");
        
        var ship = FindShipWithContainer(container.SerialNumber);
        if (ship != null)
        {
            Console.WriteLine($"  Znajduje się na statku {ship}");
            return $"Wypisano info o kontenerze {container.SerialNumber}";
        }
        
        Console.WriteLine("  Nie Znajduje się na żadnym statku");
        return $"Wypisano info o kontenerze {container.SerialNumber}";
    }
    
    private static string PrintInfoOnShip()
    {
        Console.WriteLine("=== Informacja o kontenerowcu ===");
        var ship = FindShip(InputInt("Numer statku (int): "));
        if (ship == null) return "Nie znaleziono statku!";
        
        Console.WriteLine(ship);
        return "Wypisano info o statku";
    }
}

