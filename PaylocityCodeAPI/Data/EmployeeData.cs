using System;
using System.Text;
using PaylocityCodeAPI.Logic.Domain;

namespace PaylocityCodeAPI.Data
{
    public class EmployeeData : IEmployeeData
    {
        public Employee GetEmployee(Person person)
        {
            var rand = new Random();
            var yearsOfService = rand.Next(15);
            var title = buildTitle(rand.Next());
            return new Employee
            {
                Title = title,
                FirstName = person.FirstName,
                LastName = person.LastName,
                YearsOfService = yearsOfService,
                Salary = 52000,
                PayPeriods = 26
            };
        }

        private string buildTitle(int seed)
        {
            var sb = new StringBuilder();
            if (seed % 3 == 0)
            {
                sb.Append("Senior ");
            }
            if (seed % 4 == 0)
            {
                sb.Append("Product Owner");
            }
            else if (seed % 5 == 0)
            {
                sb.Append("Automation Engineer");
            }
            else
            {
                sb.Append("Software Engineer");
            }

            return sb.ToString();
        }
    }
}