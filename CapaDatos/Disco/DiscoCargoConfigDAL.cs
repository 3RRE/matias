using CapaEntidad.Alertas;
using CapaEntidad.Disco;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Disco {
    public class DiscoCargoConfigDAL {
        string _conexion = string.Empty;
        public DiscoCargoConfigDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

        }

        public List<DiscoCargoConfigEntidad> DiscoCargoConf_Listado() {
            List<DiscoCargoConfigEntidad> lista = new List<DiscoCargoConfigEntidad>();
            string consulta = @"SELECT 
                                idDiscoConfigCargo , 
                                sala_id , 
                                cargo_id , 
                                fecha_registro
	                            FROM DiscoCargoConfiguracion order by idDiscoConfigCargo desc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var discoCargo = new DiscoCargoConfigEntidad {
                                idDiscoConfigCargo = ManejoNulos.ManageNullInteger(dr["idDiscoConfigCargo"]),
                                sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                cargo_id = ManejoNulos.ManageNullInteger(dr["cargo_id"]),
                                fechaRegistro = ManejoNulos.ManageNullDate(dr["fecha_registro"]),

                            };

                            lista.Add(discoCargo);
                        }
                    }

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }


        public int DiscoCargoConfInsertarJson(DiscoCargoConfigEntidad discoConfig) {
            //bool response = false;
            int idempleadoDispositivoInsertado = 0;
            string consulta = @"
            INSERT INTO DiscoCargoConfiguracion(cargo_id, sala_id, fecha_registro)
	            VALUES (@p0, @p1, @p2) 
                SELECT SCOPE_IDENTITY()";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(discoConfig.cargo_id));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(discoConfig.sala_id));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(discoConfig.fechaRegistro));
                    idempleadoDispositivoInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return idempleadoDispositivoInsertado;
        }


        public bool DiscoCargoConfEliminarJson(int alt_id) {
            bool response = false;
            string consulta = @"Delete from [DiscoCargoConfiguracion]
                 WHERE idDiscoConfigCargo=@p0";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", alt_id);
                    query.ExecuteNonQuery();
                    response = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                response = false;
            }
            return response;
        }

    }
}
