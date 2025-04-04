using ForexPrediction.Application.Commands;
using ForexPrediction.Application.Commands.SavePrediction;
using ForexPrediction.Domain.Entities;
using ForexPrediction.Domain.Interfaces;
using MediatR;

namespace ForexPrediction.Application.Queries;

public class GetPredictionsQuery : IRequest<List<Prediction>>
{
    public string Pair { get; set; }
    public DateTime StartDate { get; set; }
    public int Days { get; set; }
}

public class GetPredictionsQueryHandler : IRequestHandler<GetPredictionsQuery, List<Prediction>>
{
    private readonly IPredictionService _predictionService;
    private readonly IMediator _mediator;

    public GetPredictionsQueryHandler ( IPredictionService predictionService, IMediator mediator )
    {
        _predictionService = predictionService;
        _mediator = mediator;
    }

    public async Task<List<Prediction>> Handle ( GetPredictionsQuery request, CancellationToken cancellationToken )
    {
        var predictions = await _predictionService.PredictAsync(request.Pair, request.StartDate, request.Days);
        await _mediator.Send(new SavePredictionCommand { Predictions = predictions });
        return predictions;
    }
}