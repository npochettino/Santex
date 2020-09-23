using System;
using System.Threading;
using AutoMapper;
using Domain;
using Domain.Data;
using Domain.Mapping;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Services;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Santex
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
            services.AddAutoMapper(x => x.AddProfile(new MappingsProfile()));
            services.AddHttpClient("football", c =>
            {
                c.BaseAddress = new Uri(Configuration["Config:Url"]);
                c.DefaultRequestHeaders.Add("X-Auth-Token", Configuration["Config:Token"]);
            });

            //Configuration classes
            services.Configure<Config>(Configuration.GetSection("Config"));

            // Core
            services.AddDbContext<SantexContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(Configuration["ConnectionStrings:SANTEX"]);
            });

            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //Repositories
            services.AddHttpClient<IImportService, ImportService>();
            services.AddTransient<IPlayerRepository, PlayerRepository>();
            services.AddTransient<ICompetitionRepository, CompetitionRepository>();

            //Services
            services.AddTransient<IPlayerService, PlayerService>(); 
            services.AddTransient<ICompetitionService, CompetitionService>();
            services.AddTransient<IImportService, ImportService>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Santex API",
                    Description = "Santex Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Nicolas Pocettino",
                        Email = string.Empty,
                        Url = new Uri("https://npochettino.github.io/"),
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var optionsBuilder = new DbContextOptionsBuilder<SantexContext>();
            optionsBuilder.UseSqlServer(Configuration["ConnectionStrings:SANTEX"]);

            using (var client = new SantexContext(optionsBuilder.Options))
            {
                client.Database.EnsureCreated();
            }

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

        }
    }
}
