using ApiWallet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiWallet.Controllers
{
    [Authorize]
    [EnableCors("*", "*", "*")]
    public class TipoCuentasController : ApiController
    {
        private ApiWalletContext db = new ApiWalletContext();

        [HttpGet]
        [Route("api/tipocuenta")]
        public IQueryable<tipocuenta> TipoCuenta()
        {
            return db.TiposCuentas;
        }

        [HttpPost]
        [Route("api/tipocuenta")]
        public IHttpActionResult TipoCuenta(tipocuenta tipocuenta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            db.TiposCuentas.Add(tipocuenta);
            db.SaveChanges();
            return Ok();
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
