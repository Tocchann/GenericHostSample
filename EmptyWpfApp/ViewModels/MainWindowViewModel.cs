using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmptyWpfApp.Contracts.Services;
using EmptyWpfApp.Contracts.ViewModels;
using EmptyWpfApp.Properties;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EmptyWpfApp.ViewModels
{
	public partial class MainWindowViewModel : ObservableObject, IMainWindowViewModel
	{
		public ICommand FileExitCommand => m_fileExitCommand ?? (m_fileExitCommand = new RelayCommand( OnFileExit ));
		[ObservableProperty]
		private string m_title;


		private readonly ILogger<MainWindowViewModel> m_logger;
		private readonly IMessageBoxService m_msgBox;
		private readonly IHostApplicationLifetime m_lifeTime;
		private ICommand? m_fileExitCommand;
		public MainWindowViewModel( ILogger<MainWindowViewModel> logger, IMessageBoxService msgBox, IHostApplicationLifetime lifeTime )
		{
			m_logger = logger;
			m_msgBox = msgBox;
			m_lifeTime = lifeTime;
			m_title = Resources.AppTitle;
		}
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
		// XAMLデザイン用(DIで起動するため、実行時には利用されない)
		public MainWindowViewModel()
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
		{

		}
		private void OnFileExit()
		{
			m_logger?.LogInformation( "In  IHostApplicationLifetime.ApplicationStopping" );
			if( m_msgBox?.Show( Resources.QueryAppExit, IMessageBoxService.MessageBoxButton.YesNo, IMessageBoxService.MessageBoxImage.Question) == IMessageBoxService.MessageBoxResult.Yes )
			{
				m_lifeTime?.StopApplication();
			}
			m_logger?.LogInformation( "Out IHostApplicationLifetime.ApplicationStopping" );
		}
	}
}
