using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<Post> Posts { get; set; }
        public User()
        {
            Subscriptions = new List<Subscription>();
            Posts = new List<Post>();
        }
    }
}
