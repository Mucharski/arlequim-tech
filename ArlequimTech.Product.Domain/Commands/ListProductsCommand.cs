using ArlequimTech.Core.BaseClasses;
using ArlequimTech.Core.BaseClasses.Interfaces;

namespace ArlequimTech.Product.Domain.Commands;

public class ListProductsCommand : NotifiableEntity, ICommand
{
    public ListProductsCommand()
    {
    }
}
