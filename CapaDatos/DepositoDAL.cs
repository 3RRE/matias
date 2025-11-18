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
    public class DepositoDAL
    {
        string _conexion = string.Empty;
        public DepositoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public int DepositoInsertarJson(DepositoEntidad deposito)
        {
            int idInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[Deposito]
                           ([DepositoSala]
                           ,[Codsala]
                           ,[ClienteNombre]
                           ,[ClienteApelPat]
                           ,[ClienteApelMat]
                           ,[ClienteTipoDoc]
                           ,[ClienteNroDoc]
                           ,[Monto]
                           ,[NroTickets]
                           ,[Estado]
                           ,[NroOperacion]
                           ,[FechaReg],[FechaAct],[UsuarioNombreReg])
                        Output Inserted.DepositoID
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
                            ,@p10,@p11,@p12,@p13)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", deposito.DepositoSala);
                    query.Parameters.AddWithValue("@p1", deposito.Codsala);
                    query.Parameters.AddWithValue("@p2", deposito.ClienteNombre);
                    query.Parameters.AddWithValue("@p3", deposito.ClienteApelPat);
                    query.Parameters.AddWithValue("@p4", deposito.ClienteApelMat);
                    query.Parameters.AddWithValue("@p5", deposito.TipoDocNombre);
                    query.Parameters.AddWithValue("@p6", deposito.ClienteNroDoc);
                    query.Parameters.AddWithValue("@p7", deposito.Monto);
                    query.Parameters.AddWithValue("@p8", deposito.NroTickets);
                    query.Parameters.AddWithValue("@p9", deposito.Estado);
                    query.Parameters.AddWithValue("@p10", deposito.NroOperacion);
                    query.Parameters.AddWithValue("@p11", deposito.FechaReg);
                    query.Parameters.AddWithValue("@p12", deposito.FechaAct);
                    query.Parameters.AddWithValue("@p13", deposito.UsuarioNombreReg);
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
        public bool DepositoEditarJson(DepositoEntidad deposito)
        {
            bool response = false;
            string consulta = @"UPDATE [dbo].[Deposito]
                               SET [DepositoSala] = @p0
                                  ,[Codsala] =@p1
                                  ,[ClienteNombre] = @p2
                                  ,[ClienteApelPat] = @p3
                                  ,[ClienteApelMat] = @p4
                                  ,[ClienteTipoDoc] = @p5
                                  ,[ClienteNroDoc] = @p6
                                  ,[Monto] = @p7
                                  ,[NroTickets] = @p8
                                  ,[Estado] = @p9
                                  ,[NroOperacion] = @p10
                                  ,[FechaReg] = @p11
                                  ,[FechaAct] = @p12
                                    ,[UsuarioNombreReg]=@p13
                    
                             WHERE DepositoID=@p14 ;";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", deposito.DepositoSala);
                    query.Parameters.AddWithValue("@p1", deposito.Codsala);
                    query.Parameters.AddWithValue("@p2", deposito.ClienteNombre);
                    query.Parameters.AddWithValue("@p3", deposito.ClienteApelPat);
                    query.Parameters.AddWithValue("@p4", deposito.ClienteApelMat);
                    query.Parameters.AddWithValue("@p5", deposito.TipoDocNombre);
                    query.Parameters.AddWithValue("@p6", deposito.ClienteNroDoc);
                    query.Parameters.AddWithValue("@p7", deposito.Monto);
                    query.Parameters.AddWithValue("@p8", deposito.NroTickets);
                    query.Parameters.AddWithValue("@p9", deposito.Estado);
                    query.Parameters.AddWithValue("@p10", deposito.NroOperacion);
                    query.Parameters.AddWithValue("@p11", deposito.FechaReg);
                    query.Parameters.AddWithValue("@p12", deposito.FechaAct);
                    query.Parameters.AddWithValue("@p13", deposito.UsuarioNombreReg);
                    query.Parameters.AddWithValue("@p14", deposito.DepositoID);
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
        public List<DepositoEntidad> DepositoListarDepositoSalaJson(int DepositoSala)
        {
            List<DepositoEntidad> lista = new List<DepositoEntidad>();
            string consulta = @"SELECT [DepositoID]
                                          ,[DepositoSala]
                                          ,[Codsala]
                                          ,[ClienteNombre]
                                          ,[ClienteApelPat]
                                          ,[ClienteApelMat]
                                          ,[ClienteTipoDoc]
                                          ,[ClienteNroDoc]
                                          ,[Monto]
                                          ,[NroTickets]
                                          ,[Estado]
                                          ,[NroOperacion]
                                          ,[FechaReg]
                                          ,[FechaAct],[UsuarioNombreReg]
                                      FROM [dbo].[Deposito] where DepositoSala=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", DepositoSala);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new DepositoEntidad
                            {
                                DepositoID = ManejoNulos.ManageNullInteger(dr["DepositoID"]),
                                DepositoSala = ManejoNulos.ManageNullInteger(dr["DepositoSala"]),
                                Codsala = ManejoNulos.ManageNullInteger(dr["Codsala"]),
                                ClienteNombre = ManejoNulos.ManageNullStr(dr["ClienteNombre"]),
                                ClienteApelPat = ManejoNulos.ManageNullStr(dr["ClienteApelPat"]),
                                ClienteApelMat = ManejoNulos.ManageNullStr(dr["ClienteApelMat"]),
                                TipoDocNombre = ManejoNulos.ManageNullStr(dr["ClienteTipoDoc"]),
                                ClienteNroDoc = ManejoNulos.ManageNullStr(dr["ClienteNroDoc"]),
                                Monto = ManejoNulos.ManageNullDouble(dr["Monto"]),
                                NroTickets = ManejoNulos.ManageNullStr(dr["NroTickets"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                NroOperacion = ManejoNulos.ManageNullStr(dr["NroOperacion"]),
                                FechaReg = ManejoNulos.ManageNullDate(dr["FechaReg"]),
                                FechaAct = ManejoNulos.ManageNullDate(dr["FechaAct"]),
                                UsuarioNombreReg = ManejoNulos.ManageNullStr(dr["FechaAct"]),
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
