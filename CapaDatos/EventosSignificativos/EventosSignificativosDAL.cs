using CapaEntidad.Discos;
using CapaEntidad.EventosSignificativos;
using CapaEntidad.Migracion;
using FastMember;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.EventosSignificativos {
    public class EventosSignificativosDAL {
        string _conexion = string.Empty;
        public EventosSignificativosDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<EventosSignificativosEntidad> ListadoEventosSignificativos(int codsala, DateTime fechaIni, DateTime fechaFin) {
            List<EventosSignificativosEntidad> lista = new List<EventosSignificativosEntidad>();
            string consulta = @"Select IdEventoSignificativo,Cod_Even_OL,Fecha, Hora,Cod_Tarjeta, Cod_Maquina, Cod_Evento, Nombre_Evento from EventosSignificativos where Cod_Sala=" + codsala + "and convert(date,Fecha) between @p1 and @p2";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new EventosSignificativosEntidad {
                                 IdEventoSignificativo= ManejoNulos.ManageNullInteger(dr["IdEventoSignificativo"]),
                                Cod_Even_OL = ManejoNulos.ManageNullInteger(dr["Cod_Even_OL"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Hora = ManejoNulos.ManageNullDate(dr["Hora"]),
                                CodTarjeta = ManejoNulos.ManageNullStr(dr["Cod_Tarjeta"]),
                                CodMaquina = ManejoNulos.ManageNullStr(dr["Cod_Maquina"]),
                                Cod_Evento = ManejoNulos.ManageNullInteger(dr["Cod_Evento"]),
                                NombreEvento = ManejoNulos.ManageNullStr(dr["Nombre_Evento"]),
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

        public bool GuardarEventosSignificativos(List<EventosSignificativosEntidad> eventosSignificativos) {
            bool success = true;
            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    using(SqlBulkCopy bulkCopy = new SqlBulkCopy(connection)) {
                        using(var reader = ObjectReader.Create(eventosSignificativos)) {
                            // Asignar el nombre de la tabla de destino
                            bulkCopy.DestinationTableName = "EventosSignificativos";

                            // Configurar el mapeo de las columnas entre la lista y la tabla de destino
                            bulkCopy.ColumnMappings.Add("Fecha", "Fecha");
                            bulkCopy.ColumnMappings.Add("Hora", "Hora");
                            bulkCopy.ColumnMappings.Add("CodTarjeta", "Cod_Tarjeta");
                            bulkCopy.ColumnMappings.Add("CodMaquina", "Cod_Maquina");
                            bulkCopy.ColumnMappings.Add("Cod_Evento", "Cod_Evento");
                            bulkCopy.ColumnMappings.Add("NombreEvento", "Nombre_Evento");
                            bulkCopy.ColumnMappings.Add("Cod_Even_OL", "Cod_Even_OL");
                            bulkCopy.ColumnMappings.Add("COD_SALA", "Cod_Sala");

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

        public List<int> ObtenerIdsEventosSignificativosExistentes(List<int> idsInsertar, string codSala) {
            List<int> idsExistentes = new List<int>();
            string consulta = $@"
                SELECT Cod_Even_OL
                FROM eventosSignificativos 
                WHERE Cod_Sala = @codSala AND Cod_Even_OL IN ({string.Join(",", idsInsertar)})
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();

                    using(SqlCommand query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@codSala", codSala);
                        using(SqlDataReader rd = query.ExecuteReader()) {
                            while(rd.Read()) {
                                idsExistentes.Add(ManejoNulos.ManageNullInteger(rd["Cod_Even_OL"]));
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return idsExistentes;
        }

    }
}
