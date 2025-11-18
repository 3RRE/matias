using CapaEntidad.ContadoresBonusIn;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ContadoresBonusIn {
    public class ContadoresBonusInDAL {
        string _conexion = string.Empty;

        public ContadoresBonusInDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public ContadoresBonusInCompleto ObtenerUltimoContadorBonusIn(int codSala)  {
            ContadoresBonusInCompleto contadorBonusIn = new ContadoresBonusInCompleto();
            //string consulta = @"SELECT TOP (1) [Cod_ContadoresBonusIn]
            //                      ,[Cod_Cont_OL]
            //                      ,[Fecha]
            //                      ,[Hora]
            //                      ,[CodMaq]
            //                      ,[CodMaqMin]
            //                      ,[CodTarjeta]
            //                      ,[CoinIn]
            //                      ,[CoinOut]
            //                      ,[HandPay]
            //                      ,[CurrentCredits]
            //                      ,[Monto]
            //                      ,[EftIn]
            //                      ,[EftOut]
            //                      ,[CancelCredits]
            //                      ,[Jackpot]
            //                      ,[GamesPlayed]
            //                      ,[TrueIn]
            //                      ,[TrueOut]
            //                      ,[TotalDrop]
            //                      ,[Bill]
            //                      ,[Bill1]
            //                      ,[Bill2]
            //                      ,[Bill5]
            //                      ,[Bill10]
            //                      ,[Bill20]
            //                      ,[Bill50]
            //                      ,[Bill100]
            //                      ,[NroTiket]
            //                      ,[TicketIn]
            //                      ,[TicketOut]
            //                      ,[TicketBonusIn]
            //                      ,[TicketBonusOut]
            //                      ,[MontoTiket]
            //                      ,[Progresivo]
            //                      ,[Enviado]
            //                      ,[codevento]
            //                      ,[codcli]
            //                      ,[codsuper]
            //                      ,[CodPer]
            //                      ,[CodAtencion]
            //                      ,[CodCuadre]
            //                      ,[PreCredito]
            //                      ,[Token]
            //                      ,[crc]
            //                      ,[PorD]
            //                      ,[Tficha]
            //                      ,[tmpebw]
            //                      ,[tapebw]
            //                      ,[tappw]
            //                      ,[tmppw]
            //                      ,[Empresa]
            //                      ,[Sala]
            //                      ,[CodSala]
            //                  FROM [dbo].[CBI_ContadoresBonusIn] (nolock)
            //                  WHERE CodSala = @CodSala
            //                  order by Hora desc";
            string consulta = @"SELECT TOP (1) 
                                  [Hora]
                              FROM [dbo].[CBI_ContadoresBonusIn] (nolock)
                              WHERE CodSala = @CodSala
                              order by Hora desc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", codSala);
                    query.CommandTimeout=0;
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                //contadorBonusIn.Cod_ContadoresBonusIn = ManejoNulos.ManageNullInteger(dr["Cod_ContadoresBonusIn"]);
                                //contadorBonusIn.Cod_Cont_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Cont_OL"]);
                                //contadorBonusIn.Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]);
                                contadorBonusIn.Hora = ManejoNulos.ManageNullDate(dr["Hora"]);
                                //contadorBonusIn.CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]);
                                //contadorBonusIn.CodMaqMin = ManejoNulos.ManageNullStr(dr["CodMaqMin"]);
                                //contadorBonusIn.CodTarjeta = ManejoNulos.ManageNullStr(dr["CodTarjeta"]);
                                //contadorBonusIn.CoinIn = ManejoNulos.ManageNullFloat(dr["CoinIn"]);
                                //contadorBonusIn.CoinOut = ManejoNulos.ManageNullFloat(dr["CoinOut"]);
                                //contadorBonusIn.CurrentCredits = ManejoNulos.ManageNullFloat(dr["CurrentCredits"]);
                                //contadorBonusIn.Monto = ManejoNulos.ManageNullFloat(dr["Monto"]);
                                //contadorBonusIn.EftIn = ManejoNulos.ManageNullFloat(dr["EftIn"]);
                                //contadorBonusIn.EftOut = ManejoNulos.ManageNullFloat(dr["EftOut"]);
                                //contadorBonusIn.CancelCredits = ManejoNulos.ManageNullFloat(dr["CancelCredits"]);
                                //contadorBonusIn.Jackpot = ManejoNulos.ManageNullFloat(dr["Jackpot"]);
                                //contadorBonusIn.GamesPlayed = ManejoNulos.ManageNullFloat(dr["GamesPlayed"]);
                                //contadorBonusIn.TrueIn = ManejoNulos.ManageNullFloat(dr["TrueIn"]);
                                //contadorBonusIn.TrueOut = ManejoNulos.ManageNullFloat(dr["TrueOut"]);
                                //contadorBonusIn.TotalDrop = ManejoNulos.ManageNullFloat(dr["TotalDrop"]);
                                //contadorBonusIn.Bill = ManejoNulos.ManageNullFloat(dr["Bill"]);
                                //contadorBonusIn.Bill1 = ManejoNulos.ManageNullInteger64(dr["Bill1"]);
                                //contadorBonusIn.Bill2 = ManejoNulos.ManageNullInteger64(dr["Bill2"]);
                                //contadorBonusIn.Bill5 = ManejoNulos.ManageNullInteger64(dr["Bill5"]);
                                //contadorBonusIn.Bill10   = ManejoNulos.ManageNullInteger64(dr["Bill10"]);
                                //contadorBonusIn.Bill20 = ManejoNulos.ManageNullInteger64(dr["Bill20"]);
                                //contadorBonusIn.Bill50 = ManejoNulos.ManageNullInteger64(dr["Bill50"]);
                                //contadorBonusIn.Bill100 = ManejoNulos.ManageNullInteger64(dr["Bill100"]);
                                //contadorBonusIn.NroTiket = ManejoNulos.ManageNullStr(dr["NroTiket"]);
                                //contadorBonusIn.TicketIn = ManejoNulos.ManageNullFloat(dr["TicketIn"]);
                                //contadorBonusIn.TicketOut = ManejoNulos.ManageNullFloat(dr["TicketOut"]);
                                //contadorBonusIn.TicketBonusIn = ManejoNulos.ManageNullFloat(dr["TicketBonusIn"]);
                                //contadorBonusIn.TicketBonusOut = ManejoNulos.ManageNullFloat(dr["TicketBonusOut"]);
                                //contadorBonusIn.MontoTiket = ManejoNulos.ManageNullInteger64(dr["MontoTiket"]);
                                //contadorBonusIn.Progresivo = ManejoNulos.ManageNullFloat(dr["Progresivo"]);
                                //contadorBonusIn.Enviado = ManejoNulos.ManageNullStr(dr["Enviado"]);
                                //contadorBonusIn.codevento = ManejoNulos.ManageNullInteger(dr["codevento"]);
                                //contadorBonusIn.codcli = ManejoNulos.ManageNullInteger(dr["codcli"]);
                                //contadorBonusIn.codsuper = ManejoNulos.ManageNullInteger(dr["codsuper"]);
                                //contadorBonusIn.CodPer = ManejoNulos.ManageNullInteger(dr["CodPer"]);
                                //contadorBonusIn.CodAtencion = ManejoNulos.ManageNullInteger(dr["CodAtencion"]);
                                //contadorBonusIn.CodCuadre = ManejoNulos.ManageNullInteger(dr["CodCuadre"]);
                                //contadorBonusIn.PreCredito = ManejoNulos.ManageNullFloat(dr["PreCredito"]);
                                //contadorBonusIn.Token = ManejoNulos.ManageNullFloat(dr["Token"]);
                                //contadorBonusIn.crc = ManejoNulos.ManageNullStr(dr["crc"]);
                                //contadorBonusIn.PorD = ManejoNulos.ManageNullStr(dr["PorD"]);
                                //contadorBonusIn.Tficha = ManejoNulos.ManageNullStr(dr["Tficha"]);
                                //contadorBonusIn.tmpebw = ManejoNulos.ManageNullFloat(dr["tmpebw"]);
                                //contadorBonusIn.tapebw = ManejoNulos.ManageNullFloat(dr["tapebw"]);
                                //contadorBonusIn.tappw = ManejoNulos.ManageNullFloat(dr["tappw"]);
                                //contadorBonusIn.tmppw = ManejoNulos.ManageNullFloat(dr["tmppw"]);
                                //contadorBonusIn.Empresa = ManejoNulos.ManageNullStr(dr["Empresa"]);
                                //contadorBonusIn.Sala = ManejoNulos.ManageNullStr(dr["Sala"]);
                                //contadorBonusIn.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                            }
                        }
                    };

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return contadorBonusIn;
        }

        public Int64 GuardarContadoresBonusIn(ContadoresBonusInCompleto contador) {

            Int64 IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[CBI_ContadoresBonusIn]
           ([Cod_Cont_OL]
           ,[Fecha]
           ,[Hora]
           ,[CodMaq]
           ,[CodMaqMin]
           ,[CodTarjeta]
           ,[CoinIn]
           ,[CoinOut]
           ,[HandPay]
           ,[CurrentCredits]
           ,[Monto]
           ,[EftIn]
           ,[EftOut]
           ,[CancelCredits]
           ,[Jackpot]
           ,[GamesPlayed]
           ,[TrueIn]
           ,[TrueOut]
           ,[TotalDrop]
           ,[Bill]
           ,[Bill1]
           ,[Bill2]
           ,[Bill5]
           ,[Bill10]
           ,[Bill20]
           ,[Bill50]
           ,[Bill100]
           ,[NroTiket]
           ,[TicketIn]
           ,[TicketOut]
           ,[TicketBonusIn]
           ,[TicketBonusOut]
           ,[MontoTiket]
           ,[Progresivo]
           ,[Enviado]
           ,[codevento]
           ,[codcli]
           ,[codsuper]
           ,[CodPer]
           ,[CodAtencion]
           ,[CodCuadre]
           ,[PreCredito]
           ,[Token]
           ,[crc]
           ,[PorD]
           ,[Tficha]
           ,[tmpebw]
           ,[tapebw]
           ,[tappw]
           ,[tmppw]
           ,[Empresa]
           ,[Sala]
           ,[CodSala])
Output Inserted.Cod_ContadoresBonusIn
     VALUES
           (@pCod_Cont_OL
           ,@pFecha
           ,@pHora
           ,@pCodMaq
           ,@pCodMaqMin
           ,@pCodTarjeta
           ,@pCoinIn
           ,@pCoinOut
           ,@pHandPay
           ,@pCurrentCredits
           ,@pMonto
           ,@pEftIn
           ,@pEftOut
           ,@pCancelCredits
           ,@pJackpot
           ,@pGamesPlayed
           ,@pTrueIn
           ,@pTrueOut
           ,@pTotalDrop
           ,@pBill
           ,@pBill1
           ,@pBill2
           ,@pBill5
           ,@pBill10
           ,@pBill20
           ,@pBill50
           ,@pBill100
           ,@pNroTiket
           ,@pTicketIn
           ,@pTicketOut
           ,@pTicketBonusIn
           ,@pTicketBonusOut
           ,@pMontoTiket
           ,@pProgresivo
           ,@pEnviado
           ,@pcodevento
           ,@pcodcli
           ,@pcodsuper
           ,@pCodPer
           ,@pCodAtencion
           ,@pCodCuadre
           ,@pPreCredito
           ,@pToken
           ,@pcrc
           ,@pPorD
           ,@pTficha
           ,@ptmpebw
           ,@ptapebw
           ,@ptappw
           ,@ptmppw
           ,@pEmpresa
           ,@pSala
           ,@pCodSala
          );";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCod_Cont_OL", ManejoNulos.ManageNullInteger64(contador.Cod_Cont_OL));
                    query.Parameters.AddWithValue("@pFecha",ManejoNulos.ManageNullDate(contador.Fecha));
                    query.Parameters.AddWithValue("@pHora",ManejoNulos.ManageNullDate(contador.Hora));
                    query.Parameters.AddWithValue("@pCodMaq",ManejoNulos.ManageNullStr(contador.CodMaq));
                    query.Parameters.AddWithValue("@pCodMaqMin",ManejoNulos.ManageNullStr(contador.CodMaqMin));
                    query.Parameters.AddWithValue("@pCodTarjeta",ManejoNulos.ManageNullStr(contador.CodTarjeta));
                    query.Parameters.AddWithValue("@pCoinIn",ManejoNulos.ManageNullFloat(contador.CoinIn));
                    query.Parameters.AddWithValue("@pCoinOut",ManejoNulos.ManageNullFloat(contador.CoinOut));
                    query.Parameters.AddWithValue("@pHandPay",ManejoNulos.ManageNullFloat(contador.HandPay));
                    query.Parameters.AddWithValue("@pCurrentCredits",ManejoNulos.ManageNullFloat(contador.CurrentCredits));
                    query.Parameters.AddWithValue("@pMonto",ManejoNulos.ManageNullFloat(contador.Monto));
                    query.Parameters.AddWithValue("@pEftIn",ManejoNulos.ManageNullFloat(contador.EftIn));
                    query.Parameters.AddWithValue("@pEftOut",ManejoNulos.ManageNullFloat(contador.EftOut));
                    query.Parameters.AddWithValue("@pCancelCredits",ManejoNulos.ManageNullFloat(contador.CancelCredits));
                    query.Parameters.AddWithValue("@pJackpot",ManejoNulos.ManageNullFloat(contador.Jackpot));
                    query.Parameters.AddWithValue("@pGamesPlayed",ManejoNulos.ManageNullFloat(contador.GamesPlayed));
                    query.Parameters.AddWithValue("@pTrueIn",ManejoNulos.ManageNullFloat(contador.TrueIn));
                    query.Parameters.AddWithValue("@pTrueOut",ManejoNulos.ManageNullFloat(contador.TrueOut));
                    query.Parameters.AddWithValue("@pTotalDrop",ManejoNulos.ManageNullFloat(contador.TotalDrop));
                    query.Parameters.AddWithValue("@pBill",ManejoNulos.ManageNullFloat(contador.Bill));
                    query.Parameters.AddWithValue("@pBill1",ManejoNulos.ManageNullInteger64(contador.Bill1));
                    query.Parameters.AddWithValue("@pBill2",ManejoNulos.ManageNullInteger64(contador.Bill2));
                    query.Parameters.AddWithValue("@pBill5",ManejoNulos.ManageNullInteger64(contador.Bill5));
                    query.Parameters.AddWithValue("@pBill10",ManejoNulos.ManageNullInteger64(contador.Bill10));
                    query.Parameters.AddWithValue("@pBill20",ManejoNulos.ManageNullInteger64(contador.Bill20));
                    query.Parameters.AddWithValue("@pBill50",ManejoNulos.ManageNullInteger64(contador.Bill50));
                    query.Parameters.AddWithValue("@pBill100",ManejoNulos.ManageNullInteger64(contador.Bill100));
                    query.Parameters.AddWithValue("@pNroTiket",ManejoNulos.ManageNullStr(contador.NroTiket));
                    query.Parameters.AddWithValue("@pTicketIn",ManejoNulos.ManageNullFloat(contador.TicketIn));
                    query.Parameters.AddWithValue("@pTicketOut",ManejoNulos.ManageNullFloat(contador.TicketOut));
                    query.Parameters.AddWithValue("@pTicketBonusIn",ManejoNulos.ManageNullFloat(contador.TicketBonusIn));
                    query.Parameters.AddWithValue("@pTicketBonusOut",ManejoNulos.ManageNullFloat(contador.TicketBonusOut));
                    query.Parameters.AddWithValue("@pMontoTiket",ManejoNulos.ManageNullInteger64(contador.MontoTiket));
                    query.Parameters.AddWithValue("@pProgresivo",ManejoNulos.ManageNullFloat(contador.Progresivo));
                    query.Parameters.AddWithValue("@pEnviado",ManejoNulos.ManageNullStr(contador.Enviado));
                    query.Parameters.AddWithValue("@pcodevento",ManejoNulos.ManageNullInteger(contador.codevento));
                    query.Parameters.AddWithValue("@pcodcli",ManejoNulos.ManageNullInteger(contador.codcli));
                    query.Parameters.AddWithValue("@pcodsuper",ManejoNulos.ManageNullInteger(contador.codsuper));
                    query.Parameters.AddWithValue("@pCodPer",ManejoNulos.ManageNullInteger(contador.CodPer));
                    query.Parameters.AddWithValue("@pCodAtencion",ManejoNulos.ManageNullInteger(contador.CodAtencion));
                    query.Parameters.AddWithValue("@pCodCuadre",ManejoNulos.ManageNullInteger(contador.CodCuadre));
                    query.Parameters.AddWithValue("@pPreCredito",ManejoNulos.ManageNullFloat(contador.PreCredito));
                    query.Parameters.AddWithValue("@pToken",ManejoNulos.ManageNullFloat(contador.Token));
                    query.Parameters.AddWithValue("@pcrc",ManejoNulos.ManageNullStr(contador.crc));
                    query.Parameters.AddWithValue("@pPorD",ManejoNulos.ManageNullStr(contador.PorD));
                    query.Parameters.AddWithValue("@pTficha",ManejoNulos.ManageNullStr(contador.Tficha));
                    query.Parameters.AddWithValue("@ptmpebw",ManejoNulos.ManageNullFloat(contador.tmpebw));
                    query.Parameters.AddWithValue("@ptapebw",ManejoNulos.ManageNullFloat(contador.tapebw));
                    query.Parameters.AddWithValue("@ptappw",ManejoNulos.ManageNullFloat(contador.tappw));
                    query.Parameters.AddWithValue("@ptmppw",ManejoNulos.ManageNullFloat(contador.tmppw));
                    query.Parameters.AddWithValue("@pEmpresa",ManejoNulos.ManageNullStr(contador.Empresa));
                    query.Parameters.AddWithValue("@pSala",ManejoNulos.ManageNullStr(contador.Sala));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(contador.CodSala));
                    IdInsertado = Convert.ToInt64(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }


        public List<ContadoresBonusInCompleto> BuscarSiExisteContadorBonusIn(int codSala,float tmpebw, string codmaq) {
            List < ContadoresBonusInCompleto> listaContadorBonusIn = new List<ContadoresBonusInCompleto>();
            string consulta = @"SELECT TOP (1) [Cod_ContadoresBonusIn]
                                  ,[Cod_Cont_OL]
                                  ,[Fecha]
                                  ,[Hora]
                                  ,[CodMaq]
                                  ,[CodMaqMin]
                                  ,[CodTarjeta]
                                  ,[CoinIn]
                                  ,[CoinOut]
                                  ,[HandPay]
                                  ,[CurrentCredits]
                                  ,[Monto]
                                  ,[EftIn]
                                  ,[EftOut]
                                  ,[CancelCredits]
                                  ,[Jackpot]
                                  ,[GamesPlayed]
                                  ,[TrueIn]
                                  ,[TrueOut]
                                  ,[TotalDrop]
                                  ,[Bill]
                                  ,[Bill1]
                                  ,[Bill2]
                                  ,[Bill5]
                                  ,[Bill10]
                                  ,[Bill20]
                                  ,[Bill50]
                                  ,[Bill100]
                                  ,[NroTiket]
                                  ,[TicketIn]
                                  ,[TicketOut]
                                  ,[TicketBonusIn]
                                  ,[TicketBonusOut]
                                  ,[MontoTiket]
                                  ,[Progresivo]
                                  ,[Enviado]
                                  ,[codevento]
                                  ,[codcli]
                                  ,[codsuper]
                                  ,[CodPer]
                                  ,[CodAtencion]
                                  ,[CodCuadre]
                                  ,[PreCredito]
                                  ,[Token]
                                  ,[crc]
                                  ,[PorD]
                                  ,[Tficha]
                                  ,[tmpebw]
                                  ,[tapebw]
                                  ,[tappw]
                                  ,[tmppw]
                                  ,[Empresa]
                                  ,[Sala]
                                  ,[CodSala]
                              FROM [dbo].[CBI_ContadoresBonusIn] (nolock)
                              WHERE CodSala = @pCodSala AND CodMaq= @pCodMaq AND tmpebw = @ptmpebw
                              order by Hora desc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala);
                    query.Parameters.AddWithValue("@pCodMaq", codmaq);
                    query.Parameters.AddWithValue("@ptmpebw", tmpebw);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                ContadoresBonusInCompleto contadorBonusIn = new ContadoresBonusInCompleto();
                                contadorBonusIn.Cod_ContadoresBonusIn = ManejoNulos.ManageNullInteger(dr["Cod_ContadoresBonusIn"]);
                                contadorBonusIn.Cod_Cont_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Cont_OL"]);
                                contadorBonusIn.Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]);
                                contadorBonusIn.Hora = ManejoNulos.ManageNullDate(dr["Hora"]);
                                contadorBonusIn.CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]);
                                contadorBonusIn.CodMaqMin = ManejoNulos.ManageNullStr(dr["CodMaqMin"]);
                                contadorBonusIn.CodTarjeta = ManejoNulos.ManageNullStr(dr["CodTarjeta"]);
                                contadorBonusIn.CoinIn = ManejoNulos.ManageNullFloat(dr["CoinIn"]);
                                contadorBonusIn.CoinOut = ManejoNulos.ManageNullFloat(dr["CoinOut"]);
                                contadorBonusIn.CurrentCredits = ManejoNulos.ManageNullFloat(dr["CurrentCredits"]);
                                contadorBonusIn.Monto = ManejoNulos.ManageNullFloat(dr["Monto"]);
                                contadorBonusIn.EftIn = ManejoNulos.ManageNullFloat(dr["EftIn"]);
                                contadorBonusIn.EftOut = ManejoNulos.ManageNullFloat(dr["EftOut"]);
                                contadorBonusIn.CancelCredits = ManejoNulos.ManageNullFloat(dr["CancelCredits"]);
                                contadorBonusIn.Jackpot = ManejoNulos.ManageNullFloat(dr["Jackpot"]);
                                contadorBonusIn.GamesPlayed = ManejoNulos.ManageNullFloat(dr["GamesPlayed"]);
                                contadorBonusIn.TrueIn = ManejoNulos.ManageNullFloat(dr["TrueIn"]);
                                contadorBonusIn.TrueOut = ManejoNulos.ManageNullFloat(dr["TrueOut"]);
                                contadorBonusIn.TotalDrop = ManejoNulos.ManageNullFloat(dr["TotalDrop"]);
                                contadorBonusIn.Bill = ManejoNulos.ManageNullFloat(dr["Bill"]);
                                contadorBonusIn.Bill1 = ManejoNulos.ManageNullInteger64(dr["Bill1"]);
                                contadorBonusIn.Bill2 = ManejoNulos.ManageNullInteger64(dr["Bill2"]);
                                contadorBonusIn.Bill5 = ManejoNulos.ManageNullInteger64(dr["Bill5"]);
                                contadorBonusIn.Bill10 = ManejoNulos.ManageNullInteger64(dr["Bill10"]);
                                contadorBonusIn.Bill20 = ManejoNulos.ManageNullInteger64(dr["Bill20"]);
                                contadorBonusIn.Bill50 = ManejoNulos.ManageNullInteger64(dr["Bill50"]);
                                contadorBonusIn.Bill100 = ManejoNulos.ManageNullInteger64(dr["Bill100"]);
                                contadorBonusIn.NroTiket = ManejoNulos.ManageNullStr(dr["NroTiket"]);
                                contadorBonusIn.TicketIn = ManejoNulos.ManageNullFloat(dr["TicketIn"]);
                                contadorBonusIn.TicketOut = ManejoNulos.ManageNullFloat(dr["TicketOut"]);
                                contadorBonusIn.TicketBonusIn = ManejoNulos.ManageNullFloat(dr["TicketBonusIn"]);
                                contadorBonusIn.TicketBonusOut = ManejoNulos.ManageNullFloat(dr["TicketBonusOut"]);
                                contadorBonusIn.MontoTiket = ManejoNulos.ManageNullInteger64(dr["MontoTiket"]);
                                contadorBonusIn.Progresivo = ManejoNulos.ManageNullFloat(dr["Progresivo"]);
                                contadorBonusIn.Enviado = ManejoNulos.ManageNullStr(dr["Enviado"]);
                                contadorBonusIn.codevento = ManejoNulos.ManageNullInteger(dr["codevento"]);
                                contadorBonusIn.codcli = ManejoNulos.ManageNullInteger(dr["codcli"]);
                                contadorBonusIn.codsuper = ManejoNulos.ManageNullInteger(dr["codsuper"]);
                                contadorBonusIn.CodPer = ManejoNulos.ManageNullInteger(dr["CodPer"]);
                                contadorBonusIn.CodAtencion = ManejoNulos.ManageNullInteger(dr["CodAtencion"]);
                                contadorBonusIn.CodCuadre = ManejoNulos.ManageNullInteger(dr["CodCuadre"]);
                                contadorBonusIn.PreCredito = ManejoNulos.ManageNullFloat(dr["PreCredito"]);
                                contadorBonusIn.Token = ManejoNulos.ManageNullFloat(dr["Token"]);
                                contadorBonusIn.crc = ManejoNulos.ManageNullStr(dr["crc"]);
                                contadorBonusIn.PorD = ManejoNulos.ManageNullStr(dr["PorD"]);
                                contadorBonusIn.Tficha = ManejoNulos.ManageNullStr(dr["Tficha"]);
                                contadorBonusIn.tmpebw = ManejoNulos.ManageNullFloat(dr["tmpebw"]);
                                contadorBonusIn.tapebw = ManejoNulos.ManageNullFloat(dr["tapebw"]);
                                contadorBonusIn.tappw = ManejoNulos.ManageNullFloat(dr["tappw"]);
                                contadorBonusIn.tmppw = ManejoNulos.ManageNullFloat(dr["tmppw"]);
                                contadorBonusIn.Empresa = ManejoNulos.ManageNullStr(dr["Empresa"]);
                                contadorBonusIn.Sala = ManejoNulos.ManageNullStr(dr["Sala"]);
                                contadorBonusIn.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                listaContadorBonusIn.Add(contadorBonusIn);
                            }
                        }
                    };

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return listaContadorBonusIn;
        }
        
        public List<ContadoresBonusInCompleto> ObtenerListadoContadorBonusInFiltroFechas(int codSala,DateTime fechaIni, DateTime fechaFin) {
            List < ContadoresBonusInCompleto> listaContadorBonusIn = new List<ContadoresBonusInCompleto>();
            string consulta = @"SELECT [Cod_ContadoresBonusIn]
                                  ,[Cod_Cont_OL]
                                  ,[Fecha]
                                  ,[Hora]
                                  ,[CodMaq]
                                  ,[EftIn]
                                  ,[TicketIn]
                                  ,[codevento]
                                  ,[Token]
                                  ,[tmpebw]
                                  ,[tapebw]
                                  ,[tappw]
                                  ,[tmppw]
                                  ,[CodSala]
                              FROM [dbo].[CBI_ContadoresBonusIn] (nolock)
                              WHERE CodSala = @pCodSala AND CONVERT(datetime,Hora) BETWEEN convert(datetime,@pFechaIni) AND convert(datetime,@pFechaFin)
                              order by Hora desc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala);
                    query.Parameters.AddWithValue("@pFechaIni", fechaIni);
                    query.Parameters.AddWithValue("@pFechaFin", fechaFin);
                    query.CommandTimeout = 0;
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                ContadoresBonusInCompleto contadorBonusIn = new ContadoresBonusInCompleto();
                                contadorBonusIn.Cod_ContadoresBonusIn = ManejoNulos.ManageNullInteger(dr["Cod_ContadoresBonusIn"]);
                                contadorBonusIn.Cod_Cont_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Cont_OL"]);
                                contadorBonusIn.Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]);
                                contadorBonusIn.Hora = ManejoNulos.ManageNullDate(dr["Hora"]);
                                contadorBonusIn.CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]);
                                contadorBonusIn.EftIn = ManejoNulos.ManageNullFloat(dr["EftIn"]);
                                contadorBonusIn.TicketIn = ManejoNulos.ManageNullFloat(dr["TicketIn"]);
                                contadorBonusIn.codevento = ManejoNulos.ManageNullInteger(dr["codevento"]);
                                contadorBonusIn.Token = ManejoNulos.ManageNullFloat(dr["Token"]);
                                contadorBonusIn.tmpebw = ManejoNulos.ManageNullFloat(dr["tmpebw"]);
                                contadorBonusIn.tapebw = ManejoNulos.ManageNullFloat(dr["tapebw"]);
                                contadorBonusIn.tappw = ManejoNulos.ManageNullFloat(dr["tappw"]);
                                contadorBonusIn.tmppw = ManejoNulos.ManageNullFloat(dr["tmppw"]);
                                contadorBonusIn.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                listaContadorBonusIn.Add(contadorBonusIn);
                            }
                        }
                    };

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return listaContadorBonusIn;
        }
        public List<ContadoresBonusInCompleto> ObtenerListadoDetalleContadorBonusInFiltroFechas(int codSala,string codMaq, DateTime fechaIni, DateTime fechaFin) {
            List < ContadoresBonusInCompleto> listaContadorBonusIn = new List<ContadoresBonusInCompleto>();
            string consulta = @"SELECT [Cod_ContadoresBonusIn]
                                  ,[Cod_Cont_OL]
                                  ,[Fecha]
                                  ,[Hora]
                                  ,[CodMaq]
                                  ,[EftIn]
                                  ,[TicketIn]
                                  ,[codevento]
                                  ,[Token]
                                  ,[tmpebw]
                                  ,[tapebw]
                                  ,[tappw]
                                  ,[tmppw]
                                  ,[CodSala]
                              FROM [dbo].[CBI_ContadoresBonusIn] (nolock)
                              WHERE CodSala = @pCodSala AND CodMaq = @pCodMaq AND CONVERT(datetime,Hora)>=convert(datetime,@pFechaIni) AND CONVERT(datetime,Hora)<=convert(datetime,@pFechaFin)
                              order by Hora asc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala);
                    query.Parameters.AddWithValue("@pCodMaq", codMaq);
                    query.Parameters.AddWithValue("@pFechaIni", fechaIni);
                    query.Parameters.AddWithValue("@pFechaFin", fechaFin);
					query.CommandTimeout = 0;

					using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                ContadoresBonusInCompleto contadorBonusIn = new ContadoresBonusInCompleto();
								contadorBonusIn.Cod_ContadoresBonusIn = ManejoNulos.ManageNullInteger(dr["Cod_ContadoresBonusIn"]);
								contadorBonusIn.Cod_Cont_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Cont_OL"]);
								contadorBonusIn.Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]);
								contadorBonusIn.Hora = ManejoNulos.ManageNullDate(dr["Hora"]);
								contadorBonusIn.CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]);
								contadorBonusIn.EftIn = ManejoNulos.ManageNullFloat(dr["EftIn"]);
								contadorBonusIn.TicketIn = ManejoNulos.ManageNullFloat(dr["TicketIn"]);
								contadorBonusIn.codevento = ManejoNulos.ManageNullInteger(dr["codevento"]);
								contadorBonusIn.Token = ManejoNulos.ManageNullFloat(dr["Token"]);
								contadorBonusIn.tmpebw = ManejoNulos.ManageNullFloat(dr["tmpebw"]);
								contadorBonusIn.tapebw = ManejoNulos.ManageNullFloat(dr["tapebw"]);
								contadorBonusIn.tappw = ManejoNulos.ManageNullFloat(dr["tappw"]);
								contadorBonusIn.tmppw = ManejoNulos.ManageNullFloat(dr["tmppw"]);
								contadorBonusIn.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
								listaContadorBonusIn.Add(contadorBonusIn);
                            }
                        }
                    };

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return listaContadorBonusIn;
        }

        
        public List<ContadoresBonusInCompleto> ObtenerContadoresBonusInEnviarTotalBonusInJson(int codSala,string codMaq, DateTime fechaIni, DateTime fechaFin) {
            List < ContadoresBonusInCompleto> listaContadorBonusIn = new List<ContadoresBonusInCompleto>();
            string consulta = @"SELECT [Cod_ContadoresBonusIn]
                                  ,[Cod_Cont_OL]
                                  ,[Fecha]
                                  ,[Hora]
                                  ,[CodMaq]
                                  ,[CodMaqMin]
                                  ,[CodTarjeta]
                                  ,[CoinIn]
                                  ,[CoinOut]
                                  ,[HandPay]
                                  ,[CurrentCredits]
                                  ,[Monto]
                                  ,[EftIn]
                                  ,[EftOut]
                                  ,[CancelCredits]
                                  ,[Jackpot]
                                  ,[GamesPlayed]
                                  ,[TrueIn]
                                  ,[TrueOut]
                                  ,[TotalDrop]
                                  ,[Bill]
                                  ,[Bill1]
                                  ,[Bill2]
                                  ,[Bill5]
                                  ,[Bill10]
                                  ,[Bill20]
                                  ,[Bill50]
                                  ,[Bill100]
                                  ,[NroTiket]
                                  ,[TicketIn]
                                  ,[TicketOut]
                                  ,[TicketBonusIn]
                                  ,[TicketBonusOut]
                                  ,[MontoTiket]
                                  ,[Progresivo]
                                  ,[Enviado]
                                  ,[codevento]
                                  ,[codcli]
                                  ,[codsuper]
                                  ,[CodPer]
                                  ,[CodAtencion]
                                  ,[CodCuadre]
                                  ,[PreCredito]
                                  ,[Token]
                                  ,[crc]
                                  ,[PorD]
                                  ,[Tficha]
                                  ,[tmpebw]
                                  ,[tapebw]
                                  ,[tappw]
                                  ,[tmppw]
                                  ,[Empresa]
                                  ,[Sala]
                                  ,[CodSala]
                              FROM [dbo].[CBI_ContadoresBonusIn] (nolock)
                              WHERE CodSala = @pCodSala AND CodMaq = @pCodMaq AND CONVERT(datetime,Hora)>=convert(datetime,@pFechaIni) AND CONVERT(datetime,Hora)<=convert(datetime,@pFechaFin)
                              order by Hora asc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala);
                    query.Parameters.AddWithValue("@pCodMaq", codMaq);
                    query.Parameters.AddWithValue("@pFechaIni", fechaIni);
					query.Parameters.AddWithValue("@pFechaFin", fechaFin);

					using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                ContadoresBonusInCompleto contadorBonusIn = new ContadoresBonusInCompleto();
                                contadorBonusIn.Cod_ContadoresBonusIn = ManejoNulos.ManageNullInteger(dr["Cod_ContadoresBonusIn"]);
                                contadorBonusIn.Cod_Cont_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Cont_OL"]);
                                contadorBonusIn.Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]);
                                contadorBonusIn.Hora = ManejoNulos.ManageNullDate(dr["Hora"]);
                                contadorBonusIn.CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]);
                                contadorBonusIn.CodMaqMin = ManejoNulos.ManageNullStr(dr["CodMaqMin"]);
                                contadorBonusIn.CodTarjeta = ManejoNulos.ManageNullStr(dr["CodTarjeta"]);
                                contadorBonusIn.CoinIn = ManejoNulos.ManageNullFloat(dr["CoinIn"]);
                                contadorBonusIn.CoinOut = ManejoNulos.ManageNullFloat(dr["CoinOut"]);
                                contadorBonusIn.CurrentCredits = ManejoNulos.ManageNullFloat(dr["CurrentCredits"]);
                                contadorBonusIn.Monto = ManejoNulos.ManageNullFloat(dr["Monto"]);
                                contadorBonusIn.EftIn = ManejoNulos.ManageNullFloat(dr["EftIn"]);
                                contadorBonusIn.EftOut = ManejoNulos.ManageNullFloat(dr["EftOut"]);
                                contadorBonusIn.CancelCredits = ManejoNulos.ManageNullFloat(dr["CancelCredits"]);
                                contadorBonusIn.Jackpot = ManejoNulos.ManageNullFloat(dr["Jackpot"]);
                                contadorBonusIn.GamesPlayed = ManejoNulos.ManageNullFloat(dr["GamesPlayed"]);
                                contadorBonusIn.TrueIn = ManejoNulos.ManageNullFloat(dr["TrueIn"]);
                                contadorBonusIn.TrueOut = ManejoNulos.ManageNullFloat(dr["TrueOut"]);
                                contadorBonusIn.TotalDrop = ManejoNulos.ManageNullFloat(dr["TotalDrop"]);
                                contadorBonusIn.Bill = ManejoNulos.ManageNullFloat(dr["Bill"]);
                                contadorBonusIn.Bill1 = ManejoNulos.ManageNullInteger64(dr["Bill1"]);
                                contadorBonusIn.Bill2 = ManejoNulos.ManageNullInteger64(dr["Bill2"]);
                                contadorBonusIn.Bill5 = ManejoNulos.ManageNullInteger64(dr["Bill5"]);
                                contadorBonusIn.Bill10 = ManejoNulos.ManageNullInteger64(dr["Bill10"]);
                                contadorBonusIn.Bill20 = ManejoNulos.ManageNullInteger64(dr["Bill20"]);
                                contadorBonusIn.Bill50 = ManejoNulos.ManageNullInteger64(dr["Bill50"]);
                                contadorBonusIn.Bill100 = ManejoNulos.ManageNullInteger64(dr["Bill100"]);
                                contadorBonusIn.NroTiket = ManejoNulos.ManageNullStr(dr["NroTiket"]);
                                contadorBonusIn.TicketIn = ManejoNulos.ManageNullFloat(dr["TicketIn"]);
                                contadorBonusIn.TicketOut = ManejoNulos.ManageNullFloat(dr["TicketOut"]);
                                contadorBonusIn.TicketBonusIn = ManejoNulos.ManageNullFloat(dr["TicketBonusIn"]);
                                contadorBonusIn.TicketBonusOut = ManejoNulos.ManageNullFloat(dr["TicketBonusOut"]);
                                contadorBonusIn.MontoTiket = ManejoNulos.ManageNullInteger64(dr["MontoTiket"]);
                                contadorBonusIn.Progresivo = ManejoNulos.ManageNullFloat(dr["Progresivo"]);
                                contadorBonusIn.Enviado = ManejoNulos.ManageNullStr(dr["Enviado"]);
                                contadorBonusIn.codevento = ManejoNulos.ManageNullInteger(dr["codevento"]);
                                contadorBonusIn.codcli = ManejoNulos.ManageNullInteger(dr["codcli"]);
                                contadorBonusIn.codsuper = ManejoNulos.ManageNullInteger(dr["codsuper"]);
                                contadorBonusIn.CodPer = ManejoNulos.ManageNullInteger(dr["CodPer"]);
                                contadorBonusIn.CodAtencion = ManejoNulos.ManageNullInteger(dr["CodAtencion"]);
                                contadorBonusIn.CodCuadre = ManejoNulos.ManageNullInteger(dr["CodCuadre"]);
                                contadorBonusIn.PreCredito = ManejoNulos.ManageNullFloat(dr["PreCredito"]);
                                contadorBonusIn.Token = ManejoNulos.ManageNullFloat(dr["Token"]);
                                contadorBonusIn.crc = ManejoNulos.ManageNullStr(dr["crc"]);
                                contadorBonusIn.PorD = ManejoNulos.ManageNullStr(dr["PorD"]);
                                contadorBonusIn.Tficha = ManejoNulos.ManageNullStr(dr["Tficha"]);
                                contadorBonusIn.tmpebw = ManejoNulos.ManageNullFloat(dr["tmpebw"]);
                                contadorBonusIn.tapebw = ManejoNulos.ManageNullFloat(dr["tapebw"]);
                                contadorBonusIn.tappw = ManejoNulos.ManageNullFloat(dr["tappw"]);
                                contadorBonusIn.tmppw = ManejoNulos.ManageNullFloat(dr["tmppw"]);
                                contadorBonusIn.Empresa = ManejoNulos.ManageNullStr(dr["Empresa"]);
                                contadorBonusIn.Sala = ManejoNulos.ManageNullStr(dr["Sala"]);
                                contadorBonusIn.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                listaContadorBonusIn.Add(contadorBonusIn);
                            }
                        }
                    };

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return listaContadorBonusIn;
        }


        public ContadoresBonusInCompleto ObtenerAntContadoresBonusInEnviarTotalBonusInJson(int codSala, string codMaq, DateTime fecha) {
            ContadoresBonusInCompleto listaContadorBonusIn = new ContadoresBonusInCompleto();
            string consulta = @"SELECT TOP 1 [Cod_ContadoresBonusIn]
                                  ,[Cod_Cont_OL]
                                  ,[Fecha]
                                  ,[Hora]
                                  ,[CodMaq]
                                  ,[EftIn]
                                  ,[TicketIn]
                                  ,[codevento]
                                  ,[Token]
                                  ,[tmpebw]
                                  ,[tapebw]
                                  ,[tappw]
                                  ,[tmppw]
                                  ,[CodSala]
                              FROM [dbo].[CBI_ContadoresBonusIn] (nolock)
                              WHERE CodSala = @pCodSala AND CodMaq = @pCodMaq AND CONVERT(datetime,Hora)<convert(datetime,@pFecha)
                              order by Hora desc";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala);
                    query.Parameters.AddWithValue("@pCodMaq", codMaq);
                    query.Parameters.AddWithValue("@pFecha", fecha);
					query.CommandTimeout = 0;

					using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                ContadoresBonusInCompleto contadorBonusIn = new ContadoresBonusInCompleto();
								contadorBonusIn.Cod_ContadoresBonusIn = ManejoNulos.ManageNullInteger(dr["Cod_ContadoresBonusIn"]);
								contadorBonusIn.Cod_Cont_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Cont_OL"]);
								contadorBonusIn.Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]);
								contadorBonusIn.Hora = ManejoNulos.ManageNullDate(dr["Hora"]);
								contadorBonusIn.CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]);
								contadorBonusIn.EftIn = ManejoNulos.ManageNullFloat(dr["EftIn"]);
								contadorBonusIn.TicketIn = ManejoNulos.ManageNullFloat(dr["TicketIn"]);
								contadorBonusIn.codevento = ManejoNulos.ManageNullInteger(dr["codevento"]);
								contadorBonusIn.Token = ManejoNulos.ManageNullFloat(dr["Token"]);
								contadorBonusIn.tmpebw = ManejoNulos.ManageNullFloat(dr["tmpebw"]);
								contadorBonusIn.tapebw = ManejoNulos.ManageNullFloat(dr["tapebw"]);
								contadorBonusIn.tappw = ManejoNulos.ManageNullFloat(dr["tappw"]);
								contadorBonusIn.tmppw = ManejoNulos.ManageNullFloat(dr["tmppw"]);
								contadorBonusIn.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);

								listaContadorBonusIn = contadorBonusIn;
                            }
                        }
                    };

                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return listaContadorBonusIn;
        }

		public Int64 GuardarContadoresOnlineBoton(ContadoresOnlineBoton contador)
		{

			Int64 IdInsertado = 0;
			string consulta = @"
INSERT INTO [dbo].[CBI_ContadoresOnlineBoton]
           ([Cod_Cont_OL]
           ,[Fecha]
           ,[Hora]
           ,[CodMaq]
           ,[CodSala])
Output Inserted.Cod_Cont_OL
     VALUES
           (@pCod_Cont_OL
           ,@pFecha
           ,@pHora
           ,@pCodMaq
           ,@pCodSala
          );";

			try
			{
				using(var con = new SqlConnection(_conexion))
				{
					con.Open();
					var query = new SqlCommand(consulta, con);
					query.Parameters.AddWithValue("@pCod_Cont_OL", ManejoNulos.ManageNullInteger64(contador.Cod_Cont_OL));
					query.Parameters.AddWithValue("@pFecha", ManejoNulos.ManageNullDate(contador.Fecha));
					query.Parameters.AddWithValue("@pHora", ManejoNulos.ManageNullDate(contador.Hora));
					query.Parameters.AddWithValue("@pCodMaq", ManejoNulos.ManageNullStr(contador.CodMaq));
					query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(contador.CodSala));
					IdInsertado = Convert.ToInt64(query.ExecuteScalar());
					//query.ExecuteNonQuery();
					//respuesta = true;
				}
			} catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				IdInsertado = 0;
			}
			return IdInsertado;
		}

		public DateTime ObtenerHoraUltimoContadorBoton(int CodSala)
		{

			DateTime fechaTomaContadores = DateTime.Now.AddDays(-30);

			string consulta = @"SELECT TOP 1 [Hora] 
  FROM [dbo].[CBI_ContadoresOnlineBoton]  (nolock) where [CodSala]=@CodSala ORDER BY Hora DESC ";
			try
			{
				using(var con = new SqlConnection(_conexion))
				{
					con.Open();
					var query = new SqlCommand(consulta, con);
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
				fechaTomaContadores = DateTime.Now.AddDays(-30);
			}
			return fechaTomaContadores;
		}

		public DateTime GetHoraContadoresOnlineBoton(int CodSala, DateTime fecha,string order)
		{

			DateTime fechaTomaContadores = fecha.Date.AddDays(1);

			string consulta = @"SELECT TOP 1 [Hora] 
  FROM [dbo].[CBI_ContadoresOnlineBoton] (nolock) where Convert(date,[Fecha])=Convert(date,@Fecha) and [CodSala]=@CodSala ORDER BY Hora " + order;
			try
			{
				using(var con = new SqlConnection(_conexion))
				{
					con.Open();
					var query = new SqlCommand(consulta, con);
					query.Parameters.AddWithValue("@Fecha", fecha);
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
		public DateTime GetHoraMaquinaContadoresOnlineBoton(int CodSala,string CodMaq, DateTime fecha, string order)
		{

			DateTime fechaTomaContadores = fecha.Date.AddDays(1);

			string consulta = @"SELECT TOP 1 [Hora] 
  FROM [dbo].[CBI_ContadoresOnlineBoton] (nolock) where Convert(date,[Fecha])=Convert(date,@Fecha) and [CodSala]=@CodSala and [CodMaq]=@CodMaq ORDER BY Hora " + order;
			try
			{
				using(var con = new SqlConnection(_conexion))
				{
					con.Open();
					var query = new SqlCommand(consulta, con);
					query.Parameters.AddWithValue("@Fecha", fecha);
					query.Parameters.AddWithValue("@CodSala", CodSala);
					query.Parameters.AddWithValue("@CodMaq", CodMaq);

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
		public List<ContadoresOnlineBoton> GetHoraContadoresOnlineBotonAllMaquina(int CodSala, DateTime fecha)
		{

			List <ContadoresOnlineBoton> listaContadores = new List<ContadoresOnlineBoton>();

			string consulta = @"SELECT 
      [Hora]
      ,[CodMaq]
  FROM [dbo].[CBI_ContadoresOnlineBoton] (nolock) where Convert(date,[Fecha])=Convert(date,@Fecha) and [CodSala]=@CodSala ";
			try
			{
				using(var con = new SqlConnection(_conexion))
				{
					con.Open();
					var query = new SqlCommand(consulta, con);
					query.Parameters.AddWithValue("@Fecha", fecha);
					query.Parameters.AddWithValue("@CodSala", CodSala);

					using(var dr = query.ExecuteReader())
					{
						while(dr.Read())
						{
							ContadoresOnlineBoton contadorOnlineBoton = new ContadoresOnlineBoton();
							contadorOnlineBoton.Hora = ManejoNulos.ManageNullDate(dr["Hora"]);
							contadorOnlineBoton.CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]);
                            listaContadores.Add(contadorOnlineBoton);
						}
					}

				}
			} catch(Exception ex)
			{
				listaContadores = new List<ContadoresOnlineBoton>();
			}
			return listaContadores;
		}
        public List<ContadoresBonusInCompleto> ObtenerListadoContadoreBonusInPorRango(int codSala, DateTime fechaIni, DateTime fechaFin) {
            List<ContadoresBonusInCompleto> listaContadorBonusIn = new List<ContadoresBonusInCompleto>();
            string consulta = @"
                    DECLARE @fechaInicio DATE = @fIni
                    DECLARE @fechaFin DATE = @fFin
                    DECLARE @codsala INT = @sala

                    DECLARE @fechaOperacionIni DATE = DATEADD(DAY, -1, @fechaInicio)
                    DECLARE @fechaOperacionFin DATE = @fechaFin;

                    -- Versión optimizada usando Window Functions
                    WITH BotonEvents AS (
                        SELECT 
                            Fecha,
                            Hora,
                            CodMaq,
                            ROW_NUMBER() OVER (ORDER BY Hora DESC) as RowNum
                        FROM dbo.CBI_ContadoresOnlineBoton (NOLOCK)
                        WHERE CONVERT(DATE, Fecha) BETWEEN @fechaOperacionIni AND @fechaOperacionFin 
                            AND CodSala = @codsala
                    ),
                    RankedContadores AS (
                        SELECT 
                            cb.*,
                            be.Fecha as fechaoperacion,
                            ROW_NUMBER() OVER (
                                PARTITION BY be.CodMaq, be.Hora 
                                ORDER BY cb.Fecha DESC, cb.Hora DESC
                            ) as rn
                        FROM BotonEvents be
                        INNER JOIN dbo.CBI_ContadoresBonusIn cb (NOLOCK) 
                            ON cb.CodMaq = be.CodMaq 
                            AND cb.Hora <= be.Hora
                            AND cb.CodSala = @codsala
                    )
                    SELECT 
                        fechaoperacion,
                        [Cod_ContadoresBonusIn],
                        [Cod_Cont_OL],
                        [Fecha],
                        [Hora],
                        [CodMaq],
                        [CodMaqMin],
                        [CodTarjeta],
                        [CoinIn],
                        [CoinOut],
                        [HandPay],
                        [CurrentCredits],
                        [Monto],
                        [EftIn],
                        [EftOut],
                        [CancelCredits],
                        [Jackpot],
                        [GamesPlayed],
                        [TrueIn],
                        [TrueOut],
                        [TotalDrop],
                        [Bill],
                        [Bill1],
                        [Bill2],
                        [Bill5],
                        [Bill10],
                        [Bill20],
                        [Bill50],
                        [Bill100],
                        [NroTiket],
                        [TicketIn],
                        [TicketOut],
                        [TicketBonusIn],
                        [TicketBonusOut],
                        [MontoTiket],
                        [Progresivo],
                        [Enviado],
                        [codevento],
                        [codcli],
                        [codsuper],
                        [CodPer],
                        [CodAtencion],
                        [CodCuadre],
                        [PreCredito],
                        [Token],
                        [crc],
                        [PorD],
                        [Tficha],
                        [tmpebw],
                        [tapebw],
                        [tappw],
                        [tmppw],
                        [Empresa],
                        [Sala],
                        [CodSala]
                    FROM RankedContadores
                    WHERE rn = 1
                    ORDER BY fechaoperacion DESC";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@sala", codSala);
                    query.Parameters.AddWithValue("@fIni", fechaIni);
                    query.Parameters.AddWithValue("@fFin", fechaFin);
                    query.CommandTimeout = 0;
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                ContadoresBonusInCompleto contadorBonusIn = new ContadoresBonusInCompleto();
                                contadorBonusIn.fOperacion = ManejoNulos.ManageNullDate(dr["fechaoperacion"]);
                                contadorBonusIn.Cod_ContadoresBonusIn = ManejoNulos.ManageNullInteger(dr["Cod_ContadoresBonusIn"]);
                                contadorBonusIn.Cod_Cont_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Cont_OL"]);
                                contadorBonusIn.Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]);
                                contadorBonusIn.Hora = ManejoNulos.ManageNullDate(dr["Hora"]);
                                contadorBonusIn.CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]);
                                contadorBonusIn.EftIn = ManejoNulos.ManageNullFloat(dr["EftIn"]);
                                contadorBonusIn.TicketIn = ManejoNulos.ManageNullFloat(dr["TicketIn"]);
                                contadorBonusIn.codevento = ManejoNulos.ManageNullInteger(dr["codevento"]);
                                contadorBonusIn.Token = ManejoNulos.ManageNullFloat(dr["Token"]);
                                contadorBonusIn.tmpebw = ManejoNulos.ManageNullFloat(dr["tmpebw"]);
                                contadorBonusIn.tapebw = ManejoNulos.ManageNullFloat(dr["tapebw"]);
                                contadorBonusIn.tappw = ManejoNulos.ManageNullFloat(dr["tappw"]);
                                contadorBonusIn.tmppw = ManejoNulos.ManageNullFloat(dr["tmppw"]);
                                contadorBonusIn.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                listaContadorBonusIn.Add(contadorBonusIn);
                            }
                        }
                    };

                }
            } catch(Exception ex) {
                listaContadorBonusIn = new List<ContadoresBonusInCompleto>();
            }
            return listaContadorBonusIn;
        }
        public List<ContadoresOnlineBoton> GetContadoresOnlineBotonPorRangoFechas(int CodSala, DateTime fechaIni,DateTime fechaFin) {

            List<ContadoresOnlineBoton> listaContadores = new List<ContadoresOnlineBoton>();

            string consulta = @"SELECT 
        [Fecha],
      [Hora]
      ,[CodMaq],[CodSala]
  FROM [dbo].[CBI_ContadoresOnlineBoton] (nolock) where Convert(date,[Fecha]) between Convert(date,@fechaIni) and Convert(date,@fechaFin) and [CodSala]=@CodSala ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@fechaIni", fechaIni);
                    query.Parameters.AddWithValue("@fechaFin", fechaFin);
                    query.Parameters.AddWithValue("@CodSala", CodSala);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            ContadoresOnlineBoton contadorOnlineBoton = new ContadoresOnlineBoton();
                            contadorOnlineBoton.Hora = ManejoNulos.ManageNullDate(dr["Hora"]);
                            contadorOnlineBoton.CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]);
                            contadorOnlineBoton.Fecha= ManejoNulos.ManageNullDate(dr["Fecha"]);
                            contadorOnlineBoton.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                            listaContadores.Add(contadorOnlineBoton);
                        }
                    }

                }
            } catch(Exception ex) {
                listaContadores = new List<ContadoresOnlineBoton>();
            }
            return listaContadores;
        }
    }
}
