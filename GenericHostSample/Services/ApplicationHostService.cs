using GenericHostSample.Contracts.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GenericHostSample.Services;

public class ApplicationHostService : IHostedService
{
	public ApplicationHostService( IServiceProvider serviceProvider )
	{
		var lifeTime = serviceProvider.GetService<IHostApplicationLifetime>();
		// スタート処理が一通り終わったら、メインウィンドウを表示する
		lifeTime?.ApplicationStarted.Register( () => serviceProvider.GetService<IMainWindow>()?.Show() );
		// 終了要求が来たら、メインウィンドウをクローズする
		lifeTime?.ApplicationStopping.Register( () => App.Current.MainWindow?.Close() );
	}
	public async Task StartAsync( CancellationToken cancellationToken )
	{
		cancellationToken.ThrowIfCancellationRequested();
		await Task.CompletedTask;
	}

	public async Task StopAsync( CancellationToken cancellationToken )
	{
		cancellationToken.ThrowIfCancellationRequested();
		await Task.CompletedTask;
	}
}
