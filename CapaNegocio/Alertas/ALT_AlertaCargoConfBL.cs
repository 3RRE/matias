using CapaDatos.Alertas;
using CapaEntidad.Alertas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Alertas
{
    public class ALT_AlertaCargoConfBL
    {
        private ALT_AlertaCargoConfDAL alertaCargoDal = new ALT_AlertaCargoConfDAL();

        public List<ALT_AlertaCargoConfEntidad> ALT_AlertaCargoConf_Listado()
        {
            return alertaCargoDal.ALT_AlertaCargoConf_Listado();
        }

        public List<ALT_AlertaCargoConfEntidad> ALT_AlertaCargoxSala_Listado(string codsala)
        {
            return alertaCargoDal.ALT_AlertaCargoxSala_Listado(codsala);
        }

        public List<ALT_AlertaCargoConfEntidad> ALT_AlertaCargoxSala_idListado(int codsala)
        {
            return alertaCargoDal.ALT_AlertaCargoxSala_idListado(codsala);
        }
        public ALT_AlertaCargoConfEntidad ALT_AlertaCargoConf_IdObtenerJson(Int64 id)
        {
            return alertaCargoDal.ALT_AlertaCargoConf_IdObtenerJson(id);
        }

        public int ALT_AlertaCargoConfInsertarJson(ALT_AlertaCargoConfEntidad alerta, int tipo)
        {
            return alertaCargoDal.ALT_AlertaCargoConfInsertarJson(alerta, tipo);
        }
        public bool ALT_AlertaCargoConfEliminarJson(int id)
        {
            return alertaCargoDal.ALT_AlertaCargoConfEliminarJson(id);
        }
        public bool EliminarCargoAlertaSala(int sala_id, int cargo_id)
        {
            return alertaCargoDal.EliminarCargoAlertaSala(sala_id, cargo_id);
        }
    }
}
