using System;
using System.Security.Cryptography;
using System.Text;

public static class CryptoUtils
{
    private static readonly string EncryptionKey = "5f70c24a45578f8fcdf7f72fcf46e229";
    public static string EncryptString(string plainText)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
            aes.GenerateIV();
            byte[] iv = aes.IV;

            using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, iv))
            {
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                byte[] result = new byte[iv.Length + encryptedBytes.Length];
                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);

                return Convert.ToBase64String(result);
            }
        }
    }

    public static string DecryptString(string encryptedText)
    {
        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
            byte[] iv = new byte[aes.BlockSize / 8];
            byte[] cipherBytes = new byte[encryptedBytes.Length - iv.Length];

            Buffer.BlockCopy(encryptedBytes, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(encryptedBytes, iv.Length, cipherBytes, 0, cipherBytes.Length);

            using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, iv))
            {
                byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                return Encoding.UTF8.GetString(plainBytes);
            }
        }
    }
}