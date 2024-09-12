using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diary.Models
{
    public class DataModel
    {
        public List<RegisterUser> Register { get; set; }
        public List<User> Users { get; set; }
        public List<Directory> Directories { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Directory
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
    public class RegisterUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}