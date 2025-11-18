using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class SEG_AUDITORIABL
    {
        private SEG_AuditoriaDAL AUDITORIA_DAL = new SEG_AuditoriaDAL();

        public bool Guardar(SEG_AUDITORIA auditoriaobj)
        {
            return AUDITORIA_DAL.Guardar(auditoriaobj);
        }

        public List<SEG_AUDITORIA> GetAllRangoFechas(DateTime fecha, DateTime fecha2)
        {
            return AUDITORIA_DAL.GetAllRangoFechas(fecha, fecha2);
        }

        public SEG_AUDITORIA getDataAuditoria(int sub)
        {
            return AUDITORIA_DAL.getDataAuditoria(sub);
        }

    }
}
