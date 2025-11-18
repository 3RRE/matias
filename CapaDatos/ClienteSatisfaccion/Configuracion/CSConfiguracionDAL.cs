using CapaEntidad.ClienteSatisfaccion.Entidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ClienteSatisfaccion.Configuracion {
    public class CSConfiguracionDAL {
        string _conexion = string.Empty;

        public CSConfiguracionDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CSConfiguracionEntidad> ListadoConfiguraciones() {
            List<CSConfiguracionEntidad> lista = new List<CSConfiguracionEntidad>();
            string consulta = @"SELECT [IdConfiguracion], [ClaveConfig], [ValorBit] 
                        FROM CS_Configuracion";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new CSConfiguracionEntidad {
                                IdConfiguracion = ManejoNulos.ManageNullInteger(dr["IdConfiguracion"]),
                                ClaveConfig = ManejoNulos.ManageNullStr(dr["ClaveConfig"]),
                                ValorBit = ManejoNulos.ManegeNullBool(dr["ValorBit"])
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
        public bool ActualizarValorBit(int idTipoConfiguracion, bool nuevoValor, int idSala) {
            string querySelect = @"
        SELECT COUNT(*) 
        FROM CS_ConfiguracionSala 
        WHERE IdTipoConfiguracion = @idTipoConfig AND IdSala = @idSala";

            string queryInsert = @"
        INSERT INTO CS_ConfiguracionSala (IdTipoConfiguracion, IdSala, ValorBit)
        VALUES (@idTipoConfig, @idSala, @valor)";

            string queryUpdate = @"
        UPDATE CS_ConfiguracionSala
        SET ValorBit = @valor
        WHERE IdTipoConfiguracion = @idTipoConfig AND IdSala = @idSala";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();

                    // Verificar si ya existe
                    bool existe;
                    using(var cmdSelect = new SqlCommand(querySelect, con)) {
                        cmdSelect.Parameters.AddWithValue("@idTipoConfig", idTipoConfiguracion);
                        cmdSelect.Parameters.AddWithValue("@idSala", idSala);
                        existe = Convert.ToInt32(cmdSelect.ExecuteScalar()) > 0;
                    }

                    // Insertar o actualizar según corresponda
                    string queryFinal = existe ? queryUpdate : queryInsert;

                    using(var cmd = new SqlCommand(queryFinal, con)) {
                        cmd.Parameters.AddWithValue("@idTipoConfig", idTipoConfiguracion);
                        cmd.Parameters.AddWithValue("@idSala", idSala);
                        cmd.Parameters.AddWithValue("@valor", nuevoValor);

                        int filas = cmd.ExecuteNonQuery();
                        return filas > 0;
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en ActualizarValorBit: " + ex.Message);
                return false;
            }
        }




        public bool PuedeResponderEncuesta(int idSala, string nroDocumento) {
            string consulta = @"
            SELECT COUNT(1)
            FROM RespuestaEncuesta
            WHERE IdSala = @p1
              AND NroDocumento = @p2
              AND CAST(FechaRespuesta AS DATE) = CAST(GETDATE() AS DATE)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", idSala);
                    query.Parameters.AddWithValue("@p2", nroDocumento);

                    int count = (int)query.ExecuteScalar();

                    // 👉 Si ya existe un registro hoy → retorna false (no puede responder)
                    // 👉 Si no existe → retorna true (sí puede responder)
                    return count == 0;
                }
            } catch(Exception ex) {
                Console.WriteLine("Error en PuedeResponderEncuesta: " + ex.Message);
                return false;
            }
        }


        public List<CSConfiguracionEntidad> ListadoConfiguracionesPorSala(int idSala) {
            List<CSConfiguracionEntidad> lista = new List<CSConfiguracionEntidad>();
            string consulta = @"
                            SELECT 
                                c.IdConfiguracion, 
                                c.ClaveConfig, 
                                ISNULL(cs.ValorBit, 0) AS ValorBit
                            FROM CS_Configuracion c
                            LEFT JOIN CS_ConfiguracionSala cs 
                                ON cs.IdTipoConfiguracion = c.IdConfiguracion
                                AND cs.IdSala = @idSala
                            ORDER BY c.IdConfiguracion;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@idSala", idSala);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new CSConfiguracionEntidad {
                                IdConfiguracion = ManejoNulos.ManageNullInteger(dr["IdConfiguracion"]),
                                ClaveConfig = ManejoNulos.ManageNullStr(dr["ClaveConfig"]),
                                ValorBit = ManejoNulos.ManegeNullBool(dr["ValorBit"])
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

    }
}
