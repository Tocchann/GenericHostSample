using GenericHostSample.Contracts.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericHostSample.Services;

public class AppCloseService : IAppCloseService
{
	public AppCloseService( ILogger<AppCloseService> logger )
	{
		m_logger = logger;
	}
	public void Close()
	{
		m_logger?.LogInformation( $"Called {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
		App.Current.MainWindow?.Close();
	}
	private ILogger<AppCloseService> m_logger;
}
