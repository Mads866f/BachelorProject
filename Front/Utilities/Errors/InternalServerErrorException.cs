namespace Front.Utilities.Errors;
/// <summary>
/// Error that signifies that something went wrong within the system.
/// Used as an overall error, Can be specified with message
/// </summary>
public class InternalServerErrorException : Exception
{
    public InternalServerErrorException() : base("Internal Server Error")
    {
    }

    public InternalServerErrorException(string message) : base(message)
    {
    }
}