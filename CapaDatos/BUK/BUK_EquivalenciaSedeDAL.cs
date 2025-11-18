using CapaEntidad.BUK;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.BUK {
    public class BUK_EquivalenciaSedeDAL {
        string _conexion = string.Empty;

        public BUK_EquivalenciaSedeDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }


        public List<BUK_EquivalenciaSedeEntidad> ObtenerTodasLasEquivalenciasSede() {
            List<BUK_EquivalenciaSedeEntidad> lista = new List<BUK_EquivalenciaSedeEntidad>();
            string consulta = @"
                SELECT 
	                IdEquivalenciaSede,
	                s.Nombre AS NombreSede,
                    e.CodEmpresaOfisis,
	                CodSedeOfisis,
	                e.Nombre as NombreEmpresa,
	                e.IdEquivalenciaEmpresa,
                    e.IdEmpresaBuk
                FROM
	                BUK_EquivalenciaSede AS s
                LEFT JOIN
	                BUK_EquivalenciaEmpresa AS e ON e.IdEquivalenciaEmpresa = s.IdEquivalenciaEmpresa
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new BUK_EquivalenciaSedeEntidad {
                                IdEquivalenciaSede = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaSede"]),
                                NombreSede = ManejoNulos.ManageNullStr(dr["NombreSede"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
                                CodSedeOfisis = ManejoNulos.ManageNullStr(dr["CodSedeOfisis"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]),
                                IdEquivalenciaEmpresa = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaEmpresa"]),
                                IdEmpresaBuk = ManejoNulos.ManageNullInteger(dr["IdEmpresaBuk"])
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

        public List<BUK_EquivalenciaSedeEntidad> ObtenerTodasLasEquivalenciasSedeCorrectas() {
            List<BUK_EquivalenciaSedeEntidad> lista = new List<BUK_EquivalenciaSedeEntidad>();
            string consulta = @"
                SELECT
	                IdEquivalenciaSede,
	                s.Nombre AS NombreSede,
                    e.CodEmpresaOfisis,
	                CodSedeOfisis,
	                e.Nombre as NombreEmpresa,
	                e.IdEquivalenciaEmpresa,
	                e.IdEmpresaBuk
                FROM
	                BUK_EquivalenciaSede AS s
                INNER JOIN
	                BUK_EquivalenciaEmpresa AS e ON e.IdEquivalenciaEmpresa = s.IdEquivalenciaEmpresa
                WHERE
	                s.CodSedeOfisis > 0 AND
	                e.CodEmpresaOfisis > 0
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new BUK_EquivalenciaSedeEntidad {
                                IdEquivalenciaSede = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaSede"]),
                                NombreSede = ManejoNulos.ManageNullStr(dr["NombreSede"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
                                CodSedeOfisis = ManejoNulos.ManageNullStr(dr["CodSedeOfisis"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]),
                                IdEquivalenciaEmpresa = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaEmpresa"]),
                                IdEmpresaBuk = ManejoNulos.ManageNullInteger(dr["IdEmpresaBuk"])
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

        public List<BUK_EquivalenciaSedeEntidad> ObtenerEquivalenciaSedePorIdEquivalenciaEmpresa(int idEquivalenciaEmpresa) {
            List<BUK_EquivalenciaSedeEntidad> lista = new List<BUK_EquivalenciaSedeEntidad>();
            string consulta = @"
                SELECT 
	                IdEquivalenciaSede,
	                s.Nombre AS NombreSede,
                    e.CodEmpresaOfisis,
	                CodSedeOfisis,
	                e.Nombre as NombreEmpresa,
	                e.IdEquivalenciaEmpresa,
                    e.IdEmpresaBuk
                FROM
	                BUK_EquivalenciaSede AS s
                LEFT JOIN
	                BUK_EquivalenciaEmpresa AS e ON e.IdEquivalenciaEmpresa = s.IdEquivalenciaEmpresa
                WHERE
	                s.IdEquivalenciaEmpresa = @IdEquivalenciaEmpresa
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEquivalenciaEmpresa", idEquivalenciaEmpresa);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new BUK_EquivalenciaSedeEntidad {
                                IdEquivalenciaSede = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaSede"]),
                                NombreSede = ManejoNulos.ManageNullStr(dr["NombreSede"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
                                CodSedeOfisis = ManejoNulos.ManageNullStr(dr["CodSedeOfisis"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]),
                                IdEquivalenciaEmpresa = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaEmpresa"]),
                                IdEmpresaBuk = ManejoNulos.ManageNullInteger(dr["IdEmpresaBuk"])
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

        public List<BUK_EquivalenciaSedeEntidad> ObtenerEquivalenciaSedePorCodEmpresaOfisis(string codEmpresaOfisis) {
            List<BUK_EquivalenciaSedeEntidad> lista = new List<BUK_EquivalenciaSedeEntidad>();
            string consulta = @"
                SELECT
	                IdEquivalenciaSede,
	                s.Nombre AS NombreSede,
                    e.CodEmpresaOfisis,
	                CodSedeOfisis,
	                e.Nombre as NombreEmpresa,
	                e.IdEquivalenciaEmpresa,
                    e.IdEmpresaBuk
                FROM
	                BUK_EquivalenciaSede AS s
                INNER JOIN
	                BUK_EquivalenciaEmpresa AS e ON e.IdEquivalenciaEmpresa = s.IdEquivalenciaEmpresa
                WHERE
	                e.CodEmpresaOfisis = @CodEmpresaOfisis
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodEmpresaOfisis", codEmpresaOfisis);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new BUK_EquivalenciaSedeEntidad {
                                IdEquivalenciaSede = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaSede"]),
                                NombreSede = ManejoNulos.ManageNullStr(dr["NombreSede"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
                                CodSedeOfisis = ManejoNulos.ManageNullStr(dr["CodSedeOfisis"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]),
                                IdEquivalenciaEmpresa = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaEmpresa"]),
                                IdEmpresaBuk = ManejoNulos.ManageNullInteger(dr["IdEmpresaBuk"])
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


        public BUK_EquivalenciaSedeEntidad ObtenerEquivalenciaSedePorCodEmpresaOfisisYNombreEquivalenciaSede(string codEmpresaOfisis, string nombreSede) {
            BUK_EquivalenciaSedeEntidad equivalenciaSede = new BUK_EquivalenciaSedeEntidad();
            string consulta = @"
                SELECT 
	                IdEquivalenciaSede,
	                s.Nombre AS NombreSede,
                    e.CodEmpresaOfisis,
	                CodSedeOfisis,
	                e.Nombre as NombreEmpresa,
	                e.IdEquivalenciaEmpresa,
                    e.IdEmpresaBuk
                FROM
	                BUK_EquivalenciaSede AS s
                LEFT JOIN
	                BUK_EquivalenciaEmpresa AS e ON e.IdEquivalenciaEmpresa = s.IdEquivalenciaEmpresa
                WHERE
                    e.CodEmpresaOfisis = @CodigoEmpresaOfisis AND
	                s.Nombre = @NombreSede
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodigoEmpresaOfisis", codEmpresaOfisis);
                    query.Parameters.AddWithValue("@NombreSede", nombreSede);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            equivalenciaSede = new BUK_EquivalenciaSedeEntidad {
                                IdEquivalenciaSede = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaSede"]),
                                NombreSede = ManejoNulos.ManageNullStr(dr["NombreSede"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
                                CodSedeOfisis = ManejoNulos.ManageNullStr(dr["CodSedeOfisis"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]),
                                IdEquivalenciaEmpresa = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaEmpresa"]),
                                IdEmpresaBuk = ManejoNulos.ManageNullInteger(dr["IdEmpresaBuk"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return equivalenciaSede;
        }

        public BUK_EquivalenciaSedeEntidad ObtenerEquivalenciaSedePorNombreEquivalenciaSede(string nombreSede) {
            BUK_EquivalenciaSedeEntidad equivalenciaSede = new BUK_EquivalenciaSedeEntidad();
            string consulta = @"
                SELECT 
	                IdEquivalenciaSede,
	                s.Nombre AS NombreSede,
                    e.CodEmpresaOfisis,
	                CodSedeOfisis,
	                e.Nombre as NombreEmpresa,
	                e.IdEquivalenciaEmpresa,
                    e.IdEmpresaBuk
                FROM
	                BUK_EquivalenciaSede AS s
                LEFT JOIN
	                BUK_EquivalenciaEmpresa AS e ON e.IdEquivalenciaEmpresa = s.IdEquivalenciaEmpresa
                WHERE
	                s.Nombre = @NombreSede
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@NombreSede", nombreSede);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            equivalenciaSede = new BUK_EquivalenciaSedeEntidad {
                                IdEquivalenciaSede = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaSede"]),
                                NombreSede = ManejoNulos.ManageNullStr(dr["NombreSede"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
                                CodSedeOfisis = ManejoNulos.ManageNullStr(dr["CodSedeOfisis"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]),
                                IdEquivalenciaEmpresa = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaEmpresa"]),
                                IdEmpresaBuk = ManejoNulos.ManageNullInteger(dr["IdEmpresaBuk"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return equivalenciaSede;
        }

        public BUK_EquivalenciaSedeEntidad ObtenerEquivalenciaSedePorIdEquivalenciaSede(int idEquivalenciaSede) {
            BUK_EquivalenciaSedeEntidad equivalenciaSede = new BUK_EquivalenciaSedeEntidad();
            string consulta = @"
                SELECT 
	                IdEquivalenciaSede,
	                s.Nombre AS NombreSede,
                    e.CodEmpresaOfisis,
	                CodSedeOfisis,
	                e.Nombre as NombreEmpresa,
	                e.IdEquivalenciaEmpresa,
                    e.IdEmpresaBuk
                FROM
	                BUK_EquivalenciaSede AS s
                LEFT JOIN
	                BUK_EquivalenciaEmpresa AS e ON e.IdEquivalenciaEmpresa = s.IdEquivalenciaEmpresa
                WHERE
	                IdEquivalenciaSede = @IdEquivalenciaSede
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEquivalenciaSede", idEquivalenciaSede);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            equivalenciaSede = new BUK_EquivalenciaSedeEntidad {
                                IdEquivalenciaSede = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaSede"]),
                                NombreSede = ManejoNulos.ManageNullStr(dr["NombreSede"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
                                CodSedeOfisis = ManejoNulos.ManageNullStr(dr["CodSedeOfisis"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]),
                                IdEquivalenciaEmpresa = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaEmpresa"]),
                                IdEmpresaBuk = ManejoNulos.ManageNullInteger(dr["IdEmpresaBuk"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return equivalenciaSede;
        }

        public BUK_EquivalenciaSedeEntidad ObtenerEquivalenciaSedePorCodEmpresaYSedeOfisis(string codEmpresaOfisis, string codSedeOfisis) {
            BUK_EquivalenciaSedeEntidad equivalenciaSede = new BUK_EquivalenciaSedeEntidad();
            string consulta = @"
                SELECT 
	                IdEquivalenciaSede,
	                s.Nombre AS NombreSede,
                    e.CodEmpresaOfisis,
	                CodSedeOfisis,
	                e.Nombre as NombreEmpresa,
	                e.IdEquivalenciaEmpresa,
                    e.IdEmpresaBuk
                FROM
	                BUK_EquivalenciaSede AS s
                LEFT JOIN
	                BUK_EquivalenciaEmpresa AS e ON e.IdEquivalenciaEmpresa = s.IdEquivalenciaEmpresa
                WHERE
                    e.CodEmpresaOfisis = @CodigoEmpresaOfisis AND
	                CodSedeOfisis = @CodSedeOfisis
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodigoEmpresaOfisis", codEmpresaOfisis);
                    query.Parameters.AddWithValue("@CodSedeOfisis", codSedeOfisis);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            equivalenciaSede = new BUK_EquivalenciaSedeEntidad {
                                IdEquivalenciaSede = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaSede"]),
                                NombreSede = ManejoNulos.ManageNullStr(dr["NombreSede"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
                                CodSedeOfisis = ManejoNulos.ManageNullStr(dr["CodSedeOfisis"]),
                                NombreEmpresa = ManejoNulos.ManageNullStr(dr["NombreEmpresa"]),
                                IdEquivalenciaEmpresa = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaEmpresa"]),
                                IdEmpresaBuk = ManejoNulos.ManageNullInteger(dr["IdEmpresaBuk"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return equivalenciaSede;
        }

        public int InsertarEquivalenciaSede(BUK_EquivalenciaSedeEntidad equivalenciaSede) {
            int idInsertado = 0;
            string consulta = @"
                INSERT INTO BUK_EquivalenciaSede(Nombre, CodSedeOfisis, IdEquivalenciaEmpresa)
                OUTPUT INSERTED.IdEquivalenciaSede
                VALUES (@Nombre, @CodSedeOfisis, @IdEquivalenciaEmpresa)
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", equivalenciaSede.NombreSede);
                    query.Parameters.AddWithValue("@CodSedeOfisis", equivalenciaSede.CodSedeOfisis);
                    query.Parameters.AddWithValue("@IdEquivalenciaEmpresa", equivalenciaSede.IdEquivalenciaEmpresa == 0 ? DBNull.Value : (object)equivalenciaSede.IdEquivalenciaEmpresa);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                idInsertado = 0;
            }

            return idInsertado;
        }

        public int ActualizarEquivalenciaSede(BUK_EquivalenciaSedeEntidad equivalenciaSede) {
            int idActualizado = 0;
            string consulta = @"
                UPDATE  BUK_EquivalenciaSede
                SET Nombre = @Nombre,
	                CodSedeOfisis = @CodSedeOfisis,
	                IdEquivalenciaEmpresa = @IdEquivalenciaEmpresa
                OUTPUT INSERTED.IdEquivalenciaSede
                WHERE IdEquivalenciaSede = @IdEquivalenciaSede
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", equivalenciaSede.NombreSede);
                    query.Parameters.AddWithValue("@CodSedeOfisis", equivalenciaSede.CodSedeOfisis);
                    query.Parameters.AddWithValue("@IdEquivalenciaEmpresa", equivalenciaSede.IdEquivalenciaEmpresa);
                    query.Parameters.AddWithValue("@IdEquivalenciaSede", equivalenciaSede.IdEquivalenciaSede);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarEquivalenciaSede(int idEquivalenciaSede) {
            int idEliminado = 0;
            string consulta = @"
                DELETE FROM BUK_EquivalenciaSede
                OUTPUT DELETED.IdEquivalenciaSede
                WHERE IdEquivalenciaSede = @IdEquivalenciaSede
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEquivalenciaSede", idEquivalenciaSede);
                    idEliminado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                idEliminado = 0;
            }
            return idEliminado;
        }
    }
}
