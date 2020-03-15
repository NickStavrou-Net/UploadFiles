using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibraryData;
using LibraryData.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement
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
            // TODO
            #region snippet_AddMvc
            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions
                        .AddPageApplicationModelConvention("/StreamedSingleFileUploadDb",
                            model =>
                            {
                                model.Filters.Add(
                                    new GenerateAntiforgeryTokenCookieAttribute());
                                model.Filters.Add(
                                    new DisableFormValueModelBindingAttribute());
                            });
                    options.Conventions
                        .AddPageApplicationModelConvention("/StreamedSingleFileUploadPhysical",
                            model =>
                            {
                                model.Filters.Add(
                                    new GenerateAntiforgeryTokenCookieAttribute());
                                model.Filters.Add(
                                    new DisableFormValueModelBindingAttribute());
                            });
                });
            #endregion
            services.AddControllersWithViews();

            # region CookiePolicy
            /*
            The below code does a couple of things:
            !) The lambda(context => true) “determines whether user consent for non - essential cookies is needed
            for a given request” and then the CheckConsentNeeded boolean property for the options object is set to true or false.
            
            2) The property MinimumSameSitePolicy is set to SameSiteMode.None, which is an enumerator with the following possible values:
            None = 0
            Lax = 1
            Strict = 2
             From the official documentation on cookie authentication, “When set to SameSiteMode.None, the cookie header value isn’t set.
            Note that Cookie Policy Middleware might overwrite the value that you provide.
            To support OAuth authentication, the default value is SameSiteMode.Lax.” 
            This explains why even though the value is initially set to None in the ConfigureServices() method,
            using the Middleware in the Configure() method causes the “samesite” value to be set to “lax” in the cookiestring we observed earlier.
            */

            services.Configure<CookiePolicyOptions>(options =>
            {
                //This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #endregion

            // To list physical files from a path provided by configuration:
            //var physicalProvider = new PhysicalFileProvider(Configuration.GetValue<string>("StoredFilesPath"));
            // To list physical files in the temporary files folder, use:
            var physicalProvider = new PhysicalFileProvider(Path.GetTempPath());

            services.AddSingleton<IFileProvider>(physicalProvider);

            services.AddDbContext<LibraryContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
