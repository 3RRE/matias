using CapaEntidad.GLPI;
using CapaEntidad.GLPI.DTO;
using CapaEntidad.GLPI.DTO.Mantenedores;
using CapaEntidad.GLPI.Enum;
using S3k.Utilitario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaDatos.GLPI {
    public class GLPI_AsignacionTicketDAL {
        private readonly string _conexion;

        public GLPI_AsignacionTicketDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int ActualizarAsignacionTicket(GLPI_AsignacionTicket asignacionTicket) {
            int idActualizado;
            string consulta = @"
                UPDATE GLPI_AsignacionTicket
                SET
                    IdEstadoTicket = @IdEstadoTicket,
                    IdUsuarioAsignado = @IdUsuarioAsignado,
                    Correos = @Correos,
                    FechaTentativaTermino = @FechaTentativaTermino,
                    FechaModificacion = @FechaModificacion
                OUTPUT INSERTED.Id
                WHERE Id = @Id
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@Id", asignacionTicket.Id);
                    query.Parameters.AddWithValue("@IdTicket", asignacionTicket.IdTicket);
                    query.Parameters.AddWithValue("@IdEstadoTicket", asignacionTicket.IdEstadoTicket);
                    query.Parameters.AddWithValue("@IdUsuarioAsignado", asignacionTicket.IdUsuarioAsignado);
                    query.Parameters.AddWithValue("@Correos", asignacionTicket.Correos);
                    query.Parameters.AddWithValue("@FechaTentativaTermino", asignacionTicket.FechaTentativaTermino);
                    query.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                    idActualizado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idActualizado = 0;
            }
            return idActualizado;
        }

        public int EliminarAsignacionTicket(int id) {
            int idEliminado;
            string consulta = @"
                DELETE FROM GLPI_AsignacionTicket
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

        public int InsertarAsignacionTicket(GLPI_AsignacionTicket asignacionTicket) {
            int idInsertado;
            string consulta = @"
                INSERT INTO GLPI_AsignacionTicket(IdTicket, IdUsuarioAsigna, IdEstadoTicket, IdUsuarioAsignado, FechaTentativaTermino, Correos)
                OUTPUT INSERTED.Id
                VALUES (@IdTicket, @IdUsuarioAsigna, @IdEstadoTicket, @IdUsuarioAsignado, @FechaTentativaTermino, @Correos)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdTicket", asignacionTicket.IdTicket);
                    query.Parameters.AddWithValue("@IdUsuarioAsigna", asignacionTicket.IdUsuarioAsigna);
                    query.Parameters.AddWithValue("@IdEstadoTicket", asignacionTicket.IdEstadoTicket);
                    query.Parameters.AddWithValue("@IdUsuarioAsignado", asignacionTicket.IdUsuarioAsignado);
                    query.Parameters.AddWithValue("@FechaTentativaTermino", asignacionTicket.FechaTentativaTermino);
                    query.Parameters.AddWithValue("@Correos", asignacionTicket.Correos);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public bool TicketEstaAsignado(int idTicket) {
            int cantidadRegistros = 0;
            string consulta = @"
                SELECT COUNT(Id)
                FROM GLPI_AsignacionTicket
                WHERE IdTicket = @IdTicket
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdTicket", idTicket);
                    cantidadRegistros = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch { }
            return cantidadRegistros >= 1;
        }

        public GLPI_AsignacionTicketDto ObtenerAsignacionTicketPorId(int id) {
            GLPI_AsignacionTicketDto item = new GLPI_AsignacionTicketDto();
            string consulta = @"
                SELECT
                    asti.Id,
                    asti.IdTicket,
                    t.CodSala,
                    s.Nombre AS NombreSala,
                    t.IdUsuarioSolicitante,
                    empsoli.Nombres AS NombreUsuarioSolicitante,
                    empsoli.ApellidosPaterno AS ApellidoPaternoUsuarioSolicitante,
                    empsoli.ApellidosMaterno AS ApellidoMaternoUsuarioSolicitante,
                    carsoli.Descripcion AS CargoUsuarioSolicitante,
                    empsoli.DOI AS NumeroDocumentoUsuarioSolicitante,
                    t.IdTipoOperacion,
                    tiop.Nombre AS NombreTipoOperacion,
                    t.IdNivelAtencion,
                    niat.Nombre AS NombreNivelAtencion,
                    niat.Color AS ColorNivelAtencion,
                    cat.IdPartida,
                    par.Codigo AS CodigoPartida,
                    par.Nombre AS NombrePartida,
                    par.TipoGasto AS TipoGastoPartida,
                    suca.IdCategoria,
                    cat.Nombre AS NombreCategoria,
                    t.IdSubCategoria,
                    suca.Nombre AS NombreSubCategoria,
                    t.IdClasificacionProblema,
                    clapro.Nombre AS NombreClasificacionProblema,
                    t.IdEstadoActual,
                    esac.Nombre AS NombreEstadoActual,
                    t.IdIdentificador,
                    ide.Nombre AS NombreIdentificador,
                    t.Descripcion AS DescripcionTicket,
                    t.Adjunto AS AdjuntoTicket,
                    t.Correos AS CorreosTicket,
                    t.CodigoFaseTicket,
                    t.FechaRegistro AS FechaRegistroTicket,
                    asti.IdUsuarioAsigna,
                    empasi1.Nombres AS NombreUsuarioAsigna,
                    empasi1.ApellidosPaterno AS ApellidoPaternoUsuarioAsigna,
                    empasi1.ApellidosMaterno AS ApellidoMaternoUsuarioAsigna,
                    carasi1.Descripcion AS CargoUsuarioAsigna,
                    empasi1.DOI AS NumeroDocumentoUsuarioAsigna,
                    asti.IdEstadoTicket,
                    esti.Nombre AS NombreEstadoTicket,
                    asti.IdUsuarioAsignado,
                    empasi2.Nombres AS NombreUsuarioAsignado,
                    empasi2.ApellidosPaterno AS ApellidoPaternoUsuarioAsignado,
                    empasi2.ApellidosMaterno AS ApellidoMaternoUsuarioAsignado,
                    carasi2.Descripcion AS CargoUsuarioAsignado,
                    empasi2.DOI AS NumeroDocumentoUsuarioAsignado,
                    asti.FechaTentativaTermino,
                    asti.Correos,
                    asti.FechaRegistro,
                    asti.FechaModificacion
                FROM
                    GLPI_AsignacionTicket AS asti
                LEFT JOIN GLPI_Ticket AS t ON t.Id = asti.IdTicket
                INNER JOIN Sala AS s ON s.CodSala = t.CodSala
                LEFT JOIN SEG_Usuario AS ususoli ON ususoli.UsuarioID = t.IdUsuarioSolicitante
                INNER JOIN SEG_Empleado AS empsoli ON empsoli.EmpleadoID = ususoli.EmpleadoID
                INNER JOIN SEG_Cargo AS carsoli ON carsoli.CargoID = empsoli.CargoID
                LEFT JOIN SEG_Usuario AS usuasi1 ON usuasi1.UsuarioID = asti.IdUsuarioAsigna
                INNER JOIN SEG_Empleado AS empasi1 ON empasi1.EmpleadoID = usuasi1.EmpleadoID
                INNER JOIN SEG_Cargo AS carasi1 ON carasi1.CargoID = empasi1.CargoID
                LEFT JOIN SEG_Usuario AS usuasi2 ON usuasi2.UsuarioID = asti.IdUsuarioAsignado
                INNER JOIN SEG_Empleado AS empasi2 ON empasi2.EmpleadoID = usuasi2.EmpleadoID
                INNER JOIN SEG_Cargo AS carasi2 ON carasi2.CargoID = empasi2.CargoID
                LEFT JOIN GLPI_TipoOperacion AS tiop ON tiop.Id = t.IdTipoOperacion
                LEFT JOIN GLPI_NivelAtencion AS niat ON niat.Id = t.IdNivelAtencion
                LEFT JOIN GLPI_SubCategoria AS suca ON suca.Id = t.IdSubCategoria
                LEFT JOIN GLPI_Categoria AS cat ON cat.Id = suca.IdCategoria
                LEFT JOIN GLPI_Partida AS par ON par.Id = cat.IdPartida
                LEFT JOIN GLPI_ClasificacionProblema AS clapro ON clapro.Id = t.IdClasificacionProblema
                LEFT JOIN GLPI_EstadoActual AS esac ON esac.Id = t.IdEstadoActual
                LEFT JOIN GLPI_Identificador AS ide ON ide.Id = t.IdIdentificador
                LEFT JOIN GLPI_EstadoTicket AS esti ON esti.Id = asti.IdEstadoTicket
                WHERE
                    asti.Id = @Id
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

        public GLPI_AsignacionTicketDto ObtenerAsignacionTicketPorIdTicket(int idTicket) {
            GLPI_AsignacionTicketDto item = new GLPI_AsignacionTicketDto();
            string consulta = @"
                SELECT
                    asti.Id,
                    asti.IdTicket,
                    t.CodSala,
                    s.Nombre AS NombreSala,
                    t.IdUsuarioSolicitante,
                    empsoli.Nombres AS NombreUsuarioSolicitante,
                    empsoli.ApellidosPaterno AS ApellidoPaternoUsuarioSolicitante,
                    empsoli.ApellidosMaterno AS ApellidoMaternoUsuarioSolicitante,
                    carsoli.Descripcion AS CargoUsuarioSolicitante,
                    empsoli.DOI AS NumeroDocumentoUsuarioSolicitante,
                    t.IdTipoOperacion,
                    tiop.Nombre AS NombreTipoOperacion,
                    t.IdNivelAtencion,
                    niat.Nombre AS NombreNivelAtencion,
                    niat.Color AS ColorNivelAtencion,
                    cat.IdPartida,
                    par.Codigo AS CodigoPartida,
                    par.Nombre AS NombrePartida,
                    par.TipoGasto AS TipoGastoPartida,
                    suca.IdCategoria,
                    cat.Nombre AS NombreCategoria,
                    t.IdSubCategoria,
                    suca.Nombre AS NombreSubCategoria,
                    t.IdClasificacionProblema,
                    clapro.Nombre AS NombreClasificacionProblema,
                    t.IdEstadoActual,
                    esac.Nombre AS NombreEstadoActual,
                    t.IdIdentificador,
                    ide.Nombre AS NombreIdentificador,
                    t.Descripcion AS DescripcionTicket,
                    t.Adjunto AS AdjuntoTicket,
                    t.Correos AS CorreosTicket,
                    t.CodigoFaseTicket,
                    t.FechaRegistro AS FechaRegistroTicket,
                    asti.IdUsuarioAsigna,
                    empasi1.Nombres AS NombreUsuarioAsigna,
                    empasi1.ApellidosPaterno AS ApellidoPaternoUsuarioAsigna,
                    empasi1.ApellidosMaterno AS ApellidoMaternoUsuarioAsigna,
                    carasi1.Descripcion AS CargoUsuarioAsigna,
                    empasi1.DOI AS NumeroDocumentoUsuarioAsigna,
                    asti.IdEstadoTicket,
                    esti.Nombre AS NombreEstadoTicket,
                    asti.IdUsuarioAsignado,
                    empasi2.Nombres AS NombreUsuarioAsignado,
                    empasi2.ApellidosPaterno AS ApellidoPaternoUsuarioAsignado,
                    empasi2.ApellidosMaterno AS ApellidoMaternoUsuarioAsignado,
                    carasi2.Descripcion AS CargoUsuarioAsignado,
                    empasi2.DOI AS NumeroDocumentoUsuarioAsignado,
                    asti.FechaTentativaTermino,
                    asti.Correos,
                    asti.FechaRegistro,
                    asti.FechaModificacion
                FROM
                    GLPI_AsignacionTicket AS asti
                LEFT JOIN GLPI_Ticket AS t ON t.Id = asti.IdTicket
                INNER JOIN Sala AS s ON s.CodSala = t.CodSala
                LEFT JOIN SEG_Usuario AS ususoli ON ususoli.UsuarioID = t.IdUsuarioSolicitante
                INNER JOIN SEG_Empleado AS empsoli ON empsoli.EmpleadoID = ususoli.EmpleadoID
                INNER JOIN SEG_Cargo AS carsoli ON carsoli.CargoID = empsoli.CargoID
                LEFT JOIN SEG_Usuario AS usuasi1 ON usuasi1.UsuarioID = asti.IdUsuarioAsigna
                INNER JOIN SEG_Empleado AS empasi1 ON empasi1.EmpleadoID = usuasi1.EmpleadoID
                INNER JOIN SEG_Cargo AS carasi1 ON carasi1.CargoID = empasi1.CargoID
                LEFT JOIN SEG_Usuario AS usuasi2 ON usuasi2.UsuarioID = asti.IdUsuarioAsignado
                INNER JOIN SEG_Empleado AS empasi2 ON empasi2.EmpleadoID = usuasi2.EmpleadoID
                INNER JOIN SEG_Cargo AS carasi2 ON carasi2.CargoID = empasi2.CargoID
                LEFT JOIN GLPI_TipoOperacion AS tiop ON tiop.Id = t.IdTipoOperacion
                LEFT JOIN GLPI_NivelAtencion AS niat ON niat.Id = t.IdNivelAtencion
                LEFT JOIN GLPI_SubCategoria AS suca ON suca.Id = t.IdSubCategoria
                LEFT JOIN GLPI_Categoria AS cat ON cat.Id = suca.IdCategoria
                LEFT JOIN GLPI_Partida AS par ON par.Id = cat.IdPartida
                LEFT JOIN GLPI_ClasificacionProblema AS clapro ON clapro.Id = t.IdClasificacionProblema
                LEFT JOIN GLPI_EstadoActual AS esac ON esac.Id = t.IdEstadoActual
                LEFT JOIN GLPI_Identificador AS ide ON ide.Id = t.IdIdentificador
                LEFT JOIN GLPI_EstadoTicket AS esti ON esti.Id = asti.IdEstadoTicket
                WHERE
                    asti.IdTicket = @IdTicket
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdTicket", idTicket);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            item = ConstruirObjeto(dr);
                        }
                    }
                }
            } catch { }
            return item;
        }

        public List<GLPI_AsignacionTicketDto> ObtenerAsignacionesTicketsPorIdUsuarioAsigna(int idUsuarioAsigna) {
            List<GLPI_AsignacionTicketDto> items = new List<GLPI_AsignacionTicketDto>();
            string consulta = @"
                SELECT
                    asti.Id,
                    asti.IdTicket,
                    t.CodSala,
                    s.Nombre AS NombreSala,
                    t.IdUsuarioSolicitante,
                    empsoli.Nombres AS NombreUsuarioSolicitante,
                    empsoli.ApellidosPaterno AS ApellidoPaternoUsuarioSolicitante,
                    empsoli.ApellidosMaterno AS ApellidoMaternoUsuarioSolicitante,
                    carsoli.Descripcion AS CargoUsuarioSolicitante,
                    empsoli.DOI AS NumeroDocumentoUsuarioSolicitante,
                    t.IdTipoOperacion,
                    tiop.Nombre AS NombreTipoOperacion,
                    t.IdNivelAtencion,
                    niat.Nombre AS NombreNivelAtencion,
                    niat.Color AS ColorNivelAtencion,
                    cat.IdPartida,
                    par.Codigo AS CodigoPartida,
                    par.Nombre AS NombrePartida,
                    par.TipoGasto AS TipoGastoPartida,
                    suca.IdCategoria,
                    cat.Nombre AS NombreCategoria,
                    t.IdSubCategoria,
                    suca.Nombre AS NombreSubCategoria,
                    t.IdClasificacionProblema,
                    clapro.Nombre AS NombreClasificacionProblema,
                    t.IdEstadoActual,
                    esac.Nombre AS NombreEstadoActual,
                    t.IdIdentificador,
                    ide.Nombre AS NombreIdentificador,
                    t.Descripcion AS DescripcionTicket,
                    t.Adjunto AS AdjuntoTicket,
                    t.Correos AS CorreosTicket,
                    t.CodigoFaseTicket,
                    t.FechaRegistro AS FechaRegistroTicket,
                    asti.IdUsuarioAsigna,
                    empasi1.Nombres AS NombreUsuarioAsigna,
                    empasi1.ApellidosPaterno AS ApellidoPaternoUsuarioAsigna,
                    empasi1.ApellidosMaterno AS ApellidoMaternoUsuarioAsigna,
                    carasi1.Descripcion AS CargoUsuarioAsigna,
                    empasi1.DOI AS NumeroDocumentoUsuarioAsigna,
                    asti.IdEstadoTicket,
                    esti.Nombre AS NombreEstadoTicket,
                    asti.IdUsuarioAsignado,
                    empasi2.Nombres AS NombreUsuarioAsignado,
                    empasi2.ApellidosPaterno AS ApellidoPaternoUsuarioAsignado,
                    empasi2.ApellidosMaterno AS ApellidoMaternoUsuarioAsignado,
                    carasi2.Descripcion AS CargoUsuarioAsignado,
                    empasi2.DOI AS NumeroDocumentoUsuarioAsignado,
                    asti.FechaTentativaTermino,
                    asti.Correos,
                    asti.FechaRegistro,
                    asti.FechaModificacion
                FROM
                    GLPI_AsignacionTicket AS asti
                LEFT JOIN GLPI_Ticket AS t ON t.Id = asti.IdTicket
                INNER JOIN Sala AS s ON s.CodSala = t.CodSala
                LEFT JOIN SEG_Usuario AS ususoli ON ususoli.UsuarioID = t.IdUsuarioSolicitante
                INNER JOIN SEG_Empleado AS empsoli ON empsoli.EmpleadoID = ususoli.EmpleadoID
                INNER JOIN SEG_Cargo AS carsoli ON carsoli.CargoID = empsoli.CargoID
                LEFT JOIN SEG_Usuario AS usuasi1 ON usuasi1.UsuarioID = asti.IdUsuarioAsigna
                INNER JOIN SEG_Empleado AS empasi1 ON empasi1.EmpleadoID = usuasi1.EmpleadoID
                INNER JOIN SEG_Cargo AS carasi1 ON carasi1.CargoID = empasi1.CargoID
                LEFT JOIN SEG_Usuario AS usuasi2 ON usuasi2.UsuarioID = asti.IdUsuarioAsignado
                INNER JOIN SEG_Empleado AS empasi2 ON empasi2.EmpleadoID = usuasi2.EmpleadoID
                INNER JOIN SEG_Cargo AS carasi2 ON carasi2.CargoID = empasi2.CargoID
                LEFT JOIN GLPI_TipoOperacion AS tiop ON tiop.Id = t.IdTipoOperacion
                LEFT JOIN GLPI_NivelAtencion AS niat ON niat.Id = t.IdNivelAtencion
                LEFT JOIN GLPI_SubCategoria AS suca ON suca.Id = t.IdSubCategoria
                LEFT JOIN GLPI_Categoria AS cat ON cat.Id = suca.IdCategoria
                LEFT JOIN GLPI_Partida AS par ON par.Id = cat.IdPartida
                LEFT JOIN GLPI_ClasificacionProblema AS clapro ON clapro.Id = t.IdClasificacionProblema
                LEFT JOIN GLPI_EstadoActual AS esac ON esac.Id = t.IdEstadoActual
                LEFT JOIN GLPI_Identificador AS ide ON ide.Id = t.IdIdentificador
                LEFT JOIN GLPI_EstadoTicket AS esti ON esti.Id = asti.IdEstadoTicket
                WHERE asti.IdUsuarioAsigna = @IdUsuarioAsigna
                ORDER BY t.FechaRegistro DESC
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdUsuarioAsigna", idUsuarioAsigna);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<GLPI_AsignacionTicketDto> ObtenerAsignacionesTicketsPorIdUsuarioAsignado(int idUsuarioAsignado) {
            List<GLPI_AsignacionTicketDto> items = new List<GLPI_AsignacionTicketDto>();
            string consulta = @"
                SELECT
                    asti.Id,
                    asti.IdTicket,
                    t.CodSala,
                    s.Nombre AS NombreSala,
                    t.IdUsuarioSolicitante,
                    empsoli.Nombres AS NombreUsuarioSolicitante,
                    empsoli.ApellidosPaterno AS ApellidoPaternoUsuarioSolicitante,
                    empsoli.ApellidosMaterno AS ApellidoMaternoUsuarioSolicitante,
                    carsoli.Descripcion AS CargoUsuarioSolicitante,
                    empsoli.DOI AS NumeroDocumentoUsuarioSolicitante,
                    t.IdTipoOperacion,
                    tiop.Nombre AS NombreTipoOperacion,
                    t.IdNivelAtencion,
                    niat.Nombre AS NombreNivelAtencion,
                    niat.Color AS ColorNivelAtencion,
                    cat.IdPartida,
                    par.Codigo AS CodigoPartida,
                    par.Nombre AS NombrePartida,
                    par.TipoGasto AS TipoGastoPartida,
                    suca.IdCategoria,
                    cat.Nombre AS NombreCategoria,
                    t.IdSubCategoria,
                    suca.Nombre AS NombreSubCategoria,
                    t.IdClasificacionProblema,
                    clapro.Nombre AS NombreClasificacionProblema,
                    t.IdEstadoActual,
                    esac.Nombre AS NombreEstadoActual,
                    t.IdIdentificador,
                    ide.Nombre AS NombreIdentificador,
                    t.Descripcion AS DescripcionTicket,
                    t.Adjunto AS AdjuntoTicket,
                    t.Correos AS CorreosTicket,
                    t.CodigoFaseTicket,
                    t.FechaRegistro AS FechaRegistroTicket,
                    asti.IdUsuarioAsigna,
                    empasi1.Nombres AS NombreUsuarioAsigna,
                    empasi1.ApellidosPaterno AS ApellidoPaternoUsuarioAsigna,
                    empasi1.ApellidosMaterno AS ApellidoMaternoUsuarioAsigna,
                    carasi1.Descripcion AS CargoUsuarioAsigna,
                    empasi1.DOI AS NumeroDocumentoUsuarioAsigna,
                    asti.IdEstadoTicket,
                    esti.Nombre AS NombreEstadoTicket,
                    asti.IdUsuarioAsignado,
                    empasi2.Nombres AS NombreUsuarioAsignado,
                    empasi2.ApellidosPaterno AS ApellidoPaternoUsuarioAsignado,
                    empasi2.ApellidosMaterno AS ApellidoMaternoUsuarioAsignado,
                    carasi2.Descripcion AS CargoUsuarioAsignado,
                    empasi2.DOI AS NumeroDocumentoUsuarioAsignado,
                    asti.FechaTentativaTermino,
                    asti.Correos,
                    asti.FechaRegistro,
                    asti.FechaModificacion
                FROM
                    GLPI_AsignacionTicket AS asti
                LEFT JOIN GLPI_Ticket AS t ON t.Id = asti.IdTicket
                INNER JOIN Sala AS s ON s.CodSala = t.CodSala
                LEFT JOIN SEG_Usuario AS ususoli ON ususoli.UsuarioID = t.IdUsuarioSolicitante
                INNER JOIN SEG_Empleado AS empsoli ON empsoli.EmpleadoID = ususoli.EmpleadoID
                INNER JOIN SEG_Cargo AS carsoli ON carsoli.CargoID = empsoli.CargoID
                LEFT JOIN SEG_Usuario AS usuasi1 ON usuasi1.UsuarioID = asti.IdUsuarioAsigna
                INNER JOIN SEG_Empleado AS empasi1 ON empasi1.EmpleadoID = usuasi1.EmpleadoID
                INNER JOIN SEG_Cargo AS carasi1 ON carasi1.CargoID = empasi1.CargoID
                LEFT JOIN SEG_Usuario AS usuasi2 ON usuasi2.UsuarioID = asti.IdUsuarioAsignado
                INNER JOIN SEG_Empleado AS empasi2 ON empasi2.EmpleadoID = usuasi2.EmpleadoID
                INNER JOIN SEG_Cargo AS carasi2 ON carasi2.CargoID = empasi2.CargoID
                LEFT JOIN GLPI_TipoOperacion AS tiop ON tiop.Id = t.IdTipoOperacion
                LEFT JOIN GLPI_NivelAtencion AS niat ON niat.Id = t.IdNivelAtencion
                LEFT JOIN GLPI_SubCategoria AS suca ON suca.Id = t.IdSubCategoria
                LEFT JOIN GLPI_Categoria AS cat ON cat.Id = suca.IdCategoria
                LEFT JOIN GLPI_Partida AS par ON par.Id = cat.IdPartida
                LEFT JOIN GLPI_ClasificacionProblema AS clapro ON clapro.Id = t.IdClasificacionProblema
                LEFT JOIN GLPI_EstadoActual AS esac ON esac.Id = t.IdEstadoActual
                LEFT JOIN GLPI_Identificador AS ide ON ide.Id = t.IdIdentificador
                LEFT JOIN GLPI_EstadoTicket AS esti ON esti.Id = asti.IdEstadoTicket
                WHERE asti.IdUsuarioAsignado = @IdUsuarioAsignado
                ORDER BY t.FechaRegistro DESC
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdUsuarioAsignado", idUsuarioAsignado);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<GLPI_AsignacionTicketDto> ObtenerAsignacionesTicket() {
            List<GLPI_AsignacionTicketDto> items = new List<GLPI_AsignacionTicketDto>();
            string consulta = @"
                SELECT
                    asti.Id,
                    asti.IdTicket,
                    t.CodSala,
                    s.Nombre AS NombreSala,
                    t.IdUsuarioSolicitante,
                    empsoli.Nombres AS NombreUsuarioSolicitante,
                    empsoli.ApellidosPaterno AS ApellidoPaternoUsuarioSolicitante,
                    empsoli.ApellidosMaterno AS ApellidoMaternoUsuarioSolicitante,
                    carsoli.Descripcion AS CargoUsuarioSolicitante,
                    empsoli.DOI AS NumeroDocumentoUsuarioSolicitante,
                    t.IdTipoOperacion,
                    tiop.Nombre AS NombreTipoOperacion,
                    t.IdNivelAtencion,
                    niat.Nombre AS NombreNivelAtencion,
                    niat.Color AS ColorNivelAtencion,
                    cat.IdPartida,
                    par.Codigo AS CodigoPartida,
                    par.Nombre AS NombrePartida,
                    par.TipoGasto AS TipoGastoPartida,
                    suca.IdCategoria,
                    cat.Nombre AS NombreCategoria,
                    t.IdSubCategoria,
                    suca.Nombre AS NombreSubCategoria,
                    t.IdClasificacionProblema,
                    clapro.Nombre AS NombreClasificacionProblema,
                    t.IdEstadoActual,
                    esac.Nombre AS NombreEstadoActual,
                    t.IdIdentificador,
                    ide.Nombre AS NombreIdentificador,
                    t.Descripcion AS DescripcionTicket,
                    t.Adjunto AS AdjuntoTicket,
                    t.Correos AS CorreosTicket,
                    t.CodigoFaseTicket,
                    t.FechaRegistro AS FechaRegistroTicket,
                    asti.IdUsuarioAsigna,
                    empasi1.Nombres AS NombreUsuarioAsigna,
                    empasi1.ApellidosPaterno AS ApellidoPaternoUsuarioAsigna,
                    empasi1.ApellidosMaterno AS ApellidoMaternoUsuarioAsigna,
                    carasi1.Descripcion AS CargoUsuarioAsigna,
                    empasi1.DOI AS NumeroDocumentoUsuarioAsigna,
                    asti.IdEstadoTicket,
                    esti.Nombre AS NombreEstadoTicket,
                    asti.IdUsuarioAsignado,
                    empasi2.Nombres AS NombreUsuarioAsignado,
                    empasi2.ApellidosPaterno AS ApellidoPaternoUsuarioAsignado,
                    empasi2.ApellidosMaterno AS ApellidoMaternoUsuarioAsignado,
                    carasi2.Descripcion AS CargoUsuarioAsignado,
                    empasi2.DOI AS NumeroDocumentoUsuarioAsignado,
                    asti.FechaTentativaTermino,
                    asti.Correos,
                    asti.FechaRegistro,
                    asti.FechaModificacion
                FROM
                    GLPI_AsignacionTicket AS asti
                LEFT JOIN GLPI_Ticket AS t ON t.Id = asti.IdTicket
                INNER JOIN Sala AS s ON s.CodSala = t.CodSala
                LEFT JOIN SEG_Usuario AS ususoli ON ususoli.UsuarioID = t.IdUsuarioSolicitante
                INNER JOIN SEG_Empleado AS empsoli ON empsoli.EmpleadoID = ususoli.EmpleadoID
                INNER JOIN SEG_Cargo AS carsoli ON carsoli.CargoID = empsoli.CargoID
                LEFT JOIN SEG_Usuario AS usuasi1 ON usuasi1.UsuarioID = asti.IdUsuarioAsigna
                INNER JOIN SEG_Empleado AS empasi1 ON empasi1.EmpleadoID = usuasi1.EmpleadoID
                INNER JOIN SEG_Cargo AS carasi1 ON carasi1.CargoID = empasi1.CargoID
                LEFT JOIN SEG_Usuario AS usuasi2 ON usuasi2.UsuarioID = asti.IdUsuarioAsignado
                INNER JOIN SEG_Empleado AS empasi2 ON empasi2.EmpleadoID = usuasi2.EmpleadoID
                INNER JOIN SEG_Cargo AS carasi2 ON carasi2.CargoID = empasi2.CargoID
                LEFT JOIN GLPI_TipoOperacion AS tiop ON tiop.Id = t.IdTipoOperacion
                LEFT JOIN GLPI_NivelAtencion AS niat ON niat.Id = t.IdNivelAtencion
                LEFT JOIN GLPI_SubCategoria AS suca ON suca.Id = t.IdSubCategoria
                LEFT JOIN GLPI_Categoria AS cat ON cat.Id = suca.IdCategoria
                LEFT JOIN GLPI_Partida AS par ON par.Id = cat.IdPartida
                LEFT JOIN GLPI_ClasificacionProblema AS clapro ON clapro.Id = t.IdClasificacionProblema
                LEFT JOIN GLPI_EstadoActual AS esac ON esac.Id = t.IdEstadoActual
                LEFT JOIN GLPI_Identificador AS ide ON ide.Id = t.IdIdentificador
                LEFT JOIN GLPI_EstadoTicket AS esti ON esti.Id = asti.IdEstadoTicket
                ORDER BY t.FechaRegistro DESC
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjeto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        private GLPI_AsignacionTicketDto ConstruirObjeto(SqlDataReader dr) {
            return new GLPI_AsignacionTicketDto {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                Ticket = new GLPI_TicketDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdTicket"]),
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
                    Descripcion = ManejoNulos.ManageNullStr(dr["DescripcionTicket"]),
                    Adjunto = ManejoNulos.ManageNullStr(dr["AdjuntoTicket"]),
                    Correos = ManejoNulos.ManageNullStr(dr["CorreosTicket"]),
                    CodigoFaseTicket = (GLPI_FaseTicket)ManejoNulos.ManageNullInteger(dr["CodigoFaseTicket"]),
                    FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistroTicket"])
                },
                UsuarioAsigna = new GLPI_UsuarioDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdUsuarioAsigna"]),
                    Nombres = ManejoNulos.ManageNullStr(dr["NombreUsuarioAsigna"]),
                    ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaternoUsuarioAsigna"]),
                    ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaternoUsuarioAsigna"]),
                    Cargo = ManejoNulos.ManageNullStr(dr["CargoUsuarioAsigna"]),
                    NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumentoUsuarioAsigna"]),
                },
                EstadoTicket = new GLPI_EstadoTicketDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdEstadoTicket"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreEstadoTicket"]),
                },
                UsuarioAsignado = new GLPI_UsuarioDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdUsuarioAsignado"]),
                    Nombres = ManejoNulos.ManageNullStr(dr["NombreUsuarioAsignado"]),
                    ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaternoUsuarioAsignado"]),
                    ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaternoUsuarioAsignado"]),
                    Cargo = ManejoNulos.ManageNullStr(dr["CargoUsuarioAsignado"]),
                    NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumentoUsuarioAsignado"]),
                },
                FechaTentativaTermino = ManejoNulos.ManageNullDate(dr["FechaTentativaTermino"]),
                Correos = ManejoNulos.ManageNullStr(dr["Correos"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
            };
        }
    }
}
