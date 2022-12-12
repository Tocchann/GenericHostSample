namespace EmptyWpfApp.Contracts.ViewModels;
public interface IMainWindowViewModel
{
	public bool IsRunning { get; set; }
	public void OnFileExit();
}
