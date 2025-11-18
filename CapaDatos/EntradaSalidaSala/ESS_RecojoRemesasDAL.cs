using CapaEntidad.EntradaSalidaSala;
using Microsoft.Win32;
using S3k.Utilitario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.EntradaSalidaSala {
    public class ESS_RecojoRemesasDAL {
        string _conexion = string.Empty;
        public ESS_RecojoRemesasDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ESS_RecojoRemesaEntidad> ListadoRecojoRemesa(int[] codSala, DateTime fechaInicio, DateTime fechaFin) {
            List<ESS_RecojoRemesaEntidad> lista = new List<ESS_RecojoRemesaEntidad>();
            string strSala = codSala.Length > 0 ? $" A.CodSala IN ({string.Join(",", codSala)}) AND " : "";
            string consulta = $@"
            SELECT 
                A.IdRecojoRemesa,
                A.CodSala,        
                B.Nombre as Sala,
                A.IdPersonal, 
                D.CodigoPersonal, 
		            CONCAT(D.Nombres,' ',D.ApellidoPaterno,' ',D.ApellidoMaterno) as NombreCompletoPersonal,
		            E.Nombre as TipoDocumentoRegistro,
		            D.DocumentoRegistro,
                A.IdEstadoFotocheck,
                C.Nombre as EstadoFotocheck,
                A.OtroEstadoFotocheck,
                A.PlacaRodaje, 
                A.FechaIngreso, 
                A.FechaSalida, 
                A.Observaciones,
                A.UsuarioRegistro,
                A.FechaRegistro, 
                A.UsuarioModificacion, 
                A.FechaModificacion
            FROM ESS_RecojoRemesa A
            JOIN Sala B ON A.CodSala = B.CodSala
            LEFT JOIN ESS_EstadoFotocheck C ON A.IdEstadoFotocheck = C.IdEstadoFotocheck
            INNER JOIN ESS_RecojoRemesaPersonal D on A.IdPersonal = D.IdRecojoRemesaPersonal
            LEFT JOIN AST_TipoDocumento E on D.IdTipoDocumentoRegistro = E.Id
            WHERE {strSala} CONVERT(date, A.FechaIngreso) BETWEEN CONVERT(date, @fechaInicio) AND CONVERT(date, @fechaFin)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                        query.Parameters.AddWithValue("@fechaFin", fechaFin);

                        using(var dr = query.ExecuteReader()) {
                            while(dr.Read()) {
                                var remesa = new ESS_RecojoRemesaEntidad {
                                    IdRecojoRemesa = ManejoNulos.ManageNullInteger(dr["IdRecojoRemesa"]),
                                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                                    Sala = ManejoNulos.ManageNullStr(dr["Sala"]),
                                    IdPersonal = ManejoNulos.ManageNullInteger(dr["IdPersonal"]),
                                    NombreCompletoPersonal = ManejoNulos.ManageNullStr(dr["NombreCompletoPersonal"]),
                                    TipoDocumentoRegistro = ManejoNulos.ManageNullStr(dr["TipoDocumentoRegistro"]),
                                    DocumentoRegistro = ManejoNulos.ManageNullStr(dr["DocumentoRegistro"]),
                                    CodigoPersonal = ManejoNulos.ManageNullStr(dr["CodigoPersonal"]),
                                    IdEstadoFotocheck = ManejoNulos.ManageNullInteger(dr["IdEstadoFotocheck"]),
                                    OtroEstadoFotocheck = ManejoNulos.ManageNullStr(dr["OtroEstadoFotocheck"]),
                                    EstadoFotocheck = ManejoNulos.ManageNullStr(dr["EstadoFotocheck"]),
                                    PlacaRodaje = ManejoNulos.ManageNullStr(dr["PlacaRodaje"]),
                                    FechaIngreso = ManejoNulos.ManageNullDate(dr["FechaIngreso"]),
                                    FechaSalida = ManejoNulos.ManageNullDate(dr["FechaSalida"]),
                                    Observaciones = ManejoNulos.ManageNullStr(dr["Observaciones"]),
                                    UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                    UsuarioModificacion = ManejoNulos.ManageNullStr(dr["UsuarioModificacion"]),
                                    FechaModificacion = ManejoNulos.ManageNullDate(dr["FechaModificacion"])
                                };
                                lista.Add(remesa);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return lista;
        }
        public int GuardarRegistroRecojoRemesa(ESS_RecojoRemesaEntidad remesa) {
            int idInsertado = 0;

            string consulta = @"
            INSERT INTO ESS_RecojoRemesa
            (CodSala, IdPersonal, IdEstadoFotocheck,OtroEstadoFotocheck, PlacaRodaje, FechaIngreso, FechaSalida, Observaciones, UsuarioRegistro, FechaRegistro)
            OUTPUT INSERTED.IdRecojoRemesa
            VALUES
            (@CodSala, @IdPersonal, @IdEstadoFotocheck, @OtroEstadoFotocheck, @PlacaRodaje, @FechaIngreso, @FechaSalida, @Observaciones, @UsuarioRegistro, @FechaRegistro)";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var query = new SqlCommand(consulta, con)) {
                        query.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(remesa.CodSala));
                        query.Parameters.AddWithValue("@IdPersonal", ManejoNulos.ManageNullInteger(remesa.IdPersonal));
                        query.Parameters.AddWithValue("@IdEstadoFotocheck", ManejoNulos.ManageNullInteger(remesa.IdEstadoFotocheck));
                        query.Parameters.AddWithValue("@OtroEstadoFotocheck", ManejoNulos.ManageNullStr(remesa.OtroEstadoFotocheck));
                        query.Parameters.AddWithValue("@PlacaRodaje", ManejoNulos.ManageNullStr(remesa.PlacaRodaje));
                        query.Parameters.AddWithValue("@FechaIngreso", ManejoNulos.ManageNullDate(remesa.FechaIngreso));
                        //query.Parameters.AddWithValue("@FechaSalida", ManejoNulos.ManageNullDate(remesa.FechaSalida));
                        query.Parameters.AddWithValue("@Observaciones", ManejoNulos.ManageNullStr(remesa.Observaciones));
                        query.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(remesa.UsuarioRegistro));
                        query.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(remesa.FechaRegistro));
                        if(remesa.FechaSalida == DateTime.MinValue) {
                            query.Parameters.AddWithValue("@FechaSalida", DBNull.Value);
                        } else {
                            query.Parameters.AddWithValue("@FechaSalida", ManejoNulos.ManageNullDate(remesa.FechaSalida));
                        }
                        idInsertado = Convert.ToInt32(query.ExecuteScalar());
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return idInsertado;
        }
        public bool ActualizarRegistroRecojoRemesa(ESS_RecojoRemesaEntidad remesa) {
            bool respuesta = false;

            string consulta = @"
            UPDATE ESS_RecojoRemesa
            SET 
                CodSala = @CodSala,
                IdPersonal = @IdPersonal,
                IdEstadoFotocheck = @IdEstadoFotocheck,
                OtroEstadoFotocheck = @OtroEstadoFotocheck,
                PlacaRodaje = @PlacaRodaje,
                FechaIngreso = @FechaIngreso,
                FechaSalida = @FechaSalida,
                Observaciones = @Observaciones,
                UsuarioModificacion = @UsuarioModificacion,
                FechaModificacion = @FechaModificacion
            WHERE IdRecojoRemesa = @IdRecojoRemesa";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@CodSala", ManejoNulos.ManageNullInteger(remesa.CodSala));
                        cmd.Parameters.AddWithValue("@IdPersonal", ManejoNulos.ManageNullInteger(remesa.IdPersonal));
                        cmd.Parameters.AddWithValue("@IdEstadoFotocheck", ManejoNulos.ManageNullInteger(remesa.IdEstadoFotocheck));
                        cmd.Parameters.AddWithValue("@OtroEstadoFotocheck", ManejoNulos.ManageNullStr(remesa.OtroEstadoFotocheck));
                        cmd.Parameters.AddWithValue("@PlacaRodaje", ManejoNulos.ManageNullStr(remesa.PlacaRodaje));
                        cmd.Parameters.AddWithValue("@FechaIngreso", ManejoNulos.ManageNullDate(remesa.FechaIngreso));
                        //cmd.Parameters.AddWithValue("@FechaSalida", ManejoNulos.ManageNullDate(remesa.FechaSalida));
                        cmd.Parameters.AddWithValue("@Observaciones", ManejoNulos.ManageNullStr(remesa.Observaciones));
                        cmd.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(remesa.UsuarioModificacion));
                        cmd.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(remesa.FechaModificacion));
                        if(remesa.FechaSalida == DateTime.MinValue) {
                            cmd.Parameters.AddWithValue("@FechaSalida", DBNull.Value);
                        } else {
                            cmd.Parameters.AddWithValue("@FechaSalida", ManejoNulos.ManageNullDate(remesa.FechaSalida));
                        }
                        cmd.Parameters.AddWithValue("@IdRecojoRemesa", remesa.IdRecojoRemesa);

                        respuesta = cmd.ExecuteNonQuery() > 0;
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return respuesta;
        }
        public bool EliminarRegistroRecojoRemesa(int idRecojoRemesa) {
            bool respuesta = false;

            string consulta = @"
            DELETE FROM ESS_RecojoRemesa
            WHERE IdRecojoRemesa = @IdRecojoRemesa";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var cmd = new SqlCommand(consulta, con)) {
                        cmd.Parameters.AddWithValue("@IdRecojoRemesa", idRecojoRemesa);

                        respuesta = cmd.ExecuteNonQuery() > 0;
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return respuesta;
        }
        public List<ESS_EstadoFotocheckEntidad> ListadoEstadoFotocheck() {
            List<ESS_EstadoFotocheckEntidad> lista = new List<ESS_EstadoFotocheckEntidad>();
            string consulta = $@"
            SELECT 
                A.IdEstadoFotocheck,
                A.Nombre,
                A.FechaRegistro
            FROM ESS_EstadoFotocheck A
            
            WHERE A.Estado = 1";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var query = new SqlCommand(consulta, con)) {

                        using(var dr = query.ExecuteReader()) {
                            while(dr.Read()) {
                                var estadofotocheck = new ESS_EstadoFotocheckEntidad {
                                    IdEstadoFotocheck = ManejoNulos.ManageNullInteger(dr["IdEstadoFotocheck"]),
                                    Nombre = ManejoNulos.ManageNullStr(dr["Nombre"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                };
                                lista.Add(estadofotocheck);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return lista;
        }

        //CRUD para ESS_RecojoRemesaPersonal
        public List<ESS_RecojoRemesaPersonalEntidad> ListadoRecojoRemesaPersonal() {
            List<ESS_RecojoRemesaPersonalEntidad> lista = new List<ESS_RecojoRemesaPersonalEntidad>();
            string consulta = $@"
            SELECT 
               A.IdRecojoRemesaPersonal
              ,A.Nombres
              ,A.ApellidoPaterno
              ,A.ApellidoMaterno
              ,A.DocumentoRegistro
              ,A.IdTipoDocumentoRegistro
              ,B.Nombre as TipoDocumentoRegistro
              ,A.CodigoPersonal
              ,A.UsuarioRegistro
              ,A.FechaRegistro
              ,A.UsuarioModificacion
              ,A.FechaModificacion
              ,A.Estado
            FROM ESS_RecojoRemesaPersonal A
            JOIN AST_TipoDocumento B on A.IdTipoDocumentoRegistro = B.Id
            WHERE A.Estado = 1";

            try {
                using(var con = new SqlConnection(_conexion)) {
                    con.Open();
                    using(var query = new SqlCommand(consulta, con)) {

                        using(var dr = query.ExecuteReader()) {
                            while(dr.Read()) {
                                var personal = new ESS_RecojoRemesaPersonalEntidad {
                                    IdRecojoRemesaPersonal = ManejoNulos.ManageNullInteger(dr["IdRecojoRemesaPersonal"]),
                                    Nombres = ManejoNulos.ManageNullStr(dr["Nombres"]),
                                    ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaterno"]),
                                    ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaterno"]),
                                    DocumentoRegistro = ManejoNulos.ManageNullStr(dr["DocumentoRegistro"]),
                                    IdTipoDocumentoRegistro = ManejoNulos.ManageNullInteger(dr["IdTipoDocumentoRegistro"]),
                                    TipoDocumentoRegistro = ManejoNulos.ManageNullStr(dr["TipoDocumentoRegistro"]),
                                    CodigoPersonal = ManejoNulos.ManageNullStr(dr["CodigoPersonal"]),
                                    UsuarioRegistro = ManejoNulos.ManageNullStr(dr["UsuarioRegistro"]),
                                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
                                };
                                lista.Add(personal);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return lista;
        }


        public int GuardarRegistroRecojoRemesaPersonal(ESS_RecojoRemesaPersonalEntidad entidad) {
            using(SqlConnection connection = new SqlConnection(_conexion)) {
                string query = @"
                    INSERT INTO ESS_RecojoRemesaPersonal 
                    (Nombres, ApellidoPaterno, ApellidoMaterno, DocumentoRegistro, 
                     IdTipoDocumentoRegistro, CodigoPersonal, Estado, UsuarioRegistro, FechaRegistro) 
                    VALUES (@Nombres, @ApellidoPaterno, @ApellidoMaterno, @DocumentoRegistro, 
                            @IdTipoDocumentoRegistro, @CodigoPersonal, @Estado, @UsuarioRegistro, @FechaRegistro);
                    SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombres", ManejoNulos.ManageNullStr(entidad.Nombres));
                command.Parameters.AddWithValue("@ApellidoPaterno", ManejoNulos.ManageNullStr(entidad.ApellidoPaterno));
                command.Parameters.AddWithValue("@ApellidoMaterno", ManejoNulos.ManageNullStr(entidad.ApellidoMaterno));
                command.Parameters.AddWithValue("@DocumentoRegistro", ManejoNulos.ManageNullStr(entidad.DocumentoRegistro));
                command.Parameters.AddWithValue("@IdTipoDocumentoRegistro", ManejoNulos.ManageNullInteger(entidad.IdTipoDocumentoRegistro));
                command.Parameters.AddWithValue("@CodigoPersonal", ManejoNulos.ManageNullStr(entidad.CodigoPersonal));
                command.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(entidad.Estado));
                command.Parameters.AddWithValue("@UsuarioRegistro", ManejoNulos.ManageNullStr(entidad.UsuarioRegistro));
                command.Parameters.AddWithValue("@FechaRegistro", ManejoNulos.ManageNullDate(entidad.FechaRegistro));

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public bool ActualizarRegistroRecojoRemesaPersonal(ESS_RecojoRemesaPersonalEntidad entidad) {
            using(SqlConnection connection = new SqlConnection(_conexion)) {
                string query = @"
                    UPDATE ESS_RecojoRemesaPersonal
                    SET Nombres = @Nombres,
                        ApellidoPaterno = @ApellidoPaterno,
                        ApellidoMaterno = @ApellidoMaterno,
                        DocumentoRegistro = @DocumentoRegistro,
                        IdTipoDocumentoRegistro = @IdTipoDocumentoRegistro,
                        CodigoPersonal = @CodigoPersonal,
                        Estado = @Estado,
                        UsuarioModificacion = @UsuarioModificacion,
                        FechaModificacion = @FechaModificacion
                    WHERE IdRecojoRemesaPersonal = @IdRecojoRemesaPersonal";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombres", ManejoNulos.ManageNullStr(entidad.Nombres));
                command.Parameters.AddWithValue("@ApellidoPaterno", ManejoNulos.ManageNullStr(entidad.ApellidoPaterno));
                command.Parameters.AddWithValue("@ApellidoMaterno", ManejoNulos.ManageNullStr(entidad.ApellidoMaterno));
                command.Parameters.AddWithValue("@DocumentoRegistro", ManejoNulos.ManageNullStr(entidad.DocumentoRegistro));
                command.Parameters.AddWithValue("@IdTipoDocumentoRegistro", ManejoNulos.ManageNullInteger(entidad.IdTipoDocumentoRegistro));
                command.Parameters.AddWithValue("@CodigoPersonal", ManejoNulos.ManageNullStr(entidad.CodigoPersonal));
                command.Parameters.AddWithValue("@Estado", ManejoNulos.ManageNullInteger(entidad.Estado));
                command.Parameters.AddWithValue("@UsuarioModificacion", ManejoNulos.ManageNullStr(entidad.UsuarioModificacion));
                command.Parameters.AddWithValue("@FechaModificacion", ManejoNulos.ManageNullDate(entidad.FechaModificacion));
                command.Parameters.AddWithValue("@IdRecojoRemesaPersonal", entidad.IdRecojoRemesaPersonal);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool EliminarRegistroRecojoRemesaPersonal(int id) {
            using(SqlConnection connection = new SqlConnection(_conexion)) {
                string query = "DELETE FROM ESS_RecojoRemesaPersonal WHERE IdRecojoRemesaPersonal = @IdRecojoRemesaPersonal";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdRecojoRemesaPersonal", id);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        public int ExisteRegistroPersonal(int IdTipoDocumentoRegistro, string DocumentoRegistro) {
            int idRecojoRemesaPersonal = 0;
            string query = @"
                SELECT TOP 1 IdRecojoRemesaPersonal
                FROM ESS_RecojoRemesaPersonal
                WHERE IdTipoDocumentoRegistro = @IdTipoDocumentoRegistro
                AND DocumentoRegistro = @DocumentoRegistro";

            try {
                using(var connection = new SqlConnection(_conexion)) {
                    connection.Open();
                    using(var command = new SqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@IdTipoDocumentoRegistro", ManejoNulos.ManageNullInteger(IdTipoDocumentoRegistro));
                        command.Parameters.AddWithValue("@DocumentoRegistro", ManejoNulos.ManageNullStr(DocumentoRegistro));
                        idRecojoRemesaPersonal = ManejoNulos.ManageNullInteger(command.ExecuteScalar());
                    }
                }
            } catch(Exception ex) {
                Trace.WriteLine(ex.Message + " " + GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return idRecojoRemesaPersonal;
        }
    }
}
