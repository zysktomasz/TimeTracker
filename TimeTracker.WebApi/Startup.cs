using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TimeTracker.Domain.Identity;
using TimeTracker.Persistance;
using TimeTracker.Services.DTO.Activity;
using TimeTracker.Services.Interfaces;
using TimeTracker.Services.Services;

namespace TimeTracker.WebApi
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
            // Add DbContext
            services.AddDbContext<TimeTrackerDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TimeTrackerDatabase")));

            // ===== Add Jwt Authentication ========
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false, // validate the server that created this token
                        ValidateAudience = false, // ensure that the recipient of the token is authorized to receive it 
                        ValidateLifetime = true, // check that the token is not expired and that the signing key of the issuer is valid
                        ValidateIssuerSigningKey = true, // verify that the key used to sign the incoming token is part of a list of trusted keys
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        RequireSignedTokens = true
                        //RequireExpirationTime = true,
                        //ClockSkew = TimeSpan.FromMinutes(5)
                    };
                });

            // Add Identity
            // ===== Add Identity ========
            services.AddDefaultIdentity<UserAccount>()
                .AddEntityFrameworkStores<TimeTrackerDbContext>()
                .AddDefaultTokenProviders();

            // Add Services Dependency Injection
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IProjectService, ProjectService>();

            // Register AutoMapper DI
            services.AddAutoMapper();

            // Add MVC
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ActivityStartDtoValidator>());
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
