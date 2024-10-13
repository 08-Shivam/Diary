using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diary.Models
{
    public class DataModel
    {
        public List<RegisterUser> Register { get; set; } = new List<RegisterUser>();
        public List<Directory> Directories { get; set; } = new List<Directory>();
    }
    
    public class RegisterUser
    {
        public string Role {  get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class Directory
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
      
    }
}