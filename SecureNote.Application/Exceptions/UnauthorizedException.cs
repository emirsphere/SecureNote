namespace SecureNote.Application.Exceptions
{
    /// <summary>
    /// Kullanýcý yetkisiz iþlem yaptýðýnda throw edilir (403)
    /// </summary>
    public class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException(string message) : base(message) { }
    }
}