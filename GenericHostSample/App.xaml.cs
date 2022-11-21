using GenericHostSample.Contracts.Services;
using GenericHostSample.Contracts.Views;
using GenericHostSample.Models;
using GenericHostSample.Services;
using GenericHostSample.ViewModels;
using GenericHostSample.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
		public static new App Current => (App)Application.Current;
		public bool IsRunning { get; set; }
		private async void OnStartup( object sender, StartupEventArgs e )
		{
			var appLocation = Path.GetDirectoryName( Assembly.GetEntryAssembly()?.Location ) ?? string.Empty;
			m_host = Host.CreateDefaultBuilder( e.Args )
					.ConfigureAppConfiguration( c => c.SetBasePath( appLocation ) )
					.ConfigureServices( ConfigureServices )
					.Build();
			await m_host.StartAsync();

			// VMの終了コマンドをハンドリングするためのコードをここに書く(WPFのシステムに依存する部分を持ってくる
			var lifeTime = GetService<IHostApplicationLifetime>();
			lifeTime?.ApplicationStopping.Register( () => MainWindow?.Close() );
			lifeTime?.ApplicationStarted.Register( () => IsRunning = true );
			lifeTime?.ApplicationStopped.Register( () => IsRunning = false );

		}
		private void ConfigureServices( HostBuilderContext context, IServiceCollection services )
		{
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
			if( m_host != null )
			{
				await m_host.StopAsync();
			}
			m_host = null;
		}
		private void OnDispatcherUnhandledException( object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e )
		{
			e.Handled = IsRunning;
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
