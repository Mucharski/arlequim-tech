using System.ComponentModel.DataAnnotations;
using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.BaseClasses.Interfaces;
using FluentValidation;

namespace ArlequimTech.Product.Domain.Commands;

public class CreateProductCommand : NotifiableEntity, ICommand
{
    public CreateProductCommand(string name, string description, decimal price, int quantityInStock)
    {
        Name = name;
        Description = description;
        Price = price;
        QuantityInStock = quantityInStock;

        Validate();
    }

    [Required]
    public string Name { get; private set; }
    [Required]
    public string Description { get; private set; }
    [Required]
    public decimal Price { get; private set; }
    [Required]
    public int QuantityInStock { get; private set; }

    private void Validate()
    {
        CreateProductValidator validator = new();
        var result = validator.Validate(this);

        AddNotifications(result.Errors);
    }
}

internal sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome do produto é obrigatório.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("A descrição do produto é obrigatória.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("O preço deve ser maior que zero.");

        RuleFor(x => x.QuantityInStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("A quantidade em estoque não pode ser negativa.");
    }
}
