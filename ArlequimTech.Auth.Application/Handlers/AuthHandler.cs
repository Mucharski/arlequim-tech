using ArlequimTech.Auth.Application.Handlers.Contracts;
using ArlequimTech.Auth.Application.Services.Contracts;
using ArlequimTech.Auth.Domain.Commands;
using ArlequimTech.Auth.Domain.CommandsResults;
using ArlequimTech.Auth.Domain.Entities;
using ArlequimTech.Auth.Domain.Repositories;
using ArlequimTech.Core.BaseClasses.Interfaces;
using ArlequimTech.Core.Data.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ArlequimTech.Auth.Application.Handlers;

public class AuthHandler : IAuthHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<ICommandResult<CreateUserResponse>> Handle(CreateUserCommand command)
    {
        if (!command.IsValid)
            return new CreateUserCommandResult(success: false, message: "Erro ao criar usuário",
                errors: command.Notifications().Select(x => x.Message));

        var user = await _userRepository.GetAsync(x => x.Email == command.Email);

        if (user != null)
        {
            return new CreateUserCommandResult(success: false, message: "O usuário já existe.");
        }

        var hashedPassword = HashedPassword(command.Password);

        var model = new User(command.Name, command.Email, hashedPassword, command.Role);

        await _userRepository.AddAsync(model);

        return new CreateUserCommandResult(new CreateUserResponse());
    }

    public async Task<ICommandResult<LoginResponse>> Handle(LoginCommand command)
    {
        if (!command.IsValid)
            return new LoginCommandResult(success: false, message: "Erro ao realizar login",
                errors: command.Notifications().Select(x => x.Message));

        var user = await _userRepository.GetAsync(x => x.Email == command.Email);

        if (user == null)
        {
            return new LoginCommandResult(success: false, message: "E-mail ou senha inválidos.");
        }

        var hashedPassword = HashedPassword(command.Password);

        if (user.Password != hashedPassword)
        {
            return new LoginCommandResult(success: false, message: "E-mail ou senha inválidos.");
        }

        var (token, expiresAt) = _jwtTokenService.GenerateToken(user);

        var response = new LoginResponse(
            token: token,
            expiresAt: expiresAt,
            userName: user.Name,
            userEmail: user.Email,
            role: user.Role.ToString()
        );

        return new LoginCommandResult(response);
    }

    private string HashedPassword(string password)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: System.Text.Encoding.UTF8.GetBytes("4872145091"),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
    }
}