namespace Front.Utilities.Errors;

/// <summary>
/// Generic Exception for when issues with creation Arises
/// </summary>
public class CreationException : Exception
{
    public CreationException(string message) : base(message){}
    public CreationException() : base("Failed to create Entity"){}
}