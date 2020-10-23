using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Compiler.Web.Admin.Data;
using Compiler.Web.Admin.Models;
using Compiler.Web.Admin.Services;
using Microsoft.AspNetCore.Http;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Compiler.Utility.Logging;
using Compiler.Web.Playlist.Zone.Code.Settings;
using Compiler.Utility.Settings;
using Compiler.Interfaces.Common.Datatier.User;

namespace Compiler.Web.Admin
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public object CookieSecureOption { get; private set; }

        public IContainer ApplicationContainer { get; private set; }





        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(o => o.LoginPath = new PathString("/account/login"));
            /*.AddFacebook(o =>
            {
                o.AppId = Configuration["facebook:appid"];
                o.AppSecret = Configuration["facebook:appsecret"];
            });*/



            // Create the container builder.
            var builder = new ContainerBuilder();

            //builder.RegisterType<Datatier.Settings.BaseSettings>().As<Datatier.Settings.IBaseSettings>();
            //builder.RegisterType<Datatier.Settings.Playlist.BasePlaylistSettings>().As<Datatier.Settings.IBaseSettings>();


            //temporary commented - wil need to double check
            //builder.RegisterType<Datatier.Settings.Store.BaseStoreSettings>().As<Datatier.Settings.IBaseSettings>();




            //need to declide what to do here - this admin may be generic without controling sites
            //builder.RegisterType<Datatier.Settings.Store.BaseStoreSettings>().As<Datatier.Settings.IBaseStoreSettings>();
            //builder.RegisterType<Datatier.Settings.Playlist.BasePlaylistSettings>().As<Datatier.Settings.IBasePlaylistSettings>();



            builder.RegisterType<Compiler.Services.Logging.LogService>()
                   .As<Compiler.Services.Logging.ILogService>();


            //admin gets admin settings
            builder.RegisterType<BasePlaylistSettings>().As<IBaseSettings>();



            builder.RegisterType<Datatier.Users.UserEntity>().As<IUserEntity>();


            /*
            builder.RegisterType<Datatier.Music.Playlists.PlaylistEntity>().As<Datatier.Music.Playlists.IPlaylistEntity>();
            builder.RegisterType<Datatier.Music.Songs.SongsEntity>().As<Datatier.Music.Songs.ISongsEntity>();

            builder.RegisterType<Datatier.Music.Charts.ChartEntity>().As<Datatier.Music.Charts.IChartEntity>();
            builder.RegisterType<Datatier.Music.Charts.ChartSongEntity>().As<Datatier.Music.Charts.IChartSongEntity>();

            builder.RegisterType<Compiler.Services.Music.External.YouTubeSearchService>().As<Compiler.Services.Music.External.ISearchMusicService>();
            */


            

            //builder.RegisterType<LogManagement>().As<ILogManagement>();

            /*builder.RegisterType<Datatier.Common.Link.LinkEntity>().As<Datatier.Common.Link.ILinkEntity>();
            builder.RegisterType<Datatier.Common.Tag.TagEntity>().As<Datatier.Common.Tag.ITagEntity>();*/


            builder.Populate(services);


            this.ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);
        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
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


            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}
