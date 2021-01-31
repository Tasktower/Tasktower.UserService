using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tasktower.UserService.Utils.DependencyInjection
{
    public static class CustomServiceInjectExtension
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<BusinessServiceAttribute>() is not null);
            foreach (Type t in types)
            {
                services.AddScoped(t.GetInterface($"I{t.Name}"), t);
            }
            return services;
        }

        public static IServiceCollection AddBusinessRules(this IServiceCollection services)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<BusinessRulesAttribute>() is not null);
            foreach (Type t in types)
            {
                services.AddScoped(t.GetInterface($"I{t.Name}"), t);
            }
            return services;
        }
    }
}
