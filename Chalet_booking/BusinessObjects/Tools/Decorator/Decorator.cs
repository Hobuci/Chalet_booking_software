using System;

namespace BusinessObjects.Tools.Decorator
{
    [Serializable]
    public abstract class Decorator : BookingCost
    {
        /* Richard Borbely
         * Business Layer, Decorator design pattern, calculating Booking's base price and Extra's price
         * Last modified: 04 December 2017
         */
        protected BookingCost bookingcost;

        public void AddTo(BookingCost bookingcost)
        {
            this.bookingcost = bookingcost;
        }

        public override double CalculatePrice(double people, double nights, double hiredays)
        {
            return bookingcost.CalculatePrice(people, nights, hiredays);
        }
    }
}
