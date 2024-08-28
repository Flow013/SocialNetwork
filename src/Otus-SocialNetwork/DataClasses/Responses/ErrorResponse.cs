namespace OtusSocialNetwork.DataClasses.Responses;

public class ErrorResponse
{
    public ErrorResponse(string message)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    public string Message { get; set; }
}
