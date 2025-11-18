using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class UsuarioEncriptacionDAL
    {
        string _conexion = string.Empty;
        public UsuarioEncriptacionDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }


        public UsuarioEncriptacionEntidad UsuarioEncriptacionIDObtenerJson(int EmpleadoId)
        {
            UsuarioEncriptacionEntidad usuencriptacion = new UsuarioEncriptacionEntidad();
            string consulta = @"SELECT [Id]
                                  ,[EmpleadoId]
                                  ,[UsuarioNombre]
                                  ,[UsuarioPassword]
                                  ,[FechaIni]
                                  ,[FechaFin]
                                  ,[Estado]
                                  ,[FechaRegistro]
                              FROM [dbo].[UsuarioEncriptacion]
                                where [EmpleadoId] = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", EmpleadoId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                usuencriptacion.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                usuencriptacion.EmpleadoId = ManejoNulos.ManageNullInteger(dr["EmpleadoId"]);
                                usuencriptacion.UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]);
                                usuencriptacion.UsuarioPassword = ManejoNulos.ManageNullStr(dr["UsuarioPassword"]);
                                usuencriptacion.FechaIni = ManejoNulos.ManageNullDate(dr["FechaIni"]);
                                usuencriptacion.FechaFin = ManejoNulos.ManageNullDate(dr["FechaFin"]);
                                usuencriptacion.Estado = ManejoNulos.ManegeNullBool(dr["Estado"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return usuencriptacion;
        }

        public bool UsuarioEncriptacionHistorialInsertar(int usuarioEncriptacionID, int tipoAcceso)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO [dbo].[UsuarioEncriptacionHistorial]
                               (
                                 [UsuarioEncriptacionID]
                                ,[TipoAcceso]
                                ,[FechaAcceso]
                                )        
                            VALUES(
                                    @UsuarioEncriptacionID
                                    ,@TipoAcceso
                                    ,getdate()
                                  )";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@UsuarioEncriptacionID", ManejoNulos.ManageNullInteger(usuarioEncriptacionID));
                    query.Parameters.AddWithValue("@TipoAcceso", ManejoNulos.ManageNullInteger(tipoAcceso));
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

        public UsuarioEncriptacionEntidad UsuarioCoincidenciaJsonPrograma(string usuario)
        {
            var segUsuario = new UsuarioEncriptacionEntidad();
            string consulta = @"select * from [UsuarioEncriptacion] where [UsuarioNombre]=@usuario";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@usuario", usuario);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                segUsuario.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                segUsuario.EmpleadoId = ManejoNulos.ManageNullInteger(dr["EmpleadoId"]);
                                segUsuario.UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"].Trim());
                                segUsuario.UsuarioPassword = ManejoNulos.ManageNullStr(dr["UsuarioPassword"].Trim());
                                segUsuario.FechaIni = ManejoNulos.ManageNullDate(dr["FechaIni"]);
                                segUsuario.FechaFin = ManejoNulos.ManageNullDate(dr["FechaFin"]);
                                segUsuario.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                segUsuario.Estado = ManejoNulos.ManegeNullBool(dr["Estado"]);
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

        public List<TecnicoUsuarioEncriptado> TecnicoListarJson()
        {
            List<TecnicoUsuarioEncriptado> lista = new List<TecnicoUsuarioEncriptado>();
            string consulta = @"select em.EmpleadoId,(em.Nombres +' ' + em.ApellidosPaterno + ' '+ em.ApellidosMaterno) Nombre
                                from SEG_Empleado em where em.EmpleadoId not in (select ue.EmpleadoId from UsuarioEncriptacion ue)";
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
                            var tecnicousuario = new TecnicoUsuarioEncriptado
                            {
                                Id = ManejoNulos.ManageNullInteger(dr["EmpleadoId"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"])
                            };
                            lista.Add(tecnicousuario);
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

        public bool UsuarioEncriptacionRenovarContraseniaJson(UsuarioEncriptacionEntidad usuarioencriptacion)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[UsuarioEncriptacion]
                               SET [FechaIni] = @p1
                                  ,[FechaFin] = @p2
                                  ,[FechaRegistro] = @p3
                                  ,[UsuarioPassword] = @p4
                             WHERE EmpleadoId = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(usuarioencriptacion.EmpleadoId));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullDate(usuarioencriptacion.FechaIni));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(usuarioencriptacion.FechaFin));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(usuarioencriptacion.FechaRegistro));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(usuarioencriptacion.UsuarioPassword));
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

        public bool UsuarioEncriptacionInsertarJson(UsuarioEncriptacionEntidad usuarioencriptacion)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO [dbo].[UsuarioEncriptacion]
                                ([EmpleadoId]
                                ,[UsuarioNombre]
                                ,[UsuarioPassword]
                                ,[FechaIni]
                                ,[FechaFin]
                                ,[Estado]
                                ,[FechaRegistro])
                            VALUES
                                (@p0,@p1,@p2,@p3,@p4,@p5,@p6)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(usuarioencriptacion.EmpleadoId));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(usuarioencriptacion.UsuarioNombre.Trim()));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(usuarioencriptacion.UsuarioPassword));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(usuarioencriptacion.FechaIni));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(usuarioencriptacion.FechaFin));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManegeNullBool(usuarioencriptacion.Estado));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(usuarioencriptacion.FechaRegistro));
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

        public bool UsuarioEncriptacionEditarJson(UsuarioEncriptacionEntidad usuarioencriptacion)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[UsuarioEncriptacion]
                                SET  
                                    [UsuarioNombre] = @p1
                                    ,[FechaIni] = @p2
                                    ,[FechaFin] = @p3
                                    ,[Estado] = @p4
                                WHERE [Id] = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(usuarioencriptacion.Id));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(usuarioencriptacion.UsuarioNombre.Trim()));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(usuarioencriptacion.FechaIni));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(usuarioencriptacion.FechaFin));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManegeNullBool(usuarioencriptacion.Estado));
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

        public UsuarioEncriptacionEntidad VerificarUsuarioNombreEncriptacionJson(string UsuarioNombre)
        {
            UsuarioEncriptacionEntidad usuencriptacion = new UsuarioEncriptacionEntidad();
            string consulta = @"SELECT [Id]
                                      ,[EmpleadoId]
                                      ,[UsuarioNombre]
                                      ,[UsuarioPassword]
                                      ,[FechaIni]
                                      ,[FechaFin]
                                      ,[Estado]
                                      ,[FechaRegistro]
                                  FROM [dbo].[UsuarioEncriptacion]
                                where [UsuarioNombre] = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", UsuarioNombre);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                usuencriptacion.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                usuencriptacion.EmpleadoId = ManejoNulos.ManageNullInteger(dr["EmpleadoId"]);
                                usuencriptacion.UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]);
                                usuencriptacion.UsuarioPassword = ManejoNulos.ManageNullStr(dr["UsuarioPassword"]);
                                usuencriptacion.FechaIni = ManejoNulos.ManageNullDate(dr["FechaIni"]);
                                usuencriptacion.FechaFin = ManejoNulos.ManageNullDate(dr["FechaFin"]);
                                usuencriptacion.Estado = ManejoNulos.ManegeNullBool(dr["Estado"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return usuencriptacion;
        }

    }
}
