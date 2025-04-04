using MediatR;

namespace ForexPrediction.Application.Commands.LoginUser;

public class LoginUserCommand : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
}