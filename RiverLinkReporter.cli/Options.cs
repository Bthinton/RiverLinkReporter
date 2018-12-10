using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiverLinkReporter.cli
{
    public class ProgramOptions
    {
        [Option('o', "run", Required = true,
            HelpText = "Login, Get Data, and Insert data to database")]
        public string Operation { get; set; }

        [Option('u', "username",
            HelpText = "This is your RiverLink account login")]
        public string Username { get; set; }

        [Option('p', "password",
            HelpText = "This is your RiverLink account password")]
        public string Password { get; set; }

        [Option('d', "headless",
            HelpText = "Hides browser from view")]
        public string Headless { get; set; }
    }
}
