using CapaDatos.Utilitarios;
using CapaEntidad.ExcelenciaOperativa;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CapaDatos.ExcelenciaOperativa
{
    public class EO_FichaItemDAL
    {
        private static string _conexion = string.Empty;

        public EO_FichaItemDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public bool InsertarFichaItem(EO_FichaItemEntidad fichaItem)
        {
            bool response = false;

            string query = @"
            INSERT INTO EO_FichaItems
            (
                FichaId,
                CategoriaId,
                Nombre,
                PuntuacionObtenida,
                PuntuacionBase,
                FechaExpiracion,
                FechaExpiracionActivo,
                Respuesta,
                Observacion,
                TipoRespuesta,
                Codigo
            )
            VALUES
            (
                @p1,
                @p2,
                @p3,
                @p4,
                @p5,
                @p6,
                @p7,
                @p8,
                @p9,
                @p10,
                @p11
            )";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@p1", fichaItem.FichaId);
                    command.Parameters.AddWithValue("@p2", fichaItem.CategoriaId);
                    command.Parameters.AddWithValue("@p3", fichaItem.Nombre);
                    command.Parameters.AddWithValue("@p4", fichaItem.PuntuacionObtenida);
                    command.Parameters.AddWithValue("@p5", fichaItem.PuntuacionBase);
                    command.Parameters.AddWithValue("@p6", fichaItem.FechaExpiracion ?? Convert.DBNull);
                    command.Parameters.AddWithValue("@p7", fichaItem.FechaExpiracionActivo ?? Convert.DBNull);
                    command.Parameters.AddWithValue("@p8", fichaItem.Respuesta ?? Convert.DBNull);
                    command.Parameters.AddWithValue("@p9", fichaItem.Observacion ?? Convert.DBNull);
                    command.Parameters.AddWithValue("@p10", fichaItem.TipoRespuesta ?? Convert.DBNull);
                    command.Parameters.AddWithValue("@p11", fichaItem.Codigo ?? Convert.DBNull);
                    command.ExecuteNonQuery();

                    response = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return response;
        }

        public bool ActualizarFichaItem(EO_FichaItemEntidad fichaItem)
        {
            bool response = false;

            string query = @"
            UPDATE EO_FichaItems
            SET
                PuntuacionObtenida = @p1,
                PuntuacionBase = @p2,
                FechaExpiracion = @p3,
                Respuesta = @p4,
                Observacion = @p5
            WHERE
                ItemId = @p0";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@p1", fichaItem.PuntuacionObtenida);
                    command.Parameters.AddWithValue("@p2", fichaItem.PuntuacionBase);
                    command.Parameters.AddWithValue("@p3", fichaItem.FechaExpiracion ?? Convert.DBNull);
                    command.Parameters.AddWithValue("@p4", fichaItem.Respuesta ?? Convert.DBNull);
                    command.Parameters.AddWithValue("@p5", fichaItem.Observacion ?? Convert.DBNull);
                    command.Parameters.AddWithValue("@p0", fichaItem.ItemId);

                    command.ExecuteNonQuery();

                    response = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return response;
        }

        public List<EO_FichaItemEntidad> FichaItemIdCategoriaObtenerJson(long id)
        {
            List<EO_FichaItemEntidad> lista = new List<EO_FichaItemEntidad>();
            ClaseError error = new ClaseError();

            string consulta = @"SELECT item.ItemId, item.FichaId, item.CategoriaId, item.Nombre, item.PuntuacionObtenida, item.PuntuacionBase, item.FechaExpiracion, item.FechaExpiracionActivo, item.Respuesta, item.Observacion, item.TipoRespuesta, item.Codigo FROM [EO_FichaItems] as item WHERE item.CategoriaId=@p0;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                EO_FichaItemEntidad objeto = new EO_FichaItemEntidad();

                                objeto.ItemId = ManejoNulos.ManageNullInteger64(dr["ItemId"]);
                                objeto.FichaId = ManejoNulos.ManageNullInteger64(dr["FichaId"]);
                                objeto.CategoriaId = ManejoNulos.ManageNullInteger64(dr["CategoriaId"]);
                                objeto.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                objeto.PuntuacionObtenida = ManejoNulos.ManageNullFloat(dr["PuntuacionObtenida"]);
                                objeto.PuntuacionBase = ManejoNulos.ManageNullFloat(dr["PuntuacionBase"]);
                                objeto.FechaExpiracion = Convert.IsDBNull(dr["FechaExpiracion"]) ? (DateTime?) null : Convert.ToDateTime(dr["FechaExpiracion"]);
                                objeto.FechaExpiracionActivo = ManejoNulos.ManageNullInteger(dr["FechaExpiracionActivo"]);
                                objeto.Respuesta = ManejoNulos.ManageNullStr(dr["Respuesta"]);
                                objeto.Observacion = ManejoNulos.ManageNullStr(dr["Observacion"]);
                                objeto.TipoRespuesta = ManejoNulos.ManageNullInteger(dr["TipoRespuesta"]);
                                objeto.Codigo = ManejoNulos.ManageNullStr(dr["Codigo"]);

                                lista.Add(objeto);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return lista;
        }

        public bool EliminarEOItemsFicha(long fichaId)
        {
            string consulta = @"DELETE FROM [EO_FichaItems] WHERE FichaId=@p0";

            ClaseError error = new ClaseError();

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fichaId);

                    query.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                error.Value = ex.Message;
                return false;
            }
        }
    }
}
