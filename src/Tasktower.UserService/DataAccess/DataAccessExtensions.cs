using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.DataAccess
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddUnitOfWork(
            this IServiceCollection serviceCollection,
            Action<UnitOfWorkOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options),
                    @"Please provide options for MyService.");
            }
            serviceCollection.Configure(options);
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            return serviceCollection;
        }
    }
}
