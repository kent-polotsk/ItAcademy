using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDatabase.Entities
{
    internal class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? BanDate { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBanned { get; set; }


        public List<Comment?> Comments { get; set; }
    }
}
