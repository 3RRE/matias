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
    public class GLPI_SeguimientoTicketDAL {
        private readonly string _conexion;

        public GLPI_SeguimientoTicketDAL() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public int InsertarSeguimientoTicket(GLPI_SeguimientoTicket seguimientoTicket) {
            int idInsertado;
            string consulta = @"
                INSERT INTO GLPI_SeguimientoTicket(IdTicket, IdUsuarioRegistra, IdEstadoTicketAnterior, IdEstadoTicketActual, Descripcion, IdProcesoAnterior, IdProcesoActual, Correos)
                OUTPUT INSERTED.Id
                VALUES (@IdTicket, @IdUsuarioRegistra, @IdEstadoTicketAnterior, @IdEstadoTicketActual, @Descripcion, @IdProcesoAnterior, @IdProcesoActual, @Correos)
            ";

            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdTicket", seguimientoTicket.IdTicket);
                    query.Parameters.AddWithValue("@IdUsuarioRegistra", seguimientoTicket.IdUsuarioRegistra);
                    query.Parameters.AddWithValue("@IdEstadoTicketAnterior", seguimientoTicket.IdEstadoTicketAnterior);
                    query.Parameters.AddWithValue("@IdEstadoTicketActual", seguimientoTicket.IdEstadoTicketActual);
                    query.Parameters.AddWithValue("@Descripcion", seguimientoTicket.Descripcion);
                    query.Parameters.AddWithValue("@IdProcesoAnterior", seguimientoTicket.IdProcesoAnterior);
                    query.Parameters.AddWithValue("@IdProcesoActual", seguimientoTicket.IdProcesoActual);
                    query.Parameters.AddWithValue("@Correos", seguimientoTicket.Correos);
                    idInsertado = Convert.ToInt32(query.ExecuteScalar());
                }
            } catch {
                idInsertado = 0;
            }

            return idInsertado;
        }

        public List<GLPI_SeguimientoTicketDto> ObtenerSeguimientoTicketPorIdTicket(int idTicket) {
            List<GLPI_SeguimientoTicketDto> items = new List<GLPI_SeguimientoTicketDto>();
            string consulta = @"
                SELECT
	                seti.Id,
	                seti.IdTicket,
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
	                seti.IdUsuarioRegistra,
	                empreg.Nombres AS NombreUsuarioRegistra,
	                empreg.ApellidosPaterno AS ApellidoPaternoUsuarioRegistra,
	                empreg.ApellidosMaterno AS ApellidoMaternoUsuarioRegistra,
	                carreg.Descripcion AS CargoUsuarioRegistra,
	                empreg.DOI AS NumeroDocumentoUsuarioRegistra,
	                seti.IdEstadoTicketAnterior,
	                estian.Nombre AS NombreEstadoTicketAnterior,
	                seti.IdEstadoTicketActual,
	                estiac.Nombre AS NombreEstadoTicketActual,
	                seti.Correos,
	                seti.IdProcesoAnterior,
	                proan.Nombre AS NombreProcesoAnterior,
	                seti.IdProcesoActual,
	                proac.Nombre AS NombreProcesoActual,
	                seti.Descripcion,
	                seti.FechaRegistro
                FROM
	                GLPI_SeguimientoTicket AS seti
                LEFT JOIN GLPI_Ticket AS t ON t.Id = seti.IdTicket
                INNER JOIN Sala AS s ON s.CodSala = t.CodSala
                LEFT JOIN SEG_Usuario AS ususoli ON ususoli.UsuarioID = t.IdUsuarioSolicitante
                INNER JOIN SEG_Empleado AS empsoli ON empsoli.EmpleadoID = ususoli.EmpleadoID
                INNER JOIN SEG_Cargo AS carsoli ON carsoli.CargoID = empsoli.CargoID
                LEFT JOIN SEG_Usuario AS usureg ON usureg.UsuarioID = seti.IdUsuarioRegistra
                INNER JOIN SEG_Empleado AS empreg ON empreg.EmpleadoID = usureg.EmpleadoID
                INNER JOIN SEG_Cargo AS carreg ON carreg.CargoID = empreg.CargoID
                LEFT JOIN GLPI_TipoOperacion AS tiop ON tiop.Id = t.IdTipoOperacion
                LEFT JOIN GLPI_NivelAtencion AS niat ON niat.Id = t.IdNivelAtencion
                LEFT JOIN GLPI_SubCategoria AS suca ON suca.Id = t.IdSubCategoria
                LEFT JOIN GLPI_Categoria AS cat ON cat.Id = suca.IdCategoria
                LEFT JOIN GLPI_Partida AS par ON par.Id = cat.IdPartida
                LEFT JOIN GLPI_ClasificacionProblema AS clapro ON clapro.Id = t.IdClasificacionProblema
                LEFT JOIN GLPI_EstadoActual AS esac ON esac.Id = t.IdEstadoActual
                LEFT JOIN GLPI_Identificador AS ide ON ide.Id = t.IdIdentificador
                LEFT JOIN GLPI_EstadoTicket AS estian ON estian.Id = seti.IdEstadoTicketAnterior
                LEFT JOIN GLPI_EstadoTicket AS estiac ON estiac.Id = seti.IdEstadoTicketActual
                LEFT JOIN GLPI_Proceso AS proan ON proan.Id = seti.IdProcesoAnterior
                LEFT JOIN GLPI_Proceso AS proac ON proac.Id = seti.IdProcesoActual
                WHERE seti.IdTicket = @IdTicket
                ORDER BY t.FechaRegistro DESC
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdTicket", idTicket);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjetoDto(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public List<GLPI_SeguimientoDto> ObtenerSeguimientosPorIdTicket(int idTicket) {
            List<GLPI_SeguimientoDto> items = new List<GLPI_SeguimientoDto>();
            string consulta = @"
                SELECT
                    seti.Id,
                    seti.IdUsuarioRegistra,
                    empreg.Nombres AS NombreUsuarioRegistra,
                    empreg.ApellidosPaterno AS ApellidoPaternoUsuarioRegistra,
                    empreg.ApellidosMaterno AS ApellidoMaternoUsuarioRegistra,
                    carreg.Descripcion AS CargoUsuarioRegistra,
                    empreg.DOI AS NumeroDocumentoUsuarioRegistra,
                    seti.IdEstadoTicketAnterior,
                    estian.Nombre AS NombreEstadoTicketAnterior,
                    seti.IdEstadoTicketActual,
                    estiac.Nombre AS NombreEstadoTicketActual,
                    seti.Correos,
                    seti.IdProcesoAnterior,
                    proan.Nombre AS NombreProcesoAnterior,
                    seti.IdProcesoActual,
                    proac.Nombre AS NombreProcesoActual,
                    seti.Descripcion,
                    seti.FechaRegistro
                FROM GLPI_SeguimientoTicket AS seti
                LEFT JOIN SEG_Usuario AS usureg ON usureg.UsuarioID = seti.IdUsuarioRegistra
                INNER JOIN SEG_Empleado AS empreg ON empreg.EmpleadoID = usureg.EmpleadoID
                INNER JOIN SEG_Cargo AS carreg ON carreg.CargoID = empreg.CargoID
                LEFT JOIN GLPI_EstadoTicket AS estian ON estian.Id = seti.IdEstadoTicketAnterior
                LEFT JOIN GLPI_EstadoTicket AS estiac ON estiac.Id = seti.IdEstadoTicketActual
                LEFT JOIN GLPI_Proceso AS proan ON proan.Id = seti.IdProcesoAnterior
                LEFT JOIN GLPI_Proceso AS proac ON proac.Id = seti.IdProcesoActual
                WHERE seti.IdTicket = @IdTicket
                ORDER BY seti.FechaRegistro ASC
            ";
            try {
                using(SqlConnection con = new SqlConnection(_conexion)) {
                    con.Open();
                    SqlCommand query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@IdTicket", idTicket);
                    using(SqlDataReader dr = query.ExecuteReader()) {
                        while(dr.Read()) {
                            items.Add(ConstruirObjetoDto1(dr));
                        }
                    }
                }
            } catch { }
            return items;
        }

        public GLPI_SeguimientoTicket ObtenerUltimoSeguimientoDeTicket(int idTicket) {
            GLPI_SeguimientoTicket item = new GLPI_SeguimientoTicket();
            string consulta = @"
                SELECT TOP 1
	                Id,
	                IdTicket,
	                IdUsuarioRegistra,
	                IdEstadoTicketAnterior,
	                IdEstadoTicketActual,
	                Descripcion,
	                IdProcesoAnterior,
	                IdProcesoActual,
	                Correos,
	                FechaRegistro
                FROM GLPI_SeguimientoTicket
                WHERE IdTicket = @IdTicket
                ORDER BY FechaRegistro DESC
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

        public bool TicketTieneSeguimiento(int idTicket) {
            int cantidadRegistros = 0;
            string consulta = @"
                SELECT COUNT(Id)
                FROM GLPI_SeguimientoTicket
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

        private GLPI_SeguimientoTicketDto ConstruirObjetoDto(SqlDataReader dr) {
            return new GLPI_SeguimientoTicketDto {
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
                UsuarioRegistra = new GLPI_UsuarioDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdUsuarioRegistra"]),
                    Nombres = ManejoNulos.ManageNullStr(dr["NombreUsuarioRegistra"]),
                    ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaternoUsuarioRegistra"]),
                    ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaternoUsuarioRegistra"]),
                    Cargo = ManejoNulos.ManageNullStr(dr["CargoUsuarioRegistra"]),
                    NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumentoUsuarioRegistra"]),
                },
                EstadoTicketAnterior = new GLPI_EstadoTicketDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdEstadoTicketAnterior"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreEstadoTicketAnterior"]),
                },
                EstadoTicketActual = new GLPI_EstadoTicketDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdEstadoTicketActual"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreEstadoTicketActual"]),
                },
                Correos = ManejoNulos.ManageNullStr(dr["Correos"]),
                ProcesoAnterior = new GLPI_ProcesoDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdProcesoAnterior"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreProcesoAnterior"]),
                },
                ProcesoActual = new GLPI_ProcesoDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdProcesoActual"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreProcesoActual"]),
                },
                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
            };
        }

        private GLPI_SeguimientoDto ConstruirObjetoDto1(SqlDataReader dr) {
            return new GLPI_SeguimientoDto {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                UsuarioRegistra = new GLPI_UsuarioDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdUsuarioRegistra"]),
                    Nombres = ManejoNulos.ManageNullStr(dr["NombreUsuarioRegistra"]),
                    ApellidoPaterno = ManejoNulos.ManageNullStr(dr["ApellidoPaternoUsuarioRegistra"]),
                    ApellidoMaterno = ManejoNulos.ManageNullStr(dr["ApellidoMaternoUsuarioRegistra"]),
                    Cargo = ManejoNulos.ManageNullStr(dr["CargoUsuarioRegistra"]),
                    NumeroDocumento = ManejoNulos.ManageNullStr(dr["NumeroDocumentoUsuarioRegistra"]),
                },
                EstadoTicketAnterior = new GLPI_EstadoTicketDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdEstadoTicketAnterior"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreEstadoTicketAnterior"]),
                },
                EstadoTicketActual = new GLPI_EstadoTicketDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdEstadoTicketActual"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreEstadoTicketActual"]),
                },
                Correos = ManejoNulos.ManageNullStr(dr["Correos"]),
                ProcesoAnterior = new GLPI_ProcesoDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdProcesoAnterior"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreProcesoAnterior"]),
                },
                ProcesoActual = new GLPI_ProcesoDto {
                    Id = ManejoNulos.ManageNullInteger(dr["IdProcesoActual"]),
                    Nombre = ManejoNulos.ManageNullStr(dr["NombreProcesoActual"]),
                },
                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
            };
        }

        private GLPI_SeguimientoTicket ConstruirObjeto(SqlDataReader dr) {
            return new GLPI_SeguimientoTicket {
                Id = ManejoNulos.ManageNullInteger(dr["Id"]),
                IdUsuarioRegistra = ManejoNulos.ManageNullInteger(dr["IdUsuarioRegistra"]),
                IdEstadoTicketAnterior = ManejoNulos.ManageNullInteger(dr["IdEstadoTicketAnterior"]),
                IdEstadoTicketActual = ManejoNulos.ManageNullInteger(dr["IdEstadoTicketActual"]),
                Descripcion = ManejoNulos.ManageNullStr(dr["Descripcion"]),
                IdProcesoAnterior = ManejoNulos.ManageNullInteger(dr["IdProcesoAnterior"]),
                IdProcesoActual = ManejoNulos.ManageNullInteger(dr["IdProcesoActual"]),
                Correos = ManejoNulos.ManageNullStr(dr["Correos"]),
                FechaRegistro = ManejoNulos.ManageNullDate(dr["FechaRegistro"]),
            };
        }
    }
}
