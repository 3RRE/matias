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
    public class CMP_SesionSorteoSalaMigracionDAL
    {
        private readonly string _conexion = string.Empty;
        public CMP_SesionSorteoSalaMigracionDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public int GuardarSesionSorteoSalaMigracion(CMP_SesionSorteoSalaMigracion item)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
               INSERT INTO [dbo].[CMP_SesionSorteoSalaMigracion]
           ([SesionId]
           ,[SorteoId]
           ,[JugadaId]
           ,[CantidadCupones]
           ,[FechaRegistro]
           ,[SerieIni]
           ,[SerieFin]
           ,[NombreSorteo]
           ,[CondicionWin]
           ,[WinCalculado]
           ,[CondicionBet]
           ,[BetCalculado]
           ,[TopeCuponesxJugada]
           ,[ParametrosImpresion]
           ,[Cod_Cont_OL]
           ,[Fecha]
           ,[Hora]
           ,[CodMaq]
           ,[CoinOut]
           ,[CurrentCredits]
           ,[Monto]
           ,[Token]
           ,[CoinOutAnterior]
           ,[Estado_Oln]
           ,[HandPay]
           ,[JackPot]
           ,[HandPayAnterior]
           ,[JackPotAnterior]
           ,[CoinIn]
           ,[CoinInAnterior],[SesionMigracionId])
output inserted.id
     VALUES
           (@SesionId
           ,@SorteoId
           ,@JugadaId
           ,@CantidadCupones
           ,@FechaRegistro
           ,@SerieIni
           ,@SerieFin
           ,@NombreSorteo
           ,@CondicionWin
           ,@WinCalculado
           ,@CondicionBet
           ,@BetCalculado
           ,@TopeCuponesxJugada
           ,@ParametrosImpresion
           ,@Cod_Cont_OL
           ,@Fecha
           ,@Hora
           ,@CodMaq
           ,@CoinOut
           ,@CurrentCredits
           ,@Monto
           ,@Token
           ,@CoinOutAnterior
           ,@Estado_Oln
           ,@HandPay
           ,@JackPot
           ,@HandPayAnterior
           ,@JackPotAnterior
           ,@CoinIn
           ,@CoinInAnterior,@SesionMigracionId)
                      ";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@SesionId", ManejoNulos.ManageNullInteger64(item.SesionId));
                    query.Parameters.AddWithValue("@SorteoId", ManejoNulos.ManageNullInteger64(item.SorteoId));
                    query.Parameters.AddWithValue("@JugadaId", ManejoNulos.ManageNullInteger64(item.JugadaId));
                    query.Parameters.AddWithValue("@CantidadCupones", ManejoNulos.ManageNullInteger(item.CantidadCupones));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(item.FechaRegistro));
                    query.Parameters.AddWithValue("@SerieIni", ManejoNulos.ManageNullStr(item.SerieIni));
                    query.Parameters.AddWithValue("@SerieFin", ManejoNulos.ManageNullStr(item.SerieFin));
                    query.Parameters.AddWithValue("@NombreSorteo", ManejoNulos.ManageNullStr(item.NombreSorteo));
                    query.Parameters.AddWithValue("@CondicionWin", ManejoNulos.ManageNullDecimal(item.CondicionWin));
                    query.Parameters.AddWithValue("@WinCalculado", ManejoNulos.ManageNullDecimal(item.WinCalculado));
                    query.Parameters.AddWithValue("@CondicionBet", ManejoNulos.ManageNullDecimal(item.CondicionBet));
                    query.Parameters.AddWithValue("@BetCalculado", ManejoNulos.ManageNullDecimal(item.BetCalculado));
                    query.Parameters.AddWithValue("@TopeCuponesxJugada", ManejoNulos.ManageNullInteger(item.TopeCuponesxJugada));
                    query.Parameters.AddWithValue("@ParametrosImpresion", ManejoNulos.ManageNullStr(item.ParametrosImpresion));
                    query.Parameters.AddWithValue("@Cod_Cont_OL", ManejoNulos.ManageNullInteger64(item.Cod_Cont_OL));
                    query.Parameters.AddWithValue("@Fecha", ManejoNulos.ManageNullDate(item.Fecha));
                    query.Parameters.AddWithValue("@Hora", ManejoNulos.ManageNullDate(item.Hora));
                    query.Parameters.AddWithValue("@CodMaq", ManejoNulos.ManageNullStr(item.CodMaq));
                    query.Parameters.AddWithValue("@CoinOut", ManejoNulos.ManageNullDouble(item.CoinOut));
                    query.Parameters.AddWithValue("@CurrentCredits", ManejoNulos.ManageNullDouble(item.CurrentCredits));
                    query.Parameters.AddWithValue("@Monto", ManejoNulos.ManageNullDecimal(item.Monto));
                    query.Parameters.AddWithValue("@Token", ManejoNulos.ManageNullDecimal(item.Token));
                    query.Parameters.AddWithValue("@CoinOutAnterior", ManejoNulos.ManageNullDouble(item.CoinOutAnterior));
                    query.Parameters.AddWithValue("@Estado_Oln", ManejoNulos.ManageNullInteger(item.Estado_Oln));
                    query.Parameters.AddWithValue("@HandPay", ManejoNulos.ManageNullDouble(item.HandPay));
                    query.Parameters.AddWithValue("@JackPot", ManejoNulos.ManageNullDouble(item.JackPot));
                    query.Parameters.AddWithValue("@HandPayAnterior", ManejoNulos.ManageNullDouble(item.HandPayAnterior));
                    query.Parameters.AddWithValue("@JackPotAnterior", ManejoNulos.ManageNullDouble(item.JackPotAnterior));
                    query.Parameters.AddWithValue("@CoinIn", ManejoNulos.ManageNullDecimal(item.CoinIn));
                    query.Parameters.AddWithValue("@CoinInAnterior", ManejoNulos.ManageNullDecimal(item.CoinInAnterior));
                    query.Parameters.AddWithValue("@SesionMigracionId", ManejoNulos.ManageNullInteger64(item.SesionMigracionId));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public List<CMP_SesionSorteoSalaMigracion> ListarSesionSorteoSalaMigracion(string queryConsulta)
        {
            List<CMP_SesionSorteoSalaMigracion> lista = new List<CMP_SesionSorteoSalaMigracion>();
            string consulta = $@"  select
                  [SesionId]
      ,[SorteoId]
      ,[JugadaId]
      ,[CantidadCupones]
      ,[FechaRegistro]
      ,[SerieIni]
      ,[SerieFin]
      ,[NombreSorteo]
      ,[CondicionWin]
      ,[WinCalculado]
      ,[CondicionBet]
      ,[BetCalculado]
      ,[TopeCuponesxJugada]
      ,[ParametrosImpresion]
      ,[Cod_Cont_OL]
      ,[Fecha]
      ,[Hora]
      ,[CodMaq]
      ,[CoinOut]
      ,[CurrentCredits]
      ,[Monto]
      ,[Token]
      ,[CoinOutAnterior]
      ,[Estado_Oln]
      ,[HandPay]
      ,[JackPot]
      ,[HandPayAnterior]
      ,[JackPotAnterior]
      ,[CoinIn]
      ,[CoinInAnterior]
      ,[Id]
      ,[SesionMigracionId]
             FROM [dbo].[CMP_SesionSorteoSalaMigracion]
   " + queryConsulta;
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
                            var item = new CMP_SesionSorteoSalaMigracion
                            {
                                SesionId = ManejoNulos.ManageNullInteger64(dr["SesionId"]),
                                SorteoId = ManejoNulos.ManageNullInteger64(dr["SorteoId"]),
                                JugadaId = ManejoNulos.ManageNullInteger64(dr["JugadaId"]),
                                CantidadCupones = ManejoNulos.ManageNullInteger(dr["CantidadCupones"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                SerieIni = ManejoNulos.ManageNullStr(dr["SerieIni"]),
                                SerieFin = ManejoNulos.ManageNullStr(dr["SerieFin"]),
                                NombreSorteo = ManejoNulos.ManageNullStr(dr["NombreSorteo"]),
                                CondicionWin = ManejoNulos.ManageNullDecimal(dr["CondicionWin"]),
                                CondicionBet = ManejoNulos.ManageNullDecimal(dr["CondicionBet"]),
                                TopeCuponesxJugada = ManejoNulos.ManageNullInteger(dr["TopeCuponesxJugada"]),
                                WinCalculado = ManejoNulos.ManageNullDecimal(dr["WinCalculado"]),
                                BetCalculado = ManejoNulos.ManageNullDecimal(dr["BetCalculado"]),
                                ParametrosImpresion = ManejoNulos.ManageNullStr(dr["ParametrosImpresion"]),
                                Cod_Cont_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Cont_OL"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Hora = ManejoNulos.ManageNullDate(dr["Hora"]),
                                CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]),
                                CoinOut = ManejoNulos.ManageNullDouble(dr["CoinOut"]),
                                CurrentCredits = ManejoNulos.ManageNullDouble(dr["CurrentCredits"]),
                                Monto = ManejoNulos.ManageNullDecimal(dr["Monto"]),
                                Token = ManejoNulos.ManageNullDecimal(dr["Token"]),
                                CoinOutAnterior = ManejoNulos.ManageNullDouble(dr["CoinOutAnterior"]),
                                Estado_Oln = ManejoNulos.ManageNullInteger(dr["Estado_Oln"]),
                                HandPay = ManejoNulos.ManageNullDouble(dr["HandPay"]),
                                JackPot = ManejoNulos.ManageNullDouble(dr["JackPot"]),
                                HandPayAnterior = ManejoNulos.ManageNullDouble(dr["HandPayAnterior"]),
                                JackPotAnterior = ManejoNulos.ManageNullDouble(dr["JackPotAnterior"]),
                                CoinIn = ManejoNulos.ManageNullDecimal(dr["CoinIn"]),
                                CoinInAnterior = ManejoNulos.ManageNullDecimal(dr["CoinInAnterior"]),
                                Id = ManejoNulos.ManageNullInteger64(dr["Id"]),
                                SesionMigracionId = ManejoNulos.ManageNullInteger64(dr["SesionMigracionId"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<CMP_SesionSorteoSalaMigracion>();
            }
            return lista;
        }
        public List<CMP_JugadasCliente> ListarJugadasTableau(string queryConsulta)
        {
            List<CMP_JugadasCliente> lista = new List<CMP_JugadasCliente>();
            string consulta = $@" 
SELECT 
	  sesion.[Id]
      ,sesion.[SesionId]
      ,sesion.[CodMaquina]
      ,sesion.[FechaInicio]
      ,sesion.[FechaTermino]
      ,sesion.[ClienteIdIas]
      ,sesion.[NombreCliente]
      ,sesion.[NroDocumento]
      ,sesion.[UsuarioIas]
      ,sesion.[Terminado]
      ,sesion.[MotivoTermino]
      ,sesion.[UsuarioTermino]
      ,sesion.[EstadoEnvio]
      ,sesion.[Mail]
      ,sesion.[TipoDocumentoId]
      ,sesion.[NombreTipoDocumento]
      ,sesion.[TipoSesion]
      ,sesion.[NombreTipoSesion]
      ,sesion.[CodSala]
      ,sesion.[NombreSala]
      ,sesion.[FechaRegistro]
	  ,sortsala.[SesionId]
      ,sortsala.[SorteoId]
      ,sortsala.[JugadaId]
      ,sortsala.[CantidadCupones]
      ,sortsala.[FechaRegistro]
      ,sortsala.[SerieIni]
      ,sortsala.[SerieFin]
      ,sortsala.[NombreSorteo]
      ,sortsala.[CondicionWin]
      ,sortsala.[WinCalculado]
      ,sortsala.[CondicionBet]
      ,sortsala.[BetCalculado]
      ,sortsala.[TopeCuponesxJugada]
      ,sortsala.[ParametrosImpresion]
      ,sortsala.[Cod_Cont_OL]
      ,sortsala.[Fecha]
      ,sortsala.[Hora]
      ,sortsala.[CodMaq]
      ,sortsala.[CoinOut]
      ,sortsala.[CurrentCredits]
      ,sortsala.[Monto]
      ,sortsala.[Token]
      ,sortsala.[CoinOutAnterior]
      ,sortsala.[Estado_Oln]
      ,sortsala.[HandPay]
      ,sortsala.[JackPot]
      ,sortsala.[HandPayAnterior]
      ,sortsala.[JackPotAnterior]
      ,sortsala.[CoinIn]
      ,sortsala.[CoinInAnterior]
      ,sortsala.[Id]
      ,sortsala.[SesionMigracionId]

  FROM [dbo].[CMP_SesionSorteoSalaMigracion] as sortsala
  join [dbo].[CMP_SesionMigracion] as sesion
  on sesion.Id=sortsala.SesionMigracionId
   " + queryConsulta;
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
                            var item = new CMP_JugadasCliente
                            {
                                Id = ManejoNulos.ManageNullInteger64(dr["Id"]),
                                SesionId = ManejoNulos.ManageNullInteger64(dr["SesionId"]),
                                CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                                FechaInicio = ManejoNulos.ManageNullDate(dr["FechaInicio"]),
                                FechaTermino = ManejoNulos.ManageNullDate(dr["FechaTermino"]),
                                NombreCliente = ManejoNulos.ManageNullStr(dr["NombreCliente"]),
                                NroDocumento = ManejoNulos.ManageNullStr(dr["NroDocumento"]),
                                Terminado = ManejoNulos.ManageNullInteger(dr["Terminado"]),
                                MotivoTermino = ManejoNulos.ManageNullStr(dr["MotivoTermino"]),
                                UsuarioTermino = ManejoNulos.ManageNullInteger(dr["UsuarioTermino"]),
                                EstadoEnvio = ManejoNulos.ManageNullInteger(dr["EstadoEnvio"]),
                                Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                NombreTipoDocumento = ManejoNulos.ManageNullStr(dr["NombreTipoDocumento"]),
                                TipoSesion = ManejoNulos.ManageNullInteger(dr["TipoSesion"]),
                                NombreTipoSesion = ManejoNulos.ManageNullStr(dr["NombreTipoSesion"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),

                                SorteoId = ManejoNulos.ManageNullInteger64(dr["SorteoId"]),
                                JugadaId = ManejoNulos.ManageNullInteger64(dr["JugadaId"]),
                                CantidadCupones = ManejoNulos.ManageNullInteger(dr["CantidadCupones"]),
                                SerieIni = ManejoNulos.ManageNullStr(dr["SerieIni"]),
                                SerieFin = ManejoNulos.ManageNullStr(dr["SerieFin"]),
                                NombreSorteo = ManejoNulos.ManageNullStr(dr["NombreSorteo"]),
                                CondicionWin = ManejoNulos.ManageNullDecimal(dr["CondicionWin"]),
                                WinCalculado = ManejoNulos.ManageNullDecimal(dr["WinCalculado"]),
                                CondicionBet = ManejoNulos.ManageNullDecimal(dr["CondicionBet"]),
                                BetCalculado = ManejoNulos.ManageNullDecimal(dr["BetCalculado"]),
                                TopeCuponesxJugada = ManejoNulos.ManageNullInteger(dr["TopeCuponesxJugada"]),
                                ParametrosImpresion = ManejoNulos.ManageNullStr(dr["ParametrosImpresion"]),
                                Cod_Cont_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Cont_OL"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Hora = ManejoNulos.ManageNullDate(dr["Hora"]),
                                CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]),
                                CoinOut = ManejoNulos.ManageNullDouble(dr["CoinOut"]),
                                CurrentCredits = ManejoNulos.ManageNullDouble(dr["CurrentCredits"]),
                                Monto = ManejoNulos.ManageNullDecimal(dr["Monto"]),
                                Token = ManejoNulos.ManageNullDecimal(dr["Token"]),
                                CoinOutAnterior = ManejoNulos.ManageNullDouble(dr["CoinOutAnterior"]),
                                Estado_Oln = ManejoNulos.ManageNullInteger(dr["Estado_Oln"]),
                                HandPay = ManejoNulos.ManageNullDouble(dr["HandPay"]),
                                JackPot = ManejoNulos.ManageNullDouble(dr["JackPot"]),
                                HandPayAnterior = ManejoNulos.ManageNullDouble(dr["HandPayAnterior"]),
                                JackPotAnterior = ManejoNulos.ManageNullDouble(dr["JackPotAnterior"]),
                                CoinIn = ManejoNulos.ManageNullDecimal(dr["CoinIn"]),
                                CoinInAnterior = ManejoNulos.ManageNullDecimal(dr["CoinInAnterior"]),
                            };
                            lista.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<CMP_JugadasCliente>();
            }
            return lista;
        }
    }

}
