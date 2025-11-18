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
    public class ADM_PozoHistoricoDAL
    {
        string _conexion = string.Empty;

        public ADM_PozoHistoricoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ADM_PozoHistoricoEntidad> GetListadoADM_PozoHistoricoPorCodDetalleSalaProgresivoYFecha(int CodDetalleSalaProgresivo, DateTime FechaOperacion)
        {
            List<ADM_PozoHistoricoEntidad> lista = new List<ADM_PozoHistoricoEntidad>();
            string consulta = @"SELECT [CodPozoHistorico]
                          ,[CodDetalleSalaProgresivo]
                          ,[MontoActualAutomatico]
                          ,[MontoActualSala]
                          ,[MontoOcultoActualAutomatico]
                          ,[MontoOcultoActualSala]
                          ,[FechaOperacion]
                          ,[FechaRegistro]
                          ,[FechaModificacion]
                          ,[Estado]
                          ,[Activo]
                          ,[CodUsuario]
                      FROM [dbo].[ADM_PozoHistorico] where CodDetalleSalaProgresivo=@CodDetalleSalaProgresivo
                        and convert(date,FechaOperacion)=Convert(date,@FechaOperacion)
";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodDetalleSalaProgresivo", CodDetalleSalaProgresivo);
                    query.Parameters.AddWithValue("@FechaOperacion", FechaOperacion);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var progresivoSala = new ADM_PozoHistoricoEntidad
                            {
                                CodPozoHistorico = ManejoNulos.ManageNullInteger(dr["CodPozoHistorico"]),
                                CodDetalleSalaProgresivo = ManejoNulos.ManageNullInteger(dr["CodDetalleSalaProgresivo"]),
                                MontoActualAutomatico = ManejoNulos.ManageNullDecimal(dr["MontoActualAutomatico"]),
                                MontoActualSala= ManejoNulos.ManageNullDecimal(dr["MontoActualSala"]),
                                MontoOcultoActualAutomatico = ManejoNulos.ManageNullDecimal(dr["MontoOcultoActualAutomatico"]),
                                MontoOcultoActualSala = ManejoNulos.ManageNullDecimal(dr["MontoOcultoActualSala"]),
                                FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                            };

                            lista.Add(progresivoSala);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return new List<ADM_PozoHistoricoEntidad>();
            }
            return lista;
        }
        public bool EditarADM_PozoHistorico(ADM_PozoHistoricoEntidad progresivo)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[ADM_PozoHistorico]
   SET [CodDetalleSalaProgresivo] = @CodDetalleSalaProgresivo
      ,[MontoActualAutomatico] = @MontoActualAutomatico
      ,[MontoActualSala] = @MontoActualSala
      ,[MontoOcultoActualAutomatico] = @MontoOcultoActualAutomatico
      ,[MontoOcultoActualSala] = @MontoOcultoActualSala
      ,[FechaOperacion] = @FechaOperacion
      ,[FechaRegistro] = @FechaRegistro
      ,[FechaModificacion] = @FechaModificacion
      ,[Estado] = @Estado
      ,[Activo] = @Activo
      ,[CodUsuario] = @CodUsuario
 WHERE CodPozoHistorico=@CodPozoHistorico";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@CodDetalleSalaProgresivo", ManejoNulos.ManageNullInteger(progresivo.CodDetalleSalaProgresivo));
                    query.Parameters.AddWithValue("@MontoActualAutomatico", ManejoNulos.ManageNullDecimal(progresivo.MontoActualAutomatico));
                    query.Parameters.AddWithValue("@MontoActualSala", ManejoNulos.ManageNullDecimal(progresivo.MontoActualSala));
                    query.Parameters.AddWithValue("@MontoOcultoActualAutomatico", ManejoNulos.ManageNullDecimal(progresivo.MontoOcultoActualAutomatico));
                    query.Parameters.AddWithValue("@MontoOcultoActualSala", ManejoNulos.ManageNullDecimal(progresivo.MontoOcultoActualSala));
                    query.Parameters.AddWithValue("@FechaOperacion", ManejoNulos.ManageNullDate(progresivo.FechaOperacion));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(progresivo.FechaRegistro));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(progresivo.FechaModificacion));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(progresivo.Estado));
                    query.Parameters.AddWithValue("@Activo", ManejoNulos.ManegeNullBool(progresivo.Activo));
                    query.Parameters.AddWithValue("@CodUsuario", ManejoNulos.ManageNullStr(progresivo.CodUsuario));
                    query.Parameters.AddWithValue("@CodPozoHistorico", ManejoNulos.ManageNullInteger(progresivo.CodPozoHistorico));
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
        public int GuardarADM_PozoHistorico(ADM_PozoHistoricoEntidad progresivo)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"

INSERT INTO [dbo].[ADM_PozoHistorico]
           ([CodDetalleSalaProgresivo]
           ,[MontoActualAutomatico]
           ,[MontoActualSala]
           ,[MontoOcultoActualAutomatico]
           ,[MontoOcultoActualSala]
           ,[FechaOperacion]
           ,[FechaRegistro]
           ,[FechaModificacion]
           ,[Estado]
           ,[Activo]
           ,[CodUsuario])
Output Inserted.CodPozoHistorico
     VALUES
           (@CodDetalleSalaProgresivo
           ,@MontoActualAutomatico
           ,@MontoActualSala
           ,@MontoOcultoActualAutomatico
           ,@MontoOcultoActualSala
           ,@FechaOperacion
           ,@FechaRegistro
           ,@FechaModificacion
           ,@Estado
           ,@Activo
           ,@CodUsuario)
;";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodDetalleSalaProgresivo", ManejoNulos.ManageNullInteger(progresivo.CodDetalleSalaProgresivo));
                    query.Parameters.AddWithValue("@MontoActualAutomatico", ManejoNulos.ManageNullDecimal(progresivo.MontoActualAutomatico));
                    query.Parameters.AddWithValue("@MontoActualSala", ManejoNulos.ManageNullDecimal(progresivo.MontoActualSala));
                    query.Parameters.AddWithValue("@MontoOcultoActualAutomatico", ManejoNulos.ManageNullDecimal(progresivo.MontoOcultoActualAutomatico));
                    query.Parameters.AddWithValue("@MontoOcultoActualSala", ManejoNulos.ManageNullDecimal(progresivo.MontoOcultoActualSala));
                    query.Parameters.AddWithValue("@FechaOperacion", ManejoNulos.ManageNullDate(progresivo.FechaOperacion));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(progresivo.FechaRegistro));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(progresivo.FechaModificacion));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(progresivo.Estado));
                    query.Parameters.AddWithValue("@Activo", ManejoNulos.ManegeNullBool(progresivo.Activo));
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


        public List<ADM_PozoHistoricoEntidad> GetHistoricoPorDetalle(int codDetalleSalaProgresivo) {
            var lista = new List<ADM_PozoHistoricoEntidad>();
            string sql = @"
SELECT [CodPozoHistorico],
       [CodDetalleSalaProgresivo],
       [MontoActualAutomatico],
       [MontoActualSala],
       [MontoOcultoActualAutomatico],
       [MontoOcultoActualSala],
       [FechaOperacion],
       [FechaRegistro],
       [FechaModificacion],
       [Estado],
       [Activo],
       [CodUsuario]
FROM [dbo].[ADM_PozoHistorico]
WHERE CodDetalleSalaProgresivo = @CodDetalleSalaProgresivo
ORDER BY FechaOperacion DESC, FechaRegistro DESC, CodPozoHistorico DESC;";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(sql, con)) {
                        cmd.Parameters.AddWithValue("@CodDetalleSalaProgresivo", codDetalleSalaProgresivo);
                        using(var dr = cmd.ExecuteReader()) {
                            while(dr.Read()) {
                                lista.Add(new ADM_PozoHistoricoEntidad {
                                    CodPozoHistorico = ManejoNulos.ManageNullInteger(dr["CodPozoHistorico"]),
                                    CodDetalleSalaProgresivo = ManejoNulos.ManageNullInteger(dr["CodDetalleSalaProgresivo"]),
                                    MontoActualAutomatico = ManejoNulos.ManageNullDecimal(dr["MontoActualAutomatico"]),
                                    MontoActualSala = ManejoNulos.ManageNullDecimal(dr["MontoActualSala"]),
                                    MontoOcultoActualAutomatico = ManejoNulos.ManageNullDecimal(dr["MontoOcultoActualAutomatico"]),
                                    MontoOcultoActualSala = ManejoNulos.ManageNullDecimal(dr["MontoOcultoActualSala"]),
                                    FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                    FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                    Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                    Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                    CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                });
                            }
                        }
                    }
                }
            } catch {
                return new List<ADM_PozoHistoricoEntidad>();
            }

            return lista;
        }

    }
}
