using GenericHostSample.Contracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace GenericHostSample.Services;

public class SelectFileService : ISelectFileService
{
	public string OpenFile( string filter, string defaultExt, string previousFilePath )
	{
		m_logger?.LogDebug( $"Called SelectFileService.OpenFile( filter:{filter}, defaultExt={defaultExt}, previousFilePath={previousFilePath} )" );
		var dlg = new OpenFileDialog();
		dlg.Filter = filter;
		dlg.DefaultExt = defaultExt;
		dlg.FileName = previousFilePath;
		if( dlg.ShowDialog( App.Current.MainWindow ) ?? false )
		{
			m_logger?.LogDebug( $"Select return result={dlg.FileName}" );
			return dlg.FileName;
		}
		m_logger?.LogDebug( "Cancel return" );
		return string.Empty;
	}
	public SelectFileService( ILogger<SelectFileService> logger )
	{
		m_logger = logger;
	}
	private ILogger<SelectFileService> m_logger;
}
