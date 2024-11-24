namespace KamerConnect.Exceptions;

public class InvalidCredentialsException : NullReferenceException
{
    public InvalidCredentialsException()  : base("Invalid Credentials"){}
}