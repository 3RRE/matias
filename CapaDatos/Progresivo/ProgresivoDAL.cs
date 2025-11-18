using CapaEntidad;
using CapaEntidad.ProgresivoOffline;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.Progresivo
{
    public  class ProgresivoDAL
    {
        string _conexion = string.Empty;
        public ProgresivoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ProgresivoEntidad> GetProgresivos()
        {
            List<ProgresivoEntidad> lista = new List<ProgresivoEntidad>();
            string consulta = @"SELECT [WEB_PrgID],[WEB_Nombre],[WEB_NroPozos],[WEB_Url],[WEB_Estado],[WEB_FechaRegistro]
                            FROM [dbo].[WEB_Progresivo] order by WEB_PrgID Desc";
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
                            var webProgresivo = new ProgresivoEntidad
                            {
                                WEB_PrgID = ManejoNulos.ManageNullInteger(dr["WEB_PrgID"]),
                                WEB_Nombre = ManejoNulos.ManageNullStr(dr["WEB_Nombre"]),
                                WEB_NroPozos = ManejoNulos.ManageNullInteger(dr["WEB_NroPozos"].Trim()),
                                WEB_Url = ManejoNulos.ManageNullStr(dr["WEB_Url"].Trim()),
                                WEB_Estado = ManejoNulos.ManageNullStr(dr["WEB_Estado"].Trim()),
                                WEB_FechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_FechaRegistro"].Trim())
                            };
                            lista.Add(webProgresivo);
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
        public bool ProgresivoHistoricoInsertarJson(HistorialProgresivoEntidad obj)
        {
            string consulta = @"insert into HistorialProgresivo(CodSala,CodProgresivo,Parametros,FechaModificacion,UsuarioID)
values(@p1,@p2,@p3,getDate(),@p4)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", obj.CodSala);
                    query.Parameters.AddWithValue("@p2", obj.CodProgresivo);
                    query.Parameters.AddWithValue("@p3", obj.Parametros);
                    query.Parameters.AddWithValue("@p4", obj.UsuarioID);
                    query.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }



        public string ProgresivoNombre(int codProgresivo, int codSala) {
            string nombreProgresivo = "";
            string consulta = @"select WEB_Nombre from PRO_Progresivo where WEB_PrgID = @p1 and CodSala = @p2";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", codProgresivo);
                    query.Parameters.AddWithValue("@p2", codSala);

                    // Ejecuta la consulta y obtén el resultado
                    object result = query.ExecuteScalar();

                    if(result != null) {
                        nombreProgresivo = result.ToString();
                    }

                    return nombreProgresivo;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return nombreProgresivo;
            }
        }


        #region Progresivo Offline

        public CabeceraOfflineEntidad GetUltimaFechaProgresivoxSala(int CodSala, int CodProgresivo)
        {
            CabeceraOfflineEntidad item = new CabeceraOfflineEntidad();
            string consulta = @"SELECT TOP 1  [IdCabeceraProgresivo]
                                  ,[CodSala]
                                  ,[CodProgresivo]
                                  ,[ProgresivoID]
                                  ,[DetalleProgresivoID]
                                  ,[ProcesosID]
                                  ,[GanadorID]
                                  ,[SlotID]
                                  ,[Monto]
                                  ,[Valor]
                                  ,[TipoPozo]
                                  ,[Fecha]
                                  ,[Estado]
                                  ,[CoinInAct]
                                  ,[CoinInAnt]
                                  ,[Toquen]
                                  ,[CoinOut]
                                  ,[Jackpot]
                                  ,[Cancelcredits]
                                  ,[Billetero]
                                  ,[BonusWinAct]
                                  ,[BonusWinAnt]
                                  ,[CreditAct]
                                  ,[CreditAnt]
                                  ,[NroJugadores]
                                  ,[ValorReal]
                                  ,[NroJugada]
                                  ,[Pagado]
                                  ,[FechaPago]
                                  ,[indice]
                                  ,[desc_pozo]
                                  ,[desc_estado]
                                  ,[desc_modelo]
                                  ,[desc_marca]
                                  ,[desc_imagen_progresivo]
                                  ,[cod_imagen_progresivo]
                                  ,[Cantidad]
                                  ,[desc_fecha]
                                  ,[desc_hora_pago]
                                  ,[codalterno]
                                  ,[File]
                              FROM [BD_SEGURIDAD_PJ].[dbo].[PRO_Cabecera] 
                              WHERE CodProgresivo=@pCodProgresivo and CodSala=@pCodSala ORDER BY Fecha DESC";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodProgresivo", CodProgresivo);
                    query.Parameters.AddWithValue("@pCodSala", CodSala);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                                item.IdCabeceraProgresivo = ManejoNulos.ManageNullInteger(dr["IdCabeceraProgresivo"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.CodProgresivo = ManejoNulos.ManageNullInteger(dr["CodProgresivo"]);
                                item.ProgresivoID = ManejoNulos.ManageNullInteger(dr["ProgresivoID"]);
                                item.DetalleProgresivoID = ManejoNulos.ManageNullInteger(dr["DetalleProgresivoID"]);
                                item.ProcesosID = ManejoNulos.ManageNullStr(dr["ProcesosID"]);
                                item.GanadorID = ManejoNulos.ManageNullInteger(dr["GanadorID"]);
                                item.SlotID = ManejoNulos.ManageNullStr(dr["SlotID"]);
                                item.Monto = ManejoNulos.ManageNullDouble(dr["Monto"]);
                                item.Valor = ManejoNulos.ManageNullDouble(dr["Valor"]);
                                item.TipoPozo = ManejoNulos.ManageNullInteger(dr["TipoPozo"]);
                                item.Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]);
                                item.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                                item.CoinInAct = ManejoNulos.ManageNullDouble(dr["CoinInAct"]);
                                item.CoinInAnt = ManejoNulos.ManageNullDouble(dr["CoinInAnt"]);
                                item.Toquen = ManejoNulos.ManageNullDouble(dr["Toquen"]);
                                item.CoinOut = ManejoNulos.ManageNullDouble(dr["CoinOut"]);
                                item.Jackpot = ManejoNulos.ManageNullDouble(dr["Jackpot"]);
                                item.Cancelcredits = ManejoNulos.ManageNullDouble(dr["Cancelcredits"]);
                                item.Billetero = ManejoNulos.ManageNullDouble(dr["Billetero"]);
                                item.BonusWinAct = ManejoNulos.ManageNullDouble(dr["BonusWinAct"]);
                                item.BonusWinAnt = ManejoNulos.ManageNullDouble(dr["BonusWinAnt"]);
                                item.CreditAct = ManejoNulos.ManageNullDouble(dr["CreditAct"]);
                                item.CreditAnt = ManejoNulos.ManageNullDouble(dr["CreditAnt"]);
                                item.NroJugadores = ManejoNulos.ManageNullInteger(dr["NroJugadores"]);
                                item.ValorReal = ManejoNulos.ManageNullDouble(dr["ValorReal"]);
                                item.NroJugada = ManejoNulos.ManageNullDouble(dr["NroJugada"]);
                                item.Pagado = ManejoNulos.ManageNullDouble(dr["Pagado"]);
                                item.FechaPago = ManejoNulos.ManageNullDate(dr["FechaPago"]);
                                item.indice = ManejoNulos.ManageNullInteger(dr["indice"]);
                                item.desc_pozo = ManejoNulos.ManageNullStr(dr["desc_pozo"]);
                                item.desc_estado = ManejoNulos.ManageNullStr(dr["desc_estado"]);
                                item.desc_modelo = ManejoNulos.ManageNullStr(dr["desc_modelo"]);
                                item.desc_marca = ManejoNulos.ManageNullStr(dr["desc_marca"]);
                                item.desc_imagen_progresivo = ManejoNulos.ManageNullStr(dr["desc_imagen_progresivo"]);
                                item.cod_imagen_progresivo = ManejoNulos.ManageNullInteger(dr["cod_imagen_progresivo"]);
                                item.Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]);
                                item.desc_fecha = ManejoNulos.ManageNullStr(dr["desc_fecha"]);
                                item.desc_hora_pago = ManejoNulos.ManageNullStr(dr["desc_hora_pago"]);
                                item.codalterno = ManejoNulos.ManageNullStr(dr["codalterno"]);
                                item.File = ManejoNulos.ManageNullStr(dr["File"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return item;
        }

        public List<ProgresivoOfflineEntidad> GetUltimaFechaProgresivoxSala(int codSala)
        {
            List<ProgresivoOfflineEntidad> lista = new List<ProgresivoOfflineEntidad>();
            string consulta = @"SELECT [IdProgresivo]
                                  ,[CodSala]
                                  ,[WEB_PrgID]
                                  ,[WEB_Nombre]
                                  ,[WEB_NroPozos]
                                  ,[WEB_Url]
                                  ,[WEB_Estado]
                                  ,[WEB_FechaRegistro]
                              FROM [PRO_Progresivo] WHERE CodSala=@pCodSala order by IdProgresivo Desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var webProgresivo = new ProgresivoOfflineEntidad
                            {
                                IdProgresivo = ManejoNulos.ManageNullInteger(dr["IdProgresivo"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                WEB_PrgID = ManejoNulos.ManageNullInteger(dr["WEB_PrgID"]),
                                WEB_Nombre = ManejoNulos.ManageNullStr(dr["WEB_Nombre"]),
                                WEB_NroPozos = ManejoNulos.ManageNullInteger(dr["WEB_NroPozos"]),
                                WEB_Url = ManejoNulos.ManageNullStr(dr["WEB_Url"]),
                                WEB_Estado = ManejoNulos.ManageNullStr(dr["WEB_Estado"]),
                                WEB_FechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_FechaRegistro"])
                            };
                            lista.Add(webProgresivo);
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

        public List<ProgresivoOfflineEntidad> GetProgresivoOfflinexSalaxProgresivo(int codSala)
        {
            List<ProgresivoOfflineEntidad> lista = new List<ProgresivoOfflineEntidad>();
            string consulta = @"SELECT [IdProgresivo]
                                  ,[CodSala]
                                  ,[WEB_PrgID]
                                  ,[WEB_Nombre]
                                  ,[WEB_NroPozos]
                                  ,[WEB_Url]
                                  ,[WEB_Estado]
                                  ,[WEB_FechaRegistro]
                              FROM [PRO_Progresivo] WHERE CodSala=@pCodSala order by IdProgresivo Desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var webProgresivo = new ProgresivoOfflineEntidad
                            {
                                IdProgresivo = ManejoNulos.ManageNullInteger(dr["IdProgresivo"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                WEB_PrgID = ManejoNulos.ManageNullInteger(dr["WEB_PrgID"]),
                                WEB_Nombre = ManejoNulos.ManageNullStr(dr["WEB_Nombre"]),
                                WEB_NroPozos = ManejoNulos.ManageNullInteger(dr["WEB_NroPozos"]),
                                WEB_Url = ManejoNulos.ManageNullStr(dr["WEB_Url"]),
                                WEB_Estado = ManejoNulos.ManageNullStr(dr["WEB_Estado"]),
                                WEB_FechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_FechaRegistro"])
                            };
                            lista.Add(webProgresivo);
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
        public bool ProgresivoOfflineInsertarJson(ProgresivoOfflineEntidad obj)
        {
            string consulta = @"INSERT INTO [dbo].[PRO_Progresivo]
                                   ([WEB_PrgID]
                                  ,[CodSala]
                                  ,[WEB_Nombre]
                                  ,[WEB_NroPozos]
                                  ,[WEB_Url]
                                  ,[WEB_Estado]
                                  ,[WEB_FechaRegistro])
                             VALUES
                                   (@pWEB_PrgID
                                   ,@pCodSala
                                   ,@pWEB_Nombre
                                   ,@pWEB_NroPozos
                                   ,@pWEB_Url
                                   ,@pWEB_Estado
                                   ,@pWEB_FechaRegistro)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(obj.CodSala));
                    query.Parameters.AddWithValue("@pWEB_PrgID", ManejoNulos.ManageNullInteger(obj.WEB_PrgID));
                    query.Parameters.AddWithValue("@pWEB_Nombre", ManejoNulos.ManageNullStr(obj.WEB_Nombre));
                    query.Parameters.AddWithValue("@pWEB_NroPozos", ManejoNulos.ManageNullInteger(obj.WEB_NroPozos));
                    query.Parameters.AddWithValue("@pWEB_Url", ManejoNulos.ManageNullStr(obj.WEB_Url));
                    query.Parameters.AddWithValue("@pWEB_Estado", ManejoNulos.ManageNullStr(obj.WEB_Estado));
                    query.Parameters.AddWithValue("@pWEB_FechaRegistro", ManejoNulos.ManageNullDate(obj.WEB_FechaRegistro));
                    query.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public bool ProgresivoOfflineActualizarJson(ProgresivoOfflineEntidad obj)
        {
            string consulta = @"UPDATE [dbo].[PRO_Progresivo]
                               SET [WEB_Nombre] = @pWEB_Nombre
                                  ,[WEB_NroPozos] = @pWEB_NroPozos
                                  ,[WEB_Url] = @pWEB_Url
                                  ,[WEB_Estado] = @pWEB_Estado
                                  ,[WEB_FechaRegistro] = @pWEB_FechaRegistro WHERE CodSala = @pCodSala AND [WEB_PrgID] = @pWEB_PrgID";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(obj.CodSala));
                    query.Parameters.AddWithValue("@pWEB_PrgID", ManejoNulos.ManageNullInteger(obj.WEB_PrgID));
                    query.Parameters.AddWithValue("@pWEB_Nombre", ManejoNulos.ManageNullStr(obj.WEB_Nombre));
                    query.Parameters.AddWithValue("@pWEB_NroPozos", ManejoNulos.ManageNullInteger(obj.WEB_NroPozos));
                    query.Parameters.AddWithValue("@pWEB_Url", ManejoNulos.ManageNullStr(obj.WEB_Url));
                    query.Parameters.AddWithValue("@pWEB_Estado", ManejoNulos.ManageNullStr(obj.WEB_Estado));
                    query.Parameters.AddWithValue("@pWEB_FechaRegistro", ManejoNulos.ManageNullDate(obj.WEB_FechaRegistro));
                    query.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<ProgresivoOfflineEntidad> GetProgresivoOffline()
        {
            List<ProgresivoOfflineEntidad> lista = new List<ProgresivoOfflineEntidad>();
            string consulta = @"SELECT [IdProgresivo]
                                  ,[CodSala]
                                  ,[WEB_PrgID]
                                  ,[WEB_Nombre]
                                  ,[WEB_NroPozos]
                                  ,[WEB_Url]
                                  ,[WEB_Estado]
                                  ,[WEB_FechaRegistro]
                              FROM [PRO_Progresivo]";
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
                            var webProgresivo = new ProgresivoOfflineEntidad
                            {
                                IdProgresivo = ManejoNulos.ManageNullInteger(dr["IdProgresivo"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                WEB_PrgID = ManejoNulos.ManageNullInteger(dr["WEB_PrgID"]),
                                WEB_Nombre = ManejoNulos.ManageNullStr(dr["WEB_Nombre"]),
                                WEB_NroPozos = ManejoNulos.ManageNullInteger(dr["WEB_NroPozos"]),
                                WEB_Url = ManejoNulos.ManageNullStr(dr["WEB_Url"]),
                                WEB_Estado = ManejoNulos.ManageNullStr(dr["WEB_Estado"]),
                                WEB_FechaRegistro = ManejoNulos.ManageNullDate(dr["WEB_FechaRegistro"]),
                            };
                            lista.Add(webProgresivo);
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

        public List<CabeceraOfflineEntidad> GetCabeceraOfflinexSalaxProgresivo(int codSala, int codProgresivo, DateTime fechaIni, DateTime fechaFin)
        {
            List<CabeceraOfflineEntidad> lista = new List<CabeceraOfflineEntidad>();
            string consulta = @"SELECT [IdCabeceraProgresivo]
                                  ,[CodSala]
                                  ,[CodProgresivo]
                                  ,[ProgresivoID]
                                  ,[DetalleProgresivoID]
                                  ,[ProcesosID]
                                  ,[GanadorID]
                                  ,[SlotID]
                                  ,[Monto]
                                  ,[Valor]
                                  ,[TipoPozo]
                                  ,[Fecha]
                                  ,[Estado]
                                  ,[CoinInAct]
                                  ,[CoinInAnt]
                                  ,[Toquen]
                                  ,[CoinOut]
                                  ,[Jackpot]
                                  ,[Cancelcredits]
                                  ,[Billetero]
                                  ,[BonusWinAct]
                                  ,[BonusWinAnt]
                                  ,[CreditAct]
                                  ,[CreditAnt]
                                  ,[NroJugadores]
                                  ,[ValorReal]
                                  ,[NroJugada]
                                  ,[Pagado]
                                  ,[FechaPago]
                                  ,[indice]
                                  ,[desc_pozo]
                                  ,[desc_estado]
                                  ,[desc_modelo]
                                  ,[desc_marca]
                                  ,[desc_imagen_progresivo]
                                  ,[cod_imagen_progresivo]
                                  ,[Cantidad]
                                  ,[desc_fecha]
                                  ,[desc_hora_pago]
                                  ,[codalterno]
                                  ,[File]
                              FROM [PRO_Cabecera] WHERE CodSala=@pCodSala AND CodProgresivo=@pCodProgresivo and convert(date,Fecha) between @p1 and @p2 order by Fecha Desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala);
                    query.Parameters.AddWithValue("@pCodProgresivo", codProgresivo);
                    query.Parameters.AddWithValue("@p1", fechaIni.Date);
                    query.Parameters.AddWithValue("@p2", fechaFin.Date);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var webProgresivo = new CabeceraOfflineEntidad
                            {
                                IdCabeceraProgresivo = ManejoNulos.ManageNullInteger(dr["IdCabeceraProgresivo"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodProgresivo = ManejoNulos.ManageNullInteger(dr["CodProgresivo"]),
                                ProgresivoID = ManejoNulos.ManageNullInteger(dr["ProgresivoID"]),
                                DetalleProgresivoID = ManejoNulos.ManageNullInteger(dr["DetalleProgresivoID"]),
                                ProcesosID = ManejoNulos.ManageNullStr(dr["ProcesosID"]),
                                GanadorID = ManejoNulos.ManageNullInteger(dr["GanadorID"]),
                                SlotID = ManejoNulos.ManageNullStr(dr["SlotID"]),
                                Monto = ManejoNulos.ManageNullDouble(dr["Monto"]),
                                Valor = ManejoNulos.ManageNullDouble(dr["Valor"]),
                                TipoPozo = ManejoNulos.ManageNullInteger(dr["TipoPozo"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CoinInAct = ManejoNulos.ManageNullDouble(dr["CoinInAct"]),
                                CoinInAnt = ManejoNulos.ManageNullDouble(dr["CoinInAnt"]),
                                Toquen = ManejoNulos.ManageNullDouble(dr["Toquen"]),
                                CoinOut = ManejoNulos.ManageNullDouble(dr["CoinOut"]),
                                Jackpot = ManejoNulos.ManageNullDouble(dr["Jackpot"]),
                                Cancelcredits = ManejoNulos.ManageNullDouble(dr["Cancelcredits"]),
                                Billetero = ManejoNulos.ManageNullDouble(dr["Billetero"]),
                                BonusWinAct = ManejoNulos.ManageNullDouble(dr["BonusWinAct"]),
                                BonusWinAnt = ManejoNulos.ManageNullDouble(dr["BonusWinAnt"]),
                                CreditAct = ManejoNulos.ManageNullDouble(dr["CreditAct"]),
                                CreditAnt = ManejoNulos.ManageNullDouble(dr["CreditAnt"]),
                                NroJugadores = ManejoNulos.ManageNullInteger(dr["NroJugadores"]),
                                ValorReal = ManejoNulos.ManageNullDouble(dr["ValorReal"]),
                                NroJugada = ManejoNulos.ManageNullDouble(dr["NroJugada"]),
                                Pagado = ManejoNulos.ManageNullDouble(dr["Pagado"]),
                                FechaPago = ManejoNulos.ManageNullDate(dr["FechaPago"]),
                                indice = ManejoNulos.ManageNullInteger(dr["indice"]),
                                desc_pozo = ManejoNulos.ManageNullStr(dr["desc_pozo"]),
                                desc_estado = ManejoNulos.ManageNullStr(dr["desc_estado"]),
                                desc_modelo = ManejoNulos.ManageNullStr(dr["desc_modelo"]),
                                desc_marca = ManejoNulos.ManageNullStr(dr["desc_marca"]),
                                desc_imagen_progresivo = ManejoNulos.ManageNullStr(dr["desc_imagen_progresivo"]),
                                cod_imagen_progresivo = ManejoNulos.ManageNullInteger(dr["cod_imagen_progresivo"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                desc_fecha = ManejoNulos.ManageNullStr(dr["desc_fecha"]),
                                desc_hora_pago = ManejoNulos.ManageNullStr(dr["desc_hora_pago"]),
                                codalterno = ManejoNulos.ManageNullStr(dr["codalterno"]),
                                File = ManejoNulos.ManageNullStr(dr["File"]),
                            };
                            lista.Add(webProgresivo);
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
        public int CabeceraOfflineInsertarJson(CabeceraOfflineEntidad obj)
        {
            int idInsertado = 0;

            string consulta = @"INSERT INTO [dbo].[PRO_Cabecera]
                                   ([CodSala]
                                   ,[CodProgresivo]
                                   ,[ProgresivoID]
                                   ,[DetalleProgresivoID]
                                   ,[ProcesosID]
                                   ,[GanadorID]
                                   ,[SlotID]
                                   ,[Monto]
                                   ,[Valor]
                                   ,[TipoPozo]
                                   ,[Fecha]
                                   ,[Estado]
                                   ,[CoinInAct]
                                   ,[CoinInAnt]
                                   ,[Toquen]
                                   ,[CoinOut]
                                   ,[Jackpot]
                                   ,[Cancelcredits]
                                   ,[Billetero]
                                   ,[BonusWinAct]
                                   ,[BonusWinAnt]
                                   ,[CreditAct]
                                   ,[CreditAnt]
                                   ,[NroJugadores]
                                   ,[ValorReal]
                                   ,[NroJugada]
                                   ,[Pagado]
                                   ,[FechaPago]
                                   ,[indice]
                                   ,[desc_pozo]
                                   ,[desc_estado]
                                   ,[desc_modelo]
                                   ,[desc_marca]
                                   ,[desc_imagen_progresivo]
                                   ,[cod_imagen_progresivo]
                                   ,[Cantidad]
                                   ,[desc_fecha]
                                   ,[desc_hora_pago]
                                   ,[codalterno]
                                   ,[File])
                             OUTPUT Inserted.IdCabeceraProgresivo
                             VALUES
                                   (@pCodSala
                                   ,@pCodProgresivo
                                   ,@pProgresivoID
                                   ,@pDetalleProgresivoID
                                   ,@pProcesosID
                                   ,@pGanadorID
                                   ,@pSlotID 
                                   ,@pMonto 
                                   ,@pValor 
                                   ,@pTipoPozo 
                                   ,@pFecha 
                                   ,@pEstado 
                                   ,@pCoinInAct 
                                   ,@pCoinInAnt 
                                   ,@pToquen 
                                   ,@pCoinOut 
                                   ,@pJackpot 
                                   ,@pCancelcredits 
                                   ,@pBilletero 
                                   ,@pBonusWinAct 
                                   ,@pBonusWinAnt 
                                   ,@pCreditAct 
                                   ,@pCreditAnt 
                                   ,@pNroJugadores 
                                   ,@pValorReal 
                                   ,@pNroJugada 
                                   ,@pPagado 
                                   ,@pFechaPago 
                                   ,@pindice 
                                   ,@pdesc_pozo 
                                   ,@pdesc_estado 
                                   ,@pdesc_modelo 
                                   ,@pdesc_marca 
                                   ,@pdesc_imagen_progresivo 
                                   ,@pcod_imagen_progresivo 
                                   ,@pCantidad 
                                   ,@pdesc_fecha 
                                   ,@pdesc_hora_pago 
                                   ,@pcodalterno 
                                   ,@pFile )";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(obj.CodSala));
                    query.Parameters.AddWithValue("@pCodProgresivo", ManejoNulos.ManageNullInteger(obj.CodProgresivo));
                    query.Parameters.AddWithValue("@pProgresivoID", obj.ProgresivoID);
                    query.Parameters.AddWithValue("@pDetalleProgresivoID", obj.DetalleProgresivoID);
                    query.Parameters.AddWithValue("@pProcesosID", obj.ProcesosID);
                    query.Parameters.AddWithValue("@pGanadorID", obj.GanadorID);
                    query.Parameters.AddWithValue("@pSlotID ", obj.SlotID);
                    query.Parameters.AddWithValue("@pMonto ", obj.Monto);
                    query.Parameters.AddWithValue("@pValor ", obj.Valor);
                    query.Parameters.AddWithValue("@pTipoPozo ", obj.TipoPozo);
                    query.Parameters.AddWithValue("@pFecha ", ManejoNulos.ManageNullDate(obj.Fecha));
                    query.Parameters.AddWithValue("@pEstado ", obj.Estado);
                    query.Parameters.AddWithValue("@pCoinInAct ", obj.CoinInAct);
                    query.Parameters.AddWithValue("@pCoinInAnt ", obj.CoinInAnt);
                    query.Parameters.AddWithValue("@pToquen ", obj.Toquen);
                    query.Parameters.AddWithValue("@pCoinOut ", obj.CoinOut);
                    query.Parameters.AddWithValue("@pJackpot ", obj.Jackpot);
                    query.Parameters.AddWithValue("@pCancelcredits ", obj.Cancelcredits);
                    query.Parameters.AddWithValue("@pBilletero ", obj.Billetero);
                    query.Parameters.AddWithValue("@pBonusWinAct ", obj.BonusWinAct);
                    query.Parameters.AddWithValue("@pBonusWinAnt ", obj.BonusWinAnt);
                    query.Parameters.AddWithValue("@pCreditAct ", obj.CreditAct);
                    query.Parameters.AddWithValue("@pCreditAnt ", obj.CreditAnt);
                    query.Parameters.AddWithValue("@pNroJugadores ", obj.NroJugadores);
                    query.Parameters.AddWithValue("@pValorReal ", ManejoNulos.ManageNullInteger(obj.ValorReal));
                    query.Parameters.AddWithValue("@pNroJugada ", ManejoNulos.ManageNullInteger(obj.NroJugada));
                    query.Parameters.AddWithValue("@pPagado ", ManejoNulos.ManageNullInteger(obj.Pagado));
                    query.Parameters.AddWithValue("@pFechaPago ", ManejoNulos.ManageNullDate(obj.FechaPago = (obj.FechaPago.ToString("dd/MM/yyyy") == "01/01/0001") ? Convert.ToDateTime("01/01/1753") : obj.FechaPago));
                    query.Parameters.AddWithValue("@pindice ", ManejoNulos.ManageNullInteger(obj.indice));
                    query.Parameters.AddWithValue("@pdesc_pozo ", obj.desc_pozo);
                    query.Parameters.AddWithValue("@pdesc_estado ", ManejoNulos.ManageNullStr(obj.desc_estado));
                    query.Parameters.AddWithValue("@pdesc_modelo ", obj.desc_modelo);
                    query.Parameters.AddWithValue("@pdesc_marca ", obj.desc_marca);
                    query.Parameters.AddWithValue("@pdesc_imagen_progresivo", obj.desc_imagen_progresivo);
                    query.Parameters.AddWithValue("@pcod_imagen_progresivo ", obj.cod_imagen_progresivo);
                    query.Parameters.AddWithValue("@pCantidad ", obj.Cantidad);
                    query.Parameters.AddWithValue("@pdesc_fecha ", obj.desc_fecha);
                    query.Parameters.AddWithValue("@pdesc_hora_pago ", obj.desc_hora_pago);
                    query.Parameters.AddWithValue("@pcodalterno ", obj.codalterno);
                    query.Parameters.AddWithValue("@pFile", obj.File);
                    idInsertado = Convert.ToInt32( query.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                idInsertado = 0;
            }
            return idInsertado;
        }
        public List<CabeceraOfflineEntidad> GetCabeceraOffline()
        {
            List<CabeceraOfflineEntidad> lista = new List<CabeceraOfflineEntidad>();
            string consulta = @"SELECT [IdCabeceraProgresivo],[CodSala]
                                  ,[CodProgresivo]
                                  ,[ProgresivoID]
                                  ,[DetalleProgresivoID]
                                  ,[ProcesosID]
                                  ,[GanadorID]
                                  ,[SlotID]
                                  ,[Monto]
                                  ,[Valor]
                                  ,[TipoPozo]
                                  ,[Fecha]
                                  ,[Estado]
                                  ,[CoinInAct]
                                  ,[CoinInAnt]
                                  ,[Toquen]
                                  ,[CoinOut]
                                  ,[Jackpot]
                                  ,[Cancelcredits]
                                  ,[Billetero]
                                  ,[BonusWinAct]
                                  ,[BonusWinAnt]
                                  ,[CreditAct]
                                  ,[CreditAnt]
                                  ,[NroJugadores]
                                  ,[ValorReal]
                                  ,[NroJugada]
                                  ,[Pagado]
                                  ,[FechaPago]
                                  ,[indice]
                                  ,[desc_pozo]
                                  ,[desc_estado]
                                  ,[desc_modelo]
                                  ,[desc_marca]
                                  ,[desc_imagen_progresivo]
                                  ,[cod_imagen_progresivo]
                                  ,[Cantidad]
                                  ,[desc_fecha]
                                  ,[desc_hora_pago]
                                  ,[codalterno]
                                  ,[File]
                              FROM [PRO_Cabecera]";
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
                            var webProgresivo = new CabeceraOfflineEntidad
                            {
                                IdCabeceraProgresivo = ManejoNulos.ManageNullInteger(dr["IdCabeceraProgresivo"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodProgresivo = ManejoNulos.ManageNullInteger(dr["CodProgresivo"]),
                                ProgresivoID = ManejoNulos.ManageNullInteger(dr["ProgresivoID"]),
                                DetalleProgresivoID = ManejoNulos.ManageNullInteger(dr["DetalleProgresivoID"]),
                                ProcesosID = ManejoNulos.ManageNullStr(dr["ProcesosID"]),
                                GanadorID = ManejoNulos.ManageNullInteger(dr["GanadorID"]),
                                SlotID = ManejoNulos.ManageNullStr(dr["SlotID"]),
                                Monto = ManejoNulos.ManageNullDouble(dr["Monto"]),
                                Valor = ManejoNulos.ManageNullDouble(dr["Valor"]),
                                TipoPozo = ManejoNulos.ManageNullInteger(dr["TipoPozo"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CoinInAct = ManejoNulos.ManageNullDouble(dr["CoinInAct"]),
                                CoinInAnt = ManejoNulos.ManageNullDouble(dr["CoinInAnt"]),
                                Toquen = ManejoNulos.ManageNullDouble(dr["Toquen"]),
                                CoinOut = ManejoNulos.ManageNullDouble(dr["CoinOut"]),
                                Jackpot = ManejoNulos.ManageNullDouble(dr["Jackpot"]),
                                Cancelcredits = ManejoNulos.ManageNullDouble(dr["Cancelcredits"]),
                                Billetero = ManejoNulos.ManageNullDouble(dr["Billetero"]),
                                BonusWinAct = ManejoNulos.ManageNullDouble(dr["BonusWinAct"]),
                                BonusWinAnt = ManejoNulos.ManageNullDouble(dr["BonusWinAnt"]),
                                CreditAct = ManejoNulos.ManageNullDouble(dr["CreditAct"]),
                                CreditAnt = ManejoNulos.ManageNullDouble(dr["CreditAnt"]),
                                NroJugadores = ManejoNulos.ManageNullInteger(dr["NroJugadores"]),
                                ValorReal = ManejoNulos.ManageNullDouble(dr["ValorReal"]),
                                NroJugada = ManejoNulos.ManageNullDouble(dr["NroJugada"]),
                                Pagado = ManejoNulos.ManageNullDouble(dr["Pagado"]),
                                FechaPago = ManejoNulos.ManageNullDate(dr["FechaPago"]),
                                indice = ManejoNulos.ManageNullInteger(dr["indice"]),
                                desc_pozo = ManejoNulos.ManageNullStr(dr["desc_pozo"]),
                                desc_estado = ManejoNulos.ManageNullStr(dr["desc_estado"]),
                                desc_modelo = ManejoNulos.ManageNullStr(dr["desc_modelo"]),
                                desc_marca = ManejoNulos.ManageNullStr(dr["desc_marca"]),
                                desc_imagen_progresivo = ManejoNulos.ManageNullStr(dr["desc_imagen_progresivo"]),
                                cod_imagen_progresivo = ManejoNulos.ManageNullInteger(dr["cod_imagen_progresivo"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                desc_fecha = ManejoNulos.ManageNullStr(dr["desc_fecha"]),
                                desc_hora_pago = ManejoNulos.ManageNullStr(dr["desc_hora_pago"]),
                                codalterno = ManejoNulos.ManageNullStr(dr["codalterno"]),
                                File = ManejoNulos.ManageNullStr(dr["File"]),
                            };
                            lista.Add(webProgresivo);
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

        public List<DetalleOfflineEntidad> GetDetalleOfflinexSalaxProgresivo(int codSala, int codProgresivo, int codCabecera)
        {
            List<DetalleOfflineEntidad> lista = new List<DetalleOfflineEntidad>();
            string consulta = @"SELECT [IdDetalleProgresivo]
                                  ,[IdCabeceraProgresivo]
                                  ,[CodMaq]
                                  ,[codevento]
                                  ,[Bonus1]
                                  ,[Bonus2]
                                  ,[Dif_Bonus1]
                                  ,[Dif_Bonus2]
                                  ,[CurrentCredits]
                                  ,[Fecha]
                                  ,[Hora]
                                  ,[FechaCompleta]
                              FROM [PRO_Detalle] WHERE CodSala=@pCodSala AND CodProgresivo=@pCodProgresivo AND IdCabeceraProgresivo=@pIdCabeceraProgresivo order by Fecha Desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodSala", codSala);
                    query.Parameters.AddWithValue("@pCodProgresivo", codProgresivo);
                    query.Parameters.AddWithValue("@pIdCabeceraProgresivo", codCabecera);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var webProgresivo = new DetalleOfflineEntidad
                            {
                                IdDetalleProgresivo = ManejoNulos.ManageNullInteger(dr["IdDetalleProgresivo"]),
                                IdCabeceraProgresivo = ManejoNulos.ManageNullInteger(dr["IdCabeceraProgresivo"]),
                                CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]),
                                codevento = ManejoNulos.ManageNullDouble(dr["codevento"]),
                                Bonus1 = ManejoNulos.ManageNullDouble(dr["Bonus1"]),
                                Bonus2 = ManejoNulos.ManageNullDouble(dr["Bonus2"]),
                                Dif_Bonus1 = ManejoNulos.ManageNullDouble(dr["Dif_Bonus1"]),
                                Dif_Bonus2 = ManejoNulos.ManageNullDouble(dr["Dif_Bonus2"]),
                                CurrentCredits = ManejoNulos.ManageNullDouble(dr["CurrentCredits"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Hora = ManejoNulos.ManageNullDate(dr["Hora"]),
                                FechaCompleta = ManejoNulos.ManageNullDate(dr["FechaCompleta"]),
                            };
                            lista.Add(webProgresivo);
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
        public bool DetalleOfflineInsertarJson(DetalleOfflineEntidad obj)
        {
            string consulta = @"INSERT INTO [dbo].[PRO_Detalle]
                                   ([CodMaq]
                                   ,[CodSala]
                                   ,[CodProgresivo]
                                   ,[IdCabeceraProgresivo]
                                   ,[codevento]
                                   ,[Bonus1]
                                   ,[Bonus2]
                                   ,[Dif_Bonus1]
                                   ,[Dif_Bonus2]
                                   ,[CurrentCredits]
                                   ,[Fecha]
                                   ,[Hora]
                                   ,[FechaCompleta])
                             VALUES
                                   (@pCodMaq
                                   ,@pCodSala
                                   ,@pCodProgresivo
                                   ,@pIdCabeceraProgresivo
                                   ,@pcodevento
                                   ,@pBonus1
                                   ,@pBonus2
                                   ,@pDif_Bonus1
                                   ,@pDif_Bonus2
                                   ,@pCurrentCredits 
                                   ,@pFecha 
                                   ,@pHora 
                                   ,@pFechaCompleta)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodMaq", ManejoNulos.ManageNullInteger(obj.CodMaq));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(0));
                    query.Parameters.AddWithValue("@pCodProgresivo", ManejoNulos.ManageNullInteger(0));
                    query.Parameters.AddWithValue("@pIdCabeceraProgresivo", ManejoNulos.ManageNullInteger(obj.IdCabeceraProgresivo));
                    query.Parameters.AddWithValue("@pcodevento", ManejoNulos.ManageNullInteger(obj.codevento));
                    query.Parameters.AddWithValue("@pBonus1", obj.Bonus1);
                    query.Parameters.AddWithValue("@pBonus2", obj.Bonus2);
                    query.Parameters.AddWithValue("@pDif_Bonus1", obj.Dif_Bonus1);
                    query.Parameters.AddWithValue("@pDif_Bonus2", obj.Dif_Bonus2);
                    query.Parameters.AddWithValue("@pCurrentCredits ", obj.CurrentCredits);
                    query.Parameters.AddWithValue("@pFecha ", obj.Fecha);
                    query.Parameters.AddWithValue("@pHora ", obj.Hora);
                    query.Parameters.AddWithValue("@pFechaCompleta ", obj.FechaCompleta);
                    query.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public List<DetalleOfflineEntidad> GetDetalleOffline()
        {
            List<DetalleOfflineEntidad> lista = new List<DetalleOfflineEntidad>();
            string consulta = @"SELECT [IdDetalleProgresivo]
                                      ,[IdCabeceraProgresivo]
                                      ,[CodMaq]
                                      ,[codevento]
                                      ,[Bonus1]
                                      ,[Bonus2]
                                      ,[Dif_Bonus1]
                                      ,[Dif_Bonus2]
                                      ,[CurrentCredits]
                                      ,[Fecha]
                                      ,[Hora]
                                      ,[FechaCompleta]
                                  FROM [PRO_Detalle]";
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
                            var webProgresivo = new DetalleOfflineEntidad
                            {
                                IdDetalleProgresivo = ManejoNulos.ManageNullInteger(dr["IdDetalleProgresivo"]),
                                IdCabeceraProgresivo = ManejoNulos.ManageNullInteger(dr["IdCabeceraProgresivo"]),
                                CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]),
                                codevento = ManejoNulos.ManageNullDouble(dr["codevento"]),
                                Bonus1 = ManejoNulos.ManageNullDouble(dr["Bonus1"]),
                                Bonus2 = ManejoNulos.ManageNullDouble(dr["Bonus2"]),
                                Dif_Bonus1 = ManejoNulos.ManageNullDouble(dr["Dif_Bonus1"]),
                                Dif_Bonus2 = ManejoNulos.ManageNullDouble(dr["Dif_Bonus2"]),
                                CurrentCredits = ManejoNulos.ManageNullDouble(dr["CurrentCredits"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Hora = ManejoNulos.ManageNullDate(dr["Hora"]),
                                FechaCompleta = ManejoNulos.ManageNullDate(dr["FechaCompleta"]),
                            };
                            lista.Add(webProgresivo);
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
        public List<DetalleOfflineEntidad> GetDetalleOfflinexCabecera(int codCabecera)
        {
            List<DetalleOfflineEntidad> lista = new List<DetalleOfflineEntidad>();
            string consulta = @"SELECT [IdDetalleProgresivo]
                                      ,[IdCabeceraProgresivo]
                                      ,[CodMaq]
                                      ,[codevento]
                                      ,[Bonus1]
                                      ,[Bonus2]
                                      ,[Dif_Bonus1]
                                      ,[Dif_Bonus2]
                                      ,[CurrentCredits]
                                      ,[Fecha]
                                      ,[Hora]
                                      ,[FechaCompleta]
                                  FROM [PRO_Detalle] WHERE IdCabeceraProgresivo=@pCodCabecera ORDER BY FechaCompleta ASC";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodCabecera", codCabecera);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var webProgresivo = new DetalleOfflineEntidad
                            {
                                IdDetalleProgresivo = ManejoNulos.ManageNullInteger(dr["IdDetalleProgresivo"]),
                                IdCabeceraProgresivo = ManejoNulos.ManageNullInteger(dr["IdCabeceraProgresivo"]),
                                CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]),
                                codevento = ManejoNulos.ManageNullDouble(dr["codevento"]),
                                Bonus1 = ManejoNulos.ManageNullDouble(dr["Bonus1"]),
                                Bonus2 = ManejoNulos.ManageNullDouble(dr["Bonus2"]),
                                Dif_Bonus1 = ManejoNulos.ManageNullDouble(dr["Dif_Bonus1"]),
                                Dif_Bonus2 = ManejoNulos.ManageNullDouble(dr["Dif_Bonus2"]),
                                CurrentCredits = ManejoNulos.ManageNullDouble(dr["CurrentCredits"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Hora = ManejoNulos.ManageNullDate(dr["Hora"]),
                                FechaCompleta = ManejoNulos.ManageNullDate(dr["FechaCompleta"]),
                            };
                            lista.Add(webProgresivo);
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

        public List<DetalleOfflineEntidad> DetallesContadoresPremio(string id, string id2, int id3, int id4)
        {
            List<DetalleOfflineEntidad> lista = new List<DetalleOfflineEntidad>();
            string consulta = @"
                    
                    declare @codonline bigint

                    select top 1 @codonline= isnull(IdDetalleProgresivo,0) from [PRO_Detalle] nolock
                    where CodMaq=" + id2 + @" and Fecha>='" + id + @"' 

                    if(@codonline is not null)
                    begin 
                    exec ('select top '+'" + id3 + @"'+'     convert(date,fecha) fecha,convert(varchar(10),hora,108) Hora,
		                    CodMaq,codevento,Bonus1,Bonus2  from [PRO_Detalle] nolock
		                    where IdDetalleProgresivo<'+@codonline +' and codmaq='+'" + id2 + @"'+
		                    ' union 
		                    select   convert(date,fecha) fecha,convert(varchar(10),hora,108) Hora,
		                    CodMaq,codevento,Bonus1,Bonus2  from [PRO_Detalle] nolock
		                    where IdDetalleProgresivo= '+@codonline+
		                    'union
		                    select top '+'" + id4 + @"'+'     convert(date,fecha) fecha,convert(varchar(10),hora,108) Hora,
		                    CodMaq,codevento,Bonus1,Bonus2  from [PRO_Detalle] nolock
		                    where IdDetalleProgresivo>'+@codonline+' and codmaq='+'" + id2 + @"'+
		                    'order by 1,2 desc'
		                    )
                    end";
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
                            var webProgresivo = new DetalleOfflineEntidad
                            {
                                CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]),
                                codevento = ManejoNulos.ManageNullDouble(dr["codevento"]),
                                Bonus1 = ManejoNulos.ManageNullDouble(dr["Bonus1"]),
                                Bonus2 = ManejoNulos.ManageNullDouble(dr["Bonus2"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Hora = ManejoNulos.ManageNullDate(dr["Hora"]),
                            };
                            lista.Add(webProgresivo);
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
        
        public List<CabeceraOfflineEntidad>ListarCabecerasPorFechaYSala(DateTime fechaInicio,DateTime fechaFin, string listaSalas) {
            List<CabeceraOfflineEntidad> lista = new List<CabeceraOfflineEntidad>();
            string consulta = @"SELECT [IdCabeceraProgresivo],[CodSala]
                                  ,[CodProgresivo]
                                  ,[ProgresivoID]
                                  ,[DetalleProgresivoID]
                                  ,[ProcesosID]
                                  ,[GanadorID]
                                  ,[SlotID]
                                  ,[Monto]
                                  ,[Valor]
                                  ,[TipoPozo]
                                  ,[Fecha]
                                  ,[Estado]
                                  ,[CoinInAct]
                                  ,[CoinInAnt]
                                  ,[Toquen]
                                  ,[CoinOut]
                                  ,[Jackpot]
                                  ,[Cancelcredits]
                                  ,[Billetero]
                                  ,[BonusWinAct]
                                  ,[BonusWinAnt]
                                  ,[CreditAct]
                                  ,[CreditAnt]
                                  ,[NroJugadores]
                                  ,[ValorReal]
                                  ,[NroJugada]
                                  ,[Pagado]
                                  ,[FechaPago]
                                  ,[indice]
                                  ,[desc_pozo]
                                  ,[desc_estado]
                                  ,[desc_modelo]
                                  ,[desc_marca]
                                  ,[desc_imagen_progresivo]
                                  ,[cod_imagen_progresivo]
                                  ,[Cantidad]
                                  ,[desc_fecha]
                                  ,[desc_hora_pago]
                                  ,[codalterno]
                                  ,[File]
                              FROM [PRO_Cabecera] where convert(datetime,fecha) between convert(datetime,@fechaInicio) and convert(datetime,@fechaFin) " + listaSalas;
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    query.Parameters.AddWithValue("@fechaFin", fechaFin);    
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var webProgresivo = new CabeceraOfflineEntidad {
                                IdCabeceraProgresivo = ManejoNulos.ManageNullInteger(dr["IdCabeceraProgresivo"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodProgresivo = ManejoNulos.ManageNullInteger(dr["CodProgresivo"]),
                                ProgresivoID = ManejoNulos.ManageNullInteger(dr["ProgresivoID"]),
                                DetalleProgresivoID = ManejoNulos.ManageNullInteger(dr["DetalleProgresivoID"]),
                                ProcesosID = ManejoNulos.ManageNullStr(dr["ProcesosID"]),
                                GanadorID = ManejoNulos.ManageNullInteger(dr["GanadorID"]),
                                SlotID = ManejoNulos.ManageNullStr(dr["SlotID"]),
                                Monto = ManejoNulos.ManageNullDouble(dr["Monto"]),
                                Valor = ManejoNulos.ManageNullDouble(dr["Valor"]),
                                TipoPozo = ManejoNulos.ManageNullInteger(dr["TipoPozo"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CoinInAct = ManejoNulos.ManageNullDouble(dr["CoinInAct"]),
                                CoinInAnt = ManejoNulos.ManageNullDouble(dr["CoinInAnt"]),
                                Toquen = ManejoNulos.ManageNullDouble(dr["Toquen"]),
                                CoinOut = ManejoNulos.ManageNullDouble(dr["CoinOut"]),
                                Jackpot = ManejoNulos.ManageNullDouble(dr["Jackpot"]),
                                Cancelcredits = ManejoNulos.ManageNullDouble(dr["Cancelcredits"]),
                                Billetero = ManejoNulos.ManageNullDouble(dr["Billetero"]),
                                BonusWinAct = ManejoNulos.ManageNullDouble(dr["BonusWinAct"]),
                                BonusWinAnt = ManejoNulos.ManageNullDouble(dr["BonusWinAnt"]),
                                CreditAct = ManejoNulos.ManageNullDouble(dr["CreditAct"]),
                                CreditAnt = ManejoNulos.ManageNullDouble(dr["CreditAnt"]),
                                NroJugadores = ManejoNulos.ManageNullInteger(dr["NroJugadores"]),
                                ValorReal = ManejoNulos.ManageNullDouble(dr["ValorReal"]),
                                NroJugada = ManejoNulos.ManageNullDouble(dr["NroJugada"]),
                                Pagado = ManejoNulos.ManageNullDouble(dr["Pagado"]),
                                FechaPago = ManejoNulos.ManageNullDate(dr["FechaPago"]),
                                indice = ManejoNulos.ManageNullInteger(dr["indice"]),
                                desc_pozo = ManejoNulos.ManageNullStr(dr["desc_pozo"]),
                                desc_estado = ManejoNulos.ManageNullStr(dr["desc_estado"]),
                                desc_modelo = ManejoNulos.ManageNullStr(dr["desc_modelo"]),
                                desc_marca = ManejoNulos.ManageNullStr(dr["desc_marca"]),
                                desc_imagen_progresivo = ManejoNulos.ManageNullStr(dr["desc_imagen_progresivo"]),
                                cod_imagen_progresivo = ManejoNulos.ManageNullInteger(dr["cod_imagen_progresivo"]),
                                Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]),
                                desc_fecha = ManejoNulos.ManageNullStr(dr["desc_fecha"]),
                                desc_hora_pago = ManejoNulos.ManageNullStr(dr["desc_hora_pago"]),
                                codalterno = ManejoNulos.ManageNullStr(dr["codalterno"]),
                                File = ManejoNulos.ManageNullStr(dr["File"]),
                            };
                            lista.Add(webProgresivo);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return lista;
        }
        #endregion



        #region Metodos Para Migrar Data DIEGO - AsignacionMT - DetalleContadoresGame

        public List<Asignacion_M_T> GetAsignacion_M_T()
        {
            List<Asignacion_M_T> lista = new List<Asignacion_M_T>();
            string consulta = @"SELECT [COD_EMPRESA]
                                  ,[COD_SALA]
                                  ,[nro]
                                  ,[CodTarjeta]
                                  ,[CodTarjeta_Seg]
                                  ,[CodMaq]
                                  ,[CodMaqMin]
                                  ,[Modelo]
                                  ,[COD_TIPOMAQUINA]
                                  ,[Posicion]
                                  ,[Tpro]
                                  ,[Estado]
                                  ,[CodFicha]
                                  ,[CodMarca]
                                  ,[CodModelo]
                                  ,[Hopper]
                                  ,[CredOtor]
                                  ,[Token]
                                  ,[NroContradores]
                                  ,[Precio_Credito]
                                  ,[NUM_SERIE]
                                  ,[idTipoMoneda]
                                  ,[MODALIDAD]
                                  ,[MAQ_X]
                                  ,[MAQ_Y]
                                  ,[MAQ_ASIGLAYOUT]
                                  ,[MAQ_POSILAYOUT]
                                  ,[CIn]
                                  ,[COut]
                                  ,[TipoTranSac]
                                  ,[cod_caja]
                                  ,[TopeCreditos]
                                  ,[S_ONLINE]
                                  ,[FORMULA_MAQ]
                                  ,[MAQ_DEVPORCENTAJE]
                                  ,[FormulaFinal]
                                  ,[Dbase]
                                  ,[EnviaDbase]
                                  ,[cod_servidor]
                                  ,[Cod_Socio]
                                  ,[Status_Online]
                                  ,[COD_MODELO]
                                  ,[Sistema]
                                  ,[PosicionBilletero]
                                  ,[con_sorteo]
                                  ,[Block]
                                  ,[codigo_extra]
                                  ,[PromoBonus]
                                  ,[PromoTicket]
                                  ,[estadoT]
                                  ,[retiroTemporal]
                              FROM [dbo].[PRO_Asignacion_M_T]
                            ";
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
                            var item = new Asignacion_M_T
                            {
                                COD_EMPRESA = ManejoNulos.ManageNullStr(dr["COD_EMPRESA"]),
                                COD_SALA = ManejoNulos.ManageNullStr(dr["COD_SALA"]),
                                nro = ManejoNulos.ManageNullInteger(dr["nro"]),
                                CodTarjeta = ManejoNulos.ManageNullStr(dr["CodTarjeta"]),
                                CodTarjeta_Seg = ManejoNulos.ManageNullInteger(dr["CodTarjeta_Seg"]),
                                CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]),
                                CodMaqMin = ManejoNulos.ManageNullStr(dr["CodMaqMin"]),
                                Modelo = ManejoNulos.ManageNullStr(dr["Modelo"]),
                                COD_TIPOMAQUINA = ManejoNulos.ManageNullInteger(dr["COD_TIPOMAQUINA"]),
                                Posicion = ManejoNulos.ManageNullInteger(dr["Posicion"]),
                                Tpro = ManejoNulos.ManageNullInteger(dr["Tpro"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CodFicha = ManejoNulos.ManageNullInteger(dr["CodFicha"]),
                                CodMarca = ManejoNulos.ManageNullInteger(dr["CodMarca"]),
                                CodModelo = ManejoNulos.ManageNullInteger(dr["CodModelo"]),
                                Hopper = ManejoNulos.ManageNullInteger(dr["Hopper"]),
                                CredOtor = ManejoNulos.ManageNullInteger(dr["CredOtor"]),
                                Token = ManejoNulos.ManageNullDouble(dr["Token"]),
                                NroContradores = ManejoNulos.ManageNullInteger(dr["NroContradores"]),
                                Precio_Credito = ManejoNulos.ManageNullDouble(dr["Precio_Credito"]),
                                NUM_SERIE = ManejoNulos.ManageNullStr(dr["NUM_SERIE"]),
                                idTipoMoneda = ManejoNulos.ManageNullInteger(dr["idTipoMoneda"]),
                                MODALIDAD = ManejoNulos.ManageNullInteger(dr["MODALIDAD"]),
                                MAQ_X = ManejoNulos.ManageNullInteger(dr["MAQ_X"]),
                                MAQ_Y = ManejoNulos.ManageNullInteger(dr["MAQ_Y"]),
                                MAQ_ASIGLAYOUT = ManejoNulos.ManageNullStr(dr["MAQ_ASIGLAYOUT"]),
                                MAQ_POSILAYOUT = ManejoNulos.ManageNullInteger(dr["MAQ_POSILAYOUT"]),
                                CIn = ManejoNulos.ManageNullInteger(dr["CIn"]),
                                COut = ManejoNulos.ManageNullInteger(dr["COut"]),
                                TipoTranSac = ManejoNulos.ManageNullInteger(dr["TipoTranSac"]),
                                cod_caja = ManejoNulos.ManageNullInteger(dr["cod_caja"]),
                                TopeCreditos = ManejoNulos.ManageNullInteger(dr["TopeCreditos"]),
                                S_ONLINE = ManejoNulos.ManageNullInteger(dr["S_ONLINE"]),
                                FORMULA_MAQ = ManejoNulos.ManageNullStr(dr["FORMULA_MAQ"]),
                                MAQ_DEVPORCENTAJE = ManejoNulos.ManageNullDouble(dr["MAQ_DEVPORCENTAJE"]),
                                FormulaFinal = ManejoNulos.ManageNullStr(dr["FormulaFinal"]),
                                Dbase = ManejoNulos.ManageNullStr(dr["Dbase"]),
                                EnviaDbase = ManejoNulos.ManegeNullBool(dr["EnviaDbase"]),
                                cod_servidor = ManejoNulos.ManageNullInteger(dr["cod_servidor"]),
                                Cod_Socio = ManejoNulos.ManageNullInteger(dr["Cod_Socio"]),
                                Status_Online = ManejoNulos.ManageNullInteger(dr["Status_Online"]),
                                COD_MODELO = ManejoNulos.ManageNullInteger(dr["COD_MODELO"]),
                                Sistema = ManejoNulos.ManageNullInteger(dr["Sistema"]),
                                PosicionBilletero = ManejoNulos.ManageNullStr(dr["PosicionBilletero"]),
                                con_sorteo = ManejoNulos.ManageNullInteger(dr["con_sorteo"]),
                                Block = ManejoNulos.ManageNullInteger(dr["Block"]),
                                codigo_extra = ManejoNulos.ManageNullStr(dr["codigo_extra"]),
                                PromoBonus = ManejoNulos.ManageNullInteger(dr["PromoBonus"]),
                                PromoTicket = ManejoNulos.ManageNullInteger(dr["PromoTicket"]),
                                estadoT = ManejoNulos.ManageNullInteger(dr["estadoT"]),
                                retirotemporal = ManejoNulos.ManageNullInteger(dr["retiroTemporal"]),
                            };
                            lista.Add(item);
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

        public List<DetalleContadoresGame> GetDetalleContadoresGame()
        {
            List<DetalleContadoresGame> lista = new List<DetalleContadoresGame>();
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
                              FROM [dbo].[PRO_DetalleContadoresGame]";
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
                            var item = new DetalleContadoresGame
                            {
                                CodDetalleContadoresGame = ManejoNulos.ManageNullInteger(dr["CodDetalleContadoresGame"]),
                                CodContadoresGame = ManejoNulos.ManageNullInteger(dr["CodContadoresGame"]),
                                CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodEmpresa = ManejoNulos.ManageNullInteger(dr["CodEmpresa"]),
                                CodMoneda = ManejoNulos.ManageNullInteger(dr["CodMoneda"]),
                                FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"]),
                                CoinInIni = ManejoNulos.ManageNullDouble(dr["CoinInIni"]),
                                CoinInFin = ManejoNulos.ManageNullDouble(dr["CoinInFin"]),
                                CoinOutIni = ManejoNulos.ManageNullDouble(dr["CoinOutIni"]),
                                CoinOutFin = ManejoNulos.ManageNullDouble(dr["CoinOutFin"]),
                                JackpotIni = ManejoNulos.ManageNullDouble(dr["JackpotIni"]),
                                JackpotFin = ManejoNulos.ManageNullDouble(dr["JackpotFin"]),
                                HandPayIni = ManejoNulos.ManageNullDouble(dr["HandPayIni"]),
                                HandPayFin = ManejoNulos.ManageNullDouble(dr["HandPayFin"]),
                                CancelCreditIni = ManejoNulos.ManageNullDouble(dr["CancelCreditIni"]),
                                CancelCreditFin = ManejoNulos.ManageNullDouble(dr["CancelCreditFin"]),
                                GamesPlayedIni = ManejoNulos.ManageNullDouble(dr["GamesPlayedIni"]),
                                GamesPlayedFin = ManejoNulos.ManageNullDouble(dr["GamesPlayedFin"]),
                                ProduccionPorSlot1 = ManejoNulos.ManageNullDouble(dr["ProduccionPorSlot1"]),
                                ProduccionPorSlot2Reset = ManejoNulos.ManageNullDouble(dr["ProduccionPorSlot2Reset"]),
                                ProduccionPorSlot3Rollover = ManejoNulos.ManageNullDouble(dr["ProduccionPorSlot3Rollover"]),
                                ProduccionPorSlot4Prueba = ManejoNulos.ManageNullDouble(dr["ProduccionPorSlot4Prueba"]),
                                ProduccionTotalPorSlot5Dia = ManejoNulos.ManageNullDouble(dr["ProduccionTotalPorSlot5Dia"]),
                                TipoCambio = ManejoNulos.ManageNullDouble(dr["TipoCambio"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManegeNullBool(dr["Estado"]),
                                SaldoCoinIn = ManejoNulos.ManageNullDouble(dr["SaldoCoinIn"]),
                                SaldoCoinOut = ManejoNulos.ManageNullDouble(dr["SaldoCoinOut"]),
                                SaldoJackpot = ManejoNulos.ManageNullDouble(dr["SaldoJackpot"]),
                                SaldoGamesPlayed = ManejoNulos.ManageNullDouble(dr["SaldoGamesPlayed"]),
                                CodUsuario = ManejoNulos.ManageNullInteger(dr["CodUsuario"]),
                                RetiroTemporal = ManejoNulos.ManageNullInteger(dr["RetiroTemporal"]),
                                TiempoJuego = ManejoNulos.ManageNullDouble(dr["TiempoJuego"]),
                            };
                            lista.Add(item);
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


        public int Asignacion_M_TInsertarJson(Asignacion_M_T obj)
        {
            int idInsertado = 0;

            string consulta = @"INSERT INTO [dbo].[PRO_Asignacion_M_T]
                                   ([COD_EMPRESA]
                                   ,[COD_SALA]
                                   ,[nro]
                                   ,[CodTarjeta]
                                   ,[CodTarjeta_Seg]
                                   ,[CodMaq]
                                   ,[CodMaqMin]
                                   ,[Modelo]
                                   ,[COD_TIPOMAQUINA]
                                   ,[Posicion]
                                   ,[Tpro]
                                   ,[Estado]
                                   ,[CodFicha]
                                   ,[CodMarca]
                                   ,[CodModelo]
                                   ,[Hopper]
                                   ,[CredOtor]
                                   ,[Token]
                                   ,[NroContradores]
                                   ,[Precio_Credito]
                                   ,[NUM_SERIE]
                                   ,[idTipoMoneda]
                                   ,[MODALIDAD]
                                   ,[MAQ_X]
                                   ,[MAQ_Y]
                                   ,[MAQ_ASIGLAYOUT]
                                   ,[MAQ_POSILAYOUT]
                                   ,[CIn]
                                   ,[COut]
                                   ,[TipoTranSac]
                                   ,[cod_caja]
                                   ,[TopeCreditos]
                                   ,[S_ONLINE]
                                   ,[FORMULA_MAQ]
                                   ,[MAQ_DEVPORCENTAJE]
                                   ,[FormulaFinal]
                                   ,[Dbase]
                                   ,[EnviaDbase]
                                   ,[cod_servidor]
                                   ,[Cod_Socio]
                                   ,[Status_Online]
                                   ,[COD_MODELO]
                                   ,[Sistema]
                                   ,[PosicionBilletero]
                                   ,[con_sorteo]
                                   ,[Block]
                                   ,[codigo_extra]
                                   ,[PromoBonus]
                                   ,[PromoTicket]
                                   ,[estadoT]
                                   ,[retiroTemporal])
                             OUTPUT Inserted.Cod_Asignacion_M_T
                             VALUES
                                   (@pCOD_EMPRESA
                                   ,@pCOD_SALA
                                   ,@pnro
                                   ,@pCodTarjeta
                                   ,@pCodTarjeta_Seg
                                   ,@pCodMaq
                                   ,@pCodMaqMin
                                   ,@pModelo
                                   ,@pCOD_TIPOMAQUINA
                                   ,@pPosicion
                                   ,@pTpro
                                   ,@pEstado
                                   ,@pCodFicha
                                   ,@pCodMarca
                                   ,@pCodModelo
                                   ,@pHopper
                                   ,@pCredOtor
                                   ,@pToken
                                   ,@pNroContradores
                                   ,@pPrecio_Credito
                                   ,@pNUM_SERIE
                                   ,@pidTipoMoneda
                                   ,@pMODALIDAD
                                   ,@pMAQ_X
                                   ,@pMAQ_Y
                                   ,@pMAQ_ASIGLAYOUT
                                   ,@pMAQ_POSILAYOUT
                                   ,@pCIn
                                   ,@pCOut
                                   ,@pTipoTranSac
                                   ,@pcod_caja
                                   ,@pTopeCreditos
                                   ,@pS_ONLINE
                                   ,@pFORMULA_MAQ
                                   ,@pMAQ_DEVPORCENTAJE
                                   ,@pFormulaFinal
                                   ,@pDbase
                                   ,@pEnviaDbase
                                   ,@pcod_servidor
                                   ,@pCod_Socio
                                   ,@pStatus_Online
                                   ,@pCOD_MODELO
                                   ,@pSistema
                                   ,@pPosicionBilletero
                                   ,@pcon_sorteo
                                   ,@pBlock
                                   ,@pcodigo_extra
                                   ,@pPromoBonus
                                   ,@pPromoTicket
                                   ,@pestadoT
                                   ,@pretiroTemporal)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCOD_EMPRESA", ManejoNulos.ManageNullStr(obj.COD_EMPRESA));
                    query.Parameters.AddWithValue("@pCOD_SALA", ManejoNulos.ManageNullStr(obj.COD_SALA));
                    query.Parameters.AddWithValue("@pnro", ManejoNulos.ManageNullInteger(obj.nro));
                    query.Parameters.AddWithValue("@pCodTarjeta", ManejoNulos.ManageNullStr(obj.CodTarjeta));
                    query.Parameters.AddWithValue("@pCodTarjeta_Seg", ManejoNulos.ManageNullInteger(obj.CodTarjeta_Seg));
                    query.Parameters.AddWithValue("@pCodMaq", ManejoNulos.ManageNullStr(obj.CodMaq));
                    query.Parameters.AddWithValue("@pCodMaqMin ", ManejoNulos.ManageNullStr(obj.CodMaqMin));
                    query.Parameters.AddWithValue("@pModelo ", ManejoNulos.ManageNullStr(obj.Modelo));
                    query.Parameters.AddWithValue("@pCOD_TIPOMAQUINA ", ManejoNulos.ManageNullInteger(obj.COD_TIPOMAQUINA));
                    query.Parameters.AddWithValue("@pPosicion ", ManejoNulos.ManageNullInteger(obj.Posicion));
                    query.Parameters.AddWithValue("@pTpro ", ManejoNulos.ManageNullInteger(obj.Tpro));
                    query.Parameters.AddWithValue("@pEstado ", ManejoNulos.ManageNullInteger(obj.Estado));
                    query.Parameters.AddWithValue("@pCodFicha ", ManejoNulos.ManageNullInteger(obj.CodFicha));
                    query.Parameters.AddWithValue("@pCodMarca ", ManejoNulos.ManageNullInteger(obj.CodMarca));
                    query.Parameters.AddWithValue("@pCodModelo ", ManejoNulos.ManageNullInteger(obj.CodModelo));
                    query.Parameters.AddWithValue("@pHopper ", ManejoNulos.ManageNullInteger(obj.Hopper));
                    query.Parameters.AddWithValue("@pCredOtor ", ManejoNulos.ManageNullInteger(obj.CredOtor));
                    query.Parameters.AddWithValue("@pToken ", ManejoNulos.ManageNullDouble(obj.Token));
                    query.Parameters.AddWithValue("@pNroContradores ", ManejoNulos.ManageNullInteger(obj.NroContradores));
                    query.Parameters.AddWithValue("@pPrecio_Credito ", ManejoNulos.ManageNullDouble(obj.Precio_Credito));
                    query.Parameters.AddWithValue("@pNUM_SERIE ", ManejoNulos.ManageNullStr(obj.NUM_SERIE));
                    query.Parameters.AddWithValue("@pidTipoMoneda ", ManejoNulos.ManageNullInteger(obj.idTipoMoneda));
                    query.Parameters.AddWithValue("@pMODALIDAD ", ManejoNulos.ManageNullInteger(obj.MODALIDAD));
                    query.Parameters.AddWithValue("@pMAQ_X ", ManejoNulos.ManageNullInteger(obj.MAQ_X));
                    query.Parameters.AddWithValue("@pMAQ_Y ", ManejoNulos.ManageNullInteger(obj.MAQ_Y));
                    query.Parameters.AddWithValue("@pMAQ_ASIGLAYOUT ", ManejoNulos.ManageNullStr(obj.MAQ_ASIGLAYOUT));
                    query.Parameters.AddWithValue("@pMAQ_POSILAYOUT ", ManejoNulos.ManageNullInteger(obj.MAQ_POSILAYOUT));
                    query.Parameters.AddWithValue("@pCIn ", ManejoNulos.ManageNullInteger(obj.CIn));
                    query.Parameters.AddWithValue("@pCOut ", ManejoNulos.ManageNullInteger(obj.COut));
                    query.Parameters.AddWithValue("@pTipoTranSac ", ManejoNulos.ManageNullInteger(obj.TipoTranSac));
                    query.Parameters.AddWithValue("@pcod_caja ", ManejoNulos.ManageNullInteger(obj.cod_caja));
                    query.Parameters.AddWithValue("@pTopeCreditos ", ManejoNulos.ManageNullInteger(obj.TopeCreditos));
                    query.Parameters.AddWithValue("@pS_ONLINE", ManejoNulos.ManageNullInteger(obj.S_ONLINE));
                    query.Parameters.AddWithValue("@pFORMULA_MAQ", ManejoNulos.ManageNullStr(obj.FORMULA_MAQ));
                    query.Parameters.AddWithValue("@pMAQ_DEVPORCENTAJE ", ManejoNulos.ManageNullDouble(obj.MAQ_DEVPORCENTAJE));
                    query.Parameters.AddWithValue("@pFormulaFinal ", ManejoNulos.ManageNullStr(obj.FormulaFinal));
                    query.Parameters.AddWithValue("@pDbase ", ManejoNulos.ManageNullStr(obj.Dbase));
                    query.Parameters.AddWithValue("@pEnviaDbase ", ManejoNulos.ManegeNullBool(obj.EnviaDbase));
                    query.Parameters.AddWithValue("@pcod_servidor ", ManejoNulos.ManageNullInteger(obj.cod_servidor));
                    query.Parameters.AddWithValue("@pCod_Socio", ManejoNulos.ManageNullInteger(obj.Cod_Socio));
                    query.Parameters.AddWithValue("@pStatus_Online", ManejoNulos.ManageNullInteger(obj.Status_Online));
                    query.Parameters.AddWithValue("@pCOD_MODELO", ManejoNulos.ManageNullInteger(obj.COD_MODELO));
                    query.Parameters.AddWithValue("@pSistema", ManejoNulos.ManageNullInteger(obj.Sistema));
                    query.Parameters.AddWithValue("@pPosicionBilletero", ManejoNulos.ManageNullStr(obj.PosicionBilletero));
                    query.Parameters.AddWithValue("@pcon_sorteo", ManejoNulos.ManageNullInteger(obj.con_sorteo));
                    query.Parameters.AddWithValue("@pBlock", ManejoNulos.ManageNullInteger(obj.Block));
                    query.Parameters.AddWithValue("@pcodigo_extra", ManejoNulos.ManageNullStr(obj.codigo_extra));
                    query.Parameters.AddWithValue("@pPromoBonus", ManejoNulos.ManageNullInteger(obj.PromoBonus));
                    query.Parameters.AddWithValue("@pPromoTicket", ManejoNulos.ManageNullInteger(obj.PromoTicket));
                    query.Parameters.AddWithValue("@pestadoT", ManejoNulos.ManageNullInteger(obj.estadoT));
                    query.Parameters.AddWithValue("@pretiroTemporal", ManejoNulos.ManageNullInteger(obj.retirotemporal));
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                idInsertado = 0;
            }
            return idInsertado;
        }


        public int DetalleContadoresGameInsertarJson(DetalleContadoresGame obj)
        {
            int idInsertado = 0;

            string consulta = @"INSERT INTO [dbo].[PRO_DetalleContadoresGame]
                                   ([CodDetalleContadoresGame]
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
                                   ,[TiempoJuego])
                            OUTPUT Inserted.CodDetalleContadoresGame
                             VALUES
                                   (@pCodDetalleContadoresGame
                                   ,@pCodContadoresGame
                                   ,@pCodMaquina
                                   ,@pCodSala
                                   ,@pCodEmpresa
                                   ,@pCodMoneda
                                   ,@pFechaOperacion
                                   ,@pCoinInIni
                                   ,@pCoinInFin
                                   ,@pCoinOutIni
                                   ,@pCoinOutFin
                                   ,@pJackpotIni
                                   ,@pJackpotFin
                                   ,@pHandPayIni
                                   ,@pHandPayFin
                                   ,@pCancelCreditIni
                                   ,@pCancelCreditFin
                                   ,@pGamesPlayedIni
                                   ,@pGamesPlayedFin
                                   ,@pProduccionPorSlot1
                                   ,@pProduccionPorSlot2Reset
                                   ,@pProduccionPorSlot3Rollover
                                   ,@pProduccionPorSlot4Prueba
                                   ,@pProduccionTotalPorSlot5Dia
                                   ,@pTipoCambio
                                   ,@pFechaRegistro
                                   ,@pFechaModificacion
                                   ,@pActivo
                                   ,@pEstado
                                   ,@pSaldoCoinIn
                                   ,@pSaldoCoinOut
                                   ,@pSaldoJackpot
                                   ,@pSaldoGamesPlayed
                                   ,@pCodUsuario
                                   ,@pRetiroTemporal
                                   ,@pTiempoJuego)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodDetalleContadoresGame", ManejoNulos.ManageNullInteger(obj.CodDetalleContadoresGame));
                    query.Parameters.AddWithValue("@pCodContadoresGame", ManejoNulos.ManageNullInteger(obj.CodContadoresGame));
                    query.Parameters.AddWithValue("@pCodMaquina", ManejoNulos.ManageNullStr(obj.CodMaquina));
                    query.Parameters.AddWithValue("@pCodSala", ManejoNulos.ManageNullInteger(obj.CodSala));
                    query.Parameters.AddWithValue("@pCodEmpresa", ManejoNulos.ManageNullInteger(obj.CodEmpresa));
                    query.Parameters.AddWithValue("@pCodMoneda", ManejoNulos.ManageNullInteger(obj.CodMoneda));
                    query.Parameters.AddWithValue("@pFechaOperacion ", ManejoNulos.ManageNullDate(obj.FechaOperacion));
                    query.Parameters.AddWithValue("@pCoinInIni ", ManejoNulos.ManageNullDouble(obj.CoinInIni));
                    query.Parameters.AddWithValue("@pCoinInFin ", ManejoNulos.ManageNullDouble(obj.CoinInFin));
                    query.Parameters.AddWithValue("@pCoinOutIni ", ManejoNulos.ManageNullDouble(obj.CoinOutIni));
                    query.Parameters.AddWithValue("@pCoinOutFin ", ManejoNulos.ManageNullDouble(obj.CoinOutFin));
                    query.Parameters.AddWithValue("@pJackpotIni ", ManejoNulos.ManageNullDouble(obj.JackpotIni));
                    query.Parameters.AddWithValue("@pJackpotFin ", ManejoNulos.ManageNullDouble(obj.JackpotFin));
                    query.Parameters.AddWithValue("@pHandPayIni ", ManejoNulos.ManageNullDouble(obj.HandPayIni));
                    query.Parameters.AddWithValue("@pHandPayFin ", ManejoNulos.ManageNullDouble(obj.HandPayFin));
                    query.Parameters.AddWithValue("@pCancelCreditIni ", ManejoNulos.ManageNullDouble(obj.CancelCreditIni));
                    query.Parameters.AddWithValue("@pCancelCreditFin ", ManejoNulos.ManageNullDouble(obj.CancelCreditFin));
                    query.Parameters.AddWithValue("@pGamesPlayedIni ", ManejoNulos.ManageNullDouble(obj.GamesPlayedIni));
                    query.Parameters.AddWithValue("@pGamesPlayedFin ", ManejoNulos.ManageNullDouble(obj.GamesPlayedFin));
                    query.Parameters.AddWithValue("@pProduccionPorSlot1 ", ManejoNulos.ManageNullDouble(obj.ProduccionPorSlot1));
                    query.Parameters.AddWithValue("@pProduccionPorSlot2Reset ", ManejoNulos.ManageNullDouble(obj.ProduccionPorSlot2Reset));
                    query.Parameters.AddWithValue("@pProduccionPorSlot3Rollover ", ManejoNulos.ManageNullDouble(obj.ProduccionPorSlot3Rollover));
                    query.Parameters.AddWithValue("@pProduccionPorSlot4Prueba ", ManejoNulos.ManageNullDouble(obj.ProduccionPorSlot4Prueba));
                    query.Parameters.AddWithValue("@pProduccionTotalPorSlot5Dia ", ManejoNulos.ManageNullDouble(obj.ProduccionTotalPorSlot5Dia));
                    query.Parameters.AddWithValue("@pTipoCambio ", ManejoNulos.ManageNullDouble(obj.TipoCambio));
                    query.Parameters.AddWithValue("@pFechaRegistro ", ManejoNulos.ManageNullDate(obj.FechaRegistro));
                    query.Parameters.AddWithValue("@pFechaModificacion ", ManejoNulos.ManageNullDate(obj.FechaModificacion));
                    query.Parameters.AddWithValue("@pActivo ", ManejoNulos.ManegeNullBool(obj.Activo));
                    query.Parameters.AddWithValue("@pEstado ", ManejoNulos.ManegeNullBool(obj.Estado));
                    query.Parameters.AddWithValue("@pSaldoCoinIn ", ManejoNulos.ManageNullDouble(obj.SaldoCoinIn));
                    query.Parameters.AddWithValue("@pSaldoCoinOut ", ManejoNulos.ManageNullDouble(obj.SaldoCoinOut));
                    query.Parameters.AddWithValue("@pSaldoJackpot ", ManejoNulos.ManageNullDouble(obj.SaldoJackpot));
                    query.Parameters.AddWithValue("@pSaldoGamesPlayed", ManejoNulos.ManageNullDouble(obj.SaldoGamesPlayed));
                    query.Parameters.AddWithValue("@pCodUsuario", ManejoNulos.ManageNullInteger(obj.CodUsuario));
                    query.Parameters.AddWithValue("@pRetiroTemporal ", ManejoNulos.ManageNullInteger(obj.RetiroTemporal));
                    query.Parameters.AddWithValue("@pTiempoJuego ", ManejoNulos.ManageNullDouble(obj.TiempoJuego));
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                idInsertado = 0;
            }
            return idInsertado;
        }

        #endregion

    }
}
