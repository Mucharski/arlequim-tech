using System.ComponentModel.DataAnnotations;
using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.BaseClasses.Interfaces;
using FluentValidation;

namespace ArlequimTech.Product.Domain.Commands;

public class ConsultProductByNameCommand : NotifiableEntity, ICommand
{
    public ConsultProductByNameCommand(string name)
    {
        Name = name;

        Validate();
    }

    [Required]
    public string Name { get; private set; }

    private void Validate()
    {
        ConsultProductByNameValidator validator = new();
        var result = validator.Validate(this);

        AddNotifications(result.Errors);
    }
}

internal sealed class ConsultProductByNameValidator : AbstractValidator<ConsultProductByNameCommand>
{
    public ConsultProductByNameValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome do produto é obrigatório para a consulta.");
    }
}
