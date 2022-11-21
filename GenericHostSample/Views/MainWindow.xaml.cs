using GenericHostSample.Contracts.Views;
using GenericHostSample.ViewModels;

namespace GenericHostSample.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IMainWindow
{
	public MainWindow( MainWindowViewModel vm )
	{
		DataContext = vm;
		InitializeComponent();
	}
}
