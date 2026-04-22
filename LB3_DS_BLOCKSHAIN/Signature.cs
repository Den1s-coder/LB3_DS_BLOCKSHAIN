using System;
using System.Security.Cryptography;
using System.Text;

namespace LB3_DS_BLOCKSHAIN
{
    public class Signature
    {
        public static byte[] SignData(string message, string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                return rsa.SignData(Encoding.UTF8.GetBytes(message), SHA1.Create());
            }
        }

        public static bool VerifySignature(string message, byte[] signature, string publicKey)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);
                    return rsa.VerifyData(Encoding.UTF8.GetBytes(message), SHA1.Create(), signature);
                }
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return "Signature object";
        }

        public void PrintSignature()
        {
            Console.WriteLine("Signature: [Цифровий підпис]");
        }
    }
}