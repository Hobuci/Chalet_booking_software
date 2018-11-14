namespace BusinessObjects.Tools.Decorator
{
    public class Breakfast_add : Decorator
    {
        private string cost;

        public override double CalculatePrice(double people, double nights, double hiredays)
        {//Add breakfast prices
            double price = 5;
            price = price * people * nights;

            cost = price.ToString();
            //return price of the breakfasts plus the base price
            return base.CalculatePrice(people, nights, hiredays) + price;
        }

        public string getCost()
        {
            return cost;
        }
    }
}
