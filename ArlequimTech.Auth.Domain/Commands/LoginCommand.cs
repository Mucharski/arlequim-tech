using System.ComponentModel.DataAnnotations;
using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.BaseClasses.Interfaces;
using FluentValidation;

namespace ArlequimTech.Auth.Domain.Commands;

public class LoginCommand : NotifiableEntity, ICommand
{
    public LoginCommand(string email, string password)
    {
        Email = email;
        Password = password;

        Validate();
    }

    [Required]
    public string Email { get; private set; }
    [Required]
    public string Password { get; private set; }

    private void Validate()
    {
        LoginValidator validator = new();
        var result = validator.Validate(this);

        AddNotifications(result.Errors);
    }
}

internal sealed class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("E-mail inválido.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("A senha é obrigatória.");
    }
}
