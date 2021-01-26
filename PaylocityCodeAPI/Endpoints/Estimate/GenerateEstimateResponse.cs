using System.Collections.Generic;
using PaylocityCodeAPI.Logic.Domain;

namespace PaylocityCodeAPI.Endpoints.Estimate
{
    public class GenerateEstimateResponse
    {
        public EmployeeCost Employee { get; set; }
        public List<IndividualCost> Dependents { get; set; }
    }
}