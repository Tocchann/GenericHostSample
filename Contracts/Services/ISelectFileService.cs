namespace GenericHostSample.Contracts.Services;

public interface ISelectFileService
{
	string OpenFile( string filter, string defaultExt, string previousFilePath );
}
