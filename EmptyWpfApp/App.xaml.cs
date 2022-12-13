using EmptyWpfApp.Contracts.Services;
using EmptyWpfApp.Contracts.ViewModels;
using EmptyWpfApp.Contracts.Views;
using EmptyWpfApp.Services;
using EmptyWpfApp.ViewModels;
using EmptyWpfApp.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace EmptyWpfApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		public T? GetService<T>() where T : class => m_host?.Services.GetService( typeof( T ) ) as T;

		private IHost? m_host;
		private ILogger<App>? m_logger;
		private IMessageBoxService? m_msgBox;
		private async void OnStartupAsync( object sender, StartupEventArgs e )
		{
			Trace.WriteLine( $"In  App.OnStartupAsync()" );
			// アプリケーションのベースパスはexeのある場所
			var appLocation = Path.GetDirectoryName( Assembly.GetEntryAssembly()?.Location ) ?? string.Empty;
			// 本当はアプリケーションの起動パラメータをそのまま渡すのもセキュリティ的に危ないのでやらないほうがいい
			m_host = Host.CreateDefaultBuilder( e.Args )
				.ConfigureAppConfiguration( c => c.SetBasePath( appLocation ) )
				.ConfigureServices( ConfigureServices )
				.Build();

			ConfigureApplicationLifeTime( GetService<ILogger<App>>(), GetService<IHostApplicationLifetime>() );

			m_logger?.LogInformation( $"Call m_host.StartAsync();" );
			await m_host.StartAsync();
			m_logger?.LogInformation( $"Out OnStartupAsync()" );
		}
		private void ConfigureServices( HostBuilderContext context, IServiceCollection services )
		{
			// IHostedService の登録
			services.AddHostedService<ApplicationHostService>();

			// アプリケーションプロセス全体でインスタンスが一つあればよいサービス
			services.AddSingleton<IMessageBoxService, MessageBoxService>();

			// 呼び出しごとにインスタンスが生成されるサービス

			// MainWindow
			services.AddTransient<IMainWindowViewModel, MainWindowViewModel>();
			services.AddTransient<IMainWindow, MainWindow>();
		}
		private void ConfigureApplicationLifeTime( ILogger<App>? logger, IHostApplicationLifetime? lifeTime )
		{
			m_logger = logger;
			lifeTime?.ApplicationStarted.Register( () =>
			{
				m_logger?.LogInformation( "In  IHostApplicationLifetime.ApplicationStarted" );
				// 本当はここでインターフェースがとれなければ、処理を終了する必要がある
				GetService<IMainWindow>()?.Show();
				m_logger?.LogInformation( "Out IHostApplicationLifetime.ApplicationStarted" );
			} );
			lifeTime?.ApplicationStopped.Register( () => m_logger?.LogInformation( "raise IHostApplicationLifetime.ApplicationStopped" ) );
			lifeTime?.ApplicationStopping.Register( () =>
			{
				m_logger?.LogInformation( "In  IHostApplicationLifetime.ApplicationStopping" );
				// アプリケーションの終了要求はメインウィンドウのクローズを実施すればよい
				Current.MainWindow?.Close();
				m_logger?.LogInformation( "Out IHostApplicationLifetime.ApplicationStopping" );
			} );
		}
		private async void OnExitAsync( object sender, ExitEventArgs e )
		{
			m_logger?.LogInformation( $"In  OnExitAsync()" );
			if( m_host is not null )
			{
				await m_host.StopAsync();
			}
			m_logger?.LogInformation( $"Out OnExitAsync()" );
		}
		private void OnDispatcherUnhandledException( object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e )
		{
			m_msgBox ??= GetService<IMessageBoxService>();
			// メインウィンドウがある場合継続する(メインウィンドウが出る前と後は、継続できないのでデフォルト処理に任せて終了に突き進む)
			e.Handled = App.Current.MainWindow != null;
#if DEBUG
			m_msgBox?.Show( e.Exception.ToString(), IMessageBoxService.MessageBoxButton.OK, IMessageBoxService.MessageBoxImage.Error );
#else
			m_msgBox?.Show( e.Exception.Message, IMessageBoxService.MessageBoxButton.OK, IMessageBoxService.MessageBoxImage.Error );
#endif
		}
	}
}
