using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace MandelWindow
{
	public class FileHandler
	{
		public string BasePath { get; set; } = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
		public string FilePath { get; set; }

		public FileHandler()
		{
			FilePath = Path.Combine(BasePath, "Results.csv");
		}

		public void SaveToFile(List<(dynamic, double)> outputList, string header)
		{
			FilePath = Path.Combine(BasePath, "Results.csv");

			var output = new List<string>
			{
				header,
				"Parameter\tTime"
			};
			
			foreach (var (parameter, timeSpan) in outputList)
			{
				output.Add($"{parameter}\t{timeSpan}");
			}
			
			File.AppendAllLines(FilePath, output);
		}

		public void NewSessionStarted()
		{
			File.AppendAllText(FilePath,$"\nNew experiment session started at: {DateTime.Now}");
		}
	}
}
