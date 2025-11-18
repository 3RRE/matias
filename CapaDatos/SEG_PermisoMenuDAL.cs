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
    public class SEG_PermisoMenuDAL
    {
        string _conexion = string.Empty;
        public SEG_PermisoMenuDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public bool GuardarPermisoMenu(SEG_PermisoMenuEntidad permisoMENU)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO [dbo].[SEG_PermisoMenu]
           ([WEB_PMeNombre],[WEB_PMeFechaRegistro],[WEB_PMeDataMenu],[WEB_RolID],WEB_PMeEstado,WEB_ModuloNombre)VALUES(@p0,@p1,@p2,@p3,@p4,@p5)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulls.ManageNullStr(permisoMENU.WEB_PMeNombre) == String.Empty ? SqlString.Null : Convert.ToString(permisoMENU.WEB_PMeNombre));
                    query.Parameters.AddWithValue("@p1", DateTime.Now);
                    query.Parameters.AddWithValue("@p2", ManejoNulls.ManageNullStr(permisoMENU.WEB_PMeDataMenu) == String.Empty ? SqlString.Null : Convert.ToString(permisoMENU.WEB_PMeDataMenu));
                    query.Parameters.AddWithValue("@p3", ManejoNulls.ManageNullStr(permisoMENU.WEB_RolID) == String.Empty ? SqlString.Null : Convert.ToString(permisoMENU.WEB_RolID));
                    query.Parameters.AddWithValue("@p4", ManejoNulls.ManageNullStr(permisoMENU.WEB_PMeEstado) == String.Empty ? SqlString.Null : Convert.ToString(permisoMENU.WEB_PMeEstado));
                    query.Parameters.AddWithValue("@p5", ManejoNulls.ManageNullStr(permisoMENU.WEB_ModuloNombre) == String.Empty ? SqlString.Null : Convert.ToString(permisoMENU.WEB_ModuloNombre));
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

        public List<SEG_PermisoMenuEntidad> GetPermisoMenu()
        {
            List<SEG_PermisoMenuEntidad> lista = new List<SEG_PermisoMenuEntidad>();
            string consulta = @"SELECT [WEB_PMeID]
                                  ,[WEB_PMeNombre]
                                  ,[WEB_PMeFechaRegistro]
                                  ,[WEB_PMeDataMenu]
                                  ,[WEB_RolID]
                              FROM [dbo].[SEG_PermisoMenu]";
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
                            var webPermisoMenu = new SEG_PermisoMenuEntidad
                            {
                                WEB_PMeID = ManejoNulls.ManageNullInteger(dr["WEB_PMeID"]),
                                WEB_PMeNombre = ManejoNulls.ManageNullStr(dr["WEB_PMeNombre"]),
                                WEB_RolID = ManejoNulls.ManageNullInteger(dr["WEB_RolID"]),
                                WEB_PMeFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_PMeFechaRegistro"].Trim()),
                                WEB_PMeDataMenu = ManejoNulls.ManageNullStr(dr["WEB_PMeDataMenu"])
                            };

                            lista.Add(webPermisoMenu);
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

        public SEG_PermisoMenuEntidad GetPermisoMenuId(int permisoMenuId)
        {
            SEG_PermisoMenuEntidad webPermisoMenu = new SEG_PermisoMenuEntidad();
            string consulta = @"SELECT [WEB_PMeID]
                                  ,[WEB_PMeNombre]
                                  ,[WEB_PMeFechaRegistro]
                                  ,[WEB_PMeDataMenu]
                                  ,[WEB_RolID]
                              FROM [dbo].[SEG_PermisoMenu] where WEB_PMeID =@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisoMenuId);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                webPermisoMenu.WEB_PMeID = ManejoNulls.ManageNullInteger(dr["WEB_PMeID"]);
                                webPermisoMenu.WEB_PMeNombre = ManejoNulls.ManageNullStr(dr["WEB_PMeNombre"]);
                                webPermisoMenu.WEB_RolID = ManejoNulls.ManageNullInteger(dr["WEB_RolID"]);
                                webPermisoMenu.WEB_PMeFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_PMeFechaRegistro"].Trim());
                                webPermisoMenu.WEB_PMeDataMenu = ManejoNulls.ManageNullStr(dr["WEB_PMeDataMenu"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return webPermisoMenu;
        }

        public List<SEG_PermisoMenuEntidad> GetPermisoMenuRolId(int rolId)
        {
            List<SEG_PermisoMenuEntidad> lista = new List<SEG_PermisoMenuEntidad>();
            string consulta = @"SELECT [WEB_PMeID]
                                  ,[WEB_PMeNombre]
                                  ,[WEB_PMeFechaRegistro]
                                  ,[WEB_PMeDataMenu]
                                  ,[WEB_RolID]
                              FROM [dbo].[SEG_PermisoMenu] where WEB_RolID =@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rolId);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var webPermisoMenu = new SEG_PermisoMenuEntidad
                                {
                                    WEB_PMeID = ManejoNulls.ManageNullInteger(dr["WEB_PMeID"]),
                                    WEB_PMeNombre = ManejoNulls.ManageNullStr(dr["WEB_PMeNombre"]),
                                    WEB_RolID = ManejoNulls.ManageNullInteger(dr["WEB_RolID"]),
                                    WEB_PMeFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_PMeFechaRegistro"].Trim()),
                                    WEB_PMeDataMenu = ManejoNulls.ManageNullStr(dr["WEB_PMeDataMenu"])
                                };

                                lista.Add(webPermisoMenu);
                            }
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
        public List<SEG_PermisoMenuEntidad> GetPermisoFechaMax()
        {
            List<SEG_PermisoMenuEntidad> lista = new List<SEG_PermisoMenuEntidad>();
            string consulta = @"SELECT 
                                         (select Max(b.WEB_PMeFechaRegistro) from SEG_PermisoMenu b) fecha
                                FROM [dbo].[SEG_PermisoMenu] a
                                group by a.[WEB_ModuloNombre]";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var webPermisoMenu = new SEG_PermisoMenuEntidad
                                {
                                    WEB_ModuloNombre = ManejoNulls.ManageNullStr(dr["WEB_ModuloNombre"]),
                                    WEB_PMeFechaRegistro = Convert.ToDateTime(dr["fecha"]),
                                };

                                lista.Add(webPermisoMenu);
                            }
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

        public List<SEG_PermisoMenuEntidad> GetPermisoMenuIn(string datamenu)
        {
            List<SEG_PermisoMenuEntidad> lista = new List<SEG_PermisoMenuEntidad>();
            string consulta = @"SELECT [WEB_PMeID]
                                  ,[WEB_PMeNombre]
                                  ,[WEB_PMeFechaRegistro]
                                  ,[WEB_PMeDataMenu]
                                  ,[WEB_RolID]
                              FROM [dbo].[SEG_PermisoMenu] " + datamenu;
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var webPermisoMenu = new SEG_PermisoMenuEntidad
                                {
                                    WEB_PMeID = ManejoNulls.ManageNullInteger(dr["WEB_PMeID"]),
                                    WEB_PMeNombre = ManejoNulls.ManageNullStr(dr["WEB_PMeNombre"]),
                                    WEB_RolID = ManejoNulls.ManageNullInteger(dr["WEB_RolID"]),
                                    WEB_PMeFechaRegistro = Convert.ToDateTime(dr["WEB_PMeFechaRegistro"]),
                                    WEB_PMeDataMenu = ManejoNulls.ManageNullStr(dr["WEB_PMeDataMenu"])
                                };

                                lista.Add(webPermisoMenu);
                            }
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

        public bool ActualizarPermisoMenu(SEG_PermisoMenuEntidad permisoMenu)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[SEG_PermisoMenu]
                            SET [WEB_PMeNombre] = @p1
                                ,[WEB_PMeFechaRegistro] = @p2
                                ,[WEB_PMeDataMenu] = @p3
                                ,[WEB_RolID] = @p4
                            WHERE WEB_PMeID = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisoMenu.WEB_PMeID);
                    query.Parameters.AddWithValue("@p1", permisoMenu.WEB_PMeNombre);
                    query.Parameters.AddWithValue("@p2", permisoMenu.WEB_PMeFechaRegistro);
                    query.Parameters.AddWithValue("@p3", permisoMenu.WEB_PMeDataMenu);
                    query.Parameters.AddWithValue("@p4", permisoMenu.WEB_RolID);
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


        public bool EliminarPermisoMenu(string permisoDataMenu, int rolid)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM SEG_PermisoMenu WHERE WEB_PMeDataMenu = @p0 and WEB_RolID = @p1";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", permisoDataMenu);
                    query.Parameters.AddWithValue("@p1", rolid);
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
    }
}
