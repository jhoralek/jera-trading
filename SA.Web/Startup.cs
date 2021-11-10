using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using SA.Application.Email;
using SA.Application.Security;
using SA.Core.Model;
using SA.Core.Security;
using SA.EntityFramework.EntityFramework;
using SA.EntityFramework.EntityFramework.Repository;
using SA.Web.Models;
using System;
using System.Security.Claims;

namespace SA.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string domain = $"https://{_configuration["Auth0:Domain"]}/";

            services.Configure<KestrelServerOptions>(_configuration.GetSection("Kestrel"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = _configuration["Auth0:Audience"];
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    SaveSigninToken = true,
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain)));
                options.AddPolicy("admin", policy => policy.Requirements.Add(new HasScopeRequirement("admin", domain)));
            });

            var connectionString = _configuration["ConnectionString:CS"];

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 26));

            services.AddDbContext<SaDbContext>(options => options
                .UseMySql(connectionString, serverVersion)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors(),
                ServiceLifetime.Transient);

            services.AddControllersWithViews()
                .AddControllersAsServices()
                .AddNewtonsoftJson(a =>
                {
                    a.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    a.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                    a.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddRazorPages();

            services.AddTransient<IEntityRepository<Country>, CountriesRepository>();
            services.AddTransient<IEntityRepository<Address>, AddressesRepository>();
            services.AddTransient<IEntityRepository<User>, UsersRepository>();
            services.AddTransient<IEntityRepository<Customer>, CustomersRepository>();
            services.AddTransient<IEntityRepository<Bid>, BidsRepository>();
            services.AddTransient<IEntityRepository<File>, FilesRepository>();
            services.AddTransient<IEntityRepository<Record>, RecordsRepository>();
            services.AddTransient<IEntityRepository<GdprRecord>, GdprRecordsRepository>();
            services.AddTransient<IEntityRepository<UserActivation>, UserActivationsRepository>();
            services.AddTransient<IEntityRepository<Auction>, AuctionsRepository>();

            services.AddTransient<IAuthorizationHandler, HasScopeHandler>();

            services.AddSingleton<IEmailConfiguration>(_configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IUserEmailFactory, UserEmailFactory>();

            services.AddTransient<ISecurityService, SecurityService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors((x) =>
            {
                x.WithOrigins("https://www.jera-trading.cz", "https://jera-trading.cz", "https://www.jeratrading.cz", "https://jeratrading.cz", "http://localhost");
                x.AllowCredentials();
                x.AllowAnyHeader();
                x.AllowAnyMethod();
            });
            app.UseStaticFiles();
            app.UseAuthentication();

            //using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    try
            //    {
            //        var context = scope.ServiceProvider.GetRequiredService<SaDbContext>();
            //        context.Database.Migrate();
            //    }
            //    catch (Exception e)
            //    {
            //        throw e;
            //    }
            //}

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(e =>
            {
                e.MapControllers();
                e.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                e.MapFallbackToController("Index", "Home");
            });
        }
    }
}
