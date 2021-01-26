using System.Collections.Generic;

namespace TestProject.Domain
{
    public class Response
    {
        public IndividualCost Employee { get; set; }
        public List<IndividualCost> Dependents { get; set; }
    }
}