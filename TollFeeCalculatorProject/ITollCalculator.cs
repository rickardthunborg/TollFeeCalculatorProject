using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculatorProject.Models;

namespace TollFeeCalculatorProject
{
    public interface ITollCalculator
    {
        int GetTollFee(Vehicle vehicle, DateTime[] dates); 
    }
}

//Interfaces provide multiple benefits, among them are abstraction, decoupling or polymorphism. These are useful because they grant more flexibility and consistency for different
//implementations between components while retaining simplicity between them for testing and overall understanding of the code. 
