using ForexPrediction.Domain.Entities;
using MediatR;
namespace ForexPrediction.Application.Commands.SavePrediction;

public class SavePredictionCommand : IRequest<Unit>
{
    public List<Prediction> Predictions { get; set; }
}