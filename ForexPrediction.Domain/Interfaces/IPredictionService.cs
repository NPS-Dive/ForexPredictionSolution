using ForexPrediction.Domain.Entities;

namespace ForexPrediction.Domain.Interfaces;

public interface IPredictionService
{
    Task<List<Prediction>> PredictAsync ( string pair, DateTime startDate, int days );
}