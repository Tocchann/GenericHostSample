using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GenericHostSample.Models;

class Model
{
	public Model( ILogger<Model>? logger = null )
	{
		m_logger = logger;
	}
	public string FilePath { get; set; } = string.Empty;
	public WriteableBitmap? Image { get; set; }

	public void OpenFile( string filePath )
	{
		m_logger?.LogInformation( $"Called {System.Reflection.MethodBase.GetCurrentMethod()?.Name}( filePath: {filePath} )");
		// ファイル名が意図したものではない場合は例外を発行
		if( string.IsNullOrWhiteSpace( filePath ) )
		{
			throw new ArgumentNullException( filePath, nameof( filePath ) );
		}
		try
		{
			var decoder = BitmapDecoder.Create( new Uri( filePath, UriKind.RelativeOrAbsolute ), BitmapCreateOptions.None, BitmapCacheOption.Default );
			Image = new WriteableBitmap( decoder.Frames[0] );
			FilePath = filePath;
		}
		catch
		{
			FilePath = string.Empty;
			Image = null;
		}
	}
	public void CloseFile()
	{
		FilePath = string.Empty;
		Image = null;
	}

	private ILogger<Model>? m_logger;
}
