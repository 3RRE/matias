using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class EmpresaBL
    {
        private readonly EmpresaDAL _empresaDal = new EmpresaDAL();
        public List<EmpresaEntidad> ListadoEmpresa()
        {
            return _empresaDal.ListadoEmpresa();
        }
        public bool InsertarEmpresaJson(EmpresaEntidad empresa)
        {
            return _empresaDal.InsertarEmpresaJson(empresa);
        }
        public EmpresaEntidad EmpresaObtenerporIdJson(int codEmpresa)
        {
            return _empresaDal.EmpresaObtenerporIdJson(codEmpresa);
        }
        public bool EmpresaModificarJson(EmpresaEntidad empresa)
        {
            return _empresaDal.EmpresaModificarJson(empresa);
        }

        public EmpresaEntidad EmpresaListaIdJson(int empresaId)
        {
            return _empresaDal.EmpresaListaIdJson(empresaId);
        }
    }
}
