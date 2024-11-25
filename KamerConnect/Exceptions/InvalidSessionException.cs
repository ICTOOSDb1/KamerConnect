namespace KamerConnect.Exceptions;

public class InvalidSessionException : NullReferenceException
{
    public InvalidSessionException()  : base("Invalid Session"){}
    
    
    public InvalidSessionException(string message)  : base(message){}
}