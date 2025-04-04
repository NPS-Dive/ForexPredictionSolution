using ForexPrediction.Domain.Interfaces;
using MediatR;

namespace ForexPrediction.Application.Commands.UploadData;

public class UploadDataCommandHandler : IRequestHandler<UploadDataCommand, Unit>
{
    private readonly IDataService _dataService;

    public UploadDataCommandHandler ( IDataService dataService )
    {
        _dataService = dataService;
    }

    public async Task<Unit> Handle ( UploadDataCommand request, CancellationToken cancellationToken )
    {
        await _dataService.UploadDataAsync(request.Pair, request.DataStream);
        return Unit.Value;
    }
}