using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad {
    public class MaquinasCentralizadasEntidad {

        public int id_maquina { get; set; }
        public int id_sala { get; set; }
        public string sala { get; set; }
        public string tipo_maquina { get; set; }
        public string codigo { get; set; }
        public string serie { get; set; }
        public string zona { get; set; }
        public string isla { get; set; }
        public string posicion { get; set; }
        public string marca { get; set; }
        public string modelo_comercial { get; set; }
        public string codigo_modelo { get; set; }
        public string juego { get; set; }
        public string progresivo { get; set; }
        public string propietario { get; set; }
        public string propiedad { get; set; }
        public double porcentaje_teorico { get; set; }
        public string tipo_contrato { get; set; }
        public string estado { get; set; }

        //NUEVOS CAMPOS

        public DateTime fecha_ini { get; set; }
        public DateTime fecha_fin { get; set; }
        public string moneda { get; set; }
        public string medio_juego { get; set; }
        public double token { get; set; }
        public double dias_trabajados { get; set; }
        public double coin_in { get; set; }
        public double a_coin_in { get; set; }
        public double win { get; set; }
        public double media { get; set; }
        public double hold { get; set; }
        public double avbet_x_game { get; set; }
        public double games_played { get; set; }
        public double tiempo_juego { get; set; }
        public string mistery { get; set; }
        public string tipo_progresivo { get; set; }
        public string pozos { get; set; }
        public string rtp_retorno_teorico { get; set; }
        public string incremento_progresivo { get; set; }
    }

    public class ResponseMaquinasCentralizadasEntidad {

        public bool respuesta { get; set; }
        public List<MaquinasCentralizadasEntidad> data { get; set; }
    }

}
