using CapaEntidad.ClienteSatisfaccion.Entidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ClienteSatisfaccion.Flujo {
    public class FlujoDAL {
        string _conexion = string.Empty;

        public FlujoDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<FlujoEntidad> ListadoFlujoEncuesta(int TipoEncuesta) {
            List<FlujoEntidad> lista = new List<FlujoEntidad>();
            string consulta = @"SELECT  [idFlujo]
                              ,[idTipoEncuesta]
                              ,[idPreguntaActual]
                              ,[idOpcion],[idPreguntaSiguiente]
                          FROM FlujoPregunta
                            WHERE idTipoEncuesta=@p1 ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", TipoEncuesta);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new FlujoEntidad {
                                IdFlujo = ManejoNulos.ManageNullInteger(dr["IdFlujo"]),
                                IdTipoEncuesta = ManejoNulos.ManageNullInteger(dr["IdTipoEncuesta"]),
                                IdPreguntaActual = ManejoNulos.ManageNullInteger(dr["IdPreguntaActual"]),
                                IdOpcion = ManejoNulos.ManageNullInteger(dr["IdOpcion"]),
                                IdPreguntaSiguiente = ManejoNulos.ManageNullInteger(dr["IdPreguntaSiguiente"]),
                            };
                            lista.Add(item);
                        }
                    }
                }

            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
            }
            return lista;
        }
    }
}
