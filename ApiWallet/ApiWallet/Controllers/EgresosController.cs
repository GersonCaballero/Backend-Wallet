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
    public class EgresosController : ApiController
    {
        private ApiWalletContext db = new ApiWalletContext();

        [HttpGet]
        [Route("api/Egresos")]
        public IHttpActionResult EgresosId([FromUri]int accountId)
        {
            var Account = db.Cuentas.Find(accountId);

            if (Account == null)
            {
                return Ok("Esta cuenta no existe.");
            }

            var query = db.Egresos
                .Include("Cuenta")
                .Where(w => w.CuentaId == Account.CuentaId)
                .Select(s => new {
                    Cuenta = Account.Nombre,
                    Descripcion = s.Descripcion,
                    Monto = s.Monto,
                    MontoActual = s.MontoActual,
                    Fecha = s.FechaEgreso
                });

            return Ok(query);
        }

        [HttpGet]
        [Route("api/Egresos")]
        public IHttpActionResult EgresoFecha([FromUri]int id, [FromUri] DateTime fi, [FromUri] DateTime ff)
        {
            var Account = db.Cuentas.Find(id);

            if (Account == null)
            {
                return Ok("Esta cuenta no existe.");
            }

            var query = db.Egresos
                .Include("Cuenta")
                .Where(w => w.CuentaId == Account.CuentaId && w.FechaEgreso >= fi && w.FechaEgreso <= ff)
                .Select(s => new {
                    Cuenta = Account.Nombre,
                    Descripcion = s.Descripcion,
                    Monto = s.Monto,
                    MontoActual = s.MontoActual,
                    Fecha = s.FechaEgreso
                });

            return Ok(query);
        }

        [HttpPost]
        [Route("api/Egresos")]
        public IHttpActionResult Egreso([FromUri]int accountId, Egreso Egreso)
        {
            var Account = db.Cuentas.Find(accountId);

            if (Account == null)
            {
                return Ok("Esta cuenta no existe.");
            }

            var montostring = Egreso.Monto.ToString();

            if (Egreso.Descripcion == null || montostring == "0")
            {
                return Ok(new { message = "Todos los campos tienen que tener un valor." });
            }

            if (Account.Monto < Egreso.Monto && Account.TipoCuentaId == 1)
            {
                return Ok("No hay suficiente dinero en la cuenta para hacer el Egreso.");
            }

            if (Account.TipoCuentaId == 2 && Account.Monto >= 0)
            {
                return Ok("No hay saldos pendientes en esta cuenta.");
            }

            if(Account.TipoCuentaId == 1)
            {
                Account.Monto = Account.Monto - Egreso.Monto;
            }
            else
            {
                Account.Monto = Account.Monto + Egreso.Monto;
            }

            db.Entry(Account).State = EntityState.Modified;

            db.Egresos.Add(Egreso);
            db.SaveChanges();

            return Ok(new { message = "El Egreso se realizo con exito." });
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
