using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculatorProject.Models
{
    internal class TollRecord
    {
        public int Id { get; set; }
        public int Fee { get; set; }
        public int RegistrationNumber { get; set; }
        public DateTime Day { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
