using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class BancoCuentaDAL
    {
        string _conexion = string.Empty;
        public BancoCuentaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<BancoCuentaEntidad> BancoCuentaListadoJson()
        {
            List<BancoCuentaEntidad> lista = new List<BancoCuentaEntidad>();
            string consulta = @"select [BancoCuentaID]
      ,[Banco]
      ,[ClienteID]
      ,[NroCuenta]
  FROM [dbo].[BancoCuenta]";
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
                            var item = new BancoCuentaEntidad
                            {
                                BancoCuentaID = ManejoNulos.ManageNullInteger(dr["BancoCuentaID"]),
                                Banco = ManejoNulos.ManageNullStr(dr["Banco"]),
                                ClienteID = ManejoNulos.ManageNullInteger(dr["ClienteID"]),
                                NroCuenta = ManejoNulos.ManageNullStr(dr["NroCuenta"]),
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
        public List<BancoCuentaEntidad> BancoCuentaclienteidListadoJson(int id)
        {
            List<BancoCuentaEntidad> lista = new List<BancoCuentaEntidad>();
            string consulta = @"select [BancoCuentaID]
      ,[Banco]
      ,[ClienteID]
      ,[NroCuenta]
  FROM [dbo].[BancoCuenta] where clienteID=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new BancoCuentaEntidad
                            {
                                BancoCuentaID = ManejoNulos.ManageNullInteger(dr["BancoCuentaID"]),
                                Banco = ManejoNulos.ManageNullStr(dr["Banco"]),
                                ClienteID = ManejoNulos.ManageNullInteger(dr["ClienteID"]),
                                NroCuenta = ManejoNulos.ManageNullStr(dr["NroCuenta"]),
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
        public BancoCuentaEntidad BancoCuentaObtenerJson(int id)
        {
            BancoCuentaEntidad bancoCuenta = new BancoCuentaEntidad();
            string consulta = @"select [BancoCuentaID]
      ,[Banco]
      ,[ClienteID]
      ,[NroCuenta]
  FROM [dbo].[BancoCuenta] where BancoCuentaID=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                bancoCuenta.BancoCuentaID = ManejoNulos.ManageNullInteger(dr["BancoCuentaID"]);
                                bancoCuenta.Banco = ManejoNulos.ManageNullStr(dr["Banco"]);
                                bancoCuenta.ClienteID = ManejoNulos.ManageNullInteger(dr["ClienteID"]);
                                bancoCuenta.NroCuenta = ManejoNulos.ManageNullStr(dr["NroCuenta"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return bancoCuenta;
        }
        public bool BancoCuentaInsertarJson(BancoCuentaEntidad bancoCuenta)
        {
            bool response = false;
            string consulta = @"insert into [dbo].[BancoCuenta](Banco,ClienteID,NroCuenta) values(@p0,@p1,@p2)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", bancoCuenta.Banco);
                    query.Parameters.AddWithValue("@p1", bancoCuenta.ClienteID);
                    query.Parameters.AddWithValue("@p2", bancoCuenta.NroCuenta);
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
        public bool BancoCuentaEditarJson(BancoCuentaEntidad bancoCuenta)
        {
            bool response = false;
            string consulta = @"update BancoCuenta set Banco=@p0,ClienteID=@p1,NroCuenta=@p2 where BancoCuentaID=@p3";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", bancoCuenta.Banco);
                    query.Parameters.AddWithValue("@p1", bancoCuenta.ClienteID);
                    query.Parameters.AddWithValue("@p2", bancoCuenta.NroCuenta);
                    query.Parameters.AddWithValue("@p3", bancoCuenta.BancoCuentaID);
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
