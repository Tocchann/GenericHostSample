using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageViewer.Contracts.Services;
using ImageViewer.Contracts.ViewModels;
using ImageViewer.Models;
using ImageViewer.Properties;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageViewer.ViewModels;

public partial class MainWindowViewModel : ObservableObject, IMainWindowViewModel
{
	public Model Model { get; set; }
	public ICommand FileOpenCommand => m_fileOpenCommand ?? (m_fileOpenCommand = new RelayCommand( OnFileOpen ));
	public ICommand FileExitCommand => m_fileExitCommand ?? (m_fileExitCommand = new RelayCommand( OnFileExit ));
	public bool IsRunning { get; set; }

	public string Title => string.IsNullOrEmpty( Model?.FilePath ) ? Resources.AppTitle : Resources.AppTitle + " - " + Path.GetFileName( Model.FilePath );
	public BitmapSource? Image
	{
		get => Model.Image;
		set
		{
			OnPropertyChanging();
			Model.Image = new WriteableBitmap( value );
			OnPropertyChanged();
		}
	}
	private IMessageBoxService m_msgBox;
	private ISelectFileService m_fileService;
	private IHostApplicationLifetime m_lifeTime;

	public void ExitApplication()
	{
		OnFileExit();
	}

#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
	// XAMLエディタようにデフォルトコンストラクタを用意しておく
	public MainWindowViewModel()
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
	{
	}
	public MainWindowViewModel( Model model, IMessageBoxService msgBox, ISelectFileService fileService, IHostApplicationLifetime lifeTime )
	{
		Model = model;
		m_msgBox = msgBox;
		m_fileService = fileService;
		m_lifeTime = lifeTime;
		IsRunning = false;
	}
	private ICommand? m_fileOpenCommand;
	private ICommand? m_fileExitCommand;
	private string? m_selectFileFilter;
	private string? m_firstFilterExt;

	private void OnFileOpen()
	{
		var filePath = SelectFile();
		if( !string.IsNullOrEmpty( filePath) )
		{
			Model.OpenFile( filePath );
			OnPropertyChanged( nameof( Image ) );
			OnPropertyChanged( nameof( Title ) );
		}
	}

	private string SelectFile()
	{
		return m_fileService.OpenFile( GetImageFileFilter(), GetFirstFilterExt(), Model.FilePath );
		//var dlg = new OpenFileDialog
		//{
		//	Filter = GetImageFileFilter(),
		//	DefaultExt = GetFirstFilterExt(),
		//	FileName = Model.FilePath,
		//};
		//if( dlg.ShowDialog() != false )
		//{
		//	return dlg.FileName ?? string.Empty;
		//}
		//return string.Empty;
	}

	private void OnFileExit()
	{
		//if( MessageBox.Show( Resources.QueryAppExit, Resources.AppTitle,
		if( m_msgBox.Show( Resources.QueryAppExit,
			MessageBoxButton.YesNo, MessageBoxImage.Question ) != MessageBoxResult.Yes )
		{
			return;
		}
		IsRunning = false;
		//App.Current?.MainWindow?.Close();
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
	private string GetFirstFilterExt()
	{
		if( string.IsNullOrWhiteSpace( m_firstFilterExt ) )
		{
			m_firstFilterExt = "*" + Model.ImageFileFilters.First().Value.Split( ',' ).First();
		}
		return m_firstFilterExt;
	}
}
