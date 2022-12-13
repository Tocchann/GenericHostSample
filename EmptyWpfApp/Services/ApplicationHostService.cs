using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmptyWpfApp.Services;
public class ApplicationHostService : IHostedService
{
	private readonly ILogger<ApplicationHostService> m_logger;
	private readonly IServiceProvider m_serviceProvider;

	public ApplicationHostService( ILogger<ApplicationHostService> logger, IServiceProvider serviceProvider )
	{
		m_logger = logger;
		m_serviceProvider = serviceProvider;
	}
	public async Task StartAsync( CancellationToken cancellationToken )
	{
		// 必要に応じて起動時処理を記述する
		m_logger?.LogInformation( $"In  StartAsync()" );
		cancellationToken.ThrowIfCancellationRequested();
		await Task.CompletedTask;
		m_logger?.LogInformation( $"Out StartAsync()" );
	}

	public async Task StopAsync( CancellationToken cancellationToken )
	{
		// 必要に応じて終了時処理を記述する
		m_logger?.LogInformation( $"In StopAsync()" );
		cancellationToken.ThrowIfCancellationRequested();
		await Task.CompletedTask;
		m_logger?.LogInformation( $"Out StopAsync()" );
	}
}
