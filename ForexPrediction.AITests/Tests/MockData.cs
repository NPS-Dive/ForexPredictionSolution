using ForexPrediction.Domain.Entities;

namespace ForexPrediction.AITests.Tests;

public static class MockData
{
    public static List<HistoricalData> GetMockHistoricalData ( string pair )
    {
        return Enumerable.Range(0, 180).Select(i => new HistoricalData
        {
            Id = i + 1,
            Date = DateTime.Now.AddDays(-179 + i),
            Pair = pair,
            Open = 1.1000m + (decimal)(i * 0.0001),
            High = 1.1010m + (decimal)(i * 0.0001),
            Low = 1.0990m + (decimal)(i * 0.0001),
            Close = 1.1005m + (decimal)(i * 0.0001)
        }).ToList();
    }
}