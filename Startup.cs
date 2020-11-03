namespace GilesFileServer
{
    using FilesFileServer.CommandLineOptions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Website startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Callback to configure asp.net core services.
        /// </summary>
        /// <param name="services">The services collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
        }

        /// <summary>
        /// Callback to configure asp.net core application.
        /// </summary>
        /// <param name="app">The application instance to configure.</param>
        /// <param name="env">The hosting environment.</param>
        /// <param name="options">Command line options.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Options options)
        {
            // Allow error page.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            string rootPath = ResolveRootPath(env, options);

            // Allow directory static files from the root.
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(rootPath),
                ServeUnknownFileTypes = true,
            });

            // Allow directory browsing from the root.
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(rootPath),
            });
        }

        /// <summary>
        /// Resolves the root path for file sharing.
        /// </summary>
        /// <param name="env">The hosting environment.</param>
        /// <param name="options">Command line options.</param>
        /// <returns>Resolved root path.</returns>
        private static string ResolveRootPath(IWebHostEnvironment env, Options options)
        {
            var rootPath = env.ContentRootPath;

            if (!string.IsNullOrEmpty(options.RootPath))
            {
                rootPath = options.RootPath;
            }

            return rootPath;
        }
    }
}
