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
using Tasktower.UserService.Security.Auth.Middleware;

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
                options.LocalCacheConnectionString = Configuration.GetConnectionString("redisMemStoreConn");
                options.SharedCacheConnectionString = Configuration.GetConnectionString("redisSharedMemStoreConn");
            });

            // Custom scoped services
            services.AddScopedServices();

            // Routing services
            services.AddCors();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tasktower.UserService", Version = "v1" });
                c.AddSecurityDefinition("XSRF Token", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = "X-XSRF-TOKEN",
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityDefinition("Access Token", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Cookie,
                    Name = "ACCESS-TOKEN", 
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityDefinition("Refresh Token", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Cookie,
                    Name = "REFRESH-TOKEN",
                    Type = SecuritySchemeType.ApiKey,
                });
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
                app.UseSwaggerUI(c => 
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tasktower.UserService v1");
                });
            }
            app.UseCustonErrorHandler(new ErrorHandleMiddlewareOptions { 
                ShowAllErrorMessages = env.IsDevelopment(), 
                UseStackTrace = !env.IsProduction() 
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseJWTMiddleware<IUnitOfWork>(options =>
            {
                options.KeyRetrieverAsync = async (kid, unitOfWork) =>
                {
                    string pem = await unitOfWork.AuthRSAPemPubKeyLocalCache.Get(kid);
                    if (string.IsNullOrEmpty(pem))
                    {
                        var getPemTask = unitOfWork.AuthRSAPemPubKeySharedCache.Get(kid);
                        var getExprTask = unitOfWork.AuthRSAPemPubKeySharedCache.TimeUntilExpire(kid);
                        pem = await getPemTask;
                        await unitOfWork.AuthRSAPemPubKeyLocalCache.Set(kid, pem, absoluteExpireTime: await getExprTask);
                    }
                    return pem;
                };
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
