using PaylocityCodeAPI.Logic.Domain;

namespace PaylocityCodeAPI.Logic.Discounts
{
    public class NameDiscount : IDiscount
    {
        public void CheckForDiscount(IndividualCost individualCost)
        {
            if (individualCost.Individual.FirstName.ToLowerInvariant().StartsWith('a'))
            {
                individualCost.Discounts.Add(new Discount {Amount = individualCost.BaseCost*0.1m, Description = "Name starts with A"});
            }
        }
    }
}