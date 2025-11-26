using CapaEntidad.ClienteSatisfaccion.Entidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ClienteSatisfaccion.JefeSala {
    public class CsJefeSalaDAL {
        string _conexion = string.Empty;

        public CsJefeSalaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }


        public List<CSJefeSalaEntidad> ListarJefesSala(int salaId) {
            List<CSJefeSalaEntidad> resultado = new List<CSJefeSalaEntidad>();

            string consulta = @"SELECT  IdJefeSala
                              ,Nombres
                              ,Apellidos
                              ,SalaId
							  ,Celular
							  ,Codigo
							  ,FechaCreacion
                          FROM CS_JefeSala
                          WHERE SalaId=@p1";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", salaId);

                    using(var dr = query.ExecuteReader()) {
                        while (dr.Read()) {
                            var item = new CSJefeSalaEntidad {
                                IdJefeSala = ManejoNulos.ManageNullInteger(dr["IdJefeSala"]),
                                Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                Apellidos = ManejoNulos.ManageNullStr(dr["Apellidos"]),
                                Celular = ManejoNulos.ManageNullStr(dr["Celular"]),
                                Codigo = ManejoNulos.ManageNullStr(dr["Codigo"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                FechaCreacion = ManejoNulos.ManageNullDate(dr["FechaCreacion"]),
                            };

                            resultado.Add(item);
                        }
                    }

                }
            } catch(Exception) {

                return new List<CSJefeSalaEntidad>();
            }
            return resultado;
        }



        public bool EliminarJefeSala(int idJefeSala) {
            string consulta = @"DELETE FROM CS_JefeSala WHERE idJefeSala = @p1";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", idJefeSala);

                    return query.ExecuteNonQuery() > 0;
                }
            } catch(Exception ex) {
                Console.WriteLine("Error al eliminar. " + ex.Message);
                return false;
            }
        }

        public bool EditarJefeSala(CSJefeSalaEntidad jefeSala) {
            string consulta = @"UPDATE CS_JefeSala 
                                SET Nombres = @p1,
                                    Apellidos = @p2,
                                    Celular = @p3,
                                    SalaId = @p4,
                                    FechaModificacion = @p5,
                                    Codigo = @p6

                                WHERE IdJefeSala = @p7";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", jefeSala.Nombres);
                    query.Parameters.AddWithValue("@p2", jefeSala.Apellidos);
                    query.Parameters.AddWithValue("@p3", jefeSala.Celular);
                    query.Parameters.AddWithValue("@p4", jefeSala.SalaId);
                    query.Parameters.AddWithValue("@p5", jefeSala.FechaModificacion);
                    query.Parameters.AddWithValue("@p6", jefeSala.Codigo);
                    query.Parameters.AddWithValue("@p7", jefeSala.IdJefeSala);

                    return query.ExecuteNonQuery() > 0;
                }
            } catch(Exception ex) {
                Console.WriteLine("Error EditarOpcion: " + ex.Message);
                return false;
            }
        }


        public int CrearJefeSala(CSJefeSalaEntidad jefeSala) {
            int idGenerado = 0;
            string consulta = @"INSERT INTO CS_JefeSala (Nombres, Apellidos, Celular,SalaId, Codigo)
                                OUTPUT INSERTED.IdJefeSala
                                VALUES (@p1, @p2, @p3,@p4, @p5)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", jefeSala.Nombres);
                    query.Parameters.AddWithValue("@p2", jefeSala.Apellidos);
                    query.Parameters.AddWithValue("@p3", jefeSala.Celular);
                    query.Parameters.AddWithValue("@p4", jefeSala.SalaId);
                    query.Parameters.AddWithValue("@p5", jefeSala.Codigo);

                    idGenerado = (int)query.ExecuteScalar();
                }
            } catch(Exception ex) {
                Console.WriteLine("Error CrearOpcion: " + ex.Message);
            }

            return idGenerado;
        }
    }
}
