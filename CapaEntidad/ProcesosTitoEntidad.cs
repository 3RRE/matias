using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ProcesosTitoEntidad
    {
        public int PROCC_Cod_Proceso { get; set; }
        public int PROCC_Id_Tipo_Proceso { get; set; }
        public DateTime PROCC_Fecha_Apertura { get; set; }
        public DateTime PROCC_Fecha_Proceso { get; set; }
        public DateTime PROCC_Fecha_Emision { get; set; }
        public int PROCC_Id_Tipo_Documento { get; set; }
        public string PROCC_Nro_Documento { get; set; }
        public int PROCC_Cod_Cliente { get; set; }
        public string PROCC_Cod_Ticket { get; set; }
        public string PROCC_Nro_Maquina { get; set; }
        public string PROCC_Cod_Tarjeta { get; set; }
        public string PROCC_Desc_Concepto { get; set; }
        public double PROCC_Monto_Dinero { get; set; }
        public double PROCC_Igv { get; set; }
        public double PROCC_Sub_Total { get; set; }
        public double PROCC_Total { get; set; }
        public double PROCC_ImpuestoPM { get; set; }
        public string PROCC_Letras { get; set; }
        public int PROCC_Id_TipoPago { get; set; }
        public string PROCC_desc_TipoPago { get; set; }
        public string PROCC_Nro_Tarjeta_Credito { get; set; }
        public int PROCC_idTipoMoneda { get; set; }
        public double PROCC_Tipo_Cambio { get; set; }
        public string PROCC_Estado_Transaccion { get; set; }
        public string PROCC_Usuario { get; set; }
        public int PROCC_Item_Caja { get; set; }
        public string PROCC_Serie_Doc { get; set; }
        public string PROCC_Nro_Doc { get; set; }
        public DateTime PROCC_Fecha_Anulacion { get; set; }
        public string procc_razon_social { get; set; }
        public string procc_ruc { get; set; }
        public int procc_nro_caja { get; set; }
        public int PROCC_CODPER_ACCESO { get; set; }
        public string PROCC_OBSERVACION { get; set; }
        public double MontoNoCobrablePromo { get; set; }
        public string procc_N_Comprobante { get; set; }
        public int Procc_ModalidadPago { get; set; }
        public int IdTipoPago { get; set; }
        public double PROCC_MONTO_DOLARES { get; set; }
        public int procc_tipo_cliente_destino { get; set; }
        public int PROCC_idTipoMoneda_paga { get; set; }
        public double PROCC_monto_paga { get; set; }
        public string procc_motivo_pm { get; set; }
        public string procc_tipo_venta_ini { get; set; }
    }
}
