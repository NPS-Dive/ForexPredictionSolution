using MediatR;
namespace ForexPrediction.Application.Commands.UploadData;

public class UploadDataCommand : IRequest<Unit>
{
    public string Pair { get; set; }
    public Stream DataStream { get; set; }
}