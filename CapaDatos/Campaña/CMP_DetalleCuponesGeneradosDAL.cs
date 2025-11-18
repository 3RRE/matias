using CapaEntidad.Campañas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Campaña
{
    public class CMP_DetalleCuponesGeneradosDAL
    {
        string _conexion = string.Empty;

        public CMP_DetalleCuponesGeneradosDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CMP_DetalleCuponesGeneradosEntidad> GetListadoDetalleCuponGenerado(Int64 DetImId)
        {
            List<CMP_DetalleCuponesGeneradosEntidad> lista = new List<CMP_DetalleCuponesGeneradosEntidad>();
            string consulta = @"SELECT [DetGenId]
                                      ,[DetImId]
                                      ,[CodSala]
                                      ,[Serie]
                                      ,[CantidadImpresiones]
                                      ,[Fecha]
                                        ,[UsuarioId]
                                  FROM [dbo].[CMP_DetalleCuponesGenerados] where DetImId=@p0 order by Serie asc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", DetImId);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var cupon = new CMP_DetalleCuponesGeneradosEntidad
                            {
                                DetGenId = ManejoNulos.ManageNullInteger64(dr["DetGenId"]),
                                DetImId = ManejoNulos.ManageNullInteger64(dr["DetImId"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Serie = ManejoNulos.ManageNullStr(dr["Serie"]),
                                CantidadImpresiones = ManejoNulos.ManageNullInteger(dr["CantidadImpresiones"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                UsuarioId = ManejoNulos.ManageNullInteger(dr["UsuarioId"]),
                            };

                            lista.Add(cupon);
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

        public Int64 CuponesTotalJson(Int64 codsala)
        {
            Int64 total = 0;

            string consulta = @"SELECT 
								COUNT(*) as total							
                                FROM[dbo].[CMP_DetalleCuponesGenerados] 
                                where CodSala=@p0;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", codsala);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                total = ManejoNulos.ManageNullInteger64(dr["total"]);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return total;
        }

        public CMP_DetalleCuponesGeneradosEntidad GetDetalleCuponGeneradoId(Int64 DetGenId)
        {
            CMP_DetalleCuponesGeneradosEntidad cupon = new CMP_DetalleCuponesGeneradosEntidad();
            string consulta = @"SELECT [DetGenId]
                                      ,[DetImId]
                                      ,[CodSala]
                                      ,[Serie]
                                      ,[CantidadImpresiones]
                                      ,[Fecha],[UsuarioId]
                                  FROM [dbo].[CMP_DetalleCuponesGenerados] where DetGenId=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", DetGenId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                cupon.DetGenId = ManejoNulos.ManageNullInteger64(dr["DetGenId"]);
                                cupon.DetImId = ManejoNulos.ManageNullInteger64(dr["DetImId"]);
                                cupon.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                cupon.Serie = ManejoNulos.ManageNullStr(dr["Serie"]);
                                cupon.CantidadImpresiones = ManejoNulos.ManageNullInteger(dr["CantidadImpresiones"]);
                                cupon.Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]);
                                cupon.UsuarioId = ManejoNulos.ManageNullInteger(dr["UsuarioId"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cupon;
        }
        public int GuardarDetalleCuponGenerado(CMP_DetalleCuponesGeneradosEntidad cupon)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[CMP_DetalleCuponesGenerados]
           ([DetImId]
           ,[CodSala]
           ,[Serie]
           ,[CantidadImpresiones]
           ,[Fecha],[UsuarioId])
Output Inserted.DetGenId
     VALUES
           (@DetImId
           ,@CodSala
           ,@Serie
           ,@CantidadImpresiones
           ,@Fecha,@UsuarioId)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@DetImId", ManejoNulos.ManageNullInteger64(cupon.DetImId));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(cupon.CodSala));
                    query.Parameters.AddWithValue("@Serie", ManejoNulos.ManageNullStr(cupon.Serie));
                    query.Parameters.AddWithValue("@CantidadImpresiones", ManejoNulos.ManageNullInteger(cupon.CantidadImpresiones));
                    query.Parameters.AddWithValue("@Fecha", ManejoNulos.ManageNullDate(cupon.Fecha));
                    query.Parameters.AddWithValue("@UsuarioId", ManejoNulos.ManageNullInteger(cupon.UsuarioId));
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
        public bool EditarDetalleCuponGenerado(CMP_DetalleCuponesGeneradosEntidad cupon)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CMP_DetalleCuponesGenerados]
                   SET [DetImId] = @DetImId
                      ,[CodSala] = @CodSala
                      ,[Serie] = @Serie
                      ,[CantidadImpresiones] = @CantidadImpresiones
                      ,[Fecha] = @Fecha,[UsuarioId]=@UsuarioId
                 WHERE DetGenId=@DetGenId";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@DetImId", ManejoNulos.ManageNullInteger64(cupon.DetImId));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(cupon.CodSala));
                    query.Parameters.AddWithValue("@Serie", ManejoNulos.ManageNullStr(cupon.Serie));
                    query.Parameters.AddWithValue("@CantidadImpresiones", ManejoNulos.ManageNullInteger(cupon.CantidadImpresiones));
                    query.Parameters.AddWithValue("@Fecha", ManejoNulos.ManageNullDate(cupon.Fecha));
                    query.Parameters.AddWithValue("@DetGenId", ManejoNulos.ManageNullInteger64(cupon.DetGenId));
                    query.Parameters.AddWithValue("@UsuarioId", ManejoNulos.ManageNullInteger(cupon.UsuarioId));
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
        public CMP_DetalleCuponesGeneradosEntidad GetDetalleCuponGeneradoPorSerie(string Serie)
        {
            CMP_DetalleCuponesGeneradosEntidad cupon = new CMP_DetalleCuponesGeneradosEntidad();
            string consulta = @"SELECT [DetGenId]
                                      ,[DetImId]
                                      ,[CodSala]
                                      ,[Serie]
                                      ,[CantidadImpresiones]
                                      ,[Fecha],[UsuarioId]
                                  FROM [dbo].[CMP_DetalleCuponesGenerados] where Serie=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", Serie);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                cupon.DetGenId = ManejoNulos.ManageNullInteger64(dr["DetGenId"]);
                                cupon.DetImId = ManejoNulos.ManageNullInteger64(dr["DetImId"]);
                                cupon.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                cupon.Serie = ManejoNulos.ManageNullStr(dr["Serie"]);
                                cupon.CantidadImpresiones = ManejoNulos.ManageNullInteger(dr["CantidadImpresiones"]);
                                cupon.Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]);
                                cupon.UsuarioId = ManejoNulos.ManageNullInteger(dr["UsuarioId"]);
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cupon;
        }
 
        public bool AumentarCantidadImpresionesDetalleCuponGeneradoPorDetGenId(string InQuery,int UsuarioId)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CMP_DetalleCuponesGenerados]
                   SET 
                      [CantidadImpresiones] = CantidadImpresiones+1,
[UsuarioId]=@UsuarioId,
[Fecha]=getdate()
                 WHERE DetGenId in (" + InQuery + ")";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@UsuarioId", ManejoNulos.ManageNullInteger(UsuarioId));
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
        public CMP_DetalleCuponesGeneradosEntidad GetUltimoDetalleCuponGeneradoPorSerieySala(int CodSala)
        {
            CMP_DetalleCuponesGeneradosEntidad cupon = new CMP_DetalleCuponesGeneradosEntidad();
            string consulta = @"SELECT top 1 [DetGenId]
                                      ,[DetImId]
                                      ,[CodSala]
                                      ,[Serie]
                                      ,[CantidadImpresiones]
                                      ,[Fecha],[UsuarioId]
                                  FROM [dbo].[CMP_DetalleCuponesGenerados] where CodSala=@CodSala order by Serie desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", CodSala);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                cupon.DetGenId = ManejoNulos.ManageNullInteger64(dr["DetGenId"]);
                                cupon.DetImId = ManejoNulos.ManageNullInteger64(dr["DetImId"]);
                                cupon.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                cupon.Serie = ManejoNulos.ManageNullStr(dr["Serie"]);
                                cupon.CantidadImpresiones = ManejoNulos.ManageNullInteger(dr["CantidadImpresiones"]);
                                cupon.Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]);
                                cupon.UsuarioId = ManejoNulos.ManageNullInteger(dr["UsuarioId"]);
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cupon;
        }
    }
}
