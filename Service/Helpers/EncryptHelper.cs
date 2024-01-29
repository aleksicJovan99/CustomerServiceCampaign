namespace Service;

// This class provides helper methods for "fake encrypting and decrypting" data using Base64 encoding. 
public static class EncryptHelper
{
    public static string EncryptDataBase64(string text) 
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(text);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    public static string DecryptDataBase64(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}
