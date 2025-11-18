using CapaEntidad.ControlAcceso;
using CapaEntidad.ControlAcceso.Filtro;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace CapaDatos.ControlAcceso {
    public class CAL_AuditoriaDAL {
        private readonly string conexion;

        public CAL_AuditoriaDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<CAL_AuditoriaEntidad> GetAllAuditoria() {
            List<CAL_AuditoriaEntidad> lista = new List<CAL_AuditoriaEntidad>();
            string consulta = @"SELECT TOP (1000) idAuditoria
                                      ,fecha
                                      ,usuario
                                      ,dni
                                      ,tipo
                                      ,nombre
                                      ,sala
                                  FROM CAL_Auditoria(nolock) ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new CAL_AuditoriaEntidad {
                                idAuditoria = ManejoNulos.ManageNullInteger(dr["idAuditoria"]),
                                fecha = ManejoNulos.ManageNullDate(dr["fecha"]),
                                usuario = ManejoNulos.ManageNullInteger(dr["usuario"]),
                                //usuarioNombre = ManejoNulos.ManageNullStr(dr["usuarioNombre"]),
                                dni = ManejoNulos.ManageNullStr(dr["dni"]),
                                tipo = ManejoNulos.ManageNullStr(dr["tipo"]),
                                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                sala = ManejoNulos.ManageNullStr(dr["sala"]),
                            };

                            lista.Add(item);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public bool RegistrarBusqueda(CAL_AuditoriaEntidad auditoriaBusqueda) {
            var response = false;
            SqlConnection cnn = null;
            string Consulta = @" 

 declare @nomsala varchar(300)=''

declare @codsala varchar(20)=''
select    @codsala=replace(CodSala,'sala_','') from PermisosSala
where estado=1 and UsuarioID=@pUsuarioId

if @codsala=''
	set @nomsala=''
else
begin
	select @nomsala=Nombre from Sala
	where CodSala=convert(int,@codsala)
end

select @nomsala
 
INSERT INTO  AuditoriaLog 
           (tipo
           ,dni
           ,fecha
           ,usuario,
            nombre,sala)
     VALUES
           (@pTipoCliente
           ,@pDni
           ,GETDATE()
           ,@pUsuarioId
           ,@nombre,@nomsala)";

            try {
                using(cnn = new SqlConnection(conexion)) {
                    using(SqlCommand cmd = new SqlCommand(Consulta, cnn)) {
                        cnn.Open();
                        cmd.Parameters.AddWithValue("@pTipoCliente", auditoriaBusqueda.TipoCliente);
                        cmd.Parameters.AddWithValue("@pDni", auditoriaBusqueda.Dni);
                        cmd.Parameters.AddWithValue("@pUsuarioId", auditoriaBusqueda.UsuarioId);
                        cmd.Parameters.AddWithValue("@nombre", auditoriaBusqueda.NombreUsuario.ToUpper().Trim());
                        cmd.ExecuteNonQuery();
                        response = true;
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                response = false;
            } finally {
                if(cnn != null) {
                    if(cnn.State != ConnectionState.Closed) {
                        cnn.Close();
                        cnn.Dispose();
                    }
                }
            }
            return response;
        }
        public List<CAL_AuditoriaEntidad> ReporteAauditoriaBusqueda(string dni, int empresa, DateTime fechaIni, DateTime fechaFin) {
            var Lista = new List<CAL_AuditoriaEntidad>();
            SqlConnection cnn = null;
            SqlDataReader dr = null;

            string Consulta = @" select top 1 au.tipo TipoCliente , au.Dni , (c.Nombre  + ' ' +  c.Apellido) 'Cliente' , 
                (emp.Nombres +  ' ' +  emp.ApellidosPaterno  + ' ' + emp.ApellidosMaterno) Empleado , 
                u.UsuarioNombre  ,au.sala RazonSocial , au.fecha FechaRegistro  from AuditoriaLog au (nolock)
                        left join Cliente c (nolock)
             on au.Dni = c.NroDocumento 
             inner join Usuario u (nolock)  on au.usuario = u.UsuarioID 
            
             inner join SEG_Empleado emp (nolock) on u.EmpleadoID = emp.EmpleadoID  ";

            if(dni != "" || empresa != 0 || fechaIni != null || fechaFin != null) {
                Consulta = Consulta + " where ";
            }
            if(dni != "") {
                Consulta = Consulta + "  au.Dni =  '" + dni + "' ";
            }
            //if (empresa != 0)
            //{
            //    Consulta = Consulta + " au.EmpresaId =  '" + empresa + "' ";
            //}
            if(fechaIni != null && fechaFin != null) {
                Consulta = Consulta + " au.fecha between  @pFechaIni and  @pFechaFin";
            }
            if(fechaIni != null && fechaFin == null) {
                Consulta = Consulta + " au.fecha >=   @pFechaIni ";
            }
            if(fechaIni == null && fechaFin != null) {
                Consulta = Consulta + " au.fecha <=  @pFechaFin ";
            }
            Consulta = Consulta + " order by au.fecha desc ";
            try {
                using(cnn = new SqlConnection(conexion)) {
                    using(SqlCommand cmd = new SqlCommand(Consulta, cnn)) {
                        cnn.Open();
                        if(fechaIni != null && fechaFin != null) {
                            cmd.Parameters.AddWithValue("@pFechaIni", fechaIni);
                            cmd.Parameters.AddWithValue("@pFechaFin", fechaFin);
                        }
                        if(fechaIni != null && fechaFin == null) {
                            cmd.Parameters.AddWithValue("@pFechaIni", fechaIni);
                        }
                        if(fechaIni == null && fechaFin != null) {
                            cmd.Parameters.AddWithValue("@pFechaFin", fechaFin);
                        }
                        using(dr = cmd.ExecuteReader()) {
                            if(dr.HasRows) {
                                while(dr.Read()) {
                                    var item = new CAL_AuditoriaEntidad {
                                        TipoCliente = ManejoNulos.ManageNullStr(dr["TipoCliente"]),
                                        Dni = ManejoNulos.ManageNullStr(dr["Dni"]),
                                        Cliente = ManejoNulos.ManageNullStr(dr["Cliente"]),
                                        Empleado = ManejoNulos.ManageNullStr(dr["Empleado"]),
                                        NombreUsuario = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                        RazonSocial = ManejoNulos.ManageNullStr(dr["RazonSocial"]),
                                        FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                        FechaRegistroString = ManejoNulos.ManageNullStr(dr["FechaRegistro"]),
                                    };
                                    Lista.Add(item);
                                }
                            }
                        }
                    }
                }
            } catch(Exception exp) {
                throw exp;
            } finally {
                if(dr != null) {
                    dr.Close();
                    dr.Dispose();
                }
                if(cnn != null) {
                    if(cnn.State != ConnectionState.Closed) {
                        cnn.Close();
                        cnn.Dispose();
                    }
                }
            }
            return Lista;
        }

        public CAL_AuditoriaEntidad BuscarClienteEnReporte(string dni) {
            var Lista = new CAL_AuditoriaEntidad();
            SqlConnection cnn = null;
            SqlDataReader dr = null;

            string Consulta = @" 
           select top 1 au.tipo TipoCliente , au.Dni , nombre 'Cliente' , 
         au.fecha FechaRegistro,sala  from AuditoriaLog au (nolock)
           where     nombre!=''  ";


            if(dni != "") {
                Consulta = Consulta + " and  au.Dni =  '" + dni + "' ";
            }

            try {
                using(cnn = new SqlConnection(conexion)) {
                    using(SqlCommand cmd = new SqlCommand(Consulta, cnn)) {
                        cnn.Open();

                        using(dr = cmd.ExecuteReader()) {
                            if(dr.HasRows) {
                                while(dr.Read()) {
                                    var item = new CAL_AuditoriaEntidad {
                                        TipoCliente = ManejoNulos.ManageNullStr(dr["TipoCliente"]),
                                        Dni = ManejoNulos.ManageNullStr(dr["Dni"]),
                                        Cliente = ManejoNulos.ManageNullStr(dr["Cliente"]),

                                        //RazonSocial = ManejoNulos.ManageNullStr(dr["RazonSocial"]),
                                        FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                        //FechaRegistroString = ManejoNulos.ManageNullStr(dr["FechaRegistro"]),
                                    };
                                    Lista = item;
                                }
                            }
                        }
                    }
                }
            } catch(Exception exp) {
                throw exp;
            } finally {
                if(dr != null) {
                    dr.Close();
                    dr.Dispose();
                }
                if(cnn != null) {
                    if(cnn.State != ConnectionState.Closed) {
                        cnn.Close();
                        cnn.Dispose();
                    }
                }
            }
            return Lista;
        }
        public bool RegistrarBusquedaExterno(CAL_AuditoriaEntidad auditoriaBusqueda) {
            var response = false;
            SqlConnection cnn = null;
            string Consulta = @" 
INSERT INTO [dbo].[CAL_Auditoria]
           ([fecha]
           ,[usuario]
           ,[dni]
           ,[tipo]
           ,[nombre]
           ,[sala]
           ,[codigo]
      ,[observacion])
     VALUES
           (@fecha,@usuario,@dni,@tipo,@nombre,@sala,@codigo,@observacion)";

            try {
                using(cnn = new SqlConnection(conexion)) {
                    using(SqlCommand cmd = new SqlCommand(Consulta, cnn)) {
                        cnn.Open();
                        cmd.Parameters.AddWithValue("@fecha", ManejoNulos.ManageNullDate(auditoriaBusqueda.FechaRegistro));
                        cmd.Parameters.AddWithValue("@usuario", ManejoNulos.ManageNullStr(auditoriaBusqueda.NombreUsuario).ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@dni", ManejoNulos.ManageNullStr(auditoriaBusqueda.Dni));
                        cmd.Parameters.AddWithValue("@tipo", ManejoNulos.ManageNullStr(auditoriaBusqueda.TipoCliente).ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@nombre", ManejoNulos.ManageNullStr(auditoriaBusqueda.Cliente).ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@sala", ManejoNulos.ManageNullStr(auditoriaBusqueda.NombreSala).ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@codigo", ManejoNulos.ManageNullStr(auditoriaBusqueda.codigo));
                        cmd.Parameters.AddWithValue("@observacion", ManejoNulos.ManageNullStr(auditoriaBusqueda.observacion));
                        cmd.ExecuteNonQuery();
                        response = true;
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                response = false;
            } finally {
                if(cnn != null) {
                    if(cnn.State != ConnectionState.Closed) {
                        cnn.Close();
                        cnn.Dispose();
                    }
                }
            }
            return response;
        }
        public List<CAL_AuditoriaEntidad> GetAuditoriaSala(string ArraySalaId, DateTime fechaIni, DateTime fechaFin) {
            List<CAL_AuditoriaEntidad> lista = new List<CAL_AuditoriaEntidad>();
            string consulta = @"select auditoria.[idAuditoria]
                                      ,auditoria.[fecha]
                                      ,auditoria.[usuario]
                                      ,auditoria.[dni]
                                      ,auditoria.[tipo]
                                      ,auditoria.[nombre]
                                      ,auditoria.[sala],auditoria.[codigo],auditoria.[observacion]
                                 FROM [dbo].[CAL_Auditoria] as auditoria
                                 join [dbo].[Sala] as sala on 
                                 sala.Nombre=auditoria.sala where " + ArraySalaId + " CONVERT(date, auditoria.fecha) between @p1 and @p2";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new CAL_AuditoriaEntidad {
                                idAuditoria = ManejoNulos.ManageNullInteger(dr["idAuditoria"]),
                                fecha = ManejoNulos.ManageNullDate(dr["fecha"]),
                                usuarioNombre = ManejoNulos.ManageNullStr(dr["usuario"]),
                                dni = ManejoNulos.ManageNullStr(dr["dni"]),
                                tipo = ManejoNulos.ManageNullStr(dr["tipo"]),
                                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                sala = ManejoNulos.ManageNullStr(dr["sala"]),
                                codigo = ManejoNulos.ManageNullStr(dr["codigo"]),
                                observacion = ManejoNulos.ManageNullStr(dr["observacion"]),

                            };
                            lista.Add(item);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        #region Migracion DWH
        public List<CAL_AuditoriaEntidad> ObtenerIngresosClientesSalasParaDwh(CAL_IngresoClienteSalaDwhFiltro filtro) {
            List<CAL_AuditoriaEntidad> items = new List<CAL_AuditoriaEntidad>();
            string consulta = $@"        
                SELECT TOP (@CantidadRegistros)
	                idAuditoria,
	                fecha,
	                usuario,
	                dni,
	                tipo,
	                nombre,
	                sala,
	                codigo,
	                observacion
                FROM CAL_Auditoria
                WHERE 
                    FechaMigracionDwh IS NULL
                    AND tipo = 'CLIENTE'
	                AND dni <> ''
	                AND dni IS NOT NULL
	                AND nombre <> ''
	                AND nombre IS NOT NULL
	                AND sala <> ''
	                AND sala IS NOT NULL
                ORDER BY idAuditoria ASC
            ";
            try {
                using(SqlConnection con = new SqlConnection(conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CantidadRegistros", filtro.CantidadRegistros);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public int ActualizarEstadoMigracionDwhPorId(int id, DateTime? fechaMigracionDwh) {
            int idActualizado = 0;
            string consulta = $@"        
                UPDATE CAL_Auditoria
                SET FechaMigracionDwh = @FechaMigracionDwh
                OUTPUT inserted.idAuditoria
                WHERE idAuditoria = @Id
            ";
            try {
                using(SqlConnection con = new SqlConnection(conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    query.Parameters.AddWithValue("@FechaMigracionDwh", fechaMigracionDwh ?? SqlDateTime.Null);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch { }
            return idActualizado;
        }
        #endregion

        private CAL_AuditoriaEntidad ConstruirObjeto(SqlDataReader dr) {
            return new CAL_AuditoriaEntidad {
                idAuditoria = ManejoNulos.ManageNullInteger(dr["idAuditoria"]),
                fecha = ManejoNulos.ManageNullDate(dr["fecha"]),
                usuarioNombre = ManejoNulos.ManageNullStr(dr["usuario"]),
                dni = ManejoNulos.ManageNullStr(dr["dni"]),
                Dni = ManejoNulos.ManageNullStr(dr["dni"]),
                tipo = ManejoNulos.ManageNullStr(dr["tipo"]),
                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                sala = ManejoNulos.ManageNullStr(dr["sala"]),
                codigo = ManejoNulos.ManageNullStr(dr["codigo"]),
                observacion = ManejoNulos.ManageNullStr(dr["observacion"]),
            };
        }
    }
}
