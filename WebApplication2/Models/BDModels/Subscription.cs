using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int User_Sub { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
