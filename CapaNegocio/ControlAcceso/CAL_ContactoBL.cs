using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ControlAcceso
{
    public class CAL_ContactoBL
    {
        CAL_ContactoDAL capaDatos = new CAL_ContactoDAL();

        public int InsertarContacto(CAL_ContactoEntidad Entidad)
        {
            return capaDatos.InsertarContacto(Entidad);
        }
        public bool UpdateContacto(CAL_ContactoEntidad Entidad)
        {
            return capaDatos.UpdateContacto(Entidad);
        }
        public bool UpdateContactoInLudopatas(int idcontacto, int idludopata)
        {
            return capaDatos.UpdateContactoInLudopatas(idcontacto,idludopata);
        }
        public CAL_ContactoEntidad GetContactoByID(int id)
        {
            return capaDatos.GetContactoByID(id);
        }
        public bool EliminarContacto(int id)
        {
            return capaDatos.EliminarContacto(id);
        }
        
    }
}
