using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class UsuarioSalaBL
    {
        private readonly UsuarioSalaDAL _usuarioSalaDal = new UsuarioSalaDAL();
        public List<UsuarioSalaEntidad> UsuarioSalasListarJson(int usuarioId)
        {
            return _usuarioSalaDal.UsuarioSalasListarIdJson(usuarioId);
        }
        public List<UsuarioSalaEntidad> UsuarioSalasListarxsalaidJson(string salaid)
        {
            return _usuarioSalaDal.UsuarioSalasListarxsalaidJson(salaid);
        }
        public EmpleadoUsuarioEntidad UsuarioEmpleadoIdListarJson(int usuarioId)
        {
            return _usuarioSalaDal.UsuarioEmpleadoListarIdJson(usuarioId);
        }
        public UsuarioSalaEntidad UsuarioSalaIdListarJson(int usuarioId, int salaId)
        {
            return _usuarioSalaDal.UsuarioSalaListarIdJson(usuarioId, salaId);
        }
        public bool UsuarioSalaInsertarJson(int usuarioId, int salaId)
        {
            return _usuarioSalaDal.UsuarioSalaInsertarJson(usuarioId, salaId);
        }
        public bool UsuarioSalaEliminarJson(int usuarioId, int salaId)
        {
            return _usuarioSalaDal.UsuarioSalaEliminarJson(usuarioId, salaId);
        }

        public bool UsuarioSalaAsignar(int usuarioId, List<int> salaIds)
        {
            return _usuarioSalaDal.UsuarioSalaAsignar(usuarioId, salaIds);
        }

        public bool UsuarioSalaDenegar(int usuarioId, List<int> salaIds)
        {
            return _usuarioSalaDal.UsuarioSalaDenegar(usuarioId, salaIds);
        }
    }
}
