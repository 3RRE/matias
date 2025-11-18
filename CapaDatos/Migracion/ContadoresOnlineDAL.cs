using CapaEntidad.Migracion;
using CapaEntidad.Reportes._9050;
using CapaEntidad.WhatsApp;
using FastMember;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Migracion {
    public class ContadoresOnlineDAL {
        string _conexion = string.Empty;

        public ContadoresOnlineDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public bool GuarGuardarContadoresOnline(List<ContadoresOnline> contadoresOnline) {
            bool success = true;
            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    using(SqlBulkCopy bulkCopy = new SqlBulkCopy(connection)) {
                        using(var reader = ObjectReader.Create(contadoresOnline)) {
                            // Asignar el nombre de la tabla de destino
                            bulkCopy.DestinationTableName = "ContadoresOnline";

                            // Configurar el mapeo de las columnas entre la lista y la tabla de destino
                            bulkCopy.ColumnMappings.Add("Fecha", "Fecha");
                            bulkCopy.ColumnMappings.Add("Hora", "Hora");
                            bulkCopy.ColumnMappings.Add("CodMaq", "CodMaq");
                            bulkCopy.ColumnMappings.Add("CoinIn", "CoinIn");
                            bulkCopy.ColumnMappings.Add("CoinOut", "CoinOut");
                            bulkCopy.ColumnMappings.Add("HandPay", "HandPay");
                            bulkCopy.ColumnMappings.Add("CurrentCredits", "CurrentCredits");
                            bulkCopy.ColumnMappings.Add("CancelCredits", "CancelCredits");
                            bulkCopy.ColumnMappings.Add("Jackpot", "Jackpot");
                            bulkCopy.ColumnMappings.Add("GamesPlayed", "GamesPlayed");
                            bulkCopy.ColumnMappings.Add("TotalDrop", "TotalDrop");
                            bulkCopy.ColumnMappings.Add("Token", "Token");
                            bulkCopy.ColumnMappings.Add("Empresa", "Empresa");
                            bulkCopy.ColumnMappings.Add("Sala", "Sala");

                            // Realizar la inserción masiva
                            bulkCopy.WriteToServer(reader);
                        }
                    }
                }
            } catch(Exception) {
                success = false;
            }

            return success;
        }

        public List<ContadoresOnlineDto> ObtenerContadoresOnlineParaReporte9050(string fechaInicio, string fechaFin, int codSala) {
            List<ContadoresOnlineDto> lista = new List<ContadoresOnlineDto>();
            string consulta = @"
                SELECT
	                CodContadoresOnline, 
	                CASE WHEN DATEPART(HOUR,hora) BETWEEN 0 AND 7 THEN DATEADD(DAY,-1,Fecha) ELSE Fecha END AS 'FechaHora',
	                CASE WHEN DATEPART(HOUR,hora) BETWEEN 0 AND 7 THEN CAST(DATEADD(DAY,-1,Fecha) AS DATE) ELSE CAST(Fecha AS DATE) END AS 'Fecha', 
	                DATEPART(HOUR,hora) Hora,
	                CodMaq, 
	                ISNULL(maquina.NroSerie,'-') AS 'NroSerie', 
	                sa.Nombre AS 'Sala', 
	                CoinIn, 
	                CoinOut,
	                CurrentCredits,
	                CancelCredits,
	                GamesPlayed,
	                con.Token,
	                ISNULL(marca.Nombre,'-') AS 'Marca',
	                ISNULL(modelo.Nombre,'-') AS 'Modelo',
	                CAST(Fecha AS DATE) AS 'FechaReal'
                FROM
	                ContadoresOnline con (NOLOCK) 
                LEFT JOIN
	                ADM_Maquina AS maquina (NOLOCK) ON con.CodMaq = maquina.CodAlterno
                LEFT JOIN 
	                ADM_ModeloMaquina modelo (NOLOCK) ON maquina.CodModeloMaquina = modelo.CodModeloMaquina
                LEFT JOIN 
	                ADM_MarcaMaquina marca (NOLOCK) ON modelo.CodMarcaMaquina = marca.CodMarcaMaquina
                INNER JOIN
	                Sala sa (NOLOCK) ON sa.CodSala = con.Sala
                 WHERE 
	                Fecha BETWEEN CONVERT(DATETIME, @fechaInicio, 105) AND CONVERT(DATETIME, @fechaFin, 105) AND
	                Sala = @codSala
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@codSala", codSala);
                    query.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    query.Parameters.AddWithValue("@fechaFin", fechaFin);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ContadoresOnlineDto {
                                CodContadoresOnline = ManejoNulos.ManageNullInteger(dr["CodContadoresOnline"]),
                                FechaHora = ManejoNulos.ManageNullDate(dr["FechaHora"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Hora = ManejoNulos.ManageNullInteger(dr["Hora"]),
                                CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]),
                                NroSerie = ManejoNulos.ManageNullStr(dr["NroSerie"]),
                                Sala = ManejoNulos.ManageNullStr(dr["Sala"]),
                                CoinIn = ManejoNulos.ManageNullDouble(dr["CoinIn"]),
                                CoinOut = ManejoNulos.ManageNullDouble(dr["CoinOut"]),
                                CurrentCredits = ManejoNulos.ManageNullDouble(dr["CurrentCredits"]),
                                CancelCredits = ManejoNulos.ManageNullDouble(dr["CancelCredits"]),
                                GamesPlayed = ManejoNulos.ManageNullDouble(dr["GamesPlayed"]),
                                Token = ManejoNulos.ManageNullDecimal(dr["Token"]),
                                Marca = ManejoNulos.ManageNullStr(dr["Marca"]),
                                Modelo = ManejoNulos.ManageNullStr(dr["Modelo"]),
                                FechaReal = ManejoNulos.ManageNullDate(dr["FechaReal"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }

        public DateTime ObtenerFechaDeUltimoContadorPorCodSala(int codSala) {
            DateTime fecha = DateTime.Now;
            string consulta = @"
                DECLARE @fechaUltima DATETIME
                SELECT TOP 1 
	                @fechaUltima = Fecha
                FROM 
	                ContadoresOnline (nolock)
                WHERE
	                Sala = @codSala
                ORDER BY
	                Fecha DESC

                SELECT ISNULL(@fechaUltima, CONVERT(DATE,CONVERT(CHAR(10), GETDATE()-2,120))) fecha
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@codSala", codSala);
                    fecha = ManejoNulos.ManageNullDate(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return fecha;
        }
    }
}
