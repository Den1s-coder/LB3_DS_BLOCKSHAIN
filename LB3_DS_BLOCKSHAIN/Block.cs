using System;
using System.Collections.Generic;
using System.Linq;

namespace LB3_DS_BLOCKSHAIN
{
    public class Block
    {
        private readonly string _blockID;
        private readonly string _prevHash;
        private readonly List<Transaction> _transactions;
        private readonly DateTime _timestamp;
        private readonly bool _isGenesis;

        private Block(List<Transaction> transactions, string prevHash, bool isGenesis = false)
        {
            _transactions = transactions ?? new List<Transaction>();
            _prevHash = prevHash;
            _timestamp = DateTime.UtcNow;
            _isGenesis = isGenesis;
            _blockID = CalculateBlockID();
        }

        public static Block CreateBlock(List<Transaction> transactions, string prevHash)
        {
            if (transactions == null || transactions.Count == 0)
                throw new ArgumentException("Блок повинен містити транзакції");

            return new Block(transactions, prevHash, false);
        }

        public static Block CreateGenesisBlock()
        {
            return new Block(new List<Transaction>(), "0", true);
        }

        private string CalculateBlockID()
        {
            var txData = string.Concat(_transactions.Select(tx => tx.GetTransactionID()));
            return Hash.ToSHA256($"{txData}{_prevHash}{_timestamp.Ticks}");
        }

        public string GetBlockID() => _blockID;

        public string GetPrevHash() => _prevHash;

        public List<Transaction> GetTransactions() => new List<Transaction>(_transactions);

        public DateTime GetTimestamp() => _timestamp;

        public bool IsGenesis() => _isGenesis;

        public int GetTransactionCount() => _transactions.Count;

        public override string ToString()
        {
            return $"Block: {_blockID.Substring(0, Math.Min(16, _blockID.Length))}... Txs: {_transactions.Count}";
        }

        public void Print()
        {
            Console.WriteLine($"\nБлок: {_blockID.Substring(0, Math.Min(14, _blockID.Length))}...");
            Console.WriteLine($"Попередній: {_prevHash.Substring(0, Math.Min(12, _prevHash.Length))}...");
            Console.WriteLine($"Час: {_timestamp:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Транзакцій: {_transactions.Count}\n");
        }
    }
}