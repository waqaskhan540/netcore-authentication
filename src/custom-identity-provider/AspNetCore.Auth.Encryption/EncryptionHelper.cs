using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AspNetCore.Auth.Encryption
{
    public static class EncryptionHelper
    {
        public static string Encrypt(string payload, string key)
        {
            byte[] keyBytes = GetKeyBytes(key);
            byte[] encrypted;
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider { Key = keyBytes })
            {
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                            swEncrypt.Write(payload);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }
        public static string Decrypt(string encryptedData, string key)
        {
            byte[] keyBytes = GetKeyBytes(key);
            string plaintext = null;
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider { Key = keyBytes })
            {
                var cipherBytes = Convert.FromBase64String(encryptedData);
                var iv = new byte[aesAlg.IV.Length];
                Array.Copy(cipherBytes, iv, iv.Length);
                aesAlg.IV = iv;

                var text = new byte[cipherBytes.Length - iv.Length];
                Array.Copy(cipherBytes, iv.Length, text, 0, text.Length);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(text))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    plaintext = srDecrypt.ReadToEnd();
                }
            }

            return plaintext;
        }

        private static byte[] GetKeyBytes(string key)
        {
            if (key.Length > 32)
            {
                throw new ArgumentException("Key is too long. 32 character max...", "key");
            }
            return Encoding.UTF8.GetBytes(key.PadLeft(32));
        }
    }
}
