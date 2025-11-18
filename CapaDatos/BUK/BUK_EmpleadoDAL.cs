using CapaEntidad.BUK;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.BUK {
    public class BUK_EmpleadoDAL {
        string _conexion = string.Empty;

        public BUK_EmpleadoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<BUK_EmpleadoEntidad> ObtenerEmpleadosActivos(int idempresa) {
            List<BUK_EmpleadoEntidad> lista = new List<BUK_EmpleadoEntidad>();
            string consulta = @"
                SELECT [IdBuk]
                  ,[TipoDocumento]
                  ,[NumeroDocumento]
                  ,[Nombres]
                  ,[ApellidoPaterno]
                  ,[ApellidoMaterno]
                  ,[NombreCompleto]
                  ,[IdCargo]
                  ,[Cargo]
                  ,[IdEmpresa]
                  ,[Empresa]
                  ,[FechaCese]
                  ,[EstadoCese]
              FROM [dbo].[BUK_Empleado] where (EstadoCese = 0 or EstadoCese is null) and IdEmpresa = @IdEmpresa
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEmpresa", idempresa);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new BUK_EmpleadoEntidad {
                                IdBuk= ManejoNulos.ManageNullInteger(dr["IdBuk"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                                Cargo = ManejoNulos.ManageNullStr(dr["Cargo"]),
                                IdEmpresa = ManejoNulos.ManageNullInteger(dr["IdEmpresa"]),
                                Empresa = ManejoNulos.ManageNullStr(dr["Empresa"]),
                                FechaCese = ManejoNulos.ManageNullDate(dr["FechaCese"]),
                                EstadoCese = ManejoNulos.ManegeNullBool(dr["EstadoCese"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                return new List<BUK_EmpleadoEntidad>();
            }
            return lista;
        }
        public List<BUK_EmpleadoEntidad> ObtenerEmpleadosActivosPorTermino(int idempresa, string term) {
            List<BUK_EmpleadoEntidad> lista = new List<BUK_EmpleadoEntidad>();
            string consulta = $@"
                SELECT top 10 [IdBuk]
                  ,[TipoDocumento]
                  ,[NumeroDocumento]
                  ,[Nombres]
                  ,[ApellidoPaterno]
                  ,[ApellidoMaterno]
                  ,[NombreCompleto]
                  ,[IdCargo]
                  ,[Cargo]
                  ,[IdEmpresa]
                  ,[Empresa]
                  ,[FechaCese]
                  ,[EstadoCese]
              FROM [dbo].[BUK_Empleado] where (EstadoCese = 0 or EstadoCese is null) and IdEmpresa = @IdEmpresa
and (NombreCompleto like '%{term}%' or NumeroDocumento like '%{term}%')
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEmpresa", idempresa);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new BUK_EmpleadoEntidad {
                                IdBuk = ManejoNulos.ManageNullInteger(dr["IdBuk"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                                Cargo = ManejoNulos.ManageNullStr(dr["Cargo"]),
                                IdEmpresa = ManejoNulos.ManageNullInteger(dr["IdEmpresa"]),
                                Empresa = ManejoNulos.ManageNullStr(dr["Empresa"]),
                                FechaCese = ManejoNulos.ManageNullDate(dr["FechaCese"]),
                                EstadoCese = ManejoNulos.ManegeNullBool(dr["EstadoCese"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                return new List<BUK_EmpleadoEntidad>();
            }
            return lista;
        }
        public List<BUK_EmpleadoEntidad> ObtenerEmpleadosActivosPorTerminoSinEmpresa(string term) {
            List<BUK_EmpleadoEntidad> lista = new List<BUK_EmpleadoEntidad>();
            string consulta = $@"
                SELECT top 10 [IdBuk]
                  ,[TipoDocumento]
                  ,[NumeroDocumento]
                  ,[Nombres]
                  ,[ApellidoPaterno]
                  ,[ApellidoMaterno]
                  ,[NombreCompleto]
                  ,[IdCargo]
                  ,[Cargo]
                  ,[IdEmpresa]
                  ,[Empresa]
                  ,[FechaCese]
                  ,[EstadoCese]
              FROM [dbo].[BUK_Empleado] where (EstadoCese = 0 or EstadoCese is null)
and (NombreCompleto like '%{term}%' or NumeroDocumento like '%{term}%')
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new BUK_EmpleadoEntidad {
                                IdBuk = ManejoNulos.ManageNullInteger(dr["IdBuk"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                                Cargo = ManejoNulos.ManageNullStr(dr["Cargo"]),
                                IdEmpresa = ManejoNulos.ManageNullInteger(dr["IdEmpresa"]),
                                Empresa = ManejoNulos.ManageNullStr(dr["Empresa"]),
                                FechaCese = ManejoNulos.ManageNullDate(dr["FechaCese"]),
                                EstadoCese = ManejoNulos.ManegeNullBool(dr["EstadoCese"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                return new List<BUK_EmpleadoEntidad>();
            }
            return lista;
        }
        public List<BUK_EmpleadoEntidad> ObtenerEmpleadosActivosPorTerminoxCargo(string term, int[] idcargo) {
            string strCargo = string.Empty;
            strCargo = $" IdCargo in ({String.Join(",", idcargo)}) ";
            List<BUK_EmpleadoEntidad> lista = new List<BUK_EmpleadoEntidad>();
            string consulta = $@"
                SELECT top 10 [IdBuk]
                  ,[TipoDocumento]
                  ,[NumeroDocumento]
                  ,[Nombres]
                  ,[ApellidoPaterno]
                  ,[ApellidoMaterno]
                  ,[NombreCompleto]
                  ,[IdCargo]
                  ,[Cargo]
                  ,[IdEmpresa]
                  ,[Empresa]
                  ,[FechaCese]
                  ,[EstadoCese]
              FROM [dbo].[BUK_Empleado] where (EstadoCese = 0 or EstadoCese is null)
and {strCargo}
and (NombreCompleto like '%{term}%' or NumeroDocumento like '%{term}%')
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new BUK_EmpleadoEntidad {
                                IdBuk = ManejoNulos.ManageNullInteger(dr["IdBuk"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                IdCargo = ManejoNulos.ManageNullInteger(dr["IdCargo"]),
                                Cargo = ManejoNulos.ManageNullStr(dr["Cargo"]),
                                IdEmpresa = ManejoNulos.ManageNullInteger(dr["IdEmpresa"]),
                                Empresa = ManejoNulos.ManageNullStr(dr["Empresa"]),
                                FechaCese = ManejoNulos.ManageNullDate(dr["FechaCese"]),
                                EstadoCese = ManejoNulos.ManegeNullBool(dr["EstadoCese"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                return new List<BUK_EmpleadoEntidad>();
            }
            return lista;
        }
    }
}
