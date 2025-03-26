namespace MagickaForge
{
    /// <summary>
    /// Exception thrown when a Json configuration would result in an XNB that cannot be loaded in Magicka, should it be due to array size restrictions or exceptions thrown manually by the developers.
    /// </summary>
    /// <param name="message">The message provided by the exception</param>
    public class CantLoadInMagickaException(string message) : Exception(message)
    {
    }
}
