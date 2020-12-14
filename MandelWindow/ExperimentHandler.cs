using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandelWindow
{
	public class ExperimentHandler
	{
		public List<int> Experiment1Parameters { get; set; }
		public List<(int, int)> Experiment2Parameters { get; set; }
		public List<(int, int)> Experiment3Parameters { get; set; }

		public ExperimentHandler()
		{
			Experiment1Parameters = SetExperiment1Parameters();
			Experiment3Parameters = SetExperiment3Parameters();
		}

		private List<int> SetExperiment1Parameters()
		{
			return new List<int>
			{
				500,
				1000,
				2000,
				4000,
				6000
			};
		}

		private List<(int, int)> SetExperiment3Parameters()
		{
			return new List<(int, int)>
			{
				(128, 72),
				(256, 144),
				(384, 216),
				(512, 216),
				(640, 360),
				(768, 432),
				(896, 504),
				(1024, 576),
				(1152, 648),
				(1280, 720),
				(1408, 792),
				(1536, 864),
				(1664, 936),
				(1792, 1008),
				(1920, 1080),
				(2048, 1152),
				(2176, 1224),
				(2304, 1296),
				(2432, 1368),
				(2560, 1440)
			};
		}

		/// <summary>
		/// Runs experiment calculations sequentially.
		/// </summary>
		/// <param name="updateMandel"></param>
		public void RunExperiment(Action<bool> updateMandel)
		{
			updateMandel(false);
		}

		/// <summary>
		/// Runs experiment calculations in parallel.
		/// </summary>
		/// <param name="updateMandel"></param>
		public void RunExperimentParallel(Action<bool> updateMandel)
		{
			updateMandel(true);
		}
	}
}
