using GenericHostSample.Contracts.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GenericHostSample.Services;

public class ApplicationHostService : IHostedService
{
	public ApplicationHostService( IServiceProvider serviceProvider )
	{
		m_serviceProvider = serviceProvider;
	}
	public async Task StartAsync( CancellationToken cancellationToken )
	{
		// VMの終了コマンドをハンドリングするためのコードをここに書く
		var lifeTime = m_serviceProvider.GetService<IHostApplicationLifetime>();
		lifeTime?.ApplicationStopping.Register( () => App.Current.MainWindow?.Close() );
		lifeTime?.ApplicationStarted.Register( () => App.Current.IsRunning = true );
		lifeTime?.ApplicationStopped.Register( () => App.Current.IsRunning = false );
		cancellationToken.ThrowIfCancellationRequested();
		await Task.CompletedTask;

		//	メインウィンドウを構築
		var window = m_serviceProvider.GetService<IMainWindow>();
		window?.Show();

		cancellationToken.ThrowIfCancellationRequested();
		await Task.CompletedTask;
	}

	public async Task StopAsync( CancellationToken cancellationToken )
	{
		cancellationToken.ThrowIfCancellationRequested();
		await Task.CompletedTask;
	}
	private IServiceProvider m_serviceProvider;
}
