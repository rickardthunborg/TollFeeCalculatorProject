using System.Diagnostics;
using TollFeeCalculatorProject;
using TollFeeCalculatorProject.Models;

namespace TollFeeCalculatorProjectTests
{
    public class TollFeeCalculatorProjectTests
    {
        private readonly TollService _tollService;
        private readonly ITollFeeCalculator _feeCalculator;
        private readonly TollCalculator _calculator;

        public TollFeeCalculatorProjectTests()
        {
            _tollService = new TollService();
            _feeCalculator = new TollFeeCalculatorProject.TollFeeCalculator(_tollService);
            _calculator = new TollCalculator(_feeCalculator);
        }

        [Fact]
        public void CalculateFee_ForTollFreeVehicle_ShouldBeZero()
        {
            Vehicle motorcycle = new Vehicle(2, "EM123", "Emergency");
            DateTime[] dates = {
                new DateTime(2024, 10, 28, 7, 30, 0)
            };

            int tollFee = _calculator.GetTollFee(motorcycle, dates);

            Assert.Equal(0, tollFee);
        }
        [Fact]
        public void CalculateFee_OnPublicHoliday_ShouldBeZero()
        {
            Vehicle car = new Vehicle(1, "123ABC", "Car");
            DateTime[] dates = {
                new DateTime(2024, 12, 25, 8, 0, 0) // Christmas Day
            };

            int tollFee = _calculator.GetTollFee(car, dates);

            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void CalculateFee_OnWeekend_ShouldBeZero()
        {
            Vehicle car = new Vehicle(1, "123ABC", "Car");
            DateTime[] dates = {
                new DateTime(2024, 10, 27, 14, 0, 0) // Sunday
            };

            int tollFee = _calculator.GetTollFee(car, dates);

            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void CalculateFee_ExceedsDailyCap_ShouldBeMaxFee()
        {
            Vehicle car = new Vehicle(1, "123ABC", "Car");
            DateTime[] dates = {
                new DateTime(2024, 10, 28, 6, 30, 0),
                new DateTime(2024, 10, 28, 7, 15, 0),
                new DateTime(2024, 10, 28, 8, 45, 0),
                new DateTime(2024, 10, 28, 15, 30, 0),
                new DateTime(2024, 10, 28, 17, 15, 0),
                new DateTime(2024, 10, 28, 18, 29, 0)
            };

            int tollFee = _calculator.GetTollFee(car, dates);

            Assert.Equal(60, tollFee); 
        }

        [Fact]
        public void CalculateFee_ForSameInterval_ShouldTakeHighestFee()
        {
            Vehicle car = new Vehicle(1, "123ABC", "Car");
            DateTime[] dates = {
                new DateTime(2024, 10, 28, 7, 15, 0),
                new DateTime(2024, 10, 28, 7, 45, 0) // Both in the same toll interval
            };

            int tollFee = _calculator.GetTollFee(car, dates);

            Assert.Equal(18, tollFee); // Replace with the highest fee for the interval
        }

        [Fact]
        public void CalculateFee_EmptyDatesArray_ShouldBeZero()
        {
            Vehicle car = new Vehicle(1, "123ABC", "Car");
            DateTime[] dates = { };

            var exception = Assert.Throws<ArgumentException>(() => _calculator.GetTollFee(car, dates));

            Assert.Equal("Vehicle and dates must not be null or empty.", exception.Message);
        }

        [Fact]
        public void CalculateFee_CrossDayEntries_ShouldNotAddFees()
        {
            Vehicle car = new Vehicle(1, "123ABC", "Car");
            DateTime[] dates = {
                new DateTime(2024, 10, 28, 23, 59, 0),
                new DateTime(2024, 10, 29, 0, 15, 0) // Crosses into next day
            };

            var exception = Assert.Throws<Exception>(() => _calculator.GetTollFee(car, dates));


            Assert.Equal("Cannot process multiple days", exception.Message); // Toll fees reset daily
        }
    }
}