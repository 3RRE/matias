using CapaEntidad.ControlAcceso.HistorialLudopata;
using CapaEntidad.ControlAcceso.HistorialLudopata.Dto;
using CapaEntidad.ControlAcceso.HistorialLudopata.Enum;
using S3k.Utilitario;
using S3k.Utilitario.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace CapaDatos.ControlAcceso {
    public class CAL_HistorialLudopataDAL {
        private readonly string _conexion = string.Empty;

        public CAL_HistorialLudopataDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int InsertarHistorialLudopata(CAL_HistorialLudopata historialLudopata) {
            int idInsertado = 0;
            string consulta = @"
                INSERT INTO CAL_HistorialLudopata(IdLudopata, TipoMovimiento, TipoRegistro, IdUsuario)
                OUTPUT INSERTED.Id
                VALUES(@IdLudopata, @TipoMovimiento, @TipoRegistro, @IdUsuario)
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdLudopata", historialLudopata.IdLudopata);
                    query.Parameters.AddWithValue("@TipoMovimiento", historialLudopata.TipoMovimiento);
                    query.Parameters.AddWithValue("@TipoRegistro", historialLudopata.TipoRegistro);
                    query.Parameters.AddWithValue("@IdUsuario", historialLudopata.IdUsuario == 0 ? SqlInt32.Null : historialLudopata.IdUsuario);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return idInsertado;
        }

        public List<CAL_HistorialLudopataDto> ObtenerHistorialLudopata() {
            List<CAL_HistorialLudopataDto> items = new List<CAL_HistorialLudopataDto>();
            string consulta = @"
                SELECT
                    histoLudo.Id AS Id,
	                ludo.LudopataID AS IdLudopata,
	                ISNULL(TRIM(cliente.NroDoc), TRIM(ludo.DNI)) AS NumeroDocumento,
	                ISNULL(cliente.Nombre, ludo.Nombre) AS Nombres,
	                ISNULL(cliente.ApelPat, ludo.ApellidoPaterno) AS ApellidoPaterno,
	                ISNULL(cliente.ApelMat, ludo.ApellidoMaterno) AS ApellidoMaterno,
	                histoLudo.TipoMovimiento AS TipoMovimiento,
	                histoLudo.TipoRegistro AS TipoRegistro,
	                ISNULL(emple.Nombres, '') AS NombresEmpleado,
	                ISNULL(emple.ApellidosPaterno, '') AS ApellidoPaternoEmpleado,
	                ISNULL(emple.ApellidosMaterno, '') AS ApellidoMaternoEmpleado,
	                histoLudo.FechaRegistro AS FechaRegistro
                FROM CAL_HistorialLudopata AS histoLudo
                INNER JOIN CAL_Ludopata AS ludo ON ludo.LudopataID = histoLudo.IdLudopata
                LEFT JOIN AST_Cliente AS cliente ON cliente.NroDoc = ludo.DNI
                LEFT JOIN SEG_Usuario AS usu ON usu.UsuarioID = histoLudo.IdUsuario
                LEFT JOIN SEG_Empleado AS emple ON emple.EmpleadoID = usu.EmpleadoID
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<CAL_HistorialLudopataDto> ObtenerHistorialLudopataPorFechas(DateTime fechaInicio, DateTime fechaFin) {
            List<CAL_HistorialLudopataDto> items = new List<CAL_HistorialLudopataDto>();
            string consulta = @"
                SELECT
                    histoLudo.Id AS Id,
	                ludo.LudopataID AS IdLudopata,
	                ISNULL(TRIM(cliente.NroDoc), TRIM(ludo.DNI)) AS NumeroDocumento,
	                ISNULL(cliente.Nombre, ludo.Nombre) AS Nombres,
	                ISNULL(cliente.ApelPat, ludo.ApellidoPaterno) AS ApellidoPaterno,
	                ISNULL(cliente.ApelMat, ludo.ApellidoMaterno) AS ApellidoMaterno,
	                histoLudo.TipoMovimiento AS TipoMovimiento,
	                histoLudo.TipoRegistro AS TipoRegistro,
	                ISNULL(emple.Nombres, '') AS NombresEmpleado,
	                ISNULL(emple.ApellidosPaterno, '') AS ApellidoPaternoEmpleado,
	                ISNULL(emple.ApellidosMaterno, '') AS ApellidoMaternoEmpleado,
	                histoLudo.FechaRegistro AS FechaRegistro
                FROM CAL_HistorialLudopata AS histoLudo
                INNER JOIN CAL_Ludopata AS ludo ON ludo.LudopataID = histoLudo.IdLudopata
                LEFT JOIN AST_Cliente AS cliente ON cliente.NroDoc = ludo.DNI
                LEFT JOIN SEG_Usuario AS usu ON usu.UsuarioID = histoLudo.IdUsuario
                INNER JOIN SEG_Empleado AS emple ON emple.EmpleadoID = usu.EmpleadoID
                WHERE (CONVERT(date, histoLudo.FechaRegistro) BETWEEN @FechaInicio AND @FechaFin)
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    query.Parameters.AddWithValue("@FechaFin", fechaFin);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        private CAL_HistorialLudopataDto ConstruirObjeto(SqlDataReader dr) {
            CAL_TipoMovimientoHistorialLudopata tipoMovimiento = (CAL_TipoMovimientoHistorialLudopata)ManejoNulos.ManageNullInteger(dr["TipoMovimiento"]);
            CAL_TipoRegistroHistorialLudopata tipoRegistro = (CAL_TipoRegistroHistorialLudopata)ManejoNulos.ManageNullInteger(dr["TipoRegistro"]);
            return new CAL_HistorialLudopataDto {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                IdLudopata = ManejoNulos.ManageNullInteger(dr["IdLudopata"]),
                NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                TipoMovimiento = tipoMovimiento,
                TipoMovimientoStr = tipoMovimiento.GetDisplayText(),
                TipoRegistro = tipoRegistro,
                TipoRegistroStr = tipoRegistro.GetDisplayText(),
                NombresEmpleado = ManejoNulos.ManageNullStr(dr["NombresEmpleado"]),
                ApellidoPaternoEmpleado = ManejoNulos.ManageNullStr(dr["ApellidoPaternoEmpleado"]),
                ApellidoMaternoEmpleado = ManejoNulos.ManageNullStr(dr["ApellidoMaternoEmpleado"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
            };
        }
    }
}
