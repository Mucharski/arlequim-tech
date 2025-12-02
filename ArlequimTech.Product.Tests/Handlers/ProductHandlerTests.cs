using System.Linq.Expressions;
using ArlequimTech.Core.Data.Models;
using ArlequimTech.Product.Application.Handlers;
using ArlequimTech.Product.Domain.Commands;
using ArlequimTech.Product.Domain.Repositories;
using NSubstitute;
using Xunit;

namespace ArlequimTech.Product.Tests.Handlers;

public class ProductHandlerTests
{
    private readonly ProductHandler _sut;
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

    public ProductHandlerTests()
    {
        _sut = new ProductHandler(_productRepository);
    }

    #region CreateProductCommand Tests

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "CreateProduct - when command is invalid returns failure")]
    public async Task Handle_CreateProductCommand_InvalidCommand_ReturnsFailure()
    {
        // Arrange
        var command = new CreateProductCommand("", "", 0, 0);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao criar produto", result.Message);
        Assert.NotEmpty(result.Errors);
    }

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "CreateProduct - when product with same name exists returns failure")]
    public async Task Handle_CreateProductCommand_DuplicateName_ReturnsFailure()
    {
        // Arrange
        var command = new CreateProductCommand("Produto Existente", "Descrição", 100m, 10);
        var existingProduct = new Core.Data.Models.Product("Produto Existente", "Descrição", 100m, 10);

        _productRepository.GetAsync(Arg.Any<Expression<Func<Core.Data.Models.Product, bool>>>())
            .Returns(existingProduct);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Já existe um produto com este nome.", result.Message);
    }

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "CreateProduct - when valid creates product and returns success")]
    public async Task Handle_CreateProductCommand_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var command = new CreateProductCommand("Novo Produto", "Descrição", 100m, 10);

        _productRepository.GetAsync(Arg.Any<Expression<Func<Core.Data.Models.Product, bool>>>())
            .Returns((Core.Data.Models.Product?)null);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Produto criado com sucesso!", result.Message);
        await _productRepository.Received(1).AddAsync(Arg.Any<Core.Data.Models.Product>());
    }

    #endregion

    #region UpdateProductCommand Tests

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "UpdateProduct - when command is invalid returns failure")]
    public async Task Handle_UpdateProductCommand_InvalidCommand_ReturnsFailure()
    {
        // Arrange
        var command = new UpdateProductCommand(Guid.Empty, "Nome", "Descrição", 100m);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao atualizar produto", result.Message);
        Assert.NotEmpty(result.Errors);
    }

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "UpdateProduct - when product not found returns failure")]
    public async Task Handle_UpdateProductCommand_ProductNotFound_ReturnsFailure()
    {
        // Arrange
        var command = new UpdateProductCommand(Guid.NewGuid(), "Nome", "Descrição", 100m);

        _productRepository.GetAsync(Arg.Any<Expression<Func<Core.Data.Models.Product, bool>>>())
            .Returns((Core.Data.Models.Product?)null);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Produto não encontrado.", result.Message);
    }

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "UpdateProduct - when another product has same name returns failure")]
    public async Task Handle_UpdateProductCommand_DuplicateName_ReturnsFailure()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new UpdateProductCommand(productId, "Nome Existente", "Descrição", 100m);

        var existingProduct = new Core.Data.Models.Product("Nome Original", "Descrição", 100m, 10);
        typeof(Core.Data.Models.Product).GetProperty("Id")!.SetValue(existingProduct, productId);

        var otherProduct = new Core.Data.Models.Product("Nome Existente", "Descrição", 100m, 10);
        typeof(Core.Data.Models.Product).GetProperty("Id")!.SetValue(otherProduct, Guid.NewGuid());

        _productRepository.GetAsync(Arg.Any<Expression<Func<Core.Data.Models.Product, bool>>>())
            .Returns(
                existingProduct,
                otherProduct
            );

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Já existe outro produto com este nome.", result.Message);
    }

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "UpdateProduct - when valid updates product and returns success")]
    public async Task Handle_UpdateProductCommand_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new UpdateProductCommand(productId, "Nome Atualizado", "Descrição Atualizada", 150m);

        var existingProduct = new Core.Data.Models.Product("Nome Original", "Descrição", 100m, 10);
        typeof(Core.Data.Models.Product).GetProperty("Id")!.SetValue(existingProduct, productId);

        _productRepository.GetAsync(Arg.Any<Expression<Func<Core.Data.Models.Product, bool>>>())
            .Returns(existingProduct, (Core.Data.Models.Product?)null);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Produto atualizado com sucesso!", result.Message);
        await _productRepository.Received(1).UpdateAsync(Arg.Any<Core.Data.Models.Product>());
    }

    #endregion

    #region DeleteProductCommand Tests

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "DeleteProduct - when command is invalid returns failure")]
    public async Task Handle_DeleteProductCommand_InvalidCommand_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteProductCommand(Guid.Empty);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao excluir produto", result.Message);
        Assert.NotEmpty(result.Errors);
    }

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "DeleteProduct - when product not found returns failure")]
    public async Task Handle_DeleteProductCommand_ProductNotFound_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteProductCommand(Guid.NewGuid());

        _productRepository.GetAsync(Arg.Any<Expression<Func<Core.Data.Models.Product, bool>>>())
            .Returns((Core.Data.Models.Product?)null);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Produto não encontrado.", result.Message);
    }

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "DeleteProduct - when valid deletes product and returns success")]
    public async Task Handle_DeleteProductCommand_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new DeleteProductCommand(productId);

        var existingProduct = new Core.Data.Models.Product("Produto", "Descrição", 100m, 10);
        typeof(Core.Data.Models.Product).GetProperty("Id")!.SetValue(existingProduct, productId);

        _productRepository.GetAsync(Arg.Any<Expression<Func<Core.Data.Models.Product, bool>>>())
            .Returns(existingProduct);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Produto excluído com sucesso!", result.Message);
        await _productRepository.Received(1).RemoveAsync(Arg.Any<Expression<Func<Core.Data.Models.Product, bool>>>());
    }

    #endregion

    #region ConsultProductByNameCommand Tests

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "ConsultProductByName - when command is invalid returns failure")]
    public async Task Handle_ConsultProductByNameCommand_InvalidCommand_ReturnsFailure()
    {
        // Arrange
        var command = new ConsultProductByNameCommand("");

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao consultar produtos", result.Message);
        Assert.NotEmpty(result.Errors);
    }

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "ConsultProductByName - when valid returns matching products")]
    public async Task Handle_ConsultProductByNameCommand_ValidCommand_ReturnsProducts()
    {
        // Arrange
        var command = new ConsultProductByNameCommand("Produto");

        var products = new List<Core.Data.Models.Product>
        {
            new Core.Data.Models.Product("Produto 1", "Descrição", 100m, 10),
            new Core.Data.Models.Product("Produto 2", "Descrição", 200m, 20)
        };

        _productRepository.FindAsync(Arg.Any<Expression<Func<Core.Data.Models.Product, bool>>>())
            .Returns(products);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(2, result.Data.Count());
    }

    #endregion

    #region ListProductsCommand Tests

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "ListProducts - returns all products")]
    public async Task Handle_ListProductsCommand_ReturnsAllProducts()
    {
        // Arrange
        var command = new ListProductsCommand();

        var products = new List<Core.Data.Models.Product>
        {
            new Core.Data.Models.Product("Produto 1", "Descrição", 100m, 10),
            new Core.Data.Models.Product("Produto 2", "Descrição", 200m, 20),
            new Core.Data.Models.Product("Produto 3", "Descrição", 300m, 30)
        };

        _productRepository.GetAllAsync()
            .Returns(products);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(3, result.Data.Count());
    }

    #endregion

    #region AddStockQuantityToProductCommand Tests

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "AddStockQuantity - when command is invalid returns failure")]
    public async Task Handle_AddStockQuantityToProductCommand_InvalidCommand_ReturnsFailure()
    {
        // Arrange
        var command = new AddStockQuantityToProductCommand(Guid.Empty, 0);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Erro ao adicionar quantidade ao estoque", result.Message);
        Assert.NotEmpty(result.Errors);
    }

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "AddStockQuantity - when product not found returns failure")]
    public async Task Handle_AddStockQuantityToProductCommand_ProductNotFound_ReturnsFailure()
    {
        // Arrange
        var command = new AddStockQuantityToProductCommand(Guid.NewGuid(), 10);

        _productRepository.GetAsync(Arg.Any<Expression<Func<Core.Data.Models.Product, bool>>>())
            .Returns((Core.Data.Models.Product?)null);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Produto não encontrado.", result.Message);
    }

    [Trait("Product", "Unit tests for Product Handler")]
    [Fact(DisplayName = "AddStockQuantity - when valid adds quantity and returns success")]
    public async Task Handle_AddStockQuantityToProductCommand_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new AddStockQuantityToProductCommand(productId, 10);

        var existingProduct = new Core.Data.Models.Product("Produto", "Descrição", 100m, 5);
        typeof(Core.Data.Models.Product).GetProperty("Id")!.SetValue(existingProduct, productId);

        _productRepository.GetAsync(Arg.Any<Expression<Func<Core.Data.Models.Product, bool>>>())
            .Returns(existingProduct);

        // Act
        var result = await _sut.Handle(command);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Quantidade adicionada ao estoque com sucesso!", result.Message);
        Assert.Equal(15, existingProduct.QuantityInStock);
        await _productRepository.Received(1).UpdateAsync(Arg.Any<Core.Data.Models.Product>());
    }

    #endregion
}
