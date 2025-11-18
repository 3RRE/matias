using CapaEntidad;
using S3k.Utilitario;
using S3k.Utilitario.clases_especial;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class SEG_UsuarioDAL
    {
        string _conexion = string.Empty;
        public SEG_UsuarioDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<SEG_UsuarioEntidad> UsuarioListadoJson()
        {
            List<SEG_UsuarioEntidad> lista = new List<SEG_UsuarioEntidad>();
            string consulta = @"SELECT 
                             (emp.ApellidosPaterno+' '+emp.ApellidosMaterno+', '+emp.Nombres) nombreEmpleado
                              ,[UsuarioID]
                              ,seg.[EmpleadoID]
                              ,[TipoUsuarioID]
                              ,[UsuarioNombre]
                              ,[UsuarioContraseña]
                              ,[FechaRegistro]
                              ,[FailedAttempts]
                              ,[Estado]
                              ,[Token],emp.[DOI]
                                FROM [SEG_Usuario] seg
                                join SEG_Empleado emp on emp.EmpleadoID=seg.EmpleadoID
	                          order by UsuarioID Desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var usuario = new SEG_UsuarioEntidad
                            {
                                NombreEmpleado = ManejoNulos.ManageNullStr(dr["nombreEmpleado"].Trim()),
                                UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]),
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"].Trim()),
                                TipoUsuarioID = ManejoNulos.ManageNullInteger(dr["TipoUsuarioID"].Trim()),
                                UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"].Trim()),
                                UsuarioContraseña = ManejoNulos.ManageNullStr(dr["UsuarioContraseña"].Trim()),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"].Trim()),
                                FailedAttempts = ManejoNulos.ManageNullInteger(dr["FailedAttempts"].Trim()),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim()),
                                UsuarioToken = ManejoNulos.ManageNullStr(dr["Token"].Trim()),
                                DOI = ManejoNulos.ManageNullStr(dr["DOI"].Trim())
                            };

                            lista.Add(usuario);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }
        public SEG_UsuarioEntidad UsuarioEmpleadoIDObtenerJson(int usuarioid)
        {
            SEG_UsuarioEntidad segUsuario = new SEG_UsuarioEntidad();
            string consulta = @"SELECT [UsuarioID]
                              ,(emp.ApellidosPaterno+' '+emp.ApellidosMaterno+', '+emp.Nombres) nombreEmpleado
                              ,seg.[EmpleadoID]
                              ,[TipoUsuarioID]
                              ,[UsuarioNombre]
                              ,[UsuarioContraseña]
                              ,[FechaRegistro]
                              ,[FailedAttempts]
                              ,[Estado]
                          FROM [dbo].[SEG_Usuario] seg
                          join SEG_Empleado emp on emp.EmpleadoID=seg.EmpleadoID
	                       where UsuarioID=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", usuarioid);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                segUsuario.NombreEmpleado = ManejoNulos.ManageNullStr(dr["nombreEmpleado"].Trim());
                                segUsuario.UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]);
                                segUsuario.EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]);
                                segUsuario.TipoUsuarioID = ManejoNulos.ManageNullInteger(dr["TipoUsuarioID"].Trim());
                                segUsuario.UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"].Trim());
                                segUsuario.UsuarioContraseña = ManejoNulos.ManageNullStr(dr["UsuarioContraseña"].Trim());
                                segUsuario.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                segUsuario.FailedAttempts = ManejoNulos.ManageNullInteger(dr["FailedAttempts"]);
                                segUsuario.Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim());

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return segUsuario;
        }
        public SEG_UsuarioEntidad UsuarioEmpleadoIdObtenerJson(int empleadoid)
        {
            SEG_UsuarioEntidad segUsuario = new SEG_UsuarioEntidad();
            string consulta = @"SELECT [UsuarioID]
                              ,[EmpleadoID]
                              ,[TipoUsuarioID]
                              ,[UsuarioNombre]
                              ,[UsuarioContraseña]
                              ,[FechaRegistro]
                              ,[FailedAttempts]
                              ,[Estado]
                          FROM [dbo].[SEG_Usuario]
	                       where EmpleadoID=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", empleadoid);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                segUsuario.UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]);
                                segUsuario.EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]);
                                segUsuario.TipoUsuarioID = ManejoNulos.ManageNullInteger(dr["TipoUsuarioID"].Trim());
                                segUsuario.UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"].Trim());
                                segUsuario.UsuarioContraseña = ManejoNulos.ManageNullStr(dr["UsuarioContraseña"].Trim());
                                segUsuario.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                segUsuario.FailedAttempts = ManejoNulos.ManageNullInteger(dr["FailedAttempts"]);
                                segUsuario.Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim());

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return segUsuario;
        }
        public SEG_UsuarioEntidad UsuarioCoincidenciaObtenerJson(string usuario, int usuarioID, int condicion)
        {
            SEG_UsuarioEntidad segUsuario = new SEG_UsuarioEntidad();
            string consulta = @"declare  @condicion  int " +
                                " set @condicion=@p2" +
                               " if(@condicion=1) " +
                              " begin" +
                              " SELECT [UsuarioID],[EmpleadoID],[TipoUsuarioID],[UsuarioNombre]" +
                              ",[UsuarioContraseña],[FechaRegistro],[FailedAttempts],[Estado],[EstadoContrasena]" +
                              " FROM [dbo].[SEG_Usuario]" +
                              " where UsuarioNombre=@p0 and" +
                              " UsuarioID NOT in(@p1)" +
                              " end" +
                              " else" +
                              " begin" +
                              " SELECT [UsuarioID],[EmpleadoID],[TipoUsuarioID],[UsuarioNombre]" +
                              ",[UsuarioContraseña],[FechaRegistro],[FailedAttempts],[Estado],[EstadoContrasena]" +
                              " FROM [dbo].[SEG_Usuario]" +
                              " where UsuarioNombre=@p0" +
                              " end";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", usuario);
                    query.Parameters.AddWithValue("@p1", usuarioID);
                    query.Parameters.AddWithValue("@p2", condicion);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                segUsuario.UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]);
                                segUsuario.EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]);
                                segUsuario.TipoUsuarioID = ManejoNulos.ManageNullInteger(dr["TipoUsuarioID"].Trim());
                                segUsuario.UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"].Trim());
                                segUsuario.UsuarioContraseña = ManejoNulos.ManageNullStr(dr["UsuarioContraseña"].Trim());
                                segUsuario.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                segUsuario.FailedAttempts = ManejoNulos.ManageNullInteger(dr["FailedAttempts"]);
                                segUsuario.Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim());
                                segUsuario.EstadoContrasena = ManejoNulos.ManageNullInteger(dr["EstadoContrasena"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                segUsuario.UsuarioNombre = ex.Message;
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return segUsuario;
        }
        public bool UsuarioGuardarJson(SEG_UsuarioEntidad usuario)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO [dbo].[SEG_Usuario]
           ([EmpleadoID],[TipoUsuarioID],[UsuarioNombre],[UsuarioContraseña],[FechaRegistro],[Estado],[EstadoContrasena])
                VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(usuario.EmpleadoID));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(2));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(usuario.UsuarioNombre));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(usuario.UsuarioContraseña));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(usuario.FechaRegistro));
                    query.Parameters.AddWithValue("@p5", 1);
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(usuario.EstadoContrasena));
                    query.ExecuteNonQuery();

                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }

        public Int32 UsuarioGuardarGlpiJson(SEG_UsuarioEntidad usuario)
        {
            Int32 IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[SEG_Usuario]
           ([EmpleadoID],[TipoUsuarioID],[UsuarioNombre],[UsuarioContraseña],[FechaRegistro],[Estado],[EstadoContrasena])  Output Inserted.UsuarioID
                VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(usuario.EmpleadoID));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(2));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(usuario.UsuarioNombre));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(usuario.UsuarioContraseña));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(usuario.FechaRegistro));
                    query.Parameters.AddWithValue("@p5", 1);
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(usuario.EstadoContrasena));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return IdInsertado;
        }

        public SEG_UsuarioEntidad UsuarioGuardarEntidadJson(SEG_UsuarioEntidad usuario)
        {
            SEG_UsuarioEntidad respuesta = new SEG_UsuarioEntidad();
            string consulta = @"INSERT INTO [dbo].[SEG_Usuario]
           ([EmpleadoID],[TipoUsuarioID],[UsuarioNombre],[UsuarioContraseña],[FechaRegistro],[Estado],[EstadoContrasena])
                VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(usuario.EmpleadoID));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(2));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(usuario.UsuarioNombre));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(usuario.UsuarioContraseña));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(usuario.FechaRegistro));
                    query.Parameters.AddWithValue("@p5", 1);
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(usuario.EstadoContrasena));
                    query.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }


        public bool UsuarioCambiarContrasenia(SEG_UsuarioEntidad segUsuario)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[SEG_Usuario]
                        SET 
                       [UsuarioContraseña] = @p1
                       ,FailedAttempts=@p2
                       WHERE usuarioID = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", segUsuario.UsuarioID);
                    query.Parameters.AddWithValue("@p1", segUsuario.UsuarioContraseña);
                    query.Parameters.AddWithValue("@p2", segUsuario.FailedAttempts);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }

        public bool UsuarioActualizarJson(SEG_UsuarioEntidad segUsuario)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[SEG_Usuario]
                        SET 
                        EmpleadoID = @p1
                       ,UsuarioNombre = @p2
                       ,[UsuarioContraseña] = @p3
                       ,FailedAttempts=@p4
                       WHERE usuarioID = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", segUsuario.UsuarioID);
                    query.Parameters.AddWithValue("@p1", segUsuario.EmpleadoID);
                    query.Parameters.AddWithValue("@p2", segUsuario.UsuarioNombre);
                    query.Parameters.AddWithValue("@p3", segUsuario.UsuarioContraseña);
                    query.Parameters.AddWithValue("@p4", segUsuario.FailedAttempts);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }
        public bool ActualizarEstadoUsuario(int usuarioid, int estado)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[SEG_Usuario]
                        SET 
                      [Estado] = @p1
                       WHERE usuarioID = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", usuarioid);
                    query.Parameters.AddWithValue("@p1", estado);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return respuesta;
        }
        public bool EliminarUsuario(int usuarioid)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM [dbo].[SEG_Usuario] WHERE usuarioID = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", usuarioid);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }
        public bool clrEnabled()
        {
            bool respuesta = false;
            string consulta = @"EXEC sp_configure 'clr enabled';  
                                EXEC sp_configure 'clr enabled' , '1';  
                                RECONFIGURE;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }
        public bool CambiarContrasena(string usuPassword, int usuarioId)
        {
            bool respuesta = false;
            string consulta = @"UPDATE SEG_Usuario
SET UsuarioContraseña=@p0,EstadoContrasena=0
WHERE UsuarioID=@p1;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(usuPassword));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(usuarioId));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }
        public bool RestablecerContrasena(string contrasena, int UsuarioID)
        {
            var respuesta = false;
            string consulta = @"update [dbo].[SEG_Usuario] set UsuarioContraseña=@p0,EstadoContrasena=1 where UsuarioID=@p1";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", contrasena);
                    query.Parameters.AddWithValue("@p1", UsuarioID);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }
        public bool UsuarioBloquearJson(SEG_UsuarioEntidad Entidad)
        {
            bool estatus = false;
            SqlConnection cnn = null;
            SqlDataReader dr = null;
            string Consulta = @"Update SEG_Usuario 
set FailedAttempts = 5
where UsuarioID = @pUsuarioID";
            try
            {
                using (cnn = new SqlConnection(_conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(Consulta, cnn))
                    {
                        cnn.Open();
                        cmd.Parameters.AddWithValue("@pUsuarioID", Entidad.UsuarioID);
                        cmd.ExecuteNonQuery();
                        estatus = true;
                    }
                }

            }
            catch (SqlException exp)
            {

                throw exp;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }
                if (cnn != null)
                {
                    if (cnn.State != ConnectionState.Closed)
                    {
                        cnn.Close();
                        cnn.Dispose();
                    }
                }
            }
            return estatus;

        }
        public bool UsuarioDesbloquearJson(SEG_UsuarioEntidad Entidad)
        {
            bool estatus = false;
            SqlConnection cnn = null;
            SqlDataReader dr = null;
            string Consulta = @"Update SEG_Usuario 
set FailedAttempts = 0
where UsuarioID = @pUsuarioID";
            try
            {
                using (cnn = new SqlConnection(_conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(Consulta, cnn))
                    {
                        cnn.Open();
                        cmd.Parameters.AddWithValue("@pUsuarioID", Entidad.UsuarioID);
                        cmd.ExecuteNonQuery();
                        estatus = true;
                    }
                }

            }
            catch (SqlException exp)
            {

                throw exp;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }
                if (cnn != null)
                {
                    if (cnn.State != ConnectionState.Closed)
                    {
                        cnn.Close();
                        cnn.Dispose();
                    }
                }
            }
            return estatus;

        }

        public SEG_UsuarioEntidad UsuarioObtenerEmpleadoUsuarioIdJson(int usuarioId)
        {
            SEG_UsuarioEntidad segUsuario = new SEG_UsuarioEntidad();
            string consulta = @"SELECT [UsuarioID]
                              ,SEG_Usuario.[EmpleadoID]
                              ,[TipoUsuarioID]
                              ,[UsuarioNombre]
                              ,[UsuarioContraseña]
                              ,[FechaRegistro]
                              ,[FailedAttempts]
                              ,[Estado]
                              ,[MailJob]
                          FROM [dbo].[SEG_Usuario]
						  left join SEG_Empleado on SEG_Empleado.EmpleadoID = SEG_Usuario.EmpleadoID
	                       where UsuarioID=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", usuarioId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                segUsuario.UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]);
                                segUsuario.EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]);
                                segUsuario.TipoUsuarioID = ManejoNulos.ManageNullInteger(dr["TipoUsuarioID"].Trim());
                                segUsuario.UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"].Trim());
                                segUsuario.UsuarioContraseña = ManejoNulos.ManageNullStr(dr["UsuarioContraseña"].Trim());
                                segUsuario.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                segUsuario.FailedAttempts = ManejoNulos.ManageNullInteger(dr["FailedAttempts"]);
                                segUsuario.MailJob = ManejoNulos.ManageNullStr(dr["MailJob"].Trim());
                                segUsuario.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return segUsuario;
        }

        public bool UsuarioActualizarUsuarioNombreJson(int UsuarioID, string UsuarioNombre)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[SEG_Usuario]
                        SET 
                       UsuarioNombre = @p2
                       WHERE usuarioID = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", UsuarioID);
                    query.Parameters.AddWithValue("@p2", UsuarioNombre);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }

        public (string respuesta,ClaseError error) UsuarioEditarTokenAccesoIntranetJson(SEG_UsuarioEntidad usuario)
        {
            string response = "";
            ClaseError error = new ClaseError();
            string consulta = @"UPDATE [dbo].[SEG_Usuario]
                        SET 
                        OUTPUT INSERTED.Token
                       UsuarioToken = @p2
                       WHERE usuarioID = @p0;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(usuario.UsuarioToken));
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(usuario.UsuarioID));
                    //query.ExecuteNonQuery();
                    response = query.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (response,error);
        }
        public List<SEG_UsuarioEntidad> UsuarioListadoxSalasJson(string querySalas) {
            List<SEG_UsuarioEntidad> lista = new List<SEG_UsuarioEntidad>();
            string consulta = @"SELECT DISTINCT
                             (emp.ApellidosPaterno+' '+emp.ApellidosMaterno+', '+emp.Nombres) nombreEmpleado
                              ,seg.[UsuarioID]
                              ,seg.[EmpleadoID]
                              ,[TipoUsuarioID]
                              ,[UsuarioNombre]
                              ,[UsuarioContraseña]
                              ,seg.[FechaRegistro]
                              ,[FailedAttempts]
                              ,seg.[Estado]
                              ,[Token],emp.[DOI]
                              --,usa.SalaId
                                FROM [SEG_Usuario] seg
                                join SEG_Empleado emp on emp.EmpleadoID=seg.EmpleadoID 
                                join UsuarioSala usa on seg.UsuarioID=usa.UsuarioId 
                                WHERE usa.SalaId IN ("+querySalas+") order by UsuarioID Desc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var usuario = new SEG_UsuarioEntidad {
                                NombreEmpleado = ManejoNulos.ManageNullStr(dr["nombreEmpleado"].Trim()),
                                UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]),
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"].Trim()),
                                TipoUsuarioID = ManejoNulos.ManageNullInteger(dr["TipoUsuarioID"].Trim()),
                                UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"].Trim()),
                                UsuarioContraseña = ManejoNulos.ManageNullStr(dr["UsuarioContraseña"].Trim()),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"].Trim()),
                                FailedAttempts = ManejoNulos.ManageNullInteger(dr["FailedAttempts"].Trim()),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim()),
                                UsuarioToken = ManejoNulos.ManageNullStr(dr["Token"].Trim()),
                                DOI = ManejoNulos.ManageNullStr(dr["DOI"].Trim())
                            };

                            lista.Add(usuario);
                        }
                    }

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }
    }
}
