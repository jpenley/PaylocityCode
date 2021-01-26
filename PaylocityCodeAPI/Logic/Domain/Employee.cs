namespace PaylocityCodeAPI.Logic.Domain
{
    public class Employee : Person
    {
        public string Title { get; set; }
        public int YearsOfService { get; set; }
        public decimal Salary { get; set; }
        public int PayPeriods { get; set; }
    }
}