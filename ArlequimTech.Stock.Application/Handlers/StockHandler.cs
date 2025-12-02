using ArlequimTech.Core.BaseClasses.Interfaces;
using ArlequimTech.Core.Data.Models;
using ArlequimTech.Core.Messaging;
using ArlequimTech.Core.Messaging.Events;
using ArlequimTech.Core.Messaging.Interfaces;
using ArlequimTech.Product.Domain.Repositories;
using ArlequimTech.Stock.Application.Handlers.Contracts;
using ArlequimTech.Stock.Domain.Commands;
using ArlequimTech.Stock.Domain.CommandsResults;
using ArlequimTech.Stock.Domain.Repositories;

namespace ArlequimTech.Stock.Application.Handlers;

public class StockHandler : IStockHandler
{
    private readonly IStockEntryRepository _stockEntryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IEventPublisher _eventPublisher;

    public StockHandler(IStockEntryRepository stockEntryRepository, IProductRepository productRepository, IEventPublisher eventPublisher)
    {
        _stockEntryRepository = stockEntryRepository;
        _productRepository = productRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<ICommandResult<StockEntry>> Handle(AddStockCommand command)
    {
        if (!command.IsValid)
            return new AddStockCommandResult(success: false, message: "Erro ao adicionar estoque",
                errors: command.Notifications().Select(x => x.Message));

        var product = await _productRepository.GetAsync(x => x.Id == command.ProductId);

        if (product == null)
        {
            return new AddStockCommandResult(success: false, message: "Produto não encontrado.");
        }

        var stockEntry = new StockEntry(command.ProductId, command.Quantity, command.InvoiceNumber);

        await _stockEntryRepository.AddAsync(stockEntry);

        await _eventPublisher.PublishAsync(QueueNames.AddStockQuantityToProduct,
            new AddStockQuantityToProductEvent(command.ProductId, command.Quantity, DateTime.UtcNow));

        return new AddStockCommandResult(stockEntry);
    }
}
