using System;

namespace BusinessObjects
{
    [Serializable]
    public class CarHire
    {
        /* Richard Borbely
         * Business Layer, Object for storing car hire information
         * Last modified: 04 December 2017
         * 
         */
        private string driverName;  //name of driver
        private DateTime hireStart; //hire starting date
        private DateTime hireEnd; //hire end date
        private DateTime arrivalDate; //booking's arrival date
        private DateTime departureDate; //booking's departure date

        public CarHire(DateTime arrival, DateTime departure)
        {//constructor will be called with booking's date to be able to verify that the car hire is within the booking date
            arrivalDate = arrival;
            departureDate = departure;
        }

        public string DriverName { get => driverName; set => driverName = value; }
        public DateTime HireStart
        {
            get => hireStart;
            set
            {
                if (value < arrivalDate || value > departureDate)
                {
                    throw new ArgumentException("Car hire date must be within the booking date");
                }
                hireStart = value;
            }
        }
        public DateTime HireEnd
        {
            get => hireEnd;
            set
            {
                if (value < arrivalDate || value > departureDate)
                {
                    throw new ArgumentException("Car hire date must be within the booking date");
                }
                hireEnd = value;
            }
        }
    }
}
