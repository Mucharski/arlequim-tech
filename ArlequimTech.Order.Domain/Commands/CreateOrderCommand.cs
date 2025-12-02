using System.ComponentModel.DataAnnotations;
using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.BaseClasses.Interfaces;
using FluentValidation;

namespace ArlequimTech.Order.Domain.Commands;

public record OrderItems(Guid ProductId, int Quantity);

public class CreateOrderCommand : NotifiableEntity, ICommand
{
    public CreateOrderCommand(string customerDocument, string sellerName, List<OrderItems> items)
    {
        CustomerDocument = new string(customerDocument.Where(char.IsDigit).ToArray());
        SellerName = sellerName;
        Items = items;

        Validate();
    }

    [Required]
    public string CustomerDocument { get; private set; }
    [Required]
    public string SellerName { get; private set; }
    [Required]
    public List<OrderItems> Items { get; private set; }

    private void Validate()
    {
        CreateOrderValidator validator = new();
        var result = validator.Validate(this);

        AddNotifications(result.Errors);
    }
}

internal sealed class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.CustomerDocument)
            .NotEmpty()
            .WithMessage("O documento do cliente é obrigatório.")
            .MaximumLength(15)
            .WithMessage("O documento do cliente deve ter no máximo 15 caracteres.");

        RuleFor(x => x.SellerName)
            .NotEmpty()
            .WithMessage("O nome do vendedor é obrigatório.")
            .MaximumLength(300)
            .WithMessage("O nome do vendedor deve ter no máximo 300 caracteres.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("O pedido deve conter pelo menos um item.");

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEmpty()
                    .WithMessage("O ID do produto é obrigatório.");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .WithMessage("A quantidade deve ser maior que zero.");
            });
    }
}
