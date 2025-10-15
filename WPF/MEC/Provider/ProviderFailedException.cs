namespace MEC.Provider;

public class ProviderFailedException : Exception
{
    public ProviderFailedException() : base() { }
    public ProviderFailedException(string? message) : base(message) { }
    public ProviderFailedException(string? message, Exception? innerException) : base(message, innerException) { }
}
