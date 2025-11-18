using CapaDatos.Campaña;
using CapaEntidad.Campañas;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Campaña
{
    public class CMP_SalalibreBL
    {
        private CMP_SalalibreDAL salalibre_dal = new CMP_SalalibreDAL();

        public List<CMP_SalalibreEntidad> CMPsalalibretListadoCompletoJson()
        {
            return salalibre_dal.GetSalaslibre();
        }

        public List<CMP_SalalibreEntidad> CMPsalalibrexsala(int codsala)
        {
            return salalibre_dal.GetSalaslibrexCodsala(codsala);
        }
        public CMP_SalalibreEntidad CMPsalalibreIdObtenerJson(int id)
        {
            return salalibre_dal.GetSalaLibreID(id);
        }
        public int salalibreInsertarJson(CMP_SalalibreEntidad sala)
        {
            return salalibre_dal.GuardarSalalibre(sala);
        }
        public bool salalibreEliminarJson(Int64 id)
        {
            return salalibre_dal.eliminarSalalibre(id);
        }
    }
}
