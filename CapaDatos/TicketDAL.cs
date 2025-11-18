using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class TicketDAL
    {
        string _conexion = string.Empty;
        public TicketDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<TicketEntidad> TicketDepositoIDListadoJson(int DepositoID)
        {
            List<TicketEntidad> lista = new List<TicketEntidad>();
            string consulta = @"SELECT [TicketID]
                                  ,[NroTicketTito]
                                  ,[DepositoID]
                                  ,[FechaReg]
                                  ,[Monto]
                              FROM [dbo].[Ticket] where DepositoID=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", DepositoID);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new TicketEntidad
                            {
                                TicketID = ManejoNulos.ManageNullInteger(dr["TicketID"]),
                                NroTicketTito = ManejoNulos.ManageNullStr(dr["NroTicketTito"]),
                                DepositoID = ManejoNulos.ManageNullInteger(dr["DepositoID"]),
                                FechaReg = ManejoNulos.ManageNullDate(dr["FechaReg"]),
                                Monto = ManejoNulos.ManageNullDouble(dr["Monto"]),
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
            finally
            {
            }
            return lista;
        }
        public bool TicketInsertarJson(TicketEntidad ticket)
        {
            bool response = false;
            string consulta = @"INSERT INTO [dbo].[Ticket]
                                       ([NroTicketTito]
                                       ,[DepositoID]
                                       ,[FechaReg]
                                       ,[Monto])
                                 VALUES
                                       (@p0
                                       ,@p1
                                       ,@p2
                                       ,@p3)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ticket.NroTicketTito);
                    query.Parameters.AddWithValue("@p1", ticket.DepositoID);
                    query.Parameters.AddWithValue("@p2", ticket.FechaReg);
                    query.Parameters.AddWithValue("@p3", ticket.Monto);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        public bool TicketEditarJson(TicketEntidad ticket)
        {
            bool response = false;
            string consulta = @"UPDATE [dbo].[Ticket]
                           SET [NroTicketTito] = @p0
                              ,[DepositoID] = @p1
                              ,[FechaReg] = @p2
                              ,[Monto] = @p3
                         WHERE TicketID=@p4;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ticket.NroTicketTito);
                    query.Parameters.AddWithValue("@p1", ticket.DepositoID);
                    query.Parameters.AddWithValue("@p2", ticket.FechaReg);
                    query.Parameters.AddWithValue("@p3", ticket.Monto);
                    query.Parameters.AddWithValue("@p4", ticket.TicketID);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }


        public bool TicketATInsertarJson(HistorialTicketAT ticket)
        {
            bool response = false;
            string consulta = @"INSERT INTO [dbo].[HistorialTicketAT]
                                       ([Item]
                                       ,[Tito_NroTicket]
                                       ,[Tito_MontoTicket]
                                       ,[MaquinaCaja]
                                       ,[tipo_ticket]
                                       ,[Tito_fechaini]
                                       ,[juego]
                                       ,[marca]
                                       ,[codAperturaCajaIni]
                                       ,[CodigoMaquina]
                                       ,[IdTipoMoneda]
                                       ,[fecharegistro]
                                        ,CodSala,UsuarioID)
                                 VALUES
                                       (@p0
                                       ,@p1
                                       ,@p2
                                       ,@p3
                                       ,@p4
                                       ,@p5
                                       ,@p6
                                       ,@p7
                                       ,@p8
                                       ,@p9
                                       ,@p10
                                       ,@p11,@p12,@p13)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ticket.Item);
                    query.Parameters.AddWithValue("@p1", ticket.Tito_NroTicket);
                    query.Parameters.AddWithValue("@p2", ticket.Tito_MontoTicket);
                    query.Parameters.AddWithValue("@p3", ticket.MaquinaCaja);
                    query.Parameters.AddWithValue("@p4", ticket.tipo_ticket);
                    query.Parameters.AddWithValue("@p5", ticket.Tito_fechaini);
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(ticket.juego));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(ticket.marca));
                    query.Parameters.AddWithValue("@p8", ticket.codAperturaCajaIni);
                    query.Parameters.AddWithValue("@p9", ticket.CodigoMaquina);
                    query.Parameters.AddWithValue("@p10", ticket.IdTipoMoneda);
                    query.Parameters.AddWithValue("@p11", ticket.fecharegistro);
                    query.Parameters.AddWithValue("@p12", ticket.CodSala);
                    query.Parameters.AddWithValue("@p13", ticket.UsuarioID);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response = false;
            }
            return response;
        }

        public List<HistorialTicketAT> TicketATSalaListarJson(DateTime fechaini, DateTime fechafin, string codsala,string usuario)
        {
            List<HistorialTicketAT> lista = new List<HistorialTicketAT>();
            string consulta = @"SELECT h.[id]
                                  ,h.[Item]
                                  ,h.[Tito_NroTicket]
                                  ,h.[Tito_MontoTicket]
                                  ,h.[MaquinaCaja]
                                  ,h.[tipo_ticket]
                                  ,h.[Tito_fechaini]
                                  ,h.[juego]
                                  ,h.[marca]
                                  ,h.[codAperturaCajaIni]
                                  ,h.[CodigoMaquina]
                                  ,h.[IdTipoMoneda]
                                  ,s.CodSala
                                  ,s.Nombre nombreSala
                                  ,h.[fecharegistro]
                                    ,h.UsuarioID
                                    ,us.UsuarioNombre
                                FROM HistorialTicketAT h
                                left join Sala s on s.CodSala=h.Codsala
                                left join SEG_Usuario us on us.UsuarioID=h.UsuarioID
                              where " + codsala + " "+usuario+" CONVERT(date, h.fecharegistro) between @p1 and @p2 " +
                              " order by h.[id] desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaini.Date);
                    query.Parameters.AddWithValue("@p2", fechafin.Date);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var doi = new HistorialTicketAT
                            {
                                id = ManejoNulos.ManageNullInteger(dr["id"]),
                                Item = ManejoNulos.ManageNullInteger(dr["Item"]),
                                Tito_NroTicket = ManejoNulos.ManageNullStr(dr["Tito_NroTicket"].Trim()),
                                Tito_MontoTicket = ManejoNulos.ManageNullDouble(dr["Tito_MontoTicket"]),
                                MaquinaCaja = ManejoNulos.ManageNullStr(dr["MaquinaCaja"].Trim()),
                                tipo_ticket = ManejoNulos.ManageNullStr(dr["tipo_ticket"].Trim()),
                                Tito_fechaini = ManejoNulos.ManageNullDate(dr["Tito_fechaini"]),
                                juego = ManejoNulos.ManageNullStr(dr["juego"].Trim()),
                                marca = ManejoNulos.ManageNullStr(dr["marca"].Trim()),
                                codAperturaCajaIni = ManejoNulos.ManageNullInteger(dr["codAperturaCajaIni"]),
                                CodigoMaquina = ManejoNulos.ManageNullStr(dr["CodigoMaquina"].Trim()),
                                IdTipoMoneda = ManejoNulos.ManageNullInteger(dr["IdTipoMoneda"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                nombreSala = ManejoNulos.ManageNullStr(dr["nombreSala"]),
                                fecharegistro = ManejoNulos.ManageNullDate(dr["fecharegistro"]),
                                UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]),
                                UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                            };

                            lista.Add(doi);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }
    }
}
