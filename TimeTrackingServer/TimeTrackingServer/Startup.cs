using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTrackingServer.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeTrackingServer.Helpers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using TimeTrackingServer.Services;
using TimeTrackingServer.Services.Impl;
using NSwag.AspNetCore;
using System;
using TimeTrackingServer.Middlewares;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Newtonsoft.Json.Serialization;

namespace TimeTrackingServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o =>
            {
                //o.AddPolicy("cors", builder => builder
                //    .WithOrigins("http://localhost:8080", "http://localhost:5001")
                //    .AllowAnyMethod()
                //    .AllowCredentials()
                //    .WithHeaders("Accept", "Content-Type", "Origin", "X-My-Header"));

                var policy = new CorsPolicy();

                policy.Headers.Add("*");
                policy.Methods.Add("*");
                policy.Origins.Add("*");
                policy.SupportsCredentials = true;

                o.AddPolicy("cors", policy);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<ApplicationDbContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddJsonOptions(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                 .SetCompatibilityVersion(CompatibilityVersion.Version_2_2); ;

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Build the intermediate service provider
            var serviceProvider = services.BuildServiceProvider();

            //resolve implementations
            serviceProvider.GetService<ApplicationDbContext>();

            // configure DI for application services
            services.AddSingleton<ApplicationDbContext>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IActivityStaffService, ActivityStaffService>();
            services.AddScoped<IApplicationsService, ApplicationsService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<ITreatmentApplicationsService, TreatmentApplicationsService>();
            services.AddScoped<IStaffToGroupService, StaffToGroupService>();
            services.AddScoped<IStreamingDataService, StreamingDataService>();
            services.AddTransient<StreamingDataService>();
            // Register the Swagger services
            services.AddSwaggerDocument();

            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("cors");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseAuthentication();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwagger();
            app.UseSwaggerUi3();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMiddleware<ListenerSocketMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
