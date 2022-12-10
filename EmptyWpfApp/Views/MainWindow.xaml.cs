using EmptyWpfApp.Contracts.ViewModels;
using EmptyWpfApp.Contracts.Views;
using EmptyWpfApp.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

namespace EmptyWpfApp.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : IMainWindow
	{
		private IMainWindowViewModel m_viewModel;
		private ILogger<MainWindow> m_logger;

		private bool IsRunning { get; set; }

		public MainWindow( IMainWindowViewModel vm, ILogger<MainWindow> logger, IHostApplicationLifetime lifeTime )
		{
			DataContext = vm;
			InitializeComponent();
			lifeTime.ApplicationStopping.Register( () => IsRunning = false );
			m_viewModel = vm;
			m_logger = logger;
		}
		private void OnLoaded( object sender, RoutedEventArgs e )
		{
			m_logger?.LogInformation( $"In  {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
			IsRunning = true;
			m_logger?.LogInformation( $"Out {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
		}
		private void OnClosing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			m_logger?.LogInformation( $"In  {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
			if( IsRunning )
			{
				e.Cancel = true;
				// 非同期(ウィンドウメッセージの非同期処理)に終了コマンドを実行する(直接呼び出すと再帰してしまいクラッシュする)
				Dispatcher.BeginInvoke( () => m_viewModel.FileExitCommand.Execute( null ) );
			}
			m_logger?.LogInformation( $"Out {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
		}
		private void OnClosed( object sender, EventArgs e )
		{
			m_logger?.LogInformation( $"In  {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
			m_logger?.LogInformation( $"Out {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
		}
	}
}
