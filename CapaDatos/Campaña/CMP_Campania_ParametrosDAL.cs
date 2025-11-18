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
    public class CMP_Campania_ParametrosDAL
    {
        string _conexion = string.Empty;

        public CMP_Campania_ParametrosDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public CMP_Campania_ParametrosEntidad GetCampañaParametrosID(Int64 sala_id)
        {
            CMP_Campania_ParametrosEntidad campaña = new CMP_Campania_ParametrosEntidad();
            string consulta = @"SELECT [id]
      ,[sala_id]
,sa.Nombre
      ,[usuario_id]
      ,[condicion_juego]
      ,[condicion_tipo]
      ,[fecha_reg]
 FROM [dbo].[CMP_campaña_parametros] cpsala left join [Sala] sa on sa.CodSala=cpsala.sala_id  where sala_id=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sala_id);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                campaña.id = ManejoNulos.ManageNullInteger64(dr["id"]);
                                campaña.sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]);
                                campaña.sala_nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                                campaña.usuario_id = ManejoNulos.ManageNullInteger(dr["usuario_id"]);
                                campaña.condicion_juego = ManejoNulos.ManageNullInteger(dr["condicion_juego"]);
                                campaña.condicion_tipo = ManejoNulos.ManageNullInteger(dr["condicion_tipo"]);
                                campaña.fecha_reg = ManejoNulos.ManageNullDate(dr["fecha_reg"]);
                              
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
        public int GuardarCampañaParametros(CMP_Campania_ParametrosEntidad campaña)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[CMP_campaña_parametros]
           ([sala_id]
           ,[usuario_id]
           ,[condicion_juego]
           ,[condicion_tipo]
           ,[fecha_reg])
Output Inserted.id
     VALUES
           (@p0
           ,@p1
           ,@p2
           ,@p3
           ,@p4
          );";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(campaña.sala_id) == 0 ? SqlInt32.Null : campaña.sala_id);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(campaña.usuario_id));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(campaña.condicion_juego));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(campaña.condicion_tipo));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(campaña.fecha_reg));

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
        public bool EditarCampañaParametros(CMP_Campania_ParametrosEntidad campaña)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CMP_campaña_parametros]
                   SET [condicion_juego] = @p0
                  ,[condicion_tipo] = @p1
                  ,[fecha_reg] = @p2
                 WHERE sala_id=@p3                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(campaña.condicion_juego));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(campaña.condicion_tipo));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(campaña.fecha_reg));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger64(campaña.sala_id));

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
