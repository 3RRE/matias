using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers {
    [seguridad]
    public class EmpleadoController : Controller {
        private SEG_EmpleadoBL empleadoBL = new SEG_EmpleadoBL();
        private SEG_UsuarioBL segUsuarioBl = new SEG_UsuarioBL();
        private SEG_CargoBL cargoBl = new SEG_CargoBL();
        private TipoDOIBL tipoDoiBL = new TipoDOIBL();
        //private MaestroMaquinasBL maestroMaquinasBl = new MaestroMaquinasBL();

        private SEG_RolBL webRolBL = new SEG_RolBL();
        private SEG_RolUsuarioBL webRolUsuarioBL = new SEG_RolUsuarioBL();

        public ActionResult EmpleadoListadoVista() {
            return View("~/Views/Empleado/EmpleadoListadoVista.cshtml");
        }
        public ActionResult EmpleadoNuevoVista() {
            return View("~/Views/Empleado/EmpleadoNuevoVista.cshtml");
        }
        public ActionResult EmpleadoRegistroVista(string id) {
            int sub = Convert.ToInt32(id.Replace("Registro", ""));
            var errormensaje = "";
            var empleado = new SEG_EmpleadoEntidad();
            try {
                empleado = empleadoBL.EmpleadoIdObtenerJson(sub);

            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
                errormensaje = "Verifique conexion,Llame Administrador";
            }
            ViewBag.empleado = empleado;
            ViewBag.errormensaje = errormensaje;
            return View("~/Views/Empleado/EmpleadoRegistroVista.cshtml");
        }


        [HttpPost]
        public ActionResult MantenimientoCargoParcialVista() {
            return PartialView("~/Views/Mantenimiento/Cargo/MantenimientoCargoParcialVista.cshtml");
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult CargoListarJson() {
            var errormensaje = "";
            var lista = new List<SEG_CargoEntidad>();
            try {
                lista = cargoBl.CargoListarJson();

            } catch(Exception exp) {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult CargoMantenimientoListarJson() {
            var errormensaje = "";
            var lista = new List<SEG_CargoEntidad>();
            try {
                lista = cargoBl.CargoMantenimientoListarJson();

            } catch(Exception exp) {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            var listado = lista.Select(x => new { x.CargoID, x.Descripcion, x.Estado, EstadoString = x.Estado == 1 ? "ACTIVO" : "INACTIVO" }).ToList();
            return Json(new { data = listado.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult CargoGuardarJson(SEG_CargoEntidad cargo) {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try {
                respuestaConsulta = cargoBl.CargoGuardarJson(cargo);

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult CargoActualizarJson(SEG_CargoEntidad cargo) {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try {
                respuestaConsulta = cargoBl.CargoActualizarJson(cargo);

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult TipoDocumentoListarJson() {
            var errormensaje = "";
            var lista = new List<TipoDOIEntidad>();
            try {
                lista = tipoDoiBL.TipoDocumentoListarJson();

            } catch(Exception exp) {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult EmpleadoListarJson() {
            var errormensaje = "";
            var lista = new List<SEG_EmpleadoEntidad>();
            var listaRol = new List<SEG_RolEntidad>();
            var listaRolUsuario = new List<SEG_RolUsuarioEntidad>();
            try {
                lista = empleadoBL.EmpleadoListarJson();
                listaRol = webRolBL.ListarRol().OrderBy(x => x.WEB_RolNombre).ToList();
                listaRolUsuario = webRolUsuarioBL.ListarRolUsuarios();

            } catch(Exception exp) {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            var listado = from list in lista
                          orderby list.EmpleadoID descending
                          select new {
                              nombre = list.ApellidosPaterno + " " + list.ApellidosMaterno + ", " + list.Nombres,
                              list.EmpleadoID,
                              list.CargoID,
                              list.CargoNombre,
                              list.MailJob,
                              list.EstadoEmpleado,
                              EstadoNombre = list.EstadoEmpleado.ToString() == "1" ? "Habilitado" : "Deshabilitado",
                              list.UsuarioNombre,
                              list.UsuarioID
                          };
            return Json(new { data = listado.ToList(), roles = listaRol, rolUsuarios = listaRolUsuario.ToList(), mensaje = errormensaje });
            //  var aa = lista.ToList();

        }

        [HttpPost]
        public ActionResult EmpleadoGuardarJson(SEG_EmpleadoEntidad empleado) {
            var errormensaje = "";
            bool respuestaConsulta = false;

            if(empleado.FechaNacimiento < SqlDateTime.MinValue.Value || empleado.FechaNacimiento > SqlDateTime.MaxValue.Value) {
                errormensaje = $"La fecha de nacimiento esta fuera de los rangos permitidos, el valor mínimo es '{SqlDateTime.MinValue.Value:dd/MM/yyyy}' y el valor máximo es '{SqlDateTime.MaxValue.Value:dd/MM/yyyy}', seleccione una fecha valida.";
                return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
            }

            try {
                empleado.EstadoEmpleado = 1;
                empleado.FechaAlta = DateTime.Now;
                respuestaConsulta = empleadoBL.EmpleadoGuardarJson(empleado);

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult EmpleadoActualizarJson(SEG_EmpleadoEntidad empleado) {
            var errormensaje = "";
            bool respuestaConsulta = false;
            if(empleado.FechaNacimiento < SqlDateTime.MinValue.Value || empleado.FechaNacimiento > SqlDateTime.MaxValue.Value) {
                errormensaje = $"La fecha de nacimiento esta fuera de los rangos permitidos, el valor mínimo es '{SqlDateTime.MinValue.Value:dd/MM/yyyy}' y el valor máximo es '{SqlDateTime.MaxValue.Value:dd/MM/yyyy}', seleccione una fecha valida.";
                return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
            }
            try {
                respuestaConsulta = empleadoBL.EmpleadoActualizarJson(empleado);

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult EstadoEmpleadoActualizarJson(int EmpleadoId, int EstadoEmpleado) {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try {
                respuestaConsulta = empleadoBL.EstadoEmpleadoActualizarJson(EmpleadoId, EstadoEmpleado);

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult UsuarioEmpleadoIdObtenerJson(int empleadiId) {
            var errormensaje = "Accion realizada Correctamente.";
            var usuario = new SEG_UsuarioEntidad();
            try {
                usuario = segUsuarioBl.UsuarioEmpleadoIdObtenerJson(empleadiId);

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = usuario, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult EmpleadoIdObtenerJson(int empleadiId) {
            var errormensaje = "Accion realizada Correctamente.";
            var usuario = new SEG_EmpleadoEntidad();
            try {
                usuario = empleadoBL.EmpleadoIdObtenerJson(empleadiId);

            } catch(Exception exp) {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = usuario, mensaje = errormensaje });
        }

        ////[seguridad(false)]
        //[HttpPost]
        //public ActionResult CargarProveedores()
        //{
        //    var errormensaje = "";
        //    var lista = new List<dynamic>();
        //    var listaE = new List<dynamic>();
        //    try
        //    {
        //        lista = maestroMaquinasBl.CajeroProveedor(604);
        //        //cboCaja.ValueMember = "tab_codigo";
        //        //cboCaja.DisplayMember = "tab_titulo";
        //    }
        //    catch (Exception exp)
        //    {
        //        errormensaje = exp.Message + ", Llame Administrador";
        //    }
        //    var datalista = lista.Select(x => new { x.tab_codigo, x.tab_titulo }).Where(y => y.tab_codigo > 10 || y.tab_codigo == 0).ToList();
        //    return Json(new { data = datalista.ToList(), mensaje = errormensaje });
        //}

        ////[seguridad(false)]
        //[HttpPost]
        //public ActionResult CargarCajero(int fProveedor)
        //{
        //    var errormensaje = "";
        //    var lista = new List<dynamic>();
        //    var sala = new List<dynamic>(); ;
        //    if (fProveedor != 0)
        //    {
        //        try
        //        {
        //            sala = maestroMaquinasBl.GetSala();
        //            var dataSala = sala.Select(x => new { x.Cod_Empresa, x.Cod_Sala }).ToList();
        //            lista = maestroMaquinasBl.Cajas(dataSala[0].Cod_Empresa, dataSala[0].Cod_Sala);

        //        }
        //        catch (Exception exp)
        //        {
        //            errormensaje = exp.Message + ", Llame Administrador";
        //        }

        //    }
        //    var datalist = lista.Select(x => new { x.IdCaja, x.Cod_Sala, x.Nombre, x.Tipo_Caja }).Where(y => y.Tipo_Caja == fProveedor).ToList();
        //    return Json(new { data = datalist.ToList(), mensaje = errormensaje });
        //}
        [seguridad(false)]
        [HttpPost]
        public ActionResult EmpleadoListarPorNoUsadosJson() {
            var errormensaje = "";
            var lista = new List<SEG_EmpleadoEntidad>();
            try {
                lista = empleadoBL.EmpleadoListarPorNoUsadosJson();
            } catch(Exception exp) {
                errormensaje = exp.Message + ", Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });

        }

        [seguridad(false)]
        [HttpPost]
        public ActionResult EmpleadoListarPorUsuariosJson() {
            var errormensaje = "";
            bool respuesta = false;
            var lista = new List<SEG_EmpleadoEntidad>();
            try {
                lista = empleadoBL.EmpleadoListarPorUsuariosJson();
                respuesta = true;
            } catch(Exception exp) {
                errormensaje = exp.Message + ", Llame Administrador";
                respuesta = false;
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje, respuesta });

        }

    }
}