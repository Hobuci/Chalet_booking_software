namespace BusinessObjects.Tools.Decorator
{
    public class Breakfast_remove : Decorator
    {
        public override double CalculatePrice(double people, double nights, double hiredays)
        {//Add breakfast prices
            double price = 5;
            price = price * people * nights;

            return base.CalculatePrice(people, nights, hiredays) - price;
        }
    }
}
