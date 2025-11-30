namespace ArlequimTech.Core.BaseClasses.Interfaces;

public interface ICommandResult<T>
{
    bool Success { get; }
    string Message { get; }
    T Data { get; }
    IEnumerable<string> Errors { get; }
}