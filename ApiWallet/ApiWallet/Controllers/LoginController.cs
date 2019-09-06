using ApiWallet.Models;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiWallet.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    [EnableCors("*", "*", "*")]
    public class LoginController : ApiController
    {
        private ApiWalletContext db = new ApiWalletContext();

        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            var user = db.Users.FirstOrDefault(x => x.Name == login.Username && x.Password == login.Password);
            
            if (user == null)
                return Ok(new { message = "Usuario o contrasena incorrectos."});

            //TODO: Validate credentials Correctly, this code is only for demo !!

            if (user != null)
            {
                var token = TokenGenerator.GenerateTokenJwt(login.Username);

                LoginReturn loginReturn = new LoginReturn();
                loginReturn.IdUser = user.UserId;
                loginReturn.Token = token;

                return Ok(loginReturn);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
