namespace BusinessObjects.Tools.Decorator
{
    public class EveningMeal_remove : Decorator
    {
        public override double CalculatePrice(double people, double nights, double hiredays)
        {//Add evening meal costs to the price
            double price = 10;
            price = price * people * nights;

            return base.CalculatePrice(people, nights, hiredays) - price;
        }
    }
}
