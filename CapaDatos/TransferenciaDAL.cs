using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CapaDatos
{
    public class TransferenciaDAL
    {
        string _conexion = string.Empty;
        public TransferenciaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<Transferencia> TransferenciaListarJson(DateTime fechaini, DateTime fechafin, int codsala)
        {
            List<Transferencia> lista = new List<Transferencia>();
            string consulta = @"SELECT [TransferenciaID]
      ,[Codsala]
      ,[ClienteNombre]
      ,[ClienteApelPat]
      ,[ClienteApelMat]
      ,[ClienteTipoDoc]
      ,[ClienteNroDoc]
      ,[Monto]
      ,[Banco]
      ,[Cuenta]
      ,[Estado]
      ,[NroOperacion]
      ,[FechaOperacion]
      ,[Observacion]
      ,[FechaReg]
      ,[FechaAct]
                              FROM [dbo].[Transferencia]
                              where Codsala=@codsala and  CONVERT(date, FechaReg) between @p1 and @p2 
	                          order by ClienteApelPat ASC,ClienteApelMat ASC,ClienteNombre ASC";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaini);
                    query.Parameters.AddWithValue("@p2", fechafin);
                    query.Parameters.AddWithValue("@codsala", codsala);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var doi = new Transferencia
                            {
                                TransferenciaID = ManejoNulos.ManageNullInteger(dr["TransferenciaID"]),
                                Codsala = ManejoNulos.ManageNullInteger(dr["Codsala"].Trim()),
                                ClienteNombre = ManejoNulos.ManageNullStr(dr["ClienteNombre"].Trim()),
                                ClienteApelPat = ManejoNulos.ManageNullStr(dr["ClienteApelPat"].Trim()),
                                ClienteApelMat = ManejoNulos.ManageNullStr(dr["ClienteApelMat"].Trim()),
                                TipoDocNombre = ManejoNulos.ManageNullStr(dr["ClienteTipoDoc"].Trim()),
                                ClienteNroDoc = ManejoNulos.ManageNullStr(dr["ClienteNroDoc"].Trim()),
                                Monto = ManejoNulos.ManageNullDouble(dr["Monto"].Trim()),
                                BancoNombre = ManejoNulos.ManageNullStr(dr["Banco"].Trim()),
                                NroCuenta = ManejoNulos.ManageNullStr(dr["Cuenta"].Trim()),
                                NroOperacion = ManejoNulos.ManageNullStr(dr["NroOperacion"].Trim()),
                                FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"].Trim()),
                                Observacion = ManejoNulos.ManageNullStr(dr["Observacion"].Trim()),
                                FechaReg = ManejoNulos.ManageNullDate(dr["FechaReg"].Trim()),
                                FechaAct = ManejoNulos.ManageNullDate(dr["FechaAct"].Trim()),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim()),
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

        public List<Transferencia> TransferenciaBuscarListarJson(DateTime fechaini, DateTime fechafin)
        {
            List<Transferencia> lista = new List<Transferencia>();
            string consulta = @"SELECT tr.[TransferenciaID]
      ,tr.[Codsala]
        ,s.Nombre nombresala
      ,tr.[ClienteNombre]
      ,tr.[ClienteApelPat]
      ,tr.[ClienteApelMat]
      ,tr.[ClienteTipoDoc]
      ,tr.[ClienteNroDoc]
      ,tr.[Monto]
      ,tr.[Banco]
      ,tr.[Cuenta]
      ,tr.[Estado]
      ,tr.[NroOperacion]
      ,tr.[FechaOperacion]
      ,tr.[Observacion]
      ,tr.[FechaReg]
      ,tr.[FechaAct]
                              FROM [dbo].[Transferencia] tr
left join Sala s on s.CodSala=tr.Codsala
                              where  CONVERT(date, tr.FechaReg) between @p1 and @p2 
	                          order by  tr.[TransferenciaID] desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaini);
                    query.Parameters.AddWithValue("@p2", fechafin);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var doi = new Transferencia
                            {
                                TransferenciaID = ManejoNulos.ManageNullInteger(dr["TransferenciaID"]),
                                Codsala = ManejoNulos.ManageNullInteger(dr["Codsala"].Trim()),
                                nombresala = ManejoNulos.ManageNullStr(dr["nombresala"].Trim()),
                                ClienteNombre = ManejoNulos.ManageNullStr(dr["ClienteNombre"].Trim()),
                                ClienteApelPat = ManejoNulos.ManageNullStr(dr["ClienteApelPat"].Trim()),
                                ClienteApelMat = ManejoNulos.ManageNullStr(dr["ClienteApelMat"].Trim()),
                                TipoDocNombre = ManejoNulos.ManageNullStr(dr["ClienteTipoDoc"].Trim()),
                                ClienteNroDoc = ManejoNulos.ManageNullStr(dr["ClienteNroDoc"].Trim()),
                                Monto = ManejoNulos.ManageNullDouble(dr["Monto"].Trim()),
                                BancoNombre = ManejoNulos.ManageNullStr(dr["Banco"].Trim()),
                                NroCuenta = ManejoNulos.ManageNullStr(dr["Cuenta"].Trim()),
                                NroOperacion = ManejoNulos.ManageNullStr(dr["NroOperacion"].Trim()),
                                FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"]),
                                Observacion = ManejoNulos.ManageNullStr(dr["Observacion"].Trim()),
                                FechaReg = ManejoNulos.ManageNullDate(dr["FechaReg"]),
                                FechaAct = ManejoNulos.ManageNullDate(dr["FechaAct"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim()),
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

        public List<Transferencia> TransferenciaSalasListarJson(DateTime fechaini, DateTime fechafin, string codsala)
        {
            List<Transferencia> lista = new List<Transferencia>();
            string consulta = @"SELECT tr.[TransferenciaID]
                                      ,tr.[Codsala]
                                      ,s.Nombre nombresala
                                      ,tr.[ClienteNombre]
                                      ,tr.[ClienteApelPat]
                                      ,tr.[ClienteApelMat]
                                      ,tr.[ClienteTipoDoc]
                                      ,tr.[ClienteNroDoc]
                                      ,tr.[Monto]
                                      ,tr.[Banco]
                                      ,tr.[Cuenta]
                                      ,tr.[Estado]
                                      ,tr.[NroOperacion]
                                      ,tr.[FechaOperacion]
                                      ,tr.[Observacion]
                                      ,tr.[FechaReg]
                                      ,tr.[FechaAct]
                                      ,tr.UsuarioID
                                      ,us.UsuarioNombre
                                    ,tr.[ImagenVoucher]
                                    ,tr.[SolicitudTransferenciaID]
                                    ,st.UsuarioNombreReg usuariosala
                                    ,st.NroTickets
                                    ,st.SolicitudSala
                              FROM [dbo].[Transferencia] tr
                                left join Sala s on s.CodSala=tr.Codsala
                                left join SEG_Usuario us on us.UsuarioID=tr.UsuarioID
                                left join SolicitudTransferencia st on st.SolicitudID=tr.SolicitudTransferenciaID
                              where " + codsala+"  CONVERT(date, tr.FechaReg) between @p1 and @p2 "+
                              " order by tr.[TransferenciaID] desc";
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
                            var doi = new Transferencia
                            {
                                TransferenciaID = ManejoNulos.ManageNullInteger(dr["TransferenciaID"]),
                                Codsala = ManejoNulos.ManageNullInteger(dr["Codsala"].Trim()),
                                nombresala = ManejoNulos.ManageNullStr(dr["nombresala"].Trim()),
                                ClienteNombre = ManejoNulos.ManageNullStr(dr["ClienteNombre"].Trim()),
                                ClienteApelPat = ManejoNulos.ManageNullStr(dr["ClienteApelPat"].Trim()),
                                ClienteApelMat = ManejoNulos.ManageNullStr(dr["ClienteApelMat"].Trim()),
                                TipoDocNombre = ManejoNulos.ManageNullStr(dr["ClienteTipoDoc"].Trim()),
                                ClienteNroDoc = ManejoNulos.ManageNullStr(dr["ClienteNroDoc"].Trim()),
                                Monto = ManejoNulos.ManageNullDouble(dr["Monto"].Trim()),
                                BancoNombre = ManejoNulos.ManageNullStr(dr["Banco"].Trim()),
                                NroCuenta = ManejoNulos.ManageNullStr(dr["Cuenta"].Trim()),
                                NroOperacion = ManejoNulos.ManageNullStr(dr["NroOperacion"].Trim()),
                                FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"]),
                                Observacion = ManejoNulos.ManageNullStr(dr["Observacion"].Trim()),
                                FechaReg = ManejoNulos.ManageNullDate(dr["FechaReg"]),
                                FechaAct = ManejoNulos.ManageNullDate(dr["FechaAct"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim()),
                                UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]),
                                UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                ImagenVoucher = ManejoNulos.ManageNullStr(dr["ImagenVoucher"]),
                                SolicitudTransferenciaID = ManejoNulos.ManageNullInteger(dr["SolicitudTransferenciaID"]),
                                usuariosala = ManejoNulos.ManageNullStr(dr["usuariosala"]),
                                NroTickets = ManejoNulos.ManageNullInteger(dr["NroTickets"]),
                                SolicitudSala=ManejoNulos.ManageNullInteger(dr["SolicitudSala"]),
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

        public Transferencia TransferenciaIDJson(int TransferenciaID)
        {
            Transferencia solicitud = new Transferencia();
            string consulta = @"SELECT tr.[TransferenciaID]
                                      ,tr.[Codsala]
                                      ,s.Nombre nombresala
                                      ,tr.[ClienteNombre]
                                      ,tr.[ClienteApelPat]
                                      ,tr.[ClienteApelMat]
                                      ,tr.[ClienteTipoDoc]
                                      ,tr.[ClienteNroDoc]
                                      ,tr.[Monto]
                                      ,tr.[Banco]
                                      ,tr.[Cuenta]
                                      ,tr.[Estado]
                                      ,tr.[NroOperacion]
                                      ,tr.[FechaOperacion]
                                      ,tr.[Observacion]
                                      ,tr.[FechaReg]
                                      ,tr.[FechaAct]
                                      ,tr.UsuarioID
                                      ,us.UsuarioNombre
                                    ,tr.[ImagenVoucher]
                                    ,tr.[SolicitudTransferenciaID]
                                    ,st.UsuarioNombreReg usuariosala
                                    ,st.NroTickets
                              FROM [dbo].[Transferencia] tr
                                left join Sala s on s.CodSala=tr.Codsala
                                left join SEG_Usuario us on us.UsuarioID=tr.UsuarioID
                                left join SolicitudTransferencia st on st.SolicitudID=tr.SolicitudTransferenciaID
                              where  tr.[TransferenciaID] = @p1";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", TransferenciaID);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new Transferencia
                            {
                                TransferenciaID = ManejoNulos.ManageNullInteger(dr["TransferenciaID"]),
                                Codsala = ManejoNulos.ManageNullInteger(dr["Codsala"].Trim()),
                                nombresala = ManejoNulos.ManageNullStr(dr["nombresala"].Trim()),
                                ClienteNombre = ManejoNulos.ManageNullStr(dr["ClienteNombre"].Trim()),
                                ClienteApelPat = ManejoNulos.ManageNullStr(dr["ClienteApelPat"].Trim()),
                                ClienteApelMat = ManejoNulos.ManageNullStr(dr["ClienteApelMat"].Trim()),
                                TipoDocNombre = ManejoNulos.ManageNullStr(dr["ClienteTipoDoc"].Trim()),
                                ClienteNroDoc = ManejoNulos.ManageNullStr(dr["ClienteNroDoc"].Trim()),
                                Monto = ManejoNulos.ManageNullDouble(dr["Monto"].Trim()),
                                BancoNombre = ManejoNulos.ManageNullStr(dr["Banco"].Trim()),
                                NroCuenta = ManejoNulos.ManageNullStr(dr["Cuenta"].Trim()),
                                NroOperacion = ManejoNulos.ManageNullStr(dr["NroOperacion"].Trim()),
                                FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"]),
                                Observacion = ManejoNulos.ManageNullStr(dr["Observacion"].Trim()),
                                FechaReg = ManejoNulos.ManageNullDate(dr["FechaReg"]),
                                FechaAct = ManejoNulos.ManageNullDate(dr["FechaAct"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim()),
                                UsuarioID = ManejoNulos.ManageNullInteger(dr["UsuarioID"]),
                                UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                ImagenVoucher = ManejoNulos.ManageNullStr(dr["ImagenVoucher"]),
                                SolicitudTransferenciaID = ManejoNulos.ManageNullInteger(dr["SolicitudTransferenciaID"]),
                                usuariosala = ManejoNulos.ManageNullStr(dr["usuariosala"]),
                                NroTickets = ManejoNulos.ManageNullInteger(dr["NroTickets"]),
                            };
                            solicitud = item;
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
            return solicitud;
        }

        public int TransferenciaInsertarJson(Transferencia transferencia)
        {
            int IdInsertado = 0;
            string consulta = @"insert into Transferencia (TransferenciaSala,[Codsala]
           ,[ClienteNombre]
           ,[ClienteApelPat]
           ,[ClienteApelMat]
           ,[ClienteTipoDoc]
           ,[ClienteNroDoc]
           ,[Monto]
           ,[Banco]
           ,[Cuenta]
           ,[Estado]
           ,[NroOperacion]
           ,[FechaOperacion]
           ,[Observacion]
           ,[FechaReg]
           ,[FechaAct],UsuarioID,ImagenVoucher,SolicitudTransferenciaID) Output Inserted.TransferenciaID values (@TransferenciaSala,@Codsala
           ,@ClienteNombre
           ,@ClienteApelPat
           ,@ClienteApelMat
           ,@ClienteTipoDoc
           ,@ClienteNroDoc
           ,@Monto
           ,@Banco
           ,@Cuenta
           ,@Estado
           ,@NroOperacion
           ,@FechaOperacion
           ,@Observacion
           ,@FechaReg
           ,@FechaAct,@UsuarioID,@ImagenVoucher,@SolicitudTransferenciaID)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@TransferenciaSala", transferencia.TransferenciaSala);
                    query.Parameters.AddWithValue("@Codsala", transferencia.Codsala);
                    query.Parameters.AddWithValue("@ClienteNombre", transferencia.ClienteNombre);
                    query.Parameters.AddWithValue("@ClienteApelPat", transferencia.ClienteApelPat);
                    query.Parameters.AddWithValue("@ClienteApelMat", transferencia.ClienteApelMat);
                    query.Parameters.AddWithValue("@ClienteTipoDoc", transferencia.TipoDocNombre);
                    query.Parameters.AddWithValue("@ClienteNroDoc", transferencia.ClienteNroDoc);
                    query.Parameters.AddWithValue("@Monto", transferencia.Monto);
                    query.Parameters.AddWithValue("@Banco", transferencia.BancoNombre);
                    query.Parameters.AddWithValue("@Cuenta", transferencia.NroCuenta);
                    query.Parameters.AddWithValue("@Estado", transferencia.Estado);
                    query.Parameters.AddWithValue("@NroOperacion", transferencia.NroOperacion);
                    query.Parameters.AddWithValue("@FechaOperacion", transferencia.FechaOperacion);
                    query.Parameters.AddWithValue("@Observacion", transferencia.Observacion);
                    query.Parameters.AddWithValue("@FechaReg", transferencia.FechaReg);
                    query.Parameters.AddWithValue("@FechaAct", transferencia.FechaAct);
                    query.Parameters.AddWithValue("@UsuarioID", transferencia.UsuarioID);
                    query.Parameters.AddWithValue("@ImagenVoucher", transferencia.ImagenVoucher);
                    query.Parameters.AddWithValue("@SolicitudTransferenciaID", transferencia.SolicitudTransferenciaID);

                    //query.ExecuteNonQuery();
                    //response = true;
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public bool TransferenciaImagenModificarJson(int transferenciaID, string imagen)
        {

            string consulta = @"update Transferencia set ImagenVoucher = @pImagenVoucher where TransferenciaID = @ptransferenciaID";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pImagenVoucher", imagen);
                    query.Parameters.AddWithValue("@ptransferenciaID", transferenciaID);

                    query.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
                //Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
        }

        public List<Cliente> TransferenciaClientesListarJson()
        {
            List<Cliente> lista = new List<Cliente>();
            string consulta = @"SELECT 
                                [ClienteNombre]
                                ,[ClienteApelPat]
                                ,[ClienteApelMat]
                                ,[ClienteTipoDoc]
                                ,[ClienteNroDoc]
                                FROM [dbo].[Transferencia]
                                group by ClienteApelPat ,ClienteApelMat ,ClienteNombre ,[ClienteTipoDoc],ClienteNroDoc
                                order by ClienteApelPat ASC,ClienteApelMat ASC,ClienteNombre ASC";
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
                            var doi = new Cliente
                            {
                               
                                ClienteNombre = ManejoNulos.ManageNullStr(dr["ClienteNombre"].Trim()),
                                ClienteApelPat = ManejoNulos.ManageNullStr(dr["ClienteApelPat"].Trim()),
                                ClienteApelMat = ManejoNulos.ManageNullStr(dr["ClienteApelMat"].Trim()),
                                ClienteTipoDoc = ManejoNulos.ManageNullStr(dr["ClienteTipoDoc"].Trim()),
                                ClienteNroDoc = ManejoNulos.ManageNullStr(dr["ClienteNroDoc"].Trim()),
                               
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

        public List<Transferencia> TransferenciaCuentasListarJson(string tipodoc,string nrodoc)
        {
            List<Transferencia> lista = new List<Transferencia>();
            string consulta = @"SELECT 
                                Banco
                                ,Cuenta
                                FROM [dbo].[Transferencia]
                                where [ClienteTipoDoc] =@p1 and ClienteNroDoc=@p2
                                group by Banco,Cuenta
                                order by Banco ASC";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", tipodoc);
                    query.Parameters.AddWithValue("@p2", nrodoc);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var doi = new Transferencia
                            {
                                BancoNombre = ManejoNulos.ManageNullStr(dr["Banco"].Trim()),
                                NroCuenta = ManejoNulos.ManageNullStr(dr["Cuenta"].Trim()),
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

        public List<DepositoEntidad> DepositoListarJson(DateTime fechaini, DateTime fechafin, string codsala)
        {
            List<DepositoEntidad> lista = new List<DepositoEntidad>();
            string consulta = @"SELECT de.[DepositoID]
                              ,de.[DepositoSala]
                              ,de.[Codsala]
                                ,s.Nombre nombresala
                              ,de.[ClienteNombre]
                              ,de.[ClienteApelPat]
                              ,de.[ClienteApelMat]
                              ,de.[ClienteTipoDoc]
                              ,de.[ClienteNroDoc]
                              ,de.[Monto]
                              ,de.[NroTickets]
                              ,de.[Estado]
                              ,de.[NroOperacion]
                              ,de.[FechaReg]
                              ,de.[FechaAct]
                                ,de.[UsuarioNombreReg]
                              FROM [dbo].[Deposito] de  left join Sala s on s.CodSala=de.Codsala
                              where  " + codsala+ " CONVERT(date, de.FechaReg) between @p1 and @p2 " +
                             " order by DepositoID desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaini);
                    query.Parameters.AddWithValue("@p2", fechafin);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var doi = new DepositoEntidad
                            {
                                DepositoID = ManejoNulos.ManageNullInteger(dr["DepositoID"]),
                                Codsala = ManejoNulos.ManageNullInteger(dr["Codsala"].Trim()),
                                nombresala = ManejoNulos.ManageNullStr(dr["nombresala"].Trim()),
                                ClienteNombre = ManejoNulos.ManageNullStr(dr["ClienteNombre"].Trim()),
                                ClienteApelPat = ManejoNulos.ManageNullStr(dr["ClienteApelPat"].Trim()),
                                ClienteApelMat = ManejoNulos.ManageNullStr(dr["ClienteApelMat"].Trim()),
                                TipoDocNombre = ManejoNulos.ManageNullStr(dr["ClienteTipoDoc"].Trim()),
                                ClienteNroDoc = ManejoNulos.ManageNullStr(dr["ClienteNroDoc"].Trim()),
                                Monto = ManejoNulos.ManageNullDouble(dr["Monto"].Trim()),
                                NroTickets = ManejoNulos.ManageNullStr(dr["NroTickets"].Trim()),
                                NroOperacion = ManejoNulos.ManageNullStr(dr["NroOperacion"].Trim()),
                                FechaReg = ManejoNulos.ManageNullDate(dr["FechaReg"]),
                                FechaAct = ManejoNulos.ManageNullDate(dr["FechaAct"]),
                                UsuarioNombreReg = ManejoNulos.ManageNullStr(dr["UsuarioNombreReg"].Trim()),
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

        public List<TicketEntidad> TicketsDepositosListarJson()
        {
            List<TicketEntidad> lista = new List<TicketEntidad>();
            string consulta = @"SELECT 
                               [TicketID]
                              ,[NroTicketTito]
                              ,[DepositoID]
                              ,[FechaReg]
                              ,[Monto]
                                FROM [dbo].[Ticket]
                                where DepositoID=@p0";
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
                            var doi = new TicketEntidad
                            {

                                TicketID = ManejoNulos.ManageNullInteger(dr["TicketID"].Trim()),
                                NroTicketTito = ManejoNulos.ManageNullStr(dr["NroTicketTito"].Trim()),
                                DepositoID = ManejoNulos.ManageNullInteger(dr["DepositoID"].Trim()),
                                FechaReg = ManejoNulos.ManageNullDate(dr["FechaReg"].Trim()),
                                Monto = ManejoNulos.ManageNullDouble(dr["Monto"].Trim()),

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



        public SolicitudTransferencia SolicitudTransferenciaIDJson(int SolicitudID)
        {
            SolicitudTransferencia solicitud = new SolicitudTransferencia();
            string consulta = @"SELECT 
                                str.[SolicitudID]
                              ,str.[SolicitudSala]
                              ,str.[Codsala]
                             ,s.Nombre nombresala
                              ,str.[ClienteNombre]
                              ,str.[ClienteApelPat]
                              ,str.[ClienteApelMat]
                              ,str.[ClienteTipoDoc]
                              ,str.[ClienteNroDoc]
                              ,str.[Monto]
                              ,str.[NroTickets]
                              ,[Banco]
                              ,[Cuenta]
                              ,str.[Estado]
                              ,str.[FechaReg]
                              ,str.[UsuarioNombreReg]
                                FROM [dbo].[SolicitudTransferencia] str
                                left join Sala s on s.CodSala=str.Codsala
                                where str.[SolicitudID] =@pSolicitudID";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pSolicitudID", SolicitudID);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new SolicitudTransferencia
                            {
                                SolicitudID = ManejoNulos.ManageNullInteger(dr["SolicitudID"].Trim()),
                                SolicitudSala = ManejoNulos.ManageNullInteger(dr["SolicitudSala"].Trim()),
                                Codsala = ManejoNulos.ManageNullInteger(dr["Codsala"].Trim()),
                                nombresala = ManejoNulos.ManageNullStr(dr["nombresala"].Trim()),
                                ClienteNombre = ManejoNulos.ManageNullStr(dr["ClienteNombre"].Trim()),
                                ClienteApelPat = ManejoNulos.ManageNullStr(dr["ClienteApelPat"].Trim()),
                                ClienteApelMat = ManejoNulos.ManageNullStr(dr["ClienteApelMat"].Trim()),
                                TipoDocNombre = ManejoNulos.ManageNullStr(dr["ClienteTipoDoc"].Trim()),
                                ClienteNroDoc = ManejoNulos.ManageNullStr(dr["ClienteNroDoc"].Trim()),
                                Monto = ManejoNulos.ManageNullDouble(dr["Monto"]),
                                NroTickets = ManejoNulos.ManageNullStr(dr["NroTickets"].Trim()),
                                BancoNombre = ManejoNulos.ManageNullStr(dr["Banco"].Trim()),
                                NroCuenta = ManejoNulos.ManageNullStr(dr["Cuenta"].Trim()),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim()),
                                FechaReg = ManejoNulos.ManageNullDate(dr["FechaReg"]),
                                UsuarioNombreReg = ManejoNulos.ManageNullStr(dr["UsuarioNombreReg"].Trim()),
                            };
                            solicitud = item;
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
            return solicitud;
        }

        public List<SolicitudTransferencia> SolicitudTransferenciaActivasListarJson()
        {
            List<SolicitudTransferencia> lista = new List<SolicitudTransferencia>();
            string consulta = @"SELECT 
                                str.[SolicitudID]
                              ,str.[SolicitudSala]
                              ,str.[Codsala]
                             ,s.Nombre nombresala
                              ,str.[ClienteNombre]
                              ,str.[ClienteApelPat]
                              ,str.[ClienteApelMat]
                              ,str.[ClienteTipoDoc]
                              ,str.[ClienteNroDoc]
                              ,str.[Monto]
                              ,str.[NroTickets]
                              ,[Banco]
                              ,[Cuenta]
                              ,str.[Estado]
                              ,str.[FechaReg]
                              ,str.[UsuarioNombreReg]
                                FROM [dbo].[SolicitudTransferencia] str
                                left join Sala s on s.CodSala=str.Codsala
                                where str.[Estado] =0
                                order by str.FechaReg DESC";
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
                            var doi = new SolicitudTransferencia
                            {
                                SolicitudID = ManejoNulos.ManageNullInteger(dr["SolicitudID"].Trim()),
                                SolicitudSala = ManejoNulos.ManageNullInteger(dr["SolicitudSala"].Trim()),
                                Codsala = ManejoNulos.ManageNullInteger(dr["Codsala"].Trim()),
                                nombresala = ManejoNulos.ManageNullStr(dr["nombresala"].Trim()),
                                ClienteNombre = ManejoNulos.ManageNullStr(dr["ClienteNombre"].Trim()),
                                ClienteApelPat = ManejoNulos.ManageNullStr(dr["ClienteApelPat"].Trim()),
                                ClienteApelMat = ManejoNulos.ManageNullStr(dr["ClienteApelMat"].Trim()),
                                TipoDocNombre = ManejoNulos.ManageNullStr(dr["ClienteTipoDoc"].Trim()),
                                ClienteNroDoc = ManejoNulos.ManageNullStr(dr["ClienteNroDoc"].Trim()),
                                Monto = ManejoNulos.ManageNullFloat(dr["Monto"]),
                                NroTickets = ManejoNulos.ManageNullStr(dr["NroTickets"].Trim()),
                                BancoNombre = ManejoNulos.ManageNullStr(dr["Banco"].Trim()),
                                NroCuenta = ManejoNulos.ManageNullStr(dr["Cuenta"].Trim()),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"].Trim()),
                                FechaReg = ManejoNulos.ManageNullDate(dr["FechaReg"]),
                                UsuarioNombreReg = ManejoNulos.ManageNullStr(dr["UsuarioNombreReg"].Trim()),
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

        public bool SolicitudTransferenciaEstadonModificarJson(int SolicitudID)
        {

            string consulta = @"update SolicitudTransferencia set Estado = 1 where SolicitudID = @pSolicitudID";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pSolicitudID", SolicitudID);

                    query.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
                //Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
        }

        public List<SolicitudTicket> TicketSolicitudIDListadoJson(int SolicitudID)
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
    }
}
