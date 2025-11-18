using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
   public class Destinatario_DetalleBL
    {
        private Destinatario_DetalleDAL destinatarioDetalleDal = new Destinatario_DetalleDAL();                                      
        public bool DestinatarioDetalleInsertarJson(Destinatario_DetalleEntidad destinatario)
        {
            return destinatarioDetalleDal.DestinatarioDetalleInsertarJson(destinatario);
        }
        public bool DestinatarioDetalleEliminarJson(int tipoEmail)
        {
            return destinatarioDetalleDal.DestinatarioDetalleEliminarJson(tipoEmail);
        }        
    }
}
