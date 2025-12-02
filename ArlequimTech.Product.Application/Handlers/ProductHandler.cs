using ArlequimTech.Core.BaseClasses.Interfaces;
using ArlequimTech.Product.Application.Handlers.Contracts;
using ArlequimTech.Product.Domain.Commands;
using ArlequimTech.Product.Domain.CommandsResults;
using ArlequimTech.Product.Domain.Repositories;

namespace ArlequimTech.Product.Application.Handlers;

public class ProductHandler : IProductHandler
{
    private readonly IProductRepository _productRepository;

    public ProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ICommandResult<Core.Data.Models.Product>> Handle(CreateProductCommand command)
    {
        if (!command.IsValid)
            return new CreateProductCommandResult(success: false, message: "Erro ao criar produto",
                errors: command.Notifications().Select(x => x.Message));

        var existingProduct = await _productRepository.GetAsync(x => x.Name == command.Name);

        if (existingProduct != null)
        {
            return new CreateProductCommandResult(success: false, message: "Já existe um produto com este nome.");
        }

        var product = new Core.Data.Models.Product(command.Name, command.Description, command.Price, command.QuantityInStock);

        await _productRepository.AddAsync(product);

        return new CreateProductCommandResult(product);
    }

    public async Task<ICommandResult<Core.Data.Models.Product>> Handle(UpdateProductCommand command)
    {
        if (!command.IsValid)
            return new UpdateProductCommandResult(success: false, message: "Erro ao atualizar produto",
                errors: command.Notifications().Select(x => x.Message));

        var product = await _productRepository.GetAsync(x => x.Id == command.Id);

        if (product == null)
        {
            return new UpdateProductCommandResult(success: false, message: "Produto não encontrado.");
        }

        var existingProduct = await _productRepository.GetAsync(x => x.Name == command.Name && x.Id != command.Id);

        if (existingProduct != null)
        {
            return new UpdateProductCommandResult(success: false, message: "Já existe outro produto com este nome.");
        }

        product.Update(command.Name, command.Description, command.Price);

        await _productRepository.UpdateAsync(product);

        return new UpdateProductCommandResult(product);
    }

    public async Task<ICommandResult<bool>> Handle(DeleteProductCommand command)
    {
        if (!command.IsValid)
            return new DeleteProductCommandResult(success: false, message: "Erro ao excluir produto",
                errors: command.Notifications().Select(x => x.Message));

        var product = await _productRepository.GetAsync(x => x.Id == command.Id);

        if (product == null)
        {
            return new DeleteProductCommandResult(success: false, message: "Produto não encontrado.");
        }

        await _productRepository.RemoveAsync(x => x.Id == command.Id);

        return new DeleteProductCommandResult();
    }

    public async Task<ICommandResult<IEnumerable<Core.Data.Models.Product>>> Handle(ConsultProductByNameCommand command)
    {
        if (!command.IsValid)
            return new ConsultProductByNameCommandResult(success: false, message: "Erro ao consultar produtos",
                errors: command.Notifications().Select(x => x.Message));

        var products = await _productRepository.FindAsync(x => x.Name.Contains(command.Name));

        return new ConsultProductByNameCommandResult(products);
    }

    public async Task<ICommandResult<IEnumerable<Core.Data.Models.Product>>> Handle(ListProductsCommand command)
    {
        var products = await _productRepository.GetAllAsync();

        return new ListProductsCommandResult(products);
    }

    public async Task<ICommandResult<Core.Data.Models.Product>> Handle(AddStockQuantityToProductCommand command)
    {
        if (!command.IsValid)
            return new AddStockQuantityToProductCommandResult(success: false, message: "Erro ao adicionar quantidade ao estoque",
                errors: command.Notifications().Select(x => x.Message));

        var product = await _productRepository.GetAsync(x => x.Id == command.ProductId);

        if (product == null)
        {
            return new AddStockQuantityToProductCommandResult(success: false, message: "Produto não encontrado.");
        }

        product.AddQuantity(command.Quantity);

        await _productRepository.UpdateAsync(product);

        return new AddStockQuantityToProductCommandResult(product);
    }
}
