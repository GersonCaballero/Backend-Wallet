using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiWallet.Models
{
    public class Egreso
    {
        [Key]
        public Int32 EgresoId { get; set; }
        public Int32 CuentaId { get; set; }
        [StringLength(100)]
        [Required]

        public String Descripcion { get; set; }
        public Double Monto { get; set; }
        public Double MontoActual { get; set; }
        public DateTime FechaEgreso { get; set; }
    }
}