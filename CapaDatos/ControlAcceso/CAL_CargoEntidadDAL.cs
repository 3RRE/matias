using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad.ControlAcceso;
using S3k.Utilitario;

namespace CapaDatos.ControlAcceso
{
    public class CAL_CargoEntidadDAL
    {
        string conexion = string.Empty;
        public CAL_CargoEntidadDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CAL_CargoEntidadEntidad> GetAllCargoEntidad()
        {
            List<CAL_CargoEntidadEntidad> lista = new List<CAL_CargoEntidadEntidad>();
            string consulta = @"SELECT CargoEntidadID
                                      ,Nombre
                                      ,Estado
                                      ,FechaRegistro
                                  FROM CAL_CargoEntidad(nolock) ";
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
                            var campaña = new CAL_CargoEntidadEntidad
                            {
                                CargoEntidadID = ManejoNulos.ManageNullInteger(dr["CargoEntidadID"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
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
        public CAL_CargoEntidadEntidad GetIDCargoEntidad(int id)
        {
            CAL_CargoEntidadEntidad item = new CAL_CargoEntidadEntidad();
            string consulta = @"SELECT CargoEntidadID, Nombre,FechaRegistro, Estado 
                                FROM CAL_CargoEntidad(nolock) 
                                WHERE CargoEntidadID = @pCargoEntidadID ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCargoEntidadID", id);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item.CargoEntidadID = ManejoNulos.ManageNullInteger(dr["CargoEntidadID"]);
                                item.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
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
        public int InsertarCargoEntidad(CAL_CargoEntidadEntidad Entidad)
        {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO CAL_CargoEntidad (Nombre,FechaRegistro,Estado)
                                OUTPUT Inserted.CargoEntidadID  
                                VALUES(@pNombre,@pFechaRegistro,@pEstado)";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
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
        public bool EditarCargoEntidad(CAL_CargoEntidadEntidad Entidad)
        {
            bool respuesta = false;
            string consulta = @"UPDATE CAL_CargoEntidad SET 
                                 Nombre = @pNombre,
                                 Estado = @pEstado WHERE CargoEntidadID  = @pCargoEntidadID";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCargoEntidadID", ManejoNulos.ManageNullInteger(Entidad.CargoEntidadID));
                    query.Parameters.AddWithValue("@pNombre", ManejoNulos.ManageNullStr(Entidad.Nombre).ToUpper().Trim());
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
        public bool EliminarCargoEntidad(int id)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM CAL_CargoEntidad 
                                WHERE CargoEntidadID  = @pCargoEntidadID";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCargoEntidadID", ManejoNulos.ManageNullInteger(id));
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
