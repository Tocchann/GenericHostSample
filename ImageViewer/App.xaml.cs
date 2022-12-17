using ImageViewer.Contracts.Services;
using ImageViewer.Contracts.ViewModels;
using ImageViewer.Contracts.Views;
using ImageViewer.Models;
using ImageViewer.Services;
using ImageViewer.ViewModels;
using ImageViewer.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace ImageViewer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public T? GetService<T>() where T : class => m_host?.Services.GetService( typeof( T ) ) as T;


		private IHost? m_host;
		private IMessageBoxService? m_msgBox;

		private void Application_DispatcherUnhandledException( object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e )
		{
			m_msgBox ??= GetService<IMessageBoxService>();
			e.Handled = true;
			m_msgBox?.Show( e.Exception.Message, MessageBoxButton.OK, MessageBoxImage.Exclamation );
			//MessageBox.Show( e.Exception.Message, ImageViewer.Properties.Resources.AppTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation );
		}

		private async void Application_Startup( object sender, StartupEventArgs e )
		{
			// アプリケーションのベースパスはexeのある場所
			var appLocation = Path.GetDirectoryName( Assembly.GetEntryAssembly()?.Location ) ?? string.Empty;
			// 本当はアプリケーションの起動パラメータをそのまま渡すのもセキュリティ的に危ないのでやらないほうがいい
			m_host = Host.CreateDefaultBuilder( e.Args )
				.ConfigureAppConfiguration( c => c.SetBasePath( appLocation ) )
				.ConfigureServices( ConfigureServices )
				.Build();

			ConfigureApplicationLifeTime( GetService<ILogger<App>>(), GetService<IHostApplicationLifetime>() );

			await m_host.StartAsync();
		}

		private void ConfigureApplicationLifeTime( ILogger<App>? logger, IHostApplicationLifetime? lifeTime )
		{
			lifeTime?.ApplicationStarted.Register( () =>
			{
				logger?.LogInformation( "In  IHostApplicationLifetime.ApplicationStarted" );

				// 本当はここでインターフェースがとれなければ、処理を終了する必要がある
				GetService<IMainWindow>()?.Show();

				logger?.LogInformation( "Out IHostApplicationLifetime.ApplicationStarted" );
			} );
			lifeTime?.ApplicationStopped.Register( () => logger?.LogInformation( "raise IHostApplicationLifetime.ApplicationStopped" ) );
			lifeTime?.ApplicationStopping.Register( () =>
			{
				logger?.LogInformation( "In  IHostApplicationLifetime.ApplicationStopping" );
				// アプリケーションの終了要求はメインウィンドウのクローズを実施すればよい0
				Current.MainWindow?.Close();
				logger?.LogInformation( "Out IHostApplicationLifetime.ApplicationStopping" );
			} );
		}

		private void ConfigureServices( HostBuilderContext context, IServiceCollection services )
		{
			// メッセージボックスのサービス化
			// メッセージボックスは呼び出し自体が static メソッドなのでシングルトンなオブジェクトになる
			services.AddSingleton<IMessageBoxService, MessageBoxService>();

			// ファイルダイアログのサービス化
			// ファイルダイアログはシングルトンにはならない
			services.AddTransient<ISelectFileService, SelectFileService>();

			// メインウィンドウとメインのモデル(いずれもシングルトンにはしない)
			services.AddTransient<Model>();
			services.AddTransient<IMainWindowViewModel, MainWindowViewModel>();
			services.AddTransient<IMainWindow, MainWindow>();
		}

		private async void Application_Exit( object sender, ExitEventArgs e )
		{
			if( m_host is not null )
			{
				await m_host.StopAsync();
			}
		}
	}
}
