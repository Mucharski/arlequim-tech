using ArlequimTech.Core.BaseClasses.Interfaces;

namespace ArlequimTech.Core.BaseClasses;

public class GenericCommandResult<T> : ICommandResult<T>
{
    public GenericCommandResult(T data, bool success = true, string message = "Dados recuperados com sucesso!",
        IEnumerable<string> errors = null)
    {
        Success = success;
        Message = message;
        Data = data;
        Errors = errors;
    }

    public bool Success { get; private set; }
    public string Message { get; private set; }
    public T Data { get; private set; }
    public IEnumerable<string> Errors { get; private set; }
}