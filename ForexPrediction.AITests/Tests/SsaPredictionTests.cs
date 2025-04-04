using ForexPrediction.Infrastructure.Services;
using Moq;

namespace ForexPrediction.AITests.Tests;

public class SsaPredictionTests
{
    [Fact]
    public async Task PredictAsync_ReturnsPredictions ()
    {
        var unitOfWorkMock = new Mock<ForexPrediction.Domain.Interfaces.IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.HistoricalDataRepository.GetAllAsync())
            .ReturnsAsync(MockData.GetMockHistoricalData("EUR/USD"));

        var service = new SsaPredictionService(unitOfWorkMock.Object);
        var predictions = await service.PredictAsync("EUR/USD", DateTime.Now, 7);

        Assert.NotNull(predictions);
        Assert.Equal(7, predictions.Count);
        Assert.All(predictions, p => Assert.Contains(p.Signal, new[] { "Buy", "Sell" }));
    }
}