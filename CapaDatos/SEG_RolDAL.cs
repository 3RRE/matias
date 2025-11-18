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
    public class SEG_RolDAL
    {
        string _conexion = string.Empty;
        public SEG_RolDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public bool GuardarRol(SEG_RolEntidad rol)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO [dbo].[SEG_Rol]
           ([WEB_RolNombre],[WEB_RolDescripcion],[WEB_RolEstado],[WEB_RolFechaRegistro])
            VALUES(@p0,@p1,@p2,@p3)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulls.ManageNullStr(rol.WEB_RolNombre) == String.Empty ? SqlString.Null : Convert.ToString(rol.WEB_RolNombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulls.ManageNullStr(rol.WEB_RolDescripcion) == String.Empty ? SqlString.Null : Convert.ToString(rol.WEB_RolDescripcion));
                    query.Parameters.AddWithValue("@p2", 1);
                    query.Parameters.AddWithValue("@p3", DateTime.Now);
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

        public List<SEG_RolEntidad> GetRoles()
        {
            List<SEG_RolEntidad> lista = new List<SEG_RolEntidad>();
            string consulta = @"SELECT [WEB_RolID],[WEB_RolNombre],[WEB_RolDescripcion],[WEB_RolEstado],[WEB_RolFechaRegistro]
                                FROM [dbo].[SEG_Rol] order by WEB_RolID Desc";
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
                            var webRol = new SEG_RolEntidad
                            {
                                WEB_RolID = ManejoNulls.ManageNullInteger(dr["WEB_RolID"]),
                                WEB_RolNombre = ManejoNulls.ManageNullStr(dr["WEB_RolNombre"]),
                                WEB_RolDescripcion = ManejoNulls.ManageNullStr(dr["WEB_RolDescripcion"].Trim()),
                                WEB_RolEstado = ManejoNulls.ManageNullStr(dr["WEB_RolEstado"].Trim()),
                                WEB_RolFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_RolFechaRegistro"].Trim())
                            };

                            lista.Add(webRol);
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

        public List<SEG_RolEntidad> GetRolesActivos()
        {
            List<SEG_RolEntidad> lista = new List<SEG_RolEntidad>();
            string consulta = @"SELECT [WEB_RolID],[WEB_RolNombre],[WEB_RolDescripcion],[WEB_RolEstado],[WEB_RolFechaRegistro]
                                FROM [dbo].[WEB_Rol]
                                where WEB_RolEstado=1
                                order by WEB_RolID Desc";
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
                            var webRol = new SEG_RolEntidad
                            {
                                WEB_RolID = ManejoNulls.ManageNullInteger(dr["WEB_RolID"]),
                                WEB_RolNombre = ManejoNulls.ManageNullStr(dr["WEB_RolNombre"]),
                                WEB_RolDescripcion = ManejoNulls.ManageNullStr(dr["WEB_RolDescripcion"].Trim()),
                                WEB_RolEstado = ManejoNulls.ManageNullStr(dr["WEB_RolEstado"].Trim()),
                                WEB_RolFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_RolFechaRegistro"].Trim())
                            };

                            lista.Add(webRol);
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
        public SEG_RolEntidad GetRolId(int rolid)
        {
            SEG_RolEntidad webRol = new SEG_RolEntidad();
            string consulta = @"SELECT [WEB_RolID],[WEB_RolNombre],[WEB_RolDescripcion],[WEB_RolEstado],[WEB_RolFechaRegistro]
                                FROM [dbo].[SEG_Rol] where WEB_RolID =@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rolid);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                webRol.WEB_RolID = ManejoNulls.ManageNullInteger(dr["WEB_RolID"]);
                                webRol.WEB_RolNombre = ManejoNulls.ManageNullStr(dr["WEB_RolNombre"]);
                                webRol.WEB_RolDescripcion = ManejoNulls.ManageNullStr(dr["WEB_RolDescripcion"].Trim());
                                webRol.WEB_RolEstado = ManejoNulls.ManageNullStr(dr["WEB_RolEstado"].Trim());
                                webRol.WEB_RolFechaRegistro =
                                    ManejoNulls.ManageNullDate(dr["WEB_RolFechaRegistro"].Trim());

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return webRol;
        }
        public bool ActualizarRol(SEG_RolEntidad rol)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[SEG_Rol]
                            SET [WEB_RolNombre] = @p1,[WEB_RolDescripcion] = @p2
                            WHERE WEB_RolID = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rol.WEB_RolID);
                    query.Parameters.AddWithValue("@p1", rol.WEB_RolNombre);
                    query.Parameters.AddWithValue("@p2", rol.WEB_RolDescripcion);
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

        public bool ActualizarEstadoRol(int rolid, int estado)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[SEG_Rol]
                            SET [WEB_RolEstado] = @p1
                            WHERE WEB_RolID = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rolid);
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

        public bool EliminarRol(int rolid)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM [dbo].[SEG_Rol] WHERE WEB_RolID = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rolid);
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
