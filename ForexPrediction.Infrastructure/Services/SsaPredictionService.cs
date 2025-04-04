using ForexPrediction.Domain.Entities;
using ForexPrediction.Domain.Interfaces;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SsaPredictionService : IPredictionService
    {
    private readonly IUnitOfWork _unitOfWork;
    private readonly MLContext _mlContext;

    public SsaPredictionService ( IUnitOfWork unitOfWork )
        {
        _unitOfWork = unitOfWork;
        _mlContext = new MLContext();
        }

    public async Task<List<Prediction>> PredictAsync ( string pair, DateTime startDate, int days )
        {
        var data = (await _unitOfWork.HistoricalDataRepository.GetAllAsync())
            .Where(h => h.Pair == pair && h.Date >= startDate.AddMonths(-6))
            .OrderBy(h => h.Date)
            .Select(h => h.Close)
            .ToList();

        var dataView = _mlContext.Data.LoadFromEnumerable(data.Select(( v, i ) => new TimeSeriesData { Value = (float)v }));
        var pipeline = _mlContext.Forecasting.ForecastBySsa(
            outputColumnName: "PredictedValues",
            inputColumnName: "Value",
            windowSize: 30,
            seriesLength: data.Count,
            trainSize: data.Count,
            horizon: days);

        var model = pipeline.Fit(dataView);
        var forecastEngine = model.CreateTimeSeriesEngine<TimeSeriesData, TimeSeriesPrediction>(_mlContext);
        var forecast = forecastEngine.Predict(days);

        var predictions = new List<Prediction>();
        for (int i = 0; i < days; i++)
            {
            var signal = i > 0 && forecast.PredictedValues[i] > forecast.PredictedValues[i - 1] ? "Buy" : "Sell";
            predictions.Add(new Prediction
                {
                Date = startDate.AddDays(i + 1),
                Pair = pair,
                Algorithm = "SSA",                // Updated to match new property
                PredictedValue = (decimal)forecast.PredictedValues[i],  // Use PredictedValue instead of PredictedClose
                PredictionDate = DateTime.Now,    // Set to current time or adjust as needed
                Signal = signal                   // Updated to match new property
                });
            }
        return predictions;
        }
    }

public class TimeSeriesData
    {
    public float Value { get; set; }
    }

public class TimeSeriesPrediction
    {
    [VectorType(7)]
    public float[] PredictedValues { get; set; }
    }