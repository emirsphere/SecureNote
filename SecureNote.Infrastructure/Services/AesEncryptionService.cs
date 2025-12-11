using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SecureNote.Application.Interfaces;

namespace SecureNote.Infrastructure.Services
{
    public class AesEncryptionService : IEncryptionService
    {
        // Bu anahtar senin kasanın anahtarıdır. 32 karakter (256-bit) olmalı.
        // Gerçek hayatta bunu "appsettings.json" veya Environment Variable'dan okuruz.
        private readonly string _key = "Ew!Sb&9Xz#2qW@5tY8rU*1oP$4mN^7cL";

        public string Encrypt(string plainText)
        {
            // AES algoritmasını oluştur
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(_key);// bu kod bloğu ile şunu sağlıyoruz: AES algoritması için 256-bit (32 byte) anahtar kullanıyoruz.
                // IV (Initialization Vector): Her şifrelemede rastgele üretilir, şifre çözülürken lazımdır.
                // Güvenlik için IV'yi şifreli metnin başına ekleyip saklayacağız.
                aesAlg.GenerateIV();

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // Önce IV'yi yaz (ilk 16 byte)
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);// neden böyle yazdık çünkü şifre çözme sırasında IV'ye ihtiyacımız olacak ve şifreli metinle birlikte saklamamız gerekiyor.

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    // Byte dizisini String'e çevir (Base64 formatında saklanır)
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            // Base64 string'i byte dizisine çevir
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(_key);

                // IV'yi (ilk 16 byte) ayıkla
                byte[] iv = new byte[16];
                Array.Copy(fullCipher, 0, iv, 0, iv.Length);
                aesAlg.IV = iv;

                // Gerçek şifreli metni (kalan kısım) ayıkla
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(fullCipher, 16, fullCipher.Length - 16))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}