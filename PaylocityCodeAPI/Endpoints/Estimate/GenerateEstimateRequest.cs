using System.Collections.Generic;
using PaylocityCodeAPI.Logic.Domain;

namespace PaylocityCodeAPI.Endpoints.Estimate
{
    public class GenerateEstimateRequest
    {
        public Employee Employee { get; set; }
        public List<Dependent> Dependents { get; set; }
    }
}