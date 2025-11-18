using CapaEntidad.EntradaSalidaSala;
using S3k.Utilitario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.EntradaSalidaSala {
    public class ESS_LogOcurrenciaDAL {

        private readonly string _conexion;

        public ESS_LogOcurrenciaDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ESS_LogOcurrenciaEntidad> ListadoLogsOcurrencia(int[] codSala, DateTime fechaInicio, DateTime fechaFin) {
            List<ESS_LogOcurrenciaEntidad> lista = new List<ESS_LogOcurrenciaEntidad>();
            string strSala = string.Empty; 
            strSala = $" codsala in ({String.Join(",", codSala)}) and "; 
            string consulta = $@"
                SELECT 
                    IdLogOcurrencia, CodSala, NombreSala, IdArea, NombreArea, DescripcionArea,
                    IdTipologia, NombreTipologia, DescripcionTipologia, IdActuante, NombreActuante,
                    DescripcionActuante, Detalle, IdComunicacion, NombreComunicacion, DescripcionComunicacion,
                    AccionEjecutada, LO.IdEstadoOcurrencia, EO.Nombre as EstadoOcurrencia, FechaSolucion, LO.UsuarioRegistro, LO.UsuarioModificacion,
                    LO.FechaRegistro, LO.FechaModificacion, LO.Fecha
                FROM [ESS_LogOcurrencia] LO
                LEFT JOIN ESS_EstadoOcurrencia EO on EO.IdEstadoOcurrencia = LO.IdEstadoOcurrencia 
                WHERE {strSala} CONVERT(date, LO.Fecha) BETWEEN CONVERT(date, @p1) AND CONVERT(date, @p2)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@p1", fechaInicio);
                        query.Parameters.AddWithValue("@p2", fechaFin);

                        using(var dr = query.ExecuteReader()) {
                            while(dr.Read()) {
                                var logArray = new ESS_LogOcurrenciaEntidad {
                                    IdLogOcurrencia = ManejoNulos.ManageNullInteger(dr["IdLogOcurrencia"]),
                                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                    NombreSala = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                                    IdArea = ManejoNulos.ManageNullInteger(dr["IdArea"]),
                                    NombreArea = ManejoNulos.ManageNullStr(dr["NombreArea"]),
                                    DescripcionArea = ManejoNulos.ManageNullStr(dr["DescripcionArea"]),
                                    IdTipologia = ManejoNulos.ManageNullInteger(dr["IdTipologia"]),
                                    NombreTipologia = ManejoNulos.ManageNullStr(dr["NombreTipologia"]),
                                    DescripcionTipologia = ManejoNulos.ManageNullStr(dr["DescripcionTipologia"]),
                                    IdActuante = ManejoNulos.ManageNullInteger(dr["IdActuante"]),
                                    NombreActuante = ManejoNulos.ManageNullStr(dr["NombreActuante"]),
                                    DescripcionActuante = ManejoNulos.ManageNullStr(dr["DescripcionActuante"]),
                                    Detalle = ManejoNulos.ManageNullStr(dr["Detalle"]),
                                    IdComunicacion = ManejoNulos.ManageNullInteger(dr["IdComunicacion"]),
                                    NombreComunicacion = ManejoNulos.ManageNullStr(dr["NombreComunicacion"]),
                                    DescripcionComunicacion = ManejoNulos.ManageNullStr(dr["DescripcionComunicacion"]),
                                    AccionEjecutada = ManejoNulos.ManageNullStr(dr["AccionEjecutada"]),
                                    IdEstadoOcurrencia = ManejoNulos.ManageNullInteger(dr["IdEstadoOcurrencia"]),
                                    EstadoOcurrencia = ManejoNulos.ManageNullStr(dr["EstadoOcurrencia"]),
                                    FechaSolucion = ManejoNulos.ManageNullDate(dr["FechaSolucion"]),
                                    UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                    UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                    FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"]),
                                    Fecha = ManejoNulos.ManageNullDate(dr["Fecha"])
                                };
                                lista.Add(logArray);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return lista;
        }
         
        public int GuardarLogOcurrencia(ESS_LogOcurrenciaEntidad log) {
            int idInsertado = 0;

            string consulta = @"
                INSERT INTO [ESS_LogOcurrencia]
                (CodSala, NombreSala, IdArea, NombreArea, DescripcionArea, IdTipologia, NombreTipologia, DescripcionTipologia,
                 IdActuante, NombreActuante, DescripcionActuante, Detalle, IdComunicacion, NombreComunicacion, 
                 DescripcionComunicacion, AccionEjecutada, IdEstadoOcurrencia, FechaSolucion, UsuarioRegistro, FechaRegistro,Fecha)
                OUTPUT INSERTED.IdLogOcurrencia
                VALUES
                (@CodSala, @NombreSala, @IdArea, @NombreArea, @DescripcionArea, @IdTipologia, @NombreTipologia, @DescripcionTipologia,
                 @IdActuante, @NombreActuante, @DescripcionActuante, @Detalle, @IdComunicacion, @NombreComunicacion, 
                 @DescripcionComunicacion, @AccionEjecutada, @IdEstadoOcurrencia, @FechaSolucion, @UsuarioRegistro, @FechaRegistro,@Fecha)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(log.CodSala));
                        query.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(log.NombreSala));
                        query.Parameters.AddWithValue("@IdArea", ManejoNulos.ManageNullInteger(log.IdArea));
                        query.Parameters.AddWithValue("@NombreArea", ManejoNulos.ManageNullStr(log.NombreArea));
                        query.Parameters.AddWithValue("@DescripcionArea", ManejoNulos.ManageNullStr(log.DescripcionArea));
                        query.Parameters.AddWithValue("@IdTipologia", ManejoNulos.ManageNullInteger(log.IdTipologia));
                        query.Parameters.AddWithValue("@NombreTipologia", ManejoNulos.ManageNullStr(log.NombreTipologia));
                        query.Parameters.AddWithValue("@DescripcionTipologia", ManejoNulos.ManageNullStr(log.DescripcionTipologia));
                        query.Parameters.AddWithValue("@IdActuante", ManejoNulos.ManageNullInteger(log.IdActuante));
                        query.Parameters.AddWithValue("@NombreActuante", ManejoNulos.ManageNullStr(log.NombreActuante));
                        query.Parameters.AddWithValue("@DescripcionActuante", ManejoNulos.ManageNullStr(log.DescripcionActuante));
                        query.Parameters.AddWithValue("@Detalle", ManejoNulos.ManageNullStr(log.Detalle));
                        query.Parameters.AddWithValue("@IdComunicacion", ManejoNulos.ManageNullInteger(log.IdComunicacion));
                        query.Parameters.AddWithValue("@NombreComunicacion", ManejoNulos.ManageNullStr(log.NombreComunicacion));
                        query.Parameters.AddWithValue("@DescripcionComunicacion", ManejoNulos.ManageNullStr(log.DescripcionComunicacion));
                        query.Parameters.AddWithValue("@AccionEjecutada", ManejoNulos.ManageNullStr(log.AccionEjecutada));
                        query.Parameters.AddWithValue("@IdEstadoOcurrencia", ManejoNulos.ManageNullInteger(log.IdEstadoOcurrencia));
                        //query.Parameters.AddWithValue("@FechaSolucion", ManejoNulos.ManageNullDate(log.FechaSolucion));
                        query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(log.UsuarioRegistro));
                        query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(DateTime.Now));
                        query.Parameters.AddWithValue("@Fecha", ManejoNulos.ManageNullDate(log.Fecha));

                        if (log.FechaSolucion == DateTime.MinValue) {
                            query.Parameters.AddWithValue("@FechaSolucion", DBNull.Value);
                        } else {
                            query.Parameters.AddWithValue("@FechaSolucion", ManejoNulos.ManageNullDate(log.FechaSolucion));
                        }

                        idInsertado = Convert.ToInt32(query.ExecuteScalar());
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return idInsertado;
        }
        public bool ActualizarLogOcurrencia(ESS_LogOcurrenciaEntidad logOcurrencia) {
            bool respuesta = false;

            string consultaActualizarLogOcurrencia = @"
        UPDATE ESS_LogOcurrencia
        SET 
            CodSala = @CodSala,
            NombreSala = @NombreSala,
            IdArea = @IdArea,
            NombreArea = @NombreArea,
            DescripcionArea = @DescripcionArea,
            IdTipologia = @IdTipologia,
            NombreTipologia = @NombreTipologia,
            DescripcionTipologia = @DescripcionTipologia,
            IdActuante = @IdActuante,
            NombreActuante = @NombreActuante,
            DescripcionActuante = @DescripcionActuante,
            Detalle = @Detalle,
            IdComunicacion = @IdComunicacion,
            NombreComunicacion = @NombreComunicacion,
            DescripcionComunicacion = @DescripcionComunicacion,
            AccionEjecutada = @AccionEjecutada,
            IdEstadoOcurrencia = @IdEstadoOcurrencia,
            FechaSolucion = @FechaSolucion,
            UsuarioModificacion = @UsuarioModificacion,
            FechaModificacion = @FechaModificacion,
            Fecha = @Fecha
        WHERE IdLogOcurrencia = @IdLogOcurrencia";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();

                    using(var cmd = new SqlCommand(consultaActualizarLogOcurrencia, con)) {
                        cmd.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(logOcurrencia.CodSala));
                        cmd.Parameters.AddWithValue("@NombreSala", ManejoNulos.ManageNullStr(logOcurrencia.NombreSala));
                        cmd.Parameters.AddWithValue("@IdArea", ManejoNulos.ManageNullInteger(logOcurrencia.IdArea));
                        cmd.Parameters.AddWithValue("@NombreArea", ManejoNulos.ManageNullStr(logOcurrencia.NombreArea));
                        cmd.Parameters.AddWithValue("@DescripcionArea", ManejoNulos.ManageNullStr(logOcurrencia.DescripcionArea));
                        cmd.Parameters.AddWithValue("@IdTipologia", ManejoNulos.ManageNullInteger(logOcurrencia.IdTipologia));
                        cmd.Parameters.AddWithValue("@NombreTipologia", ManejoNulos.ManageNullStr(logOcurrencia.NombreTipologia));
                        cmd.Parameters.AddWithValue("@DescripcionTipologia", ManejoNulos.ManageNullStr(logOcurrencia.DescripcionTipologia));
                        cmd.Parameters.AddWithValue("@IdActuante", ManejoNulos.ManageNullInteger(logOcurrencia.IdActuante));
                        cmd.Parameters.AddWithValue("@NombreActuante", ManejoNulos.ManageNullStr(logOcurrencia.NombreActuante));
                        cmd.Parameters.AddWithValue("@DescripcionActuante", ManejoNulos.ManageNullStr(logOcurrencia.DescripcionActuante));
                        cmd.Parameters.AddWithValue("@Detalle", ManejoNulos.ManageNullStr(logOcurrencia.Detalle));
                        cmd.Parameters.AddWithValue("@IdComunicacion", ManejoNulos.ManageNullInteger(logOcurrencia.IdComunicacion));
                        cmd.Parameters.AddWithValue("@NombreComunicacion", ManejoNulos.ManageNullStr(logOcurrencia.NombreComunicacion));
                        cmd.Parameters.AddWithValue("@DescripcionComunicacion", ManejoNulos.ManageNullStr(logOcurrencia.DescripcionComunicacion));
                        cmd.Parameters.AddWithValue("@AccionEjecutada", ManejoNulos.ManageNullStr(logOcurrencia.AccionEjecutada));
                        cmd.Parameters.AddWithValue("@IdEstadoOcurrencia", ManejoNulos.ManageNullInteger(logOcurrencia.IdEstadoOcurrencia));
                        cmd.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(logOcurrencia.UsuarioModificacion));
                        cmd.Parameters.AddWithValue("@Fecha", ManejoNulos.ManageNullDate(logOcurrencia.Fecha));

                        cmd.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(logOcurrencia.FechaModificacion));
                        cmd.Parameters.AddWithValue("@IdLogOcurrencia", logOcurrencia.IdLogOcurrencia);
                        if(logOcurrencia.FechaSolucion == DateTime.MinValue) {
                            cmd.Parameters.AddWithValue("@FechaSolucion", DBNull.Value);
                        } else { 
                            cmd.Parameters.AddWithValue("@FechaSolucion", ManejoNulos.ManageNullDate(logOcurrencia.FechaSolucion));

                        }
                        cmd.ExecuteNonQuery();
                    }

                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
                respuesta = false;
            }

            return respuesta;
        }
        public bool EliminarLogOcurrencia(int id) {
            bool respuesta = false;
            string consulta = @"DELETE FROM ESS_LogOcurrencia 
                                WHERE IdLogOcurrencia  = @IdLogOcurrencia";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdLogOcurrencia", ManejoNulos.ManageNullInteger(id));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

        public List<ESS_AreaEntidad> ListarAreaPorEstado(int estado) {
            List<ESS_AreaEntidad> lista = new List<ESS_AreaEntidad>();
            string consulta = @"SELECT [IdArea]
                       ,[Nombre]
                       ,[Estado]
                       ,[FechaRegistro]
                       ,[FechaModificacion]
                       ,[UsuarioModificacion]
                       ,[UsuarioRegistro]
                   FROM [ESS_Area] where Estado = @Estado";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@Estado", estado);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_AreaEntidad {
                                IdArea = ManejoNulos.ManageNullInteger(dr["IdArea"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
                lista = new List<ESS_AreaEntidad>();
            }
            return lista;
        } 
        public List<ESS_TipologiaEntidad> ListarTipologiaPorEstado(int estado) {
            List<ESS_TipologiaEntidad> lista = new List<ESS_TipologiaEntidad>();
            string consulta = @"SELECT [IdTipologia]
                       ,[Nombre]
                       ,[Estado]
                       ,[FechaRegistro]
                       ,[FechaModificacion]
                       ,[UsuarioModificacion]
                       ,[UsuarioRegistro]
                   FROM [ESS_Tipologia] where Estado = @Estado";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@Estado", estado);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_TipologiaEntidad {
                                IdTipologia = ManejoNulos.ManageNullInteger(dr["IdTipologia"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
                lista = new List<ESS_TipologiaEntidad>();
            }
            return lista;
        } 
        public List<ESS_ActuanteEntidad> ListarActuantePorEstado(int estado) {
            List<ESS_ActuanteEntidad> lista = new List<ESS_ActuanteEntidad>();
            string consulta = @"SELECT [IdActuante]
                       ,[Nombre]
                       ,[Estado]
                       ,[FechaRegistro]
                       ,[FechaModificacion]
                       ,[UsuarioModificacion]
                       ,[UsuarioRegistro]
                   FROM [ESS_Actuante] where Estado = @Estado";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@Estado", estado);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_ActuanteEntidad {
                                IdActuante = ManejoNulos.ManageNullInteger(dr["IdActuante"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
                lista = new List<ESS_ActuanteEntidad>();
            }
            return lista;
        }
        public List<ESS_ComunicacionEntidad> ListarComunicacionPorEstado(int estado) {
            List<ESS_ComunicacionEntidad> lista = new List<ESS_ComunicacionEntidad>();
            string consulta = @"SELECT [IdComunicacion]
                       ,[Nombre]
                       ,[Estado]
                       ,[FechaRegistro]
                       ,[FechaModificacion]
                       ,[UsuarioModificacion]
                       ,[UsuarioRegistro]
                   FROM [ESS_Comunicacion] where Estado = @Estado";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@Estado", estado);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_ComunicacionEntidad {
                                IdComunicacion = ManejoNulos.ManageNullInteger(dr["IdComunicacion"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
                lista = new List<ESS_ComunicacionEntidad>();
            }
            return lista;
        }
        public List<ESS_EstadoOcurrenciaEntidad> ListarEstadoOcurrenciaPorEstado(int estado) {
            List<ESS_EstadoOcurrenciaEntidad> lista = new List<ESS_EstadoOcurrenciaEntidad>();
            string consulta = @"SELECT [IdEstadoOcurrencia]
                       ,[Nombre]
                       ,[Estado]
                       ,[FechaRegistro]
                       ,[FechaModificacion]
                       ,[UsuarioModificacion]
                       ,[UsuarioRegistro]
                   FROM [ESS_EstadoOcurrencia] where Estado = @Estado";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@Estado", estado);
                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_EstadoOcurrenciaEntidad {
                                IdEstadoOcurrencia = ManejoNulos.ManageNullInteger(dr["IdEstadoOcurrencia"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
                lista = new List<ESS_EstadoOcurrenciaEntidad>();
            }
            return lista;
        }

        public List<ESS_AreaEntidad> ListarArea() {
            List<ESS_AreaEntidad> lista = new List<ESS_AreaEntidad>();
            string consulta = @"SELECT [IdArea]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioRegistro]
                              ,[UsuarioModificacion]
                          FROM [ESS_Area]";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_AreaEntidad {
                                IdArea = ManejoNulos.ManageNullInteger(dr["IdArea"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
                lista = new List<ESS_AreaEntidad>();
            }
            return lista;
        }

        public int InsertarArea(ESS_AreaEntidad model) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [ESS_Area]
           ([Nombre]
           ,[Estado]
           ,[FechaRegistro]
           ,[UsuarioRegistro])
OUTPUT Inserted.IdArea
     VALUES
           (@Nombre
            ,@Estado
            ,@FechaRegistro
            ,@UsuarioRegistro)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(model.FechaRegistro));
                    query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(model.UsuarioRegistro).ToUpper().Trim());
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public bool EditarArea(ESS_AreaEntidad model) {
            bool respuesta = false;
            string consulta = @"UPDATE [ESS_Area]
                       SET [Nombre] = @Nombre
                          ,[Estado] = @Estado
                          ,[FechaModificacion] = @FechaModificacion
                          ,[UsuarioModificacion] = @UsuarioModificacion
                     WHERE IdArea = @IdArea";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(model.FechaModificacion));
                    query.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(model.UsuarioModificacion));
                    query.Parameters.AddWithValue("@IdArea", ManejoNulos.ManageNullInteger(model.IdArea));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                respuesta = false;
            }
            return respuesta;
        }


        public List<ESS_TipologiaEntidad> ListarTipologia() {
            List<ESS_TipologiaEntidad> lista = new List<ESS_TipologiaEntidad>();
            string consulta = @"SELECT [IdTipologia]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioRegistro]
                              ,[UsuarioModificacion]
                          FROM [ESS_Tipologia]";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_TipologiaEntidad {
                                IdTipologia = ManejoNulos.ManageNullInteger(dr["IdTipologia"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
                lista = new List<ESS_TipologiaEntidad>();
            }
            return lista;
        }

        public int InsertarTipologia(ESS_TipologiaEntidad model) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [ESS_Tipologia]
           ([Nombre]
           ,[Estado]
           ,[FechaRegistro]
           ,[UsuarioRegistro])
OUTPUT Inserted.IdTipologia
     VALUES
           (@Nombre
            ,@Estado
            ,@FechaRegistro
            ,@UsuarioRegistro)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(model.FechaRegistro));
                    query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(model.UsuarioRegistro).ToUpper().Trim());
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public bool EditarTipologia(ESS_TipologiaEntidad model) {
            bool respuesta = false;
            string consulta = @"UPDATE [ESS_Tipologia]
                       SET [Nombre] = @Nombre
                          ,[Estado] = @Estado
                          ,[FechaModificacion] = @FechaModificacion
                          ,[UsuarioModificacion] = @UsuarioModificacion
                     WHERE IdTipologia = @IdTipologia";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(model.FechaModificacion));
                    query.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(model.UsuarioModificacion));
                    query.Parameters.AddWithValue("@IdTipologia", ManejoNulos.ManageNullInteger(model.IdTipologia));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                respuesta = false;
            }
            return respuesta;
        }


        public List<ESS_ActuanteEntidad> ListarActuante() {
            List<ESS_ActuanteEntidad> lista = new List<ESS_ActuanteEntidad>();
            string consulta = @"SELECT [IdActuante]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioRegistro]
                              ,[UsuarioModificacion]
                          FROM [ESS_Actuante]";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_ActuanteEntidad {
                                IdActuante = ManejoNulos.ManageNullInteger(dr["IdActuante"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
                lista = new List<ESS_ActuanteEntidad>();
            }
            return lista;
        }

        public int InsertarActuante(ESS_ActuanteEntidad model) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [ESS_Actuante]
           ([Nombre]
           ,[Estado]
           ,[FechaRegistro]
           ,[UsuarioRegistro])
OUTPUT Inserted.IdActuante
     VALUES
           (@Nombre
            ,@Estado
            ,@FechaRegistro
            ,@UsuarioRegistro)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(model.FechaRegistro));
                    query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(model.UsuarioRegistro).ToUpper().Trim());
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public bool EditarActuante(ESS_ActuanteEntidad model) {
            bool respuesta = false;
            string consulta = @"UPDATE [ESS_Actuante]
                       SET [Nombre] = @Nombre
                          ,[Estado] = @Estado
                          ,[FechaModificacion] = @FechaModificacion
                          ,[UsuarioModificacion] = @UsuarioModificacion
                     WHERE IdActuante = @IdActuante";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(model.FechaModificacion));
                    query.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(model.UsuarioModificacion));
                    query.Parameters.AddWithValue("@IdActuante", ManejoNulos.ManageNullInteger(model.IdActuante));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                respuesta = false;
            }
            return respuesta;
        }

        public List<ESS_ComunicacionEntidad> ListarComunicacion() {
            List<ESS_ComunicacionEntidad> lista = new List<ESS_ComunicacionEntidad>();
            string consulta = @"SELECT [IdComunicacion]
                              ,[Nombre]
                              ,[Estado]
                              ,[FechaRegistro]
                              ,[FechaModificacion]
                              ,[UsuarioRegistro]
                              ,[UsuarioModificacion]
                          FROM [ESS_Comunicacion]";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    using(var dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            var item = new ESS_ComunicacionEntidad {
                                IdComunicacion = ManejoNulos.ManageNullInteger(dr["IdComunicacion"]),
                                Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                Estado = ManejoNulos.ManageNullInteger(dr["Estado"]),
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
                lista = new List<ESS_ComunicacionEntidad>();
            }
            return lista;
        }

        public int InsertarComunicacion(ESS_ComunicacionEntidad model) {
            int IdInsertado = 0;
            string consulta = @"INSERT INTO [ESS_Comunicacion]
           ([Nombre]
           ,[Estado]
           ,[FechaRegistro]
           ,[UsuarioRegistro])
OUTPUT Inserted.IdComunicacion
     VALUES
           (@Nombre
            ,@Estado
            ,@FechaRegistro
            ,@UsuarioRegistro)";
            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre).ToUpper().Trim());
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(model.FechaRegistro));
                    query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(model.UsuarioRegistro).ToUpper().Trim());
                    IdInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch(Exception ex) {
                IdInsertado = 0;
            }
            return IdInsertado;
        }

        public bool EditarComunicacion(ESS_ComunicacionEntidad model) {
            bool respuesta = false;
            string consulta = @"UPDATE [ESS_Comunicacion]
                       SET [Nombre] = @Nombre
                          ,[Estado] = @Estado
                          ,[FechaModificacion] = @FechaModificacion
                          ,[UsuarioModificacion] = @UsuarioModificacion
                     WHERE IdComunicacion = @IdComunicacion";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Nombre", ManejoNulos.ManageNullStr(model.Nombre));
                    query.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(model.Estado));
                    query.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(model.FechaModificacion));
                    query.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(model.UsuarioModificacion));
                    query.Parameters.AddWithValue("@IdComunicacion", ManejoNulos.ManageNullInteger(model.IdComunicacion));
                    query.ExecuteNonQuery();
                    respuesta = true;
                }
            } catch(Exception ex) {
                respuesta = false;
            }
            return respuesta;
        }


    }
}
