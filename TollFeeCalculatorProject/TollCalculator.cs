using System;
using System.Globalization;
using System.Linq;
using TollFeeCalculatorProject;
using TollFeeCalculatorProject.Models;

public class TollCalculator : ITollCalculator
{

    private readonly ITollFeeCalculator _feeCalculator;
    private const int MaxDailyFee = 60;

    public TollCalculator(ITollFeeCalculator feeCalculator)
    {
        _feeCalculator = feeCalculator;
    }

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        if (vehicle == null || dates == null || dates.Length == 0)
            throw new ArgumentException("Vehicle and dates must not be null or empty.");

        if (vehicle.IsTollFree) return 0;

        // Ensure dates are from the same day
        if (dates.Any(d => d.Date != dates[0].Date))
            throw new Exception("Cannot process multiple days");

        dates = dates.OrderBy(d => d).ToArray();
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        int currentMaxFee = 0;

        foreach (var date in dates)
        {
            if (totalFee >= MaxDailyFee) return MaxDailyFee;

            int fee = _feeCalculator.GetTollFee(date, vehicle);
            if ((date - intervalStart).TotalMinutes <= 60)
            {
                // Update the current interval's max fee
                if (fee > currentMaxFee)
                {
                    totalFee += fee - currentMaxFee;
                    currentMaxFee = fee;
                }
            }
            else
            {
                // Move to the next interval
                totalFee += fee;
                intervalStart = date;
                currentMaxFee = fee;
            }
        }

        return Math.Min(totalFee, MaxDailyFee);
    }

}   