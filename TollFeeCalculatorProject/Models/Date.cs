using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculatorProject.Models
{
    public class Date
    {
        public int Id { get; set; }

        public DateTime DateTime { get; private set; }

        public Date(DateTime dateTime)
        {
           DateTime = dateTime;
        }
    }
}
