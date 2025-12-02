using ArlequimTech.Core.BaseClasses.Interfaces;
using ArlequimTech.Order.Domain.Commands;

namespace ArlequimTech.Order.Application.Handlers.Contracts;

public interface IOrderHandler :
    IHandler<CreateOrderCommand, Core.Data.Models.Order>
{
}
