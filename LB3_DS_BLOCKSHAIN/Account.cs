using System;
using System.Collections.Generic;

namespace LB3_DS_BLOCKSHAIN
{
    public class Account
    {
        private readonly string _accountID;
        private readonly List<KeyPair> _wallet;
        private int _balance;

        private Account(string accountID, KeyPair initialKeyPair, int initialBalance = 0)
        {
            _accountID = accountID;
            _wallet = new List<KeyPair> { initialKeyPair };
            _balance = initialBalance;
        }

        public static Account GenAccount()
        {
            var keyPair = KeyPair.GenKeyPair();
            var accountID = Hash.ToSHA256(keyPair.PublicKey);
            return new Account(accountID, keyPair);
        }

        public void AddKeyPairToWallet()
        {
            var newKeyPair = KeyPair.GenKeyPair();
            _wallet.Add(newKeyPair);
        }

        public void UpdateBalance(int amount)
        {
            _balance += amount;
        }

        public Operation CreateOperation(Operation.OperationType operationType, string targetID, string data, int keyIndex)
        {
            if (keyIndex < 0 || keyIndex >= _wallet.Count)
                throw new ArgumentException("Невірний індекс ключа");

            var message = Hash.ToSHA1($"{_accountID}{operationType}{targetID}{data}");
            var signature = SignData(message, keyIndex);

            return Operation.CreateOperation(this, operationType, targetID, data, signature);
        }

        public int GetBalance()
        {
            return _balance;
        }

        public void PrintBalance()
        {
            Console.WriteLine($"Баланс {_accountID.Substring(0, Math.Min(16, _accountID.Length))}...: {_balance} монет");
        }

        public byte[] SignData(string message, int keyIndex)
        {
            if (keyIndex < 0 || keyIndex >= _wallet.Count)
                throw new ArgumentException("Невірний індекс ключа");

            return Signature.SignData(message, _wallet[keyIndex].GetPrivateKey());
        }

        public string GetAccountID()
        {
            return _accountID;
        }

        public KeyPair GetKeyPair(int index)
        {
            return _wallet[index];
        }

        public int GetWalletSize()
        {
            return _wallet.Count;
        }

        public override string ToString()
        {
            return $"Account: {_accountID.Substring(0, Math.Min(20, _accountID.Length))}... Balance: {_balance}";
        }

        public void Print()
        {
            Console.WriteLine($"=== Обліковий запис ===");
            Console.WriteLine($"ID: {_accountID.Substring(0, Math.Min(20, _accountID.Length))}...");
            Console.WriteLine($"Баланс: {_balance}");
            Console.WriteLine($"Ключів у гаманці: {_wallet.Count}");
        }
    }
}