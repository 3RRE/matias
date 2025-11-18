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
    public class SolicitudTransferenciaDAL
    {
        string _conexion = string.Empty;
        public SolicitudTransferenciaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public int SolicitudTransferenciaInsertarJson(SolicitudTransferencia solicitudTransferencia)
        {
            int idInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[SolicitudTransferencia]
                                   ([SolicitudSala]
                                   ,[Codsala]
                                   ,[ClienteNombre]
                                   ,[ClienteApelPat]
                                   ,[ClienteApelMat]
                                   ,[ClienteTipoDoc]
                                   ,[ClienteNroDoc]
                                   ,[Monto]
                                   ,[NroTickets]
                                   ,[Banco]
                                   ,[Cuenta]
                                   ,[FechaReg]
                                   ,[UsuarioNombreReg]
                                   ,[Estado])
                                Output Inserted.SolicitudID
                             VALUES
                                   (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", solicitudTransferencia.SolicitudSala);
                    query.Parameters.AddWithValue("@p1", solicitudTransferencia.Codsala);
                    query.Parameters.AddWithValue("@p2", solicitudTransferencia.ClienteNombre);
                    query.Parameters.AddWithValue("@p3", solicitudTransferencia.ClienteApelPat);
                    query.Parameters.AddWithValue("@p4", solicitudTransferencia.ClienteApelMat);
                    query.Parameters.AddWithValue("@p5", solicitudTransferencia.TipoDocNombre);
                    query.Parameters.AddWithValue("@p6", solicitudTransferencia.ClienteNroDoc);
                    query.Parameters.AddWithValue("@p7", solicitudTransferencia.Monto);
                    query.Parameters.AddWithValue("@p8", Convert.ToInt32(solicitudTransferencia.NroTickets));
                    query.Parameters.AddWithValue("@p9", solicitudTransferencia.BancoNombre);
                    query.Parameters.AddWithValue("@p10", solicitudTransferencia.NroCuenta);
                    query.Parameters.AddWithValue("@p11", solicitudTransferencia.FechaReg);
                    query.Parameters.AddWithValue("@p12", solicitudTransferencia.UsuarioNombreReg);
                    query.Parameters.AddWithValue("@p13", solicitudTransferencia.Estado);
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
        public bool SolicitudTransferenciaEditarJson(SolicitudTransferencia solicitudTransferencia)
        {
            bool response = false;
            string consulta = @"UPDATE [dbo].[SolicitudTransferencia]
                       SET [SolicitudSala] = @p0
                          ,[Codsala] = @p1
                          ,[ClienteNombre] = @p2
                          ,[ClienteApelPat] = @p3
                          ,[ClienteApelMat] = @p4
                          ,[ClienteTipoDoc] = @p5
                          ,[ClienteNroDoc] = @p6
                          ,[Monto] = @p7
                          ,[NroTickets] = @p8
                          ,[Banco] = @p9
                          ,[Cuenta] = @p10
                          ,[FechaReg] = @p11
                          ,[UsuarioNombreReg] = @p12
                          ,[Estado] = @p13
                     WHERE SolicitudID=@p14;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", solicitudTransferencia.SolicitudSala);
                    query.Parameters.AddWithValue("@p1", solicitudTransferencia.Codsala);
                    query.Parameters.AddWithValue("@p2", solicitudTransferencia.ClienteNombre);
                    query.Parameters.AddWithValue("@p3", solicitudTransferencia.ClienteApelPat);
                    query.Parameters.AddWithValue("@p4", solicitudTransferencia.ClienteApelMat);
                    query.Parameters.AddWithValue("@p5", solicitudTransferencia.TipoDocNombre);
                    query.Parameters.AddWithValue("@p6", solicitudTransferencia.ClienteNroDoc);
                    query.Parameters.AddWithValue("@p7", solicitudTransferencia.Monto);
                    query.Parameters.AddWithValue("@p8",Convert.ToInt32(solicitudTransferencia.NroTickets));
                    query.Parameters.AddWithValue("@p9", solicitudTransferencia.BancoNombre);
                    query.Parameters.AddWithValue("@p10", solicitudTransferencia.NroCuenta);
                    query.Parameters.AddWithValue("@p11", solicitudTransferencia.FechaReg);
                    query.Parameters.AddWithValue("@p12", solicitudTransferencia.UsuarioNombreReg);
                    query.Parameters.AddWithValue("@p13", solicitudTransferencia.Estado);
                    query.Parameters.AddWithValue("@p14", solicitudTransferencia.SolicitudID);
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
        public List<SolicitudTransferencia> SolicitudTransferenciaListarSolicitudTransferenciaSalaJson(int solicitudSala)
        {
            List<SolicitudTransferencia> lista = new List<SolicitudTransferencia>();
            string consulta = @"SELECT [SolicitudID]
                                  ,[SolicitudSala]
                                  ,[Codsala]
                                  ,[ClienteNombre]
                                  ,[ClienteApelPat]
                                  ,[ClienteApelMat]
                                  ,[ClienteTipoDoc]
                                  ,[ClienteNroDoc]
                                  ,[Monto]
                                  ,[NroTickets]
                                  ,[Banco]
                                  ,[Cuenta]
                                  ,[FechaReg]
                                  ,[UsuarioNombreReg]
                                  ,[Estado]
                              FROM [dbo].[SolicitudTransferencia] WHERE SolicitudSala=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", solicitudSala);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new SolicitudTransferencia
                            {
                                SolicitudID = ManejoNulos.ManageNullInteger(dr["SolicitudID"]),
                                SolicitudSala = ManejoNulos.ManageNullInteger(dr["SolicitudSala"]),
                                Codsala = ManejoNulos.ManageNullInteger(dr["Codsala"]),
                                ClienteNombre = ManejoNulos.ManageNullStr(dr["ClienteNombre"]),
                                ClienteApelPat = ManejoNulos.ManageNullStr(dr["ClienteApelPat"]),
                                ClienteApelMat = ManejoNulos.ManageNullStr(dr["ClienteApelMat"]),
                                TipoDocNombre = ManejoNulos.ManageNullStr(dr["ClienteTipoDoc"]),
                                ClienteNroDoc = ManejoNulos.ManageNullStr(dr["ClienteNroDoc"]),
                                Monto = ManejoNulos.ManageNullDouble(dr["Monto"]),
                                NroTickets = ManejoNulos.ManageNullStr(dr["NroTickets"]),
                                BancoNombre = ManejoNulos.ManageNullStr(dr["Banco"]),
                                NroCuenta = ManejoNulos.ManageNullStr(dr["Cuenta"]),
                                FechaReg = ManejoNulos.ManageNullDate(dr["FechaReg"]),
                                UsuarioNombreReg = ManejoNulos.ManageNullStr(dr["UsuarioNombreReg"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
        public bool SolicitudTransferenciaAnularJson(SolicitudTransferencia solicitudTransferencia)
        {
            bool response = false;
            string consulta = @"UPDATE [dbo].[SolicitudTransferencia]
                       SET [Estado] = @p0
                     WHERE SolicitudID=@p1;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", solicitudTransferencia.Estado);
                    query.Parameters.AddWithValue("@p1", solicitudTransferencia.SolicitudID);
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
