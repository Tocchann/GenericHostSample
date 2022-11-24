using GenericHostSample.Contracts.Views;
using GenericHostSample.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHostSample.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IMainWindow
{
	public MainWindow( MainWindowViewModel vm, ILogger<MainWindow> logger, IHostApplicationLifetime lifeTime )
	{
		m_logger = logger;
		lifeTime.ApplicationStopping.Register( () => m_stopping = true );
		m_viewModel = vm;
		DataContext = vm;
		InitializeComponent();
	}

	private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
	{
		m_logger.LogInformation( $"Called {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
		if( !m_stopping )
		{
			e.Cancel = true;
			//	ウィンドウをクローズしようとしているところで、Closeを呼び出すと再帰で例外が出るのであらためて終了コマンドを発行する必要がある
			Dispatcher.BeginInvoke( () => m_viewModel.FileExitCommand.Execute( null ) );
		}
	}

	private void Window_Closed( object sender, System.EventArgs e )
	{
		m_logger.LogInformation( $"Called {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
	}
	private readonly ILogger<MainWindow> m_logger;
	private readonly MainWindowViewModel m_viewModel;
	private bool m_stopping;
}
