﻿using EFDatabase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConvert.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        //public string PasswordHash { get; set; }
        //public string PasswordSalt { get; set; }
        //public bool IsVerified { get; set; } = false;

        public double PositivityRate { get; set; } = 0;

        public DateTime CreatedDate { get; set; }
        public DateTime? BanToDate { get; set; } //ban to
        //public bool? IsAdmin { get; set; }
        public bool IsBanned { get; set; }
        public bool IsSubscribed { get; set; }

        public Guid RoleId { get; set; }
        //public Role Role { get; set; }

        //public List<Comment?> Comments { get; set; }
    }
}
