using CapaEntidad.WhatsApp;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.WhatsApp {
    public class WSP_InstanciaUltraMsgDAL {
        string _conexion = string.Empty;

        public WSP_InstanciaUltraMsgDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<WSP_InstanciaUltraMsgEntidad> ObtenerTodasLasInstancias(int usuarioId) {
            List<WSP_InstanciaUltraMsgEntidad> lista = new List<WSP_InstanciaUltraMsgEntidad>();
            string consulta = @"
                SELECT
	                i.idInstanciaUltraMsg AS IdInstanciaUltraMsg,
	                i.codSala AS CodSala,
	                s.nombre AS NombreSala,
	                i.urlBase AS UrlBase,
	                i.instancia AS Instancia,
	                i.token AS Token
                FROM
	                WSP_InstanciaUltraMsg as i
                INNER JOIN Sala as s
	                ON s.CodSala = i.codSala
                WHERE
	                s.CodSala IN (SELECT SalaId FROM UsuarioSala WHERE UsuarioId = @usuarioId)
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@usuarioId", usuarioId);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new WSP_InstanciaUltraMsgEntidad {
                                IdInstanciaUltraMsg = ManejoNulos.ManageNullInteger(dr["IdInstanciaUltraMsg"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                UrlBase = ManejoNulos.ManageNullStr(dr["UrlBase"]),
                                Instancia = ManejoNulos.ManageNullStr(dr["Instancia"]),
                                Token = ManejoNulos.ManageNullStr(dr["Token"])
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

        public WSP_InstanciaUltraMsgEntidad ObtenerInstanciaPorIdInstancia(int idInstancia) {
            WSP_InstanciaUltraMsgEntidad instancia = new WSP_InstanciaUltraMsgEntidad();
            string consulta = @"
                SELECT
	                i.idInstanciaUltraMsg AS IdInstanciaUltraMsg,
	                i.codSala AS CodSala,
	                s.nombre AS NombreSala,
	                i.urlBase AS UrlBase,
	                i.instancia AS Instancia,
	                i.token AS Token
                FROM
	                WSP_InstanciaUltraMsg as i
                INNER JOIN Sala as s
	                ON s.CodSala = i.codSala
                WHERE
	                i.idInstanciaUltraMsg = @IdInstanciaUltraMsg
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdInstanciaUltraMsg", idInstancia);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            instancia = new WSP_InstanciaUltraMsgEntidad {
                                IdInstanciaUltraMsg = ManejoNulos.ManageNullInteger(dr["IdInstanciaUltraMsg"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                UrlBase = ManejoNulos.ManageNullStr(dr["UrlBase"]),
                                Instancia = ManejoNulos.ManageNullStr(dr["Instancia"]),
                                Token = ManejoNulos.ManageNullStr(dr["Token"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return instancia;
        }

        public WSP_InstanciaUltraMsgEntidad ObtenerInstanciaPorCodSala(int codSala) {
            WSP_InstanciaUltraMsgEntidad instancia = new WSP_InstanciaUltraMsgEntidad();
            string consulta = @"
                SELECT
	                i.idInstanciaUltraMsg AS IdInstanciaUltraMsg,
	                i.codSala AS CodSala,
	                s.nombre AS NombreSala,
	                i.urlBase AS UrlBase,
	                i.instancia AS Instancia,
	                i.token AS Token
                FROM
	                WSP_InstanciaUltraMsg as i
                INNER JOIN Sala as s
	                ON s.CodSala = i.codSala
                WHERE
	                i.codSala = @CodSala
            ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", codSala);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            instancia = new WSP_InstanciaUltraMsgEntidad {
                                IdInstanciaUltraMsg = ManejoNulos.ManageNullInteger(dr["IdInstanciaUltraMsg"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                UrlBase = ManejoNulos.ManageNullStr(dr["UrlBase"]),
                                Instancia = ManejoNulos.ManageNullStr(dr["Instancia"]),
                                Token = ManejoNulos.ManageNullStr(dr["Token"])
                            };
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return instancia;
        }

        public int InsertarInstancia(WSP_InstanciaUltraMsgEntidad instancia) {
            int idInsertado = 0;
            string consulta = @"
                INSERT INTO WSP_InstanciaUltraMsg (codSala, urlBase, instancia, token)
                OUTPUT INSERTED.idInstanciaUltraMsg
                VALUES (@codSala, @urlBase, @instancia, @token);
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@codSala", instancia.CodSala);
                    query.Parameters.AddWithValue("@urlBase", instancia.UrlBase);
                    query.Parameters.AddWithValue("@instancia", instancia.Instancia);
                    query.Parameters.AddWithValue("@token", instancia.Token);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                idInsertado = 0;
            }

            return idInsertado;
        }

        public int ActualizarInstancia(WSP_InstanciaUltraMsgEntidad instancia) {
            int idActualizado = 0;
            string consulta = @"
                UPDATE WSP_InstanciaUltraMsg
                SET urlBase = @urlBase,
                    instancia = @instancia,
                    token = @token
                OUTPUT INSERTED.idInstanciaUltraMsg
                WHERE idInstanciaUltraMsg = @idInstanciaUltraMsg;
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@idInstanciaUltraMsg", instancia.IdInstanciaUltraMsg);
                    query.Parameters.AddWithValue("@urlBase", instancia.UrlBase);
                    query.Parameters.AddWithValue("@instancia", instancia.Instancia);
                    query.Parameters.AddWithValue("@token", instancia.Token);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarInstancia(int idInstancia) {
            int idEliminado = 0;
            string consulta = @"
                DELETE FROM WSP_InstanciaUltraMsg
                OUTPUT DELETED.idInstanciaUltraMsg
                WHERE idInstanciaUltraMsg = @idInstanciaUltraMsg;
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@idInstanciaUltraMsg", idInstancia);
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
