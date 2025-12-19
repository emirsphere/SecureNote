namespace SecureNote.Application.Exceptions
{
    /// <summary>
    /// Kaynak bulunamadýðýnda throw edilir (404)
    /// </summary>
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base(message) { }
    }
}