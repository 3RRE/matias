using CapaEntidad;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class DispositivoDAL
    {
        string _conexion = string.Empty;
        public DispositivoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<DispositivoEntidad> DispositivoListadoJson()
        {
            List<DispositivoEntidad> lista = new List<DispositivoEntidad>();
            string consulta = @"select d.DispositivoId, d.Mac, d.EsActivo from  Dispositivo d";
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
                            var item = new DispositivoEntidad
                            {
                                DispositivoId = ManejoNulos.ManageNullInteger(dr["DispositivoId"]),
                                Mac = ManejoNulos.ManageNullStr(dr["Mac"]),
                                EsActivo =  ManejoNulos.ManageNullInteger(dr["EsActivo"]),
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

        public bool DispositivoInsertarJson(DispositivoEntidad dispositivo)
        {
            bool response = false;
            string consulta = @"insert Dispositivo (Mac, EsActivo) values (@pMac , @pEsActivo)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con); 
                    query.Parameters.AddWithValue("@pMac", dispositivo.Mac);
                    query.Parameters.AddWithValue("@pEsActivo", dispositivo.EsActivo);
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
        public bool DispositivoEditarJson(DispositivoEntidad dispositivo)
        {
            bool response = false;
            string consulta = @"update Dispositivo set Mac = @pMac , EsActivo = @pEsActivo where DispositivoId = @pDispositivoId";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pDispositivoId", dispositivo.DispositivoId);
                    query.Parameters.AddWithValue("@pMac", dispositivo.Mac);
                    query.Parameters.AddWithValue("@pEsActivo", dispositivo.EsActivo); 
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
        public DispositivoEntidad DispositivoObtenerJson(int id)
        {
            DispositivoEntidad dispositivo = new DispositivoEntidad();
            string consulta = @"select d.DispositivoId , d.Mac , d.EsActivo from Dispositivo d where d.DispositivoId = @pDispositivoId";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pDispositivoId", id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                dispositivo.DispositivoId = ManejoNulos.ManageNullInteger(dr["DispositivoId"]);
                                dispositivo.Mac = ManejoNulos.ManageNullStr(dr["Mac"]); 
                                dispositivo.EsActivo = ManejoNulos.ManageNullInteger(dr["EsActivo"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dispositivo;
        }
        public bool ComprobarDispositivoJson(string mac)
        {
            var  dispositivo = false;
            string consulta = @" declare @respuesta bit
if exists (select  d.Mac  from Dispositivo d where d.mac = @pMac and EsActivo = 1)
begin
set @respuesta = 1
end
else
begin
set @respuesta = 0 
end

select @respuesta as respuesta";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pMac", mac);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                dispositivo = ManejoNulos.ManegeNullBool(dr["respuesta"]); 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return dispositivo;
            }
            return dispositivo;
        }
    }
}
