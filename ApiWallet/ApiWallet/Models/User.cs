using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiWallet.Models
{
    public class User
    {
        [Key]
        public Int32 UserId { get; set; }
        [StringLength(100)]
        [Required]
        public String Name { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }

    }
}