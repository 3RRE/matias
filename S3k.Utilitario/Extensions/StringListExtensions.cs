using System;
using System.Collections.Generic;
using System.Linq;

namespace S3k.Utilitario.Extensions {
    public static class StringListExtensions {
        private const char DefaultDelimiter = ',';

        /// <summary>
        /// Convierte una lista de strings en un solo string delimitado.
        /// </summary>
        public static string ToDelimitedString(this IEnumerable<string> values, char delimiter = DefaultDelimiter) {
            if(values == null || !values.Any())
                return string.Empty;

            return values
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => v.Trim())
                .Distinct()
                .Aggregate((a, b) => $"{a}{delimiter}{b}");
        }

        /// <summary>
        /// Convierte una lista de enteros en un solo string delimitado.
        /// </summary>
        public static string ToDelimitedString(this IEnumerable<int> values, char delimiter = DefaultDelimiter) {
            if(values == null || !values.Any())
                return string.Empty;

            return values
                .Select(v => v.Trim())
                .Distinct()
                .Aggregate((a, b) => $"{a}{delimiter}{b}");
        }

        /// <summary>
        /// Convierte un string delimitado en una lista de strings.
        /// </summary>
        public static List<string> ToCleanedList(this string delimitedString, char delimiter = DefaultDelimiter) {
            if(string.IsNullOrWhiteSpace(delimitedString))
                return new List<string>();

            return delimitedString
                .Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim())
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Convierte un string delimitado en una lista de enteros.
        /// </summary>
        public static List<int> ToCleanedIntList(this string delimitedString, char delimiter = DefaultDelimiter) {
            if(string.IsNullOrWhiteSpace(delimitedString))
                return new List<int>();

            return delimitedString
                .Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim())
                .Where(v => int.TryParse(v, out _)) // Filtra solo los valores que se pueden convertir a int
                .Select(int.Parse)
                .Distinct()
                .ToList();
        }

        public static string JoinConjuncion(this List<string> lista, string separador = ", ") {
            if(lista == null || lista.Count == 0)
                return string.Empty;

            if(lista.Count == 1)
                return lista[0];

            // Detectar conjunción
            string ultimo = lista.Last();
            string conjuncion = NecesitaE(ultimo) ? " e " : " y ";

            if(lista.Count == 2)
                return lista[0] + conjuncion + ultimo;

            return string.Join(separador, lista.Take(lista.Count - 1)) + conjuncion + ultimo;
        }

        private static bool NecesitaE(string palabra) {
            if(string.IsNullOrWhiteSpace(palabra)) return false;
            char inicial = char.ToLower(palabra.Trim()[0]);
            // "e" se usa si la palabra empieza con "i" o "hi" (ejemplo: "hijos", "iglesia")
            return inicial == 'i' || palabra.TrimStart().ToLower().StartsWith("hi");
        }
    }
}
