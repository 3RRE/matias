using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos {
    public class SalaMaestraDAL {
        private readonly string _conexion;

        public SalaMaestraDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<SalaMaestraEntidad> ObtenerTodasLasSalasMaestras() {
            List<SalaMaestraEntidad> salasMaestras = new List<SalaMaestraEntidad>();
            string consulta = @"
                SELECT
                    CodSalaMaestra,
                    Nombre,
                    Estado
                FROM
                    SalaMaestra
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var sala = new SalaMaestraEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"])
                            };
                            salasMaestras.Add(sala);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return salasMaestras;
        }

        public List<SalaMaestraEntidad> ObtenerTodasLasSalasMaestrasActivas() {
            List<SalaMaestraEntidad> salasMaestras = new List<SalaMaestraEntidad>();
            string consulta = @"
                SELECT
                    CodSalaMaestra,
                    Nombre,
                    Estado
                FROM
                    SalaMaestra
                WHERE
                    Estado = 1
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var sala = new SalaMaestraEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"])
                            };
                            salasMaestras.Add(sala);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return salasMaestras;
        }

        public SalaMaestraEntidad ObtenerSalaMaestraPorCodigo(int codSalaMaestra) {
            SalaMaestraEntidad salaMaestra = new SalaMaestraEntidad();
            string consulta = @"
                SELECT
                    CodSalaMaestra,
                    Nombre,
                    Estado
                FROM
                    SalaMaestra
                WHERE
                    CodSalaMaestra = @pCodSalaMaestra
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSalaMaestra", codSalaMaestra);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            salaMaestra = new SalaMaestraEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return salaMaestra;
        }
        
        public SalaMaestraEntidad ObtenerSalaMaestraPorCodigoSala(int codSala) {
            SalaMaestraEntidad salaMaestra = new SalaMaestraEntidad();
            string consulta = @"
                SELECT
                    sm.CodSalaMaestra,
                    sm.Nombre,
                    sm.Estado
                FROM
                    SalaMaestra AS sm
                INNER JOIN Sala AS s ON s.CodSalaMaestra = sm.CodSalaMaestra
                WHERE
                    s.CodSala = @CodSala
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", codSala);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            salaMaestra = new SalaMaestraEntidad {
                                CodSalaMaestra = ManejoNulos.ManageNullInteger(dr["CodSalaMaestra"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return salaMaestra;
        }

        public bool InsertarSalaMaestra(SalaMaestraEntidad salaMaestra) {
            bool response = false;
            string consulta = @"
                INSERT INTO 
                    SalaMaestra(Nombre, Estado) 
                VALUES 
                    (@pNombre, 1)
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", salaMaestra.Nombre);
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public bool ActualizarSalaMaestra(SalaMaestraEntidad salaMaestra) {
            bool response = false;
            string consulta = @"
                UPDATE
                    SalaMaestra
                SET
                    Nombre = @pNombre
                WHERE
                    CodSalaMaestra = @pCodSalaMaestra
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pNombre", salaMaestra.Nombre);
                    query.Parameters.AddWithValue("@pCodSalaMaestra", salaMaestra.CodSalaMaestra);
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public bool EliminarSalaMaestra(int codSalaMaestra) {
            bool response = false;
            string consulta = @"
                DELETE FROM 
                    SalaMaestra 
                WHERE
                    CodSalaMaestra = @pCodSalaMaestra
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSalaMaestra", codSalaMaestra);
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public bool ActivarSalaMaestra(int codSalaMaestra) {
            bool response = false;
            string consulta = @"
                DECLARE @MaximaFechaRegistroSala DATETIME = '2000-01-01 00:00:00'
                DECLARE @CodSala INT = 0

                ---Obtener la fecha de registro más reciente de una sala por sala maestra
                SELECT
                    @MaximaFechaRegistroSala = MAX(FechaRegistro)
                FROM
                    Sala (nolock)
                WHERE
                    CodSalaMaestra = @pCodSalaMaestra

                ---Obtener el codigo de la ultima sala registrada de una sala maestra
                SELECT TOP 1
                    @CodSala = CodSala
                FROM
                    Sala (nolock)
                WHERE
                    CodSalaMaestra = @pCodSalaMaestra AND
                    FechaRegistro = @MaximaFechaRegistroSala

                UPDATE
                    SalaMaestra
                SET
                    Estado = 1
                WHERE
                    CodSalaMaestra = @pCodSalaMaestra
                
                UPDATE
                    Sala
                SET
                    Estado = 1
                WHERE
                    CodSala = @CodSala
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSalaMaestra", codSalaMaestra);
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public bool DesactivarSalaMaestra(int codSalaMaestra) {
            bool response = false;
            string consulta = @"
                UPDATE
                    SalaMaestra
                SET
                    Estado = 0
                WHERE
                    CodSalaMaestra = @pCodSalaMaestra

                UPDATE
                    Sala
                SET
                    Estado = 0
                WHERE
                    CodSalaMaestra = @pCodSalaMaestra
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSalaMaestra", codSalaMaestra);
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return response;
        }
    }
}
