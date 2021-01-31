using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Tasktower.UserService.DataAccess;
using Tasktower.UserService.Errors.ErrorHandling;
using Tasktower.UserService.Utils.DependencyInjection;

namespace Tasktower.UserService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddUnitOfWork(options =>
            {
                options.DBContextOptions = new DbContextOptionsBuilder<DataAccess.DBAccessor.EntityFrameworkDBContext>()
                 .UseSqlServer(Configuration.GetConnectionString("mssqlconnection"))
                 .Options;
                options.CacheConnectionString = Configuration.GetConnectionString("redisMemStoreConn");
            });

            // Business services
            services.AddBusinessServices();

            // Routing services
            services.AddCors();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tasktower.UserService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();

            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tasktower.UserService v1"));
            }
            app.UseCustonErrorHandler(new ErrorHandleMiddlewareOptions { 
                ShowAllErrorMessages = env.IsDevelopment(), 
                UseStackTrace = !env.IsProduction() 
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
