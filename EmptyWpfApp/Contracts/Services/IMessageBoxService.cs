using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyWpfApp.Contracts.Services
{
	/// <summary>
	/// WPF互換メッセージボックスインターフェース
	/// </summary>
	public interface IMessageBoxService
	{
		public enum MessageBoxResult
		{
			None = 0,
			OK = 1,
			Cancel = 2,
			Yes = 6,
			No = 7
		}
		public enum MessageBoxButton
		{
			OK = 0,
			OKCancel = 1,
			YesNoCancel = 3,
			YesNo = 4
		}
		public enum MessageBoxImage
		{
			None = 0,
			Error = 0x10,
			Question = 0x20,
			Exclamation = 0x30,
			Information = 0x40
		}
		public enum MessageBoxOptions
		{
			None = 0,
			DefaultDesktopOnly = 0x20000,
			RightAlign = 0x80000,
			RtlReading = 0x100000,
			ServiceNotification = 0x200000
		}
		MessageBoxResult Show( string message,
			MessageBoxButton button = MessageBoxButton.OK,
			MessageBoxImage image = MessageBoxImage.Exclamation,
			MessageBoxResult defaultResult = MessageBoxResult.None,
			MessageBoxOptions options = MessageBoxOptions.None );
	}
}
