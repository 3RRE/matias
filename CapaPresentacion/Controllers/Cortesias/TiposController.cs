using CapaEntidad.Cortesias;
using CapaNegocio;
using CapaNegocio.Cortesias;
using CapaPresentacion.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Cortesias {
    public class TiposController : Controller {
        private readonly CRT_TipoBL tipoBL;
        private readonly SalaBL salaBL;
        private readonly ServiceCortesias serviceCortesias;

        public TiposController() {
            tipoBL = new CRT_TipoBL();
            salaBL = new SalaBL();
            serviceCortesias = new ServiceCortesias();
        }

        #region Views
        public ActionResult Index() {
            return View("~/Views/Cortesias/Tipo/Index.cshtml");
        }

        public ActionResult Guardar(int id = 0) {
            CRT_Tipo tipo = id == 0 ? new CRT_Tipo() : tipoBL.ObtenerTipoPorId(id);
            return View("~/Views/Cortesias/Tipo/AddEdit.cshtml", tipo);
        }
        #endregion

        #region Methods
        [HttpPost]
        public JsonResult GetTipos() {
            bool success = false;
            string displayMessage;
            List<CRT_Tipo> data = new List<CRT_Tipo>();

            try {
                data = tipoBL.ObtenerTipos();
                success = data.Count > 0;

                foreach(var item in data) {

                    string PathArchivos = ConfigurationManager.AppSettings["PathArchivos"];
                    string filePath = Path.Combine(PathArchivos, "Cortesias", "Tipos", item.ImagenUrl);

                    if(System.IO.File.Exists(filePath)) {
                        byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                        string base64String = Convert.ToBase64String(imageBytes);
                        item.ImagenBase64 = $"data:image/png;base64,{base64String}";
                    } else {
                        item.ImagenBase64 = string.Empty;
                    }
                }

                displayMessage = success ? "Lista de tipos." : "No hay tipos registrados.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public JsonResult GetTipoById(int id) {
            bool success = false;
            string displayMessage;
            CRT_Tipo data = new CRT_Tipo();

            try {
                data = tipoBL.ObtenerTipoPorId(id);
                success = data.Existe();
                displayMessage = success ? "Tipo encontrado." : "No se encontró el tipo.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage, data });
        }

        [HttpPost]
        public async Task<JsonResult> SaveTipo(CRT_Tipo tipo, HttpPostedFileBase imageFile) {
            bool success = false;
            string displayMessage;
            bool isEdit = tipo.Existe();

            try {

                if(imageFile != null && imageFile.ContentLength > 0) {
                    string fileName = tipo.Nombre + Path.GetExtension(imageFile.FileName);
                    string PathArchivos = ConfigurationManager.AppSettings["PathArchivos"];
                    Directory.CreateDirectory(Path.GetDirectoryName(PathArchivos));

                    string filePath = Path.Combine(PathArchivos, "Cortesias", "Tipos", fileName);

                    using(var stream = new FileStream(filePath, FileMode.Create)) {
                        await imageFile.InputStream.CopyToAsync(stream);
                        tipo.ImagenUrl = fileName;
                    }

                }

                if(tipo.ImagenUrl == null) {
                    tipo.ImagenUrl = "default.png";
                }

                tipo.IdUsuario = Convert.ToInt32(Session["UsuarioID"]);
                success = isEdit ? tipoBL.ActualizarTipo(tipo) : tipoBL.InsertarTipo(tipo);

                if(isEdit) {

                    var salas = salaBL.ListadoTodosSalaActivosOrderJson();
                    foreach(var sala in salas) {

                        List<CRT_Tipo> dataSala = await serviceCortesias.GetList<CRT_Tipo>("GetTipos", sala.CodSala);

                        var tipoEncontrado = dataSala.FirstOrDefault(x => x.Id == tipo.Id);

                        if(tipoEncontrado != null) {

                            bool inserted = false;

                            inserted = await serviceCortesias.CreateOrUpdate<CRT_Tipo>("SaveTipo", sala.CodSala, tipo);

                            if(inserted && tipo.ImagenUrl != "default.png") {
                                string PathArchivos = ConfigurationManager.AppSettings["PathArchivos"];
                                Directory.CreateDirectory(Path.GetDirectoryName(PathArchivos));

                                string filePathTipo = Path.Combine(PathArchivos, "Cortesias", "Tipos", tipo.ImagenUrl);
                                bool insertedImageTipo = await serviceCortesias.SendImage("SaveImageTipo", sala.CodSala, filePathTipo, tipo.ImagenUrl);
                                if(insertedImageTipo) {
                                    success = true;
                                }
                            }
                        }



                    }

                }

                displayMessage = success ? "Tipo guardado correctamente." : "No se pudo guardar el tipo.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }
            return Json(new { success, displayMessage });
        }

        [HttpPost]
        public JsonResult DeleteTipo(int id) {
            bool success = false;
            string displayMessage;

            try {
                success = tipoBL.EliminarTipo(id);
                displayMessage = success ? "Tipo eliminado correctamente." : "No se pudo eliminar el tipo.";
            } catch(Exception ex) {
                displayMessage = $"{ex.Message}. Llame al administrador";
            }

            return Json(new { success, displayMessage });
        }
        #endregion
    }
}
