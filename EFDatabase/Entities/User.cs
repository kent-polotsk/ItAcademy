using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDatabase.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? BanDate { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBanned { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public List<Comment?> Comments { get; set; }
    }
}
