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
    public class ADM_GanadorDAL
    {
        string _conexion = string.Empty;

        public ADM_GanadorDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public int GuardarADM_Ganador(ADM_GanadorEntidad ganador)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[ADM_Ganador]
           ([CodDetalleSalaProgresivo]
           ,[CodMaquina]
           ,[MontoProgresivo]
           ,[SubioPremio]
           ,[FechaPremio]
           ,[FechaSubida]
           ,[Token]
           ,[AntCoinIn]
           ,[AntCoinOut]
           ,[AntJackpot]
           ,[AntCancelCredits]
           ,[AntBill]
           ,[AntGamesPlayed]
           ,[AntBonus]
           ,[AntEftIn]
           ,[ActCoinIn]
           ,[ActCoinOut]
           ,[ActJackpot]
           ,[ActCancelCredits]
           ,[ActBill]
           ,[ActGamesPlayed]
           ,[ActBonus]
           ,[ActEftIn]
           ,[FechaRegistro]
           ,[FechaModificacion]
           ,[Activo]
           ,[Estado]
           ,[CodUsuario]
           ,[FechaOperacion])
Output Inserted.CodGanador
     VALUES
           (@CodDetalleSalaProgresivo
           ,@CodMaquina
           ,@MontoProgresivo
           ,@SubioPremio
           ,@FechaPremio
           ,@FechaSubida
           ,@Token
           ,@AntCoinIn
           ,@AntCoinOut
           ,@AntJackpot
           ,@AntCancelCredits
           ,@AntBill
           ,@AntGamesPlayed
           ,@AntBonus
           ,@AntEftIn
           ,@ActCoinIn
           ,@ActCoinOut
           ,@ActJackpot
           ,@ActCancelCredits
           ,@ActBill
           ,@ActGamesPlayed
           ,@ActBonus
           ,@ActEftIn
           ,@FechaRegistro
           ,@FechaModificacion
           ,@Activo
           ,@Estado
           ,@CodUsuario
           ,@FechaOperacion)
;";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodDetalleSalaProgresivo", ManejoNulos.ManageNullInteger(ganador.CodDetalleSalaProgresivo));
                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullInteger(ganador.CodMaquina));
                    query.Parameters.AddWithValue("@MontoProgresivo", ManejoNulos.ManageNullDecimal(ganador.MontoProgresivo));
                    query.Parameters.AddWithValue("@SubioPremio", ManejoNulos.ManageNullInteger(ganador.SubioPremio));
                    query.Parameters.AddWithValue("@FechaPremio", ManejoNulos.ManageNullDate(ganador.FechaPremio));
                    query.Parameters.AddWithValue("@FechaSubida", ManejoNulos.ManageNullDate(ganador.FechaSubida));
                    query.Parameters.AddWithValue("@Token", ManejoNulos.ManageNullDecimal(ganador.Token));
                    query.Parameters.AddWithValue("@AntCoinIn", ManejoNulos.ManageNullInteger64(ganador.AntCoinIn));
                    query.Parameters.AddWithValue("@AntCoinOut", ManejoNulos.ManageNullInteger64(ganador.AntCoinOut));
                    query.Parameters.AddWithValue("@AntJackpot", ManejoNulos.ManageNullInteger64(ganador.AntJackpot));
                    query.Parameters.AddWithValue("@AntCancelCredits", ManejoNulos.ManageNullInteger64(ganador.AntCancelCredits));
                    query.Parameters.AddWithValue("@AntBill", ManejoNulos.ManageNullInteger64(ganador.AntBill));
                    query.Parameters.AddWithValue("@AntGamesPlayed", ManejoNulos.ManageNullInteger64(ganador.AntGamesPlayed));
                    query.Parameters.AddWithValue("@AntBonus", ManejoNulos.ManageNullInteger64(ganador.AntBonus));
                    query.Parameters.AddWithValue("@AntEftIn", ManejoNulos.ManageNullInteger64(ganador.AntEftIn));
                    query.Parameters.AddWithValue("@ActCoinIn", ManejoNulos.ManageNullInteger64(ganador.ActCoinIn));
                    query.Parameters.AddWithValue("@ActCoinOut", ManejoNulos.ManageNullInteger64(ganador.ActCoinOut));
                    query.Parameters.AddWithValue("@ActJackpot", ManejoNulos.ManageNullInteger64(ganador.ActJackpot));
                    query.Parameters.AddWithValue("@ActCancelCredits", ManejoNulos.ManageNullInteger64(ganador.ActCancelCredits));
                    query.Parameters.AddWithValue("@ActBill", ManejoNulos.ManageNullInteger64(ganador.ActBill));
                    query.Parameters.AddWithValue("@ActGamesPlayed", ManejoNulos.ManageNullInteger64(ganador.ActGamesPlayed));
                    query.Parameters.AddWithValue("@ActBonus", ManejoNulos.ManageNullInteger64(ganador.ActBonus));
                    query.Parameters.AddWithValue("@ActEftIn", ManejoNulos.ManageNullInteger64(ganador.ActEftIn));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(ganador.FechaRegistro));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(ganador.FechaModificacion));
                    query.Parameters.AddWithValue("@Activo", ManejoNulos.ManegeNullBool(ganador.Activo));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(ganador.Estado));
                    query.Parameters.AddWithValue("@CodUsuario", ManejoNulos.ManageNullStr(ganador.CodUsuario));
                    query.Parameters.AddWithValue("@FechaOperacion", ManejoNulos.ManageNullDate(ganador.FechaOperacion));
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
        public bool EliminarPremiosSalaFecha(int CodSalaProgresivo, DateTime FechaOperacion)
        {
            bool respuesta = false;
            string consulta = @"
                            delete ADM_Ganador
						where CodDetalleSalaProgresivo in
						(select CodDetalleSalaProgresivo from ADM_DetalleSalaProgresivo 
						where CodSalaProgresivo=@CodSalaProgresivo) and convert(date, FechaOperacion)=Convert(date,@FechaOperacion)     ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSalaProgresivo", CodSalaProgresivo);
                    query.Parameters.AddWithValue("@FechaOperacion", FechaOperacion);
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
    }
}
