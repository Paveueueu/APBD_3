
using APBD_3.Container;

namespace APBD_3;

public static class Program
{
    private static readonly List<CargoShip> Ships = [];
    private static readonly List<CargoContainer> Containers = [];

    public static void Main()
    {
        var lastMsg = "...";

        var operations = new Dictionary<string, Func<string>>
        {
            {"1", AddCargoShip}, {"2", RemoveCargoShip},
            {"3", AddCargoContainer}, {"4", RemoveCargoContainer},
            {"5", LoadContainerOnShip}, {"6", UnloadContainerFromShip},
            {"7", LoadCargoToContainer}, {"8", UnloadCargoFromContainer},
            {"9", ReplaceContainerOnShip}, {"10", MoveContainerToOtherShip},
            {"11", PrintInfoOnContainer}, {"12", PrintInfoOnShip},
        };
        
        while (true)
        {
            PrintMenu();
            
            Console.WriteLine("\nRezultat ostatniej operacji: ");
            Console.WriteLine($"  {lastMsg}");
            
            Console.Write("\n> ");
            var choice = Console.ReadLine();
            
            if (choice is "20" or null)
                return;

            PrintLists();
            try {
                var func = operations.GetValueOrDefault(choice);
                lastMsg = func != null ? func() : "Nieprawidłowy wybór operacji!";
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }

    private static void PrintLists()
    {
        Console.Clear();
        Console.WriteLine("\nLista kontenerowców: ");
        if (Ships.Count == 0) {
            Console.WriteLine("Brak");
        } else {
            for (var i=0; i<Ships.Count; i++)
                Console.WriteLine($" {i+1}. kontenerowiec {Ships[i]}");
        }
        
        Console.WriteLine("\nLista kontenerów: ");
        if (Containers.Count == 0) {
            Console.WriteLine("Brak");
        } else {
            for (var i=0; i<Containers.Count; i++)
                Console.WriteLine($" {i+1}. kontener {Containers[i]}");
        }
    }
    
    private static void PrintMenu()
    {
        PrintLists();
        
        Console.WriteLine("\n=== System ===" +
                          "\n1. Dodaj kontenerowiec" +
                          "\n2. Usuń kontenerowiec" +
                          "\n3. Dodaj kontener" +
                          "\n4. Usuń kontener" +
                          "\n5. Załaduj kontener na statek" +
                          "\n6. Rozładuj kontener ze statku" +
                          "\n7. Załaduj ładunek do kontenera" +
                          "\n8. Rozładuj ładunek z kontenera" +
                          "\n9. Zastąp kontener na statku innym kontenerem" +
                          "\n10. Przenieś kontener na inny statek" +
                          "\n11. Wypisz informacje o kontenerze" +
                          "\n12. Wypisz informacje o statku" +
                          "\n20. Wyjdź");
    }

    private static string InputString(string msg)
    {
        Console.Write(msg);
        var result = Console.ReadLine() ?? string.Empty;
        return result;
    }

    private static double InputDouble(string msg)
    {
        Console.Write(msg);
        var result = Convert.ToDouble(Console.ReadLine() ?? throw new ArgumentException("Niewłaściwy argument"));
        return result;
    }

    private static int InputInt(string msg)
    {
        Console.Write(msg);
        var result = Convert.ToInt32(Console.ReadLine() ?? throw new ArgumentException("Niewłaściwy argument"));
        return result;
    }
    
    private static CargoContainer? FindContainer(string serial)
    {
        return Containers.Find(con => con.SerialNumber == serial);
    }

    private static CargoShip? FindShip(int index)
    {
        return index >= 1 && index <= Ships.Count ? Ships[index - 1] : null;
    }
    
    private static CargoShip? FindShipWithContainer(string serial)
    {
        return Ships.Find(ship => ship.IsContainerLoaded(serial));
    }
    
    
    private static string AddCargoShip()
    {
        Console.WriteLine("\n=== Dodawanie kontenerowca ===");
        try {
            Ships.Add(new CargoShip(
                InputDouble("Max prędkość [kn] (double): "),
                InputInt("Max ilość kontenerów (int): "),
                InputDouble("Max masa ładunku [kg] (double): "))
            );
        } catch (Exception e) {
            return $"Nie dodano statku. {e.Message}";
        }
        return $"Dodano statek {Ships.Count}";
    }
    
    private static string RemoveCargoShip()
    {
        Console.WriteLine("\n=== Usuwanie kontenerowca ===");
        var index = InputInt("Numer statku (int): ");
        try {
            var ship = FindShip(index);
            if (ship == null)
                throw new ArgumentException("Nie ma takiego statku!");
            Ships.Remove(ship);
        } catch (Exception e) {
            return $"Nie usunięto statku. {e.Message}";
        }
        return $"Usunięto statek {index}";
    }
    
    private static string AddCargoContainer()
    {
        Console.WriteLine("\n=== Dodawanie kontenera ===");
        var ownMass = InputDouble("Masa własna [kg] (double): ");
        var height = InputDouble("Wysokość [cm] (double): ");
        var depth = InputDouble("Głębokość [cm] (double): ");
        var maxCargoMass = InputInt("Maksymalna masa ładunku [kg] (double): ");
        var type = InputString("typ kontenera(L/G/C): ");

        CargoContainer container;
        try {
            container = type.ToLower() switch {
                "l" => new LiquidContainer(ownMass, height, depth, maxCargoMass, 
                    InputString("Ładunek niebezpieczny (default: NIE) (TAK/..): ") == "TAK"),
                "g" => new GasContainer(ownMass, height, depth, maxCargoMass),
                "c" => new CoolingContainer(ownMass, height, depth, maxCargoMass, 
                    InputString("Typ ładunku (string): "), 
                    InputDouble("Temperatura [degC] (double): ")),
                _ => throw new ArgumentException($"Nieistniejący typ kontenera: {type}")
            };
            Containers.Add(container);
        } catch (Exception e) {
            return $"Nie dodano kontenera. {e.Message}";
        }
        return $"Dodano kontener {container.SerialNumber}";
    }
    
    private static string RemoveCargoContainer()
    {
        Console.WriteLine("\n=== Usuwanie kontenera ===");
        var container = FindContainer(InputString("Numer seryjny: "));
        
        try {
            if (container == null)
                throw new ArgumentException("Nie ma takiego kontenera!");
            
            Containers.Remove(container);
        } catch (Exception e) {
            return $"Nie usunięto kontenera. {e.Message}";
        }
        return $"Usunięto kontener {container.SerialNumber}";
    }
    
    private static string LoadContainerOnShip()
    {
        Console.WriteLine("\n=== Załadunek kontenera na kontenerowiec ===");
        var container = FindContainer(InputString("Numer seryjny: "));
        var ship = FindShip(InputInt("Statek: "));

        try {
            if (container == null)
                throw new ArgumentException("Nie ma takiego kontenera!");
            if (ship == null)
                throw new ArgumentException("Nie ma takiego statku!");
            
            ship.Load(container);
            Containers.Remove(container);
        } catch (Exception e) {
            return $"Nie załadowano kontenera na statek. {e.Message}";
        }
        return $"Załadowano kontener {container.SerialNumber} na statek";
    } 
    
    private static string UnloadContainerFromShip()
    {
        Console.WriteLine("\n=== Rozładunek kontenera z kontenerowca ===");
        var serial = InputString("Numer seryjny: ");
        var ship = FindShipWithContainer(serial);

        try {
            if (ship == null)
                throw new ArgumentException("Nie ma takiego statku!");
            
            ship.Unload(serial);
        } catch (Exception e) {
            return $"Nie rozładowano kontenera ze statku. {e.Message}";
        }
        return $"Rozładowano kontener {serial} ze statku";
    }
    
    private static string LoadCargoToContainer()
    {
        Console.WriteLine("\n=== Załadunek ładunku do kontenera ===");
        var container = FindContainer(InputString("Numer seryjny: "));
        var mass = InputDouble("Masa ładunku [kg](double): ");
        
        try {
            if (container == null)
                throw new ArgumentException("Nie ma takiego kontenera!");
            
            container.LoadCargo(mass);
        } catch (Exception e) {
            return $"Nie załadowano ładunku do kontenera. {e.Message}";
        }
        return $"Załadowano ładunek do kontenera {container.SerialNumber}";
    }
    
    private static string UnloadCargoFromContainer()
    {
        Console.WriteLine("\n=== Rozładunek ładunku z kontenera ===");
        var container = FindContainer(InputString("Numer seryjny: "));

        try {
            if (container == null)
                throw new ArgumentException("Nie ma takiego kontenera!");
            
            container.UnloadCargo();
        } catch (Exception e) {
            return $"Nie rozładowano ładunek z kontenera. {e.Message}";
        }
        return $"Rozładowano ładunek z kontenera {container.SerialNumber}";
    }
    
    private static string ReplaceContainerOnShip()
    {
        Console.WriteLine("\n=== Zamiana kontenera na kontenerowcu ===");
        var container = FindContainer(InputString("Numer seryjny: "));
        var newContainer = FindContainer(InputString("Numer seryjny nowego kontenera: "));
        var ship = FindShipWithContainer(container?.SerialNumber);
        
        try {
            if (container == null || newContainer == null)
                throw new ArgumentException("Nie ma takiego kontenera!");
            if (ship == null)
                throw new ArgumentException("Nie ma takiego statku!");
            
            ship.Replace(container.SerialNumber, newContainer);
            
        } catch (Exception e) {
            return $"Nie zastąpiono kontenera. {e.Message}";
        }
        return $"Zastąpiono kontener {container.SerialNumber}, kontenerem {newContainer.SerialNumber}";
    }
    
    private static string MoveContainerToOtherShip()
    {
        Console.WriteLine("\n=== Przeniesienie kontenera na inny kontenerowiec ===");
        var container = FindContainer(InputString("Numer seryjny: "));
        var ship = FindShipWithContainer(container?.SerialNumber);
        var newShip = FindShip(InputInt("Numer nowego statku: "));

        try {
            if (container == null)
                throw new ArgumentException("Nie ma takiego kontenera!");
            if (ship == null || newShip == null)
                throw new ArgumentException("Nie ma takiego statku!");
            
            ship.MoveContainer(container.SerialNumber, newShip);
        } catch (Exception e) {
            return $"Nie przeniesiono kontenera! {e.Message}";
        }
        return $"Przeniesiono kontener {container.SerialNumber}.";
    }
    
    private static string PrintInfoOnContainer()
    {
        Console.WriteLine("\n=== Informacja o kontenerze ===");
        var container = FindContainer(InputString("Numer seryjny: "));
        if (container == null) 
            return "Nie znaleziono kontenera!";
        
        Console.WriteLine($"  {container}");
        
        var ship = FindShipWithContainer(container.SerialNumber);
        if (ship != null) {
            Console.WriteLine($"  Znajduje się na statku {ship}");
            Console.WriteLine("...");
            Console.Read();
            return $"Wypisano info o kontenerze {container.SerialNumber}";
        }
        
        Console.WriteLine("  Nie znajduje się na żadnym statku");
        Console.WriteLine("Enter aby kontynuować...");
        Console.Read();
        return $"Wypisano info o kontenerze {container.SerialNumber}";
    }
    
    private static string PrintInfoOnShip()
    {
        Console.WriteLine("\n=== Informacja o kontenerowcu ===");
        var ship = FindShip(InputInt("Numer statku (int): "));
        if (ship == null) 
            return "Nie znaleziono statku!";
        
        Console.WriteLine(ship);
        
        Console.WriteLine("\nŁadunek: ");
        if (ship.Containers.Count == 0) {
            Console.WriteLine("Brak");
        } else {
            for (var i=0; i<ship.Containers.Count; i++)
                Console.WriteLine($" {i+1}. kontener {ship.Containers[i]}");
        }
        
        Console.WriteLine("Enter aby kontynuować...");
        Console.Read();
        return "Wypisano info o statku";
    }
}
