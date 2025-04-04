using ForexPrediction.Domain.Entities;

namespace ForexPrediction.Infrastructure.Data.Repositories;

public class HistoricalDataRepository : Repository<HistoricalData>
{
    public HistoricalDataRepository ( ForexDbContext context ) : base(context) { }
}