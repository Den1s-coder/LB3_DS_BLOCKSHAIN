using System;

namespace LB3_DS_BLOCKSHAIN
{
    public class KeyPair
    {
        private readonly string _privateKey;
        public string PublicKey { get; private set; }

        private KeyPair(string privateKey, string publicKey)
        {
            _privateKey = privateKey;
            PublicKey = publicKey;
        }

        public static KeyPair GenKeyPair()
        {
            using (var rsa = new System.Security.Cryptography.RSACryptoServiceProvider(2048))
            {
                var privateKey = rsa.ToXmlString(true);
                var publicKey = rsa.ToXmlString(false);
                return new KeyPair(privateKey, publicKey);
            }
        }

        public string GetPrivateKey()
        {
            return _privateKey;
        }

        public override string ToString()
        {
            return $"PublicKey: {PublicKey.Substring(0, Math.Min(50, PublicKey.Length))}...";
        }

        public void PrintKeyPair()
        {
            Console.WriteLine($"Публічний ключ: {PublicKey.Substring(0, Math.Min(80, PublicKey.Length))}...");
        }
    }
}