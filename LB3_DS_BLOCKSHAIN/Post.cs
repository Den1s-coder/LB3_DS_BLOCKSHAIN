using System;

namespace LB3_DS_BLOCKSHAIN
{
    public class Post
    {
        private readonly string _postID;
        private readonly Account _author;
        private readonly string _content;
        private readonly DateTime _timestamp;
        private int _likes;
        private readonly string _contentHash;

        public Post(Account author, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Контент поста не може бути порожним");

            _author = author;
            _content = content;
            _timestamp = DateTime.UtcNow;
            _likes = 0;
            _contentHash = Hash.ToSHA256(content);
            _postID = Hash.ToSHA256($"{author.GetAccountID()}{_contentHash}{_timestamp.Ticks}");
        }

        public string GetPostID() => _postID;

        public Account GetAuthor() => _author;

        public string GetContent() => _content;

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

        public string GetContentHash() => _contentHash;

        public override string ToString()
        {
            return $"Post: {_postID.Substring(0, Math.Min(16, _postID.Length))}... " +
                   $"Author: {_author.GetAccountID().Substring(0, Math.Min(10, _author.GetAccountID().Length))}... " +
                   $"Likes: {_likes}";
        }

        public void Print()
        {
            string contentPreview = _content.Substring(0, Math.Min(40, _content.Length));
            string ellipsis = _content.Length > 40 ? "…" : "";

            Console.WriteLine($"Пост: {_postID.Substring(0, Math.Min(16, _postID.Length))}...");
            Console.WriteLine($"Автор: {_author.GetAccountID().Substring(0, Math.Min(16, _author.GetAccountID().Length))}...");
            Console.WriteLine($"Час: {_timestamp:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Контент: {contentPreview}{ellipsis}");
            Console.WriteLine($"Лайків: {_likes}\n");
        }
    }
}