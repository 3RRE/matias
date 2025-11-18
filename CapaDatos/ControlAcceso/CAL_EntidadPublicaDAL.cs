using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using CapaDatos.Utilitarios;
using CapaEntidad.ControlAcceso;
using S3k.Utilitario;

namespace CapaDatos.ControlAcceso
{
    public class CAL_EntidadPublicaDAL
    {
        string conexion = string.Empty;
        public CAL_EntidadPublicaDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CAL_EntidadPublicaEntidad> GetAllEntidadPublica()
        {
            List<CAL_EntidadPublicaEntidad> lista = new List<CAL_EntidadPublicaEntidad>();
            string consulta = @"SELECT EntidadPublicaID
                                      ,Nombre
                                      ,Estado
                                      ,FechaRegistro
                                      ,FechaModificacion
                                  FROM CAL_EntidadPublica(nolock) ";
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
                            var campaña = new CAL_EntidadPublicaEntidad
                            {
                                EntidadPublicaID = ManejoNulos.ManageNullInteger(dr["EntidadPublicaID"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
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
        public CAL_EntidadPublicaEntidad GetIDEntidadPublica(int id)
        {
            CAL_EntidadPublicaEntidad item = new CAL_EntidadPublicaEntidad();
            string consulta = @"SELECT EntidadPublicaID, Nombre, Estado ,FechaRegistro,FechaModificacion
                                FROM CAL_EntidadPublica(nolock) 
                                WHERE EntidadPublicaID = @pEntidadPublicaID ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pEntidadPublicaID", id);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item.EntidadPublicaID = ManejoNulos.ManageNullInteger(dr["EntidadPublicaID"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
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
        public int InsertarEntidadPublica(CAL_EntidadPublicaEntidad Entidad)
        {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO CAL_EntidadPublica (Nombre,Estado,FechaRegistro)
                                OUTPUT Inserted.EntidadPublicaID  
                                VALUES(@pNombre,@pEstado,@pFechaRegistro)";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
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
        public bool EditarEntidadPublica(CAL_EntidadPublicaEntidad Entidad)
        {
            bool respuesta = false;
            string consulta = @"UPDATE CAL_EntidadPublica SET 
                                 Nombre = @pNombre,
                                 FechaModificacion = @pFechaModificacion, 
                                 Estado = @pEstado 
                                 WHERE EntidadPublicaID  = @pEntidadPublicaID";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pEntidadPublicaID", ManejoNulos.ManageNullInteger(Entidad.EntidadPublicaID));
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pFechaModificacion", ManejoNulos.ManageNullDate(Entidad.FechaModificacion));
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
        public bool EliminarEntidadPublica(int id)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM CAL_EntidadPublica 
                                WHERE EntidadPublicaID  = @pEntidadPublicaID";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pEntidadPublicaID", ManejoNulos.ManageNullInteger(id));
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
