using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.MaquinasInoperativas {
    public class MI_MaquinaInoperativaEntidad {

        public int CodMaquinaInoperativa { get; set; }
        public int CodSala { get; set; }
        public int CodMaquina { get; set; }
        public string MaquinaLey { get; set; }
        public string MaquinaModelo { get; set; }
        public string MaquinaLinea { get; set; }
        public string MaquinaSala { get; set; }
        public string MaquinaJuego { get; set; }
        public string MaquinaNumeroSerie { get; set; }
        public string MaquinaPropietario { get; set; }
        public string MaquinaFicha { get; set; }
        public string MaquinaMarca { get; set; }
        public string MaquinaToken { get; set; }
        public string TecnicoCreado { get; set; }
        public string TecnicoAtencion { get; set; }
        public string ObservacionCreado { get; set; }
        public string ObservacionAtencion { get; set; }
        public int CodEstadoInoperativa { get; set; }
        public int CodPrioridad { get; set; }
        public DateTime FechaInoperativa { get; set; }
        public DateTime FechaCreado { get; set; }
        public DateTime? FechaAtendidaOperativa { get; set; }
        public DateTime? FechaAtendidaInoperativa { get; set; }
        public DateTime FechaAtendidaInoperativaSolicitado { get; set; }
        public DateTime FechaAtendidaInoperativaAprobado { get; set; }
        public int CodUsuarioCreado { get; set; }
        public int CodUsuarioAtendidaOperativa { get; set; }
        public int CodUsuarioAtendidaInoperativa { get; set; }
        public int CodUsuarioAtendidaInoperativaSolicitado { get; set; }
        public int CodUsuarioAtendidaInoperativaAprobado { get; set; }
        public int CodEstadoProceso { get; set; }

        //Plus Maquina Inoperativa
        public string NombreSala { get; set; }
        public string NombreUsuarioCreado { get; set; }
        public string NombreUsuarioAtendidaOperativa { get; set; }
        public string NombreUsuarioAtendidaInoperativa { get; set; }
        public string NombreUsuarioAtendidaInoperativaSolicitado { get; set; }
        public string NombreUsuarioAtendidaInoperativaAprobado { get; set; }

        public string NombreProblema { get; set; }
        public string NombreCategoriaProblema { get; set; }



        //add

        public string IST { get; set; }
        public string ObservacionAtencionNuevo { get; set; }
        public int CodEstadoReparacion { get; set; }
        public string OrdenCompra { get; set; }
        public DateTime FechaOrdenCompra { get; set; }
        public string FechaOrdenCompraStr {  get; set; }
        // add reporte kpi
        public string NombreRepuesto { get; set; }
        public string StockResuelto { get; set; }
        public double PresupuestoRepuesto { get; set; }
        public int DiasInoperativos { get; set; }

        public string NombreZona { get; set; }
    }
}
