using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Areas.Identity.Services;
using WebdevPeriod3.Interfaces;
using WebdevPeriod3.Services;

namespace WebdevPeriod3
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
            services.AddMigrationRunner(Configuration.GetConnectionString("Master"));

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddTransient<UserRepository>();
            services.AddTransient<RoleRepository>();
            services.AddTransient<UserRoleRepository>();

            services.AddScoped<IUserStore<User>, DapperUserStore>();
            services.AddScoped<IRoleStore<Role>, DapperRoleStore>();

            services.AddIdentityCore<User>()
                .AddRoles<Role>();

            services.AddControllersWithViews(mvcOptions =>
            {
                mvcOptions.EnableEndpointRouting = false;
            });
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

            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "root",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            // Update the database to the latest schema
            {
                using var scope = app.ApplicationServices.CreateScope();

                scope.ServiceProvider.GetRequiredService<MigrationService>().UpdateDatabase();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                userManager.CreateAsync(new User("thomasio101"), "Test1234!").Wait();
            };
        }
    }
}
