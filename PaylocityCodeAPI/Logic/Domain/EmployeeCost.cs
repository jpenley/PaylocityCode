namespace PaylocityCodeAPI.Logic.Domain
{
    public class EmployeeCost:IndividualCost
    {
        private Employee _employee;
        
        public new Employee Individual
        {
            get => _employee;
            set
            {
                _employee = value;
                base.Individual = value;
            }
        }
    }
}