namespace SecureNote.Application.Exceptions
{
    /// <summary>
    /// Veri doðrulama hatasýnda throw edilir (400)
    /// </summary>
    public class ValidationException : ApplicationException
    {
        public ValidationException(string message) : base(message) { }
    }
}