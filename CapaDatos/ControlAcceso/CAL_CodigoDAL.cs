using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.Utilitarios;
using CapaEntidad.ControlAcceso;
using S3k.Utilitario;

namespace CapaDatos.ControlAcceso
{
    public class CAL_CodigoDAL
    {
        string conexion = string.Empty;
        public CAL_CodigoDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CAL_CodigoEntidad> GetAllCodigo()
        {
            List<CAL_CodigoEntidad> lista = new List<CAL_CodigoEntidad>();
            string consulta = @"SELECT [CodigoID]
                                      ,[Alerta]
                                      ,[Accion]
                                      ,[Color]
                                  FROM [CAL_Codigo](nolock)";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new CAL_CodigoEntidad
                            {
                                CodigoID = ManejoNulos.ManageNullInteger(dr["CodigoID"]),
                                Alerta = ManejoNulos.ManageNullStr(dr["Alerta"]),
                                Accion = ManejoNulos.ManageNullStr(dr["Accion"]),
                                Color = ManejoNulos.ManageNullStr(dr["Color"]),
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
        public List<CAL_CodigoEntidad> GetAllCodigoJoinCodigoPersona()
        {
            List<CAL_CodigoEntidad> lista = new List<CAL_CodigoEntidad>();
            string consulta = @"SELECT codigo.[CodigoID]
                                ,codigo.[Alerta]
                                ,codigo.[Accion]
                                ,codigo.[Color], codpers.TipoPersona
                                FROM [CAL_Codigo]as codigo
                                join CAL_CodigoPersona as codpers
                                on codigo.CodigoID=codpers.CodigoID ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);


                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new CAL_CodigoEntidad
                            {
                                CodigoID = ManejoNulos.ManageNullInteger(dr["CodigoID"]),
                                Alerta = ManejoNulos.ManageNullStr(dr["Alerta"]),
                                Accion = ManejoNulos.ManageNullStr(dr["Accion"]),
                                Color = ManejoNulos.ManageNullStr(dr["Color"]),
                                TipoPersona = ManejoNulos.ManageNullStr(dr["TipoPersona"]),
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
        public CAL_CodigoEntidad GetIDCodigo(int id)
        {
            CAL_CodigoEntidad item = new CAL_CodigoEntidad();
            string consulta = @"SELECT [CodigoID]
                                      ,[Alerta]
                                      ,[Accion]
                                      ,[Color]
                                FROM CAL_Codigo(nolock) 
                                WHERE CodigoID = @pCodigoID ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoID", id);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item.CodigoID = ManejoNulos.ManageNullInteger(dr["CodigoID"]);
                                item.Alerta = ManejoNulos.ManageNullStr(dr["Alerta"]);
                                item.Accion = ManejoNulos.ManageNullStr(dr["Accion"]);
                                item.Color = ManejoNulos.ManageNullStr(dr["Color"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return item;

        }
        public int InsertarCodigo(CAL_CodigoEntidad Entidad)
        {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO CAL_Codigo (Alerta,Accion,Color)
                                OUTPUT Inserted.CodigoID  
                                VALUES(@pAlerta,@pAccion,@pColor)";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pAlerta", ManejoNulos.ManageNullStr(Entidad.Alerta).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pAccion", ManejoNulos.ManageNullStr(Entidad.Accion));
                    query.Parameters.AddWithValue("@pColor", ManejoNulos.ManageNullStr(Entidad.Color));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool EditarCodigo(CAL_CodigoEntidad Entidad)
        {
            bool respuesta = false;
            string consulta = @"UPDATE CAL_Codigo SET 
                                 Alerta = @pAlerta,
                                 Accion = @pAccion,
                                 Color = @pColor 
                                WHERE CodigoID  = @pCodigoID";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoID", ManejoNulos.ManageNullInteger(Entidad.CodigoID));
                    query.Parameters.AddWithValue("@pAlerta", ManejoNulos.ManageNullStr(Entidad.Alerta).ToUpper().Trim());
                    query.Parameters.AddWithValue("@pAccion", ManejoNulos.ManageNullStr(Entidad.Accion));
                    query.Parameters.AddWithValue("@pColor", ManejoNulos.ManageNullStr(Entidad.Color));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
        public bool EliminarCodigo(int id)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM CAL_Codigo 
                                WHERE CodigoID  = @pCodigoID";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoID", ManejoNulos.ManageNullInteger(id));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }
    }
}
