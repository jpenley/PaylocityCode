using System.Collections.Generic;

namespace PaylocityCodeAPI.Logic.Domain
{
    public class IndividualCost
    {
        public Person Individual { get; set; }
        public decimal BaseCost { get; set; }
        public List<Discount> Discounts { get; set; } = new List<Discount>();
    }
}