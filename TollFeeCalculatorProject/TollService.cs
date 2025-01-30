using PublicHoliday;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculatorProject.Models;

namespace TollFeeCalculatorProject
{

    //By making a TollHelper or TollService we can move methods that support our main ones to a place where they won't directly sit and interfere, instead letting us call them remotely. 
    //This provides ease of maintenance and a cleaner codebase. It could even be argued that we should separate vehicle-related methods and date-related methods in the future for even more convenience.
    public class TollService
    {
        public static readonly List<(int StartHour, int StartMinute, int EndHour, int EndMinute, int TollFee)> TollFeeTimeLookup = new List<(int, int, int, int, int)>
        {
            (StartHour: 6, StartMinute: 0, EndHour: 6, EndMinute: 29, TollFee: 8), //Creating the intervals here allows us to keep this data in a single place instead of risking 
            (StartHour: 6, StartMinute: 30, EndHour: 6, EndMinute: 59, TollFee: 13), // having multiple places where we store them, forcing us to work on more code than necessary in the future.
            (StartHour: 7, StartMinute: 0, EndHour: 7, EndMinute: 59, TollFee: 18),
            (StartHour: 8, StartMinute: 0, EndHour: 8, EndMinute: 29, TollFee: 13),
            (StartHour: 8, StartMinute: 30, EndHour: 14, EndMinute: 59, TollFee: 8),
            (StartHour: 15, StartMinute: 0, EndHour: 15, EndMinute: 29, TollFee: 13),
            (StartHour: 15, StartMinute: 30, EndHour: 16, EndMinute: 59, TollFee: 18),
            (StartHour: 17, StartMinute: 0, EndHour: 17, EndMinute: 59, TollFee: 13),
            (StartHour: 18, StartMinute: 0, EndHour: 18, EndMinute: 29, TollFee: 8),
            (StartHour: 18, StartMinute: 30, EndHour: 05, EndMinute: 59, TollFee: 0),
        };

        public bool IsTollFree(Vehicle vehicle, DateTime date)
        {
            return vehicle.IsTollFree || IsTollFreeDate(date);
        }

        public bool IsTollFreeDate(DateTime date) //If it's Saturday, Sunday or a day in July, we will not charge the driver. By taking advantage of nugets/libraries in .NET we can remove the need for hardcoded days to
                                                  //keep track of and also check for every year ever. It is important to be aware of risks when using an external library as an implementation as it may not be reliable without verification.
        {
            if (date == null) throw new ArgumentNullException(nameof(date));
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || date.Month == 7) return true;

            bool isHoliday = new SwedenPublicHoliday().IsPublicHoliday(date); 
            if (isHoliday) return true;

            bool isDayBeforeHoliday = new SwedenPublicHoliday().IsPublicHoliday(date.AddDays(1));
            return isDayBeforeHoliday;
        }
    }
}
