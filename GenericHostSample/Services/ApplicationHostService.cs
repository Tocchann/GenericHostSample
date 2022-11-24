using GenericHostSample.Contracts.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GenericHostSample.Services;

public class ApplicationHostService : IHostedService
{
	public ApplicationHostService( IServiceProvider serviceProvider, ILogger<ApplicationHostService> logger )
	{
		m_logger = logger;
		m_serviceProvider = serviceProvider;
	}
	public async Task StartAsync( CancellationToken cancellationToken )
	{
		m_logger.LogInformation( "Called StartAsync()" );
		cancellationToken.ThrowIfCancellationRequested();
		await Task.CompletedTask;
		// IHostApplicationLifetime.ApplicationStarted イベントハンドラとして用意しても同じ
		// GetService<IIHostApplicationLifetime>()?.ApplicationStarted.Register( () => GetService<IMainWindow>()?.Show() );
		m_serviceProvider.GetService<IMainWindow>()?.Show();
	}

	public async Task StopAsync( CancellationToken cancellationToken )
	{
		m_logger.LogInformation( "Called StopAsync()" );
		cancellationToken.ThrowIfCancellationRequested();
		await Task.CompletedTask;
	}
	private ILogger<ApplicationHostService> m_logger;
	private IServiceProvider m_serviceProvider;
}
