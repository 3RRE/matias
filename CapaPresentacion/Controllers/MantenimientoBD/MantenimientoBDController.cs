using CapaEntidad.MantenimientoBD;
using CapaNegocio.MantenimientoBD;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.MantenimientoBD
{
    public class MantenimientoBDController : Controller
    {
        private readonly MantenimientoBDBL _mantenimientoBDBL=new MantenimientoBDBL();
        const string DATABASE_NAME = "BD_SEGURIDADPJ";

        // GET: MantenimientoBD
        public ActionResult Index()
        {
            ViewBag.RutaBackups = ObtenerRutaBackups();
            return View("~/Views/MantenimientoBD/Backup.cshtml");
        }
        //[HttpPost]
        //public ActionResult ObtenerListadoBackups(string RutaBackups)
        //{
        //    bool response = false;
        //    string errormensaje = "";
        //    var listaArchivos = new List<dynamic>();
        //    try
        //    {
        //        string direccion = RutaBackups;
        //        if (Directory.Exists(direccion))
        //        {
        //            DirectoryInfo di = new DirectoryInfo(direccion);
        //            FileInfo[] files = di.GetFiles("*.bak");
        //            foreach (FileInfo file in files)
        //            {
        //                //tamaño de archivo
        //                float length = (file.Length / 1024f) / 1024f;
        //                listaArchivos.Add(new
        //                {
        //                    nombre_completo = file.Name,
        //                    tamanio = Math.Round(length, 4)
        //                });
        //            }
        //            errormensaje = "Listando Archivos";
        //            response = true;
        //        }
        //        else
        //        {
        //            errormensaje = "No se encuentra el Directorio";
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        listaArchivos=new List<dynamic>();
        //    }
        //    return Json(new { data = listaArchivos.ToList(), respuesta = response, mensaje = errormensaje });
        //}
        [HttpPost]
        public ActionResult BorrarArchivoBackup(string RutaBackups,string NombreArchivo)
        {
            string mensaje = "No se pudo eliminar el archivo";
            bool respuesta = false;
            try
            {
                string RutaCompleta = Path.Combine(RutaBackups, NombreArchivo);
                respuesta = _mantenimientoBDBL.EliminarArchivoBackup(RutaCompleta);
                if (respuesta)
                {
                    mensaje = "Archivo Eliminado";
                }
                //if (!System.IO.File.Exists(RutaCompleta))
                //{
                //    return Json(new { mensaje=$"No existe el Archivo {RutaCompleta}",respuesta=false });
                //}
                //System.IO.File.Delete(RutaCompleta);
            }
            catch (Exception ex)
            {
                mensaje = "Ocurrio un error : "+ ex.Message;
            }
            return Json(new { mensaje,respuesta });
        }
        [HttpPost]
        public ActionResult GenerarBackup(string RutaBackups)
        {
            bool respuesta = false;
            string mensaje = "No se pudo generar el Backup";
            try
            {
                respuesta = _mantenimientoBDBL.GenerarBackup(RutaBackups);
                if (respuesta)
                {
                    mensaje = "Backup Generado";
                }
            }
            catch (Exception ex)
            {
                mensaje = "Ocurrio un error - " + ex.Message;
            }
            return Json(new { mensaje,respuesta});
        }
        [HttpPost]
        public ActionResult LimpiarTabla(string Tabla, DateTime Fecha, string Columna)
        {
            bool respuesta = false;
            string mensaje = "No se pudo eliminar la información";
            try
            {
                respuesta = _mantenimientoBDBL.LimpiarTabla(Tabla, Fecha,Columna);
                if (respuesta)
                {
                    mensaje = "Informacion eliminada";
                }
            }
            catch (Exception ex)
            {
                mensaje = "Ocurrio un error - " + ex.Message;
            }
            return Json(new { mensaje, respuesta });
        }
        public ActionResult InformacionDatabase()
        {
            try
            {
                var informacionDatabase=_mantenimientoBDBL.InformacionDatabase();
                var informacionTablas=_mantenimientoBDBL.InformacionTablas();
                object objRespuesta = new
                {
                    informacionDatabase,
                    informacionTablas
                };
                return Json(objRespuesta);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { result=false });
            }
        }
        //public ActionResult InformacionTablas()
        //{
        //    try
        //    {
        //        var result = _mantenimientoBDBL.InformacionTablas();
        //        return Json(new { result });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = false });
        //    }
        //}
        private string ObtenerRutaBackups()
        {
            string RutaBackups=string.Empty;
            try
            {
                RutaBackups = ConfigurationManager.AppSettings["RutaBackups"].ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return RutaBackups;
        }

        [HttpPost]
        public ActionResult ObtenerListadoBackups(string RutaBackups)
        {
            bool response = false;
            string errormensaje = "";
            var listaArchivos = new List<dynamic>();
            try
            {
                string direccion = RutaBackups;
                List<BackupInformacionEntidad> listaBackups = _mantenimientoBDBL.ListarBackups(RutaBackups);
                foreach(var item in listaBackups)
                {
                    if (item.Nombre == string.Empty) { continue; }
                    var RutaCompletaBackup = Path.Combine(RutaBackups, item.Nombre);
                    double tamanioBackup = _mantenimientoBDBL.TamanioBackup(RutaCompletaBackup);
                    float length = (float)(tamanioBackup / 1024f) / 1024f;
                    listaArchivos.Add(new{
                        nombre_completo=item.Nombre,
                        tamanio=Math.Round(length,4)
                    });
                }
                errormensaje = "Listando Archivos";
                response = true;
            }
            catch (Exception)
            {

                listaArchivos = new List<dynamic>();
            }
            return Json(new { data = listaArchivos.ToList(), respuesta = response, mensaje = errormensaje });
        }

    }
}