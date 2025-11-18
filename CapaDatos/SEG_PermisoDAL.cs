using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using CapaEntidad;
using CapaDatos.Utilitarios;


namespace CapaDatos
{
    public class SEG_PermisoDAL
    {
        string _conexion = string.Empty;
        public SEG_PermisoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public bool GuardarPermiso(SEG_PermisoEntidad permiso)
        {
            bool respuesta = false;
            string consulta = @"if NOT exists(select * from SEG_Permiso  WITH (UPDLOCK, HOLDLOCK) where WEB_PermNombre=@p0 AND [Web_PermControlador]=@p2) 
            INSERT INTO [dbo].[SEG_Permiso]
           ([WEB_PermNombre],[WEB_PermTipo],[WEB_PermControlador],[WEB_PermDescripcion],[WEB_PermEstado],[WEB_PermFechaRegistro], [WEB_ModuloNombre])
            VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6)
                 else 
			  update [dbo].[SEG_Permiso]
			  SET
			        [WEB_ModuloNombre]=@p6,
                    [WEB_PermTipo]=@p1
			    WHERE [WEB_PermNombre] = @p0 AND [Web_PermControlador]=@p2
                ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulls.ManageNullStr(permiso.WEB_PermNombre) == String.Empty ? SqlString.Null : Convert.ToString(permiso.WEB_PermNombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulls.ManageNullStr(permiso.WEB_PermTipo) == String.Empty ? SqlString.Null : Convert.ToString(permiso.WEB_PermTipo));
                    query.Parameters.AddWithValue("@p2", ManejoNulls.ManageNullStr(permiso.WEB_PermControlador) == String.Empty ? SqlString.Null : Convert.ToString(permiso.WEB_PermControlador));
                    query.Parameters.AddWithValue("@p3", ManejoNulls.ManageNullStr(permiso.WEB_PermDescripcion) == String.Empty ? SqlString.Null : Convert.ToString(permiso.WEB_PermDescripcion));
                    query.Parameters.AddWithValue("@p4", permiso.WEB_PermEstado);

                    query.Parameters.AddWithValue("@p6", ManejoNulls.ManageNullStr(permiso.WEB_ModuloNombre) == String.Empty ? SqlString.Null : Convert.ToString(permiso.WEB_ModuloNombre));

                    query.Parameters.AddWithValue("@p5", DateTime.Now);
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

        public bool BorrarPermiso(string permisonombre, string controlador)
        {
            bool respuesta = false;
            string consulta = @"
                    declare @id int;
                    set @id =(select  [WEB_PermID] from  [SEG_Permiso]   WHERE [WEB_PermNombre] = @p0 AND [Web_PermControlador]=@p1)
                    Delete from [SEG_Permiso] WHERE [WEB_PermNombre] = @p0 AND [Web_PermControlador]=@p1
                    Delete from [dbo].[SEG_PermisoRol] WHERE [WEB_PermID] = @id";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisonombre);
                    query.Parameters.AddWithValue("@p1", controlador);
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
        public bool GetPermisoId(string permisonombre)
        {
            bool respuesta = false;

            List<SEG_PermisoEntidad> lista = new List<SEG_PermisoEntidad>();
            string consulta = @"SELECT [WEB_PermID]
                              ,[WEB_PermNombre]
                                ,[WEB_PermNombreR]
                              ,[WEB_PermTipo]
                              ,[WEB_PermControlador]
                              ,[WEB_PermDescripcion]
                              ,[WEB_PermEstado]
                              ,[WEB_PermFechaRegistro]
                              FROM [dbo].[SEG_Permiso] where WEB_PermNombre = '" + permisonombre + "'";
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
                            respuesta = true;
                            var webPermiso = new SEG_PermisoEntidad
                            {
                                WEB_PermID = ManejoNulls.ManageNullInteger(dr["WEB_PermID"]),
                                WEB_PermNombre = ManejoNulls.ManageNullStr(dr["WEB_PermNombre"]),
                                WEB_PermNombreR = ManejoNulls.ManageNullStr(dr["WEB_PermNombreR"]),
                                WEB_PermTipo = ManejoNulls.ManageNullStr(dr["WEB_PermTipo"].Trim()),
                                WEB_PermControlador = ManejoNulls.ManageNullStr(dr["WEB_PermControlador"].Trim()),
                                WEB_PermDescripcion = ManejoNulls.ManageNullStr(dr["WEB_PermDescripcion"].Trim()),
                                WEB_PermEstado = ManejoNulls.ManageNullStr(dr["WEB_PermEstado"].Trim()),
                                WEB_PermFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_PermFechaRegistro"].Trim())
                            };

                            lista.Add(webPermiso);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }

        public List<SEG_PermisoEntidad> GetPermisos()
        {
            List<SEG_PermisoEntidad> lista = new List<SEG_PermisoEntidad>();
            string consulta = @"SELECT [WEB_PermID]
                              ,[WEB_PermNombre]
                              ,[WEB_PermNombreR]
                              ,[WEB_PermTipo]
                              ,[WEB_PermControlador]
                              ,[WEB_PermDescripcion]
                              ,[WEB_PermEstado]
                              ,[WEB_PermFechaRegistro]
                              FROM [dbo].[SEG_Permiso] order by WEB_PermControlador,WEB_PermNombre ASC";
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
                            var webPermiso = new SEG_PermisoEntidad
                            {
                                WEB_PermID = ManejoNulls.ManageNullInteger(dr["WEB_PermID"]),
                                WEB_PermNombre = ManejoNulls.ManageNullStr(dr["WEB_PermNombre"]),
                                WEB_PermNombreR = ManejoNulls.ManageNullStr(dr["WEB_PermNombreR"]),
                                WEB_PermTipo = ManejoNulls.ManageNullStr(dr["WEB_PermTipo"].Trim()),
                                WEB_PermControlador = ManejoNulls.ManageNullStr(dr["WEB_PermControlador"].Trim()),
                                WEB_PermDescripcion = ManejoNulls.ManageNullStr(dr["WEB_PermDescripcion"].Trim()),
                                WEB_PermEstado = ManejoNulls.ManageNullStr(dr["WEB_PermEstado"].Trim()),
                                WEB_PermFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_PermFechaRegistro"].Trim())
                            };

                            lista.Add(webPermiso);
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

        public List<SEG_PermisoEntidad> GetPermisosActivos()
        {
            List<SEG_PermisoEntidad> lista = new List<SEG_PermisoEntidad>();
            string consulta = @"SELECT [WEB_PermID]
                              ,[WEB_PermNombre]
                              ,[WEB_PermNombreR]
                              ,[WEB_PermTipo]
                              ,[WEB_PermControlador]
                              ,[WEB_PermDescripcion]
                              ,[WEB_PermEstado]
                              ,[WEB_PermFechaRegistro]
                              FROM [dbo].[SEG_Permiso]
                            where WEB_PermEstado = 1
                            order by WEB_PermControlador,WEB_PermNombre ASC";
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
                            var webPermiso = new SEG_PermisoEntidad
                            {
                                WEB_PermID = ManejoNulls.ManageNullInteger(dr["WEB_PermID"]),
                                WEB_PermNombre = ManejoNulls.ManageNullStr(dr["WEB_PermNombre"]),
                                WEB_PermNombreR = ManejoNulls.ManageNullStr(dr["WEB_PermNombreR"]),
                                WEB_PermTipo = ManejoNulls.ManageNullStr(dr["WEB_PermTipo"].Trim()),
                                WEB_PermControlador = ManejoNulls.ManageNullStr(dr["WEB_PermControlador"].Trim()),
                                WEB_PermDescripcion = ManejoNulls.ManageNullStr(dr["WEB_PermDescripcion"].Trim()),
                                WEB_PermEstado = ManejoNulls.ManageNullStr(dr["WEB_PermEstado"].Trim()),
                                WEB_PermFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_PermFechaRegistro"].Trim())
                            };

                            lista.Add(webPermiso);
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

        public bool ActualizarEstadoPermiso(int web_PermId, int estado)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [SEG_Permiso]
                        SET [WEB_PermEstado] =@p1
                       WHERE [WEB_PermID] = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", web_PermId);
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

        public bool ActualizarDescripcionPermiso(int web_PermId, string descripcion)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [SEG_Permiso]
                        SET [WEB_PermDescripcion] =@p1
                       WHERE [WEB_PermID] = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", web_PermId);
                    query.Parameters.AddWithValue("@p1", descripcion);
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

        public bool ActualizarNombrePermiso(int web_PermId, string nombre)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [SEG_Permiso]
                        SET [WEB_PermNombreR] =@p1
                       WHERE [WEB_PermID] = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", web_PermId);
                    query.Parameters.AddWithValue("@p1", nombre);
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

        public List<SEG_PermisoEntidad> PermissionsByController(string controllerName)
        {
            List<SEG_PermisoEntidad> lista = new List<SEG_PermisoEntidad>();

            string query = @"SELECT [WEB_PermID]
                              ,[WEB_PermNombre]
                              ,[WEB_PermNombreR]
                              ,[WEB_PermTipo]
                              ,[WEB_PermControlador]
                              ,[WEB_PermDescripcion]
                              ,[WEB_PermEstado]
                              ,[WEB_PermFechaRegistro]
                              FROM [dbo].[SEG_Permiso] WHERE WEB_PermControlador = @p0 ORDER BY WEB_PermNombre ASC";
            try
            {
                using (var connection = new SqlConnection(_conexion))
                {
                    connection.Open();
                    var sqlCommand = new SqlCommand(query, connection);
                    sqlCommand.Parameters.AddWithValue("@p0", controllerName);
                    using (var dr = sqlCommand.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var webPermiso = new SEG_PermisoEntidad
                            {
                                WEB_PermID = ManejoNulls.ManageNullInteger(dr["WEB_PermID"]),
                                WEB_PermNombre = ManejoNulls.ManageNullStr(dr["WEB_PermNombre"]),
                                WEB_PermNombreR = ManejoNulls.ManageNullStr(dr["WEB_PermNombreR"]),
                                WEB_PermTipo = ManejoNulls.ManageNullStr(dr["WEB_PermTipo"].Trim()),
                                WEB_PermControlador = ManejoNulls.ManageNullStr(dr["WEB_PermControlador"].Trim()),
                                WEB_PermDescripcion = ManejoNulls.ManageNullStr(dr["WEB_PermDescripcion"].Trim()),
                                WEB_PermEstado = ManejoNulls.ManageNullStr(dr["WEB_PermEstado"].Trim()),
                                WEB_PermFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_PermFechaRegistro"].Trim())
                            };

                            lista.Add(webPermiso);
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
    }
}
