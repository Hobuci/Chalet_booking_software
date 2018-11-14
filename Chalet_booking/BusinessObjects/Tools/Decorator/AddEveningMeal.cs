namespace BusinessObjects.Tools.Decorator
{
    public class EveningMeal_add : Decorator
    {
        private string cost;

        public override double CalculatePrice(double people, double nights, double hiredays)
        {//Add evening meal costs to the price
            double price = 10;
            price = price * people * nights;

            cost = price.ToString();
            //return the price of the evening meals plus the base price
            return base.CalculatePrice(people, nights, hiredays) + price;
        }

        public string getCost()
        {
            return cost;
        }
    }
}
