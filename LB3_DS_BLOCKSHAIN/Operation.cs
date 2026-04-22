using System;

namespace LB3_DS_BLOCKSHAIN
{
    /// <summary>
    /// Представляє операцію в мережі (лайк, коментар, поділитися)
    /// </summary>
    public class Operation
    {
        public enum OperationType
        {
            CreatePost,
            CreateComment,
            Like,
            Share,
            Follow
        }

        private readonly string _operationID;
        private readonly Account _author;
        private readonly OperationType _type;
        private readonly string _targetID;      // ID поста або користувача
        private readonly string _data;          // Контент (для постів/коментарів)
        private readonly DateTime _timestamp;
        private readonly byte[] _signature;

        private Operation(Account author, OperationType type, string targetID, string data, byte[] signature)
        {
            _author = author;
            _type = type;
            _targetID = targetID;
            _data = data ?? string.Empty;
            _timestamp = DateTime.UtcNow;
            _signature = signature;
            _operationID = Hash.ToSHA256($"{author.GetAccountID()}{type}{targetID}{_timestamp.Ticks}");
        }

        public static Operation CreateOperation(Account author, OperationType type, string targetID, string data, byte[] signature)
        {
            if (author == null)
                throw new ArgumentNullException(nameof(author));
            if (string.IsNullOrEmpty(targetID))
                throw new ArgumentException("Target ID не може бути порожнім");
            if (signature == null || signature.Length == 0)
                throw new ArgumentException("Підпис не може бути порожнім");

            return new Operation(author, type, targetID, data, signature);
        }

        public string GetOperationID() => _operationID;

        public Account GetAuthor() => _author;

        public OperationType GetOperationType() => _type;

        public string GetTargetID() => _targetID;

        public string GetData() => _data;

        public DateTime GetTimestamp() => _timestamp;

        public byte[] GetSignature() => _signature;

        public bool VerifyOperation()
        {
            var message = Hash.ToSHA1($"{_author.GetAccountID()}{_type}{_targetID}{_data}");
            var authorKeyPair = _author.GetKeyPair(0);
            return Signature.VerifySignature(message, _signature, authorKeyPair.PublicKey);
        }

        public override string ToString()
        {
            return $"Op: {_type} by {_author.GetAccountID().Substring(0, Math.Min(10, _author.GetAccountID().Length))}...";
        }

        public void Print()
        {
            Console.WriteLine($"[{_type}] {_author.GetAccountID().Substring(0, Math.Min(10, _author.GetAccountID().Length))}... → {_targetID.Substring(0, Math.Min(10, _targetID.Length))}... ({_timestamp:HH:mm:ss})");
        }
    }
}