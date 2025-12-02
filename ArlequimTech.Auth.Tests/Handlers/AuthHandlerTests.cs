using System.Linq.Expressions;
using ArlequimTech.Auth.Application.Handlers;
using ArlequimTech.Auth.Application.Services.Contracts;
using ArlequimTech.Auth.Domain.Commands;
using ArlequimTech.Auth.Domain.Repositories;
using ArlequimTech.Core.Data.Enums;
using ArlequimTech.Core.Data.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using NSubstitute;
using Xunit;

namespace ArlequimTech.Auth.Tests.Handlers;

public class AuthHandlerTests
{
    private readonly AuthHandler _sut;
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IJwtTokenService _jwtTokenService = Substitute.For<IJwtTokenService>();

    public AuthHandlerTests()
    {
        _sut = new AuthHandler(_userRepository, _jwtTokenService);
    }

    #region CreateUserCommand Tests

    [Trait("Auth", "Unit tests for Auth Handler")]
    [Fact(DisplayName = "CreateUser - when command is invalid returns failure")]
    public async Task Handle_CreateUserCommand_InvalidCommand_ReturnsFailure()
    {
        // Arrange
        var command = new CreateUserCommand("", "", "", UserRole.None);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao criar usuário", result.Message);
        Assert.NotEmpty(result.Errors);
    }

    [Trait("Auth", "Unit tests for Auth Handler")]
    [Fact(DisplayName = "CreateUser - when user already exists returns failure")]
    public async Task Handle_CreateUserCommand_UserExists_ReturnsFailure()
    {
        // Arrange
        var command = new CreateUserCommand("Zézinho do teste", "teste@email.com", "password123", UserRole.User);

        var existingUser = new User("Zézinho do teste", "teste@email.com", "hashedPassword", UserRole.User);

        _userRepository.GetAsync(Arg.Any<Expression<Func<User, bool>>>())
            .Returns(existingUser);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("O usuário já existe.", result.Message);
    }

    [Trait("Auth", "Unit tests for Auth Handler")]
    [Fact(DisplayName = "CreateUser - when valid creates user and returns success")]
    public async Task Handle_CreateUserCommand_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var command = new CreateUserCommand("Zézinho do teste", "teste@email.com", "password123", UserRole.User);

        _userRepository.GetAsync(Arg.Any<Expression<Func<User, bool>>>())
            .Returns((User?)null);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Usuário criado com sucesso!", result.Message);
        await _userRepository.Received(1).AddAsync(Arg.Any<User>());
    }

    #endregion

    #region LoginCommand Tests

    [Trait("Auth", "Unit tests for Auth Handler")]
    [Fact(DisplayName = "Login - when command is invalid returns failure")]
    public async Task Handle_LoginCommand_InvalidCommand_ReturnsFailure()
    {
        // Arrange
        var command = new LoginCommand("", "");

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao realizar login", result.Message);
        Assert.NotEmpty(result.Errors);
    }

    [Trait("Auth", "Unit tests for Auth Handler")]
    [Fact(DisplayName = "Login - when user not found returns failure")]
    public async Task Handle_LoginCommand_UserNotFound_ReturnsFailure()
    {
        // Arrange
        var command = new LoginCommand("teste@email.com", "password123");

        _userRepository.GetAsync(Arg.Any<Expression<Func<User, bool>>>())
            .Returns((User?)null);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("E-mail ou senha inválidos.", result.Message);
    }

    [Trait("Auth", "Unit tests for Auth Handler")]
    [Fact(DisplayName = "Login - when password is wrong returns failure")]
    public async Task Handle_LoginCommand_WrongPassword_ReturnsFailure()
    {
        // Arrange
        var command = new LoginCommand("teste@email.com", "wrongPassword");

        var hashedCorrectPassword = HashedPassword("correctPassword");
        var existingUser = new User("Zézinho do teste", "teste@email.com", hashedCorrectPassword, UserRole.User);

        _userRepository.GetAsync(Arg.Any<Expression<Func<User, bool>>>())
            .Returns(existingUser);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("E-mail ou senha inválidos.", result.Message);
    }

    [Trait("Auth", "Unit tests for Auth Handler")]
    [Fact(DisplayName = "Login - when valid credentials returns token and user info")]
    public async Task Handle_LoginCommand_ValidCredentials_ReturnsTokenAndUserInfo()
    {
        // Arrange
        var password = "teste12345";
        var command = new LoginCommand("teste@email.com", password);

        var hashedPassword = HashedPassword(password);
        var existingUser = new User("Zézinho do Teste", "teste@email.com", hashedPassword, UserRole.User);

        _userRepository.GetAsync(Arg.Any<Expression<Func<User, bool>>>())
            .Returns(existingUser);

        var expiresAt = DateTime.UtcNow.AddHours(1);
        _jwtTokenService.GenerateToken(Arg.Any<User>())
            .Returns(("jwt-token", expiresAt));

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("jwt-token", result.Data.Token);
        Assert.Equal(expiresAt, result.Data.ExpiresAt);
        Assert.Equal("Zézinho do Teste", result.Data.UserName);
        Assert.Equal("teste@email.com", result.Data.UserEmail);
        Assert.Equal("User", result.Data.Role);
    }

    [Trait("Auth", "Unit tests for Auth Handler")]
    [Fact(DisplayName = "Login - when valid calls jwt service with user")]
    public async Task Handle_LoginCommand_ValidCredentials_CallsJwtServiceWithUser()
    {
        // Arrange
        var password = "teste12345";
        var command = new LoginCommand("teste@email.com", password);

        var hashedPassword = HashedPassword(password);
        var existingUser = new User("Zézinho do Teste", "teste@email.com", hashedPassword, UserRole.User);

        _userRepository.GetAsync(Arg.Any<Expression<Func<User, bool>>>())
            .Returns(existingUser);

        var expiresAt = DateTime.UtcNow.AddHours(1);
        _jwtTokenService.GenerateToken(Arg.Any<User>())
            .Returns(("jwt-token", expiresAt));

        // Act
        await _sut.Handle(command);

        // Assert
        _jwtTokenService.Received(1).GenerateToken(Arg.Is<User>(u => u.Email == "teste@email.com"));
    }

    #endregion

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
