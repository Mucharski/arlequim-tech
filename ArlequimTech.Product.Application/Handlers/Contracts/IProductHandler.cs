using ArlequimTech.Core.BaseClasses.Interfaces;
using ArlequimTech.Product.Domain.Commands;

namespace ArlequimTech.Product.Application.Handlers.Contracts;

public interface IProductHandler :
    IHandler<CreateProductCommand, Core.Data.Models.Product>,
    IHandler<UpdateProductCommand, Core.Data.Models.Product>,
    IHandler<DeleteProductCommand, bool>,
    IHandler<ConsultProductByNameCommand, IEnumerable<Core.Data.Models.Product>>,
    IHandler<ListProductsCommand, IEnumerable<Core.Data.Models.Product>>,
    IHandler<AddStockQuantityToProductCommand, Core.Data.Models.Product>
{
}
