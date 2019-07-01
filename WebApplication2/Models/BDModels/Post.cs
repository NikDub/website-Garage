using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string MainText { get; set; }
        public DateTime DateTime { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
