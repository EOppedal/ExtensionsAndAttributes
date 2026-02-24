using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Encryption {
    public static class Encryption {
        private const string EncryptionKey = "LU3xcupSktVEVQMX";
        private const string EncryptionIv = "CsmcuhAj6YsLahLa";

        private static string Encrypt(string plainText) {
            var keyBytes = Encoding.UTF8.GetBytes(EncryptionKey);
            var ivBytes = Encoding.UTF8.GetBytes(EncryptionIv);

            using var aesAlg = Aes.Create();
            aesAlg.Key = keyBytes;
            aesAlg.IV = ivBytes;
            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt)) {
                swEncrypt.Write(plainText);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public static string Encrypted(this string plainText) {
            return Encrypt(plainText);
        }

        private static string Decrypt(string cipherText) {
            var keyBytes = Encoding.UTF8.GetBytes(EncryptionKey);
            var ivBytes = Encoding.UTF8.GetBytes(EncryptionIv);
            var cipherTextBytes = Convert.FromBase64String(cipherText);

            using var aesAlg = Aes.Create();
            aesAlg.Key = keyBytes;
            aesAlg.IV = ivBytes;
            var cryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using var msDecrypt = new MemoryStream(cipherTextBytes);
            using var csDecrypt = new CryptoStream(msDecrypt, cryptoTransform, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }

        public static string Decrypted(this string cipherText) {
            return Decrypt(cipherText);
        }
    }
}