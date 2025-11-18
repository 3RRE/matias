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
    public class CMP_MaquinaRestringidaDAL
    {
        string _conexion = string.Empty;

        public CMP_MaquinaRestringidaDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CMP_MaquinaRestringidaEntidad> GetListadoMaquinaRestringidaSala(int CodSala)
        {
            List<CMP_MaquinaRestringidaEntidad> lista = new List<CMP_MaquinaRestringidaEntidad>();
            string consulta = @"SELECT [CodMaquina]
                                      ,[CodSala]
                                      ,[Restringido]
                                      ,[UsuarioId],[Juego],[Marca],[Modelo]
                                  FROM [dbo].[CMP_MaquinaRestringida] where CodSala=@CodSala";
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
                            var impresora = new CMP_MaquinaRestringidaEntidad
                            {
                                CodMaquina= ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Restringido = ManejoNulos.ManageNullInteger(dr["Restringido"]),
                                UsuarioId = ManejoNulos.ManageNullInteger(dr["UsuarioId"]),
                                Juego = ManejoNulos.ManageNullStr(dr["Juego"]),
                                Marca = ManejoNulos.ManageNullStr(dr["Marca"]),
                                Modelo = ManejoNulos.ManageNullStr(dr["Modelo"]),
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
        public bool EditarEstadoRestriccionMaquina(CMP_MaquinaRestringidaEntidad maquinaRestringida)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CMP_MaquinaRestringida]
                               SET 
                                  [Restringido] = @Restringido
                                  ,[UsuarioId] = @UsuarioId
                             WHERE CodMaquina=@CodMaquina and CodSala=@CodSala";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Restringido", ManejoNulos.ManageNullInteger(maquinaRestringida.Restringido));
                    query.Parameters.AddWithValue("@UsuarioId", ManejoNulos.ManageNullInteger(maquinaRestringida.UsuarioId));
                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullStr(maquinaRestringida.CodMaquina));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(maquinaRestringida.CodSala));

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
        public bool GuardaMaquina(CMP_MaquinaRestringidaEntidad maquina)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO [dbo].[CMP_MaquinaRestringida]
           ([CodMaquina]
           ,[CodSala]
           ,[Restringido]
           ,[UsuarioId]
           ,[Juego]
           ,[Marca]
           ,[Modelo])
     VALUES
           (@CodMaquina
           ,@CodSala
           ,@Restringido
           ,@UsuarioId
           ,@Juego
           ,@Marca
           ,@Modelo);";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullStr(maquina.CodMaquina));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(maquina.CodSala));
                    query.Parameters.AddWithValue("@Restringido", ManejoNulos.ManageNullInteger(maquina.Restringido));
                    query.Parameters.AddWithValue("@UsuarioId", ManejoNulos.ManageNullInteger(maquina.UsuarioId));
                    query.Parameters.AddWithValue("@Juego", ManejoNulos.ManageNullStr(maquina.Juego));
                    query.Parameters.AddWithValue("@Marca", ManejoNulos.ManageNullStr(maquina.Marca));
                    query.Parameters.AddWithValue("@Modelo", ManejoNulos.ManageNullStr(maquina.Modelo));
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
        public bool GuardarMaquinaMasivo(string values)
        {
            bool respuesta = false;
            string consulta = @"INSERT INTO [dbo].[CMP_MaquinaRestringida]
           ([CodMaquina]
           ,[CodSala]
           ,[Restringido]
           ,[UsuarioId]
           ,[Juego]
           ,[Marca]
           ,[Modelo])
            select * from (values " + values + ") s(CodMaquina,CodSala,Restringido,UsuarioId,Juego,Marca,Modelo)";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
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
        public CMP_MaquinaRestringidaEntidad GetMaquinaRestringidaSalaPorSalaYMaquina(int CodSala,string CodMaquina)
        {
            CMP_MaquinaRestringidaEntidad maquina = new CMP_MaquinaRestringidaEntidad();
            string consulta = @"select [CodMaquina]
                                          ,[CodSala]
                                          ,[Restringido]
                                          ,[UsuarioId]
                                          ,[Juego]
                                          ,[Marca]
                                          ,[Modelo]
                                      FROM [CMP_MaquinaRestringida] where CodSala=@CodSala and CodMaquina=@CodMaquina";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", CodSala);
                    query.Parameters.AddWithValue("@CodMaquina", CodMaquina);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                maquina.CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]);
                                maquina.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                maquina.Restringido = ManejoNulos.ManageNullInteger(dr["Restringido"]);
                                maquina.UsuarioId = ManejoNulos.ManageNullInteger(dr["UsuarioId"]);
                                maquina.Juego = ManejoNulos.ManageNullStr(dr["Juego"]);
                                maquina.Marca = ManejoNulos.ManageNullStr(dr["Marca"]);
                                maquina.Modelo = ManejoNulos.ManageNullStr(dr["Modelo"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return maquina;
        }
    }
}
