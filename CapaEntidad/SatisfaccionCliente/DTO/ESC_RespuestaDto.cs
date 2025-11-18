using CapaEntidad.SatisfaccionCliente.DTO.Mantenedores;
using CapaEntidad.SatisfaccionCliente.Enum;
using S3k.Utilitario.Extensions;
using System;

namespace CapaEntidad.SatisfaccionCliente.DTO {
    public class ESC_RespuestaDto {
        public int Id { get; set; }
        public ESC_PreguntaDto Pregunta { get; set; } = new ESC_PreguntaDto();
        public ESC_Puntaje Puntaje { get; set; }
        public string PuntajeStr { get => Puntaje.GetDisplayText(); }
        public ESC_ClienteDto Cliente { get; set; } = new ESC_ClienteDto();
        public DateTime FechaRegistro { get; set; }

        public bool Existe() {
            return Id > 0;
        }
    } 
}
