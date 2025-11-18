using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using CapaDatos.Utilitarios;
using CapaEntidad.ControlAcceso;
using S3k.Utilitario;

namespace CapaDatos.ControlAcceso
{
    public class CAL_CargoPoliticoDAL
    {

        string conexion = string.Empty;
        public CAL_CargoPoliticoDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CAL_CargoPoliticoEntidad> GetAllCargoPolitico()
        {
            List < CAL_CargoPoliticoEntidad> lista = new List<CAL_CargoPoliticoEntidad>();
            string consulta = @"SELECT CargoPoliticoID
                                      ,Nombre
                                      ,Descripcion
                                      ,Estado
                                  FROM CAL_CargoPolitico(nolock) ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var campaña = new CAL_CargoPoliticoEntidad
                            {
                                CargoPoliticoID = ManejoNulos.ManageNullInteger(dr["CargoPoliticoID"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                            };

                            lista.Add(campaña);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        public CAL_CargoPoliticoEntidad GetIDCargoPolitico(int id)
        {
            CAL_CargoPoliticoEntidad item = new CAL_CargoPoliticoEntidad();
            string consulta = @"SELECT CargoPoliticoID, Nombre,Descripcion, Estado 
                                FROM CAL_CargoPolitico(nolock) 
                                WHERE CargoPoliticoID = @pCargoPoliticoID ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCargoPoliticoID", id);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item.CargoPoliticoID = ManejoNulos.ManageNullInteger(dr["CargoPoliticoID"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                item.Estado = ManejoNulos.ManegeNullBool(dr["Estado"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public int InsertarCargoPolitico(CAL_CargoPoliticoEntidad Entidad)
        {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO CAL_CargoPolitico (Nombre,Descripcion,Estado)
                                OUTPUT Inserted.CargoPoliticoID  
                                VALUES(@pNombre,@pDescripcion,@pEstado)";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManegeNullBool(Entidad.Estado));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarCargoPolitico(CAL_CargoPoliticoEntidad Entidad)
        {
            bool respuesta = false;
            string consulta = @"UPDATE CAL_CargoPolitico SET 
                                 Nombre = @pNombre,
                                 Descripcion = @pDescripcion,
                                 Estado = @pEstado WHERE CargoPoliticoID  = @pCargoPoliticoID";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCargoPoliticoID", ManejoNulos.ManageNullInteger(Entidad.CargoPoliticoID));
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pDescripcion", ManejoNulos.ManageNullStr(Entidad.Descripcion));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManegeNullBool(Entidad.Estado));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarCargoPolitico(int id)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM CAL_CargoPolitico 
                                WHERE CargoPoliticoID  = @pCargoPoliticoID";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCargoPoliticoID", ManejoNulos.ManageNullInteger(id));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
    }
}
