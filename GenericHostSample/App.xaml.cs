using GenericHostSample.Contracts.Services;
using GenericHostSample.Contracts.Views;
using GenericHostSample.Models;
using GenericHostSample.Services;
using GenericHostSample.ViewModels;
using GenericHostSample.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
		}

		private IHost? m_host;
	}
}
