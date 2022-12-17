namespace ImageViewer.Contracts.Services;

public interface ISelectFileService
{
	string OpenFile( string filter, string defaultExt, string previousFilePath );
}
