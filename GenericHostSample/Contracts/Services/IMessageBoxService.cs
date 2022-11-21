namespace GenericHostSample.Contracts.Services;
public interface IMessageBoxService
{
	System.Windows.MessageBoxResult Show(
		string message,
		System.Windows.MessageBoxButton button = System.Windows.MessageBoxButton.OK,
		System.Windows.MessageBoxImage icon = System.Windows.MessageBoxImage.Exclamation,
		System.Windows.MessageBoxResult defaultResult = System.Windows.MessageBoxResult.None,
		System.Windows.MessageBoxOptions options = System.Windows.MessageBoxOptions.None );
}
