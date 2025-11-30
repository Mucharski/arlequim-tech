using System.ComponentModel.DataAnnotations;
using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.BaseClasses.Interfaces;
using ArlequimTech.Core.Data.Enums;
using FluentValidation;

namespace ArlequimTech.Auth.Domain.Commands;

public class CreateUserCommand : NotifiableEntity, ICommand
{
    public CreateUserCommand(string name, string email, string password, UserRole role)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
        
        Validate();
    }
    
    [Required]
    public string Name { get; private set; }
    [Required]
    public string Email { get; private set; }
    [Required]
    public string Password { get; private set; }
    [Required]
    public UserRole Role { get; private set; }
    
    private void Validate()
    {
        CreateUserValidator validator = new();
        var result = validator.Validate(this);

        AddNotifications(result.Errors);
    }
}

internal sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("E-mail inválido.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("A senha deve conter no mínimo 6 caracteres.");

        RuleFor(x => x.Role)
            .NotEqual(UserRole.None)
            .WithMessage("Escolha um perfil para o usuário.");
    }
}
