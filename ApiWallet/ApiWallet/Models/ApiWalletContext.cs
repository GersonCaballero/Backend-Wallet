using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ApiWallet.Models
{
    public class ApiWalletContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ApiWalletContext() : base("name=ApiWalletContext")
        {
        }

        public System.Data.Entity.DbSet<ApiWallet.Models.User> Users { get; set; }
        public System.Data.Entity.DbSet<ApiWallet.Models.Cuenta> Cuentas { get; set; }
        public System.Data.Entity.DbSet<ApiWallet.Models.Ingreso> Ingresos { get; set; }
        public System.Data.Entity.DbSet<ApiWallet.Models.Egreso> Egresos { get; set; }
        public System.Data.Entity.DbSet<ApiWallet.Models.Traslado> Traslados { get; set; }
        public System.Data.Entity.DbSet<ApiWallet.Models.tipocuenta> TiposCuentas { get; set; }
    }
}
