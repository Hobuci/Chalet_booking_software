using System;

namespace BusinessObjects.Tools.Decorator
{
    [Serializable]
    public abstract class BookingCost
    {
        public abstract double CalculatePrice(double people, double nights, double hiredays);
    }
}
