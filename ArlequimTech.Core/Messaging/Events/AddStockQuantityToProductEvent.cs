namespace ArlequimTech.Core.Messaging.Events;

public record AddStockQuantityToProductEvent(
    Guid ProductId,
    int Quantity,
    DateTime OccurredAt
);
