using CapaEntidad.ControlAcceso;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.ControlAcceso {
    public class CAL_PersonaProhibidoIngresoIncidenciaDAL {

        private readonly string conexion;

        public CAL_PersonaProhibidoIngresoIncidenciaDAL() {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> GetAllTimadorIncidencia() {
            List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> lista = new List<CAL_PersonaProhibidoIngresoIncidenciaEntidad>();
            string consulta = @"SELECT tim.[TimadorIncidenciaID]                         
                              ,tim.[TimadorID]            
                              ,tim.[Estado]    
                              ,tim.[FechaRegistro]
                              ,tim.[EmpleadoID] 
                              ,tim.[CodSala]
                              ,tim.[Observacion]
                              ,tim.[SustentoLegal]
                                ,emp.[Nombres] AS [EmpleadoNombres]
                                ,emp.[ApellidosPaterno] AS [EmpleadoApellidoPaterno]
                                ,sal.[Nombre] AS [SalaNombre]
                                FROM [dbo].[CAL_TimadorIncidencia] tim
                                INNER JOIN [dbo].[SEG_Empleado] emp ON tim.EmpleadoID = emp.EmpleadoID
                                INNER JOIN [dbo].[Sala] sal ON tim.CodSala = sal.CodSala  ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new CAL_PersonaProhibidoIngresoIncidenciaEntidad {
                                TimadorIncidenciaID = ManejoNulos.ManageNullInteger(dr["TimadorIncidenciaID"]),
                                TimadorID = ManejoNulos.ManageNullInteger(dr["TimadorID"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Observacion = ManejoNulos.ManageNullStr(dr["Observacion"]),
                                SustentoLegal = ManejoNulos.ManageNullInteger(dr["SustentoLegal"]),

                                EmpleadoNombres = ManejoNulos.ManageNullStr(dr["EmpleadoNombres"]),
                                EmpleadoApellidoPaterno = ManejoNulos.ManageNullStr(dr["EmpleadoApellidoPaterno"]),
                                SalaNombre = ManejoNulos.ManageNullStr(dr["SalaNombre"]),
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
        
        public List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> GetAllTimadorIncidenciaxTimador(int id, string codsSalas) {
            List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> lista = new List<CAL_PersonaProhibidoIngresoIncidenciaEntidad>();
            string consulta = $@"
                DECLARE @SalasMaestras TABLE (
                    CodSalaMaestra INT
                );

                INSERT INTO @SalasMaestras (CodSalaMaestra)
                SELECT DISTINCT CodSalaMaestra
                FROM Sala as sala
                WHERE sala.CodSala IN ({codsSalas});

                SELECT 
	                inci.TimadorIncidenciaID,
	                inci.TimadorID,
	                inci.Estado,
	                inci.FechaRegistro,
	                inci.EmpleadoID,
	                inci.CodSala,
	                inci.Observacion,
	                inci.SustentoLegal,
	                emp.Nombres AS EmpleadoNombres,
	                emp.ApellidosPaterno AS EmpleadoApellidoPaterno,
	                sal.Nombre AS SalaNombre
                FROM CAL_TimadorIncidencia AS inci
                INNER JOIN SEG_Empleado AS emp ON inci.EmpleadoID = emp.EmpleadoID
                INNER JOIN Sala AS sal ON inci.CodSala = sal.CodSala 
                WHERE 
	                inci.TimadorID = @pTimadorID
	                AND sal.CodSalaMaestra IN (SELECT CodSalaMaestra FROM @SalasMaestras)
            ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTimadorID", id);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new CAL_PersonaProhibidoIngresoIncidenciaEntidad {
                                TimadorIncidenciaID = ManejoNulos.ManageNullInteger(dr["TimadorIncidenciaID"]),
                                TimadorID = ManejoNulos.ManageNullInteger(dr["TimadorID"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Observacion = ManejoNulos.ManageNullStr(dr["Observacion"]),
                                SustentoLegal = ManejoNulos.ManageNullInteger(dr["SustentoLegal"]),
                                EmpleadoNombres = ManejoNulos.ManageNullStr(dr["EmpleadoNombres"]),
                                EmpleadoApellidoPaterno = ManejoNulos.ManageNullStr(dr["EmpleadoApellidoPaterno"]),
                                SalaNombre = ManejoNulos.ManageNullStr(dr["SalaNombre"]),
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
        
        public List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> GetAllTimadorIncidenciaxTimadorActivo(int id, string codsSalas) {
            List<CAL_PersonaProhibidoIngresoIncidenciaEntidad> lista = new List<CAL_PersonaProhibidoIngresoIncidenciaEntidad>();
            string consulta = $@"
                DECLARE @SalasMaestras TABLE (
                    CodSalaMaestra INT
                );

                INSERT INTO @SalasMaestras (CodSalaMaestra)
                SELECT DISTINCT CodSalaMaestra
                FROM Sala as sala
                WHERE sala.CodSala IN ({codsSalas});

                SELECT 
	                inci.TimadorIncidenciaID,
	                inci.TimadorID,
	                inci.Estado,
	                inci.FechaRegistro,
	                inci.EmpleadoID,
	                inci.CodSala,
	                inci.Observacion,
	                inci.SustentoLegal,
	                emp.Nombres AS EmpleadoNombres,
	                emp.ApellidosPaterno AS EmpleadoApellidoPaterno,
	                sal.Nombre AS SalaNombre
                FROM CAL_TimadorIncidencia AS inci
                INNER JOIN SEG_Empleado AS emp ON inci.EmpleadoID = emp.EmpleadoID
                INNER JOIN Sala AS sal ON inci.CodSala = sal.CodSala 
                WHERE 
	                inci.TimadorID = @pTimadorID
	                AND sal.CodSalaMaestra IN (SELECT CodSalaMaestra FROM @SalasMaestras)
                    AND inci.Estado = 1
            ";
            try {
                using(SqlConnection con = new SqlConnection(conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTimadorID", id);

                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            CAL_PersonaProhibidoIngresoIncidenciaEntidad item = new CAL_PersonaProhibidoIngresoIncidenciaEntidad {
                                TimadorIncidenciaID = ManejoNulos.ManageNullInteger(dr["TimadorIncidenciaID"]),
                                TimadorID = ManejoNulos.ManageNullInteger(dr["TimadorID"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                EmpleadoID = ManejoNulos.ManageNullInteger(dr["EmpleadoID"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Observacion = ManejoNulos.ManageNullStr(dr["Observacion"]),
                                SustentoLegal = ManejoNulos.ManageNullInteger(dr["SustentoLegal"]),
                                EmpleadoNombres = ManejoNulos.ManageNullStr(dr["EmpleadoNombres"]),
                                EmpleadoApellidoPaterno = ManejoNulos.ManageNullStr(dr["EmpleadoApellidoPaterno"]),
                                SalaNombre = ManejoNulos.ManageNullStr(dr["SalaNombre"]),
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
        
        public CAL_PersonaProhibidoIngresoIncidenciaEntidad GetIDTimadorIncidencia(int id) {
            CAL_PersonaProhibidoIngresoIncidenciaEntidad item = new CAL_PersonaProhibidoIngresoIncidenciaEntidad();
            string consulta = @"SELECT tim.[TimadorIncidenciaID]                         
                              ,tim.[TimadorID]            
                              ,tim.[Estado]    
                              ,tim.[FechaRegistro]
                              ,tim.[EmpleadoID] 
                              ,tim.[CodSala]
                              ,tim.[Observacion]
                              ,tim.[SustentoLegal]
                                ,emp.[Nombres] AS [EmpleadoNombres]
                                ,emp.[ApellidosPaterno] AS [EmpleadoApellidoPaterno]
                                ,sal.[Nombre] AS [SalaNombre]
                                FROM [dbo].[CAL_TimadorIncidencia] tim
                                INNER JOIN [dbo].[SEG_Empleado] emp ON tim.EmpleadoID = emp.EmpleadoID
                                INNER JOIN [dbo].[Sala] sal ON tim.CodSala = sal.CodSala  WHERE tim.TimadorIncidenciaID = @pTimadorIncidenciaID  ";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTimadorIncidenciaID", id);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.TimadorIncidenciaID = ManejoNulos.ManageNullInteger(dr["TimadorIncidenciaID"]);
                                item.TimadorID = ManejoNulos.ManageNullInteger(dr["TimadorID"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.Observacion = ManejoNulos.ManageNullStr(dr["Observacion"]);
                                item.SustentoLegal = ManejoNulos.ManageNullInteger(dr["SustentoLegal"]);

                                item.EmpleadoNombres = ManejoNulos.ManageNullStr(dr["EmpleadoNombres"]);
                                item.EmpleadoApellidoPaterno = ManejoNulos.ManageNullStr(dr["EmpleadoApellidoPaterno"]);
                                item.SalaNombre = ManejoNulos.ManageNullStr(dr["SalaNombre"]);
                            }
                        }
                    };
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        
        public int InsertarTimadorIncidencia(CAL_PersonaProhibidoIngresoIncidenciaEntidad Entidad) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO CAL_TimadorIncidencia ([TimadorID]
           ,[Estado]
           ,[FechaRegistro]
           ,[EmpleadoID]
           ,[CodSala]
           ,[Observacion]
           ,[SustentoLegal]
)
                                OUTPUT Inserted.TimadorIncidenciaID  
                                VALUES(@pTimadorID 
                               ,@pEstado
                               ,@pFechaRegistro
                               ,@pEmpleadoID
                               ,@pCodSala
                               ,@pObservacion
                               ,@pSustentoLegal
)";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTimadorID", ManejoNulos.ManageNullInteger(Entidad.TimadorID));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pEmpleadoID", ManejoNulos.ManageNullInteger(Entidad.EmpleadoID));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(Entidad.CodSala));
                    query.Parameters.AddWithValue("@pObservacion", ManejoNulos.ManageNullStr(Entidad.Observacion));
                    query.Parameters.AddWithValue("@pSustentoLegal", ManejoNulos.ManageNullInteger(Entidad.SustentoLegal));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        
        public bool EditarTimadorIncidencia(CAL_PersonaProhibidoIngresoIncidenciaEntidad Entidad) {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CAL_TimadorIncidencia]
                               SET [TimadorID] = @pTimadorID
                                  ,[Estado] = @pEstado 
                                  ,[FechaRegistro] = @pFechaRegistro
                                  ,[EmpleadoID] = @pEmpleadoID  
                                  ,[CodSala] = @pCodSala
                                  ,[Observacion] = @pObservacion
                                  ,[SustentoLegal] = @pSustentoLegal
                                   WHERE TimadorIncidenciaID = @pTimadorIncidenciaID";

            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTimadorIncidenciaID", ManejoNulos.ManageNullInteger(Entidad.TimadorIncidenciaID));
                    query.Parameters.AddWithValue("@pTimadorID", ManejoNulos.ManageNullInteger(Entidad.TimadorID));
                    query.Parameters.AddWithValue("@pEstado", ManejoNulos.ManageNullInteger(Entidad.Estado));
                    query.Parameters.AddWithValue("@pFechaRegistro", ManejoNulos.ManageNullDate(Entidad.FechaRegistro));
                    query.Parameters.AddWithValue("@pEmpleadoID", ManejoNulos.ManageNullInteger(Entidad.EmpleadoID));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(Entidad.CodSala));
                    query.Parameters.AddWithValue("@pObservacion", ManejoNulos.ManageNullStr(Entidad.Observacion));
                    query.Parameters.AddWithValue("@pSustentoLegal", ManejoNulos.ManageNullInteger(Entidad.SustentoLegal));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        
        public bool EliminarTimadorIncidencia(int id) {
            bool respuesta = false;
            string consulta = @"DELETE FROM CAL_TimadorIncidencia 
                                WHERE TimadorIncidenciaID  = @pTimadorIncidenciaID";
            try {
                using(var con = new SqlConnection(conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTimadorIncidenciaID", ManejoNulos.ManageNullInteger(id));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
    }
}
