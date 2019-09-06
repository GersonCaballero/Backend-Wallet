using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiWallet.Models
{
    public class LoginReturn
    {
        public int IdUser { get; set; }
        public string Token { get; set; }
    }
}