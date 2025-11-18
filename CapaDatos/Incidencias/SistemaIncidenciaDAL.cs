using CapaEntidad.Incidencias;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Incidencias
{
    public class SistemaIncidenciaDAL
    {
        string _conexion = string.Empty;

        public SistemaIncidenciaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int GuardarSistemaIncidencia(SistemaIncidenciaEntidad sistema)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO SistemaIncidencia (nombre, descripcion, fecha_creacion)
            Output Inserted.idSistema
            VALUES(@p0, @p1, @p2)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(sistema.nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(sistema.descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(sistema.fecha_creacion));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }



        public List<SistemaIncidenciaEntidad> SistemaIncidenciaListado()
        {
            List<SistemaIncidenciaEntidad> lista = new List<SistemaIncidenciaEntidad>();

            string consulta = @"SELECT
	                            sis.idSistema, 
                                sis.nombre, 
                                sis.descripcion, 
                                sis.fecha_creacion
                            FROM [SistemaIncidencia] sis
                            order by idSistema desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var campania = new SistemaIncidenciaEntidad
                            {
                                idSistema = ManejoNulos.ManageNullInteger64(dr["idSistema"]),
                                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
                                fecha_creacion = ManejoNulos.ManageNullDate(dr["fecha_creacion"]),
                            };

                            lista.Add(campania);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return lista;

        }
    }
}
