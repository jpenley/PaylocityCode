using PaylocityCodeAPI.Logic.Domain;

namespace PaylocityCodeAPI.Logic.Discounts
{
    public interface IDiscount
    {
        void CheckForDiscount(IndividualCost individualCost);
    }
}