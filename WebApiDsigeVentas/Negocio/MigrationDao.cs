using Contexto;
using Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class MigrationDao
    {
        private static string db = ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public static Usuario GetLogin(Filtro f)
        {
            try
            {
                Usuario u = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_Logueo";
                    cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = f.login;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            u = new Usuario();

                            if (f.pass == dr.GetString(10))
                            {
                                u.usuarioId = dr.GetInt32(0);
                                u.documento = dr.GetString(1);
                                u.apellidos = dr.GetString(2);
                                u.nombres = dr.GetString(3);
                                u.tipo = dr.GetInt32(4);
                                u.cargoId = dr.GetInt32(5);
                                u.nombreCargo = dr.GetString(6);
                                u.telefono = dr.GetString(7);
                                u.email = dr.GetString(8);
                                u.login = dr.GetString(9);
                                u.pass = dr.GetString(10);
                                u.envioOnline = dr.GetString(11);
                                u.perfil = dr.GetInt32(12);
                                u.descripcionPerfil = dr.GetString(13);
                                u.estado = dr.GetInt32(14);
                            }
                        }
                    }
                    con.Close();
                }

                return u;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static Sync GetMigracion(string version, int operarioId)
        {
            try
            {
                Sync sync = new Sync();

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    // Version
                    SqlCommand cmdVersion = con.CreateCommand();
                    cmdVersion.CommandTimeout = 0;
                    cmdVersion.CommandType = CommandType.StoredProcedure;
                    cmdVersion.CommandText = "Movil_GetVersion";
                    cmdVersion.Parameters.Add("@version", SqlDbType.VarChar).Value = version;

                    SqlDataReader drVersion = cmdVersion.ExecuteReader();
                    if (!drVersion.HasRows)
                    {
                        sync.mensaje = "Update";
                    }
                    else
                    {
                        // Identidad
                        SqlCommand cmdIdentidad = con.CreateCommand();
                        cmdIdentidad.CommandTimeout = 0;
                        cmdIdentidad.CommandType = CommandType.StoredProcedure;
                        cmdIdentidad.CommandText = "Movil_Traer_DocIdentidad";
                        SqlDataReader drI = cmdIdentidad.ExecuteReader();
                        if (drI.HasRows)
                        {
                            List<Identidad> i = new List<Identidad>();
                            while (drI.Read())
                            {
                                i.Add(new Identidad()
                                {
                                    codigo = drI.GetString(0),
                                    descripcion = drI.GetString(1)

                                });
                            }
                            sync.identidades = i;
                        }

                        // Negocio
                        SqlCommand cmdNegocio = con.CreateCommand();
                        cmdNegocio.CommandTimeout = 0;
                        cmdNegocio.CommandType = CommandType.StoredProcedure;
                        cmdNegocio.CommandText = "Movil_Traer_GiroNegocio";
                        SqlDataReader drN = cmdNegocio.ExecuteReader();
                        if (drN.HasRows)
                        {
                            List<GiroNegocio> n = new List<GiroNegocio>();
                            while (drN.Read())
                            {
                                n.Add(new GiroNegocio()
                                {
                                    negocioId = drN.GetInt32(0),
                                    nombre = drN.GetString(1)

                                });
                            }
                            sync.negocios = n;
                        }

                        // Departamento
                        SqlCommand cmdDepartamento = con.CreateCommand();
                        cmdDepartamento.CommandTimeout = 0;
                        cmdDepartamento.CommandType = CommandType.StoredProcedure;
                        cmdDepartamento.CommandText = "Movil_Traer_Departamento";
                        SqlDataReader drD = cmdDepartamento.ExecuteReader();
                        if (drD.HasRows)
                        {
                            List<Departamento> d = new List<Departamento>();

                            while (drD.Read())
                            {
                                d.Add(new Departamento()
                                {
                                    codigo = drD.GetString(0),
                                    departamento = drD.GetString(1)

                                });
                            }
                            sync.departamentos = d;
                        }

                        SqlCommand cmdProvincia = con.CreateCommand();
                        cmdProvincia.CommandTimeout = 0;
                        cmdProvincia.CommandType = CommandType.StoredProcedure;
                        cmdProvincia.CommandText = "Movil_Traer_Provincia";
                        SqlDataReader drP = cmdProvincia.ExecuteReader();
                        if (drP.HasRows)
                        {
                            List<Provincia> p = new List<Provincia>();
                            while (drP.Read())
                            {
                                p.Add(new Provincia()
                                {
                                    provinciaId = drP.GetInt32(0),
                                    codigo = drP.GetString(1),
                                    provincia = drP.GetString(2),
                                    codigoDeparmento = drP.GetString(3)
                                });
                            }
                            sync.provincias = p;
                        }

                        SqlCommand cmdDistrito = con.CreateCommand();
                        cmdDistrito.CommandTimeout = 0;
                        cmdDistrito.CommandType = CommandType.StoredProcedure;
                        cmdDistrito.CommandText = "Movil_Traer_Distrito";
                        SqlDataReader drDi = cmdDistrito.ExecuteReader();
                        if (drDi.HasRows)
                        {
                            List<Distrito> d = new List<Distrito>();
                            while (drDi.Read())
                            {
                                d.Add(new Distrito()
                                {
                                    distritoId = drDi.GetInt32(0),
                                    codigoProvincia = drDi.GetString(1),
                                    codigoDepartamento = drDi.GetString(2),
                                    codigoDistrito = drDi.GetString(3),
                                    nombre = drDi.GetString(4)
                                });
                            }
                            sync.distritos = d;
                        }

                        SqlCommand cmdP = con.CreateCommand();
                        cmdP.CommandTimeout = 0;
                        cmdP.CommandType = CommandType.StoredProcedure;
                        cmdP.CommandText = "Movil_Traer_Stock";
                        cmdP.Parameters.Add("@Local", SqlDbType.Int).Value = 1;
                        SqlDataReader drPr = cmdP.ExecuteReader();
                        if (drPr.HasRows)
                        {
                            List<Producto> d = new List<Producto>();
                            while (drPr.Read())
                            {
                                d.Add(new Producto()
                                {
                                    productoId = drPr.GetInt32(0),
                                    codigoProducto = drPr.GetString(1),
                                    nombreProducto = drPr.GetString(2),
                                    descripcionProducto = drPr.GetString(3),
                                    abreviaturaProducto = drPr.GetString(4),
                                    stock = drPr.GetDecimal(5),
                                    precio = drPr.GetDecimal(6),
                                    nombreCategoria = drPr.GetString(7),
                                    nombreMarca = drPr.GetString(8)
                                });
                            }
                            sync.productos = d;
                        }

                        SqlCommand cmdC = con.CreateCommand();
                        cmdC.CommandTimeout = 0;
                        cmdC.CommandType = CommandType.StoredProcedure;
                        cmdC.CommandText = "Movil_Traer_Cliente";
                        cmdC.Parameters.Add("@vendedorId", SqlDbType.Int).Value = operarioId;
                        SqlDataReader drC = cmdC.ExecuteReader();
                        if (drC.HasRows)
                        {
                            List<Cliente> d = new List<Cliente>();
                            while (drC.Read())
                            {
                                d.Add(new Cliente()
                                {
                                    clienteId = drC.GetInt32(0),
                                    empresaId = drC.GetInt32(1),
                                    codigoInterno = drC.GetString(2),
                                    tipoClienteId = drC.GetInt32(3),
                                    tipo = "Juridico",
                                    documentoIdentidad = drC.GetString(4),
                                    documento = drC.GetString(5),
                                    nombreCliente = drC.GetString(6),
                                    departamentoId = drC.GetInt32(7),
                                    provinciaId = drC.GetInt32(8),
                                    distritoId = drC.GetInt32(9),
                                    direccion = drC.GetString(10),
                                    nroCelular = drC.GetString(11),
                                    giroNegocioId = drC.GetInt32(12),
                                    email = drC.GetString(13),
                                    motivoNoCompra = drC.GetString(14),
                                    productoInteres = drC.GetString(15),
                                    personalVendedorId = drC.GetInt32(16),
                                    latitud = drC.GetString(17),
                                    longitud = drC.GetString(18),
                                    estado = drC.GetInt32(19),
                                    condFacturacion = drC.GetInt32(20),
                                    fechaVisita = drC.GetDateTime(21).ToString("dd/MM/yyyy")
                                });
                            }
                            sync.clientes = d;
                        }

                        SqlCommand cmdF = con.CreateCommand();
                        cmdF.CommandTimeout = 0;
                        cmdF.CommandType = CommandType.StoredProcedure;
                        cmdF.CommandText = "Movil_Traer_FormaPago";                        
                        SqlDataReader drF = cmdF.ExecuteReader();
                        if (drF.HasRows)
                        {
                            List<FormaPago> f = new List<FormaPago>();
                            while (drF.Read())
                            {
                                f.Add(new FormaPago()
                                {
                                    formaPagoId = drF.GetInt32(0),
                                    descripcion = drF.GetString(1),
                                    diasVencimiento = drF.GetInt32(2),
                                    estado = drF.GetInt32(3)
                                });
                            }
                            sync.formaPagos = f;
                        }

                        SqlCommand cmdR = con.CreateCommand();
                        cmdR.CommandTimeout = 0;
                        cmdR.CommandType = CommandType.StoredProcedure;
                        cmdR.CommandText = "Movil_GetPedido";
                        cmdR.Parameters.Add("@usuarioId", SqlDbType.Int).Value = operarioId;
                        SqlDataReader drR = cmdR.ExecuteReader();
                        if (drR.HasRows)
                        {
                            List<Reparto> r = new List<Reparto>();
                            while (drR.Read())
                            {
                                r.Add(new Reparto()
                                {
                                    repartoId = drR.GetInt32(0),
                                    numeroPedido = drR.GetString(1),
                                    almacenId = drR.GetInt32(2),
                                    descripcion = drR.GetString(3),
                                    personalVendedorId = drR.GetInt32(4),
                                    apellidoPersonal = drR.GetString(5),
                                    clienteId = drR.GetInt32(6),
                                    apellidoNombreCliente = drR.GetString(7),
                                    direccion = drR.GetString(8),
                                    fechaEntrega = drR.GetString(9),
                                    latitud = drR.GetString(10),
                                    longitud = drR.GetString(11)
                                });
                            }
                            sync.repartos = r;
                        }

                        sync.mensaje = "Sincronización Completada.";
                    }
                    con.Close();
                }
                return sync;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Mensaje SavePedido(Pedido p)
        {
            try
            {
                Mensaje m = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_Save_Pedido";
                    cmd.Parameters.Add("@pedidoId", SqlDbType.Int).Value =0;
                    cmd.Parameters.Add("@empresaId", SqlDbType.Int).Value =p.empresaId;
                    cmd.Parameters.Add("@numeroPedido", SqlDbType.VarChar).Value = p.numeroPedido;
                    cmd.Parameters.Add("@codigoInternoSuministro", SqlDbType.VarChar).Value = p.codigoInternoSuministro;
                    cmd.Parameters.Add("@almacenId", SqlDbType.Int).Value = p.almacenId;
                    cmd.Parameters.Add("@tipoDocumento", SqlDbType.Int).Value = p.tipoDocumento;
                    cmd.Parameters.Add("@puntoVenta", SqlDbType.Int).Value = p.puntoVentaId;
                    cmd.Parameters.Add("@cuadrillaId", SqlDbType.Int).Value = p.cuadrillaId;
                    cmd.Parameters.Add("@personalVendedorId", SqlDbType.Int).Value = p.personalVendedorId;
                    cmd.Parameters.Add("@formaPagoId", SqlDbType.Int).Value = p.formaPagoId;
                    cmd.Parameters.Add("@monedaId", SqlDbType.Int).Value = p.monedaId;
                    cmd.Parameters.Add("@tipoCambio", SqlDbType.Decimal).Value = p.tipoCambio;
                    cmd.Parameters.Add("@codigoInternoCliente", SqlDbType.VarChar).Value = p.codigoInternoCliente;
                    cmd.Parameters.Add("@clienteId", SqlDbType.Int).Value = p.clienteId;
                    cmd.Parameters.Add("@direccionPedido", SqlDbType.VarChar).Value = p.direccionPedido;
                    cmd.Parameters.Add("@porcentajeIGV", SqlDbType.Decimal).Value = p.porcentajeIGV;
                    cmd.Parameters.Add("@observacion", SqlDbType.VarChar).Value = p.observacion;
                    cmd.Parameters.Add("@latitud", SqlDbType.VarChar).Value = p.latitud;
                    cmd.Parameters.Add("@longitud", SqlDbType.VarChar).Value = p.longitud;
                    cmd.Parameters.Add("@estado", SqlDbType.Int).Value = p.estado;
                    cmd.Parameters.Add("@subtotal", SqlDbType.Decimal).Value = p.subtotal;
                    cmd.Parameters.Add("@totalIgv", SqlDbType.Decimal).Value = p.totalIgv;
                    cmd.Parameters.Add("@totalNeto", SqlDbType.Decimal).Value = p.totalNeto;
                    cmd.Parameters.Add("@numeroDocumento", SqlDbType.VarChar).Value = p.numeroDocumento;
                    cmd.Parameters.Add("@fechaFacturaPedido", SqlDbType.VarChar).Value = p.fechaFacturaPedido;
                    cmd.Parameters.Add("@localId", SqlDbType.Int).Value = p.localId;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            m = new Mensaje
                            {
                                codigoBase = p.pedidoId,
                                codigoRetorno = dr.GetInt32(0),
                                mensaje = "Guardado"
                            };
                            
                            foreach (var d in p.detalles)
                            {
                                SqlCommand cmdD = con.CreateCommand();
                                cmdD.CommandType = CommandType.StoredProcedure;
                                cmdD.CommandText = "Movil_Save_Pedido_Detalle"; 
                                cmdD.Parameters.Add("@pedidoId", SqlDbType.Int).Value = m.codigoRetorno;
                                cmdD.Parameters.Add("@pedidoItem", SqlDbType.Int).Value = d.pedidoItem;
                                cmdD.Parameters.Add("@productoId", SqlDbType.Int).Value = d.productoId;
                                cmdD.Parameters.Add("@precioVenta", SqlDbType.Decimal).Value = d.precioVenta;
                                cmdD.Parameters.Add("@porcentajeDescuento", SqlDbType.Decimal).Value = d.porcentajeDescuento;
                                cmdD.Parameters.Add("@descuentoPedido", SqlDbType.Decimal).Value = d.descuentoPedido;
                                cmdD.Parameters.Add("@cantidad", SqlDbType.Decimal).Value = d.cantidad;
                                cmdD.Parameters.Add("@porcentajeIGV", SqlDbType.Decimal).Value = d.porcentajeIGV;
                                cmdD.Parameters.Add("@totalPedido", SqlDbType.Decimal).Value = d.totalPedido;
                                cmdD.Parameters.Add("@numeroPedido", SqlDbType.VarChar).Value = d.numeroPedido;                                                                 
                                cmdD.ExecuteNonQuery();
                            }
                        }
                    }

                    con.Close();
                }

                return m;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
