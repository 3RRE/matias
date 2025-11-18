using CapaEntidad.Alertas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Alertas
{
    public class ALT_AlertaCargoConfDAL
    {
        string _conexion = string.Empty;
        public ALT_AlertaCargoConfDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<ALT_AlertaCargoConfEntidad> ALT_AlertaCargoConf_Listado()
        {
            List<ALT_AlertaCargoConfEntidad> lista = new List<ALT_AlertaCargoConfEntidad>();
            string consulta = @"SELECT 
                                alt_id , 
                                sala_id , 
                                cargo_id , 
                                alt_fechareg
	                            FROM ALT_AlertaCargoConfiguracion order by alt_id desc";
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
                            var campaña = new ALT_AlertaCargoConfEntidad
                            {
                                alt_id = ManejoNulos.ManageNullInteger(dr["alt_id"]),
                                sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),cargo_id = ManejoNulos.ManageNullInteger(dr["cargo_id"]),
                                alt_fechareg = ManejoNulos.ManageNullDate(dr["alt_fechareg"]),

                        };

                            lista.Add(campaña);
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

        public List<ALT_AlertaCargoConfEntidad> ALT_AlertaCargoxSala_Listado(string codsala)
        {
            List<ALT_AlertaCargoConfEntidad> lista = new List<ALT_AlertaCargoConfEntidad>();
            string consulta = @"SELECT 
                                alt_id , 
                                sala_id , 
                                cargo_id , 
                                alt_fechareg
	                            FROM ALT_AlertaCargoConfiguracion " + codsala+"  order by alt_id desc";
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
                            var campaña = new ALT_AlertaCargoConfEntidad
                            {
                                alt_id = ManejoNulos.ManageNullInteger(dr["alt_id"]),
                                sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                cargo_id = ManejoNulos.ManageNullInteger(dr["cargo_id"]),
                                alt_fechareg = ManejoNulos.ManageNullDate(dr["alt_fechareg"]),

                            };

                            lista.Add(campaña);
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

        public List<ALT_AlertaCargoConfEntidad> ALT_AlertaCargoxSala_idListado(int codsala)
        {
            List<ALT_AlertaCargoConfEntidad> lista = new List<ALT_AlertaCargoConfEntidad>();
            string consulta = @"SELECT 
                                alt_id , 
                                sala_id , 
                                cargo_id , 
                                alt_fechareg
	                            FROM ALT_AlertaCargoConfiguracion where sala_id=@p0  order by alt_id desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", codsala);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var campaña = new ALT_AlertaCargoConfEntidad
                            {
                                alt_id = ManejoNulos.ManageNullInteger(dr["alt_id"]),
                                sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                cargo_id = ManejoNulos.ManageNullInteger(dr["cargo_id"]),
                                alt_fechareg = ManejoNulos.ManageNullDate(dr["alt_fechareg"]),

                            };

                            lista.Add(campaña);
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

        public ALT_AlertaCargoConfEntidad ALT_AlertaCargoConf_IdObtenerJson(Int64 alt_id)
        {
            ALT_AlertaCargoConfEntidad alertaCargoConfEntidad = new ALT_AlertaCargoConfEntidad();
            string consulta = @"SELECT 
                                alt_id , 
                                sala_id , 
                                cargo_id , 
                                alt_fechareg
	                            FROM ALT_AlertaCargoConfiguracion
                                where alt_id=@p0;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", alt_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                alertaCargoConfEntidad.alt_id = ManejoNulos.ManageNullInteger(dr["alt_id"]);
                                alertaCargoConfEntidad.sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]);
                                alertaCargoConfEntidad.cargo_id = ManejoNulos.ManageNullInteger(dr["cargo_id"]);
                                alertaCargoConfEntidad.alt_fechareg = ManejoNulos.ManageNullDate(dr["alt_fechareg"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return alertaCargoConfEntidad;
        }

        public int ALT_AlertaCargoConfInsertarJson(ALT_AlertaCargoConfEntidad AlertaCargo, int tipo)
        {
            //bool response = false;
            int idempleadoDispositivoInsertado = 0;
            string consulta = @"
            INSERT INTO ALT_AlertaCargoConfiguracion(sala_id, cargo_id, alt_fechareg, tipo)
	            VALUES (@p0, @p1, @p2, @p3) 
                SELECT SCOPE_IDENTITY()";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(AlertaCargo.sala_id));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(AlertaCargo.cargo_id));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(AlertaCargo.alt_fechareg));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(tipo));
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

        public bool ALT_AlertaCargoConfEditarJson(ALT_AlertaCargoConfEntidad AlertaCargo)
        {

            bool response = false;
            string consulta = @"UPDATE ALT_AlertaCargoConfiguracion
	                            SET sala_id=@p1, cargo_id=@p2, 
	                            WHERE alt_id=@p4;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(AlertaCargo.sala_id));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(AlertaCargo.cargo_id));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(AlertaCargo.alt_fechareg));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(AlertaCargo.alt_id));
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

        public bool ALT_AlertaCargoConfEliminarJson(int alt_id)
        {
            bool response = false;
            string consulta = @"Delete from [ALT_AlertaCargoConfiguracion]
                 WHERE alt_id=@p0";
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

        public bool EliminarCargoAlertaSala(int sala_id, int cargo_id)
        {
            bool response = false;
            string consulta = @"Delete from [ALT_AlertaCargoConfiguracion]
                 WHERE sala_id=@p0 and cargo_id=@p1";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sala_id);
                    query.Parameters.AddWithValue("@p1", cargo_id);
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
