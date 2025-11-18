using CapaEntidad.Campañas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
namespace CapaDatos.Campaña
{
    public class CMP_SalalibreDAL
    {
        string _conexion = string.Empty;

        public CMP_SalalibreDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CMP_SalalibreEntidad> GetSalaslibre()
        {
            List<CMP_SalalibreEntidad> lista = new List<CMP_SalalibreEntidad>();
            string consulta = @"select s.CodSala,
                                    e.CodEmpresa,
                                    e.RazonSocial,
                                    s.Nombre nombresala,
                                    c.id ,
                                    se.id Salasesion_id
                                    from Sala s 
                                join Empresa e on e.CodEmpresa= s.CodEmpresa
                                left join CMP_Salasesion se on se.codsala=s.CodSala
                                left join CMP_Salalibre c on c.codsala=s.CodSala order by e.CodEmpresa desc";
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
                            var campaña = new CMP_SalalibreEntidad
                            {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger64(dr["CodEmpresa"]),
                                CodSala = ManejoNulos.ManageNullInteger64(dr["CodSala"]),
                                RazonSocial = ManejoNulos.ManageNullStr(dr["RazonSocial"]),
                                nombresala = ManejoNulos.ManageNullStr(dr["nombresala"]),
                                Salasesion_id = ManejoNulos.ManageNullInteger64(dr["Salasesion_id"]),
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

        public List<CMP_SalalibreEntidad> GetSalaslibrexCodsala(int codsala)
        {
            List<CMP_SalalibreEntidad> lista = new List<CMP_SalalibreEntidad>();
            string consulta = @"SELECT [id]
                                  ,[codsala]
                                  ,[fechareg]
                                  ,[estado]
                              FROM [dbo].[CMP_Salalibre] where codsala=@p0";
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
                            var campaña = new CMP_SalalibreEntidad
                            {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                             
                                CodSala = ManejoNulos.ManageNullInteger64(dr["CodSala"]),
                                fechareg = ManejoNulos.ManageNullDate(dr["fechareg"]),
                                estado = ManejoNulos.ManageNullInteger(dr["estado"]),
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
        public CMP_SalalibreEntidad GetSalaLibreID(int id)
        {
            CMP_SalalibreEntidad sala = new CMP_SalalibreEntidad();
            string consulta = @"select s.CodSala,
                                    e.CodEmpresa,
                                    e.RazonSocial,
                                    s.Nombre nombresala,
                                    c.id 
                                    from Sala s 
                                join Empresa e on e.CodEmpresa= s.CodEmpresa
                                left join CMP_Salalibre c on c.id=s.CodSala where c.id=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                sala.id = ManejoNulos.ManageNullInteger64(dr["id"]);
                                sala.CodEmpresa = ManejoNulos.ManageNullInteger64(dr["CodEmpresa"]);
                                sala.CodSala = ManejoNulos.ManageNullInteger64(dr["CodSala"]);
                                sala.RazonSocial = ManejoNulos.ManageNullStr(dr["RazonSocial"]);
                                sala.nombresala = ManejoNulos.ManageNullStr(dr["nombresala"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sala;
        }

        public int GuardarSalalibre(CMP_SalalibreEntidad sala)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[CMP_Salalibre]
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

        public bool eliminarSalalibre(Int64 id)
        {
            bool respuesta = false;
            string consulta = @"Delete from [dbo].[CMP_Salalibre]
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
