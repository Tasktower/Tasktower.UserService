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
        public static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<ScopedServiceAttribute>() is not null);
            foreach (Type t in types)
            {
                services.AddScoped(t.GetInterface($"I{t.Name}"), t);
            }
            return services;
        }
    }
}
