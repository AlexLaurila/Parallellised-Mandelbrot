using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelWindow
{
	public class DataHandler
	{
		public List<(dynamic, double)> ResultData { get; set; } = new List<(dynamic, double)>();

		public DataHandler()
		{
			
		}

		public void SaveData(dynamic parameterValue, double timeSpan)
		{
			ResultData.Add((parameterValue, timeSpan));
		}
	}
}
