using ArlequimTech.Core.Data.Models;
using ArlequimTech.Order.Application.Handlers;
using ArlequimTech.Order.Domain.Commands;
using ArlequimTech.Order.Domain.Repositories;
using NSubstitute;
using Xunit;

namespace ArlequimTech.Order.Tests.Handlers;

public class OrderHandlerTests
{
    private readonly OrderHandler _sut;
    private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();

    public OrderHandlerTests()
    {
        _sut = new OrderHandler(_orderRepository);
    }

    [Trait("Order", "Unit tests for Order Handler")]
    [Fact(DisplayName = "CreateOrder - when command is invalid returns failure with validation errors")]
    public async Task Handle_CreateOrderCommand_InvalidCommand_ReturnsFailure()
    {
        // Arrange
        var command = new CreateOrderCommand("", "", new List<OrderItems>());

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao criar pedido", result.Message);
        Assert.NotEmpty(result.Errors);
    }

    [Trait("Order", "Unit tests for Order Handler")]
    [Fact(DisplayName = "CreateOrder - when products not found returns failure")]
    public async Task Handle_CreateOrderCommand_ProductsNotFound_ReturnsFailure()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var items = new List<OrderItems> { new OrderItems(productId, 2) };
        var command = new CreateOrderCommand("12345678901", "Vendedor Teste", items);

        _orderRepository.GetProductsByIdsAsync(Arg.Any<IEnumerable<Guid>>())
            .Returns(new List<Product>());

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Um ou mais produtos não foram encontrados.", result.Message);
        Assert.Contains(result.Errors, e => e.Contains(productId.ToString()));
    }

    [Trait("Order", "Unit tests for Order Handler")]
    [Fact(DisplayName = "CreateOrder - when insufficient stock returns failure")]
    public async Task Handle_CreateOrderCommand_InsufficientStock_ReturnsFailure()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var items = new List<OrderItems> { new OrderItems(productId, 10) };
        var command = new CreateOrderCommand("12345678901", "Vendedor Teste", items);

        var product = new Product("Produto Teste", "Descrição", 100m, 5);
        typeof(Product).GetProperty("Id")!.SetValue(product, productId);

        _orderRepository.GetProductsByIdsAsync(Arg.Any<IEnumerable<Guid>>())
            .Returns(new List<Product> { product });

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Não foi possível criar o pedido devido a estoque insuficiente.", result.Message);
        Assert.Contains(result.Errors, e => e.Contains("Estoque insuficiente"));
    }

    [Trait("Order", "Unit tests for Order Handler")]
    [Fact(DisplayName = "CreateOrder - when valid order creates order and decrements stock")]
    public async Task Handle_CreateOrderCommand_ValidOrder_CreatesOrderAndDecrementsStock()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var items = new List<OrderItems> { new OrderItems(productId, 2) };
        var command = new CreateOrderCommand("12345678901", "Vendedor Teste", items);

        var product = new Product("Produto Teste", "Descrição", 100m, 10);
        typeof(Product).GetProperty("Id")!.SetValue(product, productId);

        _orderRepository.GetProductsByIdsAsync(Arg.Any<IEnumerable<Guid>>())
            .Returns(new List<Product> { product });

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Pedido criado com sucesso!", result.Message);
        await _orderRepository.Received(1).AddAsync(Arg.Any<Core.Data.Models.Order>());
        Assert.Equal(8, product.QuantityInStock);
    }

    [Trait("Order", "Unit tests for Order Handler")]
    [Fact(DisplayName = "CreateOrder - when multiple products some not found returns failure")]
    public async Task Handle_CreateOrderCommand_MultipleProductsSomeNotFound_ReturnsFailure()
    {
        // Arrange
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();
        var items = new List<OrderItems>
        {
            new OrderItems(productId1, 2),
            new OrderItems(productId2, 3)
        };
        var command = new CreateOrderCommand("12345678901", "Vendedor Teste", items);

        var product1 = new Product("Produto 1", "Descrição", 100m, 10);
        typeof(Product).GetProperty("Id")!.SetValue(product1, productId1);

        _orderRepository.GetProductsByIdsAsync(Arg.Any<IEnumerable<Guid>>())
            .Returns(new List<Product> { product1 });

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Um ou mais produtos não foram encontrados.", result.Message);
        Assert.Contains(result.Errors, e => e.Contains(productId2.ToString()));
        Assert.DoesNotContain(result.Errors, e => e.Contains(productId1.ToString()));
    }
}
