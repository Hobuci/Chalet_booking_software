namespace BusinessObjects.Tools.Decorator
{
    public class CarHire_add : Decorator
    {
        private string cost;

        public override double CalculatePrice(double people, double nights, double hiredays)
        {//Add breakfast prices
            double price = 50;
            price = price * hiredays;

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