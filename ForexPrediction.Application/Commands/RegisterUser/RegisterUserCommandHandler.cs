using ForexPrediction.Domain.Interfaces;
using MediatR;

namespace ForexPrediction.Application.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
{
    private readonly IIdentityService _identityService;

    public RegisterUserCommandHandler ( IIdentityService identityService )
    {
        _identityService = identityService;
    }

    public async Task<string> Handle ( RegisterUserCommand request, CancellationToken cancellationToken )
    {
        return await _identityService.RegisterAsync(request.Email, request.Password);
    }
}