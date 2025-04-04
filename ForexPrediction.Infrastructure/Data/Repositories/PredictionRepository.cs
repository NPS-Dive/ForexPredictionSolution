using ForexPrediction.Domain.Entities;

namespace ForexPrediction.Infrastructure.Data.Repositories;

public class PredictionRepository : Repository<Prediction>
{
    public PredictionRepository ( ForexDbContext context ) : base(context) { }
}