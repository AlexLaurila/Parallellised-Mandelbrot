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
			//Experiment2Parameters = SetExperiment2Parameters();
			//Experiment3Parameters = SetExperiment3Parameters();
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

		private List<(int, int)> SetExperiment2Parameters()
		{
			throw new NotImplementedException();
		}

		private List<(int, int)> SetExperiment3Parameters()
		{
			throw new NotImplementedException();
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
