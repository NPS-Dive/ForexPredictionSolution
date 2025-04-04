using ForexPrediction.Domain.Entities;

namespace ForexPrediction.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<HistoricalData> HistoricalDataRepository { get; }
    IRepository<Prediction> PredictionRepository { get; }
    Task SaveChangesAsync ();
}