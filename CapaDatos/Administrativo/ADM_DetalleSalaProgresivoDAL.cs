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
    public class ADM_DetalleSalaProgresivoDAL
    {
        string _conexion = string.Empty;

        public ADM_DetalleSalaProgresivoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ADM_DetalleSalaProgresivoEntidad> GetListadoADM_DetalleSalaProgresivoPorCodSalaProgresivo(int CodSalaProgresivo)
        {
            List<ADM_DetalleSalaProgresivoEntidad> lista = new List<ADM_DetalleSalaProgresivoEntidad>();
            string consulta = @"SELECT [CodDetalleSalaProgresivo]
                                      ,[CodSalaProgresivo]
                                      ,[NroPozo]
                                      ,[NombrePozo]
                                      ,[Dificultad]
                                      ,[MontoBase]
                                      ,[MontoIni]
                                      ,[MontoFin]
                                      ,[Modalidad]
                                      ,[MontoOcultoBase]
                                      ,[MontoOcultoIni]
                                      ,[MontoOcultoFin]
                                      ,[Incremento]
                                      ,[IncrementoPozoOculto]
                                      ,[FechaRegistro]
                                      ,[FechaModificacion]
                                      ,[Activo]
                                      ,[Estado]
                                      ,[CodProgresivoExterno]
                                      ,[CodUsuario]
                                      ,[fechaIni]
                                      ,[fechaFin]
                                  FROM [dbo].[ADM_DetalleSalaProgresivo] where CodSalaProgresivo=@CodSalaProgresivo";
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
                            var progresivoSala = new ADM_DetalleSalaProgresivoEntidad
                            {
                                CodDetalleSalaProgresivo = ManejoNulos.ManageNullInteger(dr["CodDetalleSalaProgresivo"]),
                                CodSalaProgresivo = ManejoNulos.ManageNullInteger(dr["CodSalaProgresivo"]),
                                NroPozo = ManejoNulos.ManageNullInteger(dr["NroPozo"]),
                                NombrePozo = ManejoNulos.ManageNullStr(dr["NombrePozo"]),
                                Dificultad= ManejoNulos.ManageNullInteger(dr["Dificultad"]),
                                MontoBase= ManejoNulos.ManageNullDecimal(dr["MontoBase"]),
                                MontoIni= ManejoNulos.ManageNullDecimal(dr["MontoIni"]),
                                MontoFin= ManejoNulos.ManageNullDecimal(dr["MontoFin"]),
                                Modalidad= ManejoNulos.ManageNullInteger(dr["Modalidad"]),
                                MontoOcultoBase= ManejoNulos.ManageNullDecimal(dr["MontoOcultoBase"]),
                                MontoOcultoIni= ManejoNulos.ManageNullDecimal(dr["MontoOcultoIni"]),
                                MontoOcultoFin= ManejoNulos.ManageNullDecimal(dr["MontoOcultoFin"]),
                                Incremento= ManejoNulos.ManageNullDecimal(dr["Incremento"]),
                                IncrementoPozoOculto= ManejoNulos.ManageNullDecimal(dr["IncrementoPozoOculto"]),
                                FechaRegistro= ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion= ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo= ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado= ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CodProgresivoExterno= ManejoNulos.ManageNullInteger(dr["CodProgresivoExterno"]),
                                CodUsuario= ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                fechaIni= ManejoNulos.ManageNullDate(dr["fechaIni"]),
                                fechaFin= ManejoNulos.ManageNullDate(dr["fechaFin"]),
                   
                            };

                            lista.Add(progresivoSala);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return new List<ADM_DetalleSalaProgresivoEntidad>();
            }
            return lista;
        }
        public bool EditarADM_DetalleSalaProgresivo(ADM_DetalleSalaProgresivoEntidad progresivo)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[ADM_DetalleSalaProgresivo]
   SET [CodSalaProgresivo] = @CodSalaProgresivo
      ,[NroPozo] = @NroPozo
      ,[NombrePozo] = @NombrePozo
      ,[Dificultad] = @Dificultad
      ,[MontoBase] = @MontoBase
      ,[MontoIni] = @MontoIni
      ,[MontoFin] = @MontoFin
      ,[Modalidad] = @Modalidad
      ,[MontoOcultoBase] = @MontoOcultoBase
      ,[MontoOcultoIni] = @MontoOcultoIni
      ,[MontoOcultoFin] = @MontoOcultoFin
      ,[Incremento] = @Incremento
      ,[IncrementoPozoOculto] = @IncrementoPozoOculto
      ,[FechaRegistro] = @FechaRegistro
      ,[FechaModificacion] = @FechaModificacion
      ,[Activo] = @Activo
      ,[Estado] = @Estado
      ,[CodProgresivoExterno] = @CodProgresivoExterno
      ,[CodUsuario] = @CodUsuario
      ,[fechaIni] = @fechaIni
      ,[fechaFin] = @fechaFin
 WHERE CodDetalleSalaProgresivo=@CodDetalleSalaProgresivo";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@CodSalaProgresivo", ManejoNulos.ManageNullInteger(progresivo.CodSalaProgresivo));
                    query.Parameters.AddWithValue("@NroPozo", ManejoNulos.ManageNullInteger(progresivo.NroPozo));
                    query.Parameters.AddWithValue("@NombrePozo", ManejoNulos.ManageNullStr(progresivo.NombrePozo));
                    query.Parameters.AddWithValue("@Dificultad", ManejoNulos.ManageNullInteger(progresivo.Dificultad));
                    query.Parameters.AddWithValue("@MontoBase", ManejoNulos.ManageNullDecimal(progresivo.MontoBase));
                    query.Parameters.AddWithValue("@MontoIni", ManejoNulos.ManageNullDecimal(progresivo.MontoIni));
                    query.Parameters.AddWithValue("@MontoFin", ManejoNulos.ManageNullDecimal(progresivo.MontoFin));
                    query.Parameters.AddWithValue("@Modalidad", ManejoNulos.ManageNullInteger(progresivo.Modalidad));
                    query.Parameters.AddWithValue("@MontoOcultoBase", ManejoNulos.ManageNullDecimal(progresivo.MontoOcultoBase));
                    query.Parameters.AddWithValue("@MontoOcultoIni", ManejoNulos.ManageNullDecimal(progresivo.MontoOcultoIni));
                    query.Parameters.AddWithValue("@MontoOcultoFin", ManejoNulos.ManageNullDecimal(progresivo.MontoOcultoFin));
                    query.Parameters.AddWithValue("@Incremento", ManejoNulos.ManageNullDecimal(progresivo.Incremento));
                    query.Parameters.AddWithValue("@IncrementoPozoOculto", ManejoNulos.ManageNullDecimal(progresivo.IncrementoPozoOculto));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(progresivo.FechaRegistro));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(progresivo.FechaModificacion));
                    query.Parameters.AddWithValue("@Activo", ManejoNulos.ManegeNullBool(progresivo.Activo));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(progresivo.Estado));
                    query.Parameters.AddWithValue("@CodProgresivoExterno", ManejoNulos.ManageNullInteger(progresivo.CodProgresivoExterno));
                    query.Parameters.AddWithValue("@CodUsuario", ManejoNulos.ManageNullStr(progresivo.CodUsuario));
                    query.Parameters.AddWithValue("@fechaIni", ManejoNulos.ManageNullDate(progresivo.fechaIni));
                    query.Parameters.AddWithValue("@fechaFin", ManejoNulos.ManageNullDate(progresivo.fechaFin));
                    query.Parameters.AddWithValue("@CodDetalleSalaProgresivo", ManejoNulos.ManageNullInteger(progresivo.CodDetalleSalaProgresivo));
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
        public int GuardarADM_DetalleSalaProgresivo(ADM_DetalleSalaProgresivoEntidad progresivo)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[ADM_DetalleSalaProgresivo]
           ([CodSalaProgresivo]
           ,[NroPozo]
           ,[NombrePozo]
           ,[Dificultad]
           ,[MontoBase]
           ,[MontoIni]
           ,[MontoFin]
           ,[Modalidad]
           ,[MontoOcultoBase]
           ,[MontoOcultoIni]
           ,[MontoOcultoFin]
           ,[Incremento]
           ,[IncrementoPozoOculto]
           ,[FechaRegistro]
           ,[FechaModificacion]
           ,[Activo]
           ,[Estado]
           ,[CodProgresivoExterno]
           ,[CodUsuario]
           ,[fechaIni]
           ,[fechaFin])
Output Inserted.CodDetalleSalaProgresivo
     VALUES
           (@CodSalaProgresivo
           ,@NroPozo
           ,@NombrePozo
           ,@Dificultad
           ,@MontoBase
           ,@MontoIni
           ,@MontoFin
           ,@Modalidad
           ,@MontoOcultoBase
           ,@MontoOcultoIni
           ,@MontoOcultoFin
           ,@Incremento
           ,@IncrementoPozoOculto
           ,@FechaRegistro
           ,@FechaModificacion
           ,@Activo
           ,@Estado
           ,@CodProgresivoExterno
           ,@CodUsuario
           ,@fechaIni
           ,@fechaFin)
;";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSalaProgresivo", ManejoNulos.ManageNullInteger(progresivo.CodSalaProgresivo));
                    query.Parameters.AddWithValue("@NroPozo", ManejoNulos.ManageNullInteger(progresivo.NroPozo));
                    query.Parameters.AddWithValue("@NombrePozo", ManejoNulos.ManageNullStr(progresivo.NombrePozo));
                    query.Parameters.AddWithValue("@Dificultad", ManejoNulos.ManageNullInteger(progresivo.Dificultad));
                    query.Parameters.AddWithValue("@MontoBase", ManejoNulos.ManageNullDecimal(progresivo.MontoBase));
                    query.Parameters.AddWithValue("@MontoIni", ManejoNulos.ManageNullDecimal(progresivo.MontoIni));
                    query.Parameters.AddWithValue("@MontoFin", ManejoNulos.ManageNullDecimal(progresivo.MontoFin));
                    query.Parameters.AddWithValue("@Modalidad", ManejoNulos.ManageNullInteger(progresivo.Modalidad));
                    query.Parameters.AddWithValue("@MontoOcultoBase", ManejoNulos.ManageNullDecimal(progresivo.MontoOcultoBase));
                    query.Parameters.AddWithValue("@MontoOcultoIni", ManejoNulos.ManageNullDecimal(progresivo.MontoOcultoIni));
                    query.Parameters.AddWithValue("@MontoOcultoFin", ManejoNulos.ManageNullDecimal(progresivo.MontoOcultoFin));
                    query.Parameters.AddWithValue("@Incremento", ManejoNulos.ManageNullDecimal(progresivo.Incremento));
                    query.Parameters.AddWithValue("@IncrementoPozoOculto", ManejoNulos.ManageNullDecimal(progresivo.IncrementoPozoOculto));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(progresivo.FechaRegistro));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(progresivo.FechaModificacion));
                    query.Parameters.AddWithValue("@Activo", ManejoNulos.ManegeNullBool(progresivo.Activo));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(progresivo.Estado));
                    query.Parameters.AddWithValue("@CodProgresivoExterno", ManejoNulos.ManageNullInteger(progresivo.CodProgresivoExterno));
                    query.Parameters.AddWithValue("@CodUsuario", ManejoNulos.ManageNullStr(progresivo.CodUsuario));
                    query.Parameters.AddWithValue("@fechaIni", ManejoNulos.ManageNullDate(progresivo.fechaIni));
                    query.Parameters.AddWithValue("@fechaFin", ManejoNulos.ManageNullDate(progresivo.fechaFin));
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
    }
}
