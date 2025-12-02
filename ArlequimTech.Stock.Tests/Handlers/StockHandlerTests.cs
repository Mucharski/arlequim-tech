using System.Linq.Expressions;
using ArlequimTech.Core.Data.Models;
using ArlequimTech.Core.Messaging;
using ArlequimTech.Core.Messaging.Events;
using ArlequimTech.Core.Messaging.Interfaces;
using ArlequimTech.Product.Domain.Repositories;
using ArlequimTech.Stock.Application.Handlers;
using ArlequimTech.Stock.Domain.Commands;
using ArlequimTech.Stock.Domain.Repositories;
using NSubstitute;
using Xunit;

namespace ArlequimTech.Stock.Tests.Handlers;

public class StockHandlerTests
{
    private readonly StockHandler _sut;
    private readonly IStockEntryRepository _stockEntryRepository = Substitute.For<IStockEntryRepository>();
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IEventPublisher _eventPublisher = Substitute.For<IEventPublisher>();

    public StockHandlerTests()
    {
        _sut = new StockHandler(_stockEntryRepository, _productRepository, _eventPublisher);
    }

    [Trait("Stock", "Unit tests for Stock Handler")]
    [Fact(DisplayName = "AddStock - when command is invalid returns failure")]
    public async Task Handle_AddStockCommand_InvalidCommand_ReturnsFailure()
    {
        // Arrange
        var command = new AddStockCommand(Guid.Empty, 0, "");

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao adicionar estoque", result.Message);
        Assert.NotEmpty(result.Errors);
    }

    [Trait("Stock", "Unit tests for Stock Handler")]
    [Fact(DisplayName = "AddStock - when product not found returns failure")]
    public async Task Handle_AddStockCommand_ProductNotFound_ReturnsFailure()
    {
        // Arrange
        var command = new AddStockCommand(Guid.NewGuid(), 10, "NF-12345");

        _productRepository.GetAsync(Arg.Any<Expression<Func<Core.Data.Models.Product, bool>>>())
            .Returns((Core.Data.Models.Product?)null);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Produto não encontrado.", result.Message);
    }

    [Trait("Stock", "Unit tests for Stock Handler")]
    [Fact(DisplayName = "AddStock - when valid creates stock entry and publishes event")]
    public async Task Handle_AddStockCommand_ValidCommand_CreatesStockEntryAndPublishesEvent()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new AddStockCommand(productId, 10, "NF-12345");

        var product = new Core.Data.Models.Product("Produto Teste", "Descrição", 100m, 5);
        typeof(Core.Data.Models.Product).GetProperty("Id")!.SetValue(product, productId);

        _productRepository.GetAsync(Arg.Any<Expression<Func<Core.Data.Models.Product, bool>>>())
            .Returns(product);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Estoque adicionado com sucesso!", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(productId, result.Data.ProductId);
        Assert.Equal(10, result.Data.Quantity);
        Assert.Equal("NF-12345", result.Data.InvoiceNumber);

        await _stockEntryRepository.Received(1).AddAsync(Arg.Any<StockEntry>());
        await _eventPublisher.Received(1).PublishAsync(
            QueueNames.AddStockQuantityToProduct,
            Arg.Any<AddStockQuantityToProductEvent>()
        );
    }

    [Trait("Stock", "Unit tests for Stock Handler")]
    [Fact(DisplayName = "AddStock - when valid publishes event with correct data")]
    public async Task Handle_AddStockCommand_ValidCommand_PublishesEventWithCorrectData()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var quantity = 15;
        var command = new AddStockCommand(productId, quantity, "NF-67890");

        var product = new Core.Data.Models.Product("Produto Teste", "Descrição", 100m, 5);
        typeof(Core.Data.Models.Product).GetProperty("Id")!.SetValue(product, productId);

        _productRepository.GetAsync(Arg.Any<Expression<Func<Core.Data.Models.Product, bool>>>())
            .Returns(product);

        AddStockQuantityToProductEvent? capturedEvent = null;
        await _eventPublisher.PublishAsync(
            Arg.Any<string>(),
            Arg.Do<AddStockQuantityToProductEvent>(e => capturedEvent = e)
        );

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(capturedEvent);
        Assert.Equal(productId, capturedEvent!.ProductId);
        Assert.Equal(quantity, capturedEvent.Quantity);
    }
}
