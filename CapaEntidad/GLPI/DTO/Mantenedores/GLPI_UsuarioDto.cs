using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaEntidad.GLPI.DTO.Mantenedores {
    public class GLPI_UsuarioDto {
        public int Id { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string CorreoPersonal { get; set; } = string.Empty;
        public string CorreoTrabajo { get; set; } = string.Empty;

        public string ObtenerNombreCompleto() {
            return $"{Nombres} {ApellidoPaterno} {ApellidoMaterno}".Trim();
        }

        private bool EsCorreoValido(string correo) {
            if(string.IsNullOrWhiteSpace(correo)) return false;

            string patronCorreo = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(correo, patronCorreo);
        }

        public bool TieneCorreoValido() {
            return EsCorreoValido(CorreoTrabajo) || EsCorreoValido(CorreoPersonal);
        }

        public List<string> ObtenerCorreosValidos() {
            List<string> correosValidos = new List<string>();

            if(EsCorreoValido(CorreoTrabajo)) correosValidos.Add(CorreoTrabajo);
            if(EsCorreoValido(CorreoPersonal)) correosValidos.Add(CorreoPersonal);

            return correosValidos;
        }
    }
}
