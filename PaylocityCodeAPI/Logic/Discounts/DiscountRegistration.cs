using System.Net.Sockets;
using Microsoft.Extensions.DependencyInjection;

namespace PaylocityCodeAPI.Logic.Discounts
{
    public static class DiscountRegistration
    {
        public static void Add(IServiceCollection services)
        {
            services.AddSingleton<IDiscount, NameDiscount>();
            
            // These two services were added to show an architecture I like for things with moderate chaos.
            // If there are likely to be a lot of changes driven by the business then we would look at something
            // more dynamic with a UI.
            
            services.AddSingleton<IDiscount, EmploymentLengthDiscount>();
            services.AddSingleton<IDiscount, TitleDiscount>();
            
        }
    }
}