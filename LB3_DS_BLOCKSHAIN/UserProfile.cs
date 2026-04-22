using System;
using System.Collections.Generic;

namespace LB3_DS_BLOCKSHAIN
{
    public class UserProfile
    {
        private readonly Account _account;
        private string _username;
        private string _bio;
        private readonly List<string> _followers;
        private readonly List<string> _following;
        private readonly List<string> _posts;
        private readonly DateTime _joinDate;

        public UserProfile(Account account, string username, string bio = "")
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Імя користувача не може бути порожнім");

            _account = account;
            _username = username;
            _bio = bio ?? string.Empty;
            _followers = new List<string>();
            _following = new List<string>();
            _posts = new List<string>();
            _joinDate = DateTime.UtcNow;
        }

        public Account GetAccount() => _account;

        public string GetUsername() => _username;

        public void SetUsername(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
                _username = username;
        }

        public string GetBio() => _bio;

        public void SetBio(string bio)
        {
            _bio = bio ?? string.Empty;
        }

        public void AddPost(string postID)
        {
            if (!_posts.Contains(postID))
                _posts.Add(postID);
        }

        public List<string> GetPosts() => new List<string>(_posts);

        public void AddFollower(string userID)
        {
            if (!_followers.Contains(userID))
                _followers.Add(userID);
        }

        public void RemoveFollower(string userID)
        {
            _followers.Remove(userID);
        }

        public List<string> GetFollowers() => new List<string>(_followers);

        public int GetFollowerCount() => _followers.Count;

        public void Follow(string userID)
        {
            if (!_following.Contains(userID))
                _following.Add(userID);
        }

        public void Unfollow(string userID)
        {
            _following.Remove(userID);
        }

        public List<string> GetFollowing() => new List<string>(_following);

        public int GetFollowingCount() => _following.Count;

        public DateTime GetJoinDate() => _joinDate;

        public override string ToString()
        {
            return $"@{_username} ({_account.GetAccountID().Substring(0, Math.Min(10, _account.GetAccountID().Length))}...) " +
                   $"Posts: {_posts.Count} | Followers: {_followers.Count}";
        }

        public void PrintProfile()
        {
            string bioPreview = _bio.Substring(0, Math.Min(30, _bio.Length));
            string bioEllipsis = _bio.Length > 30 ? "…" : "";

            Console.WriteLine($"\n@{_username}");
            Console.WriteLine($"ID: {_account.GetAccountID().Substring(0, Math.Min(12, _account.GetAccountID().Length))}...");
            Console.WriteLine($"Bio: {bioPreview}{bioEllipsis}");
            Console.WriteLine($"Статистика:");
            Console.WriteLine($"Постів: {_posts.Count} | Підписників: {_followers.Count} | Підписаний: {_following.Count}");
            Console.WriteLine($"Приєднався: {_joinDate:yyyy-MM-dd HH:mm:ss}\n");
        }
    }
}