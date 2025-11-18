using CapaEntidad;
using CapaEntidad.Alertas;
using CapaEntidad.Reportes;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class LogAlertaBilleterosDAL
    {
        string _conexion = string.Empty;
        public LogAlertaBilleterosDAL()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public int GuardarLogAlerta(LogAlertaBilleterosEntidad alerta)
        {
            //bool respuesta = false;
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [dbo].[LogAlertaBilleteros]
           ([Tipo]
           ,[CodSala]
           ,[Descripcion],[FechaRegistro],Cod_Even_OL,[Preview])
Output Inserted.Id
     VALUES
           (@p0
           ,@p1
           ,@p2,@p3,@p4,@p5)";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(alerta.Tipo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(alerta.CodSala));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(alerta.Descripcion));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(alerta.FechaRegistro));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger64(alerta.Cod_Even_OL));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(alerta.Preview));
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

        public List<LogAlertaBilleterosEntidad> GetTop10Alerta(string codsalas) {
            List<LogAlertaBilleterosEntidad> lista = new List<LogAlertaBilleterosEntidad>();
            string consulta = @"SELECT top(10) 
                                     lab.Id as Id
                                    ,lab.tipo as Tipo
                                    ,lab.CodSala as CodSala
                                    ,lab.Descripcion as Descripcion
                                    ,lab.FechaRegistro as FechaRegistro
                                    ,lab.Cod_Even_OL as Cod_Even_OL
                                    ,lab.Preview as Preview
                                    ,sa.NombreCorto as NombreSala
                                from LogAlertaBilleteros  as lab
                                join Sala as sa on sa.CodSala = lab.CodSala
                                where lab.CodSala in (" + codsalas+ ") order by lab.FechaRegistro desc"
                                  ;
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var evento = new LogAlertaBilleterosEntidad {
                                NombreSala= ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                Tipo = ManejoNulos.ManageNullInteger(dr["Tipo"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                Preview = ManejoNulos.ManageNullStr(dr["Preview"]),
                                Cod_Even_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Even_OL"]),
                            };
                            lista.Add(evento);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista.Clear();
            }
            return lista;
        }

        public List<LogAlertaBilleterosEntidad> GetLogsxCod_Even_OL(string inQuery)
        {
            List<LogAlertaBilleterosEntidad> lista = new List<LogAlertaBilleterosEntidad>();
            string consulta = @"SELECT [Id]
                                  ,[Tipo]
                                  ,[CodSala]
                                  ,[Descripcion]
                                  ,[FechaRegistro]
                                  ,[Cod_Even_OL]
                              FROM [LogAlertaBilleteros]
                                  where " + inQuery + "";
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
                            var evento = new LogAlertaBilleterosEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                Cod_Even_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Even_OL"]),
                            };
                            lista.Add(evento);
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

        //Revision de estado  sobre eventos, alertas y servicio
        public List<LogAlertaBilleterosEntidad> ConsultaRegistrosAlertaBilletero()
        {
            List<LogAlertaBilleterosEntidad> lista = new List<LogAlertaBilleterosEntidad>();

            string consulta = @"SELECT lab.*, s.nombre AS NombreSala, s.CodEmpresa
                                FROM LogAlertaBilleteros lab
                                JOIN (
                                    SELECT CodSala, Tipo, MAX(FechaRegistro) AS max_fecha
                                    FROM LogAlertaBilleteros
                                    GROUP BY CodSala, Tipo
                                ) t ON lab.CodSala = t.CodSala AND lab.Tipo = t.Tipo AND lab.FechaRegistro = t.max_fecha
                                JOIN Sala s ON lab.CodSala = s.CodSala where s.Estado=1  order by CodSala,Tipo";

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
                            var evento = new LogAlertaBilleterosEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                Cod_Even_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Even_OL"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                Tipo = ManejoNulos.ManageNullInteger(dr["Tipo"]),
                                Preview = ManejoNulos.ManageNullStr(dr["Preview"]),
                            };
                            lista.Add(evento);
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

        //Revision de alerta, evento y servicio por usuario
        public List<LogAlertaBilleterosEntidad> ConsultaRegistrosAlertaBilleteroxUsuario(string salas)
        {
            List<LogAlertaBilleterosEntidad> lista = new List<LogAlertaBilleterosEntidad>();

            string consulta = @"SELECT lab.*, s.nombre AS NombreSala, s.CodEmpresa
                                FROM LogAlertaBilleteros lab
                                JOIN (
                                    SELECT CodSala, Tipo, MAX(FechaRegistro) AS max_fecha
                                    FROM LogAlertaBilleteros
                                    GROUP BY CodSala, Tipo
                                ) t ON lab.CodSala = t.CodSala AND lab.Tipo = t.Tipo AND lab.FechaRegistro = t.max_fecha
                                JOIN Sala s ON lab.CodSala = s.CodSala where s.Estado=1 and lab.CodSala in ("+ salas +")  order by CodSala,Tipo";

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
                            var evento = new LogAlertaBilleterosEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                Cod_Even_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Even_OL"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                Tipo = ManejoNulos.ManageNullInteger(dr["Tipo"]),
                                Preview = ManejoNulos.ManageNullStr(dr["Preview"]),
                            };
                            lista.Add(evento);
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

        public List<int> ConsultaSalasActivas()
        {
            List<int> lista = new List<int>();

            string consulta = @"select CodSala from Sala where Estado=1";

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
                           
                            int codSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                            lista.Add(codSala);
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


        //consulta salas activas por usuario
        public List<int> ConsultaSalasActivasxUsuario(int usuarioId)
        {
            List<int> lista = new List<int>();

            string consulta = @"SELECT CodSala FROM Sala (nolock) 
                                  inner join UsuarioSala on UsuarioSala.SalaId= Sala.CodSala
                                  where Sala.estado = 1 and UsuarioSala.UsuarioId=@p0 order by nombre asc ";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(usuarioId));
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            int codSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                            lista.Add(codSala);
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


        public List<LogAlertaBilleterosEntidad> GetLogAlertaBilleterosxFiltros (DateTime fechaini, DateTime fechafin, string whereQuery="")
        {
            List<LogAlertaBilleterosEntidad> lista = new List<LogAlertaBilleterosEntidad>();

            string consulta = @" SELECT myLog.[Id]
                                  ,myLog.[Tipo]
                                  ,myLog.[CodSala]
                                  ,myLog.[Descripcion]
                                  ,myLog.[FechaRegistro]
                                  ,myLog.[Cod_Even_OL],myLog.[Preview],sal.Nombre as NombreSala
                              FROM [LogAlertaBilleteros] as myLog 
							  join Sala  as sal 
							  on myLog.CodSala=sal.CodSala where CONVERT(date, myLog.FechaRegistro) between @p0 and @p1 " + whereQuery +"";

            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fechaini);
                    query.Parameters.AddWithValue("@p1", fechafin);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var evento = new LogAlertaBilleterosEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                Cod_Even_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Even_OL"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                Tipo = ManejoNulos.ManageNullInteger(dr["Tipo"]),
                                Preview = ManejoNulos.ManageNullStr(dr["Preview"]),
                            };
                            lista.Add(evento);
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
        public LogAlertaBilleterosEntidad GetAlertaBilleteroxId(Int64 id)
        {
            LogAlertaBilleterosEntidad alerta = new LogAlertaBilleterosEntidad();
            string consulta = @"SELECT myLog.[Id]
                                  ,myLog.[Tipo]
                                  ,myLog.[CodSala]
                                  ,myLog.[Descripcion]
                                  ,myLog.[FechaRegistro]
                                  ,myLog.[Cod_Even_OL],myLog.[Preview],sal.Nombre as NombreSala
                              FROM [LogAlertaBilleteros] as myLog 
							  join Sala  as sal 
							  on myLog.CodSala=sal.CodSala where myLog.Id=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                alerta.Id = ManejoNulos.ManageNullInteger64(dr["Id"]);
                                alerta.Tipo = ManejoNulos.ManageNullInteger(dr["Tipo"]);
                                alerta.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                alerta.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                alerta.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                alerta.Cod_Even_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Even_OL"]);
                                alerta.NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]);
                                alerta.Preview = ManejoNulos.ManageNullStr(dr["Preview"]);
                            }
                        }
                    };


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                alerta = new LogAlertaBilleterosEntidad();
            }
            return alerta;
        }
        public LogAlertaBilleterosEntidad GetLogAlertaBilleteroPorCodEvenOL(Int64 Cod_Even_OL,int CodSala, int Tipo)
        {
            LogAlertaBilleterosEntidad alerta = new LogAlertaBilleterosEntidad();
            string consulta = @"SELECT [Id]
                                  ,[Tipo]
                                  ,[CodSala]
                                  ,[Descripcion]
                                  ,[FechaRegistro]
                                  ,[Cod_Even_OL]
                              FROM [LogAlertaBilleteros] where Cod_Even_OL=@p0 and CodSala=@p1 and Tipo=@p2";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", Cod_Even_OL);
                    query.Parameters.AddWithValue("@p1", CodSala);
                    query.Parameters.AddWithValue("@p2", Tipo);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                alerta.Id = ManejoNulos.ManageNullInteger64(dr["Id"]);
                                alerta.Tipo = ManejoNulos.ManageNullInteger(dr["Tipo"]);
                                alerta.CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]);
                                alerta.Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]);
                                alerta.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                                alerta.Cod_Even_OL = ManejoNulos.ManageNullInteger64(dr["Cod_Even_OL"]);
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                alerta = new LogAlertaBilleterosEntidad();
            }
            return alerta;
        }
        public bool EditarLogAlertaBilletero(LogAlertaBilleterosEntidad alertaBilletero)
        {
            bool respuesta = false;
            string consulta = @"UPDATE [dbo].[LogAlertaBilleteros]
                   SET [Descripcion] = @p0
                 WHERE Id=@p1";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", alertaBilletero.Descripcion);
                    query.Parameters.AddWithValue("@p1", alertaBilletero.Id);
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

        public List<LogAlertaBilleterosEntidad> ObtenerAlertasEventos() {
            List<LogAlertaBilleterosEntidad> list = new List<LogAlertaBilleterosEntidad>();

            string query = @"
                SELECT
                evento.Id,
	            evento.Tipo,
	            evento.CodSala,
	            evento.Descripcion,
	            evento.FechaRegistro,
	            evento.Cod_Even_OL,
	            evento.Preview,
	            sala.Nombre AS NombreSala
            FROM LogAlertaBilleteros AS evento
			JOIN Sala AS sala ON sala.CodSala = evento.CodSala
            WHERE CONVERT(DATE, EVENTO.FechaRegistro) = CONVERT(DATE, GETDATE()) and evento.Tipo = 2
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection) {
                        CommandTimeout = 0
                    };


                    using(SqlDataReader data = command.ExecuteReader()) {
                        while(data.Read()) {
                            LogAlertaBilleterosEntidad log = new LogAlertaBilleterosEntidad {
                                Id = ManejoNulos.ManageNullInteger(data["Id"]),
                                Tipo = ManejoNulos.ManageNullInteger(data["Tipo"]),
                                CodSala = ManejoNulos.ManageNullInteger(data["CodSala"]),
                                Descripcion = ManejoNulos.ManageNullStr(data["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]),
                                Cod_Even_OL = ManejoNulos.ManageNullInteger64(data["Cod_Even_OL"]),
                                Preview = ManejoNulos.ManageNullStr(data["Preview"]),
                                NombreSala = ManejoNulos.ManageNullStr(data["NombreSala"])
                            };

                            list.Add(log);
                        }
                    }
                }
            } catch(Exception) {
                list = new List<LogAlertaBilleterosEntidad>();
            }

            return list;
        }
        public List<LogAlertaBilleterosEntidad> ObtenerAlertasBilleteros() {
            List<LogAlertaBilleterosEntidad> list = new List<LogAlertaBilleterosEntidad>();

            string query = @"
            SELECT
                evento.Id,
	            evento.Tipo,
	            evento.CodSala,
	            evento.Descripcion,
	            evento.FechaRegistro,
	            evento.Cod_Even_OL,
	            evento.Preview,
	            sala.Nombre AS NombreSala
            FROM LogAlertaBilleteros AS evento
			JOIN Sala AS sala ON sala.CodSala = evento.CodSala
            WHERE CONVERT(DATE, EVENTO.FechaRegistro) = CONVERT(DATE, GETDATE()) and evento.Tipo = 3
            ";

            try {
                using(SqlConnection connection = new SqlConnection(_conexion)) {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection) {
                        CommandTimeout = 0
                    };


                    using(SqlDataReader data = command.ExecuteReader()) {
                        while(data.Read()) {
                            LogAlertaBilleterosEntidad log = new LogAlertaBilleterosEntidad {
                                Id = ManejoNulos.ManageNullInteger(data["Id"]),
                                Tipo = ManejoNulos.ManageNullInteger(data["Tipo"]),
                                CodSala = ManejoNulos.ManageNullInteger(data["CodSala"]),
                                Descripcion = ManejoNulos.ManageNullStr(data["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]),
                                Cod_Even_OL = ManejoNulos.ManageNullInteger64(data["Cod_Even_OL"]),
                                Preview = ManejoNulos.ManageNullStr(data["Preview"]),
                                NombreSala = ManejoNulos.ManageNullStr(data["NombreSala"])
                            };

                            list.Add(log);
                        }
                    }
                }
            } catch(Exception) {
                list = new List<LogAlertaBilleterosEntidad>();
            }

            return list;
        }


        #region Logs Reporte Nominal

        public List<ALEV_LogNominalEntidad> ObtenerReporteNominal(List<int> rooms, DateTime fromDate, DateTime toDate, int type)
        {
            List<ALEV_LogNominalEntidad> list = new List<ALEV_LogNominalEntidad>();

            string query = $@"
            DECLARE @TableRoom TABLE
            (
	            ID INT IDENTITY(1,1) NOT NULL,
	            RoomCode INT NOT NULL
            )

            DECLARE @TableNominal TABLE
            (
	            SalaId INT NOT NULL,
	            Fecha DATE NOT NULL,
	            Total INT NOT NULL
            )

            DECLARE @Counter INT
            DECLARE @MaxId INT
            DECLARE @RoomCode INT

            INSERT INTO @TableRoom
            SELECT
	            sala.CodSala AS RoomCode
            FROM Sala sala WITH (NOLOCK)
            WHERE sala.Estado = 1 AND sala.Activo = 1 AND sala.CodSala IN ({string.Join(",", rooms)})

            SELECT
	            @Counter = MIN(room.ID),
	            @MaxId = MAX(room.ID)
            FROM @TableRoom room

            WHILE(@Counter IS NOT NULL AND @Counter <= @MaxId)
            BEGIN
	            SELECT
		            @RoomCode = RoomCode
	            FROM @TableRoom WHERE ID = @Counter

	            ;WITH DateRange(DateData) AS 
	            (
		            SELECT @fromDate AS DATE
		            UNION ALL
		            SELECT DATEADD(DAY, 1, DateData)
		            FROM DateRange
		            WHERE DateData < @toDate
	            ),
	            LogNominal AS (
		            SELECT
			            CAST(lab.FechaRegistro AS DATE) AS Fecha,
			            COUNT(lab.Id) AS Total
		            FROM LogAlertaBilleteros lab WITH (NOLOCK)
		            WHERE lab.CodSala = @RoomCode AND lab.Tipo = @type AND CONVERT(DATE, lab.FechaRegistro) BETWEEN CONVERT(DATE, @fromDate) AND CONVERT(DATE, @toDate)
		            GROUP BY CAST(lab.FechaRegistro AS DATE)
		            HAVING COUNT(lab.Id) >= 1
	            )

	            INSERT INTO @TableNominal
	            SELECT
		            @RoomCode AS SalaId,
		            dr.DateData AS Fecha,
		            ISNULL(ln.Total, 0) AS Total
	            FROM DateRange dr WITH (NOLOCK)
	            LEFT JOIN LogNominal ln WITH (NOLOCK) ON ln.Fecha = dr.DateData
	            ORDER BY dr.DateData ASC
	            OPTION (MAXRECURSION 0)

               SET @Counter  = @Counter + 1
            END

            SELECT
	            tn.SalaId,
	            tn.Fecha,
	            tn.Total
            FROM @TableNominal tn
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection)
                    {
                        CommandTimeout = 0
                    };

                    command.Parameters.AddWithValue("@fromDate", ManejoNulos.ManageNullDate(fromDate));
                    command.Parameters.AddWithValue("@toDate", ManejoNulos.ManageNullDate(toDate));
                    command.Parameters.AddWithValue("@type", ManejoNulos.ManageNullInteger(type));

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            ALEV_LogNominalEntidad logNominal = new ALEV_LogNominalEntidad
                            {
                                SalaId = ManejoNulos.ManageNullInteger(data["SalaId"]),
                                Fecha = ManejoNulos.ManageNullDate(data["Fecha"]).ToString("dd/MM/yyyy"),
                                Total = ManejoNulos.ManageNullInteger(data["Total"])
                            };

                            list.Add(logNominal);
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = new List<ALEV_LogNominalEntidad>();
            }

            return list;
        }

        public List<ALEV_SalaNominalEntidad> ObtenerSalaNominal(List<int> rooms)
        {
            List<ALEV_SalaNominalEntidad> list = new List<ALEV_SalaNominalEntidad>();

            string query = $@"
            SELECT
	            sala.CodSala,
	            sala.Nombre
            FROM Sala sala WITH (NOLOCK)
            WHERE sala.Estado = 1 AND sala.Activo = 1
            AND sala.CodSala IN ({string.Join(",", rooms)})
            ORDER BY sala.CodSala ASC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection)
                    {
                        CommandTimeout = 0
                    };

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            ALEV_SalaNominalEntidad salaNominal = new ALEV_SalaNominalEntidad
                            {
                                Codigo = ManejoNulos.ManageNullInteger(data["CodSala"]),
                                Nombre = ManejoNulos.ManageNullStr(data["Nombre"])
                            };

                            list.Add(salaNominal);
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = new List<ALEV_SalaNominalEntidad>();
            }

            return list;
        }

        public List<string> ObtenerRangoFechasNominal(DateTime fromDate, DateTime toDate)
        {
            List<string> dates = new List<string>();

            string query = @"
            WITH DateRange(DateData) AS 
            (
	            SELECT @fromDate AS DATE
	            UNION ALL
	            SELECT DATEADD(DAY, 1, DateData)
	            FROM DateRange
	            WHERE DateData < @toDate
            )

            SELECT
	            dr.DateData AS Fecha
            FROM DateRange dr WITH (NOLOCK)
            ORDER BY dr.DateData ASC
            OPTION (MAXRECURSION 0)
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection)
                    {
                        CommandTimeout = 0
                    };

                    command.Parameters.AddWithValue("@fromDate", ManejoNulos.ManageNullDate(fromDate));
                    command.Parameters.AddWithValue("@toDate", ManejoNulos.ManageNullDate(toDate));

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            dates.Add(ManejoNulos.ManageNullDate(data["Fecha"]).ToString("dd/MM/yyyy"));
                        }
                    }
                }
            }
            catch (Exception)
            {
                dates = new List<string>();
            }

            return dates;
        }

        #endregion

        #region Logs Alerta Billeteros

        public List<LogAlertaBilleterosEntidad> ObtenerSalaLogsFecha(int room, int type, DateTime date)
        {
            List<LogAlertaBilleterosEntidad> list = new List<LogAlertaBilleterosEntidad>();

            string query = @"
            SELECT
	            myLog.Id,
	            myLog.Tipo,
	            myLog.CodSala,
	            myLog.Descripcion,
	            myLog.FechaRegistro,
	            myLog.Cod_Even_OL,
	            myLog.Preview,
	            sala.Nombre AS NombreSala
            FROM LogAlertaBilleteros AS myLog 
            JOIN Sala AS sala ON sala.CodSala = myLog.CodSala
            WHERE myLog.CodSala = @room AND myLog.Tipo = @type AND CONVERT(DATE, myLog.FechaRegistro) = CONVERT(DATE, @date)
            ORDER BY myLog.FechaRegistro DESC
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection)
                    {
                        CommandTimeout = 0
                    };

                    command.Parameters.AddWithValue("@room", ManejoNulos.ManageNullInteger(room));
                    command.Parameters.AddWithValue("@type", ManejoNulos.ManageNullInteger(type));
                    command.Parameters.AddWithValue("@date", ManejoNulos.ManageNullDate(date));

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            LogAlertaBilleterosEntidad log = new LogAlertaBilleterosEntidad
                            {
                                Id = ManejoNulos.ManageNullInteger(data["Id"]),
                                Tipo = ManejoNulos.ManageNullInteger(data["Tipo"]),
                                CodSala = ManejoNulos.ManageNullInteger(data["CodSala"]),
                                Descripcion = ManejoNulos.ManageNullStr(data["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]),
                                Cod_Even_OL = ManejoNulos.ManageNullInteger64(data["Cod_Even_OL"]),
                                Preview = ManejoNulos.ManageNullStr(data["Preview"]),
                                NombreSala = ManejoNulos.ManageNullStr(data["NombreSala"])
                            };

                            list.Add(log);
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = new List<LogAlertaBilleterosEntidad>();
            }

            return list;
        }



        public List<EVT_EventosOnlineEntidad> ObtenerEventosDelDiaPorCodSalaMovil(int codSala)
        {
            List<EVT_EventosOnlineEntidad> list = new List<EVT_EventosOnlineEntidad>();

            string query = @"
            select[codSala],[Descripcion],[FechaRegistro], [Cod_Even_OL] from LogAlertaBilleteros where tipo = 2
AND CAST(fechaRegistro AS DATE) = CAST(GETDATE() AS DATE) and codSala = " + codSala;

 //           select TOP(100) [codSala],[Descripcion],[FechaRegistro], [Cod_Even_OL] from LogAlertaBilleteros where tipo = 2
 //AND CodSala = " + codSala + "order by FechaRegistro Desc


            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection)
                    {
                        CommandTimeout = 0
                    };

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            EVT_EventosOnlineEntidad evento = new EVT_EventosOnlineEntidad
                            {
                                COD_SALA = ManejoNulos.ManageNullStr(data["codSala"]),
                                Evento = ManejoNulos.ManageNullStr(data["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]),
                                Cod_Even_OL = ManejoNulos.ManageNullInteger(data["Cod_Even_OL"]),
                              
                            };

                            list.Add(evento);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                list = new List<EVT_EventosOnlineEntidad>();
            }

            return list;
        }


        public List<EVT_EventosOnlineEntidad> ObtenerEventosPorRangoFechaCodSala(int codSala, DateTime fechaIni, DateTime fechaFin)
        {
            List<EVT_EventosOnlineEntidad> list = new List<EVT_EventosOnlineEntidad>();

            string query = @"
            SELECT [codSala], [Descripcion], [FechaRegistro], [Cod_Even_OL]
FROM LogAlertaBilleteros
WHERE tipo = 2
  AND CodSala = "+ codSala + " AND FechaRegistro >= @fromDate AND FechaRegistro < DATEADD(DAY, 1, @toDate) ORDER BY FechaRegistro DESC ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conexion))
                {
                    connection.Open();


                    SqlCommand command = new SqlCommand(query, connection)
                    {
                        CommandTimeout = 0
                    };
                    command.Parameters.AddWithValue("@fromDate", ManejoNulos.ManageNullDate(fechaIni));
                    command.Parameters.AddWithValue("@toDate", ManejoNulos.ManageNullDate(fechaFin));

                    using (SqlDataReader data = command.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            EVT_EventosOnlineEntidad evento = new EVT_EventosOnlineEntidad
                            {
                                COD_SALA = ManejoNulos.ManageNullStr(data["codSala"]),
                                Evento = ManejoNulos.ManageNullStr(data["Descripcion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(data["FechaRegistro"]),
                                Cod_Even_OL = ManejoNulos.ManageNullInteger(data["Cod_Even_OL"]),

                            };

                            list.Add(evento);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                list = new List<EVT_EventosOnlineEntidad>();
            }

            return list;
        }

        #endregion
    }
}
