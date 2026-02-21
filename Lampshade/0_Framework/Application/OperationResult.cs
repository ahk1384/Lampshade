namespace _0_Framework.Application;

public class OperationResult
{
    public OperationResult()
    {
        IsSuccess = false;
    }

    public string Message { get; set; }

    public bool IsSuccess { get; set; }

    public OperationResult Success(string message = "The Operation Is Success")
    {
        IsSuccess = true;
        Message = message;
        return this;
    }

    public OperationResult Fail(string message = "The Operation Is Failed")
    {
        IsSuccess = false;
        Message = message;
        return this;
    }
}