using CapaEntidad.ContadoresNegativos;
using CapaEntidad.Disco;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ContadoresNegativos
{
    public class ContadoresNegativosConfigDAL
    {
        string _conexion = string.Empty;
        public ContadoresNegativosConfigDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

        }

        public List<ContadorNegativoConfigEntidad> ContadoresNegativosConfig_Listado()
        {
            List<ContadorNegativoConfigEntidad> lista = new List<ContadorNegativoConfigEntidad>();
            string consulta = @"SELECT 
                                idContadorConfigCargo , 
                                sala_id , 
                                cargo_id , 
                                fechaRegistro
	                            FROM ContadorNegativoConfig order by idContadorConfigCargo desc";
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
                            var contador = new ContadorNegativoConfigEntidad
                            {
                                idContadorConfigCargo = ManejoNulos.ManageNullInteger(dr["idContadorConfigCargo"]),
                                sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                cargo_id = ManejoNulos.ManageNullInteger(dr["cargo_id"]),
                                fechaRegistro = ManejoNulos.ManageNullDate(dr["fechaRegistro"]),

                            };

                            lista.Add(contador);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }


        public int ContadorNegativoConfigInsertarJson(ContadorNegativoConfigEntidad contador)
        {
            //bool response = false;
            int idempleadoDispositivoInsertado = 0;
            string consulta = @"
            INSERT INTO ContadorNegativoConfig(cargo_id, sala_id, fechaRegistro)
	            VALUES (@p0, @p1, @p2) 
                SELECT SCOPE_IDENTITY()";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(contador.cargo_id));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(contador.sala_id));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(contador.fechaRegistro));
                    idempleadoDispositivoInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return idempleadoDispositivoInsertado;
        }


        public bool ContadorNegativoConfigEliminarJson(int alt_id)
        {
            bool response = false;
            string consulta = @"Delete from [ContadorNegativoConfig]
                 WHERE idContadorConfigCargo=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", alt_id);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response = false;
            }
            return response;
        }




    }
}
