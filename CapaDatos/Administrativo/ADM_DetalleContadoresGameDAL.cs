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
    public class ADM_DetalleContadoresGameDAL
    {
        string _conexion = string.Empty;

        public ADM_DetalleContadoresGameDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ADM_DetalleContadoresGameEntidad> GetListado_DetalleContadoresGamePorFechaOperacion(int CodSala, DateTime FechaOperacion)
        {
            List<ADM_DetalleContadoresGameEntidad> lista = new List<ADM_DetalleContadoresGameEntidad>();
            string consulta = @"SELECT [CodDetalleContadoresGame]
      ,[CodContadoresGame]
      ,[CodMaquina]
      ,[CodSala]
      ,[CodEmpresa]
      ,[CodMoneda]
      ,[FechaOperacion]
      ,[CoinInIni]
      ,[CoinInFin]
      ,[CoinOutIni]
      ,[CoinOutFin]
      ,[JackpotIni]
      ,[JackpotFin]
      ,[HandPayIni]
      ,[HandPayFin]
      ,[CancelCreditIni]
      ,[CancelCreditFin]
      ,[GamesPlayedIni]
      ,[GamesPlayedFin]
      ,[ProduccionPorSlot1]
      ,[ProduccionPorSlot2Reset]
      ,[ProduccionPorSlot3Rollover]
      ,[ProduccionPorSlot4Prueba]
      ,[ProduccionTotalPorSlot5Dia]
      ,[TipoCambio]
      ,[FechaRegistro]
      ,[FechaModificacion]
      ,[Activo]
      ,[Estado]
      ,[SaldoCoinIn]
      ,[SaldoCoinOut]
      ,[SaldoJackpot]
      ,[SaldoGamesPlayed]
      ,[CodUsuario]
      ,[RetiroTemporal]
      ,[TiempoJuego]
  FROM [dbo].[ADM_DetalleContadoresGame] where Convert(date,[FechaOperacion])=Convert(date,@FechaOperacion) and [CodSala]=@CodSala";
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
                            var contador = new ADM_DetalleContadoresGameEntidad
                            {
                                CodDetalleContadoresGame = ManejoNulos.ManageNullInteger(dr["CodDetalleContadoresGame"]),
                                CodContadoresGame = ManejoNulos.ManageNullInteger(dr["CodContadoresGame"]),
                                CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                CodMoneda = ManejoNulos.ManageNullInteger(dr["CodMoneda"]),
                                FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"]),
                                CoinInIni = ManejoNulos.ManageNullDecimal(dr["CoinInIni"]),
                                CoinInFin = ManejoNulos.ManageNullDecimal(dr["CoinInFin"]),
                                CoinOutIni = ManejoNulos.ManageNullDecimal(dr["CoinOutIni"]),
                                CoinOutFin = ManejoNulos.ManageNullDecimal(dr["CoinOutFin"]),
                                JackpotIni = ManejoNulos.ManageNullDecimal(dr["JackpotIni"]),
                                JackpotFin = ManejoNulos.ManageNullDecimal(dr["JackpotFin"]),
                                HandPayIni = ManejoNulos.ManageNullDecimal(dr["HandPayIni"]),
                                HandPayFin = ManejoNulos.ManageNullDecimal(dr["HandPayFin"]),
                                CancelCreditIni = ManejoNulos.ManageNullDecimal(dr["CancelCreditIni"]),
                                CancelCreditFin = ManejoNulos.ManageNullDecimal(dr["CancelCreditFin"]),
                                GamesPlayedIni = ManejoNulos.ManageNullDecimal(dr["GamesPlayedIni"]),
                                GamesPlayedFin = ManejoNulos.ManageNullDecimal(dr["GamesPlayedFin"]),
                                ProduccionPorSlot1 = ManejoNulos.ManageNullDecimal(dr["ProduccionPorSlot1"]),
                                ProduccionPorSlot2Reset = ManejoNulos.ManageNullDecimal(dr["ProduccionPorSlot2Reset"]),
                                ProduccionPorSlot3Rollover = ManejoNulos.ManageNullDecimal(dr["ProduccionPorSlot3Rollover"]),
                                ProduccionPorSlot4Prueba = ManejoNulos.ManageNullDecimal(dr["ProduccionPorSlot4Prueba"]),
                                ProduccionTotalPorSlot5Dia = ManejoNulos.ManageNullDecimal(dr["ProduccionTotalPorSlot5Dia"]),
                                TipoCambio = ManejoNulos.ManageNullDecimal(dr["TipoCambio"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                SaldoCoinIn = ManejoNulos.ManageNullDecimal(dr["SaldoCoinIn"]),
                                SaldoCoinOut = ManejoNulos.ManageNullDecimal(dr["SaldoCoinOut"]),
                                SaldoJackpot = ManejoNulos.ManageNullDecimal(dr["SaldoJackpot"]),
                                SaldoGamesPlayed = ManejoNulos.ManageNullDecimal(dr["SaldoGamesPlayed"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                RetiroTemporal = ManejoNulos.ManageNullInteger(dr["RetiroTemporal"]),
                                TiempoJuego = ManejoNulos.ManageNullDecimal(dr["TiempoJuego"]),

                            };
                            lista.Add(contador);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return new List<ADM_DetalleContadoresGameEntidad>();
            }
            return lista;
        }
        public int Guardar_DetalleContadoresGame(ADM_DetalleContadoresGameEntidad contador)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[ADM_DetalleContadoresGame]
           ([CodContadoresGame]
           ,[CodMaquina]
           ,[CodSala]
           ,[CodEmpresa]
           ,[CodMoneda]
           ,[FechaOperacion]
           ,[CoinInIni]
           ,[CoinInFin]
           ,[CoinOutIni]
           ,[CoinOutFin]
           ,[JackpotIni]
           ,[JackpotFin]
           ,[HandPayIni]
           ,[HandPayFin]
           ,[CancelCreditIni]
           ,[CancelCreditFin]
           ,[GamesPlayedIni]
           ,[GamesPlayedFin]
           ,[ProduccionPorSlot1]
           ,[ProduccionPorSlot2Reset]
           ,[ProduccionPorSlot3Rollover]
           ,[ProduccionPorSlot4Prueba]
           ,[ProduccionTotalPorSlot5Dia]
           ,[TipoCambio]
           ,[FechaRegistro]
           ,[FechaModificacion]
           ,[Activo]
           ,[Estado]
           ,[SaldoCoinIn]
           ,[SaldoCoinOut]
           ,[SaldoJackpot]
           ,[SaldoGamesPlayed]
           ,[CodUsuario]
           ,[RetiroTemporal]
           ,[TiempoJuego])
Output Inserted.CodDetalleContadoresGame
     VALUES
           (@CodContadoresGame
           ,@CodMaquina
           ,@CodSala
           ,@CodEmpresa
           ,@CodMoneda
           ,@FechaOperacion
           ,@CoinInIni
           ,@CoinInFin
           ,@CoinOutIni
           ,@CoinOutFin
           ,@JackpotIni
           ,@JackpotFin
           ,@HandPayIni
           ,@HandPayFin
           ,@CancelCreditIni
           ,@CancelCreditFin
           ,@GamesPlayedIni
           ,@GamesPlayedFin
           ,@ProduccionPorSlot1
           ,@ProduccionPorSlot2Reset
           ,@ProduccionPorSlot3Rollover
           ,@ProduccionPorSlot4Prueba
           ,@ProduccionTotalPorSlot5Dia
           ,@TipoCambio
           ,@FechaRegistro
           ,@FechaModificacion
           ,@Activo
           ,@Estado
           ,@SaldoCoinIn
           ,@SaldoCoinOut
           ,@SaldoJackpot
           ,@SaldoGamesPlayed
           ,@CodUsuario
           ,@RetiroTemporal
           ,@TiempoJuego)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodContadoresGame", ManejoNulos.ManageNullInteger(contador.CodContadoresGame));
                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullInteger(contador.CodMaquina));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(contador.CodSala));
                    query.Parameters.AddWithValue("@CodEmpresa", ManejoNulos.ManageNullInteger(contador.CodEmpresa));
                    query.Parameters.AddWithValue("@CodMoneda", ManejoNulos.ManageNullInteger(contador.CodMoneda));
                    query.Parameters.AddWithValue("@FechaOperacion", ManejoNulos.ManageNullDate(contador.FechaOperacion));
                    query.Parameters.AddWithValue("@CoinInIni", ManejoNulos.ManageNullDecimal(contador.CoinInIni));
                    query.Parameters.AddWithValue("@CoinInFin", ManejoNulos.ManageNullDecimal(contador.CoinInFin));
                    query.Parameters.AddWithValue("@CoinOutIni", ManejoNulos.ManageNullDecimal(contador.CoinOutIni));
                    query.Parameters.AddWithValue("@CoinOutFin", ManejoNulos.ManageNullDecimal(contador.CoinOutFin));
                    query.Parameters.AddWithValue("@JackpotIni", ManejoNulos.ManageNullDecimal(contador.JackpotIni));
                    query.Parameters.AddWithValue("@JackpotFin", ManejoNulos.ManageNullDecimal(contador.JackpotFin));
                    query.Parameters.AddWithValue("@HandPayIni", ManejoNulos.ManageNullDecimal(contador.HandPayIni));
                    query.Parameters.AddWithValue("@HandPayFin", ManejoNulos.ManageNullDecimal(contador.HandPayFin));
                    query.Parameters.AddWithValue("@CancelCreditIni", ManejoNulos.ManageNullDecimal(contador.CancelCreditIni));
                    query.Parameters.AddWithValue("@CancelCreditFin", ManejoNulos.ManageNullDecimal(contador.CancelCreditFin));
                    query.Parameters.AddWithValue("@GamesPlayedIni", ManejoNulos.ManageNullDecimal(contador.GamesPlayedIni));
                    query.Parameters.AddWithValue("@GamesPlayedFin", ManejoNulos.ManageNullDecimal(contador.GamesPlayedFin));
                    query.Parameters.AddWithValue("@ProduccionPorSlot1", ManejoNulos.ManageNullDecimal(contador.ProduccionPorSlot1));
                    query.Parameters.AddWithValue("@ProduccionPorSlot2Reset", ManejoNulos.ManageNullDecimal(contador.ProduccionPorSlot2Reset));
                    query.Parameters.AddWithValue("@ProduccionPorSlot3Rollover", ManejoNulos.ManageNullDecimal(contador.ProduccionPorSlot3Rollover));
                    query.Parameters.AddWithValue("@ProduccionPorSlot4Prueba", ManejoNulos.ManageNullDecimal(contador.ProduccionPorSlot4Prueba));
                    query.Parameters.AddWithValue("@ProduccionTotalPorSlot5Dia", ManejoNulos.ManageNullDecimal(contador.ProduccionTotalPorSlot5Dia));
                    query.Parameters.AddWithValue("@TipoCambio", ManejoNulos.ManageNullDecimal(contador.TipoCambio));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(contador.FechaRegistro));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(contador.FechaModificacion));
                    query.Parameters.AddWithValue("@Activo", ManejoNulos.ManegeNullBool(contador.Activo));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(contador.Estado));
                    query.Parameters.AddWithValue("@SaldoCoinIn", ManejoNulos.ManageNullDecimal(contador.SaldoCoinIn));
                    query.Parameters.AddWithValue("@SaldoCoinOut", ManejoNulos.ManageNullDecimal(contador.SaldoCoinOut));
                    query.Parameters.AddWithValue("@SaldoJackpot", ManejoNulos.ManageNullDecimal(contador.SaldoJackpot));
                    query.Parameters.AddWithValue("@SaldoGamesPlayed", ManejoNulos.ManageNullDecimal(contador.SaldoGamesPlayed));
                    query.Parameters.AddWithValue("@CodUsuario", ManejoNulos.ManageNullStr(contador.CodUsuario));
                    query.Parameters.AddWithValue("@RetiroTemporal", ManejoNulos.ManageNullInteger(contador.RetiroTemporal));
                    query.Parameters.AddWithValue("@TiempoJuego", ManejoNulos.ManageNullDecimal(contador.TiempoJuego));

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
        public List<ADM_DetalleContadoresGameEntidad> GetListado_DetalleContadoresGamePorQuery(string whereQuery, IDictionary<string, DateTime> fechaParametros)
        {
            List<ADM_DetalleContadoresGameEntidad> lista = new List<ADM_DetalleContadoresGameEntidad>();
            string consulta = $@"SELECT [CodDetalleContadoresGame]
      ,[CodContadoresGame]
      ,[CodMaquina]
      ,[CodSala]
      ,[CodEmpresa]
      ,[CodMoneda]
      ,[FechaOperacion]
      ,[CoinInIni]
      ,[CoinInFin]
      ,[CoinOutIni]
      ,[CoinOutFin]
      ,[JackpotIni]
      ,[JackpotFin]
      ,[HandPayIni]
      ,[HandPayFin]
      ,[CancelCreditIni]
      ,[CancelCreditFin]
      ,[GamesPlayedIni]
      ,[GamesPlayedFin]
      ,[ProduccionPorSlot1]
      ,[ProduccionPorSlot2Reset]
      ,[ProduccionPorSlot3Rollover]
      ,[ProduccionPorSlot4Prueba]
      ,[ProduccionTotalPorSlot5Dia]
      ,[TipoCambio]
      ,[FechaRegistro]
      ,[FechaModificacion]
      ,[Activo]
      ,[Estado]
      ,[SaldoCoinIn]
      ,[SaldoCoinOut]
      ,[SaldoJackpot]
      ,[SaldoGamesPlayed]
      ,[CodUsuario]
      ,[RetiroTemporal]
      ,[TiempoJuego]
  FROM [dbo].[ADM_DetalleContadoresGame] {whereQuery}";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    foreach (var parametro in fechaParametros)
                    {
                        query.Parameters.AddWithValue(parametro.Key, parametro.Value);
                    }

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var contador = new ADM_DetalleContadoresGameEntidad
                            {
                                CodDetalleContadoresGame = ManejoNulos.ManageNullInteger(dr["CodDetalleContadoresGame"]),
                                CodContadoresGame = ManejoNulos.ManageNullInteger(dr["CodContadoresGame"]),
                                CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                CodMoneda = ManejoNulos.ManageNullInteger(dr["CodMoneda"]),
                                FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"]),
                                CoinInIni = ManejoNulos.ManageNullDecimal(dr["CoinInIni"]),
                                CoinInFin = ManejoNulos.ManageNullDecimal(dr["CoinInFin"]),
                                CoinOutIni = ManejoNulos.ManageNullDecimal(dr["CoinOutIni"]),
                                CoinOutFin = ManejoNulos.ManageNullDecimal(dr["CoinOutFin"]),
                                JackpotIni = ManejoNulos.ManageNullDecimal(dr["JackpotIni"]),
                                JackpotFin = ManejoNulos.ManageNullDecimal(dr["JackpotFin"]),
                                HandPayIni = ManejoNulos.ManageNullDecimal(dr["HandPayIni"]),
                                HandPayFin = ManejoNulos.ManageNullDecimal(dr["HandPayFin"]),
                                CancelCreditIni = ManejoNulos.ManageNullDecimal(dr["CancelCreditIni"]),
                                CancelCreditFin = ManejoNulos.ManageNullDecimal(dr["CancelCreditFin"]),
                                GamesPlayedIni = ManejoNulos.ManageNullDecimal(dr["GamesPlayedIni"]),
                                GamesPlayedFin = ManejoNulos.ManageNullDecimal(dr["GamesPlayedFin"]),
                                ProduccionPorSlot1 = ManejoNulos.ManageNullDecimal(dr["ProduccionPorSlot1"]),
                                ProduccionPorSlot2Reset = ManejoNulos.ManageNullDecimal(dr["ProduccionPorSlot2Reset"]),
                                ProduccionPorSlot3Rollover = ManejoNulos.ManageNullDecimal(dr["ProduccionPorSlot3Rollover"]),
                                ProduccionPorSlot4Prueba = ManejoNulos.ManageNullDecimal(dr["ProduccionPorSlot4Prueba"]),
                                ProduccionTotalPorSlot5Dia = ManejoNulos.ManageNullDecimal(dr["ProduccionTotalPorSlot5Dia"]),
                                TipoCambio = ManejoNulos.ManageNullDecimal(dr["TipoCambio"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                SaldoCoinIn = ManejoNulos.ManageNullDecimal(dr["SaldoCoinIn"]),
                                SaldoCoinOut = ManejoNulos.ManageNullDecimal(dr["SaldoCoinOut"]),
                                SaldoJackpot = ManejoNulos.ManageNullDecimal(dr["SaldoJackpot"]),
                                SaldoGamesPlayed = ManejoNulos.ManageNullDecimal(dr["SaldoGamesPlayed"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                RetiroTemporal = ManejoNulos.ManageNullInteger(dr["RetiroTemporal"]),
                                TiempoJuego = ManejoNulos.ManageNullDecimal(dr["TiempoJuego"]),

                            };
                            lista.Add(contador);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return new List<ADM_DetalleContadoresGameEntidad>();
            }
            return lista;
        }
        public bool Eliminar_DetalleContadoresGamePorFecha(int CodSala, DateTime fechaOperacion)
        {
            bool respuesta = false;
            string consulta = @"
            DECLARE @FechaOperacionDate date
            SET @FechaOperacionDate = CONVERT(date, @FechaOperacion)

            DELETE FROM [dbo].[ADM_DetalleContadoresGame]
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
        public bool EditarDetalleContadoresGamePorMaquina(ADM_DetalleContadoresGameEntidad item) {
            bool respuesta = false;
            string consulta = @"
            update dbo.ADM_DetalleContadoresGame set CoinInIni=@CoinInIni,CoinInFin=@CoinInFin,SaldoCoinIn=@SaldoCoinIn,FechaModificacion=getdate()
where CodDetalleContadoresGame=@CodDetalleContadoresGame
           ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CoinInIni", ManejoNulos.ManageNullDecimal(item.CoinInIni));
                    query.Parameters.AddWithValue("@CoinInFin", ManejoNulos.ManageNullDecimal(item.CoinInFin));
                    query.Parameters.AddWithValue("@SaldoCoinIn", ManejoNulos.ManageNullDecimal(item.SaldoCoinIn));
                    query.Parameters.AddWithValue("@CodDetalleContadoresGame", ManejoNulos.ManageNullInteger(item.CodDetalleContadoresGame));

                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                respuesta = false;
            }
            return respuesta;
        }
    }
}
