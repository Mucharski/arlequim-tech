using System.ComponentModel.DataAnnotations;
using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.BaseClasses.Interfaces;
using FluentValidation;

namespace ArlequimTech.Product.Domain.Commands;

public class AddStockQuantityToProductCommand : NotifiableEntity, ICommand
{
    public AddStockQuantityToProductCommand(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;

        Validate();
    }

    [Required]
    public Guid ProductId { get; private set; }
    [Required]
    public int Quantity { get; private set; }

    private void Validate()
    {
        AddStockQuantityToProductValidator validator = new();
        var result = validator.Validate(this);

        AddNotifications(result.Errors);
    }
}

internal sealed class AddStockQuantityToProductValidator : AbstractValidator<AddStockQuantityToProductCommand>
{
    public AddStockQuantityToProductValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("O ID do produto é obrigatório.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior que zero.");
    }
}
