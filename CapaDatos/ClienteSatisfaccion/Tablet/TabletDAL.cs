using CapaEntidad.ClienteSatisfaccion.Entidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ClienteSatisfaccion.Tablet {
    public class TabletDAL {
        string _conexion = string.Empty;

        public TabletDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<TabletEntidad> ListadoTablets(int salaId) {
            List<TabletEntidad> lista = new List<TabletEntidad>();
            string consulta = @"SELECT [idTablet]
                                  ,[guid]
                                  ,[nombre]
                                  ,[salaId]
                                  ,[activa]
                              FROM Tablet 
                              WHERE salaId=@p1";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", salaId);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new TabletEntidad {
                                IdTablet = ManejoNulos.ManageNullInteger(dr["idTablet"]),
                                Guid = ManejoNulos.ManageNullStr(dr["Guid"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                SalaId = ManejoNulos.ManageNullInteger(dr["SalaId"]),
                                Activa = ManejoNulos.ManegeNullBool(dr["Activa"]),
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
       
        public bool CrearTablet(TabletEntidad tablet) {
            string consulta = @"INSERT INTO Tablet 
                                (Nombre, salaId, activa) 
                                VALUES (@nombre, @salaId, @activa)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@nombre", tablet.Nombre);
                    cmd.Parameters.AddWithValue("@salaId", tablet.SalaId);
                    cmd.Parameters.AddWithValue("@activa", tablet.Activa);

                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            } catch(Exception ex) {
                Console.WriteLine("Error CrearTablet: " + ex.Message);
                return false;
            }
        }

       
        public bool EditarTablet(string nombreTablet, bool activo, int idTablet) {
            string consulta = @"UPDATE Tablet 
                                SET Activa = @activo,Nombre = @nombre
                                WHERE idTablet = @idTablet";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@nombre", nombreTablet);
                    cmd.Parameters.AddWithValue("@idTablet", idTablet);

                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            } catch(Exception ex) {
                Console.WriteLine("Error EditarTablet: " + ex.Message);
                return false;
            }
        }

        public bool ExisteTabletEnSala(int salaId, int tabletId) {
            string consulta = @"SELECT COUNT(*) 
                        FROM Tablet 
                        WHERE IdTablet = @tabletId AND SalaId = @salaId";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@tabletId", tabletId);
                        cmd.Parameters.AddWithValue("@salaId", salaId);

                        int count = (int)cmd.ExecuteScalar();
                        return count > 0; // true si existe, false si no
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error ExisteTabletEnSala: " + ex.Message);
                return false;
            }
        }



    }
}
