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
    public class SEG_RolUsuarioDAL
    {
        string _conexion = string.Empty;
        public SEG_RolUsuarioDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public bool GuardarRolUsuario(SEG_RolUsuarioEntidad rolUsuario)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO [dbo].[SEG_RolUsuario]
           ([WEB_RolID],[UsuarioID],[WEB_RUsuFechaRegistro])VALUES(@p0,@p1,@p2)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulls.ManageNullStr(rolUsuario.WEB_RolID) == String.Empty ? SqlString.Null : Convert.ToString(rolUsuario.WEB_RolID));
                    query.Parameters.AddWithValue("@p1", ManejoNulls.ManageNullStr(rolUsuario.UsuarioID) == String.Empty ? SqlString.Null : Convert.ToString(rolUsuario.UsuarioID));
                    query.Parameters.AddWithValue("@p2", ManejoNulls.ManageNullDate(rolUsuario.WEB_RUsuFechaRegistro));
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

        public List<SEG_RolUsuarioEntidad> GetRolUsuario()
        {
            List<SEG_RolUsuarioEntidad> lista = new List<SEG_RolUsuarioEntidad>();
            string consulta = @"SELECT [WEB_RUsuID]
                              ,[WEB_RolID]
                              ,[UsuarioID]
                              ,[WEB_RUsuFechaRegistro]
                                FROM [dbo].[SEG_RolUsuario] order by WEB_RUsuID Desc";
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
                            var webRolUsuario = new SEG_RolUsuarioEntidad
                            {
                                WEB_RUsuID = ManejoNulls.ManageNullInteger(dr["WEB_RUsuID"]),
                                WEB_RolID = ManejoNulls.ManageNullInteger(dr["WEB_RolID"]),
                                UsuarioID = ManejoNulls.ManageNullInteger(dr["UsuarioID"]),
                                WEB_RUsuFechaRegistro = ManejoNulls.ManageNullDate(dr["WEB_RUsuFechaRegistro"].Trim())
                            };

                            lista.Add(webRolUsuario);
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

        public SEG_RolUsuarioEntidad GetRolUsuarioId(int Usuarioid)
        {
            SEG_RolUsuarioEntidad webRolUsuario = new SEG_RolUsuarioEntidad();
            string consulta = @"SELECT [WEB_RUsuID]
                                  ,[WEB_RolID]
                                  ,[UsuarioID]
                                  ,[WEB_RUsuFechaRegistro]
                              FROM [dbo].[SEG_RolUsuario] where UsuarioID = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", Usuarioid);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                webRolUsuario.WEB_RUsuID = ManejoNulls.ManageNullInteger(dr["WEB_RUsuID"]);
                                webRolUsuario.WEB_RolID = ManejoNulls.ManageNullInteger(dr["WEB_RolID"]);
                                webRolUsuario.UsuarioID = ManejoNulls.ManageNullInteger(dr["UsuarioID"]);
                                webRolUsuario.WEB_RUsuFechaRegistro =
                                    ManejoNulls.ManageNullDate(dr["WEB_RUsuFechaRegistro"].Trim());
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return webRolUsuario;
        }
        public bool ActualizarRolUsuario(SEG_RolUsuarioEntidad rolUsuario)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[[SEG_RolUsuario]]
                            SET [[WEB_RolID]] = @p1,[[UsuarioID]] = @p2
                            WHERE WEB_RUsuID = @p0";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", rolUsuario.WEB_RUsuID);
                    query.Parameters.AddWithValue("@p1", rolUsuario.WEB_RolID);
                    query.Parameters.AddWithValue("@p2", rolUsuario.UsuarioID);
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

        public bool EliminarRolUsuario(int RolUsuid)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM [dbo].[SEG_RolUsuario] WHERE UsuarioID = @p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", RolUsuid);
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
