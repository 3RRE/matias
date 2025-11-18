using CapaDatos.MaquinasInoperativas;
using CapaEntidad.MaquinasInoperativas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.MaquinasInoperativas
{
    public class MI_SalaCorreosBL
    {
        MI_SalaCorreosDAL capaDatos = new MI_SalaCorreosDAL();
        public List<MI_SalaCorreosEntidad> GetAllSalaCorreos()
        {
            var lista = capaDatos.GetAllSalaCorreos();
            return lista;
        }
        public List<MI_SalaCorreosEntidad> GetCorreosxSala(int codSala)
        {
            var lista = capaDatos.GetCorreosxSala(codSala);
            return lista;
        }
        public bool AgregarCorreoSala(int codSala, int codUsuario,int codTipo)
        {
            var status = capaDatos.AgregarCorreoSala(codSala, codUsuario, codTipo);
            return status;
        }
        public bool QuitarCorreoSala(int codSalaCorreos)
        {
            var status = capaDatos.QuitarCorreoSala(codSalaCorreos);
            return status;
        }

        public List<MI_SalaCorreosEntidad> GetAllUsuarioCorreos()
        {
            var lista = capaDatos.GetAllUsuarioCorreos();
            return lista;
        }
    }
}
