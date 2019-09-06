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
    public class TrasladosController : ApiController
    {
        private ApiWalletContext db = new ApiWalletContext();
        [HttpGet]
        [Route("api/Traslados")]
        public IHttpActionResult Traslados([FromUri] int accountId)
        {
            var account = db.Cuentas.Find(accountId);
            var query = db.Traslados
                .Include("Cuenta")
                .Where(w => w.CuentaId == account.CuentaId)
                .Select(s =>new
                {
                    Cuenta = account.Nombre,
                    Descripcion = s.Descripcion,
                    Monto = s.Monto,
                    MontoActual = account.Monto,
                    Fecha = s.FechaTraslado
                });

            return Ok(query);
        }

        [HttpPost]
        [Route("api/Traslados")]
        public IHttpActionResult Traslado([FromUri]int idE, [FromUri]int IdR, Traslado traslado)
        {
            var AccountE = db.Cuentas.Find(idE);

            var AccountR = db.Cuentas.Find(IdR);

            if (AccountE == null || AccountR == null)
            {
                return Ok("Esta cuenta no existe.");
            }

            var montostring = traslado.Monto.ToString();

            if (traslado.Descripcion == null || montostring == "0")
            {
                return Ok(new { message = "Todos los campos tienen que tener un valor." });
            }


            if (AccountE.Monto < traslado.Monto)
            {
                return Ok("No tiene la suficiente cantidad de dinero en esta cuenta para hacer la transacción.");
            }

            if (AccountE.TipoCuentaId == 2)
            {
                return Ok(new { message = "No se puede enviar dinero a otras cuentas desde esta cuenta ya que es de credito."});
            }

            if(AccountE.CuentaId == AccountR.CuentaId)
            {
                return Ok(new { message = "No se pueden hacer traslados de dinero si es la misma cuenta la que envia y recibe." });
            }

            AccountE.Monto = AccountE.Monto - traslado.Monto;

            Ingreso ingreso = new Ingreso();

            Egreso egreso = new Egreso();

            if (AccountR.TipoCuentaId == 1)
            {
                AccountR.Monto = AccountR.Monto + traslado.Monto;
                
                ingreso.CuentaId = AccountR.CuentaId;
                ingreso.Descripcion = "Traslado de cuenta " + AccountE.Nombre + " a cuenta " + AccountR.Nombre;
                ingreso.FechaIngreso = traslado.FechaTraslado;
                ingreso.Monto = traslado.Monto;
                ingreso.MontoActual = AccountR.Monto - traslado.Monto;

                
                egreso.CuentaId = AccountE.CuentaId;
                egreso.Descripcion = "Traslado de cuenta " + AccountE.Nombre + " a cuenta " + AccountR.Nombre;
                egreso.FechaEgreso = traslado.FechaTraslado;
                egreso.Monto = traslado.Monto;
                egreso.MontoActual = AccountE.Monto + traslado.Monto;
            }
            else
            {
                if (AccountR.Monto == 0)
                {
                    return Ok(new{ message = "No tienes saldos pendientes en esta cuenta." });
                }

                AccountR.Monto = AccountR.Monto + traslado.Monto;

                if (AccountR.Monto > 0)
                {
                    return Ok(new { message = "El monto ingresado es mayor a la deuda actual."});
                }

                ingreso.CuentaId = AccountR.CuentaId;
                ingreso.Descripcion = traslado.Descripcion;
                ingreso.FechaIngreso = traslado.FechaTraslado;
                ingreso.Monto = traslado.Monto;
                ingreso.MontoActual = AccountR.Monto - traslado.Monto;

                egreso.CuentaId = AccountE.CuentaId;
                egreso.Descripcion = traslado.Descripcion;
                egreso.FechaEgreso = traslado.FechaTraslado;
                egreso.Monto = traslado.Monto;
                egreso.MontoActual = AccountE.Monto + traslado.Monto;
            }
            
            

            db.Entry(AccountE).State = EntityState.Modified;
            db.SaveChanges();

            db.Entry(AccountR).State = EntityState.Modified;
            db.SaveChanges();

            db.Ingresos.Add(ingreso);
            db.SaveChanges();

            db.Egresos.Add(egreso);
            db.SaveChanges();

            db.Traslados.Add(traslado);
            db.SaveChanges();

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return Ok(e);
            }
            return Ok(new { message = "El Traslado se realizo con exito." });
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
