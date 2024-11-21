namespace TornBlackMarket.Common.Interfaces
{
    public interface IEncryptionUtil
    {
        string Decrypt(string encrypted, byte[] vector);
        string Encrypt(string message, byte[] vector);
        byte[] GenerateVector(int size);
    }
}
