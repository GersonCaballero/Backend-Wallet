using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiWallet.Models
{
    public class Cuenta
    {
        [Key]
        public Int32 CuentaId { get; set; }
        public Int32 UserId { get; set; }
        public Int32 TipoCuentaId { get; set; }
        [StringLength(100)]
        [Required]
        public String Nombre { get; set; }
        public Double Monto { get; set; }

    }
}