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
    public class UsersController : ApiController
    {
        private ApiWalletContext db = new ApiWalletContext();

        [HttpGet]
        [Route("api/Users")]
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        [HttpGet]
        [Route("api/Users")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser([FromUri]int UserId)
        {
            User user = db.Users.Find(UserId);
            if (user == null)
            {
                return Ok(new { message = "No existe este usuario." });
            }

            return Ok(user);
        }

        [HttpPut]
        [Route("api/Users")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser([FromUri]int id, User user)
        {

            if (id != user.UserId)
            {
                return Ok(new { message = "Usario no coincide." });
            }

            if(user.Email == "" || user.Name == "" || user.Password == "")
            {
                return Ok(new { message = "Los campos no pueden estar vacios." });
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return Ok(new { message = "Usuario no existe." });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Usuario editado con exito." });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}