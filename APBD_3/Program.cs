
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
                Console.WriteLine($"  {i}. kontenerowiec {Ships[i]}");
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
                Console.WriteLine($"  {i}. kontener {Containers[i]}");
            }  
        }


        Console.WriteLine();
        Console.WriteLine("=== System ===");
        Console.WriteLine("1. Dodaj kontenerowiec");
        Console.WriteLine("2. Usuń kontenerowiec");
        Console.WriteLine("3. Dodaj kontener");
        Console.WriteLine("4. Usuń kontener");
        
    }
    

    public static void Main()
    {
        while (true)
        {
            PrintMenu();
        
            var choice = Console.ReadLine();
        
            switch (choice)
            {
                case "1":
                    AddCargoShip();
                    break;
                case "2":
                    RemoveCargoShip();
                    break;
                case "3":
                    AddCargoContainer();
                    break;
                case "4":
                    RemoveCargoContainer();
                    break;
                
                case "69":
                    return;
                

                default:
                    Console.WriteLine("Nieprawidłowy wybór! Naciśnij dowolny klawisz by kontynuować...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void AddCargoShip()
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
    }
    
    private static void RemoveCargoShip()
    {
        Console.Clear();
        Console.WriteLine("Usuń kontenerowiec");
        Console.Write("numer(int): ");
        var index = Convert.ToInt32(Console.ReadLine());
        
        Ships.RemoveAt(index - 1);
    }
    
    private static void AddCargoContainer()
    {
        Console.Clear();
        Console.WriteLine("Dodaj kontener");
        
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
                Console.Write($"Nieistniejący typ kontenera: {type}");
                return;
        }

        Containers.Add(container);
    }
    
    private static void RemoveCargoContainer()
    {
        Console.Clear();
        Console.WriteLine("Usuń kontener");
        Console.Write("numer (int): ");
        var index = Convert.ToInt32(Console.ReadLine());
        
        Containers.RemoveAt(index - 1);
    }


    public static void Test()
    {
        try
        {
            CargoShip ship1 = new CargoShip(18, 100, 40000);
            Console.WriteLine("Created CargoShip: " + ship1 + "\n");
            
            CoolingContainer bananaContainer = new CoolingContainer(3000, 170, 500, 10000, "Banana", 4);
            Console.WriteLine("Created CoolingContainer: " + bananaContainer + "\n");
            LiquidContainer liquidContainer = new LiquidContainer( 1500, 200, 600, 8000, false);
            Console.WriteLine("Created LiquidContainer: " + liquidContainer + "\n");
            LiquidContainer hazardousLiquidContainer = new LiquidContainer( 2000, 200, 600, 8000, true);
            Console.WriteLine("Created LiquidContainer: " + hazardousLiquidContainer + "\n");
            GasContainer gasContainer = new GasContainer( 2000, 180, 550, 5000);
            Console.WriteLine("Created GasContainer: " + gasContainer + "\n");
            
            bananaContainer.LoadCargo(900);
            Console.WriteLine("Loaded cargo into bananaContainer: " + bananaContainer + "\n");
            liquidContainer.LoadCargo(1500);
            Console.WriteLine("Loaded cargo into liquidContainer: " + liquidContainer + "\n");
            gasContainer.LoadCargo(450);
            Console.WriteLine("Loaded cargo into gasContainer: " + gasContainer + "\n");
            
            ship1.Load(bananaContainer);
            Console.WriteLine("Loaded container bananaContainer on ship1: " + ship1 + "\n");
            ship1.Load(liquidContainer);
            Console.WriteLine("Loaded container liquidContainer on ship1: " + ship1 + "\n");
            ship1.Load(gasContainer);
            Console.WriteLine("Loaded container gasContainer on ship1: " + ship1 + "\n");
            
            ship1.Unload(bananaContainer.SerialNumber);
            Console.WriteLine("Unloaded container bananaContainer from ship1: " + ship1 + "\n");
            
            try {
                Console.WriteLine("Trying to load too much cargo into gasContainer");
                gasContainer.LoadCargo(60000);
            }
            catch (OverfillException ex) {
                Console.WriteLine("ERROR: " + ex.Message + "\n");
            }
            
            CoolingContainer appleContainer = new CoolingContainer(3000, 170, 500, 10000, "Apple", 4);
            ship1.Replace(liquidContainer.SerialNumber, appleContainer);
            Console.WriteLine("Replaced liquidContainer with appleContainer\n");
            Console.WriteLine(ship1);
            
            CargoShip ship2 = new CargoShip(20, 100, 40000);
            ship1.MoveContainer(gasContainer.SerialNumber, ship2);
            Console.WriteLine("Moved gasContainer to ship2\n");
            Console.WriteLine(ship1);
            Console.WriteLine(ship2);
            
            List<CargoContainer> containerList =
            [
                new CoolingContainer(2500, 160, 400, 9000, "Grapes", 3),
                new LiquidContainer(1200, 180, 500, 7000, false),
                new GasContainer(1800, 170, 520, 4500)
            ];
            
            ship1.Load(containerList);
            Console.WriteLine("Loaded a list of containers on ship1:\n" + ship1);
            
            CoolingContainer orangeContainer = new CoolingContainer(2800, 165, 480, 9500, "Orange", 5);
            LiquidContainer juiceContainer = new LiquidContainer(1300, 190, 520, 7500, false);
            
            ship1.Load(orangeContainer);
            ship1.Load(juiceContainer);
            Console.WriteLine("Loaded individual containers on ship1:\n" + ship1 );
        }
        catch (Exception ex) {
            Console.WriteLine("ERROR: " + ex.Message);
        }
    }
}

