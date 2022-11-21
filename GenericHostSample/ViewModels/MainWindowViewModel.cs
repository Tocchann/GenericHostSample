using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenericHostSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GenericHostSample.ViewModels;
class MainWindowViewModel : ObservableObject
{
	public MainWindowViewModel()
	{
		Model = new Model();
	}
	public MainWindowViewModel( Model model )
	{
		Model = model;
	}
	public Model Model { get; set; }

	public BitmapSource Image
	{
		// uint == 32 == 8x4 == ARGB()
		get => Model.Image ?? BitmapSource.Create( 1, 1, 96, 96, PixelFormats.Bgra32, null, new uint[]{ 0x00000000 }, 1 );
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
		System.Windows.MessageBox.Show( "工事中...ファイル-開く" );
	}
	private void OnFileExit()
	{
		App.Current.MainWindow.Close();
	}
	private ICommand? m_fileOpenCommand;
	private ICommand? m_fileExitCommand;
}
