using CapaEntidad.ControlAcceso;
using Emgu.CV.Structure;
using Google.Apis.Drive.v3.Data;
using OfficeOpenXml.DataValidation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers.ControlAcceso
{
    [seguridad]
    public class CALLogApiReniecController : Controller
    {
        // GET: CALLogApiReniec
        public ActionResult ListadoLogApiReniec()
        {
            return View("~/Views/ControlAcceso/ListadoLogApiReniec.cshtml");
        }


        [seguridad(false)]
        [HttpPost]
        public ActionResult ListarAllLogApiReniecJson(int channel,DateTime fecha)
        {
            var errormensaje = "";
            var lista = new List<CAL_LogApiReniecEntidad>();
            var listaEstado = new List<String>();
            try
            {
                lista = LeerLogxChannel(channel, fecha);
                listaEstado = ListarEstados(channel,fecha);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            
            //Json grande xd

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new
            {
                listaEstado,
                data = lista
            };
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;


            //return Json(new { data = lista.ToList(), mensaje = errormensaje }, JsonRequestBehavior.AllowGet);
        }


        [seguridad(false)]
        public List<String> ListarEstados (int channel, DateTime dateTime)
        {
            string month = dateTime.ToString("MM");
            string year = dateTime.ToString("yyyy");
            string fileDate = $"{month}-{year}";
            string fileExtension = "log";

            var lista = new List<String>();

            if (channel == 0 || channel == 1)
            {
                //web
                string channelName = "web";
                string fileName = $"apireniec-{channelName}";
                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\logs\\{fileName}-{fileDate}.{fileExtension}";

                String line;

                try
                {
                    StreamReader sr = new StreamReader(filePath);

                    line = sr.ReadLine();

                    while (line != null)
                    {
                        bool repetido = false;
                        string[] data = line.Split('|');

                        foreach(var item in lista)
                        {
                            if (data[4].ToUpper().Equals(item.ToUpper()))
                            {
                                repetido = true;
                                break;
                            }
                        }

                        if(!repetido)
                        lista.Add(data[4]);

                        line = sr.ReadLine();
                    }

                    sr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Programa terminado.");
                    Console.WriteLine("Presione cualquier botón para cerrar la ventana.");
                }

            }
            if (channel == 0 || channel == 2)
            {

                //app
                string channelName = "app";
                string fileName = $"apireniec-{channelName}";
                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\logs\\{fileName}-{fileDate}.{fileExtension}";

                String line;

                try
                {
                    StreamReader sr = new StreamReader(filePath);

                    line = sr.ReadLine();

                    while (line != null)
                    {
                        bool repetido = false;
                        string[] data = line.Split('|');

                        foreach (var item in lista)
                        {
                            if (data[4].ToUpper().Equals(item.ToUpper()))
                            {
                                repetido = true;
                                break;
                            }
                        }

                        if (!repetido)
                            lista.Add(data[4]);

                        line = sr.ReadLine();
                    }

                    sr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Programa terminado.");
                    Console.WriteLine("Presione cualquier botón para cerrar la ventana.");
                }
            }

            return lista;
        }

        [seguridad(false)]
        public List<CAL_LogApiReniecEntidad> LeerLogxChannel(int channel, DateTime dateTime)
        {

            string month = dateTime.ToString("MM");
            string year = dateTime.ToString("yyyy");
            string fileDate = $"{month}-{year}";
            string fileExtension = "log";

            var lista = new List<CAL_LogApiReniecEntidad>();

            if (channel ==0 || channel == 1)
            {
                //web
                string channelName = "web";
                string fileName = $"apireniec-{channelName}";
                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\logs\\{fileName}-{fileDate}.{fileExtension}";

                String line;

                try
                {
                    StreamReader sr = new StreamReader(filePath);

                    line = sr.ReadLine();

                    while (line != null)
                    {
                        string[] data = line.Split('|');

                        var item = new CAL_LogApiReniecEntidad();
                        item.FechaRegistro = Convert.ToDateTime(data[0]);
                        item.Sala = data[1];
                        item.Usuario = data[2];
                        item.NroDoc = data[3];
                        item.Estado = data[4];

                        lista.Add(item);
                        line = sr.ReadLine();
                    }

                    sr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Programa terminado.");
                    Console.WriteLine("Presione cualquier botón para cerrar la ventana.");
                }

            }
            if (channel == 0 || channel == 2)
            {

                //app
                string channelName = "app";
                string fileName = $"apireniec-{channelName}";
                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\logs\\{fileName}-{fileDate}.{fileExtension}";

                String line;

                try
                {
                    StreamReader sr = new StreamReader(filePath);

                    line = sr.ReadLine();

                    while (line != null)
                    {
                        string[] data = line.Split('|');

                        var item = new CAL_LogApiReniecEntidad();
                        item.FechaRegistro = Convert.ToDateTime(data[0]);
                        item.Sala = data[1];
                        item.Usuario = data[2];
                        item.NroDoc = data[3];
                        item.Estado = data[4];

                        lista.Add(item);
                        line = sr.ReadLine();
                    }

                    sr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {
                    Console.WriteLine("Programa terminado.");
                    Console.WriteLine("Presione cualquier botón para cerrar la ventana.");
                }
            }

            return lista;

        }


        [seguridad(false)]
        public ActionResult ListarAllLogApiReniecFiltroFechasJson(int channel, DateTime fechaIni, DateTime fechaFin) {

            var errormensaje = "";
            var lista = new List<CAL_LogApiReniecEntidad>();
            var listaEstado = new List<String>();
            var listaSala = new List<String>();


            int añoIni = fechaIni.Year;
            int añoFin = fechaFin.Year;

            int añoDiff = añoFin - añoIni;

            int mesIni = fechaIni.Month;
            int mesFin = fechaFin.Month;

            int fechaDiff = mesFin - mesIni;

            if(añoDiff > 0) {
                fechaDiff = fechaDiff + (añoDiff * 12);
            }


            for(int i = 0; i <= fechaDiff; i++) {
                var listaTemporal = new List<CAL_LogApiReniecEntidad>();
                var listaEstadoTemporal = new List<String>();
                listaTemporal = LeerLogxChannelxFecha(channel, fechaIni, fechaFin.AddDays(1), fechaIni.AddMonths(i));
                listaEstado = ListarEstadosxFecha(channel, fechaIni, fechaFin.AddDays(1), fechaIni.AddMonths(i),listaEstado);
                listaSala = ListarSalasxFecha(channel, fechaIni, fechaFin.AddDays(1), fechaIni.AddMonths(i), listaSala);
                lista.AddRange(listaTemporal);
                //listaEstado.AddRange(listaEstadoTemporal);
            }

            //Json grande xd

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new {
                listaEstado,
                listaSala,
                data = lista
            };
            var result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;

        }


        [seguridad(false)]
        public ActionResult ListarAllLogApiReniecFiltroFechasxGraficoJson(int channel, DateTime fechaIni, DateTime fechaFin) {

            var errormensaje = "";
            var lista = new List<CAL_LogApiReniecEntidad>();
            var listaEstado = new List<String>();
            var listaSala = new List<String>();


            int añoIni = fechaIni.Year;
            int añoFin = fechaFin.Year;

            int añoDiff = añoFin - añoIni;

            int mesIni = fechaIni.Month;
            int mesFin = fechaFin.Month;

            int fechaDiff = mesFin - mesIni;

            if(añoDiff > 0) {
                fechaDiff = fechaDiff + (añoDiff * 12);
            }


            for(int i = 0; i <= fechaDiff; i++) {
                var listaTemporal = new List<CAL_LogApiReniecEntidad>();
                var listaEstadoTemporal = new List<String>();
                listaTemporal = LeerLogxChannelxFechaxGrafico(channel, fechaIni, fechaFin.AddDays(1), fechaIni.AddMonths(i));
                listaEstado = ListarEstadosxFecha(channel, fechaIni, fechaFin.AddDays(1), fechaIni.AddMonths(i), listaEstado);
                listaSala = ListarSalasxFecha(channel, fechaIni, fechaFin.AddDays(1), fechaIni.AddMonths(i), listaSala);
                lista.AddRange(listaTemporal);
                //listaEstado.AddRange(listaEstadoTemporal);
            }

            //Json grande xd

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new {
                listaEstado,
                listaSala,
                data = lista
            };
            var result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;

        }

        [seguridad(false)]
        public List<CAL_LogApiReniecEntidad> LeerLogxChannelxFecha(int channel, DateTime fechaIni, DateTime fechaFin, DateTime fecha) {

            string month = fecha.ToString("MM");
            string year = fecha.ToString("yyyy");
            string fileDate = $"{month}-{year}";
            string fileExtension = "log";

            var lista = new List<CAL_LogApiReniecEntidad>();

            if(channel == 0 || channel == 1) {
                //web
                string channelName = "web";
                string fileName = $"apireniec-{channelName}";
                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\logs\\{fileName}-{fileDate}.{fileExtension}";

                String line;

                try {
                    StreamReader sr = new StreamReader(filePath);

                    line = sr.ReadLine();

                    while(line != null) {
                        string[] data = line.Split('|');

                        var item = new CAL_LogApiReniecEntidad();
                        item.FechaRegistro = Convert.ToDateTime(data[0]);
                        item.Sala = data[1];
                        item.Usuario = data[2];
                        item.NroDoc = data[3];
                        item.Estado = data[4];

                        if( Convert.ToDateTime(item.FechaRegistro)>=fechaIni && Convert.ToDateTime(item.FechaRegistro) <= fechaFin) {
                                lista.Add(item);
                        }

                        line = sr.ReadLine();
                    }

                    sr.Close();
                } catch(Exception e) {
                    Console.WriteLine("Exception: " + e.Message);
                } finally {
                    Console.WriteLine("Programa terminado.");
                    Console.WriteLine("Presione cualquier botón para cerrar la ventana.");
                }

            }
            if(channel == 0 || channel == 2) {

                //app
                string channelName = "app";
                string fileName = $"apireniec-{channelName}";
                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\logs\\{fileName}-{fileDate}.{fileExtension}";

                String line;

                try {
                    StreamReader sr = new StreamReader(filePath);

                    line = sr.ReadLine();

                    while(line != null) {
                        string[] data = line.Split('|');

                        var item = new CAL_LogApiReniecEntidad();
                        item.FechaRegistro = Convert.ToDateTime(data[0]);
                        item.Sala = data[1];
                        item.Usuario = data[2];
                        item.NroDoc = data[3];
                        item.Estado = data[4];

                        if(Convert.ToDateTime(item.FechaRegistro) >= fechaIni && Convert.ToDateTime(item.FechaRegistro) <= fechaFin) {
                                lista.Add(item);
                        }

                        line = sr.ReadLine();
                    }

                    sr.Close();
                } catch(Exception e) {
                    Console.WriteLine("Exception: " + e.Message);
                } finally {
                    Console.WriteLine("Programa terminado.");
                    Console.WriteLine("Presione cualquier botón para cerrar la ventana.");
                }
            }

            return lista;
        }


        [seguridad(false)]
        public List<CAL_LogApiReniecEntidad> LeerLogxChannelxFechaxGrafico(int channel, DateTime fechaIni, DateTime fechaFin, DateTime fecha) {

            string month = fecha.ToString("MM");
            string year = fecha.ToString("yyyy");
            string fileDate = $"{month}-{year}";
            string fileExtension = "log";

            var lista = new List<CAL_LogApiReniecEntidad>();

            if(channel == 0 || channel == 1) {
                //web
                string channelName = "web";
                string fileName = $"apireniec-{channelName}";
                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\logs\\{fileName}-{fileDate}.{fileExtension}";

                String line;

                try {
                    StreamReader sr = new StreamReader(filePath);

                    line = sr.ReadLine();

                    while(line != null) {
                        string[] data = line.Split('|');

                        var item = new CAL_LogApiReniecEntidad();
                        item.FechaRegistro = Convert.ToDateTime(data[0]);
                        item.Sala = data[1];
                        item.Usuario = data[2];
                        item.NroDoc = data[3];
                        item.Estado = data[4];

                        if(Convert.ToDateTime(item.FechaRegistro) >= fechaIni && Convert.ToDateTime(item.FechaRegistro) <= fechaFin) {
                            if(item.Estado.Trim() == "Consulta completada") {
                                lista.Add(item);
                            }
                        }

                        line = sr.ReadLine();
                    }

                    sr.Close();
                } catch(Exception e) {
                    Console.WriteLine("Exception: " + e.Message);
                } finally {
                    Console.WriteLine("Programa terminado.");
                    Console.WriteLine("Presione cualquier botón para cerrar la ventana.");
                }

            }
            if(channel == 0 || channel == 2) {

                //app
                string channelName = "app";
                string fileName = $"apireniec-{channelName}";
                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\logs\\{fileName}-{fileDate}.{fileExtension}";

                String line;

                try {
                    StreamReader sr = new StreamReader(filePath);

                    line = sr.ReadLine();

                    while(line != null) {
                        string[] data = line.Split('|');

                        var item = new CAL_LogApiReniecEntidad();
                        item.FechaRegistro = Convert.ToDateTime(data[0]);
                        item.Sala = data[1];
                        item.Usuario = data[2];
                        item.NroDoc = data[3];
                        item.Estado = data[4];

                        if(Convert.ToDateTime(item.FechaRegistro) >= fechaIni && Convert.ToDateTime(item.FechaRegistro) <= fechaFin) {
                            if(item.Estado.Trim() == "Consulta Completada") {
                                lista.Add(item);
                            }
                        }

                        line = sr.ReadLine();
                    }

                    sr.Close();
                } catch(Exception e) {
                    Console.WriteLine("Exception: " + e.Message);
                } finally {
                    Console.WriteLine("Programa terminado.");
                    Console.WriteLine("Presione cualquier botón para cerrar la ventana.");
                }
            }

            return lista;
        }

        [seguridad(false)]
        public List<String> ListarEstadosxFecha(int channel, DateTime fechaIni, DateTime fechaFin, DateTime fecha, List<String> lista) {
            string month = fecha.ToString("MM");
            string year = fecha.ToString("yyyy");
            string fileDate = $"{month}-{year}";
            string fileExtension = "log";


            if(channel == 0 || channel == 1) {
                //web
                string channelName = "web";
                string fileName = $"apireniec-{channelName}";
                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\logs\\{fileName}-{fileDate}.{fileExtension}";

                String line;

                try {
                    StreamReader sr = new StreamReader(filePath);

                    line = sr.ReadLine();

                    while(line != null) {
                        bool repetido = false;
                        string[] data = line.Split('|');

                        if(Convert.ToDateTime(data[0]) >= fechaIni && Convert.ToDateTime(data[0]) <= fechaFin) {

                            foreach(var item in lista) {
                                if(data[4].ToUpper().Equals(item.ToUpper())) {
                                    repetido = true;
                                    break;
                                }
                            }

                            if(!repetido)
                                lista.Add(data[4]);
                        }

                        line = sr.ReadLine();
                    }

                    sr.Close();
                } catch(Exception e) {
                    Console.WriteLine("Exception: " + e.Message);
                } finally {
                    Console.WriteLine("Programa terminado.");
                    Console.WriteLine("Presione cualquier botón para cerrar la ventana.");
                }

            }
            if(channel == 0 || channel == 2) {

                //app
                string channelName = "app";
                string fileName = $"apireniec-{channelName}";
                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\logs\\{fileName}-{fileDate}.{fileExtension}";

                String line;

                try {
                    StreamReader sr = new StreamReader(filePath);

                    line = sr.ReadLine();

                    while(line != null) {
                        bool repetido = false;
                        string[] data = line.Split('|');

                        if(Convert.ToDateTime(data[0]) >= fechaIni && Convert.ToDateTime(data[0]) <= fechaFin) {

                            foreach(var item in lista) {
                                if(data[4].ToUpper().Equals(item.ToUpper())) {
                                    repetido = true;
                                    break;
                                }
                            }

                            if(!repetido)
                                lista.Add(data[4]);
                        }

                        line = sr.ReadLine();
                    }

                    sr.Close();
                } catch(Exception e) {
                    Console.WriteLine("Exception: " + e.Message);
                } finally {
                    Console.WriteLine("Programa terminado.");
                    Console.WriteLine("Presione cualquier botón para cerrar la ventana.");
                }
            }

            return lista;
        }


        [seguridad(false)]
        public List<String> ListarSalasxFecha(int channel, DateTime fechaIni, DateTime fechaFin, DateTime fecha, List<String> lista) {
            string month = fecha.ToString("MM");
            string year = fecha.ToString("yyyy");
            string fileDate = $"{month}-{year}";
            string fileExtension = "log";


            if(channel == 0 || channel == 1) {
                //web
                string channelName = "web";
                string fileName = $"apireniec-{channelName}";
                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\logs\\{fileName}-{fileDate}.{fileExtension}";

                String line;

                try {
                    StreamReader sr = new StreamReader(filePath);

                    line = sr.ReadLine();

                    while(line != null) {
                        bool repetido = false;
                        string[] data = line.Split('|');

                        if(Convert.ToDateTime(data[0]) >= fechaIni && Convert.ToDateTime(data[0]) <= fechaFin) {

                            foreach(var item in lista) {
                                if(data[1].ToUpper().Equals(item.ToUpper())) {
                                    repetido = true;
                                    break;
                                }
                            }

                            if(!repetido)
                                lista.Add(data[1]);
                        }

                        line = sr.ReadLine();
                    }

                    sr.Close();
                } catch(Exception e) {
                    Console.WriteLine("Exception: " + e.Message);
                } finally {
                    Console.WriteLine("Programa terminado.");
                    Console.WriteLine("Presione cualquier botón para cerrar la ventana.");
                }

            }
            if(channel == 0 || channel == 2) {

                //app
                string channelName = "app";
                string fileName = $"apireniec-{channelName}";
                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\logs\\{fileName}-{fileDate}.{fileExtension}";

                String line;

                try {
                    StreamReader sr = new StreamReader(filePath);

                    line = sr.ReadLine();

                    while(line != null) {
                        bool repetido = false;
                        string[] data = line.Split('|');

                        if(Convert.ToDateTime(data[0]) >= fechaIni && Convert.ToDateTime(data[0]) <= fechaFin) {

                            foreach(var item in lista) {
                                if(data[1].ToUpper().Equals(item.ToUpper())) {
                                    repetido = true;
                                    break;
                                }
                            }

                            if(!repetido)
                                lista.Add(data[1]);
                        }

                        line = sr.ReadLine();
                    }

                    sr.Close();
                } catch(Exception e) {
                    Console.WriteLine("Exception: " + e.Message);
                } finally {
                    Console.WriteLine("Programa terminado.");
                    Console.WriteLine("Presione cualquier botón para cerrar la ventana.");
                }
            }

            return lista;
        }


        [seguridad(false)]
        public ActionResult GraficoUsuariosxSalaJson(int channel, DateTime fechaIni, DateTime fechaFin) {

            var errormensaje = "";
            var lista = new List<CAL_LogApiReniecEntidad>();
            var listaEstado = new List<String>();


            int añoIni = fechaIni.Year;
            int añoFin = fechaFin.Year;

            int añoDiff = añoFin - añoIni;

            int mesIni = fechaIni.Month;
            int mesFin = fechaFin.Month;

            int fechaDiff = mesFin - mesIni;

            if(añoDiff > 0) {
                fechaDiff = fechaDiff + (añoDiff * 12);
            }



            for(int i = 0; i <= fechaDiff; i++) {
                var listaTemporal = new List<CAL_LogApiReniecEntidad>();
                listaTemporal = LeerLogxChannelxFechaxGrafico(channel, fechaIni, fechaFin.AddDays(1), fechaIni.AddMonths(i));
                lista.AddRange(listaTemporal);
            }

            var data = new List<UsuariosxSalaGrafico>();

            var salaVacia = new UsuariosxSalaGrafico();

            salaVacia.nombreSala = "";
            salaVacia.cantidad = 0;
            data.Add(salaVacia);

            bool agregar = false;

            foreach(var item in lista) {

                agregar = true;

                var sala = new UsuariosxSalaGrafico();

                foreach(var obj in data) {
                    if(item.Sala == obj.nombreSala) {
                        obj.cantidad++;
                        agregar = false;
                        break;
                    } 
                }

                if(agregar) {
                    sala.nombreSala = item.Sala;
                    sala.cantidad = 1;
                    data.Add(sala);
                }
            }
            /*
            var sala1 = new UsuariosxSalaGrafico();
            sala1.nombreSala = "Excalibur";
            sala1.cantidad = 20;
            data.Add(sala1);

            var sala2 = new UsuariosxSalaGrafico();
            sala2.nombreSala = "Gangas";
            sala2.cantidad = 10;
            data.Add(sala2);

            var sala3 = new UsuariosxSalaGrafico();
            sala3.nombreSala = "Damasco";
            sala3.cantidad = 30;
            data.Add(sala3);
            */

            //Json grande xd

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new {
                data = data
            };
            var result = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;

        }

        private class UsuariosxSalaGrafico {
            
            public string nombreSala { get; set; }
            public int cantidad { get; set; }
        }

    }
}