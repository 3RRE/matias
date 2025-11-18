using CapaEntidad.Campañas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace CapaDatos.Campaña
{
    public class CMP_SalasesionDAL
    {
        string _conexion = string.Empty;

        public CMP_SalasesionDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<CMP_SalasesionEntidad> GetSalassesionxCodsala(int codsala)
        {
            List<CMP_SalasesionEntidad> lista = new List<CMP_SalasesionEntidad>();
            string consulta = @"SELECT [id]
                                  ,[codsala]
                                  ,[fechareg]
                                  ,[estado]
                              FROM [dbo].[CMP_Salasesion] where codsala=@p0";
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
                            var sesion = new CMP_SalasesionEntidad
                            {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),

                                CodSala = ManejoNulos.ManageNullInteger64(dr["CodSala"]),
                                fechareg = ManejoNulos.ManageNullDate(dr["fechareg"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
                            };

                            lista.Add(sesion);
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

        public int GuardarSalasesion(CMP_SalasesionEntidad sala)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[CMP_Salasesion]
           ([codsala]
           ,[fechareg]
           ,[estado])
Output Inserted.id
     VALUES
           (@p0
           ,@p1
           ,@p2);";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger64(sala.CodSala));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullDate(sala.fechareg));
                    query.Parameters.AddWithValue("@p2", 1);
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public bool eliminarSalasesion(Int64 id)
        {
            bool respuesta = false;
            string consulta = @"Delete from [dbo].[CMP_Salasesion]
                 WHERE id=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
    }
}
