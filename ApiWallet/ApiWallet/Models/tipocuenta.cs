using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiWallet.Models
{
    public class tipocuenta
    {
        [Key]
        public Int32 TipoCuentaId { get; set; }
        [Required]
        [StringLength(100)]
        public String Nombre { get; set; }
    }
}