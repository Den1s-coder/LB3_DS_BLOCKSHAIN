using System;
using System.Collections.Generic;
using System.Linq;

namespace LB3_DS_BLOCKSHAIN
{
    public class Blockchain
    {
        private readonly List<Block> _blockHistory;
        private readonly HashSet<string> _txDatabase;
        private readonly Dictionary<string, UserProfile> _userProfiles;
        private readonly Dictionary<string, Post> _posts;
        private readonly Dictionary<string, List<Comment>> _postComments;

        public Blockchain()
        {
            _blockHistory = new List<Block>();
            _txDatabase = new HashSet<string>();
            _userProfiles = new Dictionary<string, UserProfile>();
            _posts = new Dictionary<string, Post>();
            _postComments = new Dictionary<string, List<Comment>>();
            InitBlockchain();
        }

        private void InitBlockchain()
        {
            var genesisBlock = Block.CreateGenesisBlock();
            _blockHistory.Add(genesisBlock);
            Console.WriteLine($"Блок створено: {genesisBlock.GetBlockID().Substring(0, 16)}...");
        }

        public void RegisterUser(Account account, string username, string bio = "")
        {
            var accountID = account.GetAccountID();
            if (!_userProfiles.ContainsKey(accountID))
            {
                var profile = new UserProfile(account, username, bio);
                _userProfiles[accountID] = profile;
                Console.WriteLine($"Користувач @{username} зареєстрований");
            }
        }

        public UserProfile GetUserProfile(string accountID)
        {
            return _userProfiles.ContainsKey(accountID) ? _userProfiles[accountID] : null;
        }

        public void CreatePost(Post post)
        {
            _posts[post.GetPostID()] = post;
            _postComments[post.GetPostID()] = new List<Comment>();
            
            var authorID = post.GetAuthor().GetAccountID();
            if (_userProfiles.ContainsKey(authorID))
            {
                _userProfiles[authorID].AddPost(post.GetPostID());
            }

            Console.WriteLine($"Пост створено: {post.GetPostID().Substring(0, 12)}...");
        }

        public Post GetPost(string postID)
        {
            return _posts.ContainsKey(postID) ? _posts[postID] : null;
        }

        public void AddComment(Comment comment)
        {
            if (_postComments.ContainsKey(comment.GetPostID()))
            {
                _postComments[comment.GetPostID()].Add(comment);
                Console.WriteLine($"Коментар додано до поста");
            }
        }

        public List<Comment> GetPostComments(string postID)
        {
            return _postComments.ContainsKey(postID) ? new List<Comment>(_postComments[postID]) : new List<Comment>();
        }

        public void LikePost(string postID)
        {
            if (_posts.ContainsKey(postID))
            {
                _posts[postID].AddLike();
            }
        }

        public void Follow(string followerID, string followingID)
        {
            if (_userProfiles.ContainsKey(followerID) && _userProfiles.ContainsKey(followingID))
            {
                _userProfiles[followerID].Follow(followingID);
                _userProfiles[followingID].AddFollower(followerID);
                Console.WriteLine($"Користувач підписався");
            }
        }

        public bool ValidateBlock(Block block)
        {
            if (block.GetPrevHash() != GetLastBlockID())
            {
                Console.WriteLine("Помилка: блок не посилається на останній блок");
                return false;
            }

            foreach (var transaction in block.GetTransactions())
            {
                if (_txDatabase.Contains(transaction.GetTransactionID()))
                {
                    Console.WriteLine("Помилка: транзакція вже існує");
                    return false;
                }

                foreach (var operation in transaction.GetOperations())
                {
                    if (!operation.VerifyOperation())
                    {
                        Console.WriteLine("Помилка: невірний підпис операції");
                        return false;
                    }
                }

                _txDatabase.Add(transaction.GetTransactionID());
            }

            _blockHistory.Add(block);
            return true;
        }

        public string GetLastBlockID()
        {
            return _blockHistory[_blockHistory.Count - 1].GetBlockID();
        }

        public List<Block> GetBlockHistory()
        {
            return new List<Block>(_blockHistory);
        }

        public Dictionary<string, UserProfile> GetAllProfiles()
        {
            return new Dictionary<string, UserProfile>(_userProfiles);
        }

        public int GetUserCount() => _userProfiles.Count;

        public int GetPostCount() => _posts.Count;

        public int GetBlockCount() => _blockHistory.Count;

        public override string ToString()
        {
            return $"Blockchain: Блоків: {_blockHistory.Count}, Користувачів: {_userProfiles.Count}, Постів: {_posts.Count}";
        }
    }
}