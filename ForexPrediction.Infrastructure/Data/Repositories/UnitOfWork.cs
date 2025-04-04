using ForexPrediction.Domain.Entities;
using ForexPrediction.Domain.Interfaces;
namespace ForexPrediction.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ForexDbContext _context;
    private bool _disposed = false;

    public IRepository<HistoricalData> HistoricalDataRepository { get; }
    public IRepository<Prediction> PredictionRepository { get; }

    public UnitOfWork ( ForexDbContext context )
    {
        _context = context;
        HistoricalDataRepository = new Repository<HistoricalData>(_context);
        PredictionRepository = new Repository<Prediction>(_context);
    }

    public Task SaveChangesAsync ()
    {
        return Task.Run(() => _context.SaveChanges());
    }

    public void Dispose ()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose ( bool disposing )
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}