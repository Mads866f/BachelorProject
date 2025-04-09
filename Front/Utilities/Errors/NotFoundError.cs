namespace Front.Utilities.Errors;

public class NotFoundError : Exception
{
    public NotFoundError(string message) : base(message) { }
    public NotFoundError() : base("NotFound") { }
}