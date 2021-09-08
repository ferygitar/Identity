using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Models.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersianTranslate.Identity;
using Identity.Repositories;
using Identity.Security.Default;
using Identity.Security.DynamicRole;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Identity
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            
            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("IdentityCmsDbConnection"));
            });
            //Enable Google Authentication
            services.AddAuthentication().AddGoogle(opthion =>
            {
                opthion.ClientId = "92683517948-36dphc123odcraeaj2vsqt69oj8lie3n.apps.googleusercontent.com";
                opthion.ClientSecret = "3-tCShbFPhCKoj6_BJl36p3V";
            });
            //Enable Identity
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.";
                options.User.RequireUniqueEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<PersianIdentityErrorDescriber>();
            //Config Cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/AccessDenied";
                options.Cookie.Name = "IdentityProj";
                options.LoginPath = "/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });

            services.Configure<SecurityStampValidatorOptions>(option =>
            {
                option.ValidationInterval = TimeSpan.FromMinutes(30);
            });

            services.AddScoped<IMessageSender, MessageSender>();
            services.AddAuthorization(option =>
            {
                option.AddPolicy("ManageUserEdit",
                    policy => policy.RequireClaim(ClaimTypesStore.EmployeeList, true.ToString()));

                option.AddPolicy("ClaimOrRole", policy =>
                    policy.RequireAssertion(ClaimOrRole));

                option.AddPolicy("ClaimRequirment", policy =>
                    policy.Requirements.Add(new ClaimRequirment(ClaimTypesStore.EmployeeList, true.ToString())));

                option.AddPolicy("DynamicRole", policy =>
                    policy.Requirements.Add(new DynamicRoleRequirement()));

            });
            services.AddTransient<IUtilities, Utilities>();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSingleton<IAuthorizationHandler, DynamicRoleHandler>();

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //Add Identity MidelleWare
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        private bool ClaimOrRole(AuthorizationHandlerContext context)
            => context.User.HasClaim(ClaimTypesStore.EmployeeList, true.ToString()) ||
               context.User.IsInRole("Admin");
    }
}
