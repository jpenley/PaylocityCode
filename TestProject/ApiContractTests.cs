using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TestProject.Domain;
using Person = TestProject.Domain.Person;

namespace TestProject
{
    public class ApiContractTests
    {
        private readonly HttpClient _client;
        private readonly Faker _faker;

        public ApiContractTests()
        {
            var applicationFactory = new WebApplicationFactory<PaylocityCodeAPI.Startup>();
            _client = applicationFactory.CreateClient();
            _faker = new Faker();
        }

        [Test]
        public async Task TestWithDependents()
        {
            var request = new Request
            {
                Employee = new Person
                {
                    FirstName = _faker.Name.FirstName(),
                    LastName = _faker.Name.LastName()
                },
                Dependents = new List<Person>()
                {
                    new()
                    {
                        FirstName = _faker.Name.FirstName(),
                        LastName = _faker.Name.LastName()
                    },
                    new()
                    {
                        FirstName = _faker.Name.FirstName(),
                        LastName = _faker.Name.LastName()
                    }
                }
            };
            
            var response = await CallApi(request);

            response.Employee.Individual.FirstName.Should().BeEquivalentTo(request.Employee.FirstName);
            response.Employee.Individual.LastName.Should().BeEquivalentTo(request.Employee.LastName);
            response.Employee.BaseCost.Should().BeGreaterThan(0);
            response.Dependents.Should().HaveCount(2);
            response.Dependents.Single(dependent =>
                dependent.Individual.FirstName == request.Dependents[0].FirstName
                && dependent.Individual.LastName == request.Dependents[0].LastName);
            response.Dependents.Single(dependent =>
                dependent.Individual.FirstName == request.Dependents[1].FirstName
                && dependent.Individual.LastName == request.Dependents[1].LastName);
        }
        
        [Test]
        public async Task TestNoDependents()
        {
            var request = new Request
            {
                Employee = new Person
                {
                    FirstName = _faker.Name.FirstName(),
                    LastName = _faker.Name.LastName()
                },
                Dependents = new List<Person>()
            };

            var response = await CallApi(request);

            var cost = response.Employee.BaseCost;
            var dependents = response.Dependents;

            cost.Should().BeGreaterThan(0);
            dependents.Should().BeNull();
        }
        
        [Test]
        public async Task TestDiscountForNameStartingWithA()
        {
            var request = new Request
            {
                Employee = new Person
                {
                    FirstName = "A" + _faker.Name.FirstName(),
                    LastName = _faker.Name.LastName()
                },
                Dependents = new List<Person>()
                {
                    new()
                    {
                        FirstName = "A" + _faker.Name.FirstName(),
                        LastName = _faker.Name.LastName()
                    },
                    new()
                    {
                        FirstName = "D" + _faker.Name.FirstName(),
                        LastName = _faker.Name.LastName()
                    }
                }
            };
            
            var response = await CallApi(request);

            //This should only have one discount matching the filter.
            var employeeDiscount = response.Employee.Discounts.Single(d => d.Description.Equals("Name starts with A"));
            
            //This should only have one discount matching the filter.
            var dependent1 = response.Dependents
                .Single(dependent => dependent.Discounts.Any(discount => discount.Description.Equals("Name starts with A")));
            var dependent1Discounts = dependent1.Discounts.Single(d => d.Description.Equals("Name starts with A"));
            
            //This should have no match on the discount
            var dependent2 = response.Dependents
                .Where(dependent => dependent.Discounts.All(discount => !discount.Description.Equals("Name starts with A")));;
            dependent2.Should().HaveCount(1);
        }

        private async Task<Response> CallApi(Request request)
        {
            var json = await JsonConvert.SerializeObjectAsync(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/estimate", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<Response>(responseString);

            return responseObject;
        }
    }
}