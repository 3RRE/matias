using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class SolicitudTicketDAL
    {
        string _conexion = string.Empty;
        public SolicitudTicketDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<SolicitudTicket> SolicitudTicketSolicitudIDListadoJson(int SolicitudID)
        {
            List<SolicitudTicket> lista = new List<SolicitudTicket>();
            string consulta = @"SELECT [SolicitudTicketID]
                                  ,[NroTicketTito]
                                  ,[SolicitudID]
                                  ,[FechaReg]
                                  ,[Monto]
                              FROM [dbo].[SolicitudTicket] where SolicitudID=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", SolicitudID);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new SolicitudTicket
                            {
                                SolicitudTicketID = ManejoNulos.ManageNullInteger(dr["SolicitudTicketID"]),
                                NroTicketTito = ManejoNulos.ManageNullStr(dr["NroTicketTito"]),
                                SolicitudID = ManejoNulos.ManageNullInteger(dr["SolicitudID"]),
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
        public bool SolicitudTicketInsertarJson(SolicitudTicket ticket)
        {
            bool response = false;
            string consulta = @"INSERT INTO [dbo].[SolicitudTicket]
                                   ([NroTicketTito]
                                   ,[SolicitudID]
                                   ,[FechaReg]
                                   ,[Monto])
                             VALUES
                                   (@p0,@p1,@p2,@p3)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ticket.NroTicketTito);
                    query.Parameters.AddWithValue("@p1", ticket.SolicitudID);
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
        public bool SolicitudTicketEditarJson(SolicitudTicket ticket)
        {
            bool response = false;
            string consulta = @"UPDATE [dbo].[SolicitudTicket]
                       SET [NroTicketTito] = @p0
                          ,[SolicitudID] = @p1
                          ,[FechaReg] = @p2
                          ,[Monto] = @p3
                     WHERE SolicitudTicketID=@p4";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ticket.NroTicketTito);
                    query.Parameters.AddWithValue("@p1", ticket.SolicitudID);
                    query.Parameters.AddWithValue("@p2", ticket.FechaReg);
                    query.Parameters.AddWithValue("@p3", ticket.Monto);
                    query.Parameters.AddWithValue("@p4", ticket.SolicitudTicketID);
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
    }
}
