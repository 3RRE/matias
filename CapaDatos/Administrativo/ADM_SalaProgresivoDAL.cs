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
    public class ADM_SalaProgresivoDAL
    {
        string _conexion = string.Empty;

        public ADM_SalaProgresivoDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ADM_SalaProgresivoEntidad> GetListadoADM_SalaProgresivoPorSala(int CodSala)
        {
            List<ADM_SalaProgresivoEntidad> lista = new List<ADM_SalaProgresivoEntidad>();
            string consulta = @"SELECT [CodSalaProgresivo]
                                      ,[CodSala]
                                      ,[CodProgresivo]
                                      ,[NroPozos]
                                      ,[NroJugadores]
                                      ,[SubidaCreditos]
                                      ,[FechaInstalacion]
                                      ,[FechaDesinstalacion]
                                      ,[ColorHexa]
                                      ,[Sigla]
                                      ,[FechaRegistro]
                                      ,[FechaModificacion]
                                      ,[Activo]
                                      ,[Estado]
                                      ,[Url]
                                      ,[CodUsuario]
                                      ,[CodProgresivoWO]
                                      ,[CodTipoConfiguracionProgresivo]
                                      ,[Nombre]
                                      ,[TipoProgresivo]
                                      ,[NombreSala]
                                      ,[RazonSocial]
                                      ,[ClaseProgresivo]
                                  FROM [dbo].[ADM_SalaProgresivo] where [CodSala]=@CodSala";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", CodSala);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var progresivoSala = new ADM_SalaProgresivoEntidad
                            {
                                CodSalaProgresivo= ManejoNulos.ManageNullInteger(dr["CodSalaProgresivo"]),
                                CodSala= ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodProgresivo= ManejoNulos.ManageNullInteger(dr["CodProgresivo"]),
                                NroPozos= ManejoNulos.ManageNullInteger(dr["NroPozos"]),
                                NroJugadores= ManejoNulos.ManageNullInteger(dr["NroJugadores"]),
                                SubidaCreditos= ManejoNulos.ManageNullInteger(dr["SubidaCreditos"]),
                                FechaInstalacion= ManejoNulos.ManageNullDate(dr["FechaInstalacion"]),
                                FechaDesinstalacion= ManejoNulos.ManageNullDate(dr["FechaDesinstalacion"]),
                                ColorHexa= ManejoNulos.ManageNullStr(dr["ColorHexa"]),
                                Sigla= ManejoNulos.ManageNullStr(dr["Sigla"]),
                                FechaRegistro= ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion= ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo= ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado= ManejoNulos.ManageNullInteger(dr["Estado"]),
                                Url= ManejoNulos.ManageNullStr(dr["Url"]),
                                CodUsuario= ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodProgresivoWO= ManejoNulos.ManageNullInteger(dr["CodProgresivoWO"]),
                                CodTipoConfiguracionProgresivo= ManejoNulos.ManageNullInteger(dr["CodTipoConfiguracionProgresivo"]),
                                Nombre= ManejoNulos.ManageNullStr(dr["Nombre"]),
                                TipoProgresivo= ManejoNulos.ManageNullStr(dr["TipoProgresivo"]),
                                NombreSala= ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                RazonSocial= ManejoNulos.ManageNullStr(dr["RazonSocial"]),
                                ClaseProgresivo = ManejoNulos.ManageNullStr(dr["ClaseProgresivo"])
                            };

                            lista.Add(progresivoSala);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return new List<ADM_SalaProgresivoEntidad>();
            }
            return lista;
        }
        public bool EditarADM_SalaProgresivo(ADM_SalaProgresivoEntidad progresivo)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[ADM_SalaProgresivo]
   SET 
       [CodSala] =@CodSala
      ,[CodProgresivo] =@CodProgresivo
      ,[NroPozos] =@NroPozos
      ,[NroJugadores] =@NroJugadores
      ,[SubidaCreditos] =@SubidaCreditos
      ,[FechaInstalacion] =@FechaInstalacion
      ,[FechaDesinstalacion] =@FechaDesinstalacion
      ,[ColorHexa] =@ColorHexa
      ,[Sigla] =@Sigla
      ,[FechaRegistro] =@FechaRegistro
      ,[FechaModificacion] =@FechaModificacion
      ,[Activo] =@Activo
      ,[Estado] =@Estado
      ,[Url] =@Url
      ,[CodUsuario] =@CodUsuario
      ,[CodProgresivoWO] =@CodProgresivoWO
      ,[CodTipoConfiguracionProgresivo] =@CodTipoConfiguracionProgresivo
      ,[Nombre] =@Nombre
      ,[TipoProgresivo] =@TipoProgresivo
      ,[NombreSala] =@NombreSala
      ,[RazonSocial] =@RazonSocial
      ,[ClaseProgresivo] =@ClaseProgresivo
 WHERE CodSalaProgresivo=@CodSalaProgresivo     ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(progresivo.CodSala));
                    query.Parameters.AddWithValue("@CodProgresivo", ManejoNulos.ManageNullInteger(progresivo.CodProgresivo));
                    query.Parameters.AddWithValue("@NroPozos", ManejoNulos.ManageNullInteger(progresivo.NroPozos));
                    query.Parameters.AddWithValue("@NroJugadores", ManejoNulos.ManageNullInteger(progresivo.NroJugadores));
                    query.Parameters.AddWithValue("@SubidaCreditos", ManejoNulos.ManageNullInteger(progresivo.SubidaCreditos));
                    query.Parameters.AddWithValue("@FechaInstalacion", ManejoNulos.ManageNullDate(progresivo.FechaInstalacion));
                    query.Parameters.AddWithValue("@FechaDesinstalacion", ManejoNulos.ManageNullDate(progresivo.FechaDesinstalacion));
                    query.Parameters.AddWithValue("@ColorHexa", ManejoNulos.ManageNullStr(progresivo.ColorHexa));
                    query.Parameters.AddWithValue("@Sigla", ManejoNulos.ManageNullStr(progresivo.Sigla));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(progresivo.FechaRegistro));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(progresivo.FechaModificacion));
                    query.Parameters.AddWithValue("@Activo", ManejoNulos.ManegeNullBool(progresivo.Activo));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(progresivo.Estado));
                    query.Parameters.AddWithValue("@Url", ManejoNulos.ManageNullStr(progresivo.Url));
                    query.Parameters.AddWithValue("@CodUsuario", ManejoNulos.ManageNullStr(progresivo.CodUsuario));
                    query.Parameters.AddWithValue("@CodProgresivoWO", ManejoNulos.ManageNullInteger(progresivo.CodProgresivoWO));
                    query.Parameters.AddWithValue("@CodTipoConfiguracionProgresivo", ManejoNulos.ManageNullInteger(progresivo.CodTipoConfiguracionProgresivo));
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(progresivo.Nombre));
                    query.Parameters.AddWithValue("@TipoProgresivo", ManejoNulos.ManageNullStr(progresivo.TipoProgresivo));
                    query.Parameters.AddWithValue("@CodSalaProgresivo", ManejoNulos.ManageNullInteger(progresivo.CodSalaProgresivo));
                    query.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(progresivo.NombreSala));
                    query.Parameters.AddWithValue("@RazonSocial", ManejoNulos.ManageNullStr(progresivo.RazonSocial));
                    query.Parameters.AddWithValue("@ClaseProgresivo", ManejoNulos.ManageNullStr(progresivo.ClaseProgresivo));
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
        public int GuardarADM_SalaProgresivo(ADM_SalaProgresivoEntidad progresivo)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[ADM_SalaProgresivo]
           ([CodSala]
           ,[CodProgresivo]
           ,[NroPozos]
           ,[NroJugadores]
           ,[SubidaCreditos]
           ,[FechaInstalacion]
           ,[FechaDesinstalacion]
           ,[ColorHexa]
           ,[Sigla]
           ,[FechaRegistro]
           ,[FechaModificacion]
           ,[Activo]
           ,[Estado]
           ,[Url]
           ,[CodUsuario]
           ,[CodProgresivoWO]
           ,[CodTipoConfiguracionProgresivo]
           ,[Nombre]
           ,[TipoProgresivo]
           ,[NombreSala]
           ,[RazonSocial]
           ,[ClaseProgresivo])
Output Inserted.CodSalaProgresivo
     VALUES
           (@CodSala
           ,@CodProgresivo
           ,@NroPozos
           ,@NroJugadores
           ,@SubidaCreditos
           ,@FechaInstalacion
           ,@FechaDesinstalacion
           ,@ColorHexa
           ,@Sigla
           ,@FechaRegistro
           ,@FechaModificacion
           ,@Activo
           ,@Estado
           ,@Url
           ,@CodUsuario
           ,@CodProgresivoWO
           ,@CodTipoConfiguracionProgresivo
           ,@Nombre
           ,@TipoProgresivo
           ,@NombreSala
           ,@RazonSocial
           ,@ClaseProgresivo)
    ;";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(progresivo.CodSala));
                    query.Parameters.AddWithValue("@CodProgresivo", ManejoNulos.ManageNullInteger(progresivo.CodProgresivo));
                    query.Parameters.AddWithValue("@NroPozos", ManejoNulos.ManageNullInteger(progresivo.NroPozos));
                    query.Parameters.AddWithValue("@NroJugadores", ManejoNulos.ManageNullInteger(progresivo.NroJugadores));
                    query.Parameters.AddWithValue("@SubidaCreditos", ManejoNulos.ManageNullInteger(progresivo.SubidaCreditos));
                    query.Parameters.AddWithValue("@FechaInstalacion", ManejoNulos.ManageNullDate(progresivo.FechaInstalacion));
                    query.Parameters.AddWithValue("@FechaDesinstalacion", ManejoNulos.ManageNullDate(progresivo.FechaDesinstalacion));
                    query.Parameters.AddWithValue("@ColorHexa", ManejoNulos.ManageNullStr(progresivo.ColorHexa));
                    query.Parameters.AddWithValue("@Sigla", ManejoNulos.ManageNullStr(progresivo.Sigla));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(progresivo.FechaRegistro));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(progresivo.FechaModificacion));
                    query.Parameters.AddWithValue("@Activo", ManejoNulos.ManegeNullBool(progresivo.Activo));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(progresivo.Estado));
                    query.Parameters.AddWithValue("@Url", ManejoNulos.ManageNullStr(progresivo.Url));
                    query.Parameters.AddWithValue("@CodUsuario", ManejoNulos.ManageNullStr(progresivo.CodUsuario));
                    query.Parameters.AddWithValue("@CodProgresivoWO", ManejoNulos.ManageNullInteger(progresivo.CodProgresivoWO));
                    query.Parameters.AddWithValue("@CodTipoConfiguracionProgresivo", ManejoNulos.ManageNullInteger(progresivo.CodTipoConfiguracionProgresivo));
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(progresivo.Nombre));
                    query.Parameters.AddWithValue("@TipoProgresivo", ManejoNulos.ManageNullStr(progresivo.TipoProgresivo));
                    query.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(progresivo.NombreSala));
                    query.Parameters.AddWithValue("@RazonSocial", ManejoNulos.ManageNullStr(progresivo.RazonSocial));
                    query.Parameters.AddWithValue("@ClaseProgresivo", ManejoNulos.ManageNullStr(progresivo.ClaseProgresivo));
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
        public List<ADM_SalaProgresivoEntidad> GetListadoADM_SalaProgresivoPorQuery(string whereQuery, IDictionary<string, DateTime> fechaParametros=null)
        {
            List<ADM_SalaProgresivoEntidad> lista = new List<ADM_SalaProgresivoEntidad>();
            string consulta = @"SELECT [CodSalaProgresivo]
                                      ,[CodSala]
                                      ,[CodProgresivo]
                                      ,[NroPozos]
                                      ,[NroJugadores]
                                      ,[SubidaCreditos]
                                      ,[FechaInstalacion]
                                      ,[FechaDesinstalacion]
                                      ,[ColorHexa]
                                      ,[Sigla]
                                      ,[FechaRegistro]
                                      ,[FechaModificacion]
                                      ,[Activo]
                                      ,[Estado]
                                      ,[Url]
                                      ,[CodUsuario]
                                      ,[CodProgresivoWO]
                                      ,[CodTipoConfiguracionProgresivo]
                                      ,[Nombre]
                                      ,[TipoProgresivo]
                                      ,[NombreSala]
                                      ,[RazonSocial]
                                  FROM [dbo].[ADM_SalaProgresivo]"+whereQuery;
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    foreach (var parametro in fechaParametros)
                    {
                        query.Parameters.AddWithValue(parametro.Key, parametro.Value);
                    }

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var progresivoSala = new ADM_SalaProgresivoEntidad
                            {
                                CodSalaProgresivo = ManejoNulos.ManageNullInteger(dr["CodSalaProgresivo"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                CodProgresivo = ManejoNulos.ManageNullInteger(dr["CodProgresivo"]),
                                NroPozos = ManejoNulos.ManageNullInteger(dr["NroPozos"]),
                                NroJugadores = ManejoNulos.ManageNullInteger(dr["NroJugadores"]),
                                SubidaCreditos = ManejoNulos.ManageNullInteger(dr["SubidaCreditos"]),
                                FechaInstalacion = ManejoNulos.ManageNullDate(dr["FechaInstalacion"]),
                                FechaDesinstalacion = ManejoNulos.ManageNullDate(dr["FechaDesinstalacion"]),
                                ColorHexa = ManejoNulos.ManageNullStr(dr["ColorHexa"]),
                                Sigla = ManejoNulos.ManageNullStr(dr["Sigla"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                Url = ManejoNulos.ManageNullStr(dr["Url"]),
                                CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                CodProgresivoWO = ManejoNulos.ManageNullInteger(dr["CodProgresivoWO"]),
                                CodTipoConfiguracionProgresivo = ManejoNulos.ManageNullInteger(dr["CodTipoConfiguracionProgresivo"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                TipoProgresivo = ManejoNulos.ManageNullStr(dr["TipoProgresivo"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                RazonSocial = ManejoNulos.ManageNullStr(dr["RazonSocial"]),
                            };

                            lista.Add(progresivoSala);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return new List<ADM_SalaProgresivoEntidad>();
            }
            return lista;
        }

        public List<ADM_SalaProgresivoEntidad> GetListadoPorSalaYClase(int codSala, string clase) {
            const string sql = @"
    SELECT  [CodSalaProgresivo],[CodSala],[CodProgresivo],[NroPozos],[NroJugadores],
            [SubidaCreditos],[FechaInstalacion],[FechaDesinstalacion],[ColorHexa],[Sigla],
            [FechaRegistro],[FechaModificacion],[Activo],[Estado],[Url],[CodUsuario],
            [CodProgresivoWO],[CodTipoConfiguracionProgresivo],[Nombre],[TipoProgresivo],
            [NombreSala],[RazonSocial],[ClaseProgresivo]
    FROM    [dbo].[ADM_SalaProgresivo]
    WHERE   [CodSala] = @CodSala AND [ClaseProgresivo] = @Clase;";

            var lista = new List<ADM_SalaProgresivoEntidad>();
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(sql, con)) {
                        cmd.Parameters.AddWithValue("@CodSala", codSala);
                        cmd.Parameters.AddWithValue("@Clase", clase);
                        using(var dr = cmd.ExecuteReader()) {
                            while(dr.Read()) {
                                var e = new ADM_SalaProgresivoEntidad {
                                    CodSalaProgresivo = ManejoNulos.ManageNullInteger(dr["CodSalaProgresivo"]),
                                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                    CodProgresivo = ManejoNulos.ManageNullInteger(dr["CodProgresivo"]),
                                    NroPozos = ManejoNulos.ManageNullInteger(dr["NroPozos"]),
                                    NroJugadores = ManejoNulos.ManageNullInteger(dr["NroJugadores"]),
                                    SubidaCreditos = ManejoNulos.ManageNullInteger(dr["SubidaCreditos"]),
                                    FechaInstalacion = ManejoNulos.ManageNullDate(dr["FechaInstalacion"]),
                                    FechaDesinstalacion = ManejoNulos.ManageNullDate(dr["FechaDesinstalacion"]),
                                    ColorHexa = ManejoNulos.ManageNullStr(dr["ColorHexa"]),
                                    Sigla = ManejoNulos.ManageNullStr(dr["Sigla"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                    FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                    Activo = ManejoNulos.ManegeNullBool(dr["Activo"]),
                                    Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                    Url = ManejoNulos.ManageNullStr(dr["Url"]),
                                    CodUsuario = ManejoNulos.ManageNullStr(dr["CodUsuario"]),
                                    CodProgresivoWO = ManejoNulos.ManageNullInteger(dr["CodProgresivoWO"]),
                                    CodTipoConfiguracionProgresivo = ManejoNulos.ManageNullInteger(dr["CodTipoConfiguracionProgresivo"]),
                                    Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                    TipoProgresivo = ManejoNulos.ManageNullStr(dr["TipoProgresivo"]),
                                    NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                    RazonSocial = ManejoNulos.ManageNullStr(dr["RazonSocial"]),
                                    ClaseProgresivo = ManejoNulos.ManageNullStr(dr["ClaseProgresivo"]),
                                };
                                lista.Add(e);
                            }
                        }
                    }
                }
            } catch { return new List<ADM_SalaProgresivoEntidad>(); }
            return lista;
        }

        // Conveniencia:
        public List<ADM_SalaProgresivoEntidad> GetListadoTercerosPorSala(int codSala)
            => GetListadoPorSalaYClase(codSala, "Terceros");

    }
}
