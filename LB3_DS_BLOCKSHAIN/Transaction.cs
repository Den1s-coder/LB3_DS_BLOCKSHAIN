using System;
using System.Collections.Generic;
using System.Linq;

namespace LB3_DS_BLOCKSHAIN
{
    public class Transaction
    {
        private readonly string _transactionID;
        private readonly List<Operation> _operations;
        private readonly int _nonce;
        private readonly DateTime _timestamp;

        public Transaction(List<Operation> operations, int nonce)
        {
            if (operations == null || operations.Count == 0)
                throw new ArgumentException("Транзакція повинна містити операції");

            _operations = new List<Operation>(operations);
            _nonce = nonce;
            _timestamp = DateTime.UtcNow;
            _transactionID = CalculateTransactionID();
        }

        private string CalculateTransactionID()
        {
            var operationData = string.Concat(_operations.Select(op => op.GetOperationID()));
            return Hash.ToSHA256($"{operationData}{_nonce}{_timestamp.Ticks}");
        }

        public string GetTransactionID() => _transactionID;

        public List<Operation> GetOperations() => new List<Operation>(_operations);

        public int GetNonce() => _nonce;

        public DateTime GetTimestamp() => _timestamp;

        public override string ToString()
        {
            return $"Tx: {_transactionID.Substring(0, Math.Min(16, _transactionID.Length))}... Ops: {_operations.Count}";
        }

        public void Print()
        {
            Console.WriteLine($"\nТранзакція: {_transactionID.Substring(0, Math.Min(16, _transactionID.Length))}...");
            Console.WriteLine($"Операцій: {_operations.Count} | Nonce: {_nonce}");
            foreach (var op in _operations)
            {
                Console.WriteLine($"{op}");
            }
            Console.WriteLine($"");
        }
    }
}