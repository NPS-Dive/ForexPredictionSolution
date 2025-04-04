using MediatR;

namespace ForexPrediction.Application.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
}