using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmptyWpfApp.Services
{
	public class ApplicationHostService : IHostedService
	{
		private ILogger<ApplicationHostService> m_logger;

		public ApplicationHostService( ILogger<ApplicationHostService> logger )
		{
			m_logger = logger;
		}
		public async Task StartAsync( CancellationToken cancellationToken )
		{
			m_logger?.LogInformation( $"In  {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
			cancellationToken.ThrowIfCancellationRequested();
			await Task.CompletedTask;
			m_logger?.LogInformation( $"Out {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
		}

		public async Task StopAsync( CancellationToken cancellationToken )
		{
			m_logger?.LogInformation( $"In {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
			cancellationToken.ThrowIfCancellationRequested();
			await Task.CompletedTask;
			m_logger?.LogInformation( $"Out {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
		}
	}
}
