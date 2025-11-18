using CapaEntidad.GLPI.DTO.Global;
using CapaEntidad.GLPI.DTO.Mantenedores;
using CapaEntidad.GLPI.Enum;
using S3k.Utilitario.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace CapaEntidad.GLPI.DTO {
    public class GLPI_TicketDto {
        private readonly string PathWebArchivos;

        public GLPI_TicketDto() {
            PathWebArchivos = ConfigurationManager.AppSettings["PathWebArchivos"];
        }

        public int Id { get; set; }
        public GLPI_SalaDto Sala { get; set; } = new GLPI_SalaDto();
        public GLPI_UsuarioDto UsuarioSolicitante { get; set; } = new GLPI_UsuarioDto();
        public GLPI_TipoOperacionDto TipoOperacion { get; set; } = new GLPI_TipoOperacionDto();
        public GLPI_NivelAtencionDto NivelAtencion { get; set; } = new GLPI_NivelAtencionDto();
        public GLPI_SubCategoriaDto SubCategoria { get; set; } = new GLPI_SubCategoriaDto();
        public GLPI_ClasificacionProblemaDto ClasificacionProblema { get; set; } = new GLPI_ClasificacionProblemaDto();
        public GLPI_EstadoActualDto EstadoActual { get; set; } = new GLPI_EstadoActualDto();
        public GLPI_IdentificadorDto Identificador { get; set; } = new GLPI_IdentificadorDto();
        public GLPI_AsignacionDto Asignacion { get; set; } = new GLPI_AsignacionDto();
        public List<GLPI_SeguimientoDto> Seguimientos { get; set; } = new List<GLPI_SeguimientoDto>();
        public GLPI_CierreDto Cierre { get; set; } = new GLPI_CierreDto();
        public string Descripcion { get; set; } = string.Empty;
        public string Adjunto { get; set; } = string.Empty;
        public string AdjuntoFullPath { get => $"{PathWebArchivos}/{Adjunto}"; }
        public string Correos { get; set; } = string.Empty;
        public GLPI_FaseTicket CodigoFaseTicket { get; set; }
        public string FaseTicket { get => CodigoFaseTicket.GetDisplayText(); }
        public DateTime FechaRegistro { get; set; }
        public string FechaRegistroStr { get => FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss"); }

        public bool Existe() {
            return Id > 0;
        }
    }
}
