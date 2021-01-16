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

        public static Usuario GetLoginNew(Filtro f)
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
                    cmd.CommandText = "Movil_Login_New";
                    cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = f.login;
                    cmd.Parameters.Add("@imei", SqlDbType.VarChar).Value = f.imei;
                    cmd.Parameters.Add("@version", SqlDbType.VarChar).Value = f.version;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            u = new Usuario();
                            u.existe = dr.GetInt32(0);

                            if (u.existe != 0)
                            {
                                if (f.pass == dr.GetString(11))
                                {
                                    u.usuarioId = dr.GetInt32(1);
                                    u.documento = dr.GetString(2);
                                    u.apellidos = dr.GetString(3);
                                    u.nombres = dr.GetString(4);
                                    u.tipo = dr.GetInt32(5);
                                    u.cargoId = dr.GetInt32(6);
                                    u.nombreCargo = dr.GetString(7);
                                    u.telefono = dr.GetString(8);
                                    u.email = dr.GetString(9);
                                    u.login = dr.GetString(10);
                                    u.pass = dr.GetString(11);
                                    u.envioOnline = dr.GetString(12);
                                    u.perfil = dr.GetInt32(13);
                                    u.descripcionPerfil = dr.GetString(14);
                                    u.estado = dr.GetInt32(15);
                                    u.localId = dr.GetInt32(16);
                                }
                                else
                                {
                                    u.pass = "Error";
                                }
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
        public static Mensaje GetLogout(Filtro f)
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
                    cmd.CommandText = "Movil_Logout";
                    cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = f.login;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            m = new Mensaje
                            {
                                mensaje = dr.GetString(0)
                            };

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
        public static Sync GetMigracionNew(string version, int operarioId)
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
                        cmdDepartamento.CommandText = "Movil_Traer_Departamento_New";
                        SqlDataReader drD = cmdDepartamento.ExecuteReader();
                        if (drD.HasRows)
                        {
                            List<Departamento> d = new List<Departamento>();

                            while (drD.Read())
                            {
                                d.Add(new Departamento()
                                {
                                    departamentoId = drD.GetInt32(0),
                                    codigo = drD.GetString(1),
                                    departamento = drD.GetString(2)

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
                                    nombreMarca = drPr.GetString(8),
                                    factor = drPr.GetDecimal(9),
                                    precio2 = drPr.GetDecimal(10),
                                    precioMayMenor = drPr.GetDecimal(11),
                                    precioMayMayor = drPr.GetDecimal(12)
                                });
                            }
                            sync.productos = d;
                        }

                        SqlCommand cmdC = con.CreateCommand();
                        cmdC.CommandTimeout = 0;
                        cmdC.CommandType = CommandType.StoredProcedure;
                        cmdC.CommandText = "Movil_Traer_Cliente_New";
                        cmdC.Parameters.Add("@vendedorId", SqlDbType.Int).Value = operarioId;
                        SqlDataReader drC = cmdC.ExecuteReader();
                        if (drC.HasRows)
                        {
                            List<Cliente> d = new List<Cliente>();
                            while (drC.Read())
                            {
                                d.Add(new Cliente()
                                {
                                    identity = drC.GetInt32(0),
                                    clienteId = drC.GetInt32(0),
                                    empresaId = drC.GetInt32(1),
                                    codigoInterno = drC.GetString(2),
                                    tipoClienteId = drC.GetInt32(3),
                                    tipo = drC.GetInt32(3) == 1 ? "Natural" : "Juridico",
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
                                    fechaVisita = drC.GetDateTime(21).ToString("dd/MM/yyyy"),

                                    nombreDepartamento = drC.GetString(22),
                                    nombreProvincia = drC.GetString(23),
                                    nombreDistrito = drC.GetString(24),
                                    nombreGiroNegocio = drC.GetString(25),
                                    tipoPersonal = drC.GetInt32(26)
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
                            List<Reparto> list = new List<Reparto>();
                            while (drR.Read())
                            {
                                var r = new Reparto
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
                                    longitud = drR.GetString(11),
                                    numeroDocumento = drR.GetString(12),
                                    subTotal = drR.GetDecimal(13),
                                    estado = drR.GetInt32(14),
                                    docVTA = drR.GetString(15),
                                    localId = drR.GetInt32(16),
                                    distritoId = drR.GetInt32(17),
                                    nombreDistrito = drR.GetString(18),
                                    motivoId = 0
                                };

                                SqlCommand cmdD = con.CreateCommand();
                                cmdD.CommandTimeout = 0;
                                cmdD.CommandType = CommandType.StoredProcedure;
                                cmdD.CommandText = "Movil_GetPedidoDetalle";
                                cmdD.Parameters.Add("@pedidoId", SqlDbType.Int).Value = r.repartoId;
                                SqlDataReader dr = cmdD.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    List<RepartoDetalle> d = new List<RepartoDetalle>();
                                    while (dr.Read())
                                    {
                                        d.Add(new RepartoDetalle()
                                        {
                                            detalleId = dr.GetInt32(0),
                                            repartoId = dr.GetInt32(1),
                                            pedidoItem = dr.GetInt32(2),
                                            productoId = dr.GetInt32(3),
                                            precioVenta = dr.GetDecimal(4),
                                            porcentajeDescuento = dr.GetDecimal(5),
                                            descuento = dr.GetDecimal(6),
                                            cantidad = dr.GetDecimal(7),
                                            cantidadExacta = dr.GetDecimal(7),
                                            porcentajeIGV = dr.GetDecimal(8),
                                            total = dr.GetDecimal(9),
                                            numeroPedido = dr.GetString(10),
                                            nombreProducto = dr.GetString(11),
                                            codigoProducto = dr.GetString(12),
                                            estado = dr.GetInt32(13)
                                        });
                                    }
                                    r.detalle = d;
                                }
                                list.Add(r);
                            }
                            sync.repartos = list;
                        }


                        SqlCommand cmdE = con.CreateCommand();
                        cmdE.CommandTimeout = 0;
                        cmdE.CommandType = CommandType.StoredProcedure;
                        cmdE.CommandText = "Movil_GetEstados";
                        SqlDataReader drE = cmdE.ExecuteReader();
                        if (drE.HasRows)
                        {
                            List<Estado> e = new List<Estado>();
                            while (drE.Read())
                            {
                                e.Add(new Estado()
                                {
                                    estadoId = drE.GetInt32(0),
                                    nombre = drE.GetString(1),
                                    descripcion = drE.GetString(2),
                                    tipoProceso = drE.GetString(3),
                                    descripcionTipoProceso = drE.GetString(4),
                                    moduloId = drE.GetInt32(5),
                                    backColor = drE.GetInt32(6),
                                    forecolor = drE.GetString(7),
                                    estado = drE.GetInt32(8)
                                });
                            }
                            sync.estados = e;
                        }

                        SqlCommand cmdG = con.CreateCommand();
                        cmdG.CommandTimeout = 0;
                        cmdG.CommandType = CommandType.StoredProcedure;
                        cmdG.CommandText = "Movil_GetGrupo";
                        SqlDataReader drG = cmdG.ExecuteReader();
                        if (drG.HasRows)
                        {
                            List<Grupo> g = new List<Grupo>();
                            while (drG.Read())
                            {
                                g.Add(new Grupo()
                                {
                                    detalleTablaId = drG.GetInt32(0),
                                    grupoTablaId = drG.GetInt32(1),
                                    codigoDetalle = drG.GetString(2),
                                    descripcion = drG.GetString(3),
                                    estado = drG.GetInt32(4)
                                });
                            }
                            sync.grupos = g;
                        }

                        SqlCommand cmdL = con.CreateCommand();
                        cmdL.CommandTimeout = 0;
                        cmdL.CommandType = CommandType.StoredProcedure;
                        cmdL.CommandText = "Movil_GetLocales";
                        SqlDataReader drL = cmdL.ExecuteReader();
                        if (drL.HasRows)
                        {
                            List<Local> l = new List<Local>();
                            while (drL.Read())
                            {
                                l.Add(new Local()
                                {
                                    localId = drL.GetInt32(0),
                                    nombre = drL.GetString(1),
                                    direccion = drL.GetString(2),
                                    estado = drL.GetInt32(3)
                                });
                            }
                            sync.locales = l;
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
        public static Sync GetSync(string version, int operarioId)
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
                        cmdDepartamento.CommandText = "Movil_Traer_Departamento_New";
                        SqlDataReader drD = cmdDepartamento.ExecuteReader();
                        if (drD.HasRows)
                        {
                            List<Departamento> d = new List<Departamento>();

                            while (drD.Read())
                            {
                                d.Add(new Departamento()
                                {
                                    departamentoId = drD.GetInt32(0),
                                    codigo = drD.GetString(1),
                                    departamento = drD.GetString(2)

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

                        SqlCommand cmdC = con.CreateCommand();
                        cmdC.CommandTimeout = 0;
                        cmdC.CommandType = CommandType.StoredProcedure;
                        cmdC.CommandText = "Movil_Traer_Cliente_New";
                        cmdC.Parameters.Add("@vendedorId", SqlDbType.Int).Value = operarioId;
                        SqlDataReader drC = cmdC.ExecuteReader();
                        if (drC.HasRows)
                        {
                            List<Cliente> d = new List<Cliente>();
                            while (drC.Read())
                            {
                                d.Add(new Cliente()
                                {
                                    identity = drC.GetInt32(0),
                                    clienteId = drC.GetInt32(0),
                                    empresaId = drC.GetInt32(1),
                                    codigoInterno = drC.GetString(2),
                                    tipoClienteId = drC.GetInt32(3),
                                    tipo = drC.GetInt32(3) == 1 ? "Natural" : "Juridico",
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
                                    fechaVisita = drC.GetDateTime(21).ToString("dd/MM/yyyy"),

                                    nombreDepartamento = drC.GetString(22),
                                    nombreProvincia = drC.GetString(23),
                                    nombreDistrito = drC.GetString(24),
                                    nombreGiroNegocio = drC.GetString(25),
                                    tipoPersonal = drC.GetInt32(26)
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
                            List<Reparto> list = new List<Reparto>();
                            while (drR.Read())
                            {
                                var r = new Reparto
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
                                    longitud = drR.GetString(11),
                                    numeroDocumento = drR.GetString(12),
                                    subTotal = drR.GetDecimal(13),
                                    estado = drR.GetInt32(14),
                                    docVTA = drR.GetString(15),
                                    localId = drR.GetInt32(16),
                                    distritoId = drR.GetInt32(17),
                                    nombreDistrito = drR.GetString(18),
                                    motivoId = 0
                                };

                                SqlCommand cmdD = con.CreateCommand();
                                cmdD.CommandTimeout = 0;
                                cmdD.CommandType = CommandType.StoredProcedure;
                                cmdD.CommandText = "Movil_GetPedidoDetalle";
                                cmdD.Parameters.Add("@pedidoId", SqlDbType.Int).Value = r.repartoId;
                                SqlDataReader dr = cmdD.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    List<RepartoDetalle> d = new List<RepartoDetalle>();
                                    while (dr.Read())
                                    {
                                        d.Add(new RepartoDetalle()
                                        {
                                            detalleId = dr.GetInt32(0),
                                            repartoId = dr.GetInt32(1),
                                            pedidoItem = dr.GetInt32(2),
                                            productoId = dr.GetInt32(3),
                                            precioVenta = dr.GetDecimal(4),
                                            porcentajeDescuento = dr.GetDecimal(5),
                                            descuento = dr.GetDecimal(6),
                                            cantidad = dr.GetDecimal(7),
                                            cantidadExacta = dr.GetDecimal(7),
                                            porcentajeIGV = dr.GetDecimal(8),
                                            total = dr.GetDecimal(9),
                                            numeroPedido = dr.GetString(10),
                                            nombreProducto = dr.GetString(11),
                                            codigoProducto = dr.GetString(12),
                                            estado = dr.GetInt32(13)
                                        });
                                    }
                                    r.detalle = d;
                                }
                                list.Add(r);
                            }
                            sync.repartos = list;
                        }


                        SqlCommand cmdE = con.CreateCommand();
                        cmdE.CommandTimeout = 0;
                        cmdE.CommandType = CommandType.StoredProcedure;
                        cmdE.CommandText = "Movil_GetEstados";
                        SqlDataReader drE = cmdE.ExecuteReader();
                        if (drE.HasRows)
                        {
                            List<Estado> e = new List<Estado>();
                            while (drE.Read())
                            {
                                e.Add(new Estado()
                                {
                                    estadoId = drE.GetInt32(0),
                                    nombre = drE.GetString(1),
                                    descripcion = drE.GetString(2),
                                    tipoProceso = drE.GetString(3),
                                    descripcionTipoProceso = drE.GetString(4),
                                    moduloId = drE.GetInt32(5),
                                    backColor = drE.GetInt32(6),
                                    forecolor = drE.GetString(7),
                                    estado = drE.GetInt32(8)
                                });
                            }
                            sync.estados = e;
                        }

                        SqlCommand cmdG = con.CreateCommand();
                        cmdG.CommandTimeout = 0;
                        cmdG.CommandType = CommandType.StoredProcedure;
                        cmdG.CommandText = "Movil_GetGrupo";
                        SqlDataReader drG = cmdG.ExecuteReader();
                        if (drG.HasRows)
                        {
                            List<Grupo> g = new List<Grupo>();
                            while (drG.Read())
                            {
                                g.Add(new Grupo()
                                {
                                    detalleTablaId = drG.GetInt32(0),
                                    grupoTablaId = drG.GetInt32(1),
                                    codigoDetalle = drG.GetString(2),
                                    descripcion = drG.GetString(3),
                                    estado = drG.GetInt32(4)
                                });
                            }
                            sync.grupos = g;
                        }

                        SqlCommand cmdL = con.CreateCommand();
                        cmdL.CommandTimeout = 0;
                        cmdL.CommandType = CommandType.StoredProcedure;
                        cmdL.CommandText = "Movil_GetLocales";
                        SqlDataReader drL = cmdL.ExecuteReader();
                        if (drL.HasRows)
                        {
                            List<Local> l = new List<Local>();
                            while (drL.Read())
                            {
                                l.Add(new Local()
                                {
                                    localId = drL.GetInt32(0),
                                    nombre = drL.GetString(1),
                                    direccion = drL.GetString(2),
                                    estado = drL.GetInt32(3)
                                });
                            }
                            sync.locales = l;
                        }

                        // nuevo pedidos
                        SqlCommand cmd1 = con.CreateCommand();
                        cmd1.CommandTimeout = 0;
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.CommandText = "Movil_GetPedidoCabecera";
                        cmd1.Parameters.Add("@usuarioId", SqlDbType.Int).Value = operarioId;
                        SqlDataReader dr1 = cmd1.ExecuteReader();
                        if (dr1.HasRows)
                        {
                            List<Pedido> list = new List<Pedido>();
                            while (dr1.Read())
                            {
                                var r = new Pedido
                                {
                                    pedidoId = dr1.GetInt32(0),
                                    identity = dr1.GetInt32(0),
                                    empresaId = dr1.GetInt32(1),
                                    numeroPedido = dr1.GetString(2),
                                    codigoInternoSuministro = dr1.GetString(3),
                                    almacenId = dr1.GetInt32(4),
                                    tipoDocumento = dr1.GetInt32(5),
                                    puntoVentaId = dr1.GetInt32(6),
                                    cuadrillaId = dr1.GetInt32(7),
                                    personalVendedorId = dr1.GetInt32(8),
                                    formaPagoId = dr1.GetInt32(9),
                                    monedaId = dr1.GetInt32(10),
                                    tipoCambio = dr1.GetDecimal(11),
                                    codigoInternoCliente = dr1.GetString(12),
                                    clienteId = dr1.GetInt32(13),
                                    direccionPedido = dr1.GetString(14),
                                    porcentajeIGV = dr1.GetDecimal(15),
                                    observacion = dr1.GetString(16),
                                    latitud = dr1.GetString(17),
                                    longitud = dr1.GetString(18),
                                    estado = dr1.GetInt32(19),
                                    subtotal = dr1.GetDecimal(20),
                                    totalIgv = dr1.GetDecimal(21),
                                    totalNeto = dr1.GetDecimal(22),
                                    numeroDocumento = dr1.GetString(23),
                                    fechaFacturaPedido = dr1.GetDateTime(24).ToString("dd/MM/yyyy"),
                                    localId = dr1.GetInt32(25),
                                    nombreCliente = dr1.GetString(26),
                                    tipoPersonal = dr1.GetInt32(27)
                                };

                                SqlCommand cmd2 = con.CreateCommand();
                                cmd2.CommandTimeout = 0;
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.CommandText = "Movil_GetPedidoDetalleBody";
                                cmd2.Parameters.Add("@pedidoId", SqlDbType.Int).Value = r.pedidoId;
                                SqlDataReader dr2 = cmd2.ExecuteReader();
                                if (dr2.HasRows)
                                {
                                    List<PedidoDetalle> d = new List<PedidoDetalle>();
                                    while (dr2.Read())
                                    {
                                        d.Add(new PedidoDetalle()
                                        {
                                            pedidoDetalleId = dr2.GetInt32(0),
                                            identityDetalle = dr2.GetInt32(0),
                                            pedidoId = dr2.GetInt32(1),
                                            identity = dr2.GetInt32(1),
                                            pedidoItem = dr2.GetInt32(2),
                                            productoId = dr2.GetInt32(3),
                                            precioVenta = dr2.GetDecimal(4),
                                            porcentajeDescuento = dr2.GetDecimal(5),
                                            descuentoPedido = dr2.GetDecimal(6),
                                            cantidad = dr2.GetDecimal(7),
                                            porcentajeIGV = dr2.GetDecimal(8),
                                            totalPedido = dr2.GetDecimal(9),
                                            numeroPedido = dr2.GetString(10),
                                            codigo = dr2.GetString(11),
                                            nombre = dr2.GetString(12),
                                            descripcion = dr2.GetString(13),
                                            abreviaturaProducto = dr2.GetString(14),
                                            unidadMedida = dr2.GetDecimal(15),
                                            stockMinimo = dr2.GetDecimal(16),
                                            subTotal = dr2.GetDecimal(17),
                                            factor = dr2.GetDecimal(18),
                                            precio1 = dr2.GetDecimal(19),
                                            precio2 = dr2.GetDecimal(20),
                                            precioMayMenor = dr2.GetDecimal(21),
                                            precioMayMayor = dr2.GetDecimal(22),
                                            rangoCajaHorizontal = dr2.GetDecimal(23),
                                            rangoCajaMayorista = dr2.GetDecimal(24),
                                            estado = dr2.GetInt32(25),
                                            localId = r.localId,
                                            active = 1
                                        });
                                    }
                                    r.detalles = d;
                                }
                                list.Add(r);
                            }
                            sync.pedidos = list;
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
                    cmd.Parameters.Add("@pedidoId", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@empresaId", SqlDbType.Int).Value = p.empresaId;
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
        public static Mensaje SaveGps(EstadoOperario e)
        {
            try
            {
                Mensaje m = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_Save_Gps";
                    cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                    cmd.Parameters.Add("@latitud", SqlDbType.VarChar).Value = e.latitud;
                    cmd.Parameters.Add("@longitud", SqlDbType.VarChar).Value = e.longitud;
                    cmd.Parameters.Add("@fechaGPD", SqlDbType.VarChar).Value = e.fechaGPD;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;
                    int a = cmd.ExecuteNonQuery();
                    if (a == 1)
                    {
                        m = new Mensaje
                        {
                            codigo = 1,
                            mensaje = "Enviado"
                        };
                    }
                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Mensaje SaveMovil(EstadoMovil e)
        {
            try
            {
                Mensaje m = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Save_Movil";
                    cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                    cmd.Parameters.Add("@gpsActivo", SqlDbType.Int).Value = e.gpsActivo;
                    cmd.Parameters.Add("@estadoBateria", SqlDbType.Int).Value = e.estadoBateria;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;
                    cmd.Parameters.Add("@modoAvion", SqlDbType.Int).Value = e.modoAvion;
                    cmd.Parameters.Add("@planDatos", SqlDbType.Int).Value = e.planDatos;

                    int a = cmd.ExecuteNonQuery();
                    if (a == 1)
                    {
                        m = new Mensaje
                        {
                            codigo = 1,
                            mensaje = "Enviado"
                        };
                    }

                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Mensaje SaveCliente(Cliente c)
        {
            try
            {
                Mensaje m = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Save_Cliente";
                    cmd.Parameters.Add("@clienteId", SqlDbType.Int).Value = c.identity;
                    cmd.Parameters.Add("@id_empresa", SqlDbType.Int).Value = c.empresaId;
                    cmd.Parameters.Add("@codigointerno_cliente", SqlDbType.VarChar).Value = c.codigoInterno;
                    cmd.Parameters.Add("@id_tipocliente", SqlDbType.Int).Value = (c.tipo == "Natural" ? 1 : 6);
                    cmd.Parameters.Add("@id_documentoidentidad", SqlDbType.VarChar).Value = c.documentoIdentidad;
                    cmd.Parameters.Add("@nrodoc_cliente", SqlDbType.VarChar).Value = c.documento;
                    cmd.Parameters.Add("@nombres_cliente", SqlDbType.VarChar).Value = c.nombreCliente;
                    cmd.Parameters.Add("@id_provincia", SqlDbType.Int).Value = c.provinciaId;
                    cmd.Parameters.Add("@nrocelular_cliente", SqlDbType.VarChar).Value = c.nroCelular;
                    cmd.Parameters.Add("@email_cliente", SqlDbType.VarChar).Value = c.email;
                    cmd.Parameters.Add("@motivodenocompra", SqlDbType.VarChar).Value = c.motivoNoCompra;
                    cmd.Parameters.Add("@productointeres", SqlDbType.VarChar).Value = c.productoInteres;
                    cmd.Parameters.Add("@direccion_cliente", SqlDbType.VarChar).Value = c.direccion;
                    cmd.Parameters.Add("@id_personalvendedor", SqlDbType.Int).Value = c.personalVendedorId;
                    cmd.Parameters.Add("@latitud_cliente", SqlDbType.VarChar).Value = c.latitud;
                    cmd.Parameters.Add("@longitud_cliente", SqlDbType.VarChar).Value = c.longitud;
                    cmd.Parameters.Add("@estado", SqlDbType.Int).Value = c.estado;
                    cmd.Parameters.Add("@usuario_creacion", SqlDbType.Int).Value = c.personalVendedorId;
                    cmd.Parameters.Add("@cond_facturacion", SqlDbType.Int).Value = c.condFacturacion;
                    cmd.Parameters.Add("@id_departamento", SqlDbType.Int).Value = c.departamentoId;
                    cmd.Parameters.Add("@id_gironegocio", SqlDbType.Int).Value = c.giroNegocioId;
                    cmd.Parameters.Add("@fechavisita_cliente", SqlDbType.VarChar).Value = c.fechaVisita;
                    cmd.Parameters.Add("@distritoId", SqlDbType.Int).Value = c.distritoId;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            m = new Mensaje
                            {
                                codigoBase = c.clienteId,
                                codigoRetorno = dr.GetInt32(0),
                                mensaje = "Enviado"
                            };
                        }
                    }
                    cn.Close();
                }
                return m;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static Mensaje UpdateReparto(Reparto r)
        {
            try
            {
                Mensaje m = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_UpdateReparto";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = r.repartoId;
                    cmd.Parameters.Add("@estado", SqlDbType.Int).Value = r.estado;
                    cmd.Parameters.Add("@id_MotivoDevolucion", SqlDbType.Int).Value = r.motivoId;
                    cmd.Parameters.Add("@cantidad", SqlDbType.Decimal).Value = r.subTotal;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            m = new Mensaje
                            {
                                codigoBase = r.repartoId,
                                codigoRetorno = dr.GetInt32(0),
                                mensaje = "Actualizado"
                            };


                            foreach (var d in r.detalle)
                            {
                                SqlCommand cmdD = cn.CreateCommand();
                                cmdD.CommandType = CommandType.StoredProcedure;
                                cmdD.CommandText = "Movil_Actualizar_cantidad";
                                cmdD.Parameters.Add("@id_pedido", SqlDbType.Int).Value = d.repartoId;
                                cmdD.Parameters.Add("@producto", SqlDbType.Int).Value = d.productoId;
                                cmdD.Parameters.Add("@cant", SqlDbType.Decimal).Value = d.cantidad;
                                cmdD.Parameters.Add("@accion", SqlDbType.Int).Value = d.estado;
                                cmdD.ExecuteNonQuery();
                            }
                        }
                    }
                    cn.Close();
                }
                return m;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static List<Personal> GetPersonal(string fecha)
        {
            try
            {
                List<Personal> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Get_ResumenDia";
                    cmd.Parameters.Add("@Fecha", SqlDbType.VarChar).Value = fecha;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<Personal>();
                        while (dr.Read())
                        {
                            p.Add(new Personal
                            {
                                personalId = dr.GetInt32(0),
                                nombrePersonal = dr.GetString(1),
                                countPedidos = dr.GetInt32(2),
                                countClientes = dr.GetInt32(3),
                                countProductos = dr.GetInt32(4),
                                total = dr.GetDecimal(5),
                                latitud = dr.GetString(6),
                                longitud = dr.GetString(7)
                            });
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public static List<Producto> GetProductos(int local)
        {
            try
            {
                List<Producto> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Traer_Stock_New";
                    cmd.Parameters.Add("@Local", SqlDbType.Int).Value = local;
                    SqlDataReader drPr = cmd.ExecuteReader();
                    if (drPr.HasRows)
                    {
                        p = new List<Producto>();
                        while (drPr.Read())
                        {
                            p.Add(new Producto()
                            {
                                productoId = drPr.GetInt32(0),
                                codigoProducto = drPr.GetString(1),
                                nombreProducto = drPr.GetString(2),
                                descripcionProducto = drPr.GetString(3),
                                abreviaturaProducto = drPr.GetString(4),
                                stock = drPr.GetDecimal(5),
                                precio = drPr.GetDecimal(6),
                                nombreCategoria = drPr.GetString(7),
                                nombreMarca = drPr.GetString(8),
                                factor = drPr.GetDecimal(9),
                                precio2 = drPr.GetDecimal(10),
                                precioMayMenor = drPr.GetDecimal(11),
                                precioMayMayor = drPr.GetDecimal(12),
                                rangoCajaHorizontal = drPr.GetDecimal(13),
                                rangoCajaMayorista = drPr.GetDecimal(14)
                            });
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static Resumen GetResumenes(string fecha)
        {
            try
            {
                Resumen r = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Get_TotalGeneral";
                    cmd.Parameters.Add("@Fecha", SqlDbType.VarChar).Value = fecha;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            r = new Resumen
                            {
                                totalVenta = dr.GetDecimal(0),
                                countPedidoVenta = dr.GetInt32(1),
                                countClientes = dr.GetInt32(2),
                                vendedorId = dr.GetInt32(3),
                                mejorVendedor = dr.GetString(4),
                                mejorVendedorSoles = dr.GetDecimal(5),
                                productoId = dr.GetInt32(6),
                                mejorProducto = dr.GetString(7),
                                mejorProductoSoles = dr.GetDecimal(8),
                                totalDevolucion = dr.GetDecimal(9),
                                peorVendedorId = dr.GetInt32(10),
                                peorVendedor = dr.GetString(11),
                                peorVendedorSoles = dr.GetDecimal(12)
                            };
                        }
                    }
                    cn.Close();
                }
                return r;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public static Mensaje SaveOrden(Orden o)
        {
            try
            {
                Mensaje m = new Mensaje();

                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();

                    var c = o.cliente;
                    SqlCommand cmdC = cn.CreateCommand();
                    cmdC.CommandType = CommandType.StoredProcedure;
                    cmdC.CommandTimeout = 0;
                    cmdC.CommandText = "Movil_Save_Cliente";
                    cmdC.Parameters.Add("@clienteId", SqlDbType.Int).Value = c.identity;
                    cmdC.Parameters.Add("@id_empresa", SqlDbType.Int).Value = c.empresaId;
                    cmdC.Parameters.Add("@codigointerno_cliente", SqlDbType.VarChar).Value = c.codigoInterno;
                    cmdC.Parameters.Add("@id_tipocliente", SqlDbType.Int).Value = (c.tipo == "Natural" ? 1 : 6);
                    cmdC.Parameters.Add("@id_documentoidentidad", SqlDbType.VarChar).Value = c.documentoIdentidad;
                    cmdC.Parameters.Add("@nrodoc_cliente", SqlDbType.VarChar).Value = c.documento;
                    cmdC.Parameters.Add("@nombres_cliente", SqlDbType.VarChar).Value = c.nombreCliente;
                    cmdC.Parameters.Add("@id_provincia", SqlDbType.Int).Value = c.provinciaId;
                    cmdC.Parameters.Add("@nrocelular_cliente", SqlDbType.VarChar).Value = c.nroCelular;
                    cmdC.Parameters.Add("@email_cliente", SqlDbType.VarChar).Value = c.email;
                    cmdC.Parameters.Add("@motivodenocompra", SqlDbType.VarChar).Value = c.motivoNoCompra;
                    cmdC.Parameters.Add("@productointeres", SqlDbType.VarChar).Value = c.productoInteres;
                    cmdC.Parameters.Add("@direccion_cliente", SqlDbType.VarChar).Value = c.direccion;
                    cmdC.Parameters.Add("@id_personalvendedor", SqlDbType.Int).Value = c.personalVendedorId;
                    cmdC.Parameters.Add("@latitud_cliente", SqlDbType.VarChar).Value = c.latitud;
                    cmdC.Parameters.Add("@longitud_cliente", SqlDbType.VarChar).Value = c.longitud;
                    cmdC.Parameters.Add("@estado", SqlDbType.Int).Value = c.estado;
                    cmdC.Parameters.Add("@usuario_creacion", SqlDbType.Int).Value = c.personalVendedorId;
                    cmdC.Parameters.Add("@cond_facturacion", SqlDbType.Int).Value = c.condFacturacion;
                    cmdC.Parameters.Add("@id_departamento", SqlDbType.Int).Value = c.departamentoId;
                    cmdC.Parameters.Add("@id_gironegocio", SqlDbType.Int).Value = c.giroNegocioId;
                    cmdC.Parameters.Add("@fechavisita_cliente", SqlDbType.VarChar).Value = c.fechaVisita;
                    cmdC.Parameters.Add("@distritoId", SqlDbType.Int).Value = c.distritoId;

                    SqlDataReader drC = cmdC.ExecuteReader();
                    if (drC.HasRows)
                    {
                        while (drC.Read())
                        {
                            m.codigoBaseCliente = c.clienteId;
                            m.codigoRetornoCliente = drC.GetInt32(0);
                        }
                    }

                    var p = o.pedido;

                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_Save_Pedido";
                    cmd.Parameters.Add("@pedidoId", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@empresaId", SqlDbType.Int).Value = p.empresaId;
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
                    cmd.Parameters.Add("@clienteId", SqlDbType.Int).Value = m.codigoRetornoCliente;
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

                            m.codigoBase = p.pedidoId;
                            m.codigoRetorno = dr.GetInt32(0);
                            m.mensaje = "Guardado";

                            foreach (var d in p.detalles)
                            {
                                SqlCommand cmdD = cn.CreateCommand();
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
                    cn.Close();
                }

                return m;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        //nuevo online

        public static Mensaje SaveCabeceraPedido(Pedido p)
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
                    cmd.CommandText = "Movil_Save_Pedido_New";
                    cmd.Parameters.Add("@pedidoId", SqlDbType.Int).Value = p.identity;
                    cmd.Parameters.Add("@empresaId", SqlDbType.Int).Value = p.empresaId;
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


        public static List<Mensaje> SaveDetallePedidoGroup(List<PedidoDetalle> p)
        {
            try
            {
                List<Mensaje> m = new List<Mensaje>();

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    foreach (var d in p)
                    {
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Movil_Save_Pedido_Detalle_New";
                        cmd.Parameters.Add("@pedidoDetalleId", SqlDbType.Int).Value = d.identityDetalle;
                        cmd.Parameters.Add("@pedidoId", SqlDbType.Int).Value = d.identity;
                        cmd.Parameters.Add("@pedidoItem", SqlDbType.Int).Value = d.pedidoItem;
                        cmd.Parameters.Add("@productoId", SqlDbType.Int).Value = d.productoId;
                        cmd.Parameters.Add("@precioVenta", SqlDbType.Decimal).Value = d.precioVenta;
                        cmd.Parameters.Add("@porcentajeDescuento", SqlDbType.Decimal).Value = d.porcentajeDescuento;
                        cmd.Parameters.Add("@descuentoPedido", SqlDbType.Decimal).Value = d.descuentoPedido;
                        cmd.Parameters.Add("@cantidad", SqlDbType.Decimal).Value = d.cantidad;
                        cmd.Parameters.Add("@porcentajeIGV", SqlDbType.Decimal).Value = d.porcentajeIGV;
                        cmd.Parameters.Add("@totalPedido", SqlDbType.Decimal).Value = d.totalPedido;
                        cmd.Parameters.Add("@numeroPedido", SqlDbType.VarChar).Value = d.numeroPedido;
                        cmd.Parameters.Add("@Local", SqlDbType.Int).Value = d.localId;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                m.Add(new Mensaje
                                {
                                    codigoBase = d.pedidoDetalleId,
                                    codigoRetorno = dr.GetInt32(0),
                                    mensaje = "Guardado"
                                });
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

        public static Mensaje SaveDetallePedido(PedidoDetalle d)
        {
            try
            {
                Mensaje m = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_Save_Pedido_Detalle_New";
                    cmd.Parameters.Add("@pedidoDetalleId", SqlDbType.Int).Value = d.identityDetalle;
                    cmd.Parameters.Add("@pedidoId", SqlDbType.Int).Value = d.identity;
                    cmd.Parameters.Add("@pedidoItem", SqlDbType.Int).Value = d.pedidoItem;
                    cmd.Parameters.Add("@productoId", SqlDbType.Int).Value = d.productoId;
                    cmd.Parameters.Add("@precioVenta", SqlDbType.Decimal).Value = d.precioVenta;
                    cmd.Parameters.Add("@porcentajeDescuento", SqlDbType.Decimal).Value = d.porcentajeDescuento;
                    cmd.Parameters.Add("@descuentoPedido", SqlDbType.Decimal).Value = d.descuentoPedido;
                    cmd.Parameters.Add("@cantidad", SqlDbType.Decimal).Value = d.cantidad;
                    cmd.Parameters.Add("@porcentajeIGV", SqlDbType.Decimal).Value = d.porcentajeIGV;
                    cmd.Parameters.Add("@totalPedido", SqlDbType.Decimal).Value = d.totalPedido;
                    cmd.Parameters.Add("@numeroPedido", SqlDbType.VarChar).Value = d.numeroPedido;
                    cmd.Parameters.Add("@Local", SqlDbType.Int).Value = d.localId;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            m = new Mensaje
                            {
                                codigoBase = d.pedidoDetalleId,
                                codigoRetorno = dr.GetInt32(0),
                                mensaje = dr.GetString(1),
                                stock = dr.GetDecimal(2)
                            };
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
	 
        public static Mensaje DeleteDetallePedido(PedidoDetalle d)
        {
            try
            {
                Mensaje m = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_Delete_Pedido_Detalle";
                    cmd.Parameters.Add("@pedidoDetalleId", SqlDbType.Int).Value = d.identityDetalle;
                    cmd.Parameters.Add("@pedidoId", SqlDbType.Int).Value = d.identity;
                    cmd.Parameters.Add("@pedidoItem", SqlDbType.Int).Value = d.pedidoItem;
                    cmd.Parameters.Add("@productoId", SqlDbType.Int).Value = d.productoId;
                    cmd.Parameters.Add("@precioVenta", SqlDbType.Decimal).Value = d.precioVenta;
                    cmd.Parameters.Add("@porcentajeDescuento", SqlDbType.Decimal).Value = d.porcentajeDescuento;
                    cmd.Parameters.Add("@descuentoPedido", SqlDbType.Decimal).Value = d.descuentoPedido;
                    cmd.Parameters.Add("@cantidad", SqlDbType.Decimal).Value = d.cantidad;
                    cmd.Parameters.Add("@porcentajeIGV", SqlDbType.Decimal).Value = d.porcentajeIGV;
                    cmd.Parameters.Add("@totalPedido", SqlDbType.Decimal).Value = d.totalPedido;
                    cmd.Parameters.Add("@numeroPedido", SqlDbType.VarChar).Value = d.numeroPedido;
                    cmd.Parameters.Add("@Local", SqlDbType.Int).Value = d.localId;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            m = new Mensaje
                            {
                                codigoBase = d.pedidoDetalleId,
                                codigoRetorno = dr.GetInt32(0),
                                mensaje = "Eliminado"
                            };
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

        public static Mensaje DeletePedido(PedidoDetalle d)
        {
            try
            {
                Mensaje m = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_Delete_Pedido";
                    cmd.Parameters.Add("@pedidoId", SqlDbType.Int).Value = d.identity;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            m = new Mensaje
                            {
                                codigoBase = d.pedidoId,
                                codigoRetorno = dr.GetInt32(0),
                                mensaje = "Eliminado"
                            };
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


        // reportes


        public static List<VentaVendedor> ReporteVentaVendedor(int u)
        {
            try
            {
                List<VentaVendedor> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Reporte_Venta_Un_Vendedor";
                    cmd.Parameters.Add("@vendedor", SqlDbType.Int).Value = u;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<VentaVendedor>();
                        while (dr.Read())
                        {
                            p.Add(new VentaVendedor
                            {
                                ventaReal = dr.GetDecimal(0),
                                ventaMes = dr.GetDecimal(1),
                                devolucion = dr.GetDecimal(2),
                                pedidoDia = dr.GetInt32(3),
                                ventaDia = dr.GetDecimal(4),
                                fechaEmision = dr.GetDateTime(5).ToString("dd/MM/yyyy"),
                                total = dr.GetDecimal(6)
                            });
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public static List<VentaSupervisor> ReporteVentaSupervisor(int u)
        {
            try
            {
                List<VentaSupervisor> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Reporte_Venta_Un_Supervisor";
                    cmd.Parameters.Add("@Supervisor", SqlDbType.Int).Value = u;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<VentaSupervisor>();
                        while (dr.Read())
                        {
                            p.Add(new VentaSupervisor
                            {
                                ventaMes = dr.GetDecimal(0),
                                devolucionMes = dr.GetDecimal(1),
                                ventaRealMes = dr.GetDecimal(2),
                                ventaDia = dr.GetDecimal(3),
                                pedidoDia = dr.GetInt32(4),
                                vendedorId = dr.GetInt32(5),
                                vendedor = dr.GetString(6),
                                totalVtaMes = dr.GetDecimal(7),
                                totalDevMes = dr.GetDecimal(8),
                                totalVtaRealMes = dr.GetDecimal(9),
                                totalVtaDia = dr.GetDecimal(10),
                                totalPedidoDia = dr.GetInt32(11)
                            });
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<VentaUbicacion> ReporteVentaUbicacion(int u)
        {
            try
            {
                List<VentaUbicacion> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Reporte_Venta_Ubicacion_UnVendedor";
                    cmd.Parameters.Add("@Vendedor", SqlDbType.Int).Value = u;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<VentaUbicacion>();
                        while (dr.Read())
                        {
                            p.Add(new VentaUbicacion
                            {
                                pedidoCabId = dr.GetInt32(0),
                                clienteId = dr.GetInt32(1),
                                nroDocCliente = dr.GetString(2),
                                nombreCliente = dr.GetString(3),
                                direccion = dr.GetString(4),
                                latitud = dr.GetString(5),
                                longitud = dr.GetString(6),
                                total = dr.GetDecimal(7),
                                vendedor = ""
                            });
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<VentaMes> ReporteMes(int u)
        {
            try
            {
                List<VentaMes> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Reporte_Venta_Mes_UnVendedor";
                    cmd.Parameters.Add("@Vendedor", SqlDbType.Int).Value = u;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<VentaMes>();
                        while (dr.Read())
                        {
                            p.Add(new VentaMes
                            {
                                fecha = dr.GetString(0),
                                total = dr.GetDecimal(1)
                            });
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static VentaCabecera ReporteCabecera()
        {
            try
            {
                VentaCabecera p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Reporte_Resumen_Venta_Cabecera";
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            p = new VentaCabecera
                            {
                                totalVtaMes = dr.GetDecimal(0),
                                totalDevolucion = dr.GetInt32(1),
                                totalVtaReal = dr.GetDecimal(2),
                                totalPedidoDia = dr.GetInt32(3),
                                totalVtaDia = dr.GetDecimal(4)
                            };
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static List<VentaAdmin> ReporteAdminBody(int t)
        {
            try
            {
                List<VentaAdmin> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    if (t == 1)
                        cmd.CommandText = "Movil_Reporte_Resumen_Venta_Supervisor_Det";
                    else
                        cmd.CommandText = "Movil_Reporte_Resumen_Venta_Vendedor_Det";

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<VentaAdmin>();
                        while (dr.Read())
                        {
                            p.Add(new VentaAdmin
                            {
                                localId = dr.GetInt32(0),
                                vendedorId = dr.GetInt32(1),
                                vendedor = dr.GetString(2),
                                vtaMes = dr.GetDecimal(3),
                                devMes = dr.GetDecimal(4),
                                vtaRealMes = dr.GetDecimal(5),
                                vtaDia = dr.GetDecimal(6),
                                pedidoDia = dr.GetInt32(7),
                                tipo = t
                            });
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // reporte admin supervisor

        public static List<VentaUbicacion> ReporteAdminSupervisor1(int id, int local)
        {
            try
            {
                List<VentaUbicacion> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Reporte_Resumen_Venta_Supervisor_VtaDiaria";
                    cmd.Parameters.Add("@TipoReporte", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@id_Supervisor", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@id_Local", SqlDbType.Int).Value = local;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<VentaUbicacion>();
                        while (dr.Read())
                        {
                            p.Add(new VentaUbicacion
                            {
                                pedidoCabId = dr.GetInt32(0),
                                clienteId = dr.GetInt32(1),
                                nroDocCliente = dr.GetString(2),
                                nombreCliente = dr.GetString(3),
                                direccion = dr.GetString(4),
                                latitud = dr.GetString(5),
                                longitud = dr.GetString(6),
                                total = dr.GetDecimal(7),
                                vendedor = dr.GetString(8)
                            });
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<VentaMes> ReporteAdminSupervisor2(int id, int local)
        {
            try
            {
                List<VentaMes> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Reporte_Resumen_Venta_Supervisor_VtaDiaria";
                    cmd.Parameters.Add("@TipoReporte", SqlDbType.Int).Value = 2;
                    cmd.Parameters.Add("@id_Supervisor", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@id_Local", SqlDbType.Int).Value = local;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<VentaMes>();
                        while (dr.Read())
                        {
                            p.Add(new VentaMes
                            {
                                fecha = dr.GetDateTime(0).ToString("dd/MM/yyyy"),
                                total = dr.GetDecimal(1)
                            });
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<VentaAdminVendedor> ReporteAdminSupervisor3(int id, int local)
        {
            try
            {
                List<VentaAdminVendedor> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Reporte_Resumen_Venta_Supervisor_VtaDiaria";
                    cmd.Parameters.Add("@TipoReporte", SqlDbType.Int).Value = 3;
                    cmd.Parameters.Add("@id_Supervisor", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@id_Local", SqlDbType.Int).Value = local;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<VentaAdminVendedor>();
                        while (dr.Read())
                        {
                            p.Add(new VentaAdminVendedor
                            {
                                vendedorId = dr.GetInt32(0),
                                vendedor = dr.GetString(1),
                                totalMes = dr.GetDecimal(2),
                                totalDia = dr.GetDecimal(3)
                            });
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // reporte admin vendedor

        public static List<VentaUbicacion> ReporteAdminVendedor1(int id, int local)
        {
            try
            {
                List<VentaUbicacion> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Reporte_Resumen_Venta_Vendedor_VtaDiaria";
                    cmd.Parameters.Add("@TipoReporte", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@id_Supervisor", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@id_Local", SqlDbType.Int).Value = local;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<VentaUbicacion>();
                        while (dr.Read())
                        {
                            p.Add(new VentaUbicacion
                            {
                                pedidoCabId = dr.GetInt32(0),
                                clienteId = dr.GetInt32(1),
                                nroDocCliente = dr.GetString(2),
                                nombreCliente = dr.GetString(3),
                                direccion = dr.GetString(4),
                                latitud = dr.GetString(5),
                                longitud = dr.GetString(6),
                                total = dr.GetDecimal(7),
                                vendedor = dr.GetString(8)
                            });
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<VentaMes> ReporteAdminVendedor2(int id, int local)
        {
            try
            {
                List<VentaMes> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Reporte_Resumen_Venta_Vendedor_VtaDiaria";
                    cmd.Parameters.Add("@TipoReporte", SqlDbType.Int).Value = 2;
                    cmd.Parameters.Add("@id_Supervisor", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@id_Local", SqlDbType.Int).Value = local;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<VentaMes>();
                        while (dr.Read())
                        {
                            p.Add(new VentaMes
                            {
                                fecha = dr.GetDateTime(0).ToString("dd/MM/yyyy"),
                                total = dr.GetDecimal(1)
                            });
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<VentaUbicacionVendedor> ReporteAdminVendedorUbicacion()
        {
            try
            {
                List<VentaUbicacionVendedor> p = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_Reporte_Resumen_Venta_UbicacionVendedores";
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        p = new List<VentaUbicacionVendedor>();
                        while (dr.Read())
                        {
                            p.Add(new VentaUbicacionVendedor
                            {
                                id = dr.GetInt32(0),
                                latitud = dr.GetString(1),
                                longitud = dr.GetString(2),
                                operarioId = dr.GetInt32(3),
                                vendedor = dr.GetString(4),
                                total = dr.GetDecimal(5),
                                totalPedidos = dr.GetInt32(6)
                            });
                        }
                    }
                    cn.Close();
                }
                return p;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}