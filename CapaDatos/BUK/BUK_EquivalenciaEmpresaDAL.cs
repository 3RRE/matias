using CapaEntidad.BUK;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.BUK {
    public class BUK_EquivalenciaEmpresaDAL {
        string _conexion = string.Empty;

        public BUK_EquivalenciaEmpresaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }


        public List<BUK_EquivalenciaEmpresaEntidad> ObtenerTodasLasEquivalenciasEmpresa() {
            List<BUK_EquivalenciaEmpresaEntidad> lista = new List<BUK_EquivalenciaEmpresaEntidad>();
            string consulta = @"
                SELECT 
	                IdEquivalenciaEmpresa,
	                Nombre,
	                CodEmpresaOfisis,
	                IdEmpresaBuk
                FROM
	                BUK_EquivalenciaEmpresa
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new BUK_EquivalenciaEmpresaEntidad {
                                IdEquivalenciaEmpresa = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaEmpresa"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
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

        public List<BUK_EquivalenciaEmpresaEntidad> ObtenerTodasLasEquivalenciasEmpresaActivas() {
            List<BUK_EquivalenciaEmpresaEntidad> lista = new List<BUK_EquivalenciaEmpresaEntidad>();
            string consulta = @"
                SELECT 
	                IdEquivalenciaEmpresa,
	                Nombre,
	                CodEmpresaOfisis,
	                IdEmpresaBuk,
                    Estado
                FROM
	                BUK_EquivalenciaEmpresa
                WHERE
                    Estado != 0;
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new BUK_EquivalenciaEmpresaEntidad {
                                IdEquivalenciaEmpresa = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaEmpresa"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
                                IdEmpresaBuk = ManejoNulos.ManageNullInteger(dr["IdEmpresaBuk"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"])
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

        public List<BUK_EquivalenciaEmpresaEntidad> ObtenerTodasLasEquivalenciasEmpresaCorrectas() {
            List<BUK_EquivalenciaEmpresaEntidad> lista = new List<BUK_EquivalenciaEmpresaEntidad>();
            string consulta = @"
                SELECT
	                IdEquivalenciaEmpresa,
	                Nombre,
	                CodEmpresaOfisis,
	                IdEmpresaBuk
                FROM
	                BUK_EquivalenciaEmpresa
                WHERE
	                CodEmpresaOfisis > 0
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new BUK_EquivalenciaEmpresaEntidad {
                                IdEquivalenciaEmpresa = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaEmpresa"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
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


        public BUK_EquivalenciaEmpresaEntidad ObtenerEquivalenciaEmpresaPorIdEquivalenciaEmpresa(int idEquivalenciaEmpresa) {
            BUK_EquivalenciaEmpresaEntidad equivalenciaEmpresa = new BUK_EquivalenciaEmpresaEntidad();
            string consulta = @"
                SELECT 
	                IdEquivalenciaEmpresa,
	                Nombre,
	                CodEmpresaOfisis,
	                IdEmpresaBuk
                FROM
	                BUK_EquivalenciaEmpresa
                WHERE
	                IdEquivalenciaEmpresa = @IdEquivalenciaEmpresa
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEquivalenciaEmpresa", idEquivalenciaEmpresa);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            equivalenciaEmpresa = new BUK_EquivalenciaEmpresaEntidad {
                                IdEquivalenciaEmpresa = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaEmpresa"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
                                IdEmpresaBuk = ManejoNulos.ManageNullInteger(dr["IdEmpresaBuk"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return equivalenciaEmpresa;
        }

        public BUK_EquivalenciaEmpresaEntidad ObtenerEquivalenciaEmpresaPorIdEmpresaBuk(int idEmpresaBuk) {
            BUK_EquivalenciaEmpresaEntidad equivalenciaEmpresa = new BUK_EquivalenciaEmpresaEntidad();
            string consulta = @"
                SELECT 
	                IdEquivalenciaEmpresa,
	                Nombre,
	                CodEmpresaOfisis,
	                IdEmpresaBuk
                FROM
	                BUK_EquivalenciaEmpresa
                WHERE
	                IdEmpresaBuk = @IdEmpresaBuk
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEmpresaBuk", idEmpresaBuk);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            equivalenciaEmpresa = new BUK_EquivalenciaEmpresaEntidad {
                                IdEquivalenciaEmpresa = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaEmpresa"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
                                IdEmpresaBuk = ManejoNulos.ManageNullInteger(dr["IdEmpresaBuk"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return equivalenciaEmpresa;
        }

        public BUK_EquivalenciaEmpresaEntidad ObtenerEquivalenciaEmpresaPorCodEmpresaOfisis(string codEmpresaOfisis) {
            BUK_EquivalenciaEmpresaEntidad equivalenciaEmpresa = new BUK_EquivalenciaEmpresaEntidad();
            string consulta = @"
                SELECT 
	                IdEquivalenciaEmpresa,
	                Nombre,
	                CodEmpresaOfisis,
	                IdEmpresaBuk
                FROM
	                BUK_EquivalenciaEmpresa
                WHERE
	                CodEmpresaOfisis = @CodEmpresaOfisis
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodEmpresaOfisis", codEmpresaOfisis);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            equivalenciaEmpresa = new BUK_EquivalenciaEmpresaEntidad {
                                IdEquivalenciaEmpresa = ManejoNulos.ManageNullInteger(dr["IdEquivalenciaEmpresa"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                CodEmpresaOfisis = ManejoNulos.ManageNullStr(dr["CodEmpresaOfisis"]),
                                IdEmpresaBuk = ManejoNulos.ManageNullInteger(dr["IdEmpresaBuk"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return equivalenciaEmpresa;
        }


        public int InsertarEquivalenciaEmpresa(BUK_EquivalenciaEmpresaEntidad equivalenciaEmpresa) {
            int idInsertado = 0;
            string consulta = @"
                INSERT INTO BUK_EquivalenciaEmpresa(Nombre, CodEmpresaOfisis, IdEmpresaBuk)
                OUTPUT INSERTED.IdEquivalenciaEmpresa
                VALUES (@Nombre, @CodEmpresaOfisis, @IdEmpresaBuk)
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", equivalenciaEmpresa.Nombre);
                    query.Parameters.AddWithValue("@CodEmpresaOfisis", equivalenciaEmpresa.CodEmpresaOfisis);
                    query.Parameters.AddWithValue("@IdEmpresaBuk", equivalenciaEmpresa.IdEmpresaBuk);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                idInsertado = 0;
            }

            return idInsertado;
        }

        public int ActualizarEquivalenciaEmpresa(BUK_EquivalenciaEmpresaEntidad equivalenciaEmpresa) {
            int idActualizado = 0;
            string consulta = @"
                UPDATE  BUK_EquivalenciaEmpresa
                SET Nombre = @Nombre,
	                CodEmpresaOfisis = @CodEmpresaOfisis,
	                IdEmpresaBuk = @IdEmpresaBuk
                OUTPUT INSERTED.IdEquivalenciaEmpresa
                WHERE IdEquivalenciaEmpresa = @IdEquivalenciaEmpresa
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", equivalenciaEmpresa.Nombre);
                    query.Parameters.AddWithValue("@CodEmpresaOfisis", equivalenciaEmpresa.CodEmpresaOfisis);
                    query.Parameters.AddWithValue("@IdEmpresaBuk", equivalenciaEmpresa.IdEmpresaBuk);
                    query.Parameters.AddWithValue("@IdEquivalenciaEmpresa", equivalenciaEmpresa.IdEquivalenciaEmpresa);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarEquivalenciaEmpresa(int idEquivalenciaEmpresa) {
            int idEliminado = 0;
            string consulta = @"
                DELETE FROM BUK_EquivalenciaEmpresa
                OUTPUT DELETED.IdEquivalenciaEmpresa
                WHERE IdEquivalenciaEmpresa = @IdEquivalenciaEmpresa
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEquivalenciaEmpresa", idEquivalenciaEmpresa);
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
