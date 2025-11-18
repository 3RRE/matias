using CapaEntidad;
using CapaEntidad.ControlAcceso;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class AumentoCreditoMaquinaDAL
    {
        string conexion = string.Empty;
        public AumentoCreditoMaquinaDAL()
        {
            conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public bool InsertarAumentoCreditoMaquina(AumentoCreditoMaquinaEntidad item)
        {
            //bool respuesta = false;
            bool respuesta = false;
            string consulta = @"
                            INSERT INTO [dbo].[AumentoCreditoMaquina]
                               ([CodMaq]
                               ,[FechaRegistro]
                               ,[Cantidad]
                               ,[UsuarioRegistro]
                               ,[CodSala]
                               ,[PuertoSignalr]
                               ,[Hora]
                               ,[Minuto],[FechaUltimoAumento])
                         VALUES
                               (@CodMaq
                               ,@FechaRegistro
                               ,@Cantidad
                               ,@UsuarioRegistro
                               ,@CodSala
                               ,@PuertoSignalr
                               ,@Hora
                               ,@Minuto,@FechaUltimoAumento)
                            ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodMaq", ManejoNulos.ManageNullStr(item.CodMaq));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(item.FechaRegistro));
                    query.Parameters.AddWithValue("@Cantidad", ManejoNulos.ManageNullInteger(item.Cantidad));
                    query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullInteger(item.UsuarioRegistro));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(item.CodSala));
                    query.Parameters.AddWithValue("@PuertoSignalr", ManejoNulos.ManageNullInteger(item.PuertoSignalr));
                    query.Parameters.AddWithValue("@Hora", ManejoNulos.ManageNullInteger(item.Hora));
                    query.Parameters.AddWithValue("@Minuto", ManejoNulos.ManageNullInteger(item.Minuto));
                    query.Parameters.AddWithValue("@FechaUltimoAumento", ManejoNulos.ManageNullDate(item.FechaUltimoAumento));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return respuesta;
        }
        public AumentoCreditoMaquinaEntidad ObtenerUltimoRegistro(string CodMaq,int CodSala)
        {
            AumentoCreditoMaquinaEntidad item = new AumentoCreditoMaquinaEntidad();
            //string consulta = @"select top 1 [Id]
            //                      ,[CodMaq]
            //                      ,[FechaRegistro]
            //                      ,[FechaUltimoAumento]
            //                      ,[Cantidad]
            //                      ,[UsuarioRegistro]
            //                      ,[CodSala]
            //                      ,[PuertoSignalr]
            //                      ,[Hora]
            //                      ,[Minuto] from AumentoCreditoMaquina
            //                where CodMaq=@CodMaq and CodSala=@CodSala
            //                and CONVERT(date,fecharegistro)=CONVERT(date,getdate())
            //                order by Id desc ";   
            string consulta = @"
declare @maxId int = (select isnull(max(Id),0) from AumentoCreditoMaquina where CodMaq=@CodMaq and CodSala=@CodSala and CONVERT(date,Fecharegistro)=CONVERT(date,getdate()))
select [Id]
                                  ,[CodMaq]
                                  ,[FechaRegistro]
                                  ,[FechaUltimoAumento]
                                  ,[Cantidad]
                                  ,[UsuarioRegistro]
                                  ,[CodSala]
                                  ,[PuertoSignalr]
                                  ,[Hora]
                                  ,[Minuto] from AumentoCreditoMaquina
								  where Id=@maxId ";
            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodMaq", CodMaq);
                    query.Parameters.AddWithValue("@CodSala", CodSala);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item.Id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                item.CodMaq = ManejoNulos.ManageNullStr(dr["CodMaq"]);
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                item.FechaUltimoAumento = ManejoNulos.ManageNullDate(dr["FechaUltimoAumento"]);
                                item.Cantidad = ManejoNulos.ManageNullInteger(dr["Cantidad"]);
                                item.UsuarioRegistro = ManejoNulos.ManageNullInteger(dr["UsuarioRegistro"]);
                                item.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                item.PuertoSignalr = ManejoNulos.ManageNullInteger(dr["PuertoSignalr"]);
                                item.Hora = ManejoNulos.ManageNullInteger(dr["Hora"]);
                                item.Minuto = ManejoNulos.ManageNullInteger(dr["Minuto"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                item = new AumentoCreditoMaquinaEntidad();
            }
            return item;

        }
        public bool ActualizarCantidadEnAumentoCreditoMaquina(AumentoCreditoMaquinaEntidad item)
        {
            bool respuesta = false;
            //string consulta = @"UPDATE [dbo].[AumentoCreditoMaquina]
            //                   SET 
            //                      [FechaUltimoAumento] = @FechaUltimoAumento
            //                      ,[Cantidad] = Cantidad + @Cantidad
            //                 WHERE Id=@Id";    
            string consulta = @"declare @id int = @IdAumento
                                declare @limite int =5
                                declare @cantidadActual int =(select isnull(cantidad,1) from dbo.AumentoCreditoMaquina where id=@id)
                                declare @cantidadInsertaroActualizar int =1

                                if(@cantidadActual=@limite)
                                begin
	                                set @cantidadInsertaroActualizar=1
                                end
                                else
                                begin
	                                set @cantidadInsertaroActualizar=@cantidadActual+1
                                end

                                update dbo.AumentoCreditoMaquina set FechaUltimoAumento=GETDATE(), Cantidad=@cantidadInsertaroActualizar where Id=@id";

            try
            {
                using (var con = new SqlConnection(conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    //query.Parameters.AddWithValue("@FechaUltimoAumento", ManejoNulos.ManageNullDate(item.FechaUltimoAumento));
                    //query.Parameters.AddWithValue("@Cantidad", ManejoNulos.ManageNullInteger(item.Cantidad));
                    query.Parameters.AddWithValue("@IdAumento", ManejoNulos.ManageNullInteger(item.Id));
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
