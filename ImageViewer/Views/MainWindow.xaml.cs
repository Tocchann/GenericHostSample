using ImageViewer.Contracts.ViewModels;
using ImageViewer.Contracts.Views;
using ImageViewer.ViewModels;
using System.Windows;

namespace ImageViewer.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IMainWindow
{
	public MainWindow( IMainWindowViewModel vm )
	{
		InitializeComponent();
		DataContext = vm;
	}
	private void Window_Loaded( object sender, RoutedEventArgs e )
	{
		if( DataContext is MainWindowViewModel vm )
		{
			vm.IsRunning = true;
		}
	}
	private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
	{
		if( DataContext is MainWindowViewModel vm )
		{
			if( vm.IsRunning )
			{
				e.Cancel = true;
				Dispatcher.BeginInvoke( () => vm.ExitApplication() );
			}
		}
	}
}
