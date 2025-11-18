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
    public class CMP_DetalleCuponesImpresosDAL
    {
        string _conexion = string.Empty;

        public CMP_DetalleCuponesImpresosDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CMP_DetalleCuponesImpresosEntidad> GetListadoDetalleCuponImpreso(Int64 CgId)
        {
            List<CMP_DetalleCuponesImpresosEntidad> lista = new List<CMP_DetalleCuponesImpresosEntidad>();
            string consulta = @"SELECT cim.[DetImpId]
                                ,cim.[CgId]
                                ,cim.[CodSala]
                                ,cim.[SerieIni]
                                ,cim.[SerieFin]
                                ,cim.[CantidadCuponesImpresos]
                                ,cim.[UltimoCuponImpreso]
	                            ,onc.CodMaq
	                            ,onc.CoinOutAnterior
	                            ,onc.CoinOut
	                            ,onc.CurrentCredits
	                            ,onc.Monto
	                            ,onc.Token
	                            ,onc.FechaRegistro
	                            ,onc.id	
                                ,onc.CoinOutIas,onc.HandPay,onc.JackPot,onc.HandPayAnterior,onc.JackPotAnterior
                            FROM [dbo].[CMP_DetalleCuponesImpresos] cim
                            left join CMP_Contadores_OnLine_Web_Cupones onc on onc.DetalleCuponesImpresos_id= cim.DetImpId
                            where CgId=@p0 order by [DetImpId] asc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", CgId);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var cupon = new CMP_DetalleCuponesImpresosEntidad
                            {
                                DetImpId = ManejoNulos.ManageNullInteger64(dr["DetImpId"]),
                                CgId = ManejoNulos.ManageNullInteger64(dr["CgId"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                SerieIni = ManejoNulos.ManageNullStr(dr["SerieIni"]),
                                SerieFin = ManejoNulos.ManageNullStr(dr["SerieFin"]),
                                CantidadCuponesImpresos = ManejoNulos.ManageNullInteger(dr["CantidadCuponesImpresos"]),
                                UltimoCuponImpreso = ManejoNulos.ManageNullStr(dr["UltimoCuponImpreso"]),
                                CoinOutIas = ManejoNulos.ManageNullDouble(dr["CoinOutIas"]),
                                CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]),
                                CoinOutAnterior = ManejoNulos.ManageNullDouble(dr["CoinOutAnterior"]),
                                CoinOut = ManejoNulos.ManageNullDouble(dr["CoinOut"]),
                                CurrentCredits = ManejoNulos.ManageNullDouble(dr["CurrentCredits"]),
                                Monto = ManejoNulos.ManageNullDecimal(dr["Monto"]),
                                Token = ManejoNulos.ManageNullDecimal(dr["Token"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                HandPay = ManejoNulos.ManageNullDouble(dr["HandPay"]),
                                JackPot = ManejoNulos.ManageNullDouble(dr["JackPot"]),
                                HandPayAnterior = ManejoNulos.ManageNullDouble(dr["HandPayAnterior"]),
                                JackPotAnterior = ManejoNulos.ManageNullDouble(dr["JackPotAnterior"]),
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
        public CMP_DetalleCuponesImpresosEntidad GetDetalleCuponImpresoId(Int64 DetImpId)
        {
            CMP_DetalleCuponesImpresosEntidad cupon = new CMP_DetalleCuponesImpresosEntidad();
            string consulta = @"SELECT [DetImpId]
                                      ,[CgId]
                                      ,[CodSala]
                                      ,[SerieIni]
                                      ,[SerieFin]
                                      ,[CantidadCuponesImpresos]
                                      ,[UltimoCuponImpreso]
                                  FROM [dbo].[CMP_DetalleCuponesImpresos] where DetImpId=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", DetImpId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                cupon.DetImpId = ManejoNulos.ManageNullInteger64(dr["DetGenId"]);
                                cupon.CgId = ManejoNulos.ManageNullInteger64(dr["CgId"]);
                                cupon.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                cupon.SerieIni = ManejoNulos.ManageNullStr(dr["SerieIni"]);
                                cupon.SerieFin = ManejoNulos.ManageNullStr(dr["SerieFin"]);
                                cupon.CantidadCuponesImpresos= ManejoNulos.ManageNullInteger(dr["CantidadCuponesImpresos"]);
                                cupon.UltimoCuponImpreso = ManejoNulos.ManageNullStr(dr["UltimoCuponImpreso"]);
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
        public Int64 GuardarDetalleCuponImpreso(CMP_DetalleCuponesImpresosEntidad cupon)
        {
            //bool respuesta = false;
            Int64 IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[CMP_DetalleCuponesImpresos]
           ([CgId]
           ,[CodSala]
           ,[SerieIni]
           ,[SerieFin]
           ,[CantidadCuponesImpresos]
           ,[UltimoCuponImpreso])
Output Inserted.DetImpId
     VALUES
           (@CgId
           ,@CodSala
           ,@SerieIni
           ,@SerieFin
           ,@CantidadCuponesImpresos
           ,@UltimoCuponImpreso)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CgId", ManejoNulos.ManageNullInteger64(cupon.CgId));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(cupon.CodSala));
                    query.Parameters.AddWithValue("@SerieIni", ManejoNulos.ManageNullStr(cupon.SerieIni));
                    query.Parameters.AddWithValue("@SerieFin", ManejoNulos.ManageNullStr(cupon.SerieFin));
                    query.Parameters.AddWithValue("@CantidadCuponesImpresos", ManejoNulos.ManageNullInteger(cupon.CantidadCuponesImpresos));
                    query.Parameters.AddWithValue("@UltimoCuponImpreso", ManejoNulos.ManageNullStr(cupon.UltimoCuponImpreso));
                    IdInsertado = Convert.ToInt64(query.ExecuteScalar());
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
        public bool EditarDetalleCuponImpreso(CMP_DetalleCuponesImpresosEntidad cupon)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CMP_DetalleCuponesImpresos]
               SET 
                  [SerieIni] = @SerieIni
                  ,[SerieFin] = @SerieFin
                  ,[UltimoCuponImpreso] = @UltimoCuponImpreso
                 WHERE DetImpId=@DetImpId";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
  
                    query.Parameters.AddWithValue("@SerieIni", ManejoNulos.ManageNullStr(cupon.SerieIni));
                    query.Parameters.AddWithValue("@SerieFin", ManejoNulos.ManageNullStr(cupon.SerieFin));
                    query.Parameters.AddWithValue("@UltimoCuponImpreso", ManejoNulos.ManageNullStr(cupon.UltimoCuponImpreso));
                    query.Parameters.AddWithValue("@DetImpId", ManejoNulos.ManageNullInteger64(cupon.DetImpId));
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
        public List<CMP_DetalleCuponesImpresosEntidad> GetListadoDetalleCuponImpresoExcel(string whereQuery)
        {
            List<CMP_DetalleCuponesImpresosEntidad> lista = new List<CMP_DetalleCuponesImpresosEntidad>();
            string consulta = @"SELECT cim.[DetImpId]
                                ,cim.[CgId]
                                ,cim.[CodSala]
                                ,cim.[SerieIni]
                                ,cim.[SerieFin]
                                ,cim.[CantidadCuponesImpresos]
                                ,cim.[UltimoCuponImpreso]
	                            ,onc.CodMaq
	                            ,onc.CoinOutAnterior
	                            ,onc.CoinOut
	                            ,onc.CurrentCredits
	                            ,onc.Monto
	                            ,onc.Token
	                            ,onc.FechaRegistro
	                            ,onc.id	
                                ,onc.CoinOutIas,onc.HandPay,onc.JackPot,onc.HandPayAnterior,onc.JackPotAnterior
                            FROM [dbo].[CMP_DetalleCuponesImpresos] cim
                            left join CMP_Contadores_OnLine_Web_Cupones onc on onc.DetalleCuponesImpresos_id= cim.DetImpId " + whereQuery+ " order by [DetImpId] asc";
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
                            var cupon = new CMP_DetalleCuponesImpresosEntidad
                            {
                                DetImpId = ManejoNulos.ManageNullInteger64(dr["DetImpId"]),
                                CgId = ManejoNulos.ManageNullInteger64(dr["CgId"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                SerieIni = ManejoNulos.ManageNullStr(dr["SerieIni"]),
                                SerieFin = ManejoNulos.ManageNullStr(dr["SerieFin"]),
                                CantidadCuponesImpresos = ManejoNulos.ManageNullInteger(dr["CantidadCuponesImpresos"]),
                                UltimoCuponImpreso = ManejoNulos.ManageNullStr(dr["UltimoCuponImpreso"]),
                                CoinOutIas = ManejoNulos.ManageNullDouble(dr["CoinOutIas"]),
                                CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]),
                                CoinOutAnterior = ManejoNulos.ManageNullDouble(dr["CoinOutAnterior"]),
                                CoinOut = ManejoNulos.ManageNullDouble(dr["CoinOut"]),
                                CurrentCredits = ManejoNulos.ManageNullDouble(dr["CurrentCredits"]),
                                Monto = ManejoNulos.ManageNullDecimal(dr["Monto"]),
                                Token = ManejoNulos.ManageNullDecimal(dr["Token"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                HandPay = ManejoNulos.ManageNullDouble(dr["HandPay"]),
                                JackPot = ManejoNulos.ManageNullDouble(dr["JackPot"]),
                                HandPayAnterior = ManejoNulos.ManageNullDouble(dr["HandPayAnterior"]),
                                JackPotAnterior = ManejoNulos.ManageNullDouble(dr["JackPotAnterior"]),
                            };

                            lista.Add(cupon);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lista.Clear();
            }
            return lista;
        }
    }
}
