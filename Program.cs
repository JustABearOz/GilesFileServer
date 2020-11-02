//----------------------------------------------------------------------------
//
//
//----------------------------------------------------------------------------
namespace GilesFileServer
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommandLine;
    using CommandLine.Text;
    using FilesFileServer.CommandLineOptions;

    /// <summary>
    /// Main application / entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main Entry point.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var parser = new Parser(settings =>
            {
                settings.AutoHelp = false;
            });

            var result = parser.ParseArguments<Options>(args);

            result.WithParsed(o => OnParsed(o, args))
                .WithNotParsed(errors => OnNotParsed(result, errors));
        }

        /// <summary>
        /// Handler for Not Parsed / Error state for command line parameters.
        /// </summary>
        /// <param name="result">Results of a Parse operation.</param>
        /// <param name="errors">The errors of the parse operation.</param>
        private static void OnNotParsed(ParserResult<Options> result, IEnumerable<Error> errors)
        {
            // Write errors
            if (errors.IsVersion())
            {
                Console.WriteLine(HelpText.AutoBuild(result));
            }
            else if (errors.IsHelp())
            {
                RenderHelp(result);
            }
            else
            {
                var consoleColour = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Red;

                if (errors.ElementAt(0) is UnknownOptionError unknownOptionError)
                {
                    Console.Error.WriteLine($"\nOption: {unknownOptionError.Token} is unknown\n");
                }
                else
                {
                    Console.Error.WriteLine(errors.ElementAt(0).Tag);
                }

                Console.ForegroundColor = consoleColour;

                RenderHelp(result);
            }
        }

        private static void RenderHelp(ParserResult<Options> result)
        {
            var helpText = CommandLine.Text.HelpText.AutoBuild(
                result,
                builder =>
                    {
                        builder.AdditionalNewLineAfterOption = false;
                        builder.Heading = "GilesFileServer";
                        return builder;
                    },
                e => e);

            Console.WriteLine(helpText);
        }

        private static void OnParsed(Options options, string[] args)
        {
            CreateHostBuilder(args, options).Build().Run();
        }

        /// <summary>
        /// Creates the host builder for the aps.net application.
        /// </summary>
        /// <param name="args">Command Line Arguments.</param>
        /// <param name="options">Parsed options.</param>
        /// <returns>A Host builder instance.</returns>
        private static IHostBuilder CreateHostBuilder(string[] args, Options options)
        {
            UriBuilder builder = new UriBuilder()
            {
                Port = options.Port,
                Host = options.Url,
            };

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .ConfigureServices(s => s.AddSingleton(options))
                        .UseUrls(builder.ToString());
                });
        }
    }
}
