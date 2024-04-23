using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveBindings
{
	public class FileModifier
	{
		public string SolutionFile { get; private set; }
		public string FileTimeStamp { get; private set; }		
		public string BackupFile { get; private set; }
		public string StartOfSection { get; private set; }
		public string EndOfSection { get; private set; }

		private string ModifiedFile;
		public FileModifier(string solutionFile, string startOfSection, string endOfSection)
		{
			this.SolutionFile = solutionFile;
			this.FileTimeStamp = System.DateTime.Now.ToString("yyyMMdd_HHmmss");
			this.BackupFile = this.SolutionFile + "." + this.FileTimeStamp + ".backup";
			this.ModifiedFile = this.SolutionFile + "." + this.FileTimeStamp + ".modified";

			this.StartOfSection = startOfSection;
			this.EndOfSection = endOfSection;
		}

		public void RemoveSection()
		{
			this.CreateModifiedSolutionFile();
			this.RenameOriginalFile();
			this.PromoteModifiedFile();
		}		

		private void CreateModifiedSolutionFile()
		{
						
			if (File.Exists(this.ModifiedFile))
			{
				File.Delete(this.ModifiedFile);
			}

			string[] solutionLines = File.ReadAllLines(this.SolutionFile, Encoding.UTF8);
			List<string> outputLines = new List<string>();

			bool inSection = false;
			for(int i = 0; i < solutionLines.Length; i++)
			{
				string line = solutionLines[i];
				if (line.Contains(this.StartOfSection))
				{
					inSection = true;
					continue;
				}

				if (inSection == true && line.Contains(this.EndOfSection))
				{
					inSection = false;
					continue;
				}

				if (!inSection)
				{
					outputLines.Add(line);
				}
				
			}

			File.WriteAllLines(ModifiedFile, outputLines, Encoding.UTF8);
		}

        private void RenameOriginalFile()
        {
            if (File.Exists(this.BackupFile))
            {
                File.Delete(this.BackupFile);
            }
            File.Move(this.SolutionFile, this.BackupFile);
        }

        private void PromoteModifiedFile()
        {
            File.Move(this.ModifiedFile, this.SolutionFile);
        }
		
	}
}
