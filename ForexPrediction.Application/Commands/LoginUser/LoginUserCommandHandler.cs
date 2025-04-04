using ForexPrediction.Domain.Interfaces;
using MediatR;

namespace ForexPrediction.Application.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IIdentityService _identityService;

    public LoginUserCommandHandler ( IIdentityService identityService )
    {
        _identityService = identityService;
    }

    public async Task<string> Handle ( LoginUserCommand request, CancellationToken cancellationToken )
    {
        return await _identityService.LoginAsync(request.Email, request.Password);
    }
}