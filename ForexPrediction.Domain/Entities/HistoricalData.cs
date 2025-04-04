namespace ForexPrediction.Domain.Entities;

public class HistoricalData
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Pair { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public long? Volume { get; set; }
}