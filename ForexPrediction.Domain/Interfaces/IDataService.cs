namespace ForexPrediction.Domain.Interfaces;

public interface IDataService
{
    Task UploadDataAsync ( string pair, Stream dataStream );
}