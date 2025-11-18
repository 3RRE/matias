using CapaDatos.Reclamaciones;
using CapaEntidad.Reclamaciones;
using S3k.Utilitario.clases_especial;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Reclamaciones
{
    
    public class REC_reclamacionBL
    {
        private REC_reclamacionDAL reclamacionDAL = new REC_reclamacionDAL();
        public (List<REC_reclamacionEntidad> rec_reclamacionLista, ClaseError error) REC_reclamacionListarJson()
        {          
            return reclamacionDAL.REC_reclamacionListarJson();
        }

        public (List<REC_reclamacionEntidad> rec_reclamacionLista, ClaseError error) REC_reclamacionListarxSalaFechaJson(string salas, DateTime fechaini, DateTime fechafin)
        {
            return reclamacionDAL.REC_reclamacionListarxSalaFechaJson(salas, fechaini, fechafin);
        }

        public (REC_reclamacionEntidad rec_reclamacion, ClaseError error) REC_reclamacionIdObtenerJson(Int64 id)
        {
            return reclamacionDAL.REC_reclamacionIdObtenerJson(id);
        }

        public (Int64 total, ClaseError error) REC_reclamacionTotalSalaJson(Int64 id)
        {
            return reclamacionDAL.REC_reclamacionTotalSalaJson(id);
        }

        public (REC_reclamacionEntidad rec_reclamacion, ClaseError error) REC_reclamacionHashObtenerJson(string doc)
        {
            return reclamacionDAL.REC_reclamacionHashObtenerJson(doc);
        }

        public (Int64 REC_reclamacionInsertado, ClaseError error) REC_reclamacionInsertarJson(REC_reclamacionEntidad rec_reclamacion)
        {          
            return reclamacionDAL.REC_reclamacionInsertarJson(rec_reclamacion);
        }

        public (bool REC_reclamacionEditado, ClaseError error) REC_reclamacionEditarJson(REC_reclamacionEntidad rec_reclamacion)
        {
            return reclamacionDAL.REC_reclamacionEditarJson(rec_reclamacion);
        }

        public (bool REC_reclamacionEditado, ClaseError error) ReclamacionAtencionSalaJson(REC_reclamacionEntidad rec_reclamacion)
        {
            return reclamacionDAL.ReclamacionAtencionSalaJson(rec_reclamacion);
        }

        public (bool REC_reclamacionEditado, ClaseError error) ReclamacionAtencionLegalJson(REC_reclamacionEntidad rec_reclamacion)
        {
            return reclamacionDAL.ReclamacionAtencionLegalJson(rec_reclamacion);
        }

        public (bool REC_reclamacionEditado, ClaseError error) REC_reclamacionEditarHashJson(REC_reclamacionEntidad rec_reclamacion)
        {
            return reclamacionDAL.reclamacionEditarHashJson(rec_reclamacion);
        }

        public (bool rec_reclamacionEliminado, ClaseError error) REC_reclamacionEliminarJson(Int64 id)
        {           
            return reclamacionDAL.REC_reclamacionEliminarJson(id);
        }
        public (bool rec_reclamacionEditado, ClaseError error) REC_ReclamacionEditarAdjunto(REC_reclamacionEntidad rec_reclamacion)
        {
            return reclamacionDAL.REC_ReclamacionEditarAdjunto(rec_reclamacion);
        }
        public (List<REC_reclamacionEntidad> lista, ClaseError error) REC_reclamacionListarporIdsJson(string ids)
        {
            return reclamacionDAL.REC_reclamacionListarporIdsJson(ids);
        }
        public (bool REC_reclamacionEditado, ClaseError error) reclamacionGuardarDesistimientoJson(REC_reclamacionEntidad REC_reclamacion)
        {
            return reclamacionDAL.reclamacionGuardarDesistimientoJson(REC_reclamacion);
        }

        public (bool enviarAdjuntoEditado, ClaseError error) ActualizarEnviarAdjunto(long reclamacionId, int enviarAdjunto)
        {
            return reclamacionDAL.ActualizarEnviarAdjunto(reclamacionId, enviarAdjunto);
        }
    }
}
