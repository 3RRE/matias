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
    public class EO_FichaCategoriaDAL
    {
        private static string _conexion = string.Empty;
        public EO_FichaCategoriaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public long InsertarFichaCategoria(EO_FichaCategoriaEntidad fichaCategoria)
        {
            long IdInsertado = 0;

            string query = @"
            INSERT INTO EO_FichaCategorias
            (
                FichaId,
                Nombre,
                PuntuacionObtenida,
                PuntuacionBase,
                Porcentaje,
                Codigo
            )
            VALUES
            (
                @p1,
                @p2,
                @p3,
                @p4,
                @p5,
                @p6
            );

            SELECT SCOPE_IDENTITY()";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();
                    SqlCommand commmand = new SqlCommand(query, connection);
                    commmand.Parameters.AddWithValue("@p1", fichaCategoria.FichaId);
                    commmand.Parameters.AddWithValue("@p2", fichaCategoria.Nombre);
                    commmand.Parameters.AddWithValue("@p3", fichaCategoria.PuntuacionObtenida);
                    commmand.Parameters.AddWithValue("@p4", fichaCategoria.PuntuacionBase);
                    commmand.Parameters.AddWithValue("@p5", fichaCategoria.Porcentaje);
                    commmand.Parameters.AddWithValue("@p6", fichaCategoria.Codigo ?? Convert.DBNull);

                    IdInsertado = Convert.ToInt64(commmand.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
                IdInsertado = 0;
            }

            return IdInsertado;
        }

        public bool ActualizarFichaCategoria(EO_FichaCategoriaEntidad fichaCategoria)
        {
            bool response = false;

            string query = @"
            UPDATE EO_FichaCategorias
            SET
                PuntuacionObtenida = @p1,
                PuntuacionBase = @p2,
                Porcentaje = @p3
            WHERE
                CategoriaId = @p0";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@p1", fichaCategoria.PuntuacionObtenida);
                    command.Parameters.AddWithValue("@p2", fichaCategoria.PuntuacionBase);
                    command.Parameters.AddWithValue("@p3", fichaCategoria.Porcentaje);
                    command.Parameters.AddWithValue("@p0", fichaCategoria.CategoriaId);

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

        public List<EO_FichaCategoriaEntidad> FichaCategoriaIdFichaObtenerJson(long id)
        {
            List<EO_FichaCategoriaEntidad> lista = new List<EO_FichaCategoriaEntidad>();

            string consulta = @"SELECT categoria.CategoriaId, categoria.FichaId, categoria.Nombre, categoria.PuntuacionObtenida, categoria.PuntuacionBase, categoria.Porcentaje, categoria.Codigo FROM [EO_FichaCategorias] as categoria WHERE categoria.FichaId=@p0;";
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
                                EO_FichaCategoriaEntidad objeto = new EO_FichaCategoriaEntidad();

                                objeto.CategoriaId = ManejoNulos.ManageNullInteger64(dr["CategoriaId"]);
                                objeto.FichaId = ManejoNulos.ManageNullInteger64(dr["FichaId"]);
                                objeto.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                objeto.PuntuacionObtenida = ManejoNulos.ManageNullFloat(dr["PuntuacionObtenida"]);
                                objeto.PuntuacionBase = ManejoNulos.ManageNullFloat(dr["PuntuacionBase"]);
                                objeto.Porcentaje = ManejoNulos.ManageNullFloat(dr["Porcentaje"]);
                                objeto.Codigo = ManejoNulos.ManageNullStr(dr["Codigo"]);

                                lista.Add(objeto);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }

            return lista;
        }

        public bool EliminarEOCategoriasFicha(long fichaId)
        {
            string consulta = @"DELETE FROM [EO_FichaCategorias] WHERE FichaId=@p0";

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
