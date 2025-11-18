using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.Sunat;
using FastMember;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace CapaDatos.Sunat {
    public class SunatDAL {
        private readonly string _conexion;

        public SunatDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        #region Contadores
        public bool GuardarContadoresSunat(List<ContadoresSunatEntidad> contadoresSunat) {
            bool success = true;

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    using(SqlBulkCopy bulkCopy = new SqlBulkCopy(connection)) {
                        using(ObjectReader reader = ObjectReader.Create(contadoresSunat)) {
                            // Asignar el nombre de la tabla de destino
                            bulkCopy.DestinationTableName = "ContadoresSunat";

                            // Configurar el mapeo de las columnas entre la lista y la tabla de destino
                            bulkCopy.ColumnMappings.Add("CodSala", "CodSala");
                            bulkCopy.ColumnMappings.Add("FechaMigracion", "FechaMigracion");
                            //campos
                            bulkCopy.ColumnMappings.Add("Fecha", "Fecha");
                            bulkCopy.ColumnMappings.Add("Trama", "Trama");
                            bulkCopy.ColumnMappings.Add("Cereo", "Cereo");
                            bulkCopy.ColumnMappings.Add("IdConSunat", "IdConSunat");
                            bulkCopy.ColumnMappings.Add("Envio", "Envio");
                            bulkCopy.ColumnMappings.Add("IdCereo", "IdCereo");
                            bulkCopy.ColumnMappings.Add("FechaEnvio", "FechaEnvio");
                            bulkCopy.ColumnMappings.Add("FechaProceso", "FechaProceso");
                            bulkCopy.ColumnMappings.Add("Motivo", "Motivo");
                            bulkCopy.ColumnMappings.Add("IdConfSunat", "IdConfSunat");
                            bulkCopy.ColumnMappings.Add("BandBusq", "BandBusq");
                            //datos de trama
                            bulkCopy.ColumnMappings.Add("Cabecera", "Cabecera");
                            bulkCopy.ColumnMappings.Add("DGJM", "DGJM");
                            bulkCopy.ColumnMappings.Add("CodMaq", "CodMaq");
                            bulkCopy.ColumnMappings.Add("FechaTrama", "FechaTrama");
                            bulkCopy.ColumnMappings.Add("Reserva1", "Reserva1");
                            bulkCopy.ColumnMappings.Add("Moneda", "Moneda");
                            bulkCopy.ColumnMappings.Add("Denominacion", "Denominacion");
                            bulkCopy.ColumnMappings.Add("CoinInFinal", "CoinInFinal");
                            bulkCopy.ColumnMappings.Add("CoinOutFinal", "CoinOutFinal");
                            bulkCopy.ColumnMappings.Add("PagoManualFinal", "PagoManualFinal");
                            bulkCopy.ColumnMappings.Add("OtroFinal", "OtroFinal");
                            bulkCopy.ColumnMappings.Add("TipoCambio", "TipoCambio");

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
        public List<ContadoresSunatEntidad> ObtenerUltimosContadoresSunat(int cantidadDias, string codSalas) {
            List<ContadoresSunatEntidad> lista = new List<ContadoresSunatEntidad>();
            string consulta = $@"
                SELECT cs.*, s.Nombre as NombreSala
                FROM ContadoresSunat AS cs
                INNER JOIN Sala AS s ON s.CodSala = cs.CodSala
                WHERE cs.FechaMigracion >= DATEADD(DAY, -@dias, GETDATE()) AND s.CodSala IN ({codSalas});
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@dias", cantidadDias);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ContadoresSunatEntidad {
                                //IdContadorSunat = ManejoNulos.ManageNullInteger(dr["IdContadorSunat"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Sala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                FechaMigracion = ManejoNulos.ManageNullDate(dr["FechaMigracion"]),

                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Trama = ManejoNulos.ManageNullStr(dr["Trama"]),
                                Cereo = ManejoNulos.ManegeNullBool(dr["Cereo"]),
                                IdConSunat = ManejoNulos.ManageNullInteger(dr["IdConSunat"]),
                                Envio = ManejoNulos.ManageNullInteger(dr["Envio"]),
                                IdCereo = ManejoNulos.ManageNullStr(dr["IdCereo"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                FechaProceso = ManejoNulos.ManageNullDate(dr["FechaProceso"]),
                                Motivo = ManejoNulos.ManageNullStr(dr["Motivo"]),
                                IdConfSunat = ManejoNulos.ManageNullInteger(dr["IdConfSunat"]),
                                BandBusq = ManejoNulos.ManageNullInteger(dr["BandBusq"]),

                                Cabecera = ManejoNulos.ManageNullStr(dr["Cabecera"]),
                                DGJM = ManejoNulos.ManageNullStr(dr["DGJM"]),
                                CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]),
                                FechaTrama = ManejoNulos.ManageNullStr(dr["FechaTrama"]),
                                Reserva1 = ManejoNulos.ManageNullStr(dr["Reserva1"]),
                                Moneda = ManejoNulos.ManageNullStr(dr["Moneda"]),
                                Denominacion = ManejoNulos.ManageNullStr(dr["Denominacion"]),
                                CoinInFinal = ManejoNulos.ManageNullStr(dr["CoinInFinal"]),
                                CoinOutFinal = ManejoNulos.ManageNullStr(dr["CoinOutFinal"]),
                                PagoManualFinal = ManejoNulos.ManageNullStr(dr["PagoManualFinal"]),
                                OtroFinal = ManejoNulos.ManageNullStr(dr["OtroFinal"]),
                                TipoCambio = ManejoNulos.ManageNullStr(dr["TipoCambio"]),
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
        public List<ContadoresSunatEntidad> ObtenerContadoresSunatxFecha(int codSala, DateTime fechaIni, DateTime fechaFin) {
            List<ContadoresSunatEntidad> lista = new List<ContadoresSunatEntidad>();
            string consulta = $@"
                SELECT cs.*, s.Nombre as NombreSala
                FROM ContadoresSunat AS cs
                INNER JOIN Sala AS s ON s.CodSala = cs.CodSala
                WHERE cs.CodSala=@codSala and convert(date,cs.Fecha) between @fechaIni and @fechaFin;";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@fechaIni", fechaIni);
                    query.Parameters.AddWithValue("@fechaFin", fechaFin);
                    query.Parameters.AddWithValue("@codSala", codSala);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ContadoresSunatEntidad {
                                //IdContadorSunat = ManejoNulos.ManageNullInteger(dr["IdContadorSunat"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Sala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                FechaMigracion = ManejoNulos.ManageNullDate(dr["FechaMigracion"]),

                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Trama = ManejoNulos.ManageNullStr(dr["Trama"]),
                                Cereo = ManejoNulos.ManegeNullBool(dr["Cereo"]),
                                IdConSunat = ManejoNulos.ManageNullInteger(dr["IdConSunat"]),
                                Envio = ManejoNulos.ManageNullInteger(dr["Envio"]),
                                IdCereo = ManejoNulos.ManageNullStr(dr["IdCereo"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                FechaProceso = ManejoNulos.ManageNullDate(dr["FechaProceso"]),
                                Motivo = ManejoNulos.ManageNullStr(dr["Motivo"]),
                                IdConfSunat = ManejoNulos.ManageNullInteger(dr["IdConfSunat"]),
                                BandBusq = ManejoNulos.ManageNullInteger(dr["BandBusq"]),

                                Cabecera = ManejoNulos.ManageNullStr(dr["Cabecera"]),
                                DGJM = ManejoNulos.ManageNullStr(dr["DGJM"]),
                                CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]),
                                FechaTrama = ManejoNulos.ManageNullStr(dr["FechaTrama"]),
                                Reserva1 = ManejoNulos.ManageNullStr(dr["Reserva1"]),
                                Moneda = ManejoNulos.ManageNullStr(dr["Moneda"]),
                                Denominacion = ManejoNulos.ManageNullStr(dr["Denominacion"]),
                                CoinInFinal = ManejoNulos.ManageNullStr(dr["CoinInFinal"]),
                                CoinOutFinal = ManejoNulos.ManageNullStr(dr["CoinOutFinal"]),
                                PagoManualFinal = ManejoNulos.ManageNullStr(dr["PagoManualFinal"]),
                                OtroFinal = ManejoNulos.ManageNullStr(dr["OtroFinal"]),
                                TipoCambio = ManejoNulos.ManageNullStr(dr["TipoCambio"]),
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
        public List<int> ObtenerIdsContadoresExistentes(List<int> idsInsertar, int codSala) {
            List<int> idsExistentes = new List<int>();
            string consulta = $@"
                SELECT IdConSunat 
                FROM ContadoresSunat 
                WHERE CodSala = @codSala AND IdConSunat IN ({string.Join(",", idsInsertar)})
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();

                    using(SqlCommand query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@codSala", codSala);
                        using(SqlDataReader rd = query.ExecuteReader()) {
                            while(rd.Read()) {
                                idsExistentes.Add(ManejoNulos.ManageNullInteger(rd["IdConSunat"]));
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return idsExistentes;
        }
        public int ObtenerUltimoIdContadorSunatPorCodSala(int codSala) {
            int ultimoId = 0;
            string consulta = $@"
                SELECT TOP 1 IdConSunat
                FROM ContadoresSunat
                WHERE CodSala = @codSala
                ORDER BY IdConSunat DESC
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(SqlCommand query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@codSala", codSala);
                        ultimoId = Convert.ToInt32(query.ExecuteScalar());
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return ultimoId;
        }
        public bool EditarContadoresSunat(ContadoresSunatEntidad cliente, int codSala) {
            bool respuesta = false;
            string consulta = @"  UPDATE [dbo].[ContadoresSunat]
                   SET 
                   [Envio] = @p1,
                   [FechaEnvio] = @p2
				   where IdConSunat =@IdConSunat   and CodSala = @codSala ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdConSunat", cliente.IdConSunat);
                    query.Parameters.AddWithValue("@codSala", codSala);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManegeNullBool(cliente.Envio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(cliente.FechaEnvio));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }

            return respuesta;
        }
        #endregion

        #region Eventos
        public bool GuardarEventosSunat(List<EventosSunatEntidad> eventosSunat) {
            bool success = true;

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    using(SqlBulkCopy bulkCopy = new SqlBulkCopy(connection)) {
                        using(ObjectReader reader = ObjectReader.Create(eventosSunat)) {
                            // Asignar el nombre de la tabla de destino
                            bulkCopy.DestinationTableName = "EventosSunat";

                            // Configurar el mapeo de las columnas entre la lista y la tabla de destino
                            bulkCopy.ColumnMappings.Add("CodSala", "CodSala");
                            bulkCopy.ColumnMappings.Add("FechaMigracion", "FechaMigracion");
                            //campos
                            bulkCopy.ColumnMappings.Add("Fecha", "Fecha");
                            bulkCopy.ColumnMappings.Add("Trama", "Trama");
                            bulkCopy.ColumnMappings.Add("TipoTrama", "TipoTrama");
                            bulkCopy.ColumnMappings.Add("IdEvSunat", "IdEvSunat");
                            bulkCopy.ColumnMappings.Add("Envio", "Envio");
                            bulkCopy.ColumnMappings.Add("FechaEnvio", "FechaEnvio");
                            bulkCopy.ColumnMappings.Add("FechaProceso", "FechaProceso");
                            bulkCopy.ColumnMappings.Add("Motivo", "Motivo");
                            bulkCopy.ColumnMappings.Add("IdConfSunat", "IdConfSunat");
                            bulkCopy.ColumnMappings.Add("BandBusq", "BandBusq");
                            //datos de trama
                            bulkCopy.ColumnMappings.Add("Cabecera", "Cabecera");
                            bulkCopy.ColumnMappings.Add("DGJM", "DGJM");
                            bulkCopy.ColumnMappings.Add("CodMaq", "CodMaq");
                            bulkCopy.ColumnMappings.Add("IdColector", "IdColector");
                            bulkCopy.ColumnMappings.Add("FechaTrama", "FechaTrama");
                            bulkCopy.ColumnMappings.Add("Pccm", "Pccm");
                            bulkCopy.ColumnMappings.Add("Pccsuctr", "Pccsuctr");
                            bulkCopy.ColumnMappings.Add("Rce", "Rce");
                            bulkCopy.ColumnMappings.Add("Embbram", "Embbram");
                            bulkCopy.ColumnMappings.Add("Apl", "Apl");
                            bulkCopy.ColumnMappings.Add("Fmc", "Fmc");
                            bulkCopy.ColumnMappings.Add("Frammr", "Frammr");
                            bulkCopy.ColumnMappings.Add("CereoTrama", "CereoTrama");
                            bulkCopy.ColumnMappings.Add("Reserva1", "Reserva1");
                            bulkCopy.ColumnMappings.Add("Reserva2", "Reserva2");

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
        public List<EventosSunatEntidad> ObtenerUltimosEventosSunat(int cantidadDias, string codSalas) {
            List<EventosSunatEntidad> lista = new List<EventosSunatEntidad>();
            string consulta = $@"
                SELECT es.*, s.Nombre as NombreSala
                FROM EventosSunat AS es
                INNER JOIN Sala AS s ON s.CodSala = es.CodSala
                WHERE es.FechaMigracion >= DATEADD(DAY, -@dias, GETDATE()) AND s.CodSala IN ({codSalas});
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@dias", cantidadDias);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new EventosSunatEntidad {
                                //IdEventoSunat = ManejoNulos.ManageNullInteger(dr["IdEventoSunat"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Sala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                FechaMigracion = ManejoNulos.ManageNullDate(dr["FechaMigracion"]),

                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Trama = ManejoNulos.ManageNullStr(dr["Trama"]),
                                TipoTrama = ManejoNulos.ManegeNullBool(dr["TipoTrama"]),
                                IdEvSunat = ManejoNulos.ManageNullInteger(dr["IdEvSunat"]),
                                Envio = ManejoNulos.ManageNullInteger(dr["Envio"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                FechaProceso = ManejoNulos.ManageNullDate(dr["FechaProceso"]),
                                Motivo = ManejoNulos.ManageNullStr(dr["Motivo"]),
                                IdConfSunat = ManejoNulos.ManageNullInteger(dr["IdConfSunat"]),
                                BandBusq = ManejoNulos.ManageNullInteger(dr["BandBusq"]),

                                Cabecera = ManejoNulos.ManageNullStr(dr["Cabecera"]),
                                DGJM = ManejoNulos.ManageNullStr(dr["DGJM"]),
                                CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]),
                                IdColector = ManejoNulos.ManageNullStr(dr["IdColector"]),
                                FechaTrama = ManejoNulos.ManageNullStr(dr["FechaTrama"]),
                                Pccm = ManejoNulos.ManageNullStr(dr["Pccm"]),
                                Pccsuctr = ManejoNulos.ManageNullStr(dr["Pccsuctr"]),
                                Rce = ManejoNulos.ManageNullStr(dr["Rce"]),
                                Embbram = ManejoNulos.ManageNullStr(dr["Embbram"]),
                                Apl = ManejoNulos.ManageNullStr(dr["Apl"]),
                                Fmc = ManejoNulos.ManageNullStr(dr["Fmc"]),
                                Frammr = ManejoNulos.ManageNullStr(dr["Frammr"]),
                                CereoTrama = ManejoNulos.ManageNullStr(dr["CereoTrama"]),
                                Reserva1 = ManejoNulos.ManageNullStr(dr["Reserva1"]),
                                Reserva2 = ManejoNulos.ManageNullStr(dr["Reserva2"]),
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

        public List<EventosSunatEntidad> ObtenerEventosSunatxSala(int codSala, DateTime fechaIni, DateTime fechaFin) {
            List<EventosSunatEntidad> lista = new List<EventosSunatEntidad>();
            string consulta = $@"
                     SELECT es.*, s.Nombre as NombreSala
                     FROM EventosSunat AS es
                     INNER JOIN Sala AS s ON s.CodSala = es.CodSala
                     WHERE es.CodSala = @codSala and convert(DATE,es.Fecha) between @fechaIni and @fechaFin;
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@fechaIni", fechaIni);
                    query.Parameters.AddWithValue("@fechaFin", fechaFin);
                    query.Parameters.AddWithValue("@codSala", codSala);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new EventosSunatEntidad {
                                //IdEventoSunat = ManejoNulos.ManageNullInteger(dr["IdEventoSunat"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Sala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                FechaMigracion = ManejoNulos.ManageNullDate(dr["FechaMigracion"]),

                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Trama = ManejoNulos.ManageNullStr(dr["Trama"]),
                                TipoTrama = ManejoNulos.ManegeNullBool(dr["TipoTrama"]),
                                IdEvSunat = ManejoNulos.ManageNullInteger(dr["IdEvSunat"]),
                                Envio = ManejoNulos.ManageNullInteger(dr["Envio"]),
                                FechaEnvio = ManejoNulos.ManageNullDate(dr["FechaEnvio"]),
                                FechaProceso = ManejoNulos.ManageNullDate(dr["FechaProceso"]),
                                Motivo = ManejoNulos.ManageNullStr(dr["Motivo"]),
                                IdConfSunat = ManejoNulos.ManageNullInteger(dr["IdConfSunat"]),
                                BandBusq = ManejoNulos.ManageNullInteger(dr["BandBusq"]),

                                Cabecera = ManejoNulos.ManageNullStr(dr["Cabecera"]),
                                DGJM = ManejoNulos.ManageNullStr(dr["DGJM"]),
                                CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]),
                                IdColector = ManejoNulos.ManageNullStr(dr["IdColector"]),
                                FechaTrama = ManejoNulos.ManageNullStr(dr["FechaTrama"]),
                                Pccm = ManejoNulos.ManageNullStr(dr["Pccm"]),
                                Pccsuctr = ManejoNulos.ManageNullStr(dr["Pccsuctr"]),
                                Rce = ManejoNulos.ManageNullStr(dr["Rce"]),
                                Embbram = ManejoNulos.ManageNullStr(dr["Embbram"]),
                                Apl = ManejoNulos.ManageNullStr(dr["Apl"]),
                                Fmc = ManejoNulos.ManageNullStr(dr["Fmc"]),
                                Frammr = ManejoNulos.ManageNullStr(dr["Frammr"]),
                                CereoTrama = ManejoNulos.ManageNullStr(dr["CereoTrama"]),
                                Reserva1 = ManejoNulos.ManageNullStr(dr["Reserva1"]),
                                Reserva2 = ManejoNulos.ManageNullStr(dr["Reserva2"]),
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

        public bool EditarEventoSunat(EventosSunatEntidad cliente, int codSala) {
            bool respuesta = false;
            string consulta = @"  UPDATE [dbo].[EventosSunat]
                   SET 
                   [Envio] = @p1,
                   [FechaEnvio] = @p2
				   where IdEvSunat =@IdEvSunat   and CodSala = @codSala ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEvSunat", cliente.IdEvSunat);
                    query.Parameters.AddWithValue("@codSala", codSala);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManegeNullBool(cliente.Envio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(cliente.FechaEnvio));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }

            return respuesta;
        }

        public List<int> ObtenerIdsEventosExistentes(List<int> idsInsertar, int codSala) {
            List<int> idsExistentes = new List<int>();
            string consulta = $@"
                SELECT IdEvSunat
                FROM EventosSunat 
                WHERE CodSala = @codSala AND IdEvSunat IN ({string.Join(",", idsInsertar)})
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();

                    using(SqlCommand query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@codSala", codSala);
                        using(SqlDataReader rd = query.ExecuteReader()) {
                            while(rd.Read()) {
                                idsExistentes.Add(ManejoNulos.ManageNullInteger(rd["IdEvSunat"]));
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return idsExistentes;
        }
        public int ObtenerUltimoIdEventoSunatPorCodSala(int codSala) {
            int ultimoId = 0;
            string consulta = $@"
                SELECT TOP 1 IdEvSunat
                FROM EventosSunat
                WHERE CodSala = @codSala
                ORDER BY IdEvSunat DESC
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(SqlCommand query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@codSala", codSala);
                        ultimoId = Convert.ToInt32(query.ExecuteScalar());
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return ultimoId;
        }
        #endregion

    }
}
