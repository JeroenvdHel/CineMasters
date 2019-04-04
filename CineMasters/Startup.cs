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
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CineMasters.Areas.Accounts.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

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
                options.IdleTimeout = TimeSpan.FromSeconds(120);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //services.AddMvc(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //.AddRazorPagesOptions(options => 
            //{
            //    options
            //});


            var config = new ServerConfig();
            Configuration.Bind(config);

            var mongoDataContext = new MongoDataContext(config.MongoDB);
            
            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetSection("UserIdentity:ConnectionString").Value, b =>
                b.MigrationsAssembly("CineMasters"));
            });
            services.AddIdentity<AppUser, IdentityRole>(options => {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppIdentityDbContext>()
              .AddDefaultTokenProviders();

            //services.ConfigureApplicationCookie(options =>
            //    options.LoginPath = "/User/Login");

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
            services.AddSingleton<ITicketRepository>(new TicketRepository(mongoDataContext));

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

            services.AddMemoryCache();
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
            //app.UseHttpContextItemsMiddleware();
            app.UseSwagger();
            app.UseAuthentication();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
            });

            app.UseMvc(routes =>
            {
                routes.MapAreaRoute(
                    name: "ShowArea",
                    areaName: "Shows",
                    template: "Shows/{controller}/{action}/{id?}",
                    defaults: new { controller = "Show", action = "AllShows" });

                routes.MapAreaRoute(
                    name: "AccountArea",
                    areaName: "Accounts",
                    template: "Accounts/{controller}/{action}/{id?}",
                    defaults: new { controller = "Account", action = "CreateAccount" });

                routes.MapAreaRoute(
                    name: "AdminArea",
                    areaName: "Admin",
                    template: "Admin/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                routes.MapAreaRoute(
                    name: "RegisterArea",
                    areaName: "Register",
                    template: "Register/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            });

            IdentitySeedData.EnsurePopulated(app);
        }
    }
}
