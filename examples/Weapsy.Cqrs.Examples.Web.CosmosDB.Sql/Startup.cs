﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Weapsy.Cqrs.EventStore.CosmosDB.Sql.Configuration;
using Weapsy.Cqrs.EventStore.CosmosDB.Sql.Extensions;
using Weapsy.Cqrs.Examples.Domain.Commands;
using Weapsy.Cqrs.Examples.Reporting.Queries;
using Weapsy.Cqrs.Examples.Shared;
using Weapsy.Cqrs.Extensions;

namespace Weapsy.Cqrs.Examples.Web.CosmosDB.Sql
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddWeapsyCqrs(typeof(CreateProduct), typeof(GetProduct));
            services.AddWeapsyCqrsEventStore(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDispatcher dispatcher, IOptions<EventStoreConfiguration> settings)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.EnsureEventStoreDbCreated(settings);

            app.Run(async (context) =>
            {
                // Create a sample product loading data from domain events.
                var product = await GettingStarted.CreateProduct(dispatcher);

                // Display product title.
                await context.Response.WriteAsync($"Product title: {product.Title}");
            });
        }
    }
}
