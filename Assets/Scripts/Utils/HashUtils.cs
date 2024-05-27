using System;
using System.Security.Cryptography;
using System.Text;

public static class HashUtils
{
    private static readonly string HashKey = "5f70c24a45578f8fcdf7f72fcf46e229";  

    public static string ComputeHash(string input)
    {
        using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(HashKey)))
        {
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hashBytes);
        }
    }

    public static bool VerifyHash(string input, string hash)
    {
        string computedHash = ComputeHash(input);
        return computedHash == hash;
    }
}