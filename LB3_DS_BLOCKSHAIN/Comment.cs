using System;

namespace LB3_DS_BLOCKSHAIN
{
    public class Comment
    {
        private readonly string _commentID;
        private readonly Account _author;
        private readonly string _postID;
        private readonly string _text;
        private readonly DateTime _timestamp;
        private int _likes;

        public Comment(Account author, string postID, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Текст коментара не може бути порожним");

            _author = author;
            _postID = postID;
            _text = text;
            _timestamp = DateTime.UtcNow;
            _likes = 0;
            _commentID = Hash.ToSHA256($"{author.GetAccountID()}{postID}{text}{_timestamp.Ticks}");
        }

        public string GetCommentID() => _commentID;

        public Account GetAuthor() => _author;

        public string GetPostID() => _postID;

        public string GetText() => _text;

        public DateTime GetTimestamp() => _timestamp;

        public int GetLikes() => _likes;

        public void AddLike()
        {
            _likes++;
        }

        public void RemoveLike()
        {
            if (_likes > 0)
                _likes--;
        }

        public override string ToString()
        {
            return $"Comment: {_commentID.Substring(0, Math.Min(12, _commentID.Length))}... " +
                   $"by {_author.GetAccountID().Substring(0, Math.Min(10, _author.GetAccountID().Length))}...";
        }

        public void Print()
        {
            string ellipsis = _text.Length > 50 ? "…" : "";
            Console.WriteLine($"  └─ {_author.GetAccountID()
                .Substring(0, Math.Min(12, _author.GetAccountID().Length))}...: {_text.Substring(0, Math.Min(50, _text.Length))}{ellipsis} ({_likes} лайків)");

        }
    }
}