using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class ClienteDAL
    {
        string _conexion = string.Empty;
        public ClienteDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<Cliente> ClienteListadoJson()
        {
            List<Cliente> lista = new List<Cliente>();
            string consulta = @"select [ClienteID]
      ,[ClienteNombre]
      ,[ClienteApelPat]
      ,[ClienteApelMat]
      ,[ClienteTipoDoc]
      ,[ClienteNroDoc]
      ,[FechaReg]
      ,[FechaAct] from  [Cliente]";
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
                            var item = new Cliente
                            {
                                ClienteID = ManejoNulos.ManageNullInteger(dr["ClienteID"]),
                                ClienteNombre = ManejoNulos.ManageNullStr(dr["ClienteNombre"]),
                                ClienteApelPat = ManejoNulos.ManageNullStr(dr["ClienteApelPat"]),
                                ClienteApelMat = ManejoNulos.ManageNullStr(dr["ClienteApelMat"]),
                                ClienteTipoDoc = ManejoNulos.ManageNullStr(dr["ClienteTipoDoc"]),
                                ClienteNroDoc = ManejoNulos.ManageNullStr(dr["ClienteNroDoc"]),
                                FechaReg = ManejoNulos.ManageNullDate(dr["FechaReg"]),
                                FechaAct = ManejoNulos.ManageNullDate(dr["FechaAct"]),
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

        public List<Cliente> ClienteBuscarJson(string nrodoc)
        {
            List<Cliente> lista = new List<Cliente>();
            string consulta = @"select [ClienteID]
      ,[ClienteNombre]
      ,[ClienteApelPat]
      ,[ClienteApelMat]
      ,[ClienteTipoDoc]
      ,[ClienteNroDoc]
      ,[FechaReg]
      ,[FechaAct] from  [Cliente] where ClienteNroDoc=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", nrodoc);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new Cliente
                            {
                                ClienteID = ManejoNulos.ManageNullInteger(dr["ClienteID"]),
                                ClienteNombre = ManejoNulos.ManageNullStr(dr["ClienteNombre"]),
                                ClienteApelPat = ManejoNulos.ManageNullStr(dr["ClienteApelPat"]),
                                ClienteApelMat = ManejoNulos.ManageNullStr(dr["ClienteApelMat"]),
                                ClienteTipoDoc = ManejoNulos.ManageNullStr(dr["ClienteTipoDoc"]),
                                ClienteNroDoc = ManejoNulos.ManageNullStr(dr["ClienteNroDoc"]),
                                FechaReg = ManejoNulos.ManageNullDate(dr["FechaReg"]),
                                FechaAct = ManejoNulos.ManageNullDate(dr["FechaAct"]),
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
        public bool ClienteInsertarJson(Cliente cliente)
        {
            bool response = false;
            string consulta = @"insert into Cliente (ClienteNombre,ClienteApelPat,ClienteApelMat,ClienteTipoDoc,ClienteNroDoc,FechaReg) values 
(@ClienteNombre,@ClienteApelPat,@ClienteApelMat,@ClienteTipoDoc,@ClienteNroDoc,@FechaReg)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@ClienteNombre", cliente.ClienteNombre);
                    query.Parameters.AddWithValue("@ClienteApelPat", cliente.ClienteApelPat);
                    query.Parameters.AddWithValue("@ClienteApelMat", cliente.ClienteApelMat);
                    query.Parameters.AddWithValue("@ClienteTipoDoc", cliente.ClienteTipoDoc);
                    query.Parameters.AddWithValue("@ClienteNroDoc", cliente.ClienteNroDoc);
                    query.Parameters.AddWithValue("@FechaReg", cliente.FechaReg);
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
        public bool ClienteEditarJson(Cliente cliente)
        {
            bool response = false;
            string consulta = @"update Cliente set ClienteNombre = @ClienteNombre , ClienteApelPat = @ClienteApelPat , ClienteApelMat = @ClienteApelMat , ClienteTipoDoc = @ClienteTipoDoc , ClienteNroDoc = @ClienteNroDoc , FechaReg = @FechaReg  where ClienteID = @ClienteID";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@ClienteID", cliente.ClienteID);
                    query.Parameters.AddWithValue("@ClienteNombre", cliente.ClienteNombre);
                    query.Parameters.AddWithValue("@ClienteApelPat", cliente.ClienteApelPat);
                    query.Parameters.AddWithValue("@ClienteApelMat", cliente.ClienteApelMat);
                    query.Parameters.AddWithValue("@ClienteTipoDoc", cliente.ClienteTipoDoc);
                    query.Parameters.AddWithValue("@ClienteNroDoc", cliente.ClienteNroDoc);
                    query.Parameters.AddWithValue("@FechaReg", cliente.FechaReg);
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
