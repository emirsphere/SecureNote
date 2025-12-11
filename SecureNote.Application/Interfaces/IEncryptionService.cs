namespace SecureNote.Application.Interfaces
{
    public interface IEncryptionService
    {
        // Metni şifreler (Encrypt)
        string Encrypt(string plainText);
        // Şifreli metni çözer (Decrypt)
        string Decrypt(string cipherText);
    }
}