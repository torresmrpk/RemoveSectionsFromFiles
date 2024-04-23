using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveBindings
{
	public class DirectoryModifier
	{

		public string DirectoryToModify { get; private set; }
		public string FilePattern { get; private set; }
		public string StartOfSection { get; private set; }
		public string EndOfSection { get; private set; }

		public List<string> FilesMatched { get; private set; }

		public DirectoryModifier(string dir, string pattern,string startOfSection, string endOfSection)
		{
			this.DirectoryToModify = dir;
			this.FilePattern = pattern;

			this.StartOfSection = startOfSection;
			this.EndOfSection = endOfSection;

            FilesMatched = new List<string>
            (
                Directory.GetFiles(dir, pattern, SearchOption.AllDirectories)
            );
        }
		 

		public void ProcessDir()
		{
			string[] fileList = Directory.GetFiles(this.DirectoryToModify, this.FilePattern, SearchOption.AllDirectories);
			for (int i = 0; i < fileList.Length; i++)
			{
				string file = fileList[i];
				Console.WriteLine($"Processing File {i + 1}: {file}.");

				FileModifier fileToModify = new FileModifier(solutionFile:fileList[i], this.StartOfSection, this.EndOfSection);				
				fileToModify.RemoveSection();
				Console.WriteLine($"The file {file} has been processed. A backup file named {fileToModify.BackupFile} has been created.");
			}
		}

		public async Task ProcessDirAsync()
		{			
			await Task.Run(() =>
			{
				Parallel.ForEach<string>(this.FilesMatched, file =>
				{
                    Console.WriteLine($"Processing File: {file}.");
                    FileModifier fileToModify = new FileModifier(solutionFile: file, this.StartOfSection, this.EndOfSection);
					fileToModify.RemoveSection();
                    Console.WriteLine($"The file {file} has been processed. A backup file named {fileToModify.BackupFile} has been created.");
                });
			});
		}
	}


	
}
