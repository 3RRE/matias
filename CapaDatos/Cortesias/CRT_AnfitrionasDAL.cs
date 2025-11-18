using CapaEntidad;
using CapaEntidad.Cortesias;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Cortesias {
    public class CRT_AnfitrionaDAL {
        private readonly string _conexion;
        private readonly string _conexionIAS;

        public CRT_AnfitrionaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion_regasis"].ConnectionString;
            _conexionIAS = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<CRT_Sala> GetSalas() {
            List<CRT_Sala> salas = new List<CRT_Sala>();
            string consulta = @"
                SELECT [sal_id]
                      ,[empresa_id]
                      ,[sal_codigo]
                      ,[sal_nombre]
                      ,[sal_direccion]
                      ,[sal_correo]
                      ,[sal_created]
                      ,[sal_usuario]
                      ,[sal_contrasenia]
                      ,[sal_estado]
                      ,[sal_latitud]
                      ,[sal_longitud]
                  FROM [BD_GESASISv2].[dbo].[Sala]
where sal_nombre not like'%sala%'
            ";

            try {
                using (SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    using (SqlCommand query = new SqlCommand(consulta, con)) {
                        using (SqlDataReader dr = query.ExecuteReader()) {
                            while (dr.Read()) {
                                CRT_Sala sala = new CRT_Sala {
                                    SalId = ManejoNulos.ManageNullInteger(dr["sal_id"]),
                                    EmpresaId = ManejoNulos.ManageNullInteger(dr["empresa_id"]),
                                    SalCodigo = ManejoNulos.ManageNullStr(dr["sal_codigo"]),
                                    SalNombre = ManejoNulos.ManageNullStr(dr["sal_nombre"]),
                                    SalDireccion = ManejoNulos.ManageNullStr(dr["sal_direccion"]),
                                    SalCorreo = ManejoNulos.ManageNullStr(dr["sal_correo"]),
                                    SalCreated = ManejoNulos.ManageNullDate(dr["sal_created"]),
                                    SalUsuario = ManejoNulos.ManageNullStr(dr["sal_usuario"]),
                                    SalContrasenia = ManejoNulos.ManageNullStr(dr["sal_contrasenia"]),
                                    SalEstado = ManejoNulos.ManegeNullBool(dr["sal_estado"]),
                                };

                                salas.Add(sala);
                            }
                        }
                    }
                }
            }
            catch(Exception ex) {
                salas = new List<CRT_Sala>();
            }
            return salas;
        }

        public List<CRT_Empleado> GetAnfitrionas() {
            List<CRT_Empleado> empleados = new List<CRT_Empleado>();
            string consulta = @"
                SELECT
                      [emp_id_buk]
                      ,[empresa]
                      ,sede
                      ,[emp_nombre]
                      ,[emp_apepaterno]
                      ,[emp_apematerno]
                      ,[emp_nrodocumento]
                      ,[co_empr]
                      ,[co_sede]
                      ,[sede]
                      ,[puesto]
                      ,[emp_estado]
                      ,[emp_correo]
                      ,[emp_cesado]
                  FROM [BD_GESASISv2].[dbo].[Empleado]
                  where puesto like '%anfitri%'
            ";

            try {
                using (SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    using (SqlCommand query = new SqlCommand(consulta, con)) {
                        using (SqlDataReader reader = query.ExecuteReader()) {
                            while (reader.Read()) {
                                CRT_Empleado empleado = new CRT_Empleado {
                                    EmpIdBuk = ManejoNulos.ManageNullInteger(reader["emp_id_buk"]),
                                    Empresa = ManejoNulos.ManageNullStr(reader["empresa"]),
                                    Sede = ManejoNulos.ManageNullStr(reader["sede"]),
                                    EmpNombre = ManejoNulos.ManageNullStr(reader["emp_nombre"]),
                                    EmpApePaterno = ManejoNulos.ManageNullStr(reader["emp_apepaterno"]),
                                    EmpApeMaterno = ManejoNulos.ManageNullStr(reader["emp_apematerno"]),
                                    EmpNroDocumento = ManejoNulos.ManageNullStr(reader["emp_nrodocumento"]),
                                    CoEmpr = ManejoNulos.ManageNullStr(reader["co_empr"]),
                                    CoSede = ManejoNulos.ManageNullStr(reader["co_sede"]),
                                    Puesto = ManejoNulos.ManageNullStr(reader["puesto"]),
                                    EmpEstado = ManejoNulos.ManegeNullBool(reader["emp_estado"]),
                                    EmpCorreo = ManejoNulos.ManageNullStr(reader["emp_correo"]),
                                };

                                empleados.Add(empleado);
                            }
                        }
                    }
                }
            }
            catch(Exception ex) {
                empleados = new List<CRT_Empleado>();
            }
            return empleados;
        }
        public List<CRT_Empleado> GetAnfitrionasBySala(string empresa,string sala) {
            List<CRT_Empleado> empleados = new List<CRT_Empleado>();
            string consulta = @"
                SELECT
                      [emp_id_buk]
                      ,[empresa]
                      ,sede
                      ,[emp_nombre]
                      ,[emp_apepaterno]
                      ,[emp_apematerno]
                      ,[emp_nrodocumento]
                      ,[co_empr]
                      ,[co_sede]
                      ,[sede]
                      ,[puesto]
                      ,[emp_estado]
                      ,[emp_correo]
                      ,[emp_cesado]
                  FROM [BD_GESASISv2].[dbo].[Empleado]
                  where puesto like '%anfitri%' and co_sede =@sala and co_empr=@empresa
            ";

            try {
                using (SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    using (SqlCommand query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@empresa", empresa);
                        query.Parameters.AddWithValue("@sala", sala);
                        using (SqlDataReader reader = query.ExecuteReader()) {
                            while (reader.Read()) {
                                CRT_Empleado empleado = new CRT_Empleado {
                                    EmpIdBuk = ManejoNulos.ManageNullInteger(reader["emp_id_buk"]),
                                    Empresa = ManejoNulos.ManageNullStr(reader["empresa"]),
                                    Sede = ManejoNulos.ManageNullStr(reader["sede"]),
                                    EmpNombre = ManejoNulos.ManageNullStr(reader["emp_nombre"]),
                                    EmpApePaterno = ManejoNulos.ManageNullStr(reader["emp_apepaterno"]),
                                    EmpApeMaterno = ManejoNulos.ManageNullStr(reader["emp_apematerno"]),
                                    EmpNroDocumento = ManejoNulos.ManageNullStr(reader["emp_nrodocumento"]),
                                    CoEmpr = ManejoNulos.ManageNullStr(reader["co_empr"]),
                                    CoSede = ManejoNulos.ManageNullStr(reader["co_sede"]),
                                    Puesto = ManejoNulos.ManageNullStr(reader["puesto"]),
                                    EmpEstado = ManejoNulos.ManegeNullBool(reader["emp_estado"]),
                                    EmpCorreo = ManejoNulos.ManageNullStr(reader["emp_correo"])
                                };

                                empleados.Add(empleado);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                empleados = new List<CRT_Empleado>();
            }
            return empleados;
        }
        public CRT_Empleado GetAnfitrionasByNroDoc(string doi) {
            CRT_Empleado empleado = new CRT_Empleado();
            string consulta = @"
                SELECT
                      [emp_id_buk]
                      ,[empresa]
                      ,sede
                      ,[emp_nombre]
                      ,[emp_apepaterno]
                      ,[emp_apematerno]
                      ,[emp_nrodocumento]
                      ,[co_empr]
                      ,[co_sede]
                      ,[sede]
                      ,[puesto]
                      ,[emp_estado]
                      ,[emp_correo]
                      ,[emp_cesado]
                  FROM [BD_GESASISv2].[dbo].[Empleado]
                  where emp_nrodocumento = @doi
            ";

            try {
                using (SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    using (SqlCommand query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@doi", doi);
                        using (SqlDataReader reader = query.ExecuteReader()) {
                            while (reader.Read()) {

                                empleado.EmpIdBuk = ManejoNulos.ManageNullInteger(reader["emp_id_buk"]);
                                empleado.Empresa = ManejoNulos.ManageNullStr(reader["empresa"]);
                                empleado.Sede = ManejoNulos.ManageNullStr(reader["sede"]);
                                empleado.EmpNombre = ManejoNulos.ManageNullStr(reader["emp_nombre"]);
                                empleado.EmpApePaterno = ManejoNulos.ManageNullStr(reader["emp_apepaterno"]);
                                empleado.EmpApeMaterno = ManejoNulos.ManageNullStr(reader["emp_apematerno"]);
                                empleado.EmpNroDocumento = ManejoNulos.ManageNullStr(reader["emp_nrodocumento"]);
                                empleado.CoEmpr = ManejoNulos.ManageNullStr(reader["co_empr"]);
                                empleado.CoSede = ManejoNulos.ManageNullStr(reader["co_sede"]);
                                empleado.Puesto = ManejoNulos.ManageNullStr(reader["puesto"]);
                                empleado.EmpEstado = ManejoNulos.ManegeNullBool(reader["emp_estado"]);
                                empleado.EmpCorreo = ManejoNulos.ManageNullStr(reader["emp_correo"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                empleado = new CRT_Empleado();
            }
            return empleado;
        }
        public List<SalaEntidad> GetSalasByCod(string empresa,string sala) {
            List<SalaEntidad> empleados = new List<SalaEntidad>();
            string consulta = @"
                SELECT [CodSala]
  FROM [Sala]
  where CodEmpresaOfisis = @empresa and CodSalaOfisis = @sala
            ";

            try {
                using (SqlConnection con = new SqlConnection(_conexionIAS)) {
                    con.Open();
                    using (SqlCommand query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@empresa", empresa);
                        query.Parameters.AddWithValue("@sala", sala);
                        using (SqlDataReader reader = query.ExecuteReader()) {
                            while (reader.Read()) {
                                SalaEntidad empleado = new SalaEntidad {
                                    CodSala = ManejoNulos.ManageNullInteger(reader["CodSala"]),
                                };

                                empleados.Add(empleado);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                empleados = new List<SalaEntidad>();
            }
            return empleados;
        }
    }
}
