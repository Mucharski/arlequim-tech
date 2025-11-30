namespace ArlequimTech.Core.BaseClasses.Interfaces;

public interface IHandler<TCommand, TResult> where TCommand : ICommand
{
    Task<ICommandResult<TResult>> Handle(TCommand command);
}