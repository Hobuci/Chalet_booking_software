namespace BusinessObjects.Tools.Decorator
{
    public class CarHire_remove : Decorator
    {
        public override double CalculatePrice(double people, double nights, double hiredays)
        {//Add breakfast prices
            double price = 50;
            price = price * hiredays;

            return base.CalculatePrice(people, nights, hiredays) - price;
        }
    }
}