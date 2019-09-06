using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using ApiWallet.Models;

namespace ApiWallet.Controllers
{
    [Authorize]
    [EnableCors("*", "*", "*")]
    public class CuentasController : ApiController
    {
        private ApiWalletContext db = new ApiWalletContext();

        [HttpGet]
        [Route("api/Cuentas")]
        public IQueryable<Cuenta> GetCuentas()
        {
            return db.Cuentas;
        }

        [HttpGet]
        [Route("api/Cuentas")]
        [ResponseType(typeof(Cuenta))]
        public IHttpActionResult GetCuentas([FromUri]int Userid)
        {
            User user = db.Users.Find(Userid);

            if (user == null)
            {
                return Ok(new { message = "El usuario no existe" });
            }

            var query = db.Cuentas
                .Where(w => w.UserId == user.UserId);

            return Ok(query);
        }

        [HttpGet]
        [Route("api/Cuenta")]
        [ResponseType(typeof(Cuenta))]
        public IHttpActionResult GetCuenta([FromUri]int AccountId)
        {
            Cuenta Cuenta = db.Cuentas.Find(AccountId);

            if (Cuenta == null)
            {
                return Ok(new { message = "Esta cuenta no existe" });
            }

            return Ok(Cuenta);
        }

        [HttpPut]
        [Route("api/Cuentas")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCuenta([FromUri]int Cuentaid, Cuenta cuenta)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { message = "Algo salio mal." }); 
            }

            if (Cuentaid != cuenta.CuentaId)
            {
                return Ok(new { message = "Cuenta no existe." });
            }

            db.Entry(cuenta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuentaExists(Cuentaid))
                {
                    return Ok(new { message = "Esta cuenta que buscas no existe." });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Cuenta editada correctamente." });
        }

        [HttpPost]
        [Route("api/Cuentas")]
        [ResponseType(typeof(Cuenta))]
        public IHttpActionResult PostCuenta(Cuenta cuenta)
        {
            if (cuenta.Nombre == null)
            {
                return Ok(new { message = "Pro favor ingrese un nombre a su cuenta." });
            }

            db.Cuentas.Add(cuenta);
            db.SaveChanges();

            return Ok(new { message = "Cuenta creada exitosamente." });
        }

        [HttpDelete]
        [Route("api/Cuentas")]
        [ResponseType(typeof(Cuenta))]
        public IHttpActionResult DeleteCuenta([FromUri]int accountId)
        {
            Cuenta cuenta = db.Cuentas.Find(accountId);
            if (cuenta == null)
            {
                return Ok(new { message = "Esta cuenta que buscas no existe." });
            }

            db.Cuentas.Remove(cuenta);
            db.SaveChanges();

            return Ok(new { message = "Se elimino con exito la cuenta." });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CuentaExists(int id)
        {
            return db.Cuentas.Count(e => e.CuentaId == id) > 0;
        }
    }
}