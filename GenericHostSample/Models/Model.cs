using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace GenericHostSample.Models;

public class Model
{
	public Model( ILogger<Model>? logger = null )
	{
		m_logger = logger;
		m_filters = new();
	}
	public string FilePath { get; set; } = string.Empty;
	public WriteableBitmap? Image { get; set; }
	public IEnumerable<KeyValuePair<string, string>> ImageFileFilters
	{
		get
		{
			// 要求された時に作り上げる(それまで参照しない)
			if( m_filters.Count == 0 )
			{
				// デフォルトはどこかから取り込んで構築することができないので自前で構築
				m_filters.Add( new KeyValuePair<string, string>( "JPEGファイル", ".jpg,.jpeg,.jpe" ) );
				m_filters.Add( new KeyValuePair<string, string>( "PNG ファイル", ".png" ) );
				m_filters.Add( new KeyValuePair<string, string>( "BMP ファイル", ".bmp,.dib" ) );
				m_filters.Add( new KeyValuePair<string, string>( "GIF ファイル", ".gif" ) );
				m_filters.Add( new KeyValuePair<string, string>( "TIFF ファイル", ".tif,.tiff" ) );
				m_filters.Add( new KeyValuePair<string, string>( "JPEG XR ファイル", ".wdp,.jxr" ) );
				m_filters.Add( new KeyValuePair<string, string>( "DDS ファイル", ".dds" ) );
				m_filters.Add( new KeyValuePair<string, string>( "DNG ファイル", ".dng" ) );
				// WIC がサポートしてるそのほかの形式
				var decoders = Registry.LocalMachine.OpenSubKey( @"SOFTWARE\WOW6432Node\Classes\CLSID\{7ED96837-96F0-4812-B211-F13C24117ED3}\Instance" );
				if( decoders != null )
				{
					foreach( var clsId in decoders.GetSubKeyNames() )
					{
						// コーデックのレジストリを開く
						var codec = Registry.LocalMachine.OpenSubKey( @"SOFTWARE\WOW6432Node\Classes\CLSID\" + clsId );
						if( codec != null )
						{
							var key = codec.GetValue( "FriendlyName" ) as string;
							var value = codec.GetValue( "FileExtensions" ) as string;
							if( key != null && value != null )
							{
								m_filters.Add( new KeyValuePair<string, string>( key, value ) );
							}
						}
					}
				}
			}
			return m_filters;
		}
	}
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
	private List<KeyValuePair<string, string>> m_filters;
}
