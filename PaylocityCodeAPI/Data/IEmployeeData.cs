using System;
using PaylocityCodeAPI.Logic.Domain;

namespace PaylocityCodeAPI.Data
{
    public interface IEmployeeData
    {
        Employee GetEmployee(Person person);
    }
}