using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using TornBlackMarket.Common.Interfaces;

namespace TornBlackMarket.Security
{
    internal class EncryptionUtil : IEncryptionUtil
    {
        private readonly byte[] _key;

        public EncryptionUtil(IConfiguration configuration)
        {
            string privateKey = configuration["TBM_PRIVATE_KEY"] ?? throw new ArgumentException("Privacy key was not set in the configuration");
            _key = System.Text.Encoding.UTF8.GetBytes(privateKey); ;
        }

        public byte[] GenerateVector(int size)
        {
            byte[] vector = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(vector);
            }
            return vector;
        }

        public string Encrypt(string message, byte[] vector)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = _key;
            aesAlg.IV = vector;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new();
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                swEncrypt.Write(message);
            }
            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public string Decrypt(string encrypted, byte[] vector)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = vector;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msDecrypt = new(Convert.FromBase64String(encrypted));
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new(csDecrypt);
                return srDecrypt.ReadToEnd();
            }
        }

    }
}
