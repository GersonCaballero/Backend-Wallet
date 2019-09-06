using ApiWallet.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiWallet.Controllers
{
    [Authorize]
    [EnableCors("*", "*", "*")]
    public class GraficaController : ApiController
    {
        private ApiWalletContext db = new ApiWalletContext();

        [HttpGet]
        [Route ("api/Grafica")]
        
        public IHttpActionResult Grafica([FromUri]int idAccount)
        {
            var Account = db.Cuentas.Find(idAccount);

            var ingresos = db.Ingresos
                .Include("Cuenta")
                .Where(w => w.CuentaId == Account.CuentaId)
                .Select(s => new {
                    Cuenta = Account.Nombre,
                    Descripcion = s.Descripcion,
                    Monto = s.Monto,
                    MontoActual = s.MontoActual,
                    Fecha = s.FechaIngreso
                });

            var egresos = db.Egresos
                .Include("Cuenta")
                .Where(w => w.CuentaId == Account.CuentaId)
                .Select(s => new {
                    Cuenta = Account.Nombre,
                    Descripcion = s.Descripcion,
                    Monto = s.Monto,
                    MontoActual = s.MontoActual,
                    Fecha = s.FechaEgreso
                });

            var arringresos = ingresos.ToArray();
            var arregresos = egresos.ToArray();
            var mes = "";
            var ingreso = 0;
            var egreso = 0;

            Grafica[] arrgrafica = new Grafica[DateTime.Now.Month];

            for(int i = 1; i <= DateTime.Now.Month; i++)
            {
                DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                string nombreMes = formatoFecha.GetMonthName(i);
                mes = nombreMes;

                for (int a = 0; a < arringresos.Length; a++)
                {
                    if (arringresos[a].Fecha.Month == i)
                    {
                        ingreso = ingreso + Convert.ToInt32(arringresos[a].Monto);
                    }
                }

                for(int b = 0; b < arregresos.Length; b++)
                {
                    if(arregresos[b].Fecha.Month == i)
                    {
                        egreso = egreso + Convert.ToInt32(arregresos[b].Monto);
                    }
                }

                Grafica datos = new Grafica();
                datos.Mes = mes;
                datos.Ingreso = ingreso;
                datos.Egreso = egreso;

                arrgrafica[i-1] = datos;
            }

            return Ok(arrgrafica);
        }
    }
}
