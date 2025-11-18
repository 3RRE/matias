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
    public class CMP_CuponesGeneradosDAL
    {
        string _conexion = string.Empty;

        public CMP_CuponesGeneradosDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<CMP_CuponesGeneradosEntidad> GetListadoCupones()
        {
            List<CMP_CuponesGeneradosEntidad> lista = new List<CMP_CuponesGeneradosEntidad>();
            string consulta = @"SELECT [CgId]
                              ,[CampaniaId]
                              ,ClienteId
                              ,[CodSala]
                              ,[UsuarioId]
                              ,[SlotId]
                              ,[Juego]
                              ,[Marca]
                              ,[Modelo]
                              ,[Win]
                              ,[Parametro]
                              ,[ValorJuego]
                              ,[CantidadCupones]
                              ,[SaldoCupIni]
                              ,[SaldoCupFin]
                              ,[SerieIni]
                              ,[SerieFin]
                              ,[Fecha]
                              ,[Hora]
                              ,[Estado]
                          FROM [dbo].[CMP_CuponesGenerados] order by CgId desc";
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
                            var cupon = new CMP_CuponesGeneradosEntidad
                            {
                                CgId = ManejoNulos.ManageNullInteger64(dr["CgId"]),
                                CampaniaId = ManejoNulos.ManageNullInteger64(dr["CampaniaId"]),
                                ClienteId = ManejoNulos.ManageNullInteger64(dr["ClienteId"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                UsuarioId = ManejoNulos.ManageNullInteger(dr["UsuarioId"]),
                                SlotId = ManejoNulos.ManageNullStr(dr["SlotId"]),
                                Juego = ManejoNulos.ManageNullStr(dr["Juego"]),
                                Marca = ManejoNulos.ManageNullStr(dr["Marca"]),
                                Modelo = ManejoNulos.ManageNullStr(dr["Modelo"]),
                                Win = ManejoNulos.ManageNullInteger(dr["Win"]),
                                Parametro = ManejoNulos.ManageNullInteger(dr["Parametro"]),
                                ValorJuego = ManejoNulos.ManageNullInteger(dr["ValorJuego"]),
                                CantidadCupones = ManejoNulos.ManageNullDouble(dr["CantidadCupones"]),
                                SaldoCupIni = ManejoNulos.ManageNullDouble(dr["SaldoCupIni"]),
                                SaldoCupFin = ManejoNulos.ManageNullDouble(dr["SaldoCupFin"]),
                                SerieIni = ManejoNulos.ManageNullStr(dr["SerieIni"]),
                                SerieFin = ManejoNulos.ManageNullStr(dr["SerieFin"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Hora = ManejoNulos.ManageNullTimespan(dr["Hora"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                            };

                            lista.Add(cupon);
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

        public CMP_CuponesGeneradosEntidad GetCuponGeneradoId(Int64 CgId)
        {
            CMP_CuponesGeneradosEntidad cupon = new CMP_CuponesGeneradosEntidad();
            string consulta = @"SELECT [CgId]
                              ,[CampaniaId]
                                ,ClienteId
                              ,[CodSala]
                              ,[UsuarioId]
                              ,[SlotId]
                              ,[Juego]
                              ,[Marca]
                              ,[Modelo]
                              ,[Win]
                              ,[Parametro]
                              ,[ValorJuego]
                              ,[CantidadCupones]
                              ,[SaldoCupIni]
                              ,[SaldoCupFin]
                              ,[SerieIni]
                              ,[SerieFin]
                              ,[Fecha]
                              ,[Hora]
                              ,[Estado]
                          FROM [dbo].[CMP_CuponesGenerados] where CgId=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", CgId);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                cupon.CgId= ManejoNulos.ManageNullInteger64(dr["CgId"]);
                                cupon.CampaniaId = ManejoNulos.ManageNullInteger64(dr["CampaniaId"]);
                                cupon.ClienteId = ManejoNulos.ManageNullInteger64(dr["ClienteId"]);
                                cupon.CodSala= ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                cupon.UsuarioId= ManejoNulos.ManageNullInteger(dr["UsuarioId"]);
                                cupon.SlotId= ManejoNulos.ManageNullStr(dr["SlotId"]);
                                cupon.Juego= ManejoNulos.ManageNullStr(dr["Juego"]);
                                cupon.Marca= ManejoNulos.ManageNullStr(dr["Marca"]);
                                cupon.Modelo= ManejoNulos.ManageNullStr(dr["Modelo"]);
                                cupon.Win= ManejoNulos.ManageNullInteger(dr["Win"]);
                                cupon.Parametro= ManejoNulos.ManageNullInteger(dr["Parametro"]);
                                cupon.ValorJuego= ManejoNulos.ManageNullInteger(dr["ValorJuego"]);
                                cupon.CantidadCupones= ManejoNulos.ManageNullInteger(dr["CantidadCupones"]);
                                cupon.SaldoCupIni= ManejoNulos.ManageNullDouble(dr["SaldoCupIni"]);
                                cupon.SaldoCupFin= ManejoNulos.ManageNullDouble(dr["SaldoCupFin"]);
                                cupon.SerieIni= ManejoNulos.ManageNullStr(dr["SerieIni"]);
                                cupon.SerieFin= ManejoNulos.ManageNullStr(dr["SerieFin"]);
                                cupon.Fecha= ManejoNulos.ManageNullDate(dr["Fecha"]);
                                cupon.Hora= ManejoNulos.ManageNullTimespan(dr["Hora"]);
                                cupon.Estado= ManejoNulos.ManageNullInteger(dr["Estado"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cupon;
        }
        public int GuardarCuponGenerado(CMP_CuponesGeneradosEntidad cupon)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"
INSERT INTO [dbo].[CMP_CuponesGenerados]
           ([CampaniaId]
            ,ClienteId
           ,[CodSala]
           ,[UsuarioId]
           ,[SlotId]
           ,[Juego]
           ,[Marca]
           ,[Modelo]
           ,[Win]
           ,[Parametro]
           ,[ValorJuego]
           ,[CantidadCupones]
           ,[SaldoCupIni]
           ,[SaldoCupFin]
           ,[SerieIni]
           ,[SerieFin]
           ,[Fecha]
           ,[Hora]
           ,[Estado])
Output Inserted.CgId
     VALUES
           (@CampaniaId
            ,@ClienteId
           ,@CodSala
           ,@UsuarioId
           ,@SlotId
           ,@Juego
           ,@Marca
           ,@Modelo
           ,@Win
           ,@Parametro
           ,@ValorJuego
           ,@CantidadCupones
           ,@SaldoCupIni
           ,@SaldoCupFin
           ,@SerieIni
           ,@SerieFin
           ,@Fecha
           ,@Hora
           ,@Estado)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CampaniaId", ManejoNulos.ManageNullInteger64(cupon.CampaniaId));
                    query.Parameters.AddWithValue("@ClienteId", ManejoNulos.ManageNullInteger64(cupon.ClienteId));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(cupon.CodSala));
                    query.Parameters.AddWithValue("@UsuarioId", ManejoNulos.ManageNullInteger(cupon.UsuarioId));
                    query.Parameters.AddWithValue("@SlotId", ManejoNulos.ManageNullStr(cupon.SlotId.Trim()));
                    query.Parameters.AddWithValue("@Juego", ManejoNulos.ManageNullStr(cupon.Juego));
                    query.Parameters.AddWithValue("@Marca", ManejoNulos.ManageNullStr(cupon.Marca));
                    query.Parameters.AddWithValue("@Modelo", ManejoNulos.ManageNullStr(cupon.Modelo));
                    query.Parameters.AddWithValue("@Win", ManejoNulos.ManageNullInteger(cupon.Win));
                    query.Parameters.AddWithValue("@Parametro", ManejoNulos.ManageNullInteger(cupon.Parametro));
                    query.Parameters.AddWithValue("@ValorJuego", ManejoNulos.ManageNullInteger(cupon.ValorJuego));
                    query.Parameters.AddWithValue("@CantidadCupones", ManejoNulos.ManageNullDouble(cupon.CantidadCupones));
                    query.Parameters.AddWithValue("@SaldoCupIni", ManejoNulos.ManageNullDouble(cupon.SaldoCupIni));
                    query.Parameters.AddWithValue("@SaldoCupFin", ManejoNulos.ManageNullDouble(cupon.SaldoCupFin));
                    query.Parameters.AddWithValue("@SerieIni", ManejoNulos.ManageNullStr(cupon.SerieIni));
                    query.Parameters.AddWithValue("@SerieFin", ManejoNulos.ManageNullStr(cupon.SerieFin));
                    query.Parameters.AddWithValue("@Fecha", ManejoNulos.ManageNullDate(cupon.Fecha));
                    query.Parameters.AddWithValue("@Hora", ManejoNulos.ManageNullTimespan(cupon.Hora));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(cupon.Estado));
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
        public bool EditarCuponGenerado(CMP_CuponesGeneradosEntidad cupon)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CMP_CuponesGenerados]
   SET [CampaniaId] = @CampaniaId
        ,ClienteId=@ClienteId
      ,[CodSala] = @CodSala
      ,[UsuarioId] = @UsuarioId
      ,[SlotId] = @SlotId
      ,[Juego] = @Juego
      ,[Marca] = @Marca
      ,[Modelo] = @Modelo
      ,[Win] = @Win
      ,[Parametro] = @Parametro
      ,[ValorJuego] = @ValorJuego
      ,[CantidadCupones] = @CantidadCupones
      ,[SaldoCupIni] = @SaldoCupIni
      ,[SaldoCupFin] = @SaldoCupFin
      ,[SerieIni] = @SerieIni
      ,[SerieFin] = @SerieFin
 WHERE CgId=@CgId";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CampaniaId", ManejoNulos.ManageNullInteger64(cupon.CampaniaId));
                    query.Parameters.AddWithValue("@ClienteId", ManejoNulos.ManageNullInteger64(cupon.ClienteId));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(cupon.CodSala));
                    query.Parameters.AddWithValue("@UsuarioId", ManejoNulos.ManageNullInteger(cupon.UsuarioId));
                    query.Parameters.AddWithValue("@SlotId", ManejoNulos.ManageNullStr(cupon.SlotId));
                    query.Parameters.AddWithValue("@Juego", ManejoNulos.ManageNullStr(cupon.Juego));
                    query.Parameters.AddWithValue("@Marca", ManejoNulos.ManageNullStr(cupon.Marca));
                    query.Parameters.AddWithValue("@Modelo", ManejoNulos.ManageNullStr(cupon.Modelo));
                    query.Parameters.AddWithValue("@Win", ManejoNulos.ManageNullInteger(cupon.Win));
                    query.Parameters.AddWithValue("@Parametro", ManejoNulos.ManageNullInteger(cupon.Parametro));
                    query.Parameters.AddWithValue("@ValorJuego", ManejoNulos.ManageNullInteger(cupon.ValorJuego));
                    query.Parameters.AddWithValue("@CantidadCupones", ManejoNulos.ManageNullDouble(cupon.CantidadCupones));
                    query.Parameters.AddWithValue("@SaldoCupIni", ManejoNulos.ManageNullDouble(cupon.SaldoCupIni));
                    query.Parameters.AddWithValue("@SaldoCupFin", ManejoNulos.ManageNullDouble(cupon.SaldoCupFin));
                    query.Parameters.AddWithValue("@SerieIni", ManejoNulos.ManageNullStr(cupon.SerieIni));
                    query.Parameters.AddWithValue("@SerieFin", ManejoNulos.ManageNullStr(cupon.SerieFin));
                    query.Parameters.AddWithValue("@CgId", ManejoNulos.ManageNullInteger64(cupon.CgId));
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
        public bool EditarEstadoCuponGenerado(CMP_CuponesGeneradosEntidad cupon)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CMP_CuponesGenerados]
                           SET [Estado] = @Estado
                         WHERE CgId=@CgId";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(cupon.Estado));
                    query.Parameters.AddWithValue("@CgId", ManejoNulos.ManageNullInteger64(cupon.CgId));
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

        public bool EditarCuponGeneradoSeries(CMP_CuponesGeneradosEntidad cupon)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CMP_CuponesGenerados]
   SET 
      [SerieIni] = @SerieIni
      ,[SerieFin] = @SerieFin
 WHERE CgId=@CgId";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);                 
                    query.Parameters.AddWithValue("@SerieIni", ManejoNulos.ManageNullStr(cupon.SerieIni));
                    query.Parameters.AddWithValue("@SerieFin", ManejoNulos.ManageNullStr(cupon.SerieFin));
                    query.Parameters.AddWithValue("@CgId", ManejoNulos.ManageNullInteger64(cupon.CgId));
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
        public List<CMP_CuponesGeneradosEntidad> GetListadoCuponesxCampania(Int64 CampaniaId, bool detalle=false)
        {
            List<CMP_CuponesGeneradosEntidad> lista = new List<CMP_CuponesGeneradosEntidad>();
            string consulta = @"SELECT cg.[CgId]
        ,cg.[CampaniaId]
        ,cg.ClienteId
		,cl.ApelPat
		,cl.ApelMat
		,cl.Nombre
		,cl.NombreCompleto
        ,cl.Mail
		,cl.FechaNacimiento
		,cl.NroDoc
        ,cg.[CodSala]
		,s.Nombre nombreSala
        ,cg.[UsuarioId]
        ,su.UsuarioNombre
        ,cg.[SlotId]
        ,cg.[Juego]
        ,cg.[Marca]
        ,cg.[Modelo]
        ,cg.[Win]
        ,cg.[Parametro]
        ,cg.[ValorJuego]
        ,cg.[CantidadCupones]
        ,cg.[SaldoCupIni]
        ,cg.[SaldoCupFin]
        ,cg.[SerieIni]
        ,cg.[SerieFin]
        ,cg.[Fecha]
        ,cg.[Hora]
        ,cg.[Estado]
    FROM [dbo].[CMP_CuponesGenerados] cg
	left join Sala s on s.CodSala=cg.CodSala
	left join AST_Cliente cl on cl.Id=cg.ClienteId
	left join SEG_Usuario su on su.UsuarioID=cg.UsuarioId
	where cg.CampaniaId=@p0
	order by cg.CgId desc ";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", CampaniaId);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var cupon = new CMP_CuponesGeneradosEntidad
                            {
                                CgId = ManejoNulos.ManageNullInteger64(dr["CgId"]),
                                CampaniaId = ManejoNulos.ManageNullInteger64(dr["CampaniaId"]),
                                ClienteId = ManejoNulos.ManageNullInteger64(dr["ClienteId"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                nombreSala = ManejoNulos.ManageNullStr(dr["nombreSala"]),
                                UsuarioId = ManejoNulos.ManageNullInteger(dr["UsuarioId"]),
                                UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                SlotId = ManejoNulos.ManageNullStr(dr["SlotId"]),
                                Juego = ManejoNulos.ManageNullStr(dr["Juego"]),
                                Marca = ManejoNulos.ManageNullStr(dr["Marca"]),
                                Modelo = ManejoNulos.ManageNullStr(dr["Modelo"]),
                                Win = ManejoNulos.ManageNullInteger(dr["Win"]),
                                Parametro = ManejoNulos.ManageNullInteger(dr["Parametro"]),
                                ValorJuego = ManejoNulos.ManageNullInteger(dr["ValorJuego"]),
                                CantidadCupones = ManejoNulos.ManageNullDouble(dr["CantidadCupones"]),
                                SaldoCupIni = ManejoNulos.ManageNullDouble(dr["SaldoCupIni"]),
                                SaldoCupFin = ManejoNulos.ManageNullDouble(dr["SaldoCupFin"]),
                                SerieIni = ManejoNulos.ManageNullStr(dr["SerieIni"]),
                                SerieFin = ManejoNulos.ManageNullStr(dr["SerieFin"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Hora = ManejoNulos.ManageNullTimespan(dr["Hora"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                            };

                            lista.Add(cupon);
                        }
                    }
                    if (detalle && lista.Count > 0)
                    {
                        foreach (var tipo in lista)
                        {
                            SetDetail(tipo, con);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lista.Clear();
            }
            return lista;
        }

        public List<CMP_CuponesGeneradosEntidad> GetListadoCuponesxCampaniaFecha(Int64 CampaniaId, DateTime fechaInicio, DateTime fechaFin, bool detalle = false)
        {
            List<CMP_CuponesGeneradosEntidad> lista = new List<CMP_CuponesGeneradosEntidad>();
            string consulta = @"SELECT cg.[CgId]
        ,cg.[CampaniaId]
        ,cg.ClienteId
		,cl.ApelPat
		,cl.ApelMat
		,cl.Nombre
		,cl.NombreCompleto
        ,cl.Mail
		,cl.FechaNacimiento
		,cl.NroDoc
        ,cg.[CodSala]
		,s.Nombre nombreSala
        ,cg.[UsuarioId]
        ,su.UsuarioNombre
        ,cg.[SlotId]
        ,cg.[Juego]
        ,cg.[Marca]
        ,cg.[Modelo]
        ,cg.[Win]
        ,cg.[Parametro]
        ,cg.[ValorJuego]
        ,cg.[CantidadCupones]
        ,cg.[SaldoCupIni]
        ,cg.[SaldoCupFin]
        ,cg.[SerieIni]
        ,cg.[SerieFin]
        ,cg.[Fecha]
        ,cg.[Hora]
        ,cg.[Estado]
    FROM [dbo].[CMP_CuponesGenerados] cg
	left join Sala s on s.CodSala=cg.CodSala
	left join AST_Cliente cl on cl.Id=cg.ClienteId
	left join SEG_Usuario su on su.UsuarioID=cg.UsuarioId
	where cg.CampaniaId=@p0 AND cg.Fecha BETWEEN @fechaInicio AND @fechaFin
	order by cg.CgId desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", CampaniaId);
                    query.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    query.Parameters.AddWithValue("@fechaFin", fechaFin);

                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var cupon = new CMP_CuponesGeneradosEntidad
                            {
                                CgId = ManejoNulos.ManageNullInteger64(dr["CgId"]),
                                CampaniaId = ManejoNulos.ManageNullInteger64(dr["CampaniaId"]),
                                ClienteId = ManejoNulos.ManageNullInteger64(dr["ClienteId"]),
                                ApelPat = ManejoNulos.ManageNullStr(dr["ApelPat"]),
                                ApelMat = ManejoNulos.ManageNullStr(dr["ApelMat"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                Mail = ManejoNulos.ManageNullStr(dr["Mail"]),
                                FechaNacimiento = ManejoNulos.ManageNullDate(dr["FechaNacimiento"]),
                                NroDoc = ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                nombreSala = ManejoNulos.ManageNullStr(dr["nombreSala"]),
                                UsuarioId = ManejoNulos.ManageNullInteger(dr["UsuarioId"]),
                                UsuarioNombre = ManejoNulos.ManageNullStr(dr["UsuarioNombre"]),
                                SlotId = ManejoNulos.ManageNullStr(dr["SlotId"]),
                                Juego = ManejoNulos.ManageNullStr(dr["Juego"]),
                                Marca = ManejoNulos.ManageNullStr(dr["Marca"]),
                                Modelo = ManejoNulos.ManageNullStr(dr["Modelo"]),
                                Win = ManejoNulos.ManageNullInteger(dr["Win"]),
                                Parametro = ManejoNulos.ManageNullInteger(dr["Parametro"]),
                                ValorJuego = ManejoNulos.ManageNullInteger(dr["ValorJuego"]),
                                CantidadCupones = ManejoNulos.ManageNullDouble(dr["CantidadCupones"]),
                                SaldoCupIni = ManejoNulos.ManageNullDouble(dr["SaldoCupIni"]),
                                SaldoCupFin = ManejoNulos.ManageNullDouble(dr["SaldoCupFin"]),
                                SerieIni = ManejoNulos.ManageNullStr(dr["SerieIni"]),
                                SerieFin = ManejoNulos.ManageNullStr(dr["SerieFin"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Hora = ManejoNulos.ManageNullTimespan(dr["Hora"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                            };

                            lista.Add(cupon);
                        }
                    }
                    if (detalle && lista.Count > 0)
                    {
                        foreach (var tipo in lista)
                        {
                            SetDetail(tipo, con);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lista.Clear();
            }
            return lista;
        }

        public bool EditarCantidadCuponGenerados(CMP_CuponesGeneradosEntidad cupon)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[CMP_CuponesGenerados]
                               SET 
                                  [CantidadCupones] = @CantidadCupones
                             WHERE CgId=@CgId";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CgId", ManejoNulos.ManageNullInteger64(cupon.CgId));
                    query.Parameters.AddWithValue("@CantidadCupones", ManejoNulos.ManageNullDouble(cupon.CantidadCupones));
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
        public List<CMP_CuponesGeneradosEntidad> GetListadoCuponesxCliente(string whereQuery, DateTime fechaIni, DateTime fechaFin)
        {
            List<CMP_CuponesGeneradosEntidad> lista = new List<CMP_CuponesGeneradosEntidad>();
            string consulta = @"SELECT cli.[CgId]
                              ,cli.[CampaniaId]
                              ,cli.ClienteId
                              ,cli.[CodSala]
                              ,cli.[UsuarioId]
                              ,cli.[SlotId]
                              ,cli.[Juego]
                              ,cli.[Marca]
                              ,cli.[Modelo]
                              ,cli.[Win]
                              ,cli.[Parametro]
                              ,cli.[ValorJuego]
                              ,cli.[CantidadCupones]
                              ,cli.[SaldoCupIni]
                              ,cli.[SaldoCupFin]
                              ,cli.[SerieIni]
                              ,cli.[SerieFin]
                              ,cli.[Fecha]
                              ,cli.[Hora]
                              ,cli.[Estado],camp.nombre, cl.NombreCompleto, cl.NroDoc
                            ,s.Nombre as nombreSala
                          FROM [dbo].[CMP_CuponesGenerados] as cli
						  join dbo.CMP_Campaña as camp on cli.CampaniaId=camp.id  
                          join Sala s on s.CodSala = camp.sala_id
join dbo.AST_Cliente as cl on cl.Id=cli.ClienteId where " + whereQuery+ " and CONVERT(date, cli.fecha) between @p0 and @p1 order by cli.Fecha desc";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fechaIni);
                    query.Parameters.AddWithValue("@p1", fechaFin);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var cupon = new CMP_CuponesGeneradosEntidad
                            {
                                CgId = ManejoNulos.ManageNullInteger64(dr["CgId"]),
                                CampaniaId = ManejoNulos.ManageNullInteger64(dr["CampaniaId"]),
                                ClienteId = ManejoNulos.ManageNullInteger64(dr["ClienteId"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                UsuarioId = ManejoNulos.ManageNullInteger(dr["UsuarioId"]),
                                SlotId = ManejoNulos.ManageNullStr(dr["SlotId"]),
                                Juego = ManejoNulos.ManageNullStr(dr["Juego"]),
                                Marca = ManejoNulos.ManageNullStr(dr["Marca"]),
                                Modelo = ManejoNulos.ManageNullStr(dr["Modelo"]),
                                Win = ManejoNulos.ManageNullInteger(dr["Win"]),
                                Parametro = ManejoNulos.ManageNullInteger(dr["Parametro"]),
                                ValorJuego = ManejoNulos.ManageNullInteger(dr["ValorJuego"]),
                                CantidadCupones = ManejoNulos.ManageNullDouble(dr["CantidadCupones"]),
                                SaldoCupIni = ManejoNulos.ManageNullDouble(dr["SaldoCupIni"]),
                                SaldoCupFin = ManejoNulos.ManageNullDouble(dr["SaldoCupFin"]),
                                SerieIni = ManejoNulos.ManageNullStr(dr["SerieIni"]),
                                SerieFin = ManejoNulos.ManageNullStr(dr["SerieFin"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                Hora = ManejoNulos.ManageNullTimespan(dr["Hora"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                CampaniaNombre=ManejoNulos.ManageNullStr(dr["nombre"]),
                                NombreCompleto=ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                NroDoc=ManejoNulos.ManageNullStr(dr["NroDoc"]),
                                TipoCampania="Sorteo",
                                nombreSala= ManejoNulos.ManageNullStr(dr["nombreSala"]),
                            };

                            lista.Add(cupon);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                lista.Clear();
            }
            return lista;
        }
        public void SetDetail(CMP_CuponesGeneradosEntidad cupon, SqlConnection connection)
        {
            List<CMP_DetalleCuponesImpresosEntidad> lista = new List<CMP_DetalleCuponesImpresosEntidad>();
            string consulta = @"SELECT cim.[DetImpId]
                                ,cim.[CgId]
                                ,cim.[CodSala]
                                ,cim.[SerieIni]
                                ,cim.[SerieFin]
                                ,cim.[CantidadCuponesImpresos]
                                ,cim.[UltimoCuponImpreso]
	                            ,onc.CodMaq
	                            ,onc.CoinOutAnterior
	                            ,onc.CoinOut
	                            ,onc.CurrentCredits
	                            ,onc.Monto
	                            ,onc.Token
	                            ,onc.FechaRegistro
	                            ,onc.id	
                                ,onc.CoinOutIas
                            FROM [dbo].[CMP_DetalleCuponesImpresos] cim
                            left join CMP_Contadores_OnLine_Web_Cupones onc on onc.DetalleCuponesImpresos_id= cim.DetImpId
                            where CgId=@p0 order by [DetImpId] asc";

            var query = new SqlCommand(consulta, connection);
            query.Parameters.AddWithValue("@p0", cupon.CgId);
            using (var reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    cupon.DetalleCuponesImpresos.Add(
                        new CMP_DetalleCuponesImpresosEntidad
                        {
                            DetImpId = ManejoNulos.ManageNullInteger64(reader["DetImpId"]),
                            CgId = ManejoNulos.ManageNullInteger64(reader["CgId"]),
                            CodSala = ManejoNulos.ManageNullInteger(reader["CodSala"]),
                            SerieIni = ManejoNulos.ManageNullStr(reader["SerieIni"]),
                            SerieFin = ManejoNulos.ManageNullStr(reader["SerieFin"]),
                            CantidadCuponesImpresos = ManejoNulos.ManageNullInteger(reader["CantidadCuponesImpresos"]),
                            UltimoCuponImpreso = ManejoNulos.ManageNullStr(reader["UltimoCuponImpreso"]),
                            CoinOutIas = ManejoNulos.ManageNullDouble(reader["CoinOutIas"]),
                            CodMaq = ManejoNulos.ManageNullStr(reader["CodMaq"]),
                            CoinOutAnterior = ManejoNulos.ManageNullDouble(reader["CoinOutAnterior"]),
                            CoinOut = ManejoNulos.ManageNullDouble(reader["CoinOut"]),
                            CurrentCredits = ManejoNulos.ManageNullDouble(reader["CurrentCredits"]),
                            Monto = ManejoNulos.ManageNullDecimal(reader["Monto"]),
                            Token = ManejoNulos.ManageNullDecimal(reader["Token"]),
                            FechaRegistro = ManejoNulos.ManageNullDate(reader["FechaRegistro"]),
                            id = ManejoNulos.ManageNullInteger64(reader["id"]),
                        }
                    );
                }
            }
        }
    }
}
