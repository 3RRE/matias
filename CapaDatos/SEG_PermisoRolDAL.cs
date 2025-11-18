using CapaDatos.Utilitarios;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace CapaDatos {
    public class SEG_PermisoRolDAL {
        string _conexion = string.Empty;
        public SEG_PermisoRolDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<SEG_PermisoRolEntidad> GetPermisoRolUsuario(int rol_id, string accion) {
            List<SEG_PermisoRolEntidad> lista = new List<SEG_PermisoRolEntidad>();
            string consulta = @"SELECT [WEB_PRolID],[WEB_RolID],SEG_PermisoRol.WEB_PermID,[WEB_PRolFechaRegistro]
                                                    FROM [dbo].[SEG_PermisoRol] 
                                                    left join [SEG_Permiso] on [SEG_Permiso].[WEB_PermID]=[SEG_PermisoRol].[WEB_PermID]
                        where [SEG_PermisoRol].WEB_RolID = @p0 and [SEG_Permiso].[WEB_PermNombre]=@p1";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rol_id);
                    query.Parameters.AddWithValue("@p1", accion);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var webPermisoRol = new SEG_PermisoRolEntidad {
                                WEB_PRolID = ManejoNulls.ManageNullInteger(dr["WEB_PRolID"]),
                                WEB_PermID = ManejoNulls.ManageNullInteger(dr["WEB_PermID"]),
                                WEB_RolID = ManejoNulls.ManageNullInteger(dr["WEB_RolID"]),
                                WEB_PRolFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_PRolFechaRegistro"].Trim()),
                            };

                            lista.Add(webPermisoRol);
                        }
                    }

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }

        public bool GuardarPermisoRol(SEG_PermisoRolEntidad permisoRol) {
            bool respuesta = false;
            string consulta = @"INSERT INTO [dbo].[SEG_PermisoRol]
           ([WEB_PermID],[WEB_RolID],[WEB_PRolFechaRegistro])VALUES(@p0,@p1,@p2)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulls.ManageNullStr(permisoRol.WEB_PermID) == String.Empty ? SqlString.Null : Convert.ToString(permisoRol.WEB_PermID));
                    query.Parameters.AddWithValue("@p1", ManejoNulls.ManageNullStr(permisoRol.WEB_RolID) == String.Empty ? SqlString.Null : Convert.ToString(permisoRol.WEB_RolID));
                    query.Parameters.AddWithValue("@p2", DateTime.Now);
                    query.ExecuteNonQuery();

                    respuesta = true;
                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }

        public List<SEG_PermisoRolEntidad> GetPermisoRol() {
            List<SEG_PermisoRolEntidad> lista = new List<SEG_PermisoRolEntidad>();
            string consulta = @"select  [WEB_PRolID]
                                      ,[WEB_PermID]
                                      ,[WEB_RolID]
                                      ,[WEB_PRolFechaRegistro]
                                  FROM [dbo].[SEG_PermisoRol]
";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var webPermisoRol = new SEG_PermisoRolEntidad {
                                WEB_PRolID = ManejoNulls.ManageNullInteger(dr["WEB_PRolID"]),
                                WEB_PermID = ManejoNulls.ManageNullInteger(dr["WEB_PermID"]),
                                WEB_RolID = ManejoNulls.ManageNullInteger(dr["WEB_RolID"]),
                                WEB_PRolFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_PRolFechaRegistro"].Trim()),
                            };

                            lista.Add(webPermisoRol);
                        }
                    }

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }

        public SEG_PermisoRolEntidad GetPermisoRolId(int permisoRolid) {
            SEG_PermisoRolEntidad webPermisoRol = new SEG_PermisoRolEntidad();
            string consulta = @"SELECT [WEB_PRolID]
                                  ,[WEB_PermID]
                                  ,[WEB_RolID]
                                  ,[WEB_PRolFechaRegistro]
                              FROM [dbo].[SEG_PermisoRol] where WEB_PRolID =@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisoRolid);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                webPermisoRol.WEB_PRolID = ManejoNulls.ManageNullInteger(dr["WEB_PRolID"]);
                                webPermisoRol.WEB_PermID = ManejoNulls.ManageNullInteger(dr["WEB_PermID"]);
                                webPermisoRol.WEB_RolID = ManejoNulls.ManageNullInteger(dr["WEB_RolID"].Trim());
                                webPermisoRol.WEB_PRolFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_PRolFechaRegistro"].Trim());
                            }
                        }
                    }

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return webPermisoRol;
        }

        public List<SEG_PermisoRolEntidad> GetPermisoRolrolid(int rolid) {
            List<SEG_PermisoRolEntidad> lista = new List<SEG_PermisoRolEntidad>();
            string consulta = @"SELECT [WEB_PRolID]
                                  ,[WEB_PermID]
                                  ,[WEB_RolID]
                                  ,[WEB_PRolFechaRegistro]
                              FROM [dbo].[SEG_PermisoRol] where WEB_RolID =@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rolid);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                var webPermisoRol = new SEG_PermisoRolEntidad {
                                    WEB_PRolID = ManejoNulls.ManageNullInteger(dr["WEB_PRolID"]),
                                    WEB_PermID = ManejoNulls.ManageNullInteger(dr["WEB_PermID"]),
                                    WEB_RolID = ManejoNulls.ManageNullInteger(dr["WEB_RolID"]),
                                    WEB_PRolFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_PRolFechaRegistro"].Trim()),
                                };

                                lista.Add(webPermisoRol);
                            }
                        }
                    }

                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }
        public bool ActualizarPermisoRol(SEG_PermisoRolEntidad permisoRol) {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[SEG_PermisoRol]
                            SET [WEB_PermID] = @p1,[WEB_RolID] = @p2
                            WHERE WEB_PRolID = @p0";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisoRol.WEB_PRolID);
                    query.Parameters.AddWithValue("@p1", permisoRol.WEB_PermID);
                    query.Parameters.AddWithValue("@p2", permisoRol.WEB_RolID);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }


        public bool EliminarPermisoRol(int permisoid, int rolid) {
            bool respuesta = false;
            string consulta = @"DELETE FROM SEG_PermisoRol WHERE WEB_PermID = @p0 and WEB_RolID = @p1";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisoid);
                    query.Parameters.AddWithValue("@p1", rolid);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return respuesta;
        }

        public bool AutorizedControllerAction(int rolId, string controllerName, string actionName) {
            bool response = false;

            List<SEG_PermisoRolEntidad> list = new List<SEG_PermisoRolEntidad>();

            string query = @"
            SELECT
	            permisorol.WEB_PRolID,
	            permisorol.WEB_PermID,
	            permisorol.WEB_RolID,
	            permisorol.WEB_PRolFechaRegistro
            FROM SEG_PermisoRol permisorol
            LEFT JOIN SEG_Permiso permiso ON permiso.WEB_PermID = permisorol.WEB_PermID
            WHERE permisorol.WEB_RolID = @p1 AND permiso.WEB_PermControlador = @p2 AND permiso.WEB_PermNombre = @p3
            ";

            try {
                using(SqlConnection conecction = new SqlConnection(_conexion)) {
                    conecction.Open();

                    SqlCommand command = new SqlCommand(query, conecction);
                    command.Parameters.AddWithValue("@p1", rolId);
                    command.Parameters.AddWithValue("@p2", controllerName);
                    command.Parameters.AddWithValue("@p3", actionName);

                    using(SqlDataReader data = command.ExecuteReader()) {
                        if(data.HasRows) {
                            while(data.Read()) {
                                SEG_PermisoRolEntidad permisoRol = new SEG_PermisoRolEntidad {
                                    WEB_PRolID = ManejoNulls.ManageNullInteger(data["WEB_PRolID"]),
                                    WEB_PermID = ManejoNulls.ManageNullInteger(data["WEB_PermID"]),
                                    WEB_RolID = ManejoNulls.ManageNullInteger(data["WEB_RolID"]),
                                    WEB_PRolFechaRegistro = ManejoNulls.ManageNullDate(data["WEB_PRolFechaRegistro"].Trim()),
                                };

                                list.Add(permisoRol);
                            }
                        }
                    }

                    if(list.Count > 0) {
                        response = true;
                    }
                }
            } catch(Exception exception) {
                Trace.WriteLine("" + exception.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return response;
        }

        public List<SEG_RolEntidad> GetRolesPorAccion(string accion) {
            List<SEG_RolEntidad> items = new List<SEG_RolEntidad>();
            string consulta = @"
                SELECT r.* 
                FROM SEG_Rol AS r
                INNER JOIN SEG_PermisoRol AS pr ON pr.WEB_RolID = r.WEB_RolID
                INNER JOIN SEG_Permiso AS p ON p.WEB_PermID = pr.WEB_PermID
                WHERE r.WEB_RolEstado = 1 AND p.WEB_PermNombre = @NombreAccion
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@NombreAccion", accion);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjetoRol(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        private SEG_RolEntidad ConstruirObjetoRol(SqlDataReader dr) {
            return new SEG_RolEntidad {
                WEB_RolID = ManejoNulls.ManageNullInteger(dr["WEB_RolID"]),
                WEB_RolNombre = ManejoNulls.ManageNullStr(dr["WEB_RolNombre"]),
                WEB_RolDescripcion = ManejoNulls.ManageNullStr(dr["WEB_RolDescripcion"]),
                WEB_RolEstado = ManejoNulls.ManageNullStr(dr["WEB_RolEstado"]),
                WEB_RolFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_RolFechaRegistro"]),
            };
        }
    }
}
