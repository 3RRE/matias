using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using S3k.Utilitario;
using CapaEntidad.MaquinaEstado;
using S3k.Utilitario.clases_especial;
using CapaEntidad.Sala;
using System.Diagnostics;
using CapaEntidad.Reclamaciones;
using CapaEntidad;
using CapaEntidad.EntradaSalidaSala;
using System.Reflection;
using CapaDatos.ContadoresNegativos;
using System.ComponentModel.Design;
using Microsoft.Win32;
using System.Collections;
using System.Data;

namespace CapaDatos.MaquinaEstado 
    {
    public class TEC_EstadoMaquinaDAL 
    {
        string _conexion = "";
        public TEC_EstadoMaquinaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        //Paginacion
        public (Int64 maquinaestadototalsala, ClaseError error) maquinaestadoTotalSalaJson(Int64 codsala) {
            Int64 total = 0;
            ClaseError error = new ClaseError();
            string consulta = @"SELECT 
								COUNT(*) as total							
                                FROM [TEC_EstadoMaquina] 
                                where sala_id=@p0;";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", codsala);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                total = ManejoNulos.ManageNullInteger64(dr["total"]);
                            }
                        }
                    }

                }
            } catch(Exception ex) {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (maquinaestadototalsala: total, error);
        }
        public (List<TEC_EstadoMaquinaEntidad> maquinaestadoLista, ClaseError error) maquianestadoListarJson() {
            List<TEC_EstadoMaquinaEntidad> lista = new List<TEC_EstadoMaquinaEntidad>();
            ClaseError error = new ClaseError();
            string consulta = @"SELECT [Id]
                             ,[sala_id]
                             ,[CantMaquinaConectada]
                             ,[CantMaquinaNoConectada]
                              ,[TotalMaquina]
                              ,[FechaRegistro]
                          FROM [TEC_EstadoMaquina]";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                var fila = new TEC_EstadoMaquinaEntidad {
                                    id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                    sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                    CantMaquinaConectada = ManejoNulos.ManageNullInteger(dr["CantMaquinaConectada"]),
                                    CantMaquinaNoConectada = ManejoNulos.ManageNullInteger(dr["CantMaquinaNoConectada"]),
                                    TotalMaquina = ManejoNulos.ManageNullInteger(dr["TotalMaquina"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),  
                                };

                                lista.Add(fila);
                            }
                        }
                    }

                }
            } catch(Exception ex) {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (maquinaestadoLista: lista, error);
        }

        public (List<TEC_EstadoMaquinaEntidad> maquinaestadoLista, ClaseError error) maquinaestadoListarxSalaFechaJson(int[] salas, DateTime fechaini, DateTime fechafin) {
            List<TEC_EstadoMaquinaEntidad> lista = new List<TEC_EstadoMaquinaEntidad>();
            ClaseError error = new ClaseError();
            string strSala = string.Empty;
            strSala = $" codsala in ({String.Join(",", salas)}) and ";
            string consulta = @"SELECT 
                A.[Id], 
                s.Nombre AS sala,
                A.[CantMaquinaConectada],
                A.[CantMaquinaNoConectada],
                A.[CantMaquinaPLay],
                A.CantMaquinaRetiroTemporal,
                A.[TotalMaquina],
                A.[FechaRegistro],
                A.[sala_id],
                A.FechaCierre,
                A.FechaOperacion
            FROM 
                [TEC_EstadoMaquina] AS A
            JOIN 
                Sala AS s ON s.CodSala = A.[sala_id]
            WHERE 
                " + strSala + " CONVERT(date, A.FechaRegistro) between @p0 and @p1 order by Id desc;";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fechaini);
                    query.Parameters.AddWithValue("@p1", fechafin);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                var fila = new TEC_EstadoMaquinaEntidad {
                                    id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                    sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                    sala = ManejoNulos.ManageNullStr(dr["sala"]),
                                    CantMaquinaConectada = ManejoNulos.ManageNullInteger(dr["CantMaquinaConectada"]),
                                    CantMaquinaNoConectada = ManejoNulos.ManageNullInteger(dr["CantMaquinaNoConectada"]),
                                    CantMaquinaPLay = ManejoNulos.ManageNullInteger(dr["CantMaquinaPLay"]),
                                    CantMaquinaRetiroTemporal = ManejoNulos.ManageNullInteger(dr["CantMaquinaRetiroTemporal"]),
                                    FechaCierre = ManejoNulos.ManageNullDate(dr["FechaCierre"]),
                                    FechaOperacion= ManejoNulos.ManageNullDate(dr["FechaOperacion"]),

                                    TotalMaquina = ManejoNulos.ManageNullInteger(dr["TotalMaquina"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                };

                                lista.Add(fila);
                            }
                        }
                    }

                }
            } catch(Exception ex) {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (maquinaestadoLista: lista, error);
        }
        public List<(int CodSala, DateTime FechaOperacion)> ObtenerFechaOperacionUltimaxSala(int[] salas) {
            List<(int CodSala, DateTime FechaOperacion)> lista = new List<(int CodSala, DateTime FechaOperacion)>();
            ClaseError error = new ClaseError();
            string strSala = string.Empty;
            strSala = $" sala_id in ({String.Join(",", salas)})";
            string consulta = @"SELECT
                A.sala_id as CodSala,
                MAX(A.FechaOperacion) AS FechaOperacion
            FROM 
                [TEC_EstadoMaquina] AS A
            WHERE 
                " + strSala + " " +
               "GROUP BY A.[sala_id];";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con); 
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                var fila =  (
                                    CodSala: (int)ManejoNulos.ManageNullInteger64(dr["CodSala"]),
                                    FechaOperacion: ManejoNulos.ManageNullDate(dr["FechaOperacion"])
                                );

                                lista.Add(fila);
                            }
                        }
                    }

                }
            } catch(Exception ex) {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return lista;
        }
        public (List<TEC_EstadoMaquinaEntidad> maquinaestadoLista, ClaseError error) maquinaestadoListarxSalaUltimaFechaOperacionJson(int[] salas) {
            List<TEC_EstadoMaquinaEntidad> lista = new List<TEC_EstadoMaquinaEntidad>();
            ClaseError error = new ClaseError();
            string strSala = string.Empty;
            strSala = $" codsala in ({String.Join(",", salas)}) ";
     
            string consulta = @"
            SELECT TOP 1
                A.[Id], 
                s.Nombre AS sala,
                A.[CantMaquinaConectada],
                A.[CantMaquinaNoConectada],
                A.[CantMaquinaPLay],
                A.CantMaquinaRetiroTemporal,
                A.[TotalMaquina],
                A.[FechaRegistro],
                A.[sala_id],
                A.FechaCierre,
                A.FechaOperacion
            FROM 
                [TEC_EstadoMaquina] AS A
            JOIN 
                Sala AS s ON s.CodSala = A.[sala_id]
            WHERE 
                " + strSala +
                "ORDER BY A.FechaOperacion DESC;";


            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                var fila = new TEC_EstadoMaquinaEntidad {
                                    id = ManejoNulos.ManageNullInteger64(dr["id"]),
                                    sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                    sala = ManejoNulos.ManageNullStr(dr["sala"]),
                                    CantMaquinaConectada = ManejoNulos.ManageNullInteger(dr["CantMaquinaConectada"]),
                                    CantMaquinaNoConectada = ManejoNulos.ManageNullInteger(dr["CantMaquinaNoConectada"]),
                                    CantMaquinaPLay = ManejoNulos.ManageNullInteger(dr["CantMaquinaPLay"]),
                                    CantMaquinaRetiroTemporal = ManejoNulos.ManageNullInteger(dr["CantMaquinaRetiroTemporal"]),
                                    FechaCierre = ManejoNulos.ManageNullDate(dr["FechaCierre"]),
                                    FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"]),

                                    TotalMaquina = ManejoNulos.ManageNullInteger(dr["TotalMaquina"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                };

                                lista.Add(fila);
                            }
                        }
                    }

                }
            } catch(Exception ex) {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (maquinaestadoLista: lista, error);
        }
        public int GuardarMaquinaEstado(TEC_EstadoMaquinaEntidad maquinaestado) {
            int insertedId = 0;
            TEC_EstadoMaquinaEntidad test = maquinaestado;
            string query = @"
            INSERT INTO [TEC_EstadoMaquina]
            ( 
                sala_id,
                CantMaquinaConectada,
                CantMaquinaNoConectada,
                CantMaquinaPLay,
                CantMaquinaRetiroTemporal,
                TotalMaquina,
                FechaOperacion,
                FechaCierre,
                FechaRegistro
            )
            VALUES
            (
                @sala_id,
                @CantMaquinaConectada,
                @CantMaquinaNoConectada,
                @CantMaquinaPLay,
                @CantMaquinaRetiroTemporal,
                @TotalMaquina,
                @FechaOperacion,
                @FechaCierre,
                @FechaRegistro
               
            );
            SELECT SCOPE_IDENTITY()
            "; 
                try {
                    using(SqlConnection connection = new SqlConnection(_conexion)) {
                        connection.Open();

                        SqlCommand commmand = new SqlCommand(query, connection);

                        commmand.Parameters.AddWithValue("@sala_id", maquinaestado.sala_id);
                        commmand.Parameters.AddWithValue("@CantMaquinaConectada", maquinaestado.CantMaquinaConectada);
                        commmand.Parameters.AddWithValue("@CantMaquinaNoConectada", maquinaestado.CantMaquinaNoConectada);
                        commmand.Parameters.AddWithValue("@CantMaquinaPLay", maquinaestado.CantMaquinaPLay);
                        commmand.Parameters.AddWithValue("@CantMaquinaRetiroTemporal", 0);
                        commmand.Parameters.AddWithValue("@TotalMaquina", maquinaestado.TotalMaquina);
                        commmand.Parameters.AddWithValue("@FechaOperacion", maquinaestado.FechaOperacion);
                        commmand.Parameters.AddWithValue("@FechaCierre", maquinaestado.FechaCierre);
                        commmand.Parameters.AddWithValue("@FechaRegistro", maquinaestado.FechaRegistro);

                        insertedId = Convert.ToInt32(commmand.ExecuteScalar());
                        //insertedId = Convert.ToInt32(commmand.ExecuteScalar());
                    }
                } catch(Exception exception) {
                    Trace.WriteLine(exception.Message + " " + GetType().FullName + " " + DateTime.Now.ToLongDateString());
                }

            return insertedId;
        }
        public void GuardarHistorialMaquina(List<TEC_HistorialMaquinaEntidad> listadohistorialmaquina) {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("CodSala", typeof(int));
            dataTable.Columns.Add("CodMaquina", typeof(string));
            dataTable.Columns.Add("EstadoMaquina", typeof(string));
            dataTable.Columns.Add("FechaOperacion", typeof(DateTime));

            foreach(var registro in listadohistorialmaquina) {
                dataTable.Rows.Add(registro.CodSala, registro.CodMaquina, registro.EstadoMaquina, registro.FechaOperacion);
            }

            using(SqlConnection connection = new SqlConnection(_conexion)) {
                connection.Open();

                using(SqlBulkCopy bulkCopy = new SqlBulkCopy(connection)) {
                    bulkCopy.DestinationTableName = "TEC_HistorialMaquina";
                     
                    bulkCopy.ColumnMappings.Add("CodSala", "CodSala");
                    bulkCopy.ColumnMappings.Add("CodMaquina", "CodMaquina");
                    bulkCopy.ColumnMappings.Add("EstadoMaquina", "EstadoMaquina");
                    bulkCopy.ColumnMappings.Add("FechaOperacion", "FechaOperacion"); 
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }

        public (List<TEC_ConsolidadoMaquinaEntidad> consolidadoLista, ClaseError error) ObtenerConsolidado(string strElementos, DateTime fechaini, DateTime fechafin) {
            List<TEC_ConsolidadoMaquinaEntidad> listaConsolidado = new List<TEC_ConsolidadoMaquinaEntidad>();
            ClaseError error = new ClaseError();

            string consulta = $@"
        SELECT 
            A.sala_id, 
            s.Nombre AS sala,
            SUM(A.CantMaquinaConectada) AS TotalConectadas, 
            SUM(A.CantMaquinaNoConectada) AS TotalDesconectadas, 
            SUM(A.TotalMaquina) AS TotalMaquinas
        FROM 
            TEC_EstadoMaquina AS A
        JOIN 
            Sala AS s ON s.CodSala = A.sala_id
        WHERE
            {strElementos} CONVERT(date, A.FechaRegistro) BETWEEN @p0 AND @p1
        GROUP BY 
            A.sala_id, s.Nombre
        ORDER BY 
            A.sala_id";

            try {
                using(SqlConnection conexion = new SqlConnection(_conexion)) {
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@p0", fechaini);
                    comando.Parameters.AddWithValue("@p1", fechafin);

                    conexion.Open();
                    SqlDataReader lector = comando.ExecuteReader();

                    while(lector.Read()) {
                        TEC_ConsolidadoMaquinaEntidad consolidado = new TEC_ConsolidadoMaquinaEntidad {
                            SalaId = Convert.ToInt32(lector["sala_id"]),
                            Sala = lector["sala"].ToString(),
                            TotalConectadas = Convert.ToInt32(lector["TotalConectadas"]),
                            TotalDesconectadas = Convert.ToInt32(lector["TotalDesconectadas"]),
                            TotalMaquinas = Convert.ToInt32(lector["TotalMaquinas"])
                        };
                        listaConsolidado.Add(consolidado);
                    }
                }
            } catch(Exception ex) {
                error.Key = "Error";
                error.Value = ex.Message;
            }

            return (listaConsolidado, error);
        }
        public (List<TEC_EstadoMaquinaEntidad> lista, ClaseError error) TEC_EstadoMaquinaListarporIdsJson(string ids) {
            List<TEC_EstadoMaquinaEntidad> lista = new List<TEC_EstadoMaquinaEntidad>();
            ClaseError error = new ClaseError();
            string consulta = @"SELECT Id
								,A.sala_id
								,B.Nombre as sala
								,A.CantMaquinaConectada
								,A.CantMaquinaNoConectada
                                ,A.CantMaquinaPLay
								,A.TotalMaquina
								,A.FechaRegistro
							
                                FROM [TEC_EstadoMaquina] as A
								join Sala B on B.CodSala = A.sala_id 
                                where " + ids + ";";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                TEC_EstadoMaquinaEntidad maquinaestado = new TEC_EstadoMaquinaEntidad() {
                                    id = ManejoNulos.ManageNullInteger64(dr["Id"]),
                                    sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                    sala = ManejoNulos.ManageNullStr(dr["sala"]),
                                    CantMaquinaConectada = ManejoNulos.ManageNullInteger(dr["CantMaquinaConectada"]),
                                    CantMaquinaNoConectada= ManejoNulos.ManageNullInteger(dr["CantMaquinaNoConectada"]),
                                    CantMaquinaPLay = ManejoNulos.ManageNullInteger(dr["CantMaquinaPLay"]),
                                    TotalMaquina= ManejoNulos.ManageNullInteger(dr["TotalMaquina"]),
                                    FechaRegistro= ManejoNulos.ManageNullDate(dr["FechaRegistro"]), 
                                 
                                };
                                lista.Add(maquinaestado);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (lista: lista, error);
        } 
        public string NombreId(int usuarioId) {

            string nombreUsuario = string.Empty;
            string query = " SELECT UsuarioNombre FROM SEG_Usuario WHERE UsuarioID = @UsuarioID";

            using(SqlConnection connection = new SqlConnection(_conexion)) {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@UsuarioID", usuarioId);

                var result = command.ExecuteScalar();
                if(result != null) {
                    nombreUsuario = result.ToString();
                }

            }

            return nombreUsuario;
        } 
        public List<SalaEntidad> ObtenerTodasLasSalas() {
            List<SalaEntidad> listaSalas = new List<SalaEntidad>();
            string consulta = "SELECT CodSala, Nombre FROM Salas WHERE estado = 1"; // Ajusta la consulta según tu base de datos

            using(var con = new SqlConnection(_conexion)) {
                con.Open();
                using(var cmd = new SqlCommand(consulta, con)) {
                    using(var reader = cmd.ExecuteReader()) {
                        while(reader.Read()) {
                            SalaEntidad sala = new SalaEntidad {
                                CodSala = reader.GetInt32(0),
                                Nombre = reader.GetString(1)
                            };
                            listaSalas.Add(sala);
                        }
                    }
                }
            }
            return listaSalas;
        } 
        public TEC_EstadoMaquinaEntidad GetMaquinaEstadoPorId(int id) {
            TEC_EstadoMaquinaEntidad item = new TEC_EstadoMaquinaEntidad();
            string consulta = @"SELECT  
                              [Id]
                              ,[sala_id]
                              ,[CantMaquinaConectada]
                              ,[CantMaquinaNoConectada]
                              ,[CantMaquinaPLay]
                              ,[TotalMaquina]
                              ,[CantMaquinaRetiroTemporal]
                              ,[FechaRegistro]
                          FROM TEC_EstadoMaquina where Id = @IdEstadoMaquina";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEstadoMaquina", id);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                item.id = ManejoNulos.ManageNullInteger(dr["Id"]);
                                item.sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]); 
                                item.CantMaquinaConectada = ManejoNulos.ManageNullInteger(dr["CantMaquinaConectada"]);
                                item.CantMaquinaNoConectada = ManejoNulos.ManageNullInteger(dr["Id"]);
                                item.CantMaquinaPLay = ManejoNulos.ManageNullInteger(dr["CantMaquinaPLay"]);
                                item.TotalMaquina = ManejoNulos.ManageNullInteger(dr["TotalMaquina"]);
                                item.CantMaquinaRetiroTemporal = ManejoNulos.ManageNullInteger(dr["CantMaquinaRetiroTemporal"]); 
                                item.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                            }
                        }
                    };
                    SetMaquinas(item, con);
                }
            } catch(Exception ex) {
                item = new TEC_EstadoMaquinaEntidad();
            }
            return item;

        }
        private void SetMaquinas(TEC_EstadoMaquinaEntidad estadomaquina, SqlConnection context) {
            var maquinas = new List<TEC_EstadoMaquinaDetalleEntidad>();
            var command = new SqlCommand(@"SELECT [IdEstadoMaquinaDetalle]
              ,[IdEstadoMaquina]
              ,[CodMaquina]
              ,[CodSala]
              ,[NombreSala]
              ,[Fecha]
              ,[UsuarioRegistro]
              ,[UsuarioModificacion]
              ,[FechaModificacion]
              ,[FechaRegistro]
              FROM [TEC_EstadoMaquinaDetalle] where [IdEstadoMaquina] = @p0", context);
            command.Parameters.AddWithValue("@p0", estadomaquina.id);
            using(var reader = command.ExecuteReader()) {
                if(reader.HasRows) {
                    while(reader.Read()) {
                        maquinas.Add(new TEC_EstadoMaquinaDetalleEntidad() {
                            IdEstadoMaquina = ManejoNulos.ManageNullInteger(reader["IdEstadoMaquina"]),
                            CodMaquina = ManejoNulos.ManageNullStr(reader["CodMaquina"]),
                            CodSala = ManejoNulos.ManageNullInteger(reader["CodSala"]),
                            NombreSala = ManejoNulos.ManageNullStr(reader["NombreSala"]),
                            Fecha = ManejoNulos.ManageNullDate(reader["Fecha"]),
                            FechaRegistro = ManejoNulos.ManageNullDate(reader["FechaRegistro"]),
                        });
                    }
                }
            };
            estadomaquina.Maquinas = maquinas;
        }

        //con.Open();
        //            var query = new SqlCommand(consulta, con);
        //query.Parameters.AddWithValue("@p0", codsala);
        public List<TEC_EstadoMaquinaDetalleEntidad> ListarRetiroTemporal(int IdEstadoMaquina ) {
            List<TEC_EstadoMaquinaDetalleEntidad> lista = new List<TEC_EstadoMaquinaDetalleEntidad>();
            string consulta = @"SELECT
                               [IdEstadoMaquinaDetalle]
                              ,[IdEstadoMaquina]
                              ,[CodMaquina]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioModificacion]
                              ,[UsuarioRegistro]
                          FROM [TEC_EstadoMaquinaDetalle]
where IdEstadoMaquina = @IdEstadoMaquina";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEstadoMaquina", ManejoNulos.ManageNullInteger(IdEstadoMaquina));

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new TEC_EstadoMaquinaDetalleEntidad {
                                IdEstadoMaquinaDetalle = ManejoNulos.ManageNullInteger(dr["IdEstadoMaquinaDetalle"]),
                                IdEstadoMaquina = ManejoNulos.ManageNullInteger(dr["IdEstadoMaquina"]),
                                CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]),
                            };
                            lista.Add(item);
                        }
                    }

                }
            } catch(Exception ex) {
                lista = new List<TEC_EstadoMaquinaDetalleEntidad>();
            }
            return lista;
        }

        public int InsertarRetiroTemporal(TEC_EstadoMaquinaDetalleEntidad model) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [TEC_EstadoMaquinaDetalle]
           ([IdEstadoMaquina]
           ,[CodMaquina]
           ,[FechaRegistro]
           ,[UsuarioRegistro])
OUTPUT Inserted.IdEstadoMaquinaDetalle
     VALUES
           (@IdEstadoMaquina
            ,@CodMaquina
            ,@FechaRegistro
            ,@UsuarioRegistro)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEstadoMaquina", ManejoNulos.ManageNullInteger(model.IdEstadoMaquina));
                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullStr(model.CodMaquina));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(model.FechaRegistro));
                    query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(model.UsuarioRegistro));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                IdInsertado = 0;
            }
            return IdInsertado;
        }
        public bool ActualizarCantidadRetiroTemporal(int idEstadoMaquina) {
            bool resultado = false;
            string consulta = @"
        UPDATE TEC_EstadoMaquina
        SET CantMaquinaRetiroTemporal = (
            SELECT COUNT(*) ActualizarCantidadRetiroTemporal
            FROM TEC_EstadoMaquinaDetalle 
            WHERE IdEstadoMaquina = @IdEstadoMaquina
        )
        WHERE Id = @IdEstadoMaquina";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEstadoMaquina", idEstadoMaquina);
                    int filasAfectadas = query.ExecuteNonQuery();
                    if(filasAfectadas > 0) {
                        resultado = true;
                    }
                }
            } catch(Exception ex) {
                resultado = false;
                Console.WriteLine("Error al actualizar la cantidad de retiro temporal: " + ex.Message);
            }
            return resultado;
        }

        public bool EliminarRetiroTemporal(int idEstadoMaquinaDetalle) {
            bool resultado = false;
            string consulta = @"DELETE FROM [TEC_EstadoMaquinaDetalle]
                        WHERE [IdEstadoMaquinaDetalle] = @IdEstadoMaquinaDetalle";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEstadoMaquinaDetalle", idEstadoMaquinaDetalle);

                    int filasAfectadas = query.ExecuteNonQuery();
                    if(filasAfectadas > 0) {
                        resultado = true; 
                    }
                }
            } catch(Exception ex) {
                resultado = false;
                Console.WriteLine("Error al eliminar el registro: " + ex.Message);
            }

            return resultado;
        }
         
        public int ExisteRegistro(DateTime FechaOperacion, int salaId) {
            int idEstadoMaquina = 0;
            string query = @"
        SELECT TOP 1 Id
        FROM TEC_EstadoMaquina
        WHERE CAST(FechaOperacion AS DATE) = @FechaOperacion
        AND sala_id = @salaId
    ";

            try {
                using(var connection = new SqlConnection(_conexion)) {
                    connection.Open();
                    using(var command = new SqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@FechaOperacion", ManejoNulos.ManageNullDate(FechaOperacion.Date));
                        command.Parameters.AddWithValue("@salaId", ManejoNulos.ManageNullInteger(salaId));
                        idEstadoMaquina = ManejoNulos.ManageNullInteger(command.ExecuteScalar());
                    }
                }
            } catch(Exception ex) {
                Trace.WriteLine(ex.Message + " " + GetType().FullName + " " + DateTime.Now.ToLongDateString()); 
            }

            return idEstadoMaquina;
        }

        public bool ActualizarMaquinaEstado(TEC_EstadoMaquinaEntidad maquinaestado, int idEstadoMaquina) {
            bool actualizado = false;
            string query = @"
        UPDATE TEC_EstadoMaquina
        SET 
            CantMaquinaConectada = @CantMaquinaConectada,
            CantMaquinaNoConectada = @CantMaquinaNoConectada,
            CantMaquinaPLay = @CantMaquinaPLay,
            TotalMaquina = @TotalMaquina,
            FechaOperacion = @FechaOperacion,
            FechaCierre = @FechaCierre,
            FechaModificacion = @FechaModificacion
        WHERE 
            Id = @idEstadoMaquina"; 
            try {
                using(var connection = new SqlConnection(_conexion)) {
                    connection.Open();
                    using(var command = new SqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@CantMaquinaConectada", ManejoNulos.ManageNullInteger(maquinaestado.CantMaquinaConectada));
                        command.Parameters.AddWithValue("@CantMaquinaNoConectada", ManejoNulos.ManageNullInteger(maquinaestado.CantMaquinaNoConectada));
                        command.Parameters.AddWithValue("@CantMaquinaPLay", ManejoNulos.ManageNullInteger(maquinaestado.CantMaquinaPLay));
                        command.Parameters.AddWithValue("@TotalMaquina", ManejoNulos.ManageNullInteger(maquinaestado.TotalMaquina));
                        command.Parameters.AddWithValue("@FechaOperacion", ManejoNulos.ManageNullDate(maquinaestado.FechaOperacion.Date));
                        command.Parameters.AddWithValue("@FechaCierre", ManejoNulos.ManageNullDate(maquinaestado.FechaCierre));
                        command.Parameters.AddWithValue("@idEstadoMaquina", ManejoNulos.ManageNullInteger(idEstadoMaquina));

                        command.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(DateTime.Now));

                        int rowsAffected = command.ExecuteNonQuery();
                        if(rowsAffected > 0) {
                            actualizado = true;
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                actualizado = false;
            }

            return actualizado;
        }

        public int InsertarRegistroMaquina(TEC_RegistroMaquinaEntidad registro) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [TEC_RegistroMaquina]
           ([CodSala]
           ,[CodMaquinaINDECI]
           ,[CodMaquinaRD]
           ,[TotalMaquina]
           ,[FechaRegistro]
           ,[UsuarioRegistro])
            OUTPUT Inserted.IdRegistroMaquina
            VALUES
           (@CodSala
            ,@CodMaquinaINDECI
            ,@CodMaquinaRD
            ,@TotalMaquina
            ,@FechaRegistro
            ,@UsuarioRegistro)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(registro.CodSala));
                    query.Parameters.AddWithValue("@CodMaquinaINDECI", ManejoNulos.ManageNullStr(registro.CodMaquinaINDECI));
                    query.Parameters.AddWithValue("@CodMaquinaRD", ManejoNulos.ManageNullStr(registro.CodMaquinaRD));
                    query.Parameters.AddWithValue("@TotalMaquina", ManejoNulos.ManageNullInteger(registro.TotalMaquina));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(registro.FechaRegistro));
                    query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(registro.UsuarioRegistro));
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                IdInsertado = 0;
            }
            return IdInsertado;
        }

            public int ObtenerTotalMaquinaPorCodSala(int codSala) {
                int totalMaquina = 0;

                string consulta = @"
            SELECT em.TotalMaquina
            FROM TEC_EstadoMaquina em
            WHERE em.sala_id = @CodSala";

                try {
                    using(var con = new SqlConnection(_conexion)) {
                        con.Open();
                        var query = new SqlCommand(consulta, con);
                        query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(codSala));

                        var resultado = query.ExecuteScalar();

                        if(resultado != null && resultado != DBNull.Value) {
                            totalMaquina = Convert.ToInt32(resultado);
                        }
                    }
                } catch(Exception ex) {
                    totalMaquina = 0; 
                }

                return totalMaquina;
            }


        public (List<TEC_RegistroMaquinaEntidad> maquinaestadoLista, ClaseError error) ListaReporteRegistroMaquinaxSalaJson(string salas) {
            List<TEC_RegistroMaquinaEntidad> lista = new List<TEC_RegistroMaquinaEntidad>();
            ClaseError error = new ClaseError();
            string consulta = @"
   SELECT 
       tec.IdRegistroMaquina, 
       tec.CodSala,
       sa.Nombre as NombreSala,
       tec.CodMaquinaINDECI, 
       tec.CodMaquinaRD, 
       em.TotalMaquina as TotalMaquina, 
       tec.UsuarioRegistro, 
       tec.FechaRegistro,
       tec.FechaModificacion
   FROM 
       TEC_RegistroMaquina as tec
   JOIN 
       Sala as sa ON sa.CodSala = tec.CodSala
   LEFT JOIN 
       TEC_EstadoMaquina as em ON em.sala_id = tec.CodSala AND em.FechaOperacion = (SELECT MAX(FechaOperacion) FROM TEC_EstadoMaquina WHERE sala_id = tec.CodSala)
   WHERE 
        tec.CodSala IN (" + salas + ") ";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                var fila = new TEC_RegistroMaquinaEntidad {
                                    IdRegistroMaquina = ManejoNulos.ManageNullInteger(dr["IdRegistroMaquina"]),
                                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                    CodMaquinaINDECI = ManejoNulos.ManageNullStr(dr["CodMaquinaINDECI"]),
                                    NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                    CodMaquinaRD = ManejoNulos.ManageNullStr(dr["CodMaquinaRD"]),
                                    TotalMaquina = ManejoNulos.ManageNullInteger(dr["TotalMaquina"]),
                                    UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                    FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"])
                                };

                                lista.Add(fila);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }

            return (maquinaestadoLista: lista, error);
        }
        public TEC_RegistroMaquinaEntidad ReporteRegistroMaquinaxSalaJson(int codsala) {
            TEC_RegistroMaquinaEntidad lista = new TEC_RegistroMaquinaEntidad();
            ClaseError error = new ClaseError();
            string consulta = @"
    SELECT 
        tec.IdRegistroMaquina, 
        tec.CodSala,
        sa.Nombre as NombreSala,
        tec.CodMaquinaINDECI, 
        tec.CodMaquinaRD, 
        em.TotalMaquina as TotalMaquina, 
        tec.UsuarioRegistro, 
        tec.FechaRegistro 
    FROM 
        TEC_RegistroMaquina as tec
    JOIN 
        Sala as sa ON sa.CodSala = tec.CodSala
    LEFT JOIN 
        TEC_EstadoMaquina as em ON em.sala_id = tec.CodSala AND em.FechaOperacion = (SELECT MAX(FechaOperacion) FROM TEC_EstadoMaquina WHERE sala_id = tec.CodSala)
    WHERE 
        tec.CodSala = " + codsala + " ;";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                lista = new TEC_RegistroMaquinaEntidad {
                                    IdRegistroMaquina = ManejoNulos.ManageNullInteger(dr["IdRegistroMaquina"]),
                                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                    CodMaquinaINDECI = ManejoNulos.ManageNullStr(dr["CodMaquinaINDECI"]),
                                    NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                    CodMaquinaRD = ManejoNulos.ManageNullStr(dr["CodMaquinaRD"]),
                                    TotalMaquina = ManejoNulos.ManageNullInteger(dr["TotalMaquina"]),
                                    UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"])
                                }; 

                            }
                        }
                    }
                }
            } catch(Exception ex) {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }

            return lista;
        }

        public List<TEC_EstadoMaquinaDetalleEntidad> ObtenerMaquinaDetallePorIdEstadoMaquina(long idestadomaquina, SqlConnection con) {
            List<TEC_EstadoMaquinaDetalleEntidad> empleados = new List<TEC_EstadoMaquinaDetalleEntidad>();

            string consultaEmpleados = @"
        SELECT [IdEstadoMaquinaDetalle]
              ,[IdEstadoMaquina]
              ,[CodMaquina]  
              ,[FechaRegistro]
              FROM [TEC_EstadoMaquinaDetalle] where [IdEstadoMaquina] = @idestadomaquina";


            using(var cmd = new SqlCommand(consultaEmpleados, con)) {
                cmd.Parameters.AddWithValue("@idestadomaquina", idestadomaquina);

                using(var dr = cmd.ExecuteReader()) {
                    while(dr.Read()) {
                        var empleado = new TEC_EstadoMaquinaDetalleEntidad {
                            IdEstadoMaquinaDetalle = ManejoNulos.ManageNullInteger(dr["IdEstadoMaquinaDetalle"]),
                            IdEstadoMaquina = ManejoNulos.ManageNullInteger(dr["IdEstadoMaquina"]),
                            CodMaquina = ManejoNulos.ManageNullStr(dr["CodMaquina"]),
                            FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]), 
                        };
                        empleados.Add(empleado);
                    }
                }
            }
            return empleados;
        }

        public List<TEC_EstadoMaquinaEntidad> ListadoEstadoMaquina(int[] codsala, DateTime fechaIni, DateTime fechaFin) {
            List<TEC_EstadoMaquinaEntidad> lista = new List<TEC_EstadoMaquinaEntidad>();
            string strSala = string.Empty;
            strSala = $" sala_id in ({String.Join(",", codsala)}) and ";
            string consulta = $@"SELECT 
                A.Id, 
                s.Nombre AS sala,
                A.CantMaquinaConectada,
                A.CantMaquinaNoConectada,
                A.CantMaquinaPLay,
                A.CantMaquinaRetiroTemporal,
                A.[TotalMaquina],
                A.[FechaRegistro],
                A.[sala_id],
                A.FechaCierre,
                A.FechaOperacion
            FROM 
                [TEC_EstadoMaquina] AS A
            JOIN 
                Sala AS s ON s.CodSala = A.[sala_id] 
            WHERE {strSala} convert(date, A.FechaRegistro) between convert(date,@p1) and convert(date,@p2)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni);
                    query.Parameters.AddWithValue("@p2", fechaFin);


                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new TEC_EstadoMaquinaEntidad {
                                id = ManejoNulos.ManageNullInteger(dr["Id"]),
                                sala_id= ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                sala = ManejoNulos.ManageNullStr(dr["sala"]),
                                CantMaquinaConectada = ManejoNulos.ManageNullInteger(dr["CantMaquinaConectada"]),
                                CantMaquinaNoConectada = ManejoNulos.ManageNullInteger(dr["CantMaquinaNoConectada"]),
                                CantMaquinaPLay = ManejoNulos.ManageNullInteger(dr["CantMaquinaPLay"]),
                                CantMaquinaRetiroTemporal = ManejoNulos.ManageNullInteger(dr["CantMaquinaRetiroTemporal"]),
                                TotalMaquina = ManejoNulos.ManageNullInteger(dr["TotalMaquina"]),
                                FechaCierre = ManejoNulos.ManageNullDate(dr["FechaCierre"]),
                                FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"]),   

                            };

                            using(var conMaquina = new SqlConnection(_conexion)) {
                                conMaquina.Open(); 
                                    item.Maquinas = ObtenerMaquinaDetallePorIdEstadoMaquina(item.id, conMaquina);

                            }
                            lista.Add(item);
                        }
                    }
                }

            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista = new List<TEC_EstadoMaquinaEntidad>();
            } finally {
            }
            return lista;
        }

        public bool ActualizarRegistroMaquina(TEC_RegistroMaquinaEntidad registroMaquina) {
            bool actualizado = false;
            string query = @"
            UPDATE dbo.TEC_RegistroMaquina
            SET 
                CodSala = @CodSala,
                CodMaquinaINDECI = @CodMaquinaINDECI,
                CodMaquinaRD = @CodMaquinaRD,
                UsuarioModificacion = @UsuarioModificacion,
                FechaModificacion = @FechaModificacion
                
            WHERE 
                IdRegistroMaquina = @IdRegistroMaquina";

            try {
                using(var connection = new SqlConnection(_conexion)) {
                    connection.Open();
                    using(var command = new SqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(registroMaquina.CodSala));
                        command.Parameters.AddWithValue("@CodMaquinaINDECI", ManejoNulos.ManageNullStr(registroMaquina.CodMaquinaINDECI));
                        command.Parameters.AddWithValue("@CodMaquinaRD", ManejoNulos.ManageNullStr(registroMaquina.CodMaquinaRD));
                        command.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(registroMaquina.UsuarioModificacion));
                        command.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(registroMaquina.FechaModificacion)); 

                        command.Parameters.AddWithValue("@IdRegistroMaquina", ManejoNulos.ManageNullInteger(registroMaquina.IdRegistroMaquina));

                        int rowsAffected = command.ExecuteNonQuery();
                        if(rowsAffected > 0) {
                            actualizado = true;
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                actualizado = false;
            }

            return actualizado;
        }
        public bool ActualizarRegistroMaquinaINDECI(TEC_RegistroMaquinaEntidad registroMaquina) {
            bool actualizado = false;
            string query = @"
            UPDATE dbo.TEC_RegistroMaquina
            SET 
                CodMaquinaINDECI = @CodMaquinaINDECI,
                UsuarioModificacion = @UsuarioModificacion,
                FechaModificacion = @FechaModificacion
                
            WHERE 
                IdRegistroMaquina = @IdRegistroMaquina";

            try {
                using(var connection = new SqlConnection(_conexion)) {
                    connection.Open();
                    using(var command = new SqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@CodMaquinaINDECI", ManejoNulos.ManageNullStr(registroMaquina.CodMaquinaINDECI));
                        command.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(registroMaquina.UsuarioModificacion));
                        command.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(registroMaquina.FechaModificacion)); 

                        command.Parameters.AddWithValue("@IdRegistroMaquina", ManejoNulos.ManageNullInteger(registroMaquina.IdRegistroMaquina));

                        int rowsAffected = command.ExecuteNonQuery();
                        if(rowsAffected > 0) {
                            actualizado = true;
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                actualizado = false;
            }

            return actualizado;
        }
        public bool ActualizarRegistroMaquinaRD(TEC_RegistroMaquinaEntidad registroMaquina) {
            bool actualizado = false;
            string query = @"
            UPDATE dbo.TEC_RegistroMaquina
            SET 
                CodMaquinaRD = @CodMaquinaRD,
                UsuarioModificacion = @UsuarioModificacion,
                FechaModificacion = @FechaModificacion
                
            WHERE 
                IdRegistroMaquina = @IdRegistroMaquina";

            try {
                using(var connection = new SqlConnection(_conexion)) {
                    connection.Open();
                    using(var command = new SqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@CodMaquinaRD", ManejoNulos.ManageNullStr(registroMaquina.CodMaquinaRD));
                        command.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(registroMaquina.UsuarioModificacion));
                        command.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(registroMaquina.FechaModificacion)); 

                        command.Parameters.AddWithValue("@IdRegistroMaquina", ManejoNulos.ManageNullInteger(registroMaquina.IdRegistroMaquina));

                        int rowsAffected = command.ExecuteNonQuery();
                        if(rowsAffected > 0) {
                            actualizado = true;
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                actualizado = false;
            }

            return actualizado;
        }


        public int CrearSalaRegistroMaquina(TEC_RegistroMaquinaEntidad registro) {
            string query = @"
        INSERT INTO TEC_RegistroMaquina (CodSala, CodMaquinaINDECI, CodMaquinaRD, UsuarioRegistro, FechaRegistro)
        VALUES (@CodSala, @CodMaquinaINDECI, @CodMaquinaRD, @UsuarioRegistro, @FechaRegistro);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";  

            try {
                using(var connection = new SqlConnection(_conexion)) {
                    connection.Open();
                    using(var command = new SqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(registro.CodSala));
                        command.Parameters.AddWithValue("@CodMaquinaINDECI", ManejoNulos.ManageNullStr(registro.CodMaquinaINDECI));
                        command.Parameters.AddWithValue("@CodMaquinaRD", ManejoNulos.ManageNullStr(registro.CodMaquinaRD));
                        command.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(registro.UsuarioRegistro));
                        command.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(registro.FechaRegistro));

                        return (int)command.ExecuteScalar();  
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                return 0; 
            }
        }
    
        public bool AgregarRetiroTemporal(int idEstadoMaquina, string EstadoMaquina) {
            bool resultado = false;
            string consulta = @"
        UPDATE TEC_EstadoMaquina
        SET 
            CantMaquinaRetiroTemporal =  ISNULL(CantMaquinaRetiroTemporal, 0) + 1,
            CantMaquinaConectada = CASE 
                                       WHEN @EstadoMaquina = 'on' THEN CantMaquinaConectada - 1
                                       ELSE CantMaquinaConectada
                                   END,
            CantMaquinaNoConectada = CASE 
                                          WHEN @EstadoMaquina = 'off' THEN CantMaquinaNoConectada - 1
                                          ELSE CantMaquinaNoConectada
                                      END,
            CantMaquinaPLay = CASE 
                                  WHEN @EstadoMaquina = 'onplay' THEN CantMaquinaPLay - 1
                                  ELSE CantMaquinaPLay
                              END
        WHERE Id = @IdEstadoMaquina";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEstadoMaquina", idEstadoMaquina);
                    query.Parameters.AddWithValue("@EstadoMaquina", EstadoMaquina);
                    int filasAfectadas = query.ExecuteNonQuery();
                    if(filasAfectadas > 0) {
                        resultado = true;
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error al agregar retiro temporal: " + ex.Message);
            }
            return resultado;
        }
        public bool QuitarRetiroTemporal(int idEstadoMaquina,string EstadoMaquina) {
            bool resultado = false;
            string consulta = @"
        UPDATE TEC_EstadoMaquina
        SET 
            CantMaquinaRetiroTemporal =  ISNULL(CantMaquinaRetiroTemporal, 0) - 1,
            CantMaquinaConectada = CASE 
                                       WHEN @EstadoMaquina = 'on' THEN CantMaquinaConectada + 1
                                       ELSE CantMaquinaConectada
                                   END,
            CantMaquinaNoConectada = CASE 
                                          WHEN @EstadoMaquina = 'off' THEN CantMaquinaNoConectada + 1
                                          ELSE CantMaquinaNoConectada
                                      END,
            CantMaquinaPLay = CASE 
                                  WHEN @EstadoMaquina = 'onplay' THEN CantMaquinaPLay + 1
                                  ELSE CantMaquinaPLay
                              END
        WHERE Id = @IdEstadoMaquina";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEstadoMaquina", idEstadoMaquina);
                    query.Parameters.AddWithValue("@EstadoMaquina", EstadoMaquina);
                    int filasAfectadas = query.ExecuteNonQuery();
                    if(filasAfectadas > 0) {
                        resultado = true;
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine("Error al quitar retiro temporal: " + ex.Message);
            }
            return resultado;
        }

        public bool ExisteRegistroHistorialMaquinaxFechaOperacion(DateTime FechaOperacion, int CodSala) {
            bool existe = false;
            string query = @"
            SELECT COUNT(1) 
            FROM TEC_HistorialMaquina
            WHERE FechaOperacion = @FechaOperacion
              AND CodSala = @CodSala"; 

            try {
                using(var connection = new SqlConnection(_conexion)) {
                    connection.Open();
                    using(var command = new SqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@FechaOperacion", ManejoNulos.ManageNullInteger(FechaOperacion));
                        command.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullStr(CodSala));

                        int rowsAffected = (int)command.ExecuteScalar();
                        if(rowsAffected > 0) {
                            existe = true;
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                existe = false;
            }

            return existe;
        }

        public int EliminarRegistrosHistorialMaquinaAntiguos(DateTime FechaOperacion,int CodSala) {
            int filasAfectadas = 0;
            string query = @"
            DELETE FROM TEC_HistorialMaquina
            WHERE FechaOperacion < @FechaOperacion AND CodSala = @CodSala";

            using(var connection = new SqlConnection(_conexion)) {
                connection.Open();
                using(var command = new SqlCommand(query, connection)) {
                    command.Parameters.AddWithValue("@FechaOperacion", ManejoNulos.ManageNullDate(FechaOperacion));
                    command.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(CodSala));
                    filasAfectadas =(int)command.ExecuteNonQuery();
                }
            }
            return filasAfectadas; 
        }


        public string BuscarEstadoMaquinaxCodMaquina(string CodMaquina, DateTime FechaOperacion, int CodSala) {
            ClaseError error = new ClaseError();
            string resultado = string.Empty;
            string consulta = @"SELECT [EstadoMaquina]
                          FROM [TEC_HistorialMaquina]
                          WHERE CodMaquina = @CodMaquina AND CAST(FechaOperacion AS DATE) = @FechaOperacion AND CodSala = @CodSala";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@FechaOperacion", ManejoNulos.ManageNullDate(FechaOperacion.Date));
                    query.Parameters.AddWithValue("@CodMaquina", ManejoNulos.ManageNullStr(CodMaquina));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(CodSala));

                    var queryResult = query.ExecuteScalar();
                    resultado = queryResult != DBNull.Value ? (string)queryResult : string.Empty;
                }
            } catch(Exception ex) {
                resultado = string.Empty;
            }
            return resultado;
        }

        public TEC_EstadoMaquinaEntidad ObtenerEstadoMaquinaporId(int IdEstadoMaquina) {
            TEC_EstadoMaquinaEntidad registro = new TEC_EstadoMaquinaEntidad();
            ClaseError error = new ClaseError();
            string consulta = @"SELECT [Id]
                             ,[FechaOperacion]
                             ,[sala_id]
                          FROM [TEC_EstadoMaquina]
                          WHERE Id = @IdEstadoMaquina";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdEstadoMaquina", ManejoNulos.ManageNullInteger(IdEstadoMaquina));

                    using(var dr = query.ExecuteReader()) {
                        if(dr.HasRows) {
                            while(dr.Read()) {
                                registro = new TEC_EstadoMaquinaEntidad {
                                    id = ManejoNulos.ManageNullInteger64(dr["Id"]),
                                    sala_id = ManejoNulos.ManageNullInteger(dr["sala_id"]),
                                    FechaOperacion = ManejoNulos.ManageNullDate(dr["FechaOperacion"]),
                                };

                            }
                        }
                    }

                }
            } catch(Exception ex) {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (registro);
        }
    }

} 