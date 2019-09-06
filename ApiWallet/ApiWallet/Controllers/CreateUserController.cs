using ApiWallet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace ApiWallet.Controllers
{
    [EnableCors("*","*","*")]
    public class CreateUserController : ApiController
    {
        private ApiWalletContext db = new ApiWalletContext();

        [HttpPost]
        [Route("api/CreateUsers")]
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { message = "No se puede crear tu usuario" });
            }

            if(user.Name == "")
                return Ok(new { message = "El nombre de usuario es requerido." });

            if (user.Email == "")
                return Ok(new { message = "El Email es requerido." });

            if (user.Password == "")
                return Ok(new { message = "Por favor ingrese una contrasena." });

            db.Users.Add(user);
            db.SaveChanges();

            return Ok(new { message = "Usuario creado con exito!!" });
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
