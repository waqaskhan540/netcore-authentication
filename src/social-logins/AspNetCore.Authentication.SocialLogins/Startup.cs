using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Authentication.SocialLogins.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Authentication.SocialLogins
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
            services.AddMvc();

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = "Temporary";
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddFacebook(options =>
                {
                    options.AppId = "396850710793600";
                    options.AppSecret = "23e6ba83fe929351109c83313a80aa0a";
                })
                .AddTwitter(options =>
                {
                    options.ConsumerKey = "UVbeJWXYJ4bIkW73YI4mNtbSi";
                    options.ConsumerSecret = "lPAr9bKJkxmIkxJ0SeSKW0HxFzash3MDzW8gDHfOg57ChjMBWM";
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/signin";
                })
                //for adding extra use info from social logins, we first create a temporary session 
                // and when the info is fetched, we change it to a real session.for this we need to user
                //a temporary cookie.
                .AddCookie("Temporary");

            services.AddSingleton<IUserService, UserService>();
                
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
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
