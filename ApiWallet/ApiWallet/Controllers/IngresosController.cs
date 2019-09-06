using ApiWallet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiWallet.Controllers
{
    [Authorize]
    [EnableCors("*", "*", "*")]
    public class IngresosController : ApiController
    {
        private ApiWalletContext db = new ApiWalletContext();

        [HttpGet]
        [Route("api/Ingresos")]
        public IHttpActionResult IngresosId([FromUri]int accountId)
        {
            var Account = db.Cuentas.Find(accountId);

            if (Account == null)
            {
                return Ok("Esta cuenta no existe.");
            }

            var query = db.Ingresos
                .Include("Cuenta")
                .Where(w => w.CuentaId == Account.CuentaId)
                .Select(s => new {
                    Cuenta = Account.Nombre,
                    Descripcion = s.Descripcion,
                    Monto = s.Monto,
                    MontoActual = s.MontoActual,
                    Fecha = s.FechaIngreso
                });

            return Ok(query);
        }

        [HttpGet]
        [Route("api/Ingresos")]
        public IHttpActionResult IngresoFecha([FromUri] int id, [FromUri] DateTime fi, [FromUri] DateTime ff)
        {
            var Account = db.Cuentas.Find(id);

            if (Account == null)
            {
                return Ok("Esta cuenta no existe.");
            }

            var query = db.Ingresos
                .Include("Cuenta")
                .Where(w => w.CuentaId == Account.CuentaId && w.FechaIngreso >= fi && w.FechaIngreso<= ff)
                .Select(s => new {
                    Cuenta = Account.Nombre,
                    Descripcion = s.Descripcion,
                    Monto = s.Monto,
                    MontoActual = s.MontoActual,
                    Fecha = s.FechaIngreso
                });

            return Ok(query);
        }

        [HttpPost]
        [Route("api/Ingresos")]
        public IHttpActionResult Ingreso([FromUri]int accountId, Ingreso ingreso)
        {
            var Account = db.Cuentas.Find(accountId);

            if(Account == null)
            {
                return Ok("Esta cuenta no existe.");
            }
            if (Account.TipoCuentaId == 1)
            {
                Account.Monto = Account.Monto + ingreso.Monto;
            }
            else
            {
                Account.Monto = Account.Monto - ingreso.Monto;
            }

            var montostring = ingreso.Monto.ToString();

            if (ingreso.Descripcion == null || montostring == "0")
            {
                return Ok(new { message = "Todos los campos tienen que tener un valor." });
            }

            db.Entry(Account).State = EntityState.Modified;
            //db.SaveChanges();

            db.Ingresos.Add(ingreso);
            db.SaveChanges();

            return Ok(new {message = "El ingreso se realizo con exito." });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
