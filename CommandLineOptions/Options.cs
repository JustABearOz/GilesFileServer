// ------------------------------------------------------------------------------------------
// <copyright file="Options.cs" company="n/a">
//  Copyright (c) Eddie de Bear. 2020, under MIT Licence.
//  See LICENCE file for usage rights.
// </copyright>
// <summary>Command line options.</summary>
// ------------------------------------------------------------------------------------------
namespace FilesFileServer.CommandLineOptions
{
    using CommandLine;

    /// <summary>
    /// Command line flag / options.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets or sets the port to use for hosting.
        /// </summary>
        [Option('p', "port", Required = false, HelpText = "Specify the port to host the file server on. Default port 8080", Default = 8080)]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the url to host the file server on.
        /// </summary>
        [Option('u', "url", Required = false, HelpText = "Specify the url or ip address to host on. Default 0.0.0.0 to listen on all available ips", Default = "0.0.0.0")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the root path for sharing.
        /// </summary>
        [Option('r', "rootPath", Required = false, HelpText = "Specify the root path of of the file share. If rootPath is not defined, the current working directory is used.")]
        public string RootPath { get; set; }

        /// <summary>
        /// Gets or sets the hosting scheme to use.
        /// </summary>
        /// <value></value>
        [Option('s', "scheme", Required = false, HelpText = "Specify the scheme to use, either http or https", Default = "http")]
        public string Scheme { get; set; }
    }
}