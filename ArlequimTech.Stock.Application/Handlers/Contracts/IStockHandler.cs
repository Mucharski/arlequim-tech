using ArlequimTech.Core.BaseClasses.Interfaces;
using ArlequimTech.Core.Data.Models;
using ArlequimTech.Stock.Domain.Commands;

namespace ArlequimTech.Stock.Application.Handlers.Contracts;

public interface IStockHandler : IHandler<AddStockCommand, StockEntry>
{
}
