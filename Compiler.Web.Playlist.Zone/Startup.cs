using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Compiler.Utility.Logging;
using Playlist.Zone.Datatier.Music.Playlists;
using Playlist.Zone.Datatier.Music.Songs;
using Playlist.Zone.Datatier.Music.Charts;
using Playlist.Zone.Services.Music.External;
using Compiler.Interfaces.Common.Datatier.User;
using Compiler.Interfaces.Common.Datatier.Tag;
using Compiler.Datatier.Common.Tag;
using Compiler.Datatier.Common.Link;
using Compiler.Utility.Settings;
using Compiler.Web.Playlist.Zone.Code.Settings;
using Playlist.Zone.Datatier.Users;

namespace Compiler.Web.Playlist.Zone
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

            builder.RegisterType<Compiler.Services.Logging.LogService>().As<Compiler.Services.Logging.ILogService>();
            
            builder.RegisterType<BasePlaylistSettings>().As<IBaseSettings>();

            builder.RegisterType<UserEntity>().As<IUserEntity>();

            builder.RegisterType<PlaylistEntity>().As<IPlaylistEntity>();
            builder.RegisterType<SongsEntity>().As<ISongsEntity>();
            builder.RegisterType<ChartEntity>().As<IChartEntity>();
            builder.RegisterType<ChartSongEntity>().As<IChartSongEntity>();
            builder.RegisterType<YouTubeSearchService>().As<ISearchMusicService>();
            builder.RegisterType<LinkEntity>().As<ILinkEntity>();
            builder.RegisterType<TagEntity>().As<ITagEntity>();

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
