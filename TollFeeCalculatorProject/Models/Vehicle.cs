using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculatorProject.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string VehicleType { get; set; }
        public bool IsTollFree => TollFreeVehicleTypes.Contains(VehicleType);

        private static readonly HashSet<string> TollFreeVehicleTypes = new HashSet<string>
    {
        "Emergency",
        "Diplomat",
        "Military",
        "Police",
    };

        public Vehicle(int id, string registrationNumber, string vehicleType)
        {
            Id = id;
            RegistrationNumber = registrationNumber;
            VehicleType = vehicleType;
        }
    }
}
