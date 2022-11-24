using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenericHostSample.Contracts.Services;
using GenericHostSample.Models;
using GenericHostSample.Properties;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GenericHostSample.ViewModels;
public class MainWindowViewModel : ObservableObject
{
	public MainWindowViewModel( Model model, ILogger<MainWindowViewModel> logger, ISelectFileService fileService, IHostApplicationLifetime lifeTime, IMessageBoxService messageBoxService )
	{
		Model = model;
		m_logger = logger;
		m_selectFileService = fileService;
		m_lifeTime = lifeTime;
		m_messageBoxService = messageBoxService;
		m_selectFileFilter = string.Empty;
	}
	public Model Model { get; set; }

	public string Title => string.IsNullOrEmpty( Model?.FilePath ) ? Resources.AppTitle : Resources.AppTitle + " - " + Path.GetFileName( Model.FilePath );
	public BitmapSource? Image
	{
		get => Model.Image;
		set
		{
			// ビットマップソースから構築する
			OnPropertyChanging();
			Model.Image = new WriteableBitmap( value );
			OnPropertyChanged();
		}
	}
	public ICommand FileOpenCommand => m_fileOpenCommand ?? (m_fileOpenCommand = new RelayCommand( OnFileOpen ));

	public ICommand FileExitCommand => m_fileExitCommand ?? (m_fileExitCommand = new RelayCommand( OnFileExit ));

	private void OnFileOpen()
	{
		m_logger.LogInformation( $"Called {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
		// フィルターは、VM側で調整してやる
		var filePath = m_selectFileService.OpenFile( GetImageFileFilter(), string.Empty, Model.FilePath );
		if( !string.IsNullOrEmpty( filePath ) )
		{
			Model.OpenFile( filePath );
			OnPropertyChanged( nameof( Image ) );
			OnPropertyChanged( nameof( Title ) );
			OnPropertyChanged( nameof( Title ) );
		}
	}
	private void OnFileExit()
	{
		m_logger.LogInformation( $"Called {System.Reflection.MethodBase.GetCurrentMethod()?.Name}()" );
		if( m_messageBoxService.Show( Resources.ExitQuestionMessage, 
			System.Windows.MessageBoxButton.YesNo, 
			System.Windows.MessageBoxImage.Question ) != System.Windows.MessageBoxResult.Yes )
		{
			return;
		}
		m_lifeTime.StopApplication();
	}

	private string GetImageFileFilter()
	{
		if( string.IsNullOrWhiteSpace( m_selectFileFilter ) )
		{
			m_selectFileFilter = string.Join( "|",
				Model.ImageFileFilters.Select( filter =>
					string.Join( "|", filter.Key, string.Join( ';', filter.Value.Split( ',' ).Select( ext => "*" + ext ) ) )
				)
			);
			m_selectFileFilter += "|すべてのファイル|*.*";
		}
		return m_selectFileFilter;
	}
	// デザイナー用のデフォルトコンストラクタ(プロパティの宣言しか参照されることはない
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
	public MainWindowViewModel()
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
	{
	}
	private ICommand? m_fileOpenCommand;
	private ICommand? m_fileExitCommand;
	private readonly ILogger<MainWindowViewModel> m_logger;
	private readonly ISelectFileService m_selectFileService;
	private readonly IHostApplicationLifetime m_lifeTime;
	private readonly IMessageBoxService m_messageBoxService;
	private string m_selectFileFilter;
}
