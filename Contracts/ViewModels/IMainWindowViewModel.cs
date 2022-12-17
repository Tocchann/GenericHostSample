using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageViewer.Contracts.ViewModels
{
	public interface IMainWindowViewModel
	{
		public bool IsRunning { get; set; }
		public void ExitApplication();
	}
}
