using CapaEntidad.Disco;
using CapaEntidad.Discos;
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
    public class IncidenciaDAL
    {
        string _conexion = string.Empty;

        public IncidenciaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int GuardarIncidencia(IncidenciaEntidad incidencia)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO incidencia (titulo, descripcion, idSistema, fecha_registro)
            Output Inserted.idIncidencia
            VALUES(@p0, @p1, @p2, @p3)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(incidencia.titulo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(incidencia.descripcion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger64(incidencia.idSistema));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(incidencia.fecha_registro));
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



        public List<IncidenciaEntidad> IncidenciaListado(int sistemaId) {
            List<IncidenciaEntidad> lista = new List<IncidenciaEntidad>();

            string consulta = @"SELECT
	                            ins.idIncidencia, 
                                ins.titulo, 
                                ins.descripcion, 
                                ins.idSistema, 
                                s.nombre,
                                ins.fecha_registro
                            FROM [Incidencia] ins
                            join SistemaIncidencia s on ins.idSistema=s.idSistema
                            where ins.idSistema = @p0
                            order by idIncidencia desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sistemaId);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var campania = new IncidenciaEntidad
                            {
                                idIncidencia = ManejoNulos.ManageNullInteger64(dr["idIncidencia"]),
                                titulo = ManejoNulos.ManageNullStr(dr["titulo"]),
                                descripcion = ManejoNulos.ManageNullStr(dr["descripcion"]),
                                idSistema = ManejoNulos.ManageNullInteger64(dr["idSistema"]),
                                fecha_registro = ManejoNulos.ManageNullDate(dr["fecha_registro"]),
                            };

                            lista.Add(campania);
                        }
                    }
                }
            }catch(Exception ex)
            {

            }

            return lista;

        }

    }
}
