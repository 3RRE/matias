using CapaEntidad.Campañas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Campaña
{
    public class CMP_ContadoresOnlineWebCuponesDAL
    {
        string _conexion = string.Empty;

        public CMP_ContadoresOnlineWebCuponesDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CMP_ContadoresOnlineWebCuponesEntidad> GetListadoCMP_ContadoresOnlineWebCupones()
        {
            List<CMP_ContadoresOnlineWebCuponesEntidad> lista = new List<CMP_ContadoresOnlineWebCuponesEntidad>();
            string consulta = @"SELECT [id]
                              ,[Cod_Cont]
                              ,[Cod_Cont_OL]
                              ,[Fecha]
                              ,[Hora]
                              ,[CodMaq]
                              ,[CodMaqMin]
                              ,[CoinOut]
                              ,[CurrentCredits]
                              ,[Monto]
                              ,[Token]
                              ,[CoinOutAnterior]
                              ,[Estado_Oln]
                              ,[Win]
                              ,[CantidadCupones]
                              ,[FechaRegistro]
                              ,[CodCliente]
                              ,[CoinOutIas]
                              ,[CodSala]
                              ,[Estado_Envio]
                              ,[FechaLlegada]
                                ,DetalleCuponesImpresos_id,HandPay,JackPot,HandPayAnterior,JackPotAnterior
                          FROM [dbo].[CMP_Contadores_OnLine_Web_Cupones] order by id asc";
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
                            var cupon = new CMP_ContadoresOnlineWebCuponesEntidad
                            {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                Cod_Cont = ManejoNulos.ManageNullInteger64(dr["Cod_Cont"]),
                                Cod_Cont_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Cont_OL"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Hora = ManejoNulos.ManageNullDate(dr["Hora"]),
                                CodMaq = ManejoNulos.ManageNullStr(dr["SlotId"]),
                                CodMaqMin = ManejoNulos.ManageNullStr(dr["Juego"]),
                                CoinOut = ManejoNulos.ManageNullDouble(dr["Marca"]),
                                CurrentCredits = ManejoNulos.ManageNullDouble(dr["Modelo"]),
                                Monto = ManejoNulos.ManageNullDouble(dr["Win"]),
                                Token = ManejoNulos.ManageNullDouble(dr["Parametro"]),
                                CoinOutAnterior = ManejoNulos.ManageNullDouble(dr["ValorJuego"]),
                                Estado_Oln = ManejoNulos.ManageNullInteger(dr["CantidadCupones"]),
                                Win = ManejoNulos.ManageNullDouble(dr["SaldoCupIni"]),
                                CantidadCupones = ManejoNulos.ManageNullInteger(dr["SaldoCupFin"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["SerieIni"]),
                                CodCliente = ManejoNulos.ManageNullStr(dr["SerieFin"]),
                                CoinOutIas = ManejoNulos.ManageNullDouble(dr["CoinOutIas"]),
                                EstadoEnvio = ManejoNulos.ManageNullInteger(dr["Hora"]),
                                CodSala = ManejoNulos.ManageNullStr(dr["CodSala"]),
                                FechaLlegada = ManejoNulos.ManageNullDate(dr["FechaLlegada"]),
                                DetalleCuponesImpresos_id = ManejoNulos.ManageNullInteger64(dr["DetalleCuponesImpresos_id"]),
                                HandPay = ManejoNulos.ManageNullDouble(dr["HandPay"]),
                                JackPot= ManejoNulos.ManageNullDouble(dr["JackPot"]),
                                HandPayAnterior= ManejoNulos.ManageNullDouble(dr["HandPayAnterior"]),
                                JackPotAnterior= ManejoNulos.ManageNullDouble(dr["JackPotAnterior"]),
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

        public CMP_ContadoresOnlineWebCuponesEntidad GetCMP_ContadoresOnlineWebCuponesId(Int64 id)
        {
            CMP_ContadoresOnlineWebCuponesEntidad contadoresonline = new CMP_ContadoresOnlineWebCuponesEntidad();
            string consulta = @"SELECT [id]
                              ,[Cod_Cont]
                              ,[Cod_Cont_OL]
                              ,[Fecha]
                              ,[Hora]
                              ,[CodMaq]
                              ,[CodMaqMin]
                              ,[CoinOut]
                              ,[CurrentCredits]
                              ,[Monto]
                              ,[Token]
                              ,[CoinOutAnterior]
                              ,[Estado_Oln]
                              ,[Win]
                              ,[CantidadCupones]
                              ,[FechaRegistro]
                              ,[CodCliente]
                              ,[CoinOutIas]
                              ,[CodSala]
                              ,[Estado_Envio]
                              ,[FechaLlegada]
                                ,DetalleCuponesImpresos_id,HandPay,JackPot,HandPayAnterior,JackPotAnterior
                          FROM [dbo].[CMP_Contadores_OnLine_Web_Cupones] where id=@p0";
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

                                contadoresonline.id = ManejoNulos.ManageNullInteger64(dr["id"]);
                                contadoresonline.Cod_Cont = ManejoNulos.ManageNullInteger64(dr["Cod_Cont"]);
                                contadoresonline.Cod_Cont_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Cont_OL"]);
                                contadoresonline.Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]);
                                contadoresonline.Hora = ManejoNulos.ManageNullDate(dr["Hora"]);
                                contadoresonline.CodMaq = ManejoNulos.ManageNullStr(dr["SlotId"]);
                                contadoresonline.CodMaqMin = ManejoNulos.ManageNullStr(dr["Juego"]);
                                contadoresonline.CoinOut = ManejoNulos.ManageNullDouble(dr["Marca"]);
                                contadoresonline.CurrentCredits = ManejoNulos.ManageNullDouble(dr["Modelo"]);
                                contadoresonline.Monto = ManejoNulos.ManageNullDouble(dr["Win"]);
                                contadoresonline.Token = ManejoNulos.ManageNullDouble(dr["Parametro"]);
                                contadoresonline.CoinOutAnterior = ManejoNulos.ManageNullDouble(dr["ValorJuego"]);
                                contadoresonline.Estado_Oln = ManejoNulos.ManageNullInteger(dr["CantidadCupones"]);
                                contadoresonline.Win = ManejoNulos.ManageNullDouble(dr["SaldoCupIni"]);
                                contadoresonline.CantidadCupones = ManejoNulos.ManageNullInteger(dr["SaldoCupFin"]);
                                contadoresonline.FechaRegistro = ManejoNulos.ManageNullDate(dr["SerieIni"]);
                                contadoresonline.CodCliente = ManejoNulos.ManageNullStr(dr["SerieFin"]);
                                contadoresonline.CoinOutIas = ManejoNulos.ManageNullDouble(dr["CoinOutIas"]);
                                contadoresonline.EstadoEnvio = ManejoNulos.ManageNullInteger(dr["Hora"]);
                                contadoresonline.CodSala = ManejoNulos.ManageNullStr(dr["CodSala"]);
                                contadoresonline.FechaLlegada = ManejoNulos.ManageNullDate(dr["FechaLlegada"]);
                                contadoresonline.DetalleCuponesImpresos_id = ManejoNulos.ManageNullInteger64(dr["DetalleCuponesImpresos_id"]);
                                contadoresonline.HandPay = ManejoNulos.ManageNullDouble(dr["HandPay"]);
                                contadoresonline.JackPot= ManejoNulos.ManageNullDouble(dr["JackPot"]);
                                contadoresonline.HandPayAnterior= ManejoNulos.ManageNullDouble(dr["HandPayAnterior"]);
                                contadoresonline.JackPotAnterior= ManejoNulos.ManageNullDouble(dr["JackPotAnterior"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return contadoresonline;
        }

        public Int64 GuardarCMP_ContadoresOnlineWebCupones(CMP_ContadoresOnlineWebCuponesEntidad cupon)
        {
            //bool respuesta = false;
            Int64 IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[CMP_Contadores_OnLine_Web_Cupones]
           ([Cod_Cont]
           ,[Cod_Cont_OL]
           ,[Fecha]
           ,[Hora]
           ,[CodMaq]
           ,[CodMaqMin]
           ,[CoinOut]
           ,[CurrentCredits]
           ,[Monto]
           ,[Token]
           ,[CoinOutAnterior]
           ,[Estado_Oln]
           ,[Win]
           ,[CantidadCupones]
           ,[FechaRegistro]
           ,[CodCliente]
           ,[CoinOutIas]
           ,[CodSala]
           ,[Estado_Envio]
           ,[FechaLlegada]
           ,[DetalleCuponesImpresos_id],[HandPay],[JackPot],[HandPayAnterior],[JackPotAnterior])
Output Inserted.id
     VALUES
           (@Cod_Cont
            ,@Cod_Cont_OL
           ,@Fecha
           ,@Hora
           ,@CodMaq
           ,@CodMaqMin
           ,@CoinOut
           ,@CurrentCredits
           ,@Monto
           ,@Token
           ,@CoinOutAnterior
           ,@Estado_Oln
           ,@Win
           ,@CantidadCupones
           ,@FechaRegistro
           ,@CodCliente
           ,@CoinOutIas
           ,@CodSala
           ,@EstadoEnvio
            ,@FechaLlegada
            ,@DetalleCuponesImpresos_id,@HandPay,@JackPot,@HandPayAnterior,@JackPotAnterior)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Cod_Cont", ManejoNulos.ManageNullInteger64(cupon.Cod_Cont));
                    query.Parameters.AddWithValue("@Cod_Cont_OL", ManejoNulos.ManageNullInteger64(cupon.Cod_Cont_OL));
                    query.Parameters.AddWithValue("@Fecha", ManejoNulos.ManageNullDate(cupon.Fecha));
                    query.Parameters.AddWithValue("@Hora", ManejoNulos.ManageNullDate(cupon.Hora));
                    query.Parameters.AddWithValue("@CodMaq", ManejoNulos.ManageNullStr(cupon.CodMaq.Trim()));
                    query.Parameters.AddWithValue("@CodMaqMin", ManejoNulos.ManageNullStr(cupon.CodMaqMin));
                    query.Parameters.AddWithValue("@CoinOut", ManejoNulos.ManageNullDouble(cupon.CoinOut));
                    query.Parameters.AddWithValue("@CurrentCredits", ManejoNulos.ManageNullDouble(cupon.CurrentCredits));
                    query.Parameters.AddWithValue("@Monto", ManejoNulos.ManageNullDouble(cupon.Monto));
                    query.Parameters.AddWithValue("@Token", ManejoNulos.ManageNullDouble(cupon.Token));
                    query.Parameters.AddWithValue("@CoinOutAnterior", ManejoNulos.ManageNullDouble(cupon.CoinOutAnterior));
                    query.Parameters.AddWithValue("@Estado_Oln", ManejoNulos.ManageNullInteger(cupon.Estado_Oln));
                    query.Parameters.AddWithValue("@Win", ManejoNulos.ManageNullDouble(cupon.Win));
                    query.Parameters.AddWithValue("@CantidadCupones", ManejoNulos.ManageNullDouble(cupon.CantidadCupones));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(cupon.FechaRegistro));
                    query.Parameters.AddWithValue("@CodCliente", ManejoNulos.ManageNullStr(cupon.CodCliente));
                    query.Parameters.AddWithValue("@CoinOutIas", ManejoNulos.ManageNullDouble(cupon.CoinOutIas));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullStr(cupon.CodSala));
                    query.Parameters.AddWithValue("@EstadoEnvio", ManejoNulos.ManageNullInteger(cupon.EstadoEnvio));
                    query.Parameters.AddWithValue("@FechaLlegada", ManejoNulos.ManageNullDate(cupon.FechaLlegada));
                    query.Parameters.AddWithValue("@DetalleCuponesImpresos_id", ManejoNulos.ManageNullInteger64(cupon.DetalleCuponesImpresos_id));
                    query.Parameters.AddWithValue("@HandPay", ManejoNulos.ManageNullDouble(cupon.HandPay));
                    query.Parameters.AddWithValue("@JackPot", ManejoNulos.ManageNullDouble(cupon.JackPot));
                    query.Parameters.AddWithValue("@HandPayAnterior", ManejoNulos.ManageNullDouble(cupon.HandPayAnterior));
                    query.Parameters.AddWithValue("@JackPotAnterior", ManejoNulos.ManageNullDouble(cupon.JackPotAnterior));
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
    }
}
