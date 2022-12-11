using ImageViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageViewer.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			m_viewModel = (MainWindowViewModel)DataContext;
		}
		public MainWindowViewModel m_viewModel;

		private void Window_Loaded( object sender, RoutedEventArgs e )
		{
			m_viewModel.IsRunning = true;
		}
		private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			if( m_viewModel.IsRunning )
			{
				e.Cancel = true;
				Dispatcher.BeginInvoke( () => m_viewModel.FileExitCommand.Execute( null ) );
			}
		}
	}
}
