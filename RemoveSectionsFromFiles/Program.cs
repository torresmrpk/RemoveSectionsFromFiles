using RemoveBindings;
using CommandLine;
using System.Configuration;
using System.Collections.Specialized;

namespace RemoveSourceControl
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            // Will return the default of 0 on success
            int returnCode = 0;            

            returnCode = await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(async (CommandLineOptions options) =>
                {
                    try
                    {
                        return await ModifyFilesAsync(options.Directory, options.FilePattern, options.OpeningSection, options.ClosingSection);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ModifyFilesAsync Outer Exception: {ex.Message}");                        

                        if (ex.InnerException is not null)
                        {
                            Console.WriteLine($"ModifyFilesAsync Inner Exception: {ex.InnerException.Message}");
                        }
                        return -3;
                    }

                },
                errs => Task.FromResult(-1));

            return returnCode;
        }

        private static async Task<int> ModifyFilesAsync(string dir, string filePattern, string openingSection ,string closingSection)
        {

            DirectoryModifier directoryModifier = new DirectoryModifier(dir: dir, pattern:filePattern,
                startOfSection: openingSection, endOfSection: closingSection);
            //  Validate a file with the pattern exist.            
            if (directoryModifier.FilesMatched.Count == 0)
            {
                Console.WriteLine($"No  files exist in the dir {dir} matching the pattern {filePattern}.");
                return -2;
            }
            
            Console.WriteLine($"Are you sure you want to remove data from the files in the directory: {dir}, {directoryModifier.FilesMatched.Count} files will be affected? Type Yes or anything else to exit.");
            string? response = Console.ReadLine();
            if (response?.ToUpper() != "YES")
            {
                return -2;
            }
                        
            await directoryModifier.ProcessDirAsync();

            return 0; // on success
        }
    }
}
