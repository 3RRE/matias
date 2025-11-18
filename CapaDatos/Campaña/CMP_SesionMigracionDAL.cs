using CapaEntidad.Campañas;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Campaña
{
    public class CMP_SesionMigracionDAL
    {
        private readonly string _conexion = string.Empty;
        public CMP_SesionMigracionDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public int GuardarSesion(CMP_SesionMigracion sesion)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
                IF not EXISTS (SELECT * FROM [dbo].[CMP_SesionMigracion] WHERE Sesionid =@SesionId and CodSala=@CodSala)
                begin
	                INSERT INTO [dbo].[CMP_SesionMigracion]
                           ([SesionId],[CodMaquina]
                           ,[FechaInicio]
                           ,[ClienteIdIas]
                           ,[NombreCliente]
                           ,[NroDocumento]
                           ,[UsuarioIas]
                           ,[EstadoEnvio],[CantidadJugadas],[CantidadCupones],[SerieIni],[SerieFin],[Mail],[TipoDocumentoId],[NombreTipoDocumento],[TipoSesion],[NombreTipoSesion],[CodSala],[NombreSala],[FechaTermino],[Terminado],UsuarioTermino,[MotivoTermino],[FechaRegistro])
                output inserted.Id
                     VALUES
                           (@SesionId,@CodMaquina
                           ,@FechaInicio
                           ,@ClienteIdIas
                           ,@NombreCliente
                           ,@NroDocumento
                           ,@UsuarioIas
                           ,@EstadoEnvio,@CantidadJugadas,@CantidadCupones,@SerieIni,@SerieFin,@Mail,@TipoDocumentoId,@NombreTipoDocumento,@TipoSesion,@NombreTipoSesion,@CodSala,@NombreSala,@FechaTermino,@Terminado,@UsuarioTermino,@MotivoTermino,getdate())
                end
                else
                begin
	                select 0
                end
                      ";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@SesionId", ManejoNulos.ManageNullInteger64(sesion.SesionId));
                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullStr(sesion.CodMaquina));
                    query.Parameters.AddWithValue("@FechaInicio", ManejoNulos.ManageNullDate(sesion.FechaInicio));
                    query.Parameters.AddWithValue("@ClienteIdIas", ManejoNulos.ManageNullInteger(sesion.ClienteIdIas));
                    query.Parameters.AddWithValue("@NombreCliente", ManejoNulos.ManageNullStr(sesion.NombreCliente).ToUpper());
                    query.Parameters.AddWithValue("@NroDocumento", ManejoNulos.ManageNullStr(sesion.NroDocumento));
                    query.Parameters.AddWithValue("@UsuarioIas", ManejoNulos.ManageNullInteger(sesion.UsuarioIas));
                    query.Parameters.AddWithValue("@EstadoEnvio", ManejoNulos.ManageNullInteger(sesion.EstadoEnvio));
                    query.Parameters.AddWithValue("@CantidadCupones", ManejoNulos.ManageNullInteger(sesion.CantidadCupones));
                    query.Parameters.AddWithValue("@CantidadJugadas", ManejoNulos.ManageNullInteger(sesion.CantidadJugadas));
                    query.Parameters.AddWithValue("@SerieIni", ManejoNulos.ManageNullStr(sesion.SerieIni));
                    query.Parameters.AddWithValue("@SerieFin", ManejoNulos.ManageNullStr(sesion.SerieFin));
                    query.Parameters.AddWithValue("@Mail", ManejoNulos.ManageNullStr(sesion.Mail).ToUpper());
                    query.Parameters.AddWithValue("@TipoDocumentoId", ManejoNulos.ManageNullInteger(sesion.TipoDocumentoId));
                    query.Parameters.AddWithValue("@NombreTipoDocumento", ManejoNulos.ManageNullStr(sesion.NombreTipoDocumento).ToUpper());
                    query.Parameters.AddWithValue("@TipoSesion", ManejoNulos.ManageNullInteger(sesion.TipoSesion));
                    query.Parameters.AddWithValue("@NombreTipoSesion", ManejoNulos.ManageNullStr(sesion.NombreTipoSesion));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(sesion.CodSala));
                    query.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(sesion.NombreSala));
                    query.Parameters.AddWithValue("@FechaTermino", ManejoNulos.ManageNullDate(sesion.FechaTermino));
                    query.Parameters.AddWithValue("@Terminado", ManejoNulos.ManageNullInteger(sesion.Terminado));
                    query.Parameters.AddWithValue("@UsuarioTermino", ManejoNulos.ManageNullInteger(sesion.UsuarioTermino));
                    query.Parameters.AddWithValue("@MotivoTermino", ManejoNulos.ManageNullStr(sesion.MotivoTermino));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public List<CMP_SesionMigracion> ListarSesionPorQuery(string Query)
        {
            List<CMP_SesionMigracion> lista = new List<CMP_SesionMigracion>();
            string consulta = $@"
set dateformat dmy
SELECT [Id]
      ,[SesionId]
      ,[CodMaquina]
      ,[FechaInicio]
      ,[FechaTermino]
      ,[ClienteIdIas]
      ,[NombreCliente]
      ,[NroDocumento]
      ,[UsuarioIas]
      ,[Terminado]
      ,[MotivoTermino]
      ,[UsuarioTermino]
      ,[EstadoEnvio]
      ,[CantidadJugadas]
      ,[CantidadCupones]
      ,[SerieIni]
      ,[SerieFin]
      ,[Mail]
      ,[TipoDocumentoId]
      ,[NombreTipoDocumento]
      ,[TipoSesion]
      ,[NombreTipoSesion]
      ,[CodSala]
      ,[NombreSala]
      ,[FechaRegistro]
  FROM [dbo].[CMP_SesionMigracion] (nolock) {Query}";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.CommandTimeout = 300;//5 minutos

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var registro = new CMP_SesionMigracion
                                {
                                    Id = ManejoNulos.ManageNullInteger64(dr["Id"]),
                                    SesionId = ManejoNulos.ManageNullInteger64(dr["SesionId"]),
                                    CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                                    FechaInicio = ManejoNulos.ManageNullDate(dr["FechaInicio"]),
                                    FechaTermino = ManejoNulos.ManageNullDate(dr["FechaTermino"]),
                                    ClienteIdIas = ManejoNulos.ManageNullInteger(dr["ClienteIdIas"]),
                                    NombreCliente = ManejoNulos.ManageNullStr(dr["NombreCliente"]),
                                    NroDocumento = ManejoNulos.ManageNullStr(dr["NroDocumento"]),
                                    UsuarioIas = ManejoNulos.ManageNullInteger(dr["UsuarioIas"]),
                                    Terminado = ManejoNulos.ManageNullInteger(dr["Terminado"]),
                                    MotivoTermino = ManejoNulos.ManageNullStr(dr["MotivoTermino"]),
                                    UsuarioTermino = ManejoNulos.ManageNullInteger(dr["UsuarioTermino"]),
                                    EstadoEnvio = ManejoNulos.ManageNullInteger(dr["EstadoEnvio"]),
                                    CantidadCupones = ManejoNulos.ManageNullInteger(dr["CantidadCupones"]),
                                    CantidadJugadas = ManejoNulos.ManageNullInteger(dr["CantidadJugadas"]),
                                    SerieIni = ManejoNulos.ManageNullStr(dr["SerieIni"]),
                                    SerieFin = ManejoNulos.ManageNullStr(dr["SerieFin"]),
                                    Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                    TipoDocumentoId = ManejoNulos.ManageNullInteger(dr["TipoDocumentoId"]),
                                    NombreTipoDocumento = ManejoNulos.ManageNullStr(dr["NombreTipoDocumento"]),
                                    TipoSesion = ManejoNulos.ManageNullInteger(dr["TipoSesion"]),
                                    NombreTipoSesion = ManejoNulos.ManageNullStr(dr["NombreTipoSesion"]),
                                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                    NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                };
                                lista.Add(registro);
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                lista = new List<CMP_SesionMigracion>();
            }
            return lista;
        }
    }
}
