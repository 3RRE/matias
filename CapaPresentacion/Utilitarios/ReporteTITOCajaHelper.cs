using CapaEntidad.AsistenciaCliente;
using CapaEntidad.TITO;
using CapaNegocio.AsistenciaCliente;
using System.Collections.Generic;
using System.Linq;

namespace CapaPresentacion.Utilitarios {
    public class ReporteTITOCajaHelper
    {
        public static List<Reporte_Detalle_TITO_Caja> ChangeDataCustomers(List<Reporte_Detalle_TITO_Caja> listReport)
        {
            AST_ClienteBL astClienteBL = new AST_ClienteBL();

            List<string> numberDocuments = listReport.Where(item => !string.IsNullOrEmpty(item.ClienteDni.Trim())).Select(item => item.ClienteDni.Trim()).Distinct().ToList();
            List<AST_ClienteEntidad> customers = astClienteBL.GetListaMasivoClientesxNroDoc(numberDocuments);

            listReport.Where(item => !string.IsNullOrEmpty(item.ClienteDni.Trim())).ToList().ForEach(item => {

                AST_ClienteEntidad customer = customers.Where(customerItem => customerItem.NroDoc.Trim().Equals(item.ClienteDni.Trim())).FirstOrDefault();

                if(customer != null)
                {
                    item.ClienteCodigo = customer.Id;
                    item.ClienteTelefono = string.IsNullOrEmpty(item.ClienteTelefono.Trim()) ? customer.Celular1.Trim() : item.ClienteTelefono.Trim();
                    item.ClienteCorreo = string.IsNullOrEmpty(item.ClienteCorreo.Trim()) || item.ClienteCorreo.Trim().Equals("CORREO@PROVEEDOR.COM") ? customer.Mail.Trim() : item.ClienteCorreo.Trim();
                }

            });

            return listReport;
        }
    }
}