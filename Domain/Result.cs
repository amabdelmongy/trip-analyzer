namespace Domain;

public class Result<T>
{
    public readonly IEnumerable<Error> Errors;
    public readonly bool IsOk;
    public readonly T? Value;

    internal Result(IEnumerable<Error> errors)
    {
        IsOk = false;
        Value = default;
        Errors = errors;
    }

    internal Result()
    {
        IsOk = true;
        Value = default;
        Errors = new List<Error>();
    }

    internal Result(T? value)
    {
        IsOk = true;
        Value = value;
        Errors = new List<Error>();
    }

    public bool HasErrors => !IsOk;
}

public static class Result
{
    public static Result<T?> Ok<T>(T value)
    {
        return new Result<T?>(value);
    }

    public static Result<T> Ok<T>()
    {
        return new Result<T>();
    }

    public static Result<T> Failed<T>(IEnumerable<Error> errors)
    {
        return new Result<T>(errors);
    }

    public static Result<T> Failed<T>(Error error)
    {
        return new Result<T>(new List<Error> { error });
    }
}

public class Error
{
    protected Error(Exception? exception, string message)
    {
        Message = message;
        Exception = exception;
    }

    public Exception? Exception { get; }
    public string Message { get; }

    public static Error CreateFrom(string message)
    {
        return new Error(null, message);
    }

    public static Error CreateFrom(Exception exception)
    {
        return new Error(exception, exception.Message);
    }
}
