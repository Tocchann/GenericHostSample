using GenericHostSample.Contracts.Services;
using GenericHostSample.Contracts.Views;
using GenericHostSample.Models;
using GenericHostSample.Services;
using GenericHostSample.ViewModels;
using GenericHostSample.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace GenericHostSample
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public T? GetService<T>() where T : class => m_host?.Services.GetService( typeof( T ) ) as T;
		private async void OnStartup( object sender, StartupEventArgs e )
		{
			// WinForms で使う場合は、Main() の Run の手前で記述
			var appLocation = Path.GetDirectoryName( Assembly.GetEntryAssembly()?.Location ) ?? string.Empty;
			m_host = Host.CreateDefaultBuilder( e.Args )
					.ConfigureAppConfiguration( c => c.SetBasePath( appLocation ) )
					.ConfigureServices( ConfigureServices )
					.Build();

			var logger = GetService<ILogger<App>>();
			// ロギングなどが不要ならメソッドアウトせずに以下の１行で済む
			//GetService<IHostApplicationLifetime>()?.ApplicationStopping.Register( () => App.Current.MainWindow?.Close() );
			SetupLifeTimeEvents( logger, GetService<IHostApplicationLifetime>() );
			logger?.LogInformation( "PreCall m_host.StartAsync()" );

			await m_host.StartAsync();
		}

		private void SetupLifeTimeEvents( ILogger<App>? logger, IHostApplicationLifetime? lifeTime )
		{

			// どのタイミングでどのイベントが動作しているかを確認できるように、ロギングしている(デバッグ実行で出力画面で確認可能)
			lifeTime?.ApplicationStarted.Register( () =>
			{
				logger?.LogInformation( "Called lifeTime?.ApplicationStarted" );
			} );

			// フレームワークに終了処理を開始させるためのイベント(StopApplicationを呼び出すことでコールされる)
			lifeTime?.ApplicationStopping.Register( () =>
			{
				logger?.LogInformation( "Called lifeTime?.ApplicationStopping" );

				// メインウィンドウのクローズすることで、フレームワークが終了処理を開始する
				App.Current.MainWindow?.Close();
			} );
			lifeTime?.ApplicationStopped.Register( () =>
			{
				logger?.LogInformation( "Called lifeTime?.ApplicationStopped" );
			} );
		}

		private void ConfigureServices( HostBuilderContext context, IServiceCollection services )
		{
			// IHost の動作状況に合わせてコールバックされるサービス
			services.AddHostedService<ApplicationHostService>();

			// 独自に作ったサービス
			services.AddSingleton<IMessageBoxService, MessageBoxService>();

			services.AddTransient<ISelectFileService, SelectFileService>();
			// Model
			services.AddTransient<Model>();

			// View と ViewModel
			services.AddTransient<MainWindowViewModel>();
			services.AddTransient<IMainWindow, MainWindow>();
		}
		private async void OnExit( object sender, ExitEventArgs e )
		{
			// WinForms で使う場合は、Application.ApplicationExit イベントに実装
			var logger = GetService<ILogger<App>>();
			logger?.LogInformation( "PreCall m_host.StopAsync()" );
			if( m_host is not null )
			{
				await m_host.StopAsync();
			}
			m_host = null;
		}
		private void OnDispatcherUnhandledException( object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e )
		{
			e.Handled = MainWindow is not null;
			var msgBoxService = GetService<IMessageBoxService>();
#if DEBUG
			msgBoxService?.Show( e.Exception.ToString(), MessageBoxButton.OK, MessageBoxImage.Error );
#else
			msgBoxService?.Show( e.Exception.Message, MessageBoxButton.OK, MessageBoxImage.Error );
#endif
		}
		private IHost? m_host;
	}
}
