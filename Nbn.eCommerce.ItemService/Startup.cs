// <copyright file="Startup.cs" company="Nabin eCommerce API">
// Copyright (c) 2020 Nanina Trading Est. All rights reserved.
// </copyright>

namespace Nbn.eCommerce.ItemService
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using Nbn.eCommerce.ItemService.Common;
    using Nbn.eCommerce.ItemService.Repository;
    using Nbn.eCommerce.ItemService.Service;
    using Nbn.eCommerce.ItemService.Utility;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Startup class.
    /// </summary>
    public class Startup
    {

        private readonly ILogger<Startup> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// Returns queue information based on given values.
        /// </summary>
        /// <param name="configuration">configuration</param>
        /// <param name="fileLogger">filelogger</param>
        public Startup(IConfiguration configuration, ILogger<Startup> fileLogger)
        {
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.logger = fileLogger;
            this.logger.LogInformation("{Banner}", "Application Started");
        }

        /// <summary>
        /// Gets configuration handle
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection handle</param>
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                // This will allow calls to reach the controller's handler function
                // so that we can validate model state ourselves and respond with a consistent
                // response shape for valiation errors.
                services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });

                // Add Cross Origin Request policy that allows a client from any source domain to hit this service.
                services.AddCors(options =>
                {
                    options.AddPolicy(
                  "unrestricted-cors-policy",
                  builder => builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());
                });

                services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new EnumNameConverter());
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

                var connectionDict = new Dictionary<string, string>
                {
                    { DatabaseNameConstant.Nbm, this.Configuration.GetConnectionString(DatabaseNameConstant.Nbm) },
                    { DatabaseNameConstant.NbmKm, this.Configuration.GetConnectionString(DatabaseNameConstant.NbmKm) },
                };

                // Inject this dict
                services.AddScoped<IDbConnection>(db => new SqlConnection(this.Configuration.GetConnectionString(DatabaseNameConstant.Nbm)));

                services.AddSingleton<IDictionary<string, string>>(connectionDict);

                services.AddScoped<IBuildingMaterialRepository, BuildingMaterialRepository>();
                services.AddScoped<IKitchenRepository, KitchenRepository>();
                services.AddScoped<IItemsService, ItemsService>();
                services.AddScoped<IDivisionService, DivisionService>();

            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "CATCH {MethodFileLine}", MethodInfo.GetInfo());
            }

            services.AddSwaggerGen(c =>
            {
                c.DescribeAllParametersInCamelCase();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nabina API" });
                /// Configure Swagger to use the xml documentation file
                var xmlFile = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                c.IncludeXmlComments(xmlFile);
                c.DescribeAllEnumsAsStrings();
                ///c.OperationFilter<RequireBearerAuthentication>(); ;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nabina API");
                ////c.RoutePrefix = string.Empty;
            });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
