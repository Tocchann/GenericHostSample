using EmptyWpfApp.Contracts.Services;
using EmptyWpfApp.Properties;
using Microsoft.Extensions.Logging;

namespace EmptyWpfApp.Services;
public class MessageBoxService : IMessageBoxService
{
	private readonly ILogger<MessageBoxService> m_logger;

	public MessageBoxService( ILogger<MessageBoxService> logger )
	{
		m_logger = logger;
	}
	public IMessageBoxService.MessageBoxResult Show( string message,
		IMessageBoxService.MessageBoxButton button = IMessageBoxService.MessageBoxButton.OK,
		IMessageBoxService.MessageBoxImage image = IMessageBoxService.MessageBoxImage.Exclamation,
		IMessageBoxService.MessageBoxResult defaultResult = IMessageBoxService.MessageBoxResult.None,
		IMessageBoxService.MessageBoxOptions options = IMessageBoxService.MessageBoxOptions.None )
	{
		m_logger?.LogInformation( $"{System.Reflection.MethodBase.GetCurrentMethod()?.Name}( message:{message}, button: {button}, image: {image}, defaultResult : {defaultResult}, options : {options}" );
		var result = App.Current.MainWindow != null
			? System.Windows.MessageBox.Show( App.Current.MainWindow, message, Resources.AppTitle, (System.Windows.MessageBoxButton)button, (System.Windows.MessageBoxImage)image, (System.Windows.MessageBoxResult)defaultResult, (System.Windows.MessageBoxOptions)options )
			: System.Windows.MessageBox.Show( message, Resources.AppTitle, (System.Windows.MessageBoxButton)button, (System.Windows.MessageBoxImage)image, (System.Windows.MessageBoxResult)defaultResult, (System.Windows.MessageBoxOptions)options );
		return (IMessageBoxService.MessageBoxResult)result;
	}
}
