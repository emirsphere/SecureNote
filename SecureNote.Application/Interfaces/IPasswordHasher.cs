namespace SecureNote.Application.Interfaces
{
    public interface IPasswordHasher
    {
        // Şifreyi alır, tuzlar ve karmaşık bir string döndürür.
        string Hash(string password);

        // Kullanıcının girdiği şifre (password) ile veritabanındaki hash'i (passwordHash) kıyaslar.
        // Geriye Hash'i çözüp şifreyi VERMEZ (bu imkansızdır), sadece eşleşip eşleşmediğini (true/false) döner.
        bool Verify(string password, string passwordHash);
    }
}