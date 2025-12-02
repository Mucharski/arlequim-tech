using System.ComponentModel.DataAnnotations;
using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.BaseClasses.Interfaces;
using FluentValidation;

namespace ArlequimTech.Product.Domain.Commands;

public class UpdateProductCommand : NotifiableEntity, ICommand
{
    public UpdateProductCommand(Guid id, string name, string description, decimal price)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;

        Validate();
    }

    [Required]
    public Guid Id { get; private set; }
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public decimal? Price { get; private set; }

    private void Validate()
    {
        UpdateProductValidator validator = new();
        var result = validator.Validate(this);

        AddNotifications(result.Errors);
    }
}

internal sealed class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O ID do produto é obrigatório.");
    }
}
