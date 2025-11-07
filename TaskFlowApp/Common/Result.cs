namespace TaskFlowApp.Common;

public class Result<T>
{
    public T? Value { get;}
    public bool IsSuccess { get;}
    public string? Error { get;}

    private Result(T? value, bool isSuccess, string? error)
    {
        this.Value = value;
        this.IsSuccess = isSuccess;
        this.Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, null);
    }

    public static Result<T> Failure(string errorMessage)
    {
        return new Result<T>(default, false, errorMessage);
    }
    
}