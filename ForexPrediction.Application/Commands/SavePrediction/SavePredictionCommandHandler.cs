using ForexPrediction.Domain.Interfaces;
using MediatR;

namespace ForexPrediction.Application.Commands.SavePrediction;

public class SavePredictionCommandHandler : IRequestHandler<SavePredictionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public SavePredictionCommandHandler ( IUnitOfWork unitOfWork )
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle ( SavePredictionCommand request, CancellationToken cancellationToken )
    {
        foreach (var prediction in request.Predictions)
        {
            await _unitOfWork.PredictionRepository.AddAsync(prediction);
        }
        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}