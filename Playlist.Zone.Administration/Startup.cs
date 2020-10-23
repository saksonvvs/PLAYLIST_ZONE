using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Compiler.Datatier.Common.Tag;
using Compiler.Interfaces.Common.Datatier.Tag;
using Compiler.Services.Logging;
using Compiler.Utility.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Playlist.Zone.Administration.Code.Settings;
using Playlist.Zone.Datatier.Music.Charts;
using Playlist.Zone.Datatier.Music.Playlists;
using Playlist.Zone.Datatier.Music.Songs;
using Playlist.Zone.Services.Music.External;

namespace Playlist.Zone.Administration
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
            services.AddControllersWithViews();



            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(o => o.LoginPath = new PathString("/account/login"));


            services.AddScoped<IBaseSettings, BasePlaylistSettings>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IChartEntity, ChartEntity>();
            services.AddScoped<IChartSongEntity, ChartSongEntity>();
            services.AddScoped<ISearchMusicService, YouTubeSearchService>();
            services.AddScoped<IPlaylistEntity, PlaylistEntity>();
            services.AddScoped<ITagEntity, TagEntity>();
            services.AddScoped<ISongsEntity, SongsEntity>();
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
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
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
