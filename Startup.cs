using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using app.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace app
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

	public interface ISmsSender
	{
    		Task SendSmsAsync(string number, string message);
	}
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
	{
	    // Database connection string.
    	    // Make sure to update the Password value below from "Your_password123" to your actual password.
    		var connection = @"Server=db;Database=master;User=sa;Password=Your_password123;";

    		// This line uses 'UseSqlServer' in the 'options' parameter
    		// with the connection string defined above.
    		services.AddDbContext<ApplicationDbContext>(
        		options => options.UseSqlServer(connection));

//    		services.AddIdentity<ApplicationUser, IdentityRole>()
		services.AddIdentity<IdentityUser, IdentityRole>()
        		.AddEntityFrameworkStores<ApplicationDbContext>()
        		.AddDefaultTokenProviders();
    		services.AddMvc();
		
   		 // Add application services.
    		services.AddTransient<IEmailSender, IEmailSender>();
    		services.AddTransient<ISmsSender, ISmsSender>();
	//	services.Configure<SMSoptions>(Configuration);
	}
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
