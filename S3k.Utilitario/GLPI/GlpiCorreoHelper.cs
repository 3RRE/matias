using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace S3k.Utilitario.GLPI {
    public class GlpiCorreoHelper {
        private static readonly string DELIMITADOR_CORREO = ",";
        private static readonly Regex EmailRegex = new Regex(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        public static string FormarCorreos(List<string> correos) {
            if(correos == null || !correos.Any()) {
                return string.Empty;
            }

            List<string> correosLimpios = correos
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .Distinct()
                .ToList();

            return string.Join(DELIMITADOR_CORREO.ToString(), correosLimpios);
        }

        public static List<string> FormarCorreos(string correosStr) {
            if(string.IsNullOrWhiteSpace(correosStr)) {
                return new List<string>();
            }

            return correosStr
                .Split(new[] { DELIMITADOR_CORREO }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .Where(c => !string.IsNullOrWhiteSpace(c) && EsCorreoValido(c))
                .Distinct()
                .ToList();
        }

        public static List<string> ValidarCorreos(List<string> correos) {
            if(correos == null) {
                return new List<string>();
            }

            return correos
                .Select(c => c.Trim())
                .Where(c => !string.IsNullOrWhiteSpace(c) && EsCorreoValido(c))
                .Distinct()
                .ToList();
        }

        private static bool EsCorreoValido(string correo) {
            return EmailRegex.IsMatch(correo);
        }
    }
}
