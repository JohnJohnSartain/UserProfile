namespace Interface.Models;

public class AutoWrapperResponseModel<TEntity>
{
    public string Message { get; set; }
    public TEntity Result { get; set; }
    public bool IsError { get; set; }
    public ErrorDetails Errors { get; set; }
    public ResponseException ResponseException { get; set; }
    public IEnumerable<ValidationError> ValidationErrors { get; set; }
}

public class ResponseException
{
    public string ExceptionMessage { get; set; }
    public IEnumerable<ValidationError> ValidationErrors { get; set; }
}

public class ErrorDetails
{
    public string Message { get; set; }
    public string Type { get; set; }
    public string Source { get; set; }
    public string Raw { get; set; }
}

public class ValidationError
{
    public string Name { get; }
    public string Reason { get; }

    public ValidationError(string name, string reason)
    {
        Name = name != string.Empty ? name : null;
        Reason = reason;
    }
}