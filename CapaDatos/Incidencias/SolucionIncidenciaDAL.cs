using CapaEntidad;
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
    public class SolucionIncidenciaDAL
    {


        string _conexion = string.Empty;

        public SolucionIncidenciaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int GuardarSolucion(SolucionIncidenciaEntidad solucion)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO SolucionIncidencia (nombre, descripcion, idIncidencia, fecha_registro)
            Output Inserted.idSolucion
            VALUES(@p0, @p1, @p2,@p3)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(solucion.nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(solucion.descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger64(solucion.idIncidencia));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(solucion.fecha_registro));
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



        public List<SolucionIncidenciaEntidad> SolucionIncidenciaListado(int idSolucion)
        {
            List<SolucionIncidenciaEntidad> lista = new List<SolucionIncidenciaEntidad>();

            string consulta = @"SELECT
	                            sol.idSolucion, 
                                sol.nombre, 
                                sol.descripcion, 
                                sol.fecha_registro
                            FROM [SolucionIncidencia] sol
                            where sol.idIncidencia = @p0
                            order by idSolucion desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", idSolucion);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var campania = new SolucionIncidenciaEntidad
                            {
                                idSolucion = ManejoNulos.ManageNullInteger64(dr["idSolucion"]),
                                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
                                fecha_registro = ManejoNulos.ManageNullDate(dr["fecha_registro"]),
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
