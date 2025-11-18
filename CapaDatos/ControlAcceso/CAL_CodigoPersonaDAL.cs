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
    public class CAL_CodigoPersonaDAL
    {

        string conexion = string.Empty;
        public CAL_CodigoPersonaDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CAL_CodigoPersonaEntidad> GetAllCodigoPersona()
        {
            List<CAL_CodigoPersonaEntidad> lista = new List<CAL_CodigoPersonaEntidad>();
            string consulta = @"SELECT codPer.[Id]
                                  ,codPer.[TipoPersona]
                                  ,codPer.[CodigoID]
                                  ,codPer.[Editable]
                                  ,cod.[Alerta] as CodigoNombre
                              FROM [BD_SEGURIDAD_PJ].[dbo].[CAL_CodigoPersona] codPer
                              INNER JOIN [BD_SEGURIDAD_PJ].[dbo].[CAL_Codigo] cod
                                ON codPer.CodigoID=cod.CodigoID";
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
                            var item = new CAL_CodigoPersonaEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                TipoPersona = ManejoNulos.ManageNullStr(dr["TipoPersona"]),
                                CodigoID = ManejoNulos.ManageNullInteger(dr["CodigoID"]),
                                CodigoNombre = ManejoNulos.ManageNullStr(dr["CodigoNombre"]),
                                Editable = ManejoNulos.ManageNullInteger(dr["Editable"]),
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
        public CAL_CodigoPersonaEntidad GetIDCodigoPersona(int id)
        {
            CAL_CodigoPersonaEntidad item = new CAL_CodigoPersonaEntidad();
            string consulta = @"SELECT codPer.[Id]
                                  ,codPer.[TipoPersona]
                                  ,codPer.[CodigoID]
                                  ,codPer.[Editable]
                                  ,cod.[Alerta] as CodigoNombre
                              FROM [BD_SEGURIDAD_PJ].[dbo].[CAL_CodigoPersona] codPer
                              INNER JOIN [BD_SEGURIDAD_PJ].[dbo].[CAL_Codigo] cod
                                ON codPer.CodigoID=cod.CodigoID
                                WHERE Id = @pCodigoPersonaID ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoPersonaID", id);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                item.TipoPersona = ManejoNulos.ManageNullStr(dr["TipoPersona"]);
                                item.CodigoID = ManejoNulos.ManageNullInteger(dr["CodigoID"]);
                                item.CodigoNombre = ManejoNulos.ManageNullStr(dr["CodigoNombre"]);
                                item.Editable = ManejoNulos.ManageNullInteger(dr["Editable"]);
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
        public int InsertarCodigoPersona(CAL_CodigoPersonaEntidad Entidad)
        {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO CAL_CodigoPersona (TipoPersona,CodigoID,Editable)
                                OUTPUT Inserted.Id  
                                VALUES(@pTipoPersona,@pCodigoID,@pEditable)";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pTipoPersona", ManejoNulos.ManageNullStr(Entidad.TipoPersona));
                    query.Parameters.AddWithValue("@pCodigoID", ManejoNulos.ManageNullInteger(Entidad.CodigoID));
                    query.Parameters.AddWithValue("@pEditable", ManejoNulos.ManageNullInteger(Entidad.Editable));
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
        public bool EditarCodigoPersona(CAL_CodigoPersonaEntidad Entidad)
        {
            bool respuesta = false;
            string consulta = @"UPDATE CAL_CodigoPersona SET 
                                 TipoPersona = @pTipoPersona,
                                 CodigoID = @pCodigoID,
                                 Editable = @pEditable 
                                WHERE Id  = @pCodigoPersonaID";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoPersonaID", ManejoNulos.ManageNullInteger(Entidad.Id));
                    query.Parameters.AddWithValue("@pTipoPersona", ManejoNulos.ManageNullStr(Entidad.TipoPersona));
                    query.Parameters.AddWithValue("@pCodigoID", ManejoNulos.ManageNullInteger(Entidad.CodigoID));
                    query.Parameters.AddWithValue("@pEditable", ManejoNulos.ManageNullInteger(Entidad.Editable));
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
        public bool EliminarCodigoPersona(int id)
        {
            bool respuesta = false;
            string consulta = @"DELETE FROM CAL_CodigoPersona 
                                WHERE Id  = @pCodigoPersonaID";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@pCodigoPersonaID", ManejoNulos.ManageNullInteger(id));
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
