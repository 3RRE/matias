using CapaDatos;
using CapaDatos.ManenimientoBD;
using CapaEntidad.MantenimientoBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.MantenimientoBD
{
    public class MantenimientoBDBL
    {
        private MantenimientoBDDAL _mantenimientoDAL = new MantenimientoBDDAL();

        public MantenimientoBDBL() { 

        }
        public bool GenerarBackup(string RutaBackup)
        {
            return _mantenimientoDAL.GenerarBackup(RutaBackup);
        }
        public bool LimpiarTabla(string Tabla, DateTime Fecha, string Columna)
        {
            return _mantenimientoDAL.LimpiarTabla(Tabla, Fecha, Columna);
        }
        public DatabaseInformacionEntidad InformacionDatabase()
        {
            return _mantenimientoDAL.InformacionDatabase();
        }
        public List<TablaInformacionEntidad> InformacionTablas()
        {
            return _mantenimientoDAL.InformacionTablas();
        }
        public List<BackupInformacionEntidad> ListarBackups(string RutaBackup)
        {
            return _mantenimientoDAL.ListarBackups(RutaBackup);
        }
        public double TamanioBackup(string RutaBackup)
        {
            return _mantenimientoDAL.TamanioBackup(RutaBackup);
        }
        public bool EliminarArchivoBackup(string RutaBackup)
        {
            return _mantenimientoDAL.EliminarArchivoBackup(RutaBackup);
        }
    }
}
