using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class UsuarioEncriptacionEntidad
    {
        public int Id { get; set; }
        public int EmpleadoId { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioPassword { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class TecnicoUsuarioEncriptado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class TecnicoEncriptacion
    {
        public int TecnicoId { get; set; }
        public int NivelTecnicoID { get; set; }
        public int EmpleadoID { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioPassword { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Estado { get; set; }
        public int IdUsuarioEncriptacion { get; set; }
    }
}
