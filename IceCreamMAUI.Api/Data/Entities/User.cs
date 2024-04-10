﻿using System.ComponentModel.DataAnnotations;

namespace IceCreamMAUI.Api.Data.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(30)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(150)]
        public string Address { get; set; }

        [Required, MaxLength(180)]
        public string PasswordHash { get; set; }

        [Required, MaxLength(20)]
        public string Salt { get; set; }
    }
}
