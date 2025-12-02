using System.ComponentModel.DataAnnotations;
using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.BaseClasses.Interfaces;
using FluentValidation;

namespace ArlequimTech.Product.Domain.Commands;

public class DeleteProductCommand : NotifiableEntity, ICommand
{
    public DeleteProductCommand(Guid id)
    {
        Id = id;

        Validate();
    }

    [Required]
    public Guid Id { get; private set; }

    private void Validate()
    {
        DeleteProductValidator validator = new();
        var result = validator.Validate(this);

        AddNotifications(result.Errors);
    }
}

internal sealed class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O ID do produto é obrigatório.");
    }
}
