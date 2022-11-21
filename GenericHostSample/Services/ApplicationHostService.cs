using GenericHostSample.Contracts.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		cancellationToken.ThrowIfCancellationRequested();
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
