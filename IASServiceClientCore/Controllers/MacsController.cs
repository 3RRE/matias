using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace IASServiceClientCore.Controllers;
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MacsController : ControllerBase {
    [HttpGet]
    [Route("/api/data/getfirstmacaddress")]
    public ActionResult<string> GetFirstMacAddress() {
        string macAddress = NetworkInterface.GetAllNetworkInterfaces()
            .Select(adapter => adapter.GetPhysicalAddress().ToString())
            .FirstOrDefault(mac => !string.IsNullOrEmpty(mac)) ?? string.Empty;

        return Ok(macAddress);
    }

    [HttpGet]
    [Route("/api/data/getallmacaddress")]
    public ActionResult<List<string>> GetAllMacAddress() {
        List<string> macs = [];

        foreach(NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces()) {
            string macAddress = adapter.GetPhysicalAddress().ToString();
            if(!string.IsNullOrEmpty(macAddress)) {
                macs.Add(macAddress);
            }
        }

        return Ok(macs);
    }

    [HttpGet]
    [Route("/api/data/getmacaddress")]
    public ActionResult<string> GetCurrentMacAddress() {
        string macAddress = NetworkInterface.GetAllNetworkInterfaces()
            .Where(adapter => adapter.OperationalStatus == OperationalStatus.Up && adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .Select(adapter => adapter.GetPhysicalAddress().ToString())
            .FirstOrDefault() ?? string.Empty;

        return Ok(macAddress);
    }
}
