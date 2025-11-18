using CapaEntidad.Administrativo;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Administrativo
{
    public class ADM_MaquinaSalaProgresivoDAL
    {
        string _conexion = string.Empty;

        public ADM_MaquinaSalaProgresivoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ADM_MaquinaSalaProgresivoEntidad> GetListadoADM_MaquinaSalaProgresivoPorCodSalaProgresivo(int CodSalaProgresivo)
        {
            List<ADM_MaquinaSalaProgresivoEntidad> lista = new List<ADM_MaquinaSalaProgresivoEntidad>();
            string consulta = @"SELECT [CodMaquinaSalaProgresivo]
                                      ,[CodMaquina]
                                      ,[CodSalaProgresivo]
                                      ,[FechaEnlace]
                                      ,[FechaDesactivacion]
                                      ,[FechaRegistro]
                                      ,[FechaModificacion]
                                      ,[Activo]
                                      ,[Estado]
                                      ,[CodUsuario]
                                  FROM [dbo].[ADM_MaquinaSalaProgresivo]
                                  where [CodSalaProgresivo]=@CodSalaProgresivo";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSalaProgresivo", CodSalaProgresivo);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var progresivoSala = new ADM_MaquinaSalaProgresivoEntidad
                            {
                                CodMaquinaSalaProgresivo = ManejoNulos.ManageNullInteger(dr["CodMaquinaSalaProgresivo"]),
                                CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                CodSalaProgresivo = ManejoNulos.ManageNullInteger(dr["CodSalaProgresivo"]),
                                FechaEnlace = ManejoNulos.ManageNullDate(dr["FechaEnlace"]),
                                FechaDesactivacion = ManejoNulos.ManageNullDate(dr["FechaDesactivacion"]),
                                FechaRegistro= ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado= ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CodUsuario= ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                            };

                            lista.Add(progresivoSala);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return new List<ADM_MaquinaSalaProgresivoEntidad>();
            }
            return lista;
        }
        public bool EditarADM_MaquinaSalaProgresivo(ADM_MaquinaSalaProgresivoEntidad progresivo)
        {
            bool respuesta = false;
            string consulta = @"
UPDATE [dbo].[ADM_MaquinaSalaProgresivo]
   SET [CodMaquina] = @CodMaquina
      ,[CodSalaProgresivo] = @CodSalaProgresivo
      ,[FechaEnlace] = @FechaEnlace
      ,[FechaDesactivacion] = @FechaDesactivacion
      ,[FechaRegistro] = @FechaRegistro
      ,[FechaModificacion] = @FechaModificacion
      ,[Activo] = @Activo
      ,[Estado] = @Estado
      ,[CodUsuario] = @CodUsuario
 WHERE CodMaquinaSalaProgresivo=@CodMaquinaSalaProgresivo     ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullInteger(progresivo.CodMaquina));
                    query.Parameters.AddWithValue("@CodSalaProgresivo", ManejoNulos.ManageNullInteger(progresivo.CodSalaProgresivo));
                    query.Parameters.AddWithValue("@FechaEnlace", ManejoNulos.ManageNullDate(progresivo.FechaEnlace));
                    query.Parameters.AddWithValue("@FechaDesactivacion", ManejoNulos.ManageNullDate(progresivo.FechaDesactivacion));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(progresivo.FechaRegistro));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(progresivo.FechaModificacion));
                    query.Parameters.AddWithValue("@Activo", ManejoNulos.ManegeNullBool(progresivo.Activo));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(progresivo.Estado));
                    query.Parameters.AddWithValue("@CodUsuario", ManejoNulos.ManageNullStr(progresivo.CodUsuario));
                    query.Parameters.AddWithValue("@CodMaquinaSalaProgresivo", ManejoNulos.ManageNullInteger(progresivo.CodMaquinaSalaProgresivo));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
            }

            return respuesta;
        }
        public int GuardarADM_MaquinaSalaProgresivo(ADM_MaquinaSalaProgresivoEntidad progresivo)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[ADM_MaquinaSalaProgresivo]
           ([CodMaquina]
           ,[CodSalaProgresivo]
           ,[FechaEnlace]
           ,[FechaDesactivacion]
           ,[FechaRegistro]
           ,[FechaModificacion]
           ,[Activo]
           ,[Estado]
           ,[CodUsuario])
Output Inserted.CodMaquinaSalaProgresivo
     VALUES
           (@CodMaquina
           ,@CodSalaProgresivo
           ,@FechaEnlace
           ,@FechaDesactivacion
           ,@FechaRegistro
           ,@FechaModificacion
           ,@Activo
           ,@Estado
           ,@CodUsuario)
;";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullInteger(progresivo.CodMaquina));
                    query.Parameters.AddWithValue("@CodSalaProgresivo", ManejoNulos.ManageNullInteger(progresivo.CodSalaProgresivo));
                    query.Parameters.AddWithValue("@FechaEnlace", ManejoNulos.ManageNullDate(progresivo.FechaEnlace));
                    query.Parameters.AddWithValue("@FechaDesactivacion", ManejoNulos.ManageNullDate(progresivo.FechaDesactivacion));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(progresivo.FechaRegistro));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(progresivo.FechaModificacion));
                    query.Parameters.AddWithValue("@Activo", ManejoNulos.ManegeNullBool(progresivo.Activo));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(progresivo.Estado));
                    query.Parameters.AddWithValue("@CodUsuario", ManejoNulos.ManageNullStr(progresivo.CodUsuario));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                    //query.ExecuteNonQuery();
                    //respuesta = true;
                }
            }
            catch (Exception ex)
            {
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public ADM_MaquinaSalaProgresivoEntidad GetADM_MaquinaSalaProgresivoPorCodSalaProgresivoyCodMaquina(int CodSalaProgresivo,int CodMaquina)
        {
            ADM_MaquinaSalaProgresivoEntidad result = new ADM_MaquinaSalaProgresivoEntidad();
            string consulta = @"SELECT [CodMaquinaSalaProgresivo]
                                      ,[CodMaquina]
                                      ,[CodSalaProgresivo]
                                      ,[FechaEnlace]
                                      ,[FechaDesactivacion]
                                      ,[FechaRegistro]
                                      ,[FechaModificacion]
                                      ,[Activo]
                                      ,[Estado]
                                      ,[CodUsuario]
                                  FROM [dbo].[ADM_MaquinaSalaProgresivo]
                                  where [CodSalaProgresivo]=@CodSalaProgresivo and [CodMaquina]=@CodMaquina";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSalaProgresivo", CodSalaProgresivo);
                    query.Parameters.AddWithValue("@CodMaquina", CodMaquina);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            result.CodMaquinaSalaProgresivo = ManejoNulos.ManageNullInteger(dr["CodMaquinaSalaProgresivo"]);
                            result.CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]);
                            result.CodSalaProgresivo = ManejoNulos.ManageNullInteger(dr["CodSalaProgresivo"]);
                            result.FechaEnlace = ManejoNulos.ManageNullDate(dr["FechaEnlace"]);
                            result.FechaDesactivacion = ManejoNulos.ManageNullDate(dr["FechaDesactivacion"]);
                            result.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                            result.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                            result.Activo = ManejoNulos.ManegeNullBool(dr["Activo"]);
                            result.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                            result.CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return new ADM_MaquinaSalaProgresivoEntidad();
            }
            return result;
        }

        // Lista máquinas ACTIVAS (m.Activo=1 && m.Estado=1) que NO tienen asignación ACTIVA
        // Opcionalmente filtra por sala (codSala).
        public List<ADM_MaquinaEntidad> GetMaquinasActivasSinAsignacion(int? codSala = null) {
            var lista = new List<ADM_MaquinaEntidad>();

            var sql = new StringBuilder(@"
                        SELECT  m.CodMaquina,
                                m.CodMaquinaLey,
                                m.CodAlterno,
                                m.CodSala,
                                m.Activo,
                                m.Estado
                        FROM    dbo.ADM_Maquina AS m
                        WHERE   m.Activo = 1
                            AND m.Estado = 1
                            AND NOT EXISTS (
                                SELECT 1
                                FROM dbo.ADM_MaquinaSalaProgresivo AS ms
                                WHERE ms.CodMaquina = m.CodMaquina
                                  AND ms.Activo = 1
                                  AND ms.Estado = 1
                            );
                        ");

            if(codSala.HasValue && codSala.Value > 0)
                sql.Replace("/**FILTRO_SALA**/", "AND m.CodSala = @CodSala");
            else
                sql.Replace("/**FILTRO_SALA**/", "");

            try {
                using(var con = new SqlConnection(_conexion))
                using(var cmd = new SqlCommand(sql.ToString(), con)) {
                    if(codSala.HasValue && codSala.Value > 0)
                        cmd.Parameters.AddWithValue("@CodSala", codSala.Value);

                    con.Open();
                    using(var dr = cmd.ExecuteReader()) {
                        while(dr.Read()) {
                            lista.Add(new ADM_MaquinaEntidad {
                                CodMaquina = ManejoNulos.ManageNullInteger(dr["CodMaquina"]),
                                CodMaquinaLey = ManejoNulos.ManageNullStr(dr["CodMaquinaLey"]),
                                CodAlterno = ManejoNulos.ManageNullStr(dr["CodAlterno"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"])
                            });
                        }
                    }
                }
            } catch(Exception) {
                return new List<ADM_MaquinaEntidad>();
            }

            return lista;
        }

    }
}
