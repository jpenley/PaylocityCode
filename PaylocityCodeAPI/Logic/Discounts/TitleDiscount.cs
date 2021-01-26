using Microsoft.VisualBasic;
using PaylocityCodeAPI.Logic.Domain;

namespace PaylocityCodeAPI.Logic.Discounts
{
    public class TitleDiscount:IDiscount
    {
        public void CheckForDiscount(IndividualCost individualCost)
        {
            if (individualCost.Individual is not Employee employee) return;
            if (employee.Title.ToLowerInvariant().Contains("senior"))
            {
                individualCost.Discounts.Add(new Discount() {Amount = 100, Description = "Employee title discount"});
            }
        }
    }
}