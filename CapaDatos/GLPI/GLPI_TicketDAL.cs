using CapaEntidad.GLPI;
using CapaEntidad.GLPI.DTO;
using CapaEntidad.GLPI.DTO.Global;
using CapaEntidad.GLPI.DTO.Mantenedores;
using CapaEntidad.GLPI.Enum;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.GLPI {
    public class GLPI_TicketDAL {
        private readonly string _conexion;
        private readonly string _baseQuerySelect;

        public GLPI_TicketDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
            _baseQuerySelect = @"
                SELECT
                    --Ticket
                    t.Id,
                    --Sala
                    t.CodSala,
                    s.Nombre AS NombreSala,
                    --Solicitante
                    t.IdUsuarioSolicitante,
                    empsoli.Nombres AS NombreUsuarioSolicitante,
                    empsoli.ApellidosPaterno AS ApellidoPaternoUsuarioSolicitante,
                    empsoli.ApellidosMaterno AS ApellidoMaternoUsuarioSolicitante,
                    carsoli.Descripcion AS CargoUsuarioSolicitante,
                    empsoli.DOI AS NumeroDocumentoUsuarioSolicitante,
                    empsoli.MailPersonal AS CorreoPersonalUsuarioSolicitante,
                    empsoli.MailJob AS CorreoTrabajoUsuarioSolicitante,
                    --TipoOperacion
                    t.IdTipoOperacion,
                    tiop.Nombre AS NombreTipoOperacion,
                    --NivelAtencion
                    t.IdNivelAtencion,
                    niat.Nombre AS NombreNivelAtencion,
                    niat.Color AS ColorNivelAtencion,
                    --Subcategoria
                    cat.IdPartida,
                    par.Codigo AS CodigoPartida,
                    par.Nombre AS NombrePartida,
                    par.TipoGasto AS TipoGastoPartida,
                    suca.IdCategoria,
                    cat.Nombre AS NombreCategoria,
                    t.IdSubCategoria,
                    suca.Nombre AS NombreSubCategoria,
                    --ClasificacionProblema
                    t.IdClasificacionProblema,
                    clapro.Nombre AS NombreClasificacionProblema,
                    --EstadoActual
                    t.IdEstadoActual,
                    esac.Nombre AS NombreEstadoActual,
                    --Identificador
                    t.IdIdentificador,
                    ide.Nombre AS NombreIdentificador,
                    --Asignacion
                    asti.Id AS IdAsignacion,
                    --Asignacion.UsuarioAsigna
                    asti.IdUsuarioAsigna,
                    empasi1.Nombres AS NombreUsuarioAsigna,
                    empasi1.ApellidosPaterno AS ApellidoPaternoUsuarioAsigna,
                    empasi1.ApellidosMaterno AS ApellidoMaternoUsuarioAsigna,
                    carasi1.Descripcion AS CargoUsuarioAsigna,
                    empasi1.DOI AS NumeroDocumentoUsuarioAsigna,
	                empasi1.MailPersonal AS CorreoPersonalUsuarioAsigna,
                    empasi1.MailJob AS CorreoTrabajoUsuarioAsigna,
                    --Asignacion.EstadoTicket
                    asti.IdEstadoTicket AS IdEstadoTicketAsignacion,
                    esti.Nombre AS NombreEstadoTicketAsignacion,
                    --Asignacion.UsuarioAsignado
                    asti.IdUsuarioAsignado,
                    empasi2.Nombres AS NombreUsuarioAsignado,
                    empasi2.ApellidosPaterno AS ApellidoPaternoUsuarioAsignado,
                    empasi2.ApellidosMaterno AS ApellidoMaternoUsuarioAsignado,
                    carasi2.Descripcion AS CargoUsuarioAsignado,
                    empasi2.DOI AS NumeroDocumentoUsuarioAsignado,
	                empasi2.MailPersonal AS CorreoPersonalUsuarioAsignado,
                    empasi2.MailJob AS CorreoTrabajoUsuarioAsignado,
                    --Asignacion.FechaTentativaTermino
                    asti.FechaTentativaTermino AS FechaTentativaTerminoAsignacion,
                    --Asignacion.Coreos
                    asti.Correos AS CorreosAsignacion,
                    --Asignacion.FechaRegistro
                    asti.FechaRegistro AS FechaRegistroAsignacion,
                    --Cierre
                    citi.Id AS IdCierre,
                    --Cierre.UsuarioCierra
                    citi.IdUsuarioCierra,
                    empasi3.Nombres AS NombreUsuarioCierra,
                    empasi3.ApellidosPaterno AS ApellidoPaternoUsuarioCierra,
                    empasi3.ApellidosMaterno AS ApellidoMaternoUsuarioCierra,
                    carasi3.Descripcion AS CargoUsuarioCierra,
                    empasi3.DOI AS NumeroDocumentoUsuarioCierra,
	                empasi3.MailPersonal AS CorreoPersonalUsuarioCierra,
                    empasi3.MailJob AS CorreoTrabajoUsuarioCierra,
                    --Cierre.EstadoAnterior
                    citi.IdEstadoTicketAnterior AS IdEstadoAnteriorCierre,
                    estian.Nombre AS NombreEstadoAnteriorCierre,
                    --Cierre.EstadoActual
                    citi.IdEstadoTicketActual AS IsEstadoActualCierre,
                    estiac.Nombre AS NombreEstadoActualCierre,
                    --Cierre.Descripcion
                    citi.Descripcion AS DescripcionCierre,
                    --Cierre.UsuarioConfirmaCierre
                    citi.IdUsuarioConfirma,
                    empasi4.Nombres AS NombreUsuarioConfirma,
                    empasi4.ApellidosPaterno AS ApellidoPaternoUsuarioConfirma,
                    empasi4.ApellidosMaterno AS ApellidoMaternoUsuarioConfirma,
                    carasi4.Descripcion AS CargoUsuarioConfirma,
                    empasi4.DOI AS NumeroDocumentoUsuarioConfirma,
	                empasi4.MailPersonal AS CorreoPersonalUsuarioConfirma,
                    empasi4.MailJob AS CorreoTrabajoUsuarioConfirma,
                    --Cierre.FechaRegistro
                    citi.FechaRegistro AS FechaRegistroCierre,
                    --Ticket
                    t.Descripcion AS DescripcionTicket,
                    t.Adjunto AS AdjuntoTicket,
                    t.Correos AS CorreosTicket,
                    t.CodigoFaseTicket,
                    t.FechaRegistro AS FechaRegistroTicket
                FROM GLPI_Ticket AS t
                    LEFT JOIN GLPI_AsignacionTicket AS asti ON asti.IdTicket = t.Id
                    INNER JOIN Sala AS s ON s.CodSala = t.CodSala
                    LEFT JOIN SEG_Usuario AS ususoli ON ususoli.UsuarioID = t.IdUsuarioSolicitante
                    LEFT JOIN SEG_Empleado AS empsoli ON empsoli.EmpleadoID = ususoli.EmpleadoID
                    LEFT JOIN SEG_Cargo AS carsoli ON carsoli.CargoID = empsoli.CargoID
                    LEFT JOIN SEG_Usuario AS usuasi1 ON usuasi1.UsuarioID = asti.IdUsuarioAsigna
                    LEFT JOIN SEG_Empleado AS empasi1 ON empasi1.EmpleadoID = usuasi1.EmpleadoID
                    LEFT JOIN SEG_Cargo AS carasi1 ON carasi1.CargoID = empasi1.CargoID
                    LEFT JOIN SEG_Usuario AS usuasi2 ON usuasi2.UsuarioID = asti.IdUsuarioAsignado
                    LEFT JOIN SEG_Empleado AS empasi2 ON empasi2.EmpleadoID = usuasi2.EmpleadoID
                    LEFT JOIN SEG_Cargo AS carasi2 ON carasi2.CargoID = empasi2.CargoID
                    LEFT JOIN GLPI_CierreTicket AS citi ON citi.IdTicket = t.Id
                    LEFT JOIN SEG_Usuario AS usuasi3 ON usuasi3.UsuarioID = citi.IdUsuarioCierra
                    LEFT JOIN SEG_Empleado AS empasi3 ON empasi3.EmpleadoID = usuasi3.EmpleadoID
                    LEFT JOIN SEG_Cargo AS carasi3 ON carasi3.CargoID = empasi3.CargoID
                    LEFT JOIN SEG_Usuario AS usuasi4 ON usuasi4.UsuarioID = citi.IdUsuarioConfirma
                    LEFT JOIN SEG_Empleado AS empasi4 ON empasi4.EmpleadoID = usuasi4.EmpleadoID
                    LEFT JOIN SEG_Cargo AS carasi4 ON carasi4.CargoID = empasi4.CargoID
                    LEFT JOIN GLPI_TipoOperacion AS tiop ON tiop.Id = t.IdTipoOperacion
                    LEFT JOIN GLPI_NivelAtencion AS niat ON niat.Id = t.IdNivelAtencion
                    LEFT JOIN GLPI_SubCategoria AS suca ON suca.Id = t.IdSubCategoria
                    LEFT JOIN GLPI_Categoria AS cat ON cat.Id = suca.IdCategoria
                    LEFT JOIN GLPI_Partida AS par ON par.Id = cat.IdPartida
                    LEFT JOIN GLPI_ClasificacionProblema AS clapro ON clapro.Id = t.IdClasificacionProblema
                    LEFT JOIN GLPI_EstadoActual AS esac ON esac.Id = t.IdEstadoActual
                    LEFT JOIN GLPI_Identificador AS ide ON ide.Id = t.IdIdentificador
                    LEFT JOIN GLPI_EstadoTicket AS esti ON esti.Id = asti.IdEstadoTicket
                    LEFT JOIN GLPI_EstadoTicket AS estian ON estian.Id = citi.IdEstadoTicketAnterior
                    LEFT JOIN GLPI_EstadoTicket AS estiac ON estiac.Id = citi.IdEstadoTicketActual
            ";
        }

        public int ActualizarTicket(GLPI_Ticket ticket) {
            int idActualizado;
            string consulta = @"
                UPDATE GLPI_Ticket
                SET 
                    IdTipoOperacion = @IdTipoOperacion,
                    IdNivelAtencion = @IdNivelAtencion,
                    IdSubCategoria = @IdSubCategoria,
                    IdClasificacionProblema = @IdClasificacionProblema,
                    IdEstadoActual = @IdEstadoActual,
                    IdIdentificador = @IdIdentificador,
                    Descripcion = @Descripcion,
                    Adjunto = @Adjunto,
                    Correos = @Correos,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", ticket.Id);
                    query.Parameters.AddWithValue("@IdTipoOperacion", ticket.IdTipoOperacion);
                    query.Parameters.AddWithValue("@IdNivelAtencion", ticket.IdNivelAtencion);
                    query.Parameters.AddWithValue("@IdSubCategoria", ticket.IdSubCategoria);
                    query.Parameters.AddWithValue("@IdClasificacionProblema", ticket.IdClasificacionProblema);
                    query.Parameters.AddWithValue("@IdEstadoActual", ticket.IdEstadoActual);
                    query.Parameters.AddWithValue("@IdIdentificador", ticket.IdIdentificador);
                    query.Parameters.AddWithValue("@Descripcion", ManejoNulos.ManageNullStr(ticket.Descripcion));
                    query.Parameters.AddWithValue("@Adjunto", ticket.Adjunto);
                    query.Parameters.AddWithValue("@Correos", ticket.Correos);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int ActualizarFaseTicketPorIdTicket(int idTicket, GLPI_FaseTicket faseTicket) {
            int idActualizado;
            string consulta = @"
                UPDATE GLPI_Ticket
                SET 
                    CodigoFaseTicket = @CodigoFaseTicket,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", idTicket);
                    query.Parameters.AddWithValue("@CodigoFaseTicket", faseTicket);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarTicket(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM GLPI_Ticket
                OUTPUT DELETED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    idEliminado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idEliminado = 0;
            }
            return idEliminado;
        }

        public int InsertarTicket(GLPI_Ticket ticket) {
            int idInsertado;
            string consulta = @"
                INSERT INTO GLPI_Ticket(CodSala, IdUsuarioSolicitante, IdTipoOperacion, IdNivelAtencion, IdSubCategoria, IdClasificacionProblema, IdEstadoActual, IdIdentificador, Descripcion, Adjunto, Correos, CodigoFaseTicket)
                OUTPUT INSERTED.Id
                VALUES (@CodSala, @IdUsuarioSolicitante, @IdTipoOperacion, @IdNivelAtencion, @IdSubCategoria, @IdClasificacionProblema, @IdEstadoActual, @IdIdentificador, @Descripcion, @Adjunto, @Correos, @CodigoFaseTicket)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodSala", ticket.CodSala);
                    query.Parameters.AddWithValue("@IdUsuarioSolicitante", ticket.IdUsuarioSolicitante);
                    query.Parameters.AddWithValue("@IdTipoOperacion", ticket.IdTipoOperacion);
                    query.Parameters.AddWithValue("@IdNivelAtencion", ticket.IdNivelAtencion);
                    query.Parameters.AddWithValue("@IdSubCategoria", ticket.IdSubCategoria);
                    query.Parameters.AddWithValue("@IdClasificacionProblema", ticket.IdClasificacionProblema);
                    query.Parameters.AddWithValue("@IdEstadoActual", ticket.IdEstadoActual);
                    query.Parameters.AddWithValue("@IdIdentificador", ticket.IdIdentificador);
                    query.Parameters.AddWithValue("@Descripcion", ManejoNulos.ManageNullStr(ticket.Descripcion));
                    query.Parameters.AddWithValue("@Adjunto", ticket.Adjunto);
                    query.Parameters.AddWithValue("@Correos", ticket.Correos);
                    query.Parameters.AddWithValue("@CodigoFaseTicket", GLPI_FaseTicket.Creado);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public GLPI_TicketDto ObtenerTicketPorId(int id) {
            GLPI_TicketDto item = new GLPI_TicketDto();
            string consulta = $@"
                {_baseQuerySelect}
                WHERE t.Id = @Id
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", id);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = ConstruirObjeto(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<GLPI_TicketDto> ObtenerTicketsReportes(DateTime fechaInicio, DateTime fechaFin, string codsSalas, string codsFases) {
            List<GLPI_TicketDto> items = new List<GLPI_TicketDto>();
            string consulta = $@"
                {_baseQuerySelect}
                WHERE t.CodSala IN ({codsSalas}) AND t.CodigoFaseTicket IN ({codsFases}) AND (convert(date, t.FechaRegistro) BETWEEN convert(date, @FechaInicio) AND  convert(date, @FechaFin))
                ORDER BY t.FechaRegistro DESC
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
                    query.Parameters.AddWithValue("@FechaFin", fechaFin.Date);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<GLPI_TicketDto> ObtenerTicketsPorIdUsuarioSolicitante(int idUsuario) {
            List<GLPI_TicketDto> items = new List<GLPI_TicketDto>();
            string consulta = $@"
                {_baseQuerySelect}
                WHERE t.IdUsuarioSolicitante = @IdUsuarioSolicitante AND t.CodigoFaseTicket <> @CodigoFaseTicket
                ORDER BY t.FechaRegistro DESC
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdUsuarioSolicitante", idUsuario);
                    query.Parameters.AddWithValue("@CodigoFaseTicket", GLPI_FaseTicket.Cerrado);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<GLPI_TicketDto> ObtenerTicketsPorIdUsuarioAsigna(int idUsuario) {
            List<GLPI_TicketDto> items = new List<GLPI_TicketDto>();
            string consulta = $@"
                {_baseQuerySelect}
                WHERE asti.IdUsuarioAsigna = @IdUsuarioAsigna AND t.CodigoFaseTicket <> @CodigoFaseTicket
                ORDER BY t.FechaRegistro DESC
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdUsuarioAsigna", idUsuario);
                    query.Parameters.AddWithValue("@CodigoFaseTicket", GLPI_FaseTicket.Cerrado);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<GLPI_TicketDto> ObtenerTicketsPorIdUsuarioAsignado(int idUsuario) {
            List<GLPI_TicketDto> items = new List<GLPI_TicketDto>();
            string consulta = $@"
                {_baseQuerySelect}
                WHERE asti.IdUsuarioAsignado = @IdUsuarioAsignado AND t.CodigoFaseTicket <> @CodigoFaseTicket
                ORDER BY t.FechaRegistro DESC
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdUsuarioAsignado", idUsuario);
                    query.Parameters.AddWithValue("@CodigoFaseTicket", GLPI_FaseTicket.Cerrado);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<GLPI_TicketDto> ObtenerTicketsPorCodsSalaYFase(string codsSala, GLPI_FaseTicket faseTicket) {
            List<GLPI_TicketDto> items = new List<GLPI_TicketDto>();
            string consulta = $@"
                {_baseQuerySelect}
                WHERE t.CodSala IN ({codsSala}) AND t.CodigoFaseTicket = @CodigoFaseTicket
                ORDER BY t.FechaRegistro DESC
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@CodigoFaseTicket", faseTicket);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public bool TicketEstaEnFase(int idTicket, GLPI_FaseTicket faseTicket) {
            int cantidadRegistros = 0;
            string consulta = @"
                SELECT COUNT(Id)
                FROM GLPI_Ticket
                WHERE Id = @Id AND CodigoFaseTicket = @CodigoFaseTicket
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", idTicket);
                    query.Parameters.AddWithValue("@CodigoFaseTicket", faseTicket);
                    cantidadRegistros = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch { }

            return cantidadRegistros >= 1;
        }

        private GLPI_TicketDto ConstruirObjeto(SqlDataReader dr) {
            return new GLPI_TicketDto {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Sala = new GLPI_SalaDto {
                    CodSala = ManejoNulos.ManageNullInteger(dr["CodSala"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreSala"]),
                },
                UsuarioSolicitante = new GLPI_UsuarioDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdUsuarioSolicitante"]),
                    Nombres = ManejoNulos.ManageNullStr(dr["NombreUsuarioSolicitante"]),
                    ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaternoUsuarioSolicitante"]),
                    ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaternoUsuarioSolicitante"]),
                    Cargo = ManejoNulos.ManageNullStr(dr["CargoUsuarioSolicitante"]),
                    NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumentoUsuarioSolicitante"]),
                    CorreoPersonal = ManejoNulos.ManageNullStr(dr["CorreoPersonalUsuarioSolicitante"]),
                    CorreoTrabajo = ManejoNulos.ManageNullStr(dr["CorreoTrabajoUsuarioSolicitante"]),
                },
                TipoOperacion = new GLPI_TipoOperacionDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdTipoOperacion"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreTipoOperacion"]),
                },
                NivelAtencion = new GLPI_NivelAtencionDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdNivelAtencion"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreNivelAtencion"]),
                    Color = ManejoNulos.ManageNullStr(dr["ColorNivelAtencion"]),
                },
                SubCategoria = new GLPI_SubCategoriaDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdSubCategoria"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreSubCategoria"]),
                    Categoria = new GLPI_CategoriaDto {
                        Id = ManejoNulos.ManageNullInteger(dr["IdCategoria"]),
                        Nombre = ManejoNulos.ManageNullStr(dr["NombreCategoria"]),
                        Partida = new GLPI_PartidaDto {
                            Id = ManejoNulos.ManageNullInteger(dr["IdPartida"]),
                            Codigo = ManejoNulos.ManageNullStr(dr["CodigoPartida"]),
                            Nombre = ManejoNulos.ManageNullStr(dr["NombrePartida"]),
                            TipoGasto = (GLPI_TipoGasto)ManejoNulos.ManageNullInteger(dr["TipoGastoPartida"]),
                        }
                    }
                },
                ClasificacionProblema = new GLPI_ClasificacionProblemaDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdClasificacionProblema"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreClasificacionProblema"]),
                },
                EstadoActual = new GLPI_EstadoActualDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdEstadoActual"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreEstadoActual"]),
                },
                Identificador = new GLPI_IdentificadorDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdIdentificador"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreIdentificador"]),
                },
                Asignacion = new GLPI_AsignacionDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdAsignacion"]),
                    UsuarioAsigna = new GLPI_UsuarioDto {
                        Id = ManejoNulos.ManageNullInteger(dr["IdUsuarioAsigna"]),
                        Nombres = ManejoNulos.ManageNullStr(dr["NombreUsuarioAsigna"]),
                        ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaternoUsuarioAsigna"]),
                        ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaternoUsuarioAsigna"]),
                        Cargo = ManejoNulos.ManageNullStr(dr["CargoUsuarioAsigna"]),
                        NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumentoUsuarioAsigna"]),
                        CorreoPersonal = ManejoNulos.ManageNullStr(dr["CorreoPersonalUsuarioAsigna"]),
                        CorreoTrabajo = ManejoNulos.ManageNullStr(dr["CorreoTrabajoUsuarioAsigna"]),
                    },
                    EstadoTicket = new GLPI_EstadoTicketDto {
                        Id = ManejoNulos.ManageNullInteger(dr["IdEstadoTicketAsignacion"]),
                        Nombre = ManejoNulos.ManageNullStr(dr["NombreEstadoTicketAsignacion"]),
                    },
                    UsuarioAsignado = new GLPI_UsuarioDto {
                        Id = ManejoNulos.ManageNullInteger(dr["IdUsuarioAsignado"]),
                        Nombres = ManejoNulos.ManageNullStr(dr["NombreUsuarioAsignado"]),
                        ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaternoUsuarioAsignado"]),
                        ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaternoUsuarioAsignado"]),
                        Cargo = ManejoNulos.ManageNullStr(dr["CargoUsuarioAsignado"]),
                        NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumentoUsuarioAsignado"]),
                        CorreoPersonal = ManejoNulos.ManageNullStr(dr["CorreoPersonalUsuarioAsignado"]),
                        CorreoTrabajo = ManejoNulos.ManageNullStr(dr["CorreoTrabajoUsuarioAsignado"]),
                    },
                    FechaTentativaTermino = ManejoNulos.ManageNullDate(dr["FechaTentativaTerminoAsignacion"]),
                    Correos = ManejoNulos.ManageNullStr(dr["CorreosAsignacion"]),
                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistroAsignacion"]),
                },
                Cierre = new GLPI_CierreDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdCierre"]),
                    UsuarioCierra = new GLPI_UsuarioDto {
                        Id = ManejoNulos.ManageNullInteger(dr["IdUsuarioCierra"]),
                        Nombres = ManejoNulos.ManageNullStr(dr["NombreUsuarioCierra"]),
                        ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaternoUsuarioCierra"]),
                        ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaternoUsuarioCierra"]),
                        Cargo = ManejoNulos.ManageNullStr(dr["CargoUsuarioCierra"]),
                        NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumentoUsuarioCierra"]),
                        CorreoPersonal = ManejoNulos.ManageNullStr(dr["CorreoPersonalUsuarioCierra"]),
                        CorreoTrabajo = ManejoNulos.ManageNullStr(dr["CorreoTrabajoUsuarioCierra"]),
                    },
                    EstadoAnterior = new GLPI_EstadoTicketDto {
                        Id = ManejoNulos.ManageNullInteger(dr["IdEstadoAnteriorCierre"]),
                        Nombre = ManejoNulos.ManageNullStr(dr["NombreEstadoAnteriorCierre"])
                    },
                    EstadoActual = new GLPI_EstadoTicketDto {
                        Id = ManejoNulos.ManageNullInteger(dr["IsEstadoActualCierre"]),
                        Nombre = ManejoNulos.ManageNullStr(dr["NombreEstadoActualCierre"])
                    },
                    Descripcion = ManejoNulos.ManageNullStr(dr["DescripcionCierre"]),
                    UsuarioConfirma = new GLPI_UsuarioDto {
                        Id = ManejoNulos.ManageNullInteger(dr["IdUsuarioConfirma"]),
                        Nombres = ManejoNulos.ManageNullStr(dr["NombreUsuarioConfirma"]),
                        ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaternoUsuarioConfirma"]),
                        ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaternoUsuarioConfirma"]),
                        Cargo = ManejoNulos.ManageNullStr(dr["CargoUsuarioConfirma"]),
                        NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumentoUsuarioConfirma"]),
                        CorreoPersonal = ManejoNulos.ManageNullStr(dr["CorreoPersonalUsuarioConfirma"]),
                        CorreoTrabajo = ManejoNulos.ManageNullStr(dr["CorreoTrabajoUsuarioConfirma"]),
                    },
                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistroCierre"])
                },
                Descripcion = ManejoNulos.ManageNullStr(dr["DescripcionTicket"]),
                Adjunto = ManejoNulos.ManageNullStr(dr["AdjuntoTicket"]),
                Correos = ManejoNulos.ManageNullStr(dr["CorreosTicket"]),
                CodigoFaseTicket = (GLPI_FaseTicket)ManejoNulos.ManageNullInteger(dr["CodigoFaseTicket"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistroTicket"])
            };
        }
    }
}
