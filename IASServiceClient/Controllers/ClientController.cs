using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;

namespace IASServiceClient.Controllers
{
    public class ClientController : ApiController
    {
        private readonly eMail _eMail = new eMail();
        [Route("api/data/getfirstmacaddress")]
        public string GetMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }

        [Route("api/data/getallmacaddress")]
        public List<string> GetAllMacAddress() {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            List<string> macs = new List<string>();
            foreach(NetworkInterface adapter in nics) {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                string sMacAddress = adapter.GetPhysicalAddress().ToString();
                macs.Add(sMacAddress);
            }
            return macs;
        }

        [Route("api/data/getmacaddress")]
        public string GetCurrentMacAddress() {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            string currentMac = string.Empty;

            foreach(NetworkInterface adapter in nics) {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                string status = adapter.OperationalStatus.ToString();
                string mac = adapter.GetPhysicalAddress().ToString();
                if(status.Equals("up", StringComparison.OrdinalIgnoreCase) && !String.IsNullOrEmpty(mac)) {
                    currentMac = mac;
                    break;
                }
            }
            return currentMac;
        }

        [Route("api/data/prueba")] 
        public void NotificacionIASmodificacionBolsaChicas(string RazonSocial, string Sala)
        {

            //0 ANULADO   ,   1 COBRABLEv
            //var x = 2;
        }

        [Route("api/data/ias")]
        public async Task NotificacionIASmodificacionBolsaChica(string RazonSocial , string Sala, string Item , string Tito_NroTicket, string Tito_NroTicket_Ant, string Tito_MontoTicket, string Tito_MontoTicket_Ant , string Tito_MTicket_NoCobrable , string Tito_MTicket_NoCobrable_Ant, string Estado , string Estado_Ant, string PuntoVenta , string PuntoVenta_Ant  )
        {    
            if((Tito_NroTicket != Tito_NroTicket_Ant) || (Tito_MontoTicket != Tito_MontoTicket_Ant) ||(Tito_MTicket_NoCobrable != Tito_MTicket_NoCobrable_Ant)|| (Estado !=  Estado_Ant))
            {
                //0 ANULADO   ,   1 COBRABLEv
                await _eMail.NotificacionBolsaChica(RazonSocial, Sala, Item, Tito_NroTicket, Tito_NroTicket_Ant, Tito_MontoTicket, Tito_MontoTicket_Ant, Tito_MTicket_NoCobrable, Tito_MTicket_NoCobrable_Ant, Estado, Estado_Ant, PuntoVenta, PuntoVenta_Ant);
            } 
        }
    }
}
