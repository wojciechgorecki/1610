namespace MalaUkladnica.Resources
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Klasa szyfrująca i odszyfrująca przy pomocy klucza zapisanego w klasie
    /// </summary>
    internal static class Cryptography
    {
        private const string InitVector = "tu89geji340t89u2";

        private const int Keysize = 256;
        private static readonly string Key = "1B2c3D4e5F6g7H8";

        /// <summary>
        /// Metoda szyfrujaca podany text za pomocą klucza
        /// </summary>
        /// <param name="text">Text który zostanie zaszyfrowany</param>
        /// <returns>Zwróci zaszyfrowany string</returns>
        public static string Encrypt(string text)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(text);
            PasswordDeriveBytes password = new PasswordDeriveBytes(Key, null);
            byte[] keyBytes = password.GetBytes(Keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] encrypted = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// Metoda odszyfrująca podany text za pomocą klucza
        /// </summary>
        /// <param name="encryptedText">Zaszyfrowany text który zostanie odszyfrowany</param>
        /// <returns>Zwróci odszyfrowany string</returns>
        public static string Decrypt(string encryptedText)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
            byte[] decEncryptedText = Convert.FromBase64String(encryptedText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(Key, null);
            byte[] keyBytes = password.GetBytes(Keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(decEncryptedText);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[decEncryptedText.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }
}
