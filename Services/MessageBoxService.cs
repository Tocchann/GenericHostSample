using GenericHostSample.Contracts.Services;
using GenericHostSample.Properties;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace GenericHostSample.Services;

class MessageBoxService : IMessageBoxService
{
	public MessageBoxResult Show( string message, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Exclamation, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None )
	{
		m_logger?.LogInformation( $"MessageBoxService.Show( message:{message}, button:{button}, icon:{icon},defaultResult:{defaultResult},options:{options}" );

		return App.Current?.MainWindow is not null
			? System.Windows.MessageBox.Show( App.Current.MainWindow, message, Resources.AppTitle, button, icon, defaultResult, options )
			: System.Windows.MessageBox.Show( message, Resources.AppTitle, button, icon, defaultResult, options );
	}
	public MessageBoxService( ILogger<MessageBoxService> logger )
	{
		m_logger = logger;
	}
	private ILogger<MessageBoxService> m_logger;
}
