using CapaEntidad.BUK;
using CapaEntidad.EntradaSalidaSala;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.EntradaSalidaSala {
    public class ESS_AccionCajaTemporizadaDAL {
        string _conexion = string.Empty;
        public ESS_AccionCajaTemporizadaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<ESS_AccionCajaTemporizadaEntidad> ListadoAccionCajaTemporizada(int[] codSala, DateTime fechaIni, DateTime fechaFin) {
            List<ESS_AccionCajaTemporizadaEntidad> lista = new List<ESS_AccionCajaTemporizadaEntidad>();
            string strSala = string.Empty;

            //if(codSala != null && codSala.Length > 0 && !codSala.Contains(-1)) {
            strSala = $" CodSala IN ({String.Join(",", codSala)}) AND ";
            //}

            string consulta = $@"
                SELECT IdAccionCajaTemporizada,
                IdEmpleadoESS,
                CodSala,
                NombreSala,
                Fecha,
                IdDispositivo, NombreDispositivo, DescripcionDispositivo, IdDeficiencia, NombreDeficiencia,
                MedidaAdoptada, FechaSolucion, UsuarioRegistro, UsuarioModificacion, 
                A.FechaRegistro, A.FechaModificacion, Estado, IdAutoriza, NombreAutoriza,
                B.Cargo,
                B.NumeroDocumento,
                B.TipoDocumento
                FROM ESS_AccionCajaTemporizada as A
                LEFT JOIN BUK_Empleado as B on A.IdAutoriza = B.IdBuk
                WHERE {strSala} CONVERT(date, A.Fecha) BETWEEN CONVERT(date, @p1) AND CONVERT(date, @p2)
            ";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", fechaIni);
                    query.Parameters.AddWithValue("@p2", fechaFin);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_AccionCajaTemporizadaEntidad {
                                IdAccionCajaTemporizada = ManejoNulos.ManageNullInteger(dr["IdAccionCajaTemporizada"]),
                                IdEmpleadoESS = ManejoNulos.ManageNullInteger(dr["IdEmpleadoESS"]),
                                CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                Fecha = ManejoNulos.ManageNullDate(dr["Fecha"]),
                                IdDispositivo = ManejoNulos.ManageNullInteger(dr["IdDispositivo"]),
                                NombreDispositivo = ManejoNulos.ManageNullStr(dr["NombreDispositivo"]),
                                DescripcionDispositivo = ManejoNulos.ManageNullStr(dr["DescripcionDispositivo"]),
                                IdDeficiencia = ManejoNulos.ManageNullInteger(dr["IdDeficiencia"]),
                                NombreDeficiencia = ManejoNulos.ManageNullStr(dr["NombreDeficiencia"]),
                                MedidaAdoptada = ManejoNulos.ManageNullStr(dr["MedidaAdoptada"]),
                                FechaSolucion = ManejoNulos.ManageNullDate(dr["FechaSolucion"]),
                                UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]),
                                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
                                IdAutoriza = ManejoNulos.ManageNullInteger(dr["IdAutoriza"]),
                                NombreAutoriza = ManejoNulos.ManageNullStr(dr["NombreAutoriza"]),
                                Cargo = ManejoNulos.ManageNullStr(dr["Cargo"]),
                                TipoDocumento = ManejoNulos.ManageNullStr(dr["TipoDocumento"]),
                                DocumentoRegistro = ManejoNulos.ManageNullStr(dr["NumeroDocumento"]),
                            };

                            using(var conRelacion = new SqlConnection(_conexion)) {
                                conRelacion.Open();
                                item.Dispositivo = ObtenerDispositivoPorId(item.IdDispositivo, conRelacion);
                                item.Deficiencia = ObtenerDeficienciaPorId(item.IdDeficiencia, conRelacion);
                            }

                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                lista = new List<ESS_AccionCajaTemporizadaEntidad>();
            }

            return lista;
        }

        private ESS_DispositivoEntidad ObtenerDispositivoPorId(int idDispositivo, SqlConnection con) {
            var dispositivo = new ESS_DispositivoEntidad();
            string consulta = "SELECT [IdDispositivo], [Nombre], [Estado], [FechaRegistro], [FechaModificacion], [UsuarioRegistro], [UsuarioModificacion] FROM [ESS_Dispositivo] WHERE IdDispositivo = @idDispositivo";

            using(var query = new SqlCommand(consulta, con)) {
                query.Parameters.AddWithValue("@idDispositivo", idDispositivo);
                using(var dr = query.ExecuteReader()) {
                    if(dr.Read()) {
                        dispositivo.IdDispositivo = ManejoNulos.ManageNullInteger(dr["IdDispositivo"]);
                        dispositivo.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                        dispositivo.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                        dispositivo.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                        dispositivo.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                        dispositivo.UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]);
                        dispositivo.UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]);
                    }
                }
            }

            return dispositivo;
        }

        private ESS_DeficienciaEntidad ObtenerDeficienciaPorId(int idDeficiencia, SqlConnection con) {
            var deficiencia = new ESS_DeficienciaEntidad();
            string consulta = "SELECT [IdDeficiencia], [Nombre], [Estado], [FechaRegistro], [FechaModificacion], [UsuarioRegistro], [UsuarioModificacion] FROM [ESS_Deficiencia] WHERE IdDeficiencia = @idDeficiencia";

            using(var query = new SqlCommand(consulta, con)) {
                query.Parameters.AddWithValue("@idDeficiencia", idDeficiencia);
                using(var dr = query.ExecuteReader()) {
                    if(dr.Read()) {
                        deficiencia.IdDeficiencia = ManejoNulos.ManageNullInteger(dr["IdDeficiencia"]);
                        deficiencia.Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]);
                        deficiencia.Estado = ManejoNulos.ManageNullInteger(dr["Estado"]);
                        deficiencia.FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]);
                        deficiencia.FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]);
                        deficiencia.UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]);
                        deficiencia.UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]);
                    }
                }
            }

            return deficiencia;
        }

        public int GuardarAccionCajaTemporizada(ESS_AccionCajaTemporizadaEntidad registro) {
            int idInsertado = 0;
            string queryAccionCajaTemporizada = @"INSERT INTO [ESS_AccionCajaTemporizada]
                                            ([IdEmpleadoESS], [CodSala], [NombreSala], [Fecha], [IdDispositivo], [NombreDispositivo], [DescripcionDispositivo],
                                             [IdDeficiencia], [NombreDeficiencia], [MedidaAdoptada], 
                                             [UsuarioRegistro], [FechaRegistro], [IdAutoriza],[NombreAutoriza])
                                          OUTPUT inserted.IdAccionCajaTemporizada
                                          VALUES (@IdEmpleadoESS, @CodSala, @NombreSala, @Fecha, @IdDispositivo, @NombreDispositivo, @DescripcionDispositivo,
                                                  @IdDeficiencia, @NombreDeficiencia, @MedidaAdoptada, 
                                                  @UsuarioRegistro, @FechaRegistro, @IdAutoriza,@NombreAutoriza)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var transaction = con.BeginTransaction()) {
                        var query = new SqlCommand(queryAccionCajaTemporizada, con, transaction);
                        query.Parameters.AddWithValue("@IdEmpleadoESS", ManejoNulos.ManageNullInteger(registro.IdEmpleadoESS));
                        query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(registro.CodSala));
                        query.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(registro.NombreSala));
                        query.Parameters.AddWithValue("@Fecha", ManejoNulos.ManageNullDate(registro.Fecha));
                        query.Parameters.AddWithValue("@IdDispositivo", ManejoNulos.ManageNullInteger(registro.IdDispositivo));
                        query.Parameters.AddWithValue("@NombreDispositivo", ManejoNulos.ManageNullStr(registro.NombreDispositivo));
                        query.Parameters.AddWithValue("@DescripcionDispositivo", ManejoNulos.ManageNullStr(registro.DescripcionDispositivo));
                        query.Parameters.AddWithValue("@IdDeficiencia", ManejoNulos.ManageNullInteger(registro.IdDeficiencia));
                        query.Parameters.AddWithValue("@NombreDeficiencia", ManejoNulos.ManageNullStr(registro.NombreDeficiencia));
                        query.Parameters.AddWithValue("@MedidaAdoptada", ManejoNulos.ManageNullStr(registro.MedidaAdoptada));
                        //query.Parameters.AddWithValue("@FechaSolucion", ManejoNulos.ManageNullDate(registro.FechaSolucion));
                        query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(registro.UsuarioRegistro));
                        query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(registro.FechaRegistro));
                        query.Parameters.AddWithValue("@IdAutoriza", ManejoNulos.ManageNullInteger(registro.IdAutoriza));
                        query.Parameters.AddWithValue("@NombreAutoriza", ManejoNulos.ManageNullStr(registro.NombreAutoriza));

                        idInsertado = Convert.ToInt32(query.ExecuteScalar());

                        transaction.Commit();
                    }
                }
            } catch(Exception ex) {
                idInsertado = 0;
                Console.WriteLine(ex.Message);
            }

            return idInsertado;
        }
        public int GuardarAccionCajaTemporizadafromImportar(ESS_AccionCajaTemporizadaEntidad registro) {
            int idInsertado = 0;
            string queryAccionCajaTemporizada = @"INSERT INTO [ESS_AccionCajaTemporizada]
                                            ([IdEmpleadoESS], [CodSala], [NombreSala], [Fecha], [IdDispositivo], [NombreDispositivo], [DescripcionDispositivo],
                                             [IdDeficiencia], [NombreDeficiencia], [MedidaAdoptada],FechaSolucion, 
                                             [UsuarioRegistro], [FechaRegistro], [IdAutoriza],[NombreAutoriza])
                                          OUTPUT inserted.IdAccionCajaTemporizada
                                          VALUES (@IdEmpleadoESS, @CodSala, @NombreSala, @Fecha, @IdDispositivo, @NombreDispositivo, @DescripcionDispositivo,
                                                  @IdDeficiencia, @NombreDeficiencia, @MedidaAdoptada,@FechaSolucion,
                                                  @UsuarioRegistro, @FechaRegistro, @IdAutoriza,@NombreAutoriza)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var transaction = con.BeginTransaction()) {
                        var query = new SqlCommand(queryAccionCajaTemporizada, con, transaction);
                        query.Parameters.AddWithValue("@IdEmpleadoESS", ManejoNulos.ManageNullInteger(registro.IdEmpleadoESS));
                        query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(registro.CodSala));
                        query.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(registro.NombreSala));
                        query.Parameters.AddWithValue("@Fecha", ManejoNulos.ManageNullDate(registro.Fecha));
                        query.Parameters.AddWithValue("@IdDispositivo", ManejoNulos.ManageNullInteger(registro.IdDispositivo));
                        query.Parameters.AddWithValue("@NombreDispositivo", ManejoNulos.ManageNullStr(registro.NombreDispositivo));
                        query.Parameters.AddWithValue("@DescripcionDispositivo", ManejoNulos.ManageNullStr(registro.DescripcionDispositivo));
                        query.Parameters.AddWithValue("@IdDeficiencia", ManejoNulos.ManageNullInteger(registro.IdDeficiencia));
                        query.Parameters.AddWithValue("@NombreDeficiencia", ManejoNulos.ManageNullStr(registro.NombreDeficiencia));
                        query.Parameters.AddWithValue("@MedidaAdoptada", ManejoNulos.ManageNullStr(registro.MedidaAdoptada));
                        query.Parameters.AddWithValue("@FechaSolucion", ManejoNulos.ManageNullDate(registro.FechaSolucion));
                        query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(registro.UsuarioRegistro));
                        query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(registro.FechaRegistro));
                        query.Parameters.AddWithValue("@IdAutoriza", ManejoNulos.ManageNullInteger(registro.IdAutoriza));
                        query.Parameters.AddWithValue("@NombreAutoriza", ManejoNulos.ManageNullStr(registro.NombreAutoriza));

                        idInsertado = Convert.ToInt32(query.ExecuteScalar());

                        transaction.Commit();
                    }
                }
            } catch(Exception ex) {
                idInsertado = 0;
                Console.WriteLine(ex.Message);
            }

            return idInsertado;
        }

        public bool ActualizarAccionCajaTemporizada(ESS_AccionCajaTemporizadaEntidad registro) {
            bool respuesta = false;

            string consulta = @"UPDATE [ESS_AccionCajaTemporizada]
                        SET [CodSala] = @CodSala, [NombreSala] = @NombreSala, [Fecha] = @Fecha, 
                            [IdDispositivo] = @IdDispositivo, [NombreDispositivo] = @NombreDispositivo, 
                            [IdDeficiencia] = @IdDeficiencia, [NombreDeficiencia] = @NombreDeficiencia, 
                            [MedidaAdoptada] = @MedidaAdoptada, [NombreAutoriza] = @NombreAutoriza,IdAutoriza = @IdAutoriza,
                            [UsuarioModificacion] = @UsuarioModificacion, [FechaModificacion] = @FechaModificacion,
                            [DescripcionDispositivo] = @DescripcionDispositivo
                        WHERE [IdAccionCajaTemporizada] = @IdAccionCajaTemporizada";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();

                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdAccionCajaTemporizada", ManejoNulos.ManageNullInteger(registro.IdAccionCajaTemporizada));
                    query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(registro.CodSala));
                    query.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(registro.NombreSala));
                    query.Parameters.AddWithValue("@Fecha", ManejoNulos.ManageNullDate(registro.Fecha));
                    query.Parameters.AddWithValue("@IdDispositivo", ManejoNulos.ManageNullInteger(registro.IdDispositivo));
                    query.Parameters.AddWithValue("@NombreDispositivo", ManejoNulos.ManageNullStr(registro.NombreDispositivo));
                    query.Parameters.AddWithValue("@IdDeficiencia", ManejoNulos.ManageNullInteger(registro.IdDeficiencia));
                    query.Parameters.AddWithValue("@NombreDeficiencia", ManejoNulos.ManageNullStr(registro.NombreDeficiencia));
                    query.Parameters.AddWithValue("@MedidaAdoptada", ManejoNulos.ManageNullStr(registro.MedidaAdoptada));
                    //query.Parameters.AddWithValue("@FechaSolucion", ManejoNulos.ManageNullDate(registro.FechaSolucion));
                    query.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(registro.UsuarioModificacion));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(registro.FechaModificacion));
                    query.Parameters.AddWithValue("@NombreAutoriza", ManejoNulos.ManageNullStr(registro.NombreAutoriza));
                    query.Parameters.AddWithValue("@IdAutoriza", ManejoNulos.ManageNullInteger(registro.IdAutoriza));
                    query.Parameters.AddWithValue("@DescripcionDispositivo", ManejoNulos.ManageNullStr(registro.DescripcionDispositivo));


                    int filasAfectadas = query.ExecuteNonQuery();
                    respuesta = (filasAfectadas > 0);
                }
            } catch(Exception ex) {
                respuesta = false;
                Console.WriteLine(ex.Message);
            }

            return respuesta;
        }


        public bool EliminarAccionCajaTemporizada(int idAccionCajaTemporizada) {
            bool respuesta = false;
            string consulta = "DELETE FROM [ESS_AccionCajaTemporizada] WHERE [IdAccionCajaTemporizada] = @IdAccionCajaTemporizada";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdAccionCajaTemporizada", idAccionCajaTemporizada);
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine("Error: " + ex.Message);
                respuesta = false;
            }

            return respuesta;
        }


        public bool FinalizarHoraRegistroAccionCajaTemporizada(int idAccionCajaTemporizada, DateTime horaSalida) {
            bool respuesta = false;

            string consulta = @"
                         UPDATE [ESS_AccionCajaTemporizada]
                         SET FechaSolucion = @FechaSolucion
                         WHERE IdAccionCajaTemporizada = @IdAccionCajaTemporizada";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var queryRegistro = new SqlCommand(consulta, con);
                    queryRegistro.Parameters.AddWithValue("@FechaSolucion", horaSalida);
                    queryRegistro.Parameters.AddWithValue("@IdAccionCajaTemporizada", idAccionCajaTemporizada);
                    queryRegistro.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }


        public List<ESS_AccionCajaTemporizadaCargoEntidad> ListarEmpleadosGerentesYJefes() {
            List<ESS_AccionCajaTemporizadaCargoEntidad> lista = new List<ESS_AccionCajaTemporizadaCargoEntidad>();
            string consulta = @"

                        SELECT 
                            [IdBuk]
                            ,[NombreCompleto]
                            ,[Cargo]
                            ,NumeroDocumento 
                        FROM [BUK_Empleado]
                        WHERE IdCargo IN (44,59,62)
						 
";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_AccionCajaTemporizadaCargoEntidad {
                                IdAutoriza = ManejoNulos.ManageNullInteger(dr["IdBuk"]),
                                NombreAutoriza = ManejoNulos.ManageNullStr(dr["NombreCompleto"]),
                                Cargo = ManejoNulos.ManageNullStr(dr["Cargo"]),
                                NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumento"])
                            };
                            lista.Add(item);
                        }
                    }
                }
            } catch(Exception ex) {
                lista = new List<ESS_AccionCajaTemporizadaCargoEntidad>();
            }
            return lista;
        }


        public BUK_EmpleadoEntidad ObtenerEmpleadoPorDocumentoBUK(string numeroDocumento) {
            string consulta = @"
        SELECT TOP 1 
            IdBuk,
            NombreCompleto
        FROM [dbo].[BUK_Empleado] 
        WHERE NumeroDocumento = @NumeroDocumento
        AND (EstadoCese = 0 OR EstadoCese IS NULL)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@NumeroDocumento", numeroDocumento);

                    using(var dr = query.ExecuteReader()) {
                        if(dr.Read()) {
                            return new BUK_EmpleadoEntidad {
                                IdBuk = ManejoNulos.ManageNullInteger(dr["IdBuk"]),
                                NombreCompleto = ManejoNulos.ManageNullStr(dr["NombreCompleto"])
                            };
                        }
                    }
                }
            } catch(Exception) {
                return null;
            }
            return null;
        }
    }
}