using CapaEntidad.Administrativo;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Administrativo
{
    public class WS_DetalleContadoresGameWebDAL
    {
        string _conexion = string.Empty;

        public WS_DetalleContadoresGameWebDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<WS_DetalleContadoresGameWebEntidad> GetListadoWS_DetalleContadoresGamePorFechaOperacion(int CodSala,DateTime FechaOperacion)
        {
            List<WS_DetalleContadoresGameWebEntidad> lista = new List<WS_DetalleContadoresGameWebEntidad>();
            string consulta = @"SELECT 
        [CodDetalleContadoresGame]
      ,[CodMaquina]
      ,[CodSala]
      ,[CodEmpresa]
      ,[CodMoneda]
      ,[FechaOperacion]
      ,[CoinIn]
      ,[CoinOut]
      ,[Jackpot]
      ,[HandPay]
      ,[CancelCredit]
      ,[GamesPlayed]
      ,[TipoCambio],[Hora]
  FROM [dbo].[WS_DetalleContadoresGameWeb] where Convert(date,[FechaOperacion])=Convert(date,@FechaOperacion) and [CodSala]=@CodSala";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@FechaOperacion", FechaOperacion);
                    query.Parameters.AddWithValue("@CodSala", CodSala);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var contador = new WS_DetalleContadoresGameWebEntidad
                            {
                                CodDetalleContadoresGame = ManejoNulos.ManageNullInteger(dr["CodDetalleContadoresGame"]),
                                CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                CodMoneda = ManejoNulos.ManageNullInteger(dr["CodMoneda"]),
                                FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"]),
                                CoinIn = ManejoNulos.ManageNullDecimal(dr["CoinIn"]),
                                CoinOut = ManejoNulos.ManageNullDecimal(dr["CoinOut"]),
                                Jackpot = ManejoNulos.ManageNullDecimal(dr["JackPot"]),
                                HandPay = ManejoNulos.ManageNullDecimal(dr["HandPay"]),
                                CancelCredit = ManejoNulos.ManageNullDecimal(dr["CancelCredit"]),
                                GamesPlayed = ManejoNulos.ManageNullDecimal(dr["GamesPlayed"]),
                                TipoCambio = ManejoNulos.ManageNullDecimal(dr["TipoCambio"]),
                                Hora = ManejoNulos.ManageNullDate(dr["Hora"]),
                             
                            };
                            lista.Add(contador);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return new List<WS_DetalleContadoresGameWebEntidad>();
            }
            return lista;
        }
        public int GuardarWS_DetalleContadoresGameWeb(WS_DetalleContadoresGameWebEntidad contador)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[WS_DetalleContadoresGameWeb]
           ([CodMaquina]
           ,[CodSala]
           ,[CodEmpresa]
           ,[CodMoneda]
           ,[FechaOperacion]
           ,[CoinIn]
           ,[CoinOut]
           ,[Jackpot]
           ,[HandPay]
           ,[CancelCredit]
           ,[GamesPlayed]
           ,[TipoCambio],[Hora])
Output Inserted.CodDetalleContadoresGame
     VALUES
           (@CodMaquina
           ,@CodSala
           ,@CodEmpresa
           ,@CodMoneda
           ,@FechaOperacion
           ,@CoinIn
           ,@CoinOut
           ,@Jackpot
           ,@HandPay
           ,@CancelCredit
           ,@GamesPlayed
           ,@TipoCambio,@Hora)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullStr(contador.CodMaquina));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(contador.CodSala));
                    query.Parameters.AddWithValue("@CodEmpresa", ManejoNulos.ManageNullInteger(contador.CodEmpresa));
                    query.Parameters.AddWithValue("@CodMoneda", ManejoNulos.ManageNullInteger(contador.CodMoneda));
                    query.Parameters.AddWithValue("@FechaOperacion", ManejoNulos.ManageNullDate(contador.FechaOperacion));
                    query.Parameters.AddWithValue("@CoinIn", ManejoNulos.ManageNullDecimal(contador.CoinIn));
                    query.Parameters.AddWithValue("@CoinOut", ManejoNulos.ManageNullDecimal(contador.CoinOut));
                    query.Parameters.AddWithValue("@JackPot", ManejoNulos.ManageNullDecimal(contador.Jackpot));
                    query.Parameters.AddWithValue("@HandPay", ManejoNulos.ManageNullDecimal(contador.HandPay));
                    query.Parameters.AddWithValue("@CancelCredit", ManejoNulos.ManageNullDecimal(contador.CancelCredit));
                    query.Parameters.AddWithValue("@GamesPlayed", ManejoNulos.ManageNullDecimal(contador.GamesPlayed));
                    query.Parameters.AddWithValue("@TipoCambio", ManejoNulos.ManageNullInteger(contador.TipoCambio));
                    query.Parameters.AddWithValue("@Hora", ManejoNulos.ManageNullDate(contador.Hora));
                
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            }
            catch (Exception ex)
            {
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EliminarWS_DetalleContadoresGameWebPorFecha(int CodSala,DateTime fechaOperacion)
        {
            bool respuesta = false;
            string consulta = @"
            DECLARE @FechaOperacionDate date
            SET @FechaOperacionDate = CONVERT(date, @FechaOperacion)

            DELETE FROM [dbo].[WS_DetalleContadoresGameWeb]
            WHERE [FechaOperacion] >= @FechaOperacionDate
              AND [FechaOperacion] < DATEADD(day, 1, @FechaOperacionDate) and CodSala=@CodSala
           ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@FechaOperacion", ManejoNulos.ManageNullDate(fechaOperacion));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(CodSala));

                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return respuesta;
        }


        public WS_DetalleContadoresGameWebEntidad GetWS_DetalleContadoresGamePorHoraYCodMaq(int CodSala,string CodMaq, DateTime Hora) {
            WS_DetalleContadoresGameWebEntidad detalleContador = new WS_DetalleContadoresGameWebEntidad();
            string consulta = @"SELECT 
        [CodDetalleContadoresGame]
      ,[CodMaquina]
      ,[CodSala]
      ,[CodEmpresa]
      ,[CodMoneda]
      ,[FechaOperacion]
      ,[CoinIn]
      ,[CoinOut]
      ,[Jackpot]
      ,[HandPay]
      ,[CancelCredit]
      ,[GamesPlayed]
      ,[TipoCambio],[Hora]
  FROM [dbo].[WS_DetalleContadoresGameWeb] where Convert(date,[Hora])=Convert(date,@pHora) and [CodSala]=@pCodSala and [CodMaquina]=@pCodMaquina";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pHora", Hora);
                    query.Parameters.AddWithValue("@pCodSala", CodSala);
                    query.Parameters.AddWithValue("@pCodMaquina", CodMaq);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            detalleContador = new WS_DetalleContadoresGameWebEntidad {
                                CodDetalleContadoresGame = ManejoNulos.ManageNullInteger(dr["CodDetalleContadoresGame"]),
                                CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                CodMoneda = ManejoNulos.ManageNullInteger(dr["CodMoneda"]),
                                FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"]),
                                CoinIn = ManejoNulos.ManageNullDecimal(dr["CoinIn"]),
                                CoinOut = ManejoNulos.ManageNullDecimal(dr["CoinOut"]),
                                Jackpot = ManejoNulos.ManageNullDecimal(dr["JackPot"]),
                                HandPay = ManejoNulos.ManageNullDecimal(dr["HandPay"]),
                                CancelCredit = ManejoNulos.ManageNullDecimal(dr["CancelCredit"]),
                                GamesPlayed = ManejoNulos.ManageNullDecimal(dr["GamesPlayed"]),
                                TipoCambio = ManejoNulos.ManageNullDecimal(dr["TipoCambio"]),
                                Hora = ManejoNulos.ManageNullDate(dr["Hora"]),

                            };
                        }
                    }

                }
            } catch(Exception ex) {
                detalleContador = new WS_DetalleContadoresGameWebEntidad();
            }
            return detalleContador;
        }

		public DateTime GetHoraDetalleContadoresGameWeb(int CodSala, DateTime fecha)
		{

            DateTime fechaTomaContadores = fecha.Date.AddDays(1);

			string consulta = @"SELECT TOP 1 [Hora] 
  FROM [dbo].[WS_DetalleContadoresGameWeb] where Convert(date,[FechaOperacion])=Convert(date,@FechaOperacion) and [CodSala]=@CodSala ORDER BY Hora ASC ";
			try
			{
				using(var con = new SqlConnection(_conexion))
				{
					con.Open();
					var query = new SqlCommand(consulta, con);
					query.Parameters.AddWithValue("@FechaOperacion", fecha);
					query.Parameters.AddWithValue("@CodSala", CodSala);

					using(var dr = query.ExecuteReader())
					{
						while(dr.Read())
						{
                            fechaTomaContadores = ManejoNulos.ManageNullDate(dr["Hora"]);
						}
					}

				}
			} catch(Exception ex)
			{
				fechaTomaContadores = fecha.Date.AddDays(1);
			}
			return fechaTomaContadores;
		}

	}
}
