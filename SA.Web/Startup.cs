﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using SA.Application.Account;
using SA.Application.Country;
using SA.Application.Customer;
using SA.Application.Records;
using SA.Application.Security;
using SA.Core.Model;
using SA.Core.Security;
using SA.EntityFramework.EntityFramework;
using SA.EntityFramework.EntityFramework.Repository;
using System.Linq;
using System.Reflection;

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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = _configuration["Auth0:Audience"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain)));
            });

            var controllerAssembly = Assembly.Load(new AssemblyName("SA.WebApi"));

            services.AddMvc()
                .AddApplicationPart(controllerAssembly).AddControllersAsServices()
                .AddJsonOptions(a => a.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .AddJsonOptions(a => a.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects)
                .AddJsonOptions(a => a.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<SaDbContext>(options => options.UseMySQL(_configuration["ConnectionString:Prod"]), ServiceLifetime.Singleton);

            services.AddSingleton<IEntityRepository<Country>, CountriesRepository>();
            services.AddSingleton<IEntityRepository<Address>, AddressesRepository>();
            services.AddSingleton<IEntityRepository<User>, UsersRepository>();
            services.AddSingleton<IEntityRepository<Customer>, CustomersRepository>();
            services.AddSingleton<IEntityRepository<Bid>, BidsRepository>();
            services.AddSingleton<IEntityRepository<File>, FilesRepository>();
            services.AddSingleton<IEntityRepository<Record>, RecordsRepository>();

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddSingleton<ISecurityService, SecurityService>();

            Mapper.Initialize(cfg => {
                cfg.CreateMap<Record, RecordTableDto>()
                    .ForMember(dto => dto.CurrentPrice, dto => dto.MapFrom(x => x.Bids.Any()
                        ? x.Bids.OrderByDescending(y => y.Price).FirstOrDefault().Price
                        : x.StartingPrice))
                    .ForMember(dto => dto.NumberOfBids, dto => dto.MapFrom(x => x.Bids.Count()));
                cfg.CreateMap<Record, RecordDetailDto>();
                cfg.CreateMap<Record, RecordMinimumDto>()
                    .ForMember(dto => dto.CurrentPrice, dto => dto.MapFrom(x => x.Bids.Any()
                        ? x.Bids.OrderByDescending(y => y.Price).FirstOrDefault().Price
                        : x.StartingPrice));

                cfg.CreateMap<File, FileSimpleDto>();

                cfg.CreateMap<Bid, BidSimpleDto>();

                cfg.CreateMap<Customer, CustomerSimpleDto>();

                cfg.CreateMap<User, UserShortInfoDto>();

                cfg.CreateMap<Country, CountryLookupDto>();
                cfg.CreateMap<Country, CountryDto>();

                // reverse mapping
                cfg.CreateMap<UserShortInfoDto, User>();
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
            }
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}