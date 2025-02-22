﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiWallet.Models
{
    public class Traslado
    {
        [Key]
        public Int32 TrasladoId { get; set; }
        public Int32 CuentaId { get; set; }
        [StringLength(100)]
        [Required]

        public String Descripcion { get; set; }
        public Double Monto { get; set; }
        public Double MontoActual { get; set; }
        public DateTime FechaTraslado { get; set; }
    }
}