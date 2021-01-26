using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PaylocityCodeAPI.Data;
using PaylocityCodeAPI.Logic.Discounts;
using PaylocityCodeAPI.Logic.Domain;
using Swashbuckle.AspNetCore.Annotations;

namespace PaylocityCodeAPI.Endpoints.Estimate
{
    public class GenerateEstimate : BaseAsyncEndpoint<GenerateEstimateRequest, GenerateEstimateResponse>
    {
        private readonly List<IDiscount> _discounts;
        private readonly IEmployeeData _employeeData;
        
        public GenerateEstimate(IServiceProvider services, IEmployeeData employeeData)
        {
            _discounts = services.GetServices<IDiscount>().ToList();
            _employeeData = employeeData;
        }

        [HttpPost("/estimate")]
        [SwaggerOperation(
                Summary = "Get an estimate for employee benefits",
                Description = "Estimate benefit costs for employee and dependents.",
                OperationId = "Estimate.Generate",
                Tags = new[] {"EstimateEndpoint"}
            )
        ]
        public override async Task<ActionResult<GenerateEstimateResponse>> HandleAsync(GenerateEstimateRequest request,
            CancellationToken cancellationToken)
        {
            var response = new GenerateEstimateResponse();

            var employee = _employeeData.GetEmployee(request.Employee);
            response.Employee = (EmployeeCost) GetCost(employee);
            GetDiscounts(response.Employee);
            
            if (!request.Dependents.Any()) return response;

            response.Dependents = new List<IndividualCost>();
            foreach (var dependent in request.Dependents)
            {
                var dependentCost = GetCost(dependent);
                GetDiscounts(dependentCost);
                response.Dependents.Add(dependentCost);
            }

            return response;
        }

        private IndividualCost GetCost(Person person)
        {
            return person switch
            {
                Employee employee => new EmployeeCost{Individual = employee, BaseCost = 1000},
                Dependent => new IndividualCost{Individual = person, BaseCost = 500},
                _ => throw new ArgumentOutOfRangeException(nameof(person))
            };
        }

        private void GetDiscounts(IndividualCost individualCost)
        {
            foreach (var discount in _discounts)
            {
                discount.CheckForDiscount(individualCost);
            }
        }
    }
}