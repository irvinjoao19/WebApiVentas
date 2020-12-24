using Contexto;
using Entidades;
using Negocio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace WebApiDsigeVentas.Controllers
{
    [RoutePrefix("api/Ventas")]
    public class MigrationController : ApiController
    {

        private static readonly string path = ConfigurationManager.AppSettings["uploadFile"];

        [HttpPost]
        [Route("LoginNew")]
        public IHttpActionResult LoginNew(Filtro f)
        {
            try
            {
                Usuario u = MigrationDao.GetLoginNew(f);
                if (u != null)
                {
                    if (u.existe == 0)
                    {
                        return BadRequest("Usuario no Existe");
                    }
                    else if (u.pass == "Error")
                    {
                        return BadRequest("Contraseña Incorrecta");
                    }
                    else
                    {
                        return Ok(u);
                    }
                }
                else return BadRequest("Tu usuario esta disponible en otro movil");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("Logout")]
        public IHttpActionResult Logout(Filtro f)
        {
            try
            {
                Mensaje m = MigrationDao.GetLogout(f);
                if (m != null)
                {
                    return Ok(m);
                }
                else return BadRequest("Error");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
 

        [HttpGet]
        [Route("SyncNew")]
        public IHttpActionResult SyncNew(int operarioId, string version)
        {
            try
            {
                Sync m = MigrationDao.GetMigracionNew(version, operarioId);
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


        [HttpGet]
        [Route("Sync")]
        public IHttpActionResult Sync(int operarioId, string version)
        {
            try
            {
                Sync m = MigrationDao.GetSync(version, operarioId);
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

        [HttpPost]
        [Route("SaveGps")]
        public IHttpActionResult SaveOperarioGps(EstadoOperario estadoOperario)
        {
            Mensaje mensaje = MigrationDao.SaveGps(estadoOperario);
            if (mensaje != null)
            {
                return Ok(mensaje);
            }
            else
                return BadRequest("Error de Envio");

        }

        [HttpPost]
        [Route("SaveMovil")]
        public IHttpActionResult SaveMovil(EstadoMovil e)
        {
            Mensaje mensaje = MigrationDao.SaveMovil(e);
            if (mensaje != null)
            {
                return Ok(mensaje);
            }
            else
                return BadRequest("Error de Envio");

        }

        [HttpPost]
        [Route("SavePedidoNew")]
        public IHttpActionResult SaveOrden(Orden p)
        { 
            Mensaje m = MigrationDao.SaveOrden(p);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error de Envio"); 
        }

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

        [HttpPost]
        [Route("SaveCliente")]
        public IHttpActionResult SaveCliente(Cliente c)
        {

            Mensaje m = MigrationDao.SaveCliente(c);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error de Envio");

        }

        [HttpPost]
        [Route("UpdateReparto")]
        public IHttpActionResult UpdateReparto(Reparto r)
        {

            Mensaje m = MigrationDao.UpdateReparto(r);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error de Envio");

        }

        [HttpGet]
        [Route("GetPersonal")]
        public IHttpActionResult GetPersonal(string fecha)
        {

            List<Personal> p = MigrationDao.GetPersonal(fecha);
            if (p != null)
            {
                return Ok(p);
            }
            else
                return BadRequest("No hay datos");

        }

        [HttpGet]
        [Route("GetResumen")]
        public IHttpActionResult GetResumen(string fecha)
        {

            Resumen r = MigrationDao.GetResumenes(fecha);
            if (r != null)
            {
                return Ok(r);
            }
            else
                return BadRequest("No hay datos");

        }

        [HttpGet]
        [Route("GetProductos")]
        public IHttpActionResult GetProductos(int local)
        {

            List<Producto> p = MigrationDao.GetProductos(local);
            if (p != null)
            {
                return Ok(p);
            }
            else
                return BadRequest("No hay datos");

        }

        // ONLINE


        [HttpPost]
        [Route("SaveCabeceraPedido")]
        public IHttpActionResult SaveCabeceraPedido(Pedido p)
        {

            Mensaje m = MigrationDao.SaveCabeceraPedido(p);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error de Envio");
        }

        [HttpPost]
        [Route("SaveDetallePedidoGroup")]
        public IHttpActionResult SaveDetallePedidoGroup(List<PedidoDetalle> p)
        {

            List<Mensaje> m = MigrationDao.SaveDetallePedidoGroup(p);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error de Envio");
        }

        [HttpPost]
        [Route("SaveDetallePedido")]
        public IHttpActionResult SaveDetallePedido(PedidoDetalle p)
        {
            Mensaje m = MigrationDao.SaveDetallePedido(p);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error de Envio");
        }

        [HttpPost]
        [Route("DeletePedidoDetalle")]
        public IHttpActionResult DeletePedidoDetalle(PedidoDetalle p)
        {
            Mensaje m = MigrationDao.DeleteDetallePedido(p);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error de Envio");
        }
		
		[HttpPost]
        [Route("DeletePedido")]
        public IHttpActionResult DeletePedido(PedidoDetalle p)
        {
            Mensaje m = MigrationDao.DeletePedido(p);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("Error de Envio");
        }

        // reportes

        [HttpGet]
        [Route("ReporteVentaVendedor")]
        public IHttpActionResult ReporteVentaVendedor(int u)
        {
            List<VentaVendedor> m = MigrationDao.ReporteVentaVendedor(u);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("No hay datos");
        }

        [HttpGet]
        [Route("ReporteVentaSupervisor")]
        public IHttpActionResult ReporteVentaSupervisor(int u)
        {
            List<VentaSupervisor> m = MigrationDao.ReporteVentaSupervisor(u);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("No hay datos");
        }

        [HttpGet]
        [Route("ReporteVentaUbicacion")]
        public IHttpActionResult ReporteVentaUbicacion(int u)
        {
            List<VentaUbicacion> m = MigrationDao.ReporteVentaUbicacion(u);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("No hay datos");
        }

        [HttpGet]
        [Route("ReporteMes")]
        public IHttpActionResult ReporteMes(int u)
        {
            List<VentaMes> m = MigrationDao.ReporteMes(u);
            if (m != null)
            {
                return Ok(m);
            }
            else
                return BadRequest("No hay datos");
        }
    }
}
