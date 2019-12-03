using Contexto;
using Entidades;
using Negocio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiDsigeVentas.Controllers
{
    [RoutePrefix("api/Ventas")]
    public class MigrationController : ApiController
    {

        private static readonly string path = ConfigurationManager.AppSettings["uploadFile"];

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(Filtro f)
        {

            Usuario u = MigrationDao.GetLogin(f);
            if (u != null)
            {
                if (u.pass != null)
                    return Ok(u);
                else
                    return BadRequest("Constraseña Incorrecta");
            }
            else return BadRequest("Usuario no Existe");
          
        }


        [HttpGet]
        [Route("Sync")]
        public IHttpActionResult Sync(int operarioId, string version)
        {
            try
            {
                Sync m = MigrationDao.GetMigracion(version,operarioId);
                if (m.mensaje != "Update")
                {
                    return Ok(m);
                }
                else return BadRequest("Actualizar Versión del Aplicativo.");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("Save")]
        public IHttpActionResult Save()
        {
            try
            {
                //var fotos = HttpContext.Current.Request.Files;
                //var json = HttpContext.Current.Request.Form["model"];
                //Registro p = JsonConvert.DeserializeObject<Registro>(json);

                //Mensaje mensaje = MigrationDAO.SaveRegistro(p);

                //if (mensaje != null)
                //{
                //    for (int i = 0; i < fotos.Count; i++)
                //    {
                //        string fileName = Path.GetFileName(fotos[i].FileName);
                //        fotos[i].SaveAs(path + fileName);
                //    }
                //}
                //else
                //{
                //    mensaje = new Mensaje
                //    {
                //        mensaje = "Registro repetido"
                //    };
                //}

                return Ok("ok");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[Route("SaveGps")]
        //public IHttpActionResult SaveOperarioGps(EstadoOperario estadoOperario)
        //{
        //    Mensaje mensaje = MigrationDAO.SaveGps(estadoOperario);
        //    if (mensaje != null)
        //    {
        //        return Ok(mensaje);
        //    }
        //    else
        //        return BadRequest("Error de Envio");

        //}

        //[HttpPost]
        //[Route("SaveMovil")]
        //public IHttpActionResult SaveMovil(EstadoMovil e)
        //{
        //    Mensaje mensaje = MigrationDAO.SaveMovil(e);
        //    if (mensaje != null)
        //    {
        //        return Ok(mensaje);
        //    }
        //    else
        //        return BadRequest("Error de Envio");

        //} 

        [HttpPost]
        [Route("SavePedido")]
        public IHttpActionResult SavePedido(Pedido p)
        {
 
            Mensaje m = MigrationDao.SavePedido(p);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error de Envio");
 
        }
    }
}
