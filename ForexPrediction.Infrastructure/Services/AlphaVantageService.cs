using ForexPrediction.Domain.Entities;
using ForexPrediction.Domain.Interfaces;
using System.Net.Http.Json;
namespace ForexPrediction.Infrastructure.Services;

public class AlphaVantageService
{
    private readonly HttpClient _client;
    private readonly IUnitOfWork _unitOfWork;

    public AlphaVantageService ( HttpClient client, IUnitOfWork unitOfWork )
    {
        _client = client;
        _unitOfWork = unitOfWork;
        _client.BaseAddress = new Uri("https://www.alphavantage.co/");
    }

    public async Task FetchDataAsync ( string pair )
    {
        var response = await _client.GetFromJsonAsync<Dictionary<string, dynamic>>(
            $"query?function=TIME_SERIES_DAILY&symbol={pair}&apikey=YOUR_API_KEY");
        var timeSeries = response["Time Series (Daily)"];

        foreach (var day in timeSeries)
        {
            var entry = new HistoricalData
            {
                Date = DateTime.Parse(day.Name),
                Pair = pair,
                Open = decimal.Parse(day.Value["1. open"]),
                High = decimal.Parse(day.Value["2. high"]),
                Low = decimal.Parse(day.Value["3. low"]),
                Close = decimal.Parse(day.Value["4. close"])
            };
            await _unitOfWork.HistoricalDataRepository.AddAsync(entry);
        }
        await _unitOfWork.SaveChangesAsync();
    }
}