using System;
using System.Collections.Generic;
using System.Linq;

namespace dot_net_test
{
    class Program
    {
        static void Main(string[] args)
        {
            
            int numLots = 6;
            ParkingLot parkingLot = new ParkingLot(numLots);

            while (true)
            {
               Console.WriteLine("Available lots: {0}", parkingLot.GetAvailableLots());
               Console.WriteLine("Available commands:");
               Console.WriteLine("- park [Mobil/Motor]");
               Console.WriteLine("- leave [lot number]");
               Console.WriteLine("- type_of_vehicles [Mobil/Motor]");
               Console.WriteLine("- registration_numbers_for_vehicles_with_colour [color]");
               Console.WriteLine("- slot_numbers_for_vehicles_with_colour [color]");
               Console.WriteLine("- registration_numbers_for_vehicles_with_odd_plate");
               Console.WriteLine("- registration_numbers_for_vehicles_with_even_plate");
               Console.WriteLine("- exit");
                string input = Console.ReadLine();

                if (input.ToLower() == "exit")
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                else if (input.ToLower().StartsWith("park"))
                {
                    string licensePlate = input.Split(' ')[1];
                    string vehicleColor = input.Split(' ')[2].ToLower();
                    VehicleType vehicleType = (VehicleType) Enum.Parse(typeof(VehicleType), input.Split(' ')[3]);
                    parkingLot.ParkVehicle(vehicleType, licensePlate, vehicleColor);
                }
                else if (input.ToLower().StartsWith("leave"))
                {
                    int vehicleType = int.Parse(input.Split(' ')[1]);
                    parkingLot.RemoveVehicle(vehicleType);
                }
                else if (input.ToLower().StartsWith("type_of_vehicles"))
                {
                    VehicleType vehicleType = (VehicleType) Enum.Parse(typeof(VehicleType), input.Split(' ')[1]);
                    Console.WriteLine(  parkingLot.TypeOfVehicles(vehicleType));
          
                }
                else if (input.ToLower().StartsWith("registration_numbers_for_vehicles_with_colour"))
                {
                    string color = input.Split(' ')[1];
                    var res=parkingLot.RegistrationNumbersForVehiclesWithColour(color).Select(lot => lot.LicensePlate);
                    foreach (var data in res)
                    {
                        Console.WriteLine(data);
                    }
                }
                else if (input.ToLower().StartsWith("slot_numbers_for_vehicles_with_colour"))
                {
                    string color = input.Split(' ')[1];
                    var res = parkingLot.slot_numbers_for_vehicles_with_colour(color);
                    foreach (var data in res)
                    {
                        Console.WriteLine(data);
                    }
                }
                else if (input.ToLower() == "registration_numbers_for_vehicles_with_odd_plate")
                {
                    var res=parkingLot.RegistrationNumbersForVehiclesWithOodPlate();
                    foreach (var data in res)
                    {
                        Console.WriteLine(data);
                        
                    }
                }
                else if (input.ToLower() == "registration_numbers_for_vehicles_with_even_plate")
                {
                    var res=parkingLot.RegistrationNumbersForVehiclesWithEvenPlate();
                    foreach (var data in res)
                    {
                        Console.WriteLine(data);
                        
                    }
                }
                else
                {
                    Console.WriteLine("Unknown command.");
                }

                Console.WriteLine();
            }
        }
        public enum VehicleType 
        {
            Mobil,
            Motor
        }
        public class Lot
        {
            public bool IsAvailable { get; set; }
            public string VehicleType { get; set; }
            public string VechileColor { get; set; }
            public string LicensePlate { get; set; }
            public DateTime ParkedTime { get; set; }
            public int index { get;set; }
            public Lot()
            {
                IsAvailable = true;
                VehicleType = null;
                index = 1;
            }
        }
      
        public class ParkingLot
        {
            private List<Lot> _lots;

          
         
           
            public ParkingLot(int numLots)
            {
                _lots = new List<Lot>();
                for (int i = 0; i < numLots; i++)
                {
                    _lots.Add(new Lot());
                }

           

            }

            public int GetAvailableLots()
            {
                return _lots.Count(lot => lot.IsAvailable);
            }
            public int TypeOfVehicles(VehicleType vehicleType)
            {
                return _lots.Count(lot => lot.VehicleType==vehicleType.ToString());
            }
            public IEnumerable<Lot> RegistrationNumbersForVehiclesWithColour(string color)
            {
                return _lots.Where(lot => string.Equals(lot.VechileColor, color, StringComparison.OrdinalIgnoreCase));
            }
            public IEnumerable<int> slot_numbers_for_vehicles_with_colour(string color)
            {
                return _lots
                    .Select((lot, index) => new { index, Lot = lot })
                    .Where(pair => !string.IsNullOrEmpty(pair.Lot.VechileColor) && string.Equals(pair.Lot.VechileColor, color, StringComparison.OrdinalIgnoreCase))
                    .Select(pair => pair.index);
            }
            public int GetUnAvailableLots()
            {
                return _lots.Count(lot => !lot.IsAvailable);
            }
           
            public IEnumerable<string> RegistrationNumbersForVehiclesWithOodPlate()
            {
       
                return _lots.Where(lot => int.Parse(lot.LicensePlate.Split("-")[1]) % 2 == 1)
                    .Select(lot => lot.LicensePlate);
            }

            public IEnumerable<string> RegistrationNumbersForVehiclesWithEvenPlate()
            {
                return _lots.Where(lot => int.Parse(lot.LicensePlate.Split("-")[1]) % 2 == 0)
                    .Select(lot => lot.LicensePlate);
            }
            public void ParkVehicle(VehicleType vehicleType,string licensePlate,string vechileColor)
            {
                Lot availableLot = _lots.FirstOrDefault(lot => lot.IsAvailable);
                if (availableLot == null)
                {
                    Console.WriteLine("Sorry, the parking lot is full.");
       
                }
                else if (availableLot.VehicleType != null)
                {
                    Console.WriteLine("Sorry, this lot is already taken.");
                }
                else if (availableLot.index >= _lots.Count)
                {
                    Console.WriteLine("sorry,the park lot is full");
                }
                else
                {
                    availableLot.IsAvailable = false;
                    availableLot.VehicleType = vehicleType.ToString();
                    availableLot.LicensePlate = licensePlate;
                    availableLot.VechileColor = vechileColor;
                    availableLot.ParkedTime = DateTime.Now;
                    availableLot.index++;
                    Console.WriteLine("Vehicle parked successfully.");
                    Console.WriteLine("Allocated slot number: "+ availableLot.index++);
                }
            }

            public void RemoveVehicle(int index)
            {
                if (index >= _lots.Count)
                {
                    Console.WriteLine("Invalid slot number.");
                }
                else if (_lots[index].IsAvailable)
                {
                    Console.WriteLine("Slot {0} is already empty.", index);
                }
                else
                {
                    Lot occupiedLot = _lots[index];
                    Console.WriteLine($"Vehicle removed successfully. Parked time: {occupiedLot.ParkedTime}");
                    Console.WriteLine("lot :  "+index);
                    Console.WriteLine("type : "+occupiedLot.VehicleType);
                    Console.WriteLine("platNumber : "+occupiedLot.LicensePlate);
                    Console.WriteLine("charged for {0} Hours ",(int)(DateTime.Now - occupiedLot.ParkedTime).TotalHours==0?1:(int)(DateTime.Now - occupiedLot.ParkedTime).TotalHours+1);
                    occupiedLot.IsAvailable = true;
                    occupiedLot.VehicleType = null;
                    occupiedLot.VechileColor = null;
                    occupiedLot.LicensePlate = null;
       
                }
            }
        }
    }
}
