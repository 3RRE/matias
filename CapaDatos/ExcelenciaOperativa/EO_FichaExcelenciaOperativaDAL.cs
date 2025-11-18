using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using CapaDatos.Utilitarios;
using CapaEntidad.ExcelenciaOperativa;
using S3k.Utilitario;

namespace CapaDatos.ExcelenciaOperativa
{
    public class EO_FichaExcelenciaOperativaDAL
    {
        private static string _conexion = string.Empty;
        public EO_FichaExcelenciaOperativaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public long InsertarFichaExcelenciaOperativa(EO_FichaExcelenciaOperativaEntidad ficha)
        {
            long IdInsertado = 0;

            string query = @"
            INSERT INTO EO_FichaExcelenciaOperativa
            (
                UsuarioId,
                SalaId,
                Tipo,
                Fecha,
                PuntuacionObtenida,
                PuntuacionBase,
                Porcentaje,
                Codigo,
                FichaVersion,
                FechaCreado
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
                GETDATE()
            );

            SELECT SCOPE_IDENTITY()";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", ficha.UsuarioId);
                    command.Parameters.AddWithValue("@p2", ficha.SalaId);
                    command.Parameters.AddWithValue("@p3", ficha.Tipo);
                    command.Parameters.AddWithValue("@p4", ficha.Fecha);
                    command.Parameters.AddWithValue("@p5", ficha.PuntuacionObtenida);
                    command.Parameters.AddWithValue("@p6", ficha.PuntuacionBase);
                    command.Parameters.AddWithValue("@p7", ficha.Porcentaje);
                    command.Parameters.AddWithValue("@p8", ficha.Codigo ?? Convert.DBNull);
                    command.Parameters.AddWithValue("@p9", ficha.FichaVersion);

                    IdInsertado = Convert.ToInt64(command.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
                IdInsertado = 0;
            }

            return IdInsertado;
        }

        public bool ActualizarFichaExcelenciaOperativa(EO_FichaExcelenciaOperativaEntidad ficha)
        {
            bool response = false;

            string query = @"
            UPDATE EO_FichaExcelenciaOperativa
            SET
                PuntuacionObtenida = @p1,
                PuntuacionBase = @p2,
                Porcentaje = @p3,
                FechaActualizado = GETDATE()
            WHERE
                FichaId = @p0";

            try
            {
                using (var connection = new SqlConnection(_conexion))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@p1", ficha.PuntuacionObtenida);
                    command.Parameters.AddWithValue("@p2", ficha.PuntuacionBase);
                    command.Parameters.AddWithValue("@p3", ficha.Porcentaje);
                    command.Parameters.AddWithValue("@p0", ficha.FichaId);

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

        public (List<EO_FichaExcelenciaOperativaEntidad> fichaExperienciasOperativasLista, ClaseError error) FichaEOFiltroListarxTipoFechaJson(string tipos, DateTime fechaini, DateTime fechafin)
        {
            List<EO_FichaExcelenciaOperativaEntidad> lista = new List<EO_FichaExcelenciaOperativaEntidad>();
            ClaseError error = new ClaseError();

            string query = $@"
            SELECT
                ficha.FichaId,
                ficha.UsuarioId,
                ficha.SalaId,
                ficha.Tipo,
                ficha.Fecha,
                ficha.PuntuacionObtenida,
                ficha.PuntuacionBase,
                ficha.Porcentaje,
                ficha.Codigo,
                usuario.UsuarioNombre,
                sala.Nombre AS SalaNombre
            FROM EO_FichaExcelenciaOperativa AS ficha
            INNER JOIN SEG_Usuario AS usuario ON usuario.UsuarioID = ficha.UsuarioId
            INNER JOIN Sala AS sala ON sala.CodSala = ficha.SalaId
            WHERE {tipos} ficha.Fecha BETWEEN @p1 AND @p2
            ORDER BY ficha.FichaId DESC";
            
            try
            {
                using (var connection = new SqlConnection(_conexion))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@p1", fechaini);
                    command.Parameters.AddWithValue("@p2", fechafin);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                var fila = new EO_FichaExcelenciaOperativaEntidad
                                {
                                    FichaId = ManejoNulos.ManageNullInteger64(data["FichaId"]),
                                    UsuarioId = ManejoNulos.ManageNullInteger(data["UsuarioId"]),
                                    SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]),
                                    Tipo = ManejoNulos.ManageNullInteger(data["Tipo"]),
                                    Fecha = ManejoNulos.ManageNullDate(data["Fecha"]),
                                    PuntuacionObtenida = ManejoNulos.ManageNullFloat(data["PuntuacionObtenida"]),
                                    PuntuacionBase = ManejoNulos.ManageNullFloat(data["PuntuacionBase"]),
                                    Porcentaje = ManejoNulos.ManageNullFloat(data["Porcentaje"]),
                                    UsuarioNombre = ManejoNulos.ManageNullStr(data["UsuarioNombre"]),
                                    SalaNombre = ManejoNulos.ManageNullStr(data["SalaNombre"]),
                                    Codigo = ManejoNulos.ManageNullStr(data["Codigo"])
                                };

                                lista.Add(fila);
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

            return (fichaExperienciasOperativasLista: lista, error);
        }

        public (EO_FichaExcelenciaOperativaEntidad fichaExcelenciaOperativa, ClaseError error) FichaEOIdObtenerJson(long id)
        {
            EO_FichaExcelenciaOperativaEntidad ficha = new EO_FichaExcelenciaOperativaEntidad();
            ClaseError error = new ClaseError();
            
            string query = @"
            SELECT
                ficha.FichaId,
                ficha.UsuarioId,
                ficha.SalaId,
                ficha.Tipo,
                ficha.Fecha,
                ficha.PuntuacionObtenida,
                ficha.PuntuacionBase,
                ficha.Porcentaje,
                ficha.Codigo,
                ficha.FichaVersion,
                usuario.UsuarioNombre,
                sala.Nombre AS SalaNombre
            FROM EO_FichaExcelenciaOperativa AS ficha
            INNER JOIN SEG_Usuario AS usuario ON usuario.UsuarioID = ficha.UsuarioId
            INNER JOIN Sala AS sala ON sala.CodSala = ficha.SalaId
            WHERE ficha.FichaId = @p0";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();
                    var command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@p0", id);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                ficha.FichaId = ManejoNulos.ManageNullInteger64(data["FichaId"]);
                                ficha.UsuarioId = ManejoNulos.ManageNullInteger(data["UsuarioId"]);
                                ficha.SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]);
                                ficha.Tipo = ManejoNulos.ManageNullInteger(data["Tipo"]);
                                ficha.Fecha = ManejoNulos.ManageNullDate(data["Fecha"]);
                                ficha.PuntuacionObtenida = ManejoNulos.ManageNullFloat(data["PuntuacionObtenida"]);
                                ficha.PuntuacionBase = ManejoNulos.ManageNullFloat(data["PuntuacionBase"]);
                                ficha.Porcentaje = ManejoNulos.ManageNullFloat(data["Porcentaje"]);
                                ficha.UsuarioNombre = ManejoNulos.ManageNullStr(data["UsuarioNombre"]);
                                ficha.SalaNombre = ManejoNulos.ManageNullStr(data["SalaNombre"]);
                                ficha.Codigo = ManejoNulos.ManageNullStr(data["Codigo"]);
                                ficha.FichaVersion = ManejoNulos.ManageNullInteger(data["FichaVersion"]);
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

            return (fichaExcelenciaOperativa: ficha, error);
        }

        // refactored
        public List<EO_FichaExcelenciaOperativaEntidad> ListaFichaEOFilters(string inFilters, DateTime startDate, DateTime endDate, string orderBy = "DESC")
        {
            List<EO_FichaExcelenciaOperativaEntidad> listaFicha = new List<EO_FichaExcelenciaOperativaEntidad>();

            string query = $@"
            SELECT
                ficha.FichaId,
                ficha.UsuarioId,
                ficha.SalaId,
                ficha.Tipo,
                ficha.Fecha,
                ficha.PuntuacionObtenida,
                ficha.PuntuacionBase,
                ficha.Porcentaje,
                ficha.Codigo,
                ficha.FichaVersion,
                usuario.UsuarioNombre,
                sala.Nombre AS SalaNombre
            FROM EO_FichaExcelenciaOperativa AS ficha
            INNER JOIN SEG_Usuario AS usuario ON usuario.UsuarioID = ficha.UsuarioId
            INNER JOIN Sala AS sala ON sala.CodSala = ficha.SalaId
            WHERE {inFilters} ficha.Fecha BETWEEN @p1 AND @p2
            ORDER BY ficha.FichaId {orderBy}";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@p1", startDate);
                    command.Parameters.AddWithValue("@p2", endDate);

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                var fila = new EO_FichaExcelenciaOperativaEntidad
                                {
                                    FichaId = ManejoNulos.ManageNullInteger64(data["FichaId"]),
                                    UsuarioId = ManejoNulos.ManageNullInteger(data["UsuarioId"]),
                                    SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]),
                                    Tipo = ManejoNulos.ManageNullInteger(data["Tipo"]),
                                    Fecha = ManejoNulos.ManageNullDate(data["Fecha"]),
                                    PuntuacionObtenida = ManejoNulos.ManageNullFloat(data["PuntuacionObtenida"]),
                                    PuntuacionBase = ManejoNulos.ManageNullFloat(data["PuntuacionBase"]),
                                    Porcentaje = ManejoNulos.ManageNullFloat(data["Porcentaje"]),
                                    UsuarioNombre = ManejoNulos.ManageNullStr(data["UsuarioNombre"]),
                                    SalaNombre = ManejoNulos.ManageNullStr(data["SalaNombre"]),
                                    Codigo = ManejoNulos.ManageNullStr(data["Codigo"]),
                                    FichaVersion = ManejoNulos.ManageNullInteger(data["FichaVersion"])
                                };

                                listaFicha.Add(fila);
                            }
                        }
                    }

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return listaFicha;
        }

        public EO_FichaExcelenciaOperativaEntidad FichaEOId(long fichaId)
        {
            EO_FichaExcelenciaOperativaEntidad ficha = new EO_FichaExcelenciaOperativaEntidad();

            string query = @"
            SELECT
                ficha.FichaId,
                ficha.UsuarioId,
                ficha.SalaId,
                ficha.Tipo,
                ficha.Fecha,
                ficha.PuntuacionObtenida,
                ficha.PuntuacionBase,
                ficha.Porcentaje,
                ficha.Codigo,
                usuario.UsuarioNombre,
                sala.Nombre AS SalaNombre
            FROM EO_FichaExcelenciaOperativa AS ficha
            INNER JOIN SEG_Usuario AS usuario ON usuario.UsuarioID = ficha.UsuarioId
            INNER JOIN Sala AS sala ON sala.CodSala = ficha.SalaId
            WHERE ficha.FichaId = @p0";

            try
            {
                using(SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@p0", fichaId);

                    using(SqlDataReader data = command.ExecuteReader())
                    {
                        if(data.HasRows)
                        {
                            if(data.Read())
                            {
                                ficha.FichaId = ManejoNulos.ManageNullInteger64(data["FichaId"]);
                                ficha.UsuarioId = ManejoNulos.ManageNullInteger(data["UsuarioId"]);
                                ficha.SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]);
                                ficha.Tipo = ManejoNulos.ManageNullInteger(data["Tipo"]);
                                ficha.Fecha = ManejoNulos.ManageNullDate(data["Fecha"]);
                                ficha.PuntuacionObtenida = ManejoNulos.ManageNullFloat(data["PuntuacionObtenida"]);
                                ficha.PuntuacionBase = ManejoNulos.ManageNullFloat(data["PuntuacionBase"]);
                                ficha.Porcentaje = ManejoNulos.ManageNullFloat(data["Porcentaje"]);
                                ficha.UsuarioNombre = ManejoNulos.ManageNullStr(data["UsuarioNombre"]);
                                ficha.SalaNombre = ManejoNulos.ManageNullStr(data["SalaNombre"]);
                                ficha.Codigo = ManejoNulos.ManageNullStr(data["Codigo"]);
                            }
                        }
                    }
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return ficha;
        }

        public bool EliminarEOId(long fichaId)
        {
            bool response = false;

            string query = @"DELETE FROM EO_FichaExcelenciaOperativa WHERE FichaId = @p0";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@p0", fichaId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if(rowsAffected > 0)
                    {
                        response = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return response;
        }
    }
}
