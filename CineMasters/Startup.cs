using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Config;
using CineMasters.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using CineMasters.Models.Mollie.Services;
using AutoMapper;
using CineMasters.Models.Mollie.Middleware;

namespace CineMasters
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
            });

            services.AddMvc(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())).SetCompatibilityVersion(CompatibilityVersion.Version_2_2); ;

            services.AddSession(options =>
            {
                options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
            });

            var config = new ServerConfig();
            Configuration.Bind(config);

            var mongoDataContext = new MongoDataContext(config.MongoDB);

            #region Mollie
            string apiKey = Configuration.GetSection("MollieApiKey").Value;
            services.AddMollieApi(Configuration.GetSection("Mollie:MollieApiKey").Value);
            services.AddScoped<IPaymentOverviewClient, PaymentOverviewClient>();
            services.AddScoped<IPaymentStorageClient, PaymentStorageClient>();

            services.AddAutoMapper();
            #endregion

            services.AddSingleton<IMovieRepository>(new MovieRepository(mongoDataContext));
            services.AddSingleton<IShowRepository>(new ShowRepository(mongoDataContext));
            services.AddSingleton<IRoomRepository>(new RoomRepository(mongoDataContext));

            #region Mollie
            services.AddTransient<IPaymentOverviewClient, PaymentOverviewClient>();
            services.AddTransient<IPaymentStorageClient, PaymentStorageClient>();
            #endregion

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Cinemasters API",
                    Version = "v1",
                    Description = "Cinemasters API interface for development purposes",

                });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Show}/{action=Index}/{id?}");
            });
        }
    }
}
