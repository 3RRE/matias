using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IASServiceClient
{

    public class eMail
    {
        private readonly LogError _log = new LogError();
        private readonly MailMessage _msg = new MailMessage();
        public string NombreMetodo;
        public string NombreEmpresa;
        public string NombreSala;
        public string TipoAlerta;
        public string Destinatarios;
        public string Origen;
        public string Host;
        public string Clave;
        public string Port;
        public string De ;
        public string Nombre;
        public bool Ssl1;
        public DateTime FechaRegistro;
        public int EnviaCorreo;

        public eMail()
        {

            Destinatarios = ConfigurationManager.AppSettings["destinatarios"];
            Origen = ConfigurationManager.AppSettings["origen"];
            Host = ConfigurationManager.AppSettings["host"];
            Clave = ConfigurationManager.AppSettings["clave"];
            Port = ConfigurationManager.AppSettings["port"];
            De = ConfigurationManager.AppSettings["de"];
            Nombre = ConfigurationManager.AppSettings["nombre"];
            Ssl1 = Convert.ToBoolean(ConfigurationManager.AppSettings["ssl1"]);
        }


       

        public async System.Threading.Tasks.Task<bool> NotificacionBolsaChica(string RazonSocial, string Sala, string Item, string Tito_NroTicket, string Tito_NroTicket_Ant, string Tito_MontoTicket, string Tito_MontoTicket_Ant, string Tito_MTicket_NoCobrable, string Tito_MTicket_NoCobrable_Ant, string Estado, string Estado_Ant, string PuntoVenta, string PuntoVenta_Ant)
        { 
            try
            {
                var contenido = @"<div>MODIFICACION BOLSA CHICA / " + RazonSocial.ToUpper() + " - " + Sala.ToUpper() + " <p></div>" +
                    " <table width= 50% bgcolor= #f6f8f1 border=0 cellpadding=0 cellspacing=0>   " +

                    "<tr> " +
                    "<tr> " +
                    " <td style= 'background: #69c3c3'; 'font - size: 24'>   </td> " +
                    " <td style= 'background: #69c3c3'; 'font - size: 24'> ANTES </td>" +
                    " <td style= 'background: #69c3c3'; 'font - size: 24'> DESPUES </td>" +
                    " </tr>  " +
                    " <td style= 'background: #69c3c3'; 'font - size: 24'> Tito_NroTicket </td> " +
                    " <td style= 'background: #edbff'; 'font - size: 24'> " + Tito_NroTicket_Ant + " </td> " +
                    " <td style= 'background: #ecc9c9'; 'font - size: 24'> " + Tito_NroTicket + " </td> " +
                    " </tr>  " + " " +
                   " </tr>  " +
                    " <td style= 'background: #69c3c3'; 'font - size: 24'> Tito_MontoTicket </td> " +
                    " <td style= 'background: #edbff'; 'font - size: 24'> " + Tito_MontoTicket_Ant + " </td> " +
                    " <td style= 'background: #ecc9c9'; 'font - size: 24'> " + Tito_MontoTicket + " </td> " +
                    " </tr>  " +
                    "<tr> " +
                    " <td style= 'background: #69c3c3'; 'font - size: 24'> Tito_MTicket_NoCobrable </td> " +
                    " <td style= 'background: #edbff'; 'font - size: 24'> " + Tito_MTicket_NoCobrable_Ant + " </td> " +
                    " <td style= 'background: #ecc9c9'; 'font - size: 24'> " + Tito_MTicket_NoCobrable + " </td> " +
                    " </tr>  " + "<tr> " +
                    " <td style= 'background: #69c3c3'; 'font - size: 24'> Estado </td> " +
                    " <td style= 'background: #edbff'; 'font - size: 24'> " + Estado_Ant + " </td> " +
                    " <td style= 'background: #ecc9c9'; 'font - size: 24'> " + Estado + " </td> " +
                    " </tr>  " + "<tr> " +
                    " <td style= 'background: #69c3c3'; 'font - size: 24'> PuntoVenta </td> " +
                    " <td style= 'background: #edbff'; 'font - size: 24'> " + PuntoVenta_Ant + " </td> " +
                    " <td style= 'background: #ecc9c9'; 'font - size: 24'> " + PuntoVenta + " </td> " +
                    " </tr>  " +
                    " </table> " +
                    "<div><br> </br> <font size = 2> <font color = #0f243e >Atte.</font><br><font color= #0f243e>" +
                               " Adm. Sys IAS.</font><p>" +
                               "</font></div> <div><font size= 2 ><font color= #0f243e >no responder a este mensaje, esta cuenta no es monitoreada.</font></font></div> ";


                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Origen);
                mail.To.Add(Destinatarios);
                mail.Subject = NombreEmpresa + "IAS MODIFICACION BOLSA CHICA"; 
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = contenido;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;

                SmtpClient client = new SmtpClient
                {
                    Credentials = new System.Net.NetworkCredential(Origen, Clave),
                    Port = Convert.ToInt32(Port),
                    EnableSsl = Ssl1,
                    Host = Host
                };
                try
                {
                    await Task.Run(() =>
                    {
                        client.SendCompleted += (s, e) => {
                            client.Dispose();
                            mail.Dispose();
                        };
                        client.SendAsync(mail, null);
                    });
                    return true;
                }
                catch (Exception ex)
                {
                    var targetSite = ex.TargetSite.Name + ex.TargetSite.Module;
                    var source = ex.Source.ToString();
                    var message = ex.Message.ToString();
                    throw;
                } 
            }
            catch (Exception ex)
            { 
                Console.WriteLine("Unable to send email. Error : " + ex);
                return false;
            } 
            
        }
    }
}
