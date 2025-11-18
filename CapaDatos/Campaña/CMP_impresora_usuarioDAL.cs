using CapaEntidad;
using CapaEntidad.Campañas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Campaña
{
    public class CMP_impresora_usuarioDAL
    {
        string _conexion = string.Empty;

        public CMP_impresora_usuarioDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<CMP_impresora_usuarioEntidad> GetListadoImpresoraUsuario()
        {
            List<CMP_impresora_usuarioEntidad> lista = new List<CMP_impresora_usuarioEntidad>();
            string consulta = @"SELECT iu.[id]
        ,iu.[usuario_id]
		,su.UsuarioNombre
        ,iu.[impresora_id]
		,cim.nombre
		,cim.ip
    FROM [dbo].[CMP_impresora_usuario] iu
	left join SEG_Usuario su on su.UsuarioID=iu.usuario_id 
	left join CMP_impresora cim on cim.id=iu.impresora_id
                                                         order by iu.impresora_id desc";
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
                            var impresora = new CMP_impresora_usuarioEntidad
                            {
                                id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                usuario_id = ManejoNulos.ManageNullInteger64(dr["usuario_id"]),
                                UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                impresora_id = ManejoNulos.ManageNullInteger64(dr["impresora_id"]),
                                nombre = ManejoNulos.ManageNullStr(dr["nombre"]),
                                ip = ManejoNulos.ManageNullStr(dr["ip"]),
                            };
                            lista.Add(impresora);
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

        public CMP_impresora_usuarioEntidad GetImpresoraUsuarioID(Int64 id)
        {
            CMP_impresora_usuarioEntidad campaña = new CMP_impresora_usuarioEntidad();
            string consulta = @"SELECT iu.[id]
        ,iu.[usuario_id]
		,su.UsuarioNombre
        ,iu.[impresora_id]
		,cim.nombre
		,cim.ip
    FROM [dbo].[CMP_impresora_usuario] iu
	left join SEG_Usuario su on su.UsuarioID=iu.usuario_id 
	left join CMP_impresora cim on cim.id=iu.impresora_id where iu.id=@p0";
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
                                campaña.id = ManejoNulos.ManageNullInteger64(dr["id"]);
                                campaña.usuario_id = ManejoNulos.ManageNullInteger64(dr["usuario_id"]);
                                campaña.UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]);
                                campaña.impresora_id = ManejoNulos.ManageNullInteger64(dr["impresora_id"]);
                                campaña.nombre = ManejoNulos.ManageNullStr(dr["nombre"]);
                                campaña.ip = ManejoNulos.ManageNullStr(dr["ip"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return campaña;
        }

        public int GuardarImpresoraUsuario(CMP_impresora_usuarioEntidad impresora)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[CMP_impresora_usuario]
           ([usuario_id]
           ,[impresora_id])
Output Inserted.id
     VALUES
           (@p0
           ,@p1
          );";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger64(impresora.usuario_id) == 0 ? SqlInt64.Null : impresora.usuario_id);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger64(impresora.impresora_id));
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
        public bool EditarImpresoraUsuario(CMP_impresora_usuarioEntidad impresora)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CMP_impresora_usuario]
                   SET [usuario_id] = @p0
                      ,[impresora_id] = @p1  
                 WHERE id=@p2                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger64(impresora.usuario_id) == 0 ? SqlInt64.Null : impresora.usuario_id);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger64(impresora.impresora_id));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger64(impresora.id));
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

        public bool EliminarImpresoraUsuario(Int64 id)
        {
            bool response = false;
            string consulta = @"Delete from [CMP_impresora_usuario]
                 WHERE id=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
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
    }
}
