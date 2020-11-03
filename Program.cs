// ------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="n/a">
//  Copyright (c) Eddie de Bear. 2020, under MIT Licence.
//  See LICENCE file for usage rights.
// </copyright>
// ------------------------------------------------------------------------------------------
namespace GilesFileServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using CommandLine;
    using CommandLine.Text;
    using Crayon;
    using FilesFileServer.CommandLineOptions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Main application / entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main Entry point.
        /// </summary>
        /// <param name="args">Commaand line arguments.</param>
        public static void Main(string[] args)
        {
            using (var parser = new Parser(settings =>
            {
                settings.AutoHelp = false;
            }))
            {
                var result = parser.ParseArguments<Options>(args);

                result.WithParsed(o => OnParsed(o, args))
                    .WithNotParsed(errors => OnNotParsed(result, errors));
            }
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

                return;
            }
            else if (errors.IsHelp())
            {
                RenderHelp(result);
            }
            else
            {
                if (errors.ElementAt(0) is UnknownOptionError unknownOptionError)
                {
                    Console.Error.WriteLine(Output.Red($"\nOption: {unknownOptionError.Token} is unknown\n"));
                }
                else
                {
                    Console.Error.WriteLine(Output.Red(errors.ElementAt(0).Tag.ToString()));
                }

                RenderHelp(result);
            }
        }

        /// <summary>
        /// Renders the help menu to the console.
        /// </summary>
        /// <param name="result">Result of the command line argument parsing operaiton.</param>
        private static void RenderHelp(ParserResult<Options> result)
        {
            var helpText = CommandLine.Text.HelpText.AutoBuild(
                result,
                builder =>
                    {
                        builder.AdditionalNewLineAfterOption = false;
                        builder.Heading = "Giles File Server\n==================";
                        return builder;
                    },
                e => e);

            Console.WriteLine(helpText);
        }

        /// <summary>
        /// Callback for passed command line parameter. Launches site.
        /// </summary>
        /// <param name="options">Parsed command line parameters.</param>
        /// <param name="args">Raw command line args.</param>
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
                Scheme = options.Scheme
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
