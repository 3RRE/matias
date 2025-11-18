using CapaEntidad.TransaccionTarjetaCliente.Dto;
using CapaEntidad.TransaccionTarjetaCliente.Entidad;
using CapaEntidad.TransaccionTarjetaCliente.Filtro;
using FastMember;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace CapaDatos.TransaccionTarjetaCliente {
    public class TTC_TransaccionDAL {
        private readonly string _conexion;

        public TTC_TransaccionDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<TTC_TransaccionDto> ObtenerTransaccionesPorCodSala(TTC_TransaccionFiltro filtro) {
            List<TTC_TransaccionDto> items = new List<TTC_TransaccionDto>();
            string consulta = $@"
                SET DATEFORMAT dmy
                DECLARE @CodSalaMaestra INT = 0;
                SELECT @CodSalaMaestra = ISNULL(CodSalaMaestra, 0) FROM Sala WHERE CodSala = @CodSala;

                SELECT
	                tra.Id AS Id,
	                sala.CodSala AS CodSala,
	                sala.Nombre AS NombreSala,
	                ROW_NUMBER() OVER (ORDER BY tra.FechaRegistro DESC) AS ItemVoucherTransaccion,
	                --tra.ItemVoucher AS ItemVoucherTransaccion,
	                tra.Monto AS MontoTransaccion,
	                tra.IdCliente AS IdCliente,
	                tra.NombreCompletoCliente AS NombreCompletoCliente,
	                tra.NumeroDocumentoCliente AS NumeroDocumentoCliente,
	                tra.EntidadEmisora AS EntidadEmisoraTarjeta,
	                tra.MedioPago AS MedioPagoTarjeta,
	                tra.NumeroTarjeta AS NumeroTarjeta,
	                tra.TipoTarjeta AS TipoTarjeta,
	                tra.Caja AS NumeroCaja,
	                tra.Turno AS TurnoCaja,
	                tra.FechaRegistro AS FechaRegistroTransaccion
                FROM TTC_Transaccion AS tra
                INNER JOIN Sala AS sala ON sala.CodSala = tra.CodSala
                WHERE 
	                sala.CodSalaMaestra = @CodSalaMaestra 
	                AND CONVERT(DATE, tra.FechaRegistro) BETWEEN CONVERT(DATE, @FechaInicio) AND CONVERT(DATE, @FechaFin)
                ORDER BY tra.FechaRegistro DESC
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", filtro.CodSala);
                    query.Parameters.AddWithValue("@FechaInicio", filtro.FechaInicio);
                    query.Parameters.AddWithValue("@FechaFin", filtro.FechaFin);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjetoDto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        #region Migracion
        public bool GuardarTransaccionesMasivo(List<TTC_TransaccionEntidad> transacciones) {
            bool success = true;

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    using(SqlBulkCopy bulkCopy = new SqlBulkCopy(connection)) {
                        using(ObjectReader reader = ObjectReader.Create(transacciones)) {
                            // Asignar el nombre de la tabla de destino
                            bulkCopy.DestinationTableName = "TTC_Transaccion";

                            // Configurar el mapeo de las columnas entre la lista y la tabla de destino
                            bulkCopy.ColumnMappings.Add("CodSala", "CodSala");
                            bulkCopy.ColumnMappings.Add("FechaMigracion", "FechaMigracion");
                            bulkCopy.ColumnMappings.Add("IdCliente", "IdCliente");
                            //campos
                            bulkCopy.ColumnMappings.Add("ItemVoucher", "ItemVoucher");
                            bulkCopy.ColumnMappings.Add("NombreCompletoCliente", "NombreCompletoCliente");
                            bulkCopy.ColumnMappings.Add("NumeroDocumentoCliente", "NumeroDocumentoCliente");
                            bulkCopy.ColumnMappings.Add("MedioPago", "MedioPago");
                            bulkCopy.ColumnMappings.Add("EntidadEmisora", "EntidadEmisora");
                            bulkCopy.ColumnMappings.Add("TipoTarjeta", "TipoTarjeta");
                            bulkCopy.ColumnMappings.Add("Monto", "Monto");
                            bulkCopy.ColumnMappings.Add("NumeroTarjeta", "NumeroTarjeta");
                            bulkCopy.ColumnMappings.Add("FechaRegistro", "FechaRegistro");
                            bulkCopy.ColumnMappings.Add("Caja", "Caja");
                            bulkCopy.ColumnMappings.Add("Turno", "Turno");

                            // Realizar la inserción masiva
                            bulkCopy.WriteToServer(reader);
                        }
                    }
                }
            } catch {
                success = false;
            }

            return success;
        }

        public int ObtenerUltimoItemVoucherPorCodSala(int codSala) {
            int ultimoId = 0;
            string consulta = $@"
                DECLARE @ItemVoucher INT = 0;
                DECLARE @CodSalaMaestra INT = 0;
                SELECT @CodSalaMaestra = ISNULL(CodSalaMaestra, 0) FROM Sala WHERE CodSala = @CodSala;

                SELECT @ItemVoucher = ISNULL((
                    SELECT TOP 1 tra.ItemVoucher
                    FROM TTC_Transaccion AS tra
                    INNER JOIN Sala AS sala ON sala.CodSala = tra.CodSala
                    WHERE sala.CodSalaMaestra = @CodSalaMaestra 
                    ORDER BY ItemVoucher DESC
                ), 0);

                SELECT @ItemVoucher AS ItemVoucher;
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(SqlCommand query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@CodSala", codSala);
                        ultimoId = Convert.ToInt32(query.ExecuteScalar());
                    }
                }
            } catch {
            }
            return ultimoId;
        }
        #endregion

        #region Migracion Moldat
        public List<TTC_TransaccionEntidad> ObtenerTransaccionesParaMoldat(TTC_TransaccionMoldatFiltro filtro) {
            List<TTC_TransaccionEntidad> items = new List<TTC_TransaccionEntidad>();
            string consulta = $@"        
                SELECT TOP (@CantidadRegistros)
	                Id,
                    CodSala,
                    IdCliente,
                    ItemVoucher,
                    NombreCompletoCliente,
                    NumeroDocumentoCliente,
                    MedioPago,
                    EntidadEmisora,
                    TipoTarjeta,
                    Monto,
                    NumeroTarjeta,
                    FechaRegistro,
                    Caja,
                    Turno
                FROM TTC_Transaccion
                WHERE FechaMigracionMoldat IS NULL
                ORDER BY Id ASC
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CantidadRegistros", filtro.CantidadRegistros);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public int ActualizarEstadoMigracionMoldatPorId(int id, DateTime? fechaMigracionMoldat) {
            int idActualizado = 0;
            string consulta = $@"        
                UPDATE TTC_Transaccion
                SET FechaMigracionMoldat = @FechaMigracionMoldat
                OUTPUT inserted.Id
                WHERE Id = @Id
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    query.Parameters.AddWithValue("@FechaMigracionMoldat", fechaMigracionMoldat ?? SqlDateTime.Null);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch { }
            return idActualizado;
        }
        #endregion

        private TTC_TransaccionDto ConstruirObjetoDto(SqlDataReader dr) {
            return new TTC_TransaccionDto {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Sala = new TTC_SalaDto {
                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                },
                ItemVoucher = ManejoNulos.ManageNullInteger(dr["ItemVoucherTransaccion"]),
                Monto = ManejoNulos.ManageNullDecimal(dr["MontoTransaccion"]),
                Cliente = new TTC_Cliente {
                    Id = ManejoNulos.ManageNullInteger(dr["IdCliente"]),
                    NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompletoCliente"]),
                    NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumentoCliente"]),
                },
                Tarjeta = new TTC_Tarjeta {
                    EntidadEmisora = ManejoNulos.ManageNullStr(dr["EntidadEmisoraTarjeta"]),
                    MedioPago = ManejoNulos.ManageNullStr(dr["MedioPagoTarjeta"]),
                    Numero = ManejoNulos.ManageNullStr(dr["NumeroTarjeta"]),
                    Tipo = ManejoNulos.ManageNullStr(dr["TipoTarjeta"]),
                },
                Caja = new TTC_CajaDto {
                    Numero = ManejoNulos.ManageNullInteger(dr["NumeroCaja"]),
                    Turno = ManejoNulos.ManageNullInteger(dr["TurnoCaja"]),
                },
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistroTransaccion"])
            };
        }

        private TTC_TransaccionEntidad ConstruirObjeto(SqlDataReader dr) {
            return new TTC_TransaccionEntidad {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                ItemVoucher = ManejoNulos.ManageNullInteger(dr["ItemVoucher"]),
                NombreCompletoCliente = ManejoNulos.ManageNullStr(dr["NombreCompletoCliente"]),
                NumeroDocumentoCliente = ManejoNulos.ManageNullStr(dr["NumeroDocumentoCliente"]),
                MedioPago = ManejoNulos.ManageNullStr(dr["MedioPago"]),
                EntidadEmisora = ManejoNulos.ManageNullStr(dr["EntidadEmisora"]),
                TipoTarjeta = ManejoNulos.ManageNullStr(dr["TipoTarjeta"]),
                Monto = ManejoNulos.ManageNullDecimal(dr["Monto"]),
                NumeroTarjeta = ManejoNulos.ManageNullStr(dr["NumeroTarjeta"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                Caja = ManejoNulos.ManageNullInteger(dr["Caja"]),
                Turno = ManejoNulos.ManageNullInteger(dr["Turno"]),
            };
        }
    }
}
