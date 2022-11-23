using GenericHostSample.Contracts.Views;
using GenericHostSample.ViewModels;
using Microsoft.Extensions.Logging;

namespace GenericHostSample.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IMainWindow
{
	public MainWindow( MainWindowViewModel vm, ILogger<MainWindow> logger )
	{
		m_logger = logger;
		DataContext = vm;
		InitializeComponent();
	}

	private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
	{
		m_logger.LogInformation( $"Called {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
	}

	private void Window_Closed( object sender, System.EventArgs e )
	{
		m_logger.LogInformation( $"Called {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
	}
	private ILogger<MainWindow> m_logger;
}
