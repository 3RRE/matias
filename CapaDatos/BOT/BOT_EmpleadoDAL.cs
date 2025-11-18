using CapaEntidad.BOT.Entities;
using CapaEntidad.BOT.Enum;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Cortesias {
    public class BOT_EmpleadoDAL {
        private readonly string _conexion;

        public BOT_EmpleadoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarEmpleado(BOT_EmpleadoEntidad empleado) {
            int idActualizado;
            string consulta = @"
                UPDATE BOT_Empleado
                SET
                    NumeroDocumento = @NumeroDocumento,
                    CodigoPaisCelular = @CodigoPaisCelular,
                    TelefonoParticular = @TelefonoParticular,
                    TipoDocumento = @TipoDocumento,
                    Nombres = @Nombres,
                    ApellidoPaterno = @ApellidoPaterno,
                    ApellidoMaterno = @ApellidoMaterno,
                    NombreCompleto = @NombreCompleto,
                    Genero = @Genero,
                    Nacionalidad = @Nacionalidad,
                    FechaNacimiento = @FechaNacimiento,
                    EstadoCivil = @EstadoCivil,
                    Direccion = @Direccion,
                    Departamento = @Departamento,
                    Provincia = @Provincia,
                    Distrito = @Distrito,
                    TelefonoOficina = @TelefonoOficina,
                    Email = @Email,
                    EmailPersonal = @EmailPersonal,
                    SituacionEducativa = @SituacionEducativa,
                    CodigoFicha = @CodigoFicha,
                    FechaIncorporacion = @FechaIncorporacion,
                    FormaPago = @FormaPago,
                    Banco = @Banco,
                    TipoCuenta = @TipoCuenta,
                    NumeroCuenta = @NumeroCuenta,
                    RegimenPensionario = @RegimenPensionario,
                    MonedaCts = @MonedaCts,
                    FechaInicioTrabajo = @FechaInicioTrabajo,
                    TipoContrato = @TipoContrato,
                    FechaFinTrabajo = @FechaFinTrabajo,
                    PlanEps = @PlanEps,
                    Empresa = @Empresa,
                    Area = @Area,
                    Cargo = @Cargo,
                    Sede = @Sede,
                    FechaModificacion = GETDATE()
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", empleado.Id);

                    query.Parameters.AddWithValue("@NumeroDocumento", ManejoNulos.ManageNullStr(empleado.NumeroDocumento));
                    query.Parameters.AddWithValue("@CodigoPaisCelular", ManejoNulos.ManageNullStr(empleado.CodigoPaisCelular));
                    query.Parameters.AddWithValue("@TelefonoParticular", ManejoNulos.ManageNullStr(empleado.TelefonoParticular));

                    query.Parameters.AddWithValue("@TipoDocumento", ManejoNulos.ManageNullStr(empleado.TipoDocumento));
                    query.Parameters.AddWithValue("@Nombres", ManejoNulos.ManageNullStr(empleado.Nombres));
                    query.Parameters.AddWithValue("@ApellidoPaterno", ManejoNulos.ManageNullStr(empleado.ApellidoPaterno));
                    query.Parameters.AddWithValue("@ApellidoMaterno", ManejoNulos.ManageNullStr(empleado.ApellidoMaterno));
                    query.Parameters.AddWithValue("@NombreCompleto", ManejoNulos.ManageNullStr(empleado.NombreCompleto));
                    query.Parameters.AddWithValue("@Genero", ManejoNulos.ManageNullStr(empleado.Genero));
                    query.Parameters.AddWithValue("@Nacionalidad", ManejoNulos.ManageNullStr(empleado.Nacionalidad));
                    query.Parameters.AddWithValue("@FechaNacimiento", (object)empleado.FechaNacimiento ?? DBNull.Value);

                    query.Parameters.AddWithValue("@EstadoCivil", ManejoNulos.ManageNullStr(empleado.EstadoCivil));
                    query.Parameters.AddWithValue("@Direccion", ManejoNulos.ManageNullStr(empleado.Direccion));
                    query.Parameters.AddWithValue("@Departamento", ManejoNulos.ManageNullStr(empleado.Departamento));
                    query.Parameters.AddWithValue("@Provincia", ManejoNulos.ManageNullStr(empleado.Provincia));
                    query.Parameters.AddWithValue("@Distrito", ManejoNulos.ManageNullStr(empleado.Distrito));
                    query.Parameters.AddWithValue("@TelefonoOficina", ManejoNulos.ManageNullStr(empleado.TelefonoOficina));
                    query.Parameters.AddWithValue("@Email", ManejoNulos.ManageNullStr(empleado.Email));
                    query.Parameters.AddWithValue("@EmailPersonal", ManejoNulos.ManageNullStr(empleado.EmailPersonal));
                    query.Parameters.AddWithValue("@SituacionEducativa", ManejoNulos.ManageNullStr(empleado.SituacionEducativa));
                    query.Parameters.AddWithValue("@CodigoFicha", ManejoNulos.ManageNullStr(empleado.CodigoFicha));
                    query.Parameters.AddWithValue("@FechaIncorporacion", (object)empleado.FechaIncorporacion ?? DBNull.Value);

                    query.Parameters.AddWithValue("@FormaPago", ManejoNulos.ManageNullStr(empleado.FormaPago));
                    query.Parameters.AddWithValue("@Banco", ManejoNulos.ManageNullStr(empleado.Banco));
                    query.Parameters.AddWithValue("@TipoCuenta", ManejoNulos.ManageNullStr(empleado.TipoCuenta));
                    query.Parameters.AddWithValue("@NumeroCuenta", ManejoNulos.ManageNullStr(empleado.NumeroCuenta));
                    query.Parameters.AddWithValue("@RegimenPensionario", ManejoNulos.ManageNullStr(empleado.RegimenPensionario));
                    query.Parameters.AddWithValue("@MonedaCts", ManejoNulos.ManageNullStr(empleado.MonedaCts));

                    query.Parameters.AddWithValue("@FechaInicioTrabajo", (object)empleado.FechaInicioTrabajo ?? DBNull.Value);
                    query.Parameters.AddWithValue("@TipoContrato", ManejoNulos.ManageNullStr(empleado.TipoContrato));
                    query.Parameters.AddWithValue("@FechaFinTrabajo", (object)empleado.FechaFinTrabajo ?? DBNull.Value);
                    query.Parameters.AddWithValue("@PlanEps", ManejoNulos.ManageNullStr(empleado.PlanEps));

                    query.Parameters.AddWithValue("@Empresa", ManejoNulos.ManageNullStr(empleado.Empresa));
                    query.Parameters.AddWithValue("@Area", ManejoNulos.ManageNullStr(empleado.Area));
                    query.Parameters.AddWithValue("@Cargo", ManejoNulos.ManageNullStr(empleado.Cargo));
                    query.Parameters.AddWithValue("@Sede", ManejoNulos.ManageNullStr(empleado.Sede));
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarEmpleado(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM BOT_Empleado
                OUTPUT DELETED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    idEliminado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idEliminado = 0;
            }
            return idEliminado;
        }

        public int InsertarEmpleado(BOT_EmpleadoEntidad empleado) {
            int idInsertado;
            string consulta = @"
                INSERT INTO BOT_Empleado (
                    NumeroDocumento, CodigoPaisCelular, TelefonoParticular, Estado, OrigenRegistro,
                    TipoDocumento, Nombres, ApellidoPaterno, ApellidoMaterno, NombreCompleto, Genero, Nacionalidad, FechaNacimiento,
                    EstadoCivil, Direccion, Departamento, Provincia, Distrito, TelefonoOficina,
                    Email, EmailPersonal, SituacionEducativa, CodigoFicha, FechaIncorporacion,
                    FormaPago, Banco, TipoCuenta, NumeroCuenta, RegimenPensionario, MonedaCts,
                    FechaInicioTrabajo, TipoContrato, FechaFinTrabajo, PlanEps, Empresa, Area, Cargo, Sede
                )
                OUTPUT Inserted.Id
                VALUES (
                    @NumeroDocumento, @CodigoPaisCelular, @TelefonoParticular, @Estado, @OrigenRegistro,
                    @TipoDocumento, @Nombres, @ApellidoPaterno, @ApellidoMaterno, @NombreCompleto, @Genero, @Nacionalidad, @FechaNacimiento,
                    @EstadoCivil, @Direccion, @Departamento, @Provincia, @Distrito, @TelefonoOficina,
                    @Email, @EmailPersonal, @SituacionEducativa, @CodigoFicha, @FechaIncorporacion,
                    @FormaPago, @Banco, @TipoCuenta, @NumeroCuenta, @RegimenPensionario, @MonedaCts,
                    @FechaInicioTrabajo, @TipoContrato, @FechaFinTrabajo, @PlanEps, @Empresa, @Area, @Cargo, @Sede
                );
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@NumeroDocumento", (object)empleado.NumeroDocumento ?? DBNull.Value);
                    query.Parameters.AddWithValue("@CodigoPaisCelular", ManejoNulos.ManageNullStr(empleado.CodigoPaisCelular));
                    query.Parameters.AddWithValue("@TelefonoParticular", (object)empleado.TelefonoParticular ?? DBNull.Value);
                    query.Parameters.AddWithValue("@Estado", empleado.Estado);
                    query.Parameters.AddWithValue("@OrigenRegistro", BOT_OrigenRegistro.Manual);

                    query.Parameters.AddWithValue("@TipoDocumento", ManejoNulos.ManageNullStr(empleado.TipoDocumento));
                    query.Parameters.AddWithValue("@Nombres", ManejoNulos.ManageNullStr(empleado.Nombres));
                    query.Parameters.AddWithValue("@ApellidoPaterno", ManejoNulos.ManageNullStr(empleado.ApellidoPaterno));
                    query.Parameters.AddWithValue("@ApellidoMaterno", ManejoNulos.ManageNullStr(empleado.ApellidoMaterno));
                    query.Parameters.AddWithValue("@NombreCompleto", ManejoNulos.ManageNullStr(empleado.NombreCompleto));
                    query.Parameters.AddWithValue("@Genero", ManejoNulos.ManageNullStr(empleado.Genero));
                    query.Parameters.AddWithValue("@Nacionalidad", ManejoNulos.ManageNullStr(empleado.Nacionalidad));
                    query.Parameters.AddWithValue("@FechaNacimiento", (object)empleado.FechaNacimiento ?? DBNull.Value);

                    query.Parameters.AddWithValue("@EstadoCivil", ManejoNulos.ManageNullStr(empleado.EstadoCivil));
                    query.Parameters.AddWithValue("@Direccion", ManejoNulos.ManageNullStr(empleado.Direccion));
                    query.Parameters.AddWithValue("@Departamento", ManejoNulos.ManageNullStr(empleado.Departamento));
                    query.Parameters.AddWithValue("@Provincia", ManejoNulos.ManageNullStr(empleado.Provincia));
                    query.Parameters.AddWithValue("@Distrito", ManejoNulos.ManageNullStr(empleado.Distrito));
                    query.Parameters.AddWithValue("@TelefonoOficina", ManejoNulos.ManageNullStr(empleado.TelefonoOficina));
                    query.Parameters.AddWithValue("@Email", ManejoNulos.ManageNullStr(empleado.Email));
                    query.Parameters.AddWithValue("@EmailPersonal", ManejoNulos.ManageNullStr(empleado.EmailPersonal));
                    query.Parameters.AddWithValue("@SituacionEducativa", ManejoNulos.ManageNullStr(empleado.SituacionEducativa));
                    query.Parameters.AddWithValue("@CodigoFicha", ManejoNulos.ManageNullStr(empleado.CodigoFicha));
                    query.Parameters.AddWithValue("@FechaIncorporacion", (object)empleado.FechaIncorporacion ?? DBNull.Value);

                    query.Parameters.AddWithValue("@FormaPago", ManejoNulos.ManageNullStr(empleado.FormaPago));
                    query.Parameters.AddWithValue("@Banco", ManejoNulos.ManageNullStr(empleado.Banco));
                    query.Parameters.AddWithValue("@TipoCuenta", ManejoNulos.ManageNullStr(empleado.TipoCuenta));
                    query.Parameters.AddWithValue("@NumeroCuenta", ManejoNulos.ManageNullStr(empleado.NumeroCuenta));
                    query.Parameters.AddWithValue("@RegimenPensionario", ManejoNulos.ManageNullStr(empleado.RegimenPensionario));
                    query.Parameters.AddWithValue("@MonedaCts", ManejoNulos.ManageNullStr(empleado.MonedaCts));

                    query.Parameters.AddWithValue("@FechaInicioTrabajo", (object)empleado.FechaInicioTrabajo ?? DBNull.Value);
                    query.Parameters.AddWithValue("@TipoContrato", ManejoNulos.ManageNullStr(empleado.TipoContrato));
                    query.Parameters.AddWithValue("@FechaFinTrabajo", (object)empleado.FechaFinTrabajo ?? DBNull.Value);
                    query.Parameters.AddWithValue("@PlanEps", ManejoNulos.ManageNullStr(empleado.PlanEps));

                    query.Parameters.AddWithValue("@Empresa", ManejoNulos.ManageNullStr(empleado.Empresa));
                    query.Parameters.AddWithValue("@Area", ManejoNulos.ManageNullStr(empleado.Area));
                    query.Parameters.AddWithValue("@Cargo", ManejoNulos.ManageNullStr(empleado.Cargo));
                    query.Parameters.AddWithValue("@Sede", ManejoNulos.ManageNullStr(empleado.Sede));

                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public BOT_EmpleadoEntidad ObtenerEmpleadoPorId(int id) {
            BOT_EmpleadoEntidad item = new BOT_EmpleadoEntidad();
            string consulta = @"
                SELECT
                    e.*, 
	                ISNULL(c.Id, 0) as IdCargo
                FROM BOT_Empleado AS e
                LEFT JOIN BOT_Area AS a ON e.Area = a.Nombre
                LEFT JOIN BOT_Cargo AS c ON e.Cargo = c.Nombre AND c.IdArea = a.Id
                WHERE e.Id = @Id
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = MapearEmpleado(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        public BOT_EmpleadoEntidad ObtenerEmpleadoPorNumeroDocumento(string documentNumber) {
            BOT_EmpleadoEntidad item = new BOT_EmpleadoEntidad();
            string consulta = @"
                SELECT 
	                e.*, 
	                ISNULL(c.Id, 0) as IdCargo
                FROM BOT_Empleado AS e
                LEFT JOIN BOT_Area AS a ON e.Area = a.Nombre
                LEFT JOIN BOT_Cargo AS c ON e.Cargo = c.Nombre AND c.IdArea = a.Id
                WHERE NumeroDocumento = @NumeroDocumento
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@NumeroDocumento", documentNumber);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = MapearEmpleado(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<BOT_EmpleadoEntidad> ObtenerEmpleados() {
            List<BOT_EmpleadoEntidad> items = new List<BOT_EmpleadoEntidad>();
            string consulta = @"
                SELECT
                    e.*, 
	                ISNULL(c.Id, 0) as IdCargo
                FROM BOT_Empleado AS e
                LEFT JOIN BOT_Area AS a ON e.Area = a.Nombre
                LEFT JOIN BOT_Cargo AS c ON e.Cargo = c.Nombre AND c.IdArea = a.Id
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            BOT_EmpleadoEntidad item = MapearEmpleado(dr);
                            items.Add(item);
                        }
                    }
                }
            } catch { }
            return items;
        }

        #region Helper
        private BOT_EmpleadoEntidad MapearEmpleado(SqlDataReader dr) {
            return new BOT_EmpleadoEntidad {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                IdBuk = ManejoNulos.ManageNullInteger(dr["IdBuk"]),
                NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                CodigoPaisCelular = ManejoNulos.ManageNullStr(dr["CodigoPaisCelular"]),
                TelefonoParticular = ManejoNulos.ManageNullStr(dr["TelefonoParticular"]),
                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                OrigenRegistro = (BOT_OrigenRegistro)ManejoNulos.ManageNullInteger(dr["OrigenRegistro"]),
                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                Genero = ManejoNulos.ManageNullStr(dr["Genero"]),
                Nacionalidad = ManejoNulos.ManageNullStr(dr["Nacionalidad"]),
                FechaNacimiento = dr["FechaNacimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaNacimiento"]),
                EstadoCivil = ManejoNulos.ManageNullStr(dr["EstadoCivil"]),
                Direccion = ManejoNulos.ManageNullStr(dr["Direccion"]),
                Departamento = ManejoNulos.ManageNullStr(dr["Departamento"]),
                Provincia = ManejoNulos.ManageNullStr(dr["Provincia"]),
                Distrito = ManejoNulos.ManageNullStr(dr["Distrito"]),
                TelefonoOficina = ManejoNulos.ManageNullStr(dr["TelefonoOficina"]),
                Email = ManejoNulos.ManageNullStr(dr["Email"]),
                EmailPersonal = ManejoNulos.ManageNullStr(dr["EmailPersonal"]),
                SituacionEducativa = ManejoNulos.ManageNullStr(dr["SituacionEducativa"]),
                CodigoFicha = ManejoNulos.ManageNullStr(dr["CodigoFicha"]),
                FechaIncorporacion = dr["FechaIncorporacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaIncorporacion"]),
                FormaPago = ManejoNulos.ManageNullStr(dr["FormaPago"]),
                Banco = ManejoNulos.ManageNullStr(dr["Banco"]),
                TipoCuenta = ManejoNulos.ManageNullStr(dr["TipoCuenta"]),
                NumeroCuenta = ManejoNulos.ManageNullStr(dr["NumeroCuenta"]),
                RegimenPensionario = ManejoNulos.ManageNullStr(dr["RegimenPensionario"]),
                MonedaCts = ManejoNulos.ManageNullStr(dr["MonedaCts"]),
                FechaInicioTrabajo = dr["FechaInicioTrabajo"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaInicioTrabajo"]),
                TipoContrato = ManejoNulos.ManageNullStr(dr["TipoContrato"]),
                FechaFinTrabajo = dr["FechaFinTrabajo"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaFinTrabajo"]),
                PlanEps = ManejoNulos.ManageNullStr(dr["PlanEps"]),
                IdEmpresaBuk = ManejoNulos.ManageNullInteger(dr["IdEmpresaBuk"]),
                Empresa = ManejoNulos.ManageNullStr(dr["Empresa"]),
                IdAreaBuk = ManejoNulos.ManageNullInteger(dr["IdAreaBuk"]),
                Area = ManejoNulos.ManageNullStr(dr["Area"]),
                IdCargoBuk = ManejoNulos.ManageNullInteger(dr["IdCargoBuk"]),
                Cargo = ManejoNulos.ManageNullStr(dr["Cargo"]),
                Sede = ManejoNulos.ManageNullStr(dr["Sede"]),
                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"])
            };
        }
        #endregion
    }
}
