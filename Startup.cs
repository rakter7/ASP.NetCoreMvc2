using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using C229_G2_A4.Models;
using Microsoft.AspNetCore.Identity;

namespace C229_G2_A4
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["Data:CourseHandlerCourses:ConnectionString"])
                );

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:CourseHandlerIdentity:ConnectionString"]));
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

       
//            services.AddTransient<ICourseRepository, EFCourseRepository>();
            services.AddTransient<ICollegeProgramRepository, EFCollegeProgramRepository>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
  
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

            SeedData.EnsurePopulated(app);

            IdentitySeedData.CreateAccounts(app.ApplicationServices,
               Configuration).Wait();
            /* IDs
                anAdmin/Pa$$word1
                aFaculty/Pa$$word1
             */
        }
    }
}

