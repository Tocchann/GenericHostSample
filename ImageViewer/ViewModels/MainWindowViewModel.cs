using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageViewer.Models;
using ImageViewer.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageViewer.ViewModels
{
	public class MainWindowViewModel : ObservableObject
	{
		public Model Model { get; set; }
		public bool IsRunning { get; set; }
		public ICommand FileOpenCommand => m_fileOpenCommand ?? (m_fileOpenCommand = new RelayCommand( OnFileOpen ));
		public ICommand FileExitCommand => m_fileExitCommand ?? (m_fileExitCommand = new RelayCommand( OnFileExit ));

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

		public MainWindowViewModel()
		{
			Model = new();
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
			var dlg = new OpenFileDialog
			{
				Filter = GetImageFileFilter(),
				DefaultExt = GetFirstFilterExt(),
				FileName = Model.FilePath,
			};
			if( dlg.ShowDialog() != false )
			{
				return dlg.FileName ?? string.Empty;
			}
			return string.Empty;
		}

		private void OnFileExit()
		{
			if( MessageBox.Show( Resources.QueryAppExit, Resources.AppTitle,
				MessageBoxButton.YesNo, MessageBoxImage.Question ) != MessageBoxResult.Yes )
			{
				return;
			}
			IsRunning = false;
			App.Current.MainWindow.Close();
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
}
