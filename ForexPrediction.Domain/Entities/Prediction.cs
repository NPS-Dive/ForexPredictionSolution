namespace ForexPrediction.Domain.Entities;

public class Prediction
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Pair { get; set; }  // e.g., "EURUSD"
    public decimal PredictedValue { get; set; }  // Use this instead of PredictedClose
    public DateTime PredictionDate { get; set; }
    public string Algorithm { get; set; }  // Added for SSA or other algorithms
    public string Signal { get; set; }     // Added for Buy/Sell signals
}