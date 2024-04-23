using System;
using System.Collections.Generic;
using CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveSourceControl
{
    public class CommandLineOptions
    {
        [Option(shortName: 'd', longName: "directory", Required = true,
            HelpText = "The Directory to look for files to alter.", Default = "")]
        public string Directory { get; set; } = string.Empty;


        [Option(shortName: 'p', longName: "pattern", Required = true,
            HelpText = "The file pattern of the files that will be altered.", Default = "")]
        public string FilePattern { get; set; } = string.Empty;

        [Option(shortName: 'o', longName: "first opening section", Required = true,
            HelpText = "The first opening text of the section of data that will be deleted.", Default = "")]
        public string OpeningSection { get; set; } = string.Empty;

        [Option(shortName: 'c', longName: "last closing section", Required = true,
            HelpText = "The last closing text of the section of data that will be deleted.", Default = "")]
        public string ClosingSection { get; set; } = string.Empty;
    }
}
