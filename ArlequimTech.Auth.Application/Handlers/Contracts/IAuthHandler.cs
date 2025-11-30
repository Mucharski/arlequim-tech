using ArlequimTech.Auth.Domain.Commands;
using ArlequimTech.Auth.Domain.Entities;
using ArlequimTech.Core.BaseClasses.Interfaces;

namespace ArlequimTech.Auth.Application.Handlers.Contracts;

public interface IAuthHandler :
    IHandler<CreateUserCommand, CreateUserResponse>,
    IHandler<LoginCommand, LoginResponse>
{

}