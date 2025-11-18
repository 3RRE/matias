using CapaEntidad.ClienteSatisfaccion.Entidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ClienteSatisfaccion.Opciones {
    public class OpcionesDAL {
        string _conexion = string.Empty;

        public OpcionesDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<OpcionEntidad> ListadoOpciones() {
            List<OpcionEntidad> lista = new List<OpcionEntidad>();
            string consulta = @"SELECT  [idOpcion]
                              ,[idPregunta]
                              ,[texto]
                              ,[tieneComentario]
                          FROM Opcion";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new OpcionEntidad {
                                IdOpcion = ManejoNulos.ManageNullInteger(dr["IdOpcion"]),
                                IdPregunta = ManejoNulos.ManageNullInteger(dr["IdPregunta"]),
                                Texto = ManejoNulos.ManageNullStr(dr["Texto"]),
                                TieneComentario = ManejoNulos.ManegeNullBool(dr["TieneComentario"]),
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

        // 🔹 Listado de opciones por pregunta
        public List<OpcionEntidad> ListadoOpciones(int idPregunta) {
            var lista = new List<OpcionEntidad>();
            string consulta = @"SELECT [idOpcion], [idPregunta], [texto], [tieneComentario]
                                FROM Opcion
                                WHERE idPregunta = @p1";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", idPregunta);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            lista.Add(new OpcionEntidad {
                                IdOpcion = ManejoNulos.ManageNullInteger(dr["idOpcion"]),
                                IdPregunta = ManejoNulos.ManageNullInteger(dr["idPregunta"]),
                                Texto = ManejoNulos.ManageNullStr(dr["texto"]),
                                TieneComentario = ManejoNulos.ManegeNullBool(dr["tieneComentario"])
                            });
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error ListadoOpciones: " + ex.Message);
            }

            return lista;
        }

        // 🔹 Crear opción
        public int CrearOpcion(OpcionEntidad opcion) {
            int idGenerado = 0;
            string consulta = @"INSERT INTO Opcion (idPregunta, texto, tieneComentario, valor)
                                OUTPUT INSERTED.idOpcion
                                VALUES (@p1, @p2, @p3,@p4)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", opcion.IdPregunta);
                    query.Parameters.AddWithValue("@p2", opcion.Texto);
                    query.Parameters.AddWithValue("@p3", opcion.TieneComentario);
                    query.Parameters.AddWithValue("@p4", (object)opcion.Valor ?? DBNull.Value);

                    idGenerado = (int)query.ExecuteScalar();
                }
            } catch(Exception ex) {
                Console.WriteLine("Error CrearOpcion: " + ex.Message);
            }

            return idGenerado;
        }

        // 🔹 Editar opción
        public bool EditarOpcion(OpcionEntidad opcion) {
            string consulta = @"UPDATE Opcion 
                                SET texto = @p1, tieneComentario = @p2
                                WHERE idOpcion = @p3";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", opcion.Texto);
                    query.Parameters.AddWithValue("@p2", opcion.TieneComentario);
                    query.Parameters.AddWithValue("@p3", opcion.IdOpcion);

                    return query.ExecuteNonQuery() > 0;
                }
            } catch(Exception ex) {
                Console.WriteLine("Error EditarOpcion: " + ex.Message);
                return false;
            }
        }

        // 🔹 Eliminar opción
        public bool EliminarOpcion(int idOpcion) {
            string consulta = @"DELETE FROM Opcion WHERE idOpcion = @p1";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", idOpcion);

                    return query.ExecuteNonQuery() > 0;
                }
            } catch(Exception ex) {
                Console.WriteLine("Error EliminarOpcion: " + ex.Message);
                return false;
            }
        }
    }


}
