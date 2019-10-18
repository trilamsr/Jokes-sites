using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Belt_Exam.Models;

namespace Belt_Exam.ViewModels 
{
    public class DashboardViewModel
    {
        public Account LoggedAccount { get; set; }
        public string Greeting {get;set;}
        public List<Post> AllPost {get;set;}
        public NewPostForm NewPost {get;set;}

    }

    public class PostViewModel
    {
        public Post SaidPost { get; set; }
    }

    public class AccountViewModel
    {
        public Account SaidAccount { get; set; }
        public int AllLikes {get;set;}
    }


    public class NewPostForm
    {
        [MinLength(5, ErrorMessage="Joke must be funnier than 5 Codies")]
        [Display(Name="Make some jokes?")]
        public string Content {get;set;}
    }


    
}