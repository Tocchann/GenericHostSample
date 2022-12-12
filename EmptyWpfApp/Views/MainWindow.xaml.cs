using EmptyWpfApp.Contracts.ViewModels;
using EmptyWpfApp.Contracts.Views;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace EmptyWpfApp.Views;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IMainWindow
{
	private readonly ILogger<MainWindow> m_logger;

	public MainWindow( IMainWindowViewModel vm, ILogger<MainWindow> logger, IHostApplicationLifetime lifeTime )
	{
		DataContext = vm;
		InitializeComponent();
		m_logger = logger;
	}
	private void OnLoaded( object sender, RoutedEventArgs e )
	{
		m_logger?.LogInformation( $"In  {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
		if( DataContext is IMainWindowViewModel vm )
		{
			vm.IsRunning = true;
		}
		m_logger?.LogInformation( $"Out {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
	}
	private void OnClosing( object sender, System.ComponentModel.CancelEventArgs e )
	{
		m_logger?.LogInformation( $"In  {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
		if( DataContext is IMainWindowViewModel vm )
		{
			if( vm.IsRunning )
			{
				e.Cancel = true;
				// 非同期(ウィンドウメッセージの非同期処理)に終了コマンドを実行する(直接呼び出すと再帰してしまいクラッシュする)
				Dispatcher.BeginInvoke( () => vm.OnFileExit() );
			}
		}
		m_logger?.LogInformation( $"Out {System.Reflection.MethodBase.GetCurrentMethod()?.Name}( e.Cancel={e.Cancel})" );
	}
	private void OnClosed( object sender, EventArgs e )
	{
		m_logger?.LogInformation( $"In  {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
		m_logger?.LogInformation( $"Out {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
	}
}
