using ForexPrediction.Domain.Entities;
using ForexPrediction.Domain.Interfaces;
using Newtonsoft.Json.Linq;

namespace ForexPrediction.Infrastructure.Services;

public class DataService : IDataService
    {
    private readonly IUnitOfWork _unitOfWork;

    public DataService ( IUnitOfWork unitOfWork )
        {
        _unitOfWork = unitOfWork;
        }

    public async Task UploadDataAsync ( string pair, Stream dataStream )
        {
        using var reader = new StreamReader(dataStream);
        var json = await reader.ReadToEndAsync();

        // 1. Parse the entire JSON into a JObject
        var jObject = JObject.Parse(json);

        // 2. Select the "Time Series (Daily)" token
        var timeSeriesToken = jObject["Time Series (Daily)"];

        if (timeSeriesToken == null || timeSeriesToken.Type == JTokenType.Null) // Check if the key exists and is not null
            {
            throw new InvalidOperationException("The 'Time Series (Daily)' key was not found or is null in the JSON data.");
            }

        // 3. Convert JUST that token into the dictionary structure you need
        var data = timeSeriesToken.ToObject<Dictionary<string, Dictionary<string, string>>>();

        if (data == null)
            {
            throw new InvalidOperationException("Could not convert the 'Time Series (Daily)' JSON section to the expected dictionary structure.");
            }

        // 4. Now your loop should work as intended
        foreach (var day in data) // 'day' is now correctly KeyValuePair<string, Dictionary<string, string>>
            {
            // 'day.Value' is now Dictionary<string, string>
            Dictionary<string, string> dailyValues = day.Value;

            try
                {
                // Debug: Log raw values using the new variable
                Console.WriteLine($"Date: {day.Key}, Open: '{dailyValues["1. open"]}', High: '{dailyValues["2. high"]}', Low: '{dailyValues["3. low"]}', Close: '{dailyValues["4. close"]}', Volume: '{dailyValues.GetValueOrDefault("5. volume")}'"); // Use GetValueOrDefault if volume might be missing

                var entry = new HistoricalData
                    {
                    Date = DateTime.Parse(day.Key),
                    Pair = pair,
                    Open = ConvertToDecimal(dailyValues["1. open"], "Open"),
                    High = ConvertToDecimal(dailyValues["2. high"], "High"),
                    Low = ConvertToDecimal(dailyValues["3. low"], "Low"),
                    Close = ConvertToDecimal(dailyValues["4. close"], "Close"),
                    // Handle potentially missing volume key
                    Volume = dailyValues.ContainsKey("5. volume") ? ConvertToLong(dailyValues["5. volume"], "Volume") : null
                    };
                await _unitOfWork.HistoricalDataRepository.AddAsync(entry);
                }
            catch (KeyNotFoundException knfEx)
                {
                Console.WriteLine($"Error processing date {day.Key}: Missing expected key in daily data. Details: {knfEx.Message}. Skipping this entry.");
                // Decide whether to continue or stop
                // continue;
                }
            catch (Exception ex)
                {
                Console.WriteLine($"Error processing date {day.Key}: {ex.Message}");
                // Consider logging ex.ToString() for full details in case of parsing errors etc.
                throw; // Re-throw to halt processing or handle as needed
                }
            }
        await _unitOfWork.SaveChangesAsync();
        }

    private static decimal ConvertToDecimal ( string value, string fieldName )
        {
        if (string.IsNullOrWhiteSpace(value))
            {
            throw new ArgumentException($"The value for '{fieldName}' is null or empty.");
            }


        if (decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal result))
            {
            return result;
            }

        throw new FormatException($"Unable to parse '{value}' as a decimal for '{fieldName}'.");
        }


    private static long? ConvertToLong ( string value, string fieldName )
        {
        if (string.IsNullOrWhiteSpace(value))
            {
            return null; // Volume is nullable, so return null if empty
            }

        if (long.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out long result))
            {
            return result;
            }
        throw new FormatException($"Unable to parse '{value}' as a long for '{fieldName}'.");
        }
    }
