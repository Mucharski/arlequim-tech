using System.ComponentModel.DataAnnotations;
using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.BaseClasses.Interfaces;
using FluentValidation;

namespace ArlequimTech.Stock.Domain.Commands;

public class AddStockCommand : NotifiableEntity, ICommand
{
    public AddStockCommand(Guid productId, int quantity, string invoiceNumber)
    {
        ProductId = productId;
        Quantity = quantity;
        InvoiceNumber = invoiceNumber;

        Validate();
    }

    [Required]
    public Guid ProductId { get; private set; }
    [Required]
    public int Quantity { get; private set; }
    [Required]
    public string InvoiceNumber { get; private set; }

    private void Validate()
    {
        AddStockValidator validator = new();
        var result = validator.Validate(this);

        AddNotifications(result.Errors);
    }
}

internal sealed class AddStockValidator : AbstractValidator<AddStockCommand>
{
    public AddStockValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("O ID do produto é obrigatório.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior que zero.");

        RuleFor(x => x.InvoiceNumber)
            .NotEmpty()
            .WithMessage("O número da nota fiscal é obrigatório.");
    }
}
