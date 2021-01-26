using PaylocityCodeAPI.Logic.Domain;

namespace PaylocityCodeAPI.Logic.Discounts
{
    public class EmploymentLengthDiscount : IDiscount
    {
        public void CheckForDiscount(IndividualCost individualCost)
        {
            if (individualCost.Individual is not Employee employee) return;
            switch (employee)
            {
                case {YearsOfService: > 10}:
                    individualCost.Discounts.Add(new Discount() {Amount = 200, Description = "10 year discount"});
                    break;
                case {YearsOfService: > 5}:
                    individualCost.Discounts.Add(new Discount() {Amount = 100, Description = "5 year discount"});
                    break;
            }
        }
    }
}