using ArlequimTech.Core.BaseClasses.Interfaces;
using ArlequimTech.Order.Application.Handlers.Contracts;
using ArlequimTech.Order.Domain.Commands;
using ArlequimTech.Order.Domain.CommandsResults;
using ArlequimTech.Order.Domain.Repositories;

namespace ArlequimTech.Order.Application.Handlers;

public class OrderHandler : IOrderHandler
{
    private readonly IOrderRepository _orderRepository;

    public OrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<ICommandResult<Core.Data.Models.Order>> Handle(CreateOrderCommand command)
    {
        if (!command.IsValid)
            return new CreateOrderCommandResult(success: false, message: "Erro ao criar pedido",
                errors: command.Notifications().Select(x => x.Message));

        var productIds = command.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = await _orderRepository.GetProductsByIdsAsync(productIds);

        var missingProductIds = productIds.Except(products.Select(p => p.Id)).ToList();
        if (missingProductIds.Any())
        {
            return new CreateOrderCommandResult(success: false,
                message: "Um ou mais produtos não foram encontrados.",
                errors: missingProductIds.Select(id => $"Produto com ID '{id}' não encontrado."));
        }

        var stockErrors = new List<string>();
        foreach (var item in command.Items)
        {
            var product = products.First(p => p.Id == item.ProductId);
            if (product.QuantityInStock < item.Quantity)
            {
                stockErrors.Add($"Estoque insuficiente para o produto '{product.Name}'. Disponível: {product.QuantityInStock}, Solicitado: {item.Quantity}");
            }
        }

        if (stockErrors.Any())
        {
            return new CreateOrderCommandResult(success: false,
                message: "Não foi possível criar o pedido devido a estoque insuficiente.",
                errors: stockErrors);
        }
        
        var order = new Core.Data.Models.Order(command.CustomerDocument, command.SellerName);
        
        foreach (var item in command.Items)
        {
            var product = products.First(p => p.Id == item.ProductId);
            order.AddProduct(product);
            product.DecrementQuantity(item.Quantity);
        }

        await _orderRepository.AddAsync(order);
        
        return new CreateOrderCommandResult();
    }
}
