namespace SecureNote.Application.Exceptions
{
    /// <summary>
    /// Uygulama özel hatalarýnýn temel sýnýfý
    /// </summary>
    public class AppException : Exception
    {
        public AppException(string message) : base(message) { }
    }
}