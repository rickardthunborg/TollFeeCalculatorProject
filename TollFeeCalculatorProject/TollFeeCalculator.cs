using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculatorProject.Models;


namespace TollFeeCalculatorProject
{
    public class TollFeeCalculator : ITollFeeCalculator
    {
        public TollService TollService { get; set; }


        public TollFeeCalculator(TollService tollService)
        {
            TollService = tollService;
        }
        public int GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
            if (TollService.IsTollFreeDate(date)) return 0;

            var fee = TollService.TollFeeTimeLookup
                .FirstOrDefault(interval =>
                    (date.Hour > interval.StartHour || (date.Hour == interval.StartHour && date.Minute >= interval.StartMinute)) &&
                    (date.Hour < interval.EndHour || (date.Hour == interval.EndHour && date.Minute <= interval.EndMinute)))
                .TollFee;

            return fee;
        }
    } //By separating the toll calculator into more than one class we allow the solution to adhere to single responsibility principles, letting us keep classes focused on a single task
      //instead of grouping them together, which could lead to confusion when more code is developed and the solution gets more complicated.
}

