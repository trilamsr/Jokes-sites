using System;
using System.Collections.Generic;

namespace Belt_Exam.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
        public List<Like> Likes {get;set;}
        public List<Post> Posts {get;set;}
    }

    public class Like
    {
        public int LikeId { get; set; }
        public int PostId { get; set; }
        public int AccountId { get; set; }
        public Post Post { get; set; }
        public Account Account { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;

        public int AccountId {get;set;}
        public Account Creator {get;set;}
        public List<Like> LikesFroms { get; set; }
    }
}