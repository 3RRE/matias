using CapaDatos.ExcelenciaOperativa;
using CapaDatos.Utilitarios;
using CapaEntidad.ExcelenciaOperativa;
using System;
using System.Collections.Generic;

namespace CapaNegocio.ExcelenciaOperativa
{
    public class EO_FichaExcelenciaOperativaBL
    {
        // Business Layer
        private EO_FichaCategoriaBL fichaCategoriaBL = new EO_FichaCategoriaBL();
        private EO_FichaItemBL fichaItemBL = new EO_FichaItemBL();

        // Data Access Layer
        private EO_FichaExcelenciaOperativaDAL fichaExcelenciaOperativaDAL = new EO_FichaExcelenciaOperativaDAL();
        private EO_FichaCategoriaDAL fichaCategoriaDAL = new EO_FichaCategoriaDAL();
        private EO_FichaItemDAL fichaItemDAL = new EO_FichaItemDAL();

        public long InsertarFichaExcelenciaOperativa(EO_FichaExcelenciaOperativaEntidad ficha)
        {
            return fichaExcelenciaOperativaDAL.InsertarFichaExcelenciaOperativa(ficha);
        }

        public bool ActualizarFichaExcelenciaOperativa(EO_FichaExcelenciaOperativaEntidad ficha)
        {
            return fichaExcelenciaOperativaDAL.ActualizarFichaExcelenciaOperativa(ficha);
        }

        public bool GuardarFichaExcelenciaOperativa(EO_FichaExcelenciaOperativaEntidad ficha)
        {
            bool response = false;

            long fichaId = InsertarFichaExcelenciaOperativa(ficha);

            if (fichaId != 0)
            {
                foreach (EO_FichaCategoriaEntidad categoria in ficha.Categorias)
                {
                    EO_FichaCategoriaEntidad fichaCategoria = new EO_FichaCategoriaEntidad();

                    fichaCategoria.FichaId = fichaId;
                    fichaCategoria.Nombre = categoria.Nombre;
                    fichaCategoria.PuntuacionObtenida = categoria.PuntuacionObtenida;
                    fichaCategoria.PuntuacionBase = categoria.PuntuacionBase;
                    fichaCategoria.Porcentaje = categoria.Porcentaje;
                    fichaCategoria.Codigo = categoria.Codigo;

                    long fichaCategoriaId = fichaCategoriaBL.InsertarFichaCategoria(fichaCategoria);

                    if (fichaCategoriaId != 0)
                    {
                        foreach (EO_FichaItemEntidad item in categoria.Items)
                        {
                            EO_FichaItemEntidad fichaItem = new EO_FichaItemEntidad();

                            fichaItem.FichaId = fichaId;
                            fichaItem.CategoriaId = fichaCategoriaId;
                            fichaItem.Nombre = item.Nombre;
                            fichaItem.PuntuacionObtenida = item.PuntuacionObtenida;
                            fichaItem.PuntuacionBase = item.PuntuacionBase;
                            fichaItem.FechaExpiracion = item.FechaExpiracion;
                            fichaItem.FechaExpiracionActivo = item.FechaExpiracionActivo;
                            fichaItem.Respuesta = item.Respuesta;
                            fichaItem.Observacion = item.Observacion;
                            fichaItem.TipoRespuesta = item.TipoRespuesta;
                            fichaItem.Codigo = item.Codigo;

                            fichaItemBL.InsertarFichaItem(fichaItem);
                        }
                    }
                }

                response = true;
            }

            return response;
        }

        public bool UpdateFichaExcelenciaOperativa(EO_FichaExcelenciaOperativaEntidad fichaExcelenciaOperativa)
        {
            bool respuesta = false;

            EO_FichaCategoriaBL fichaCategoriaBL = new EO_FichaCategoriaBL();
            EO_FichaItemBL fichaItemBL = new EO_FichaItemBL();

            bool fichaUpdate = fichaExcelenciaOperativaDAL.ActualizarFichaExcelenciaOperativa(fichaExcelenciaOperativa);

            if (fichaUpdate)
            {
                foreach (EO_FichaCategoriaEntidad categoria in fichaExcelenciaOperativa.Categorias)
                {
                    bool fichaCategoriaUpdate = fichaCategoriaBL.ActualizarFichaCategoria(categoria);

                    if (fichaCategoriaUpdate)
                    {
                        foreach (EO_FichaItemEntidad item in categoria.Items)
                        {
                            fichaItemBL.ActualizarFichaItem(item);
                        }
                    }
                }

                respuesta = true;
            }

            return respuesta;
        }

        public (List<EO_FichaExcelenciaOperativaEntidad> fichaExcelenciasOperativasLista, ClaseError error) FichaEOFiltroListarxTipoFechaJson(string tipos, DateTime inicio, DateTime fin)
        {
            return fichaExcelenciaOperativaDAL.FichaEOFiltroListarxTipoFechaJson(tipos, inicio, fin);
        }

        public (EO_FichaExcelenciaOperativaEntidad fichaExcelenciaOperativa, ClaseError error) FichaEOIdObtenerJson(long id)
        {
            return fichaExcelenciaOperativaDAL.FichaEOIdObtenerJson(id);
        }

        // refactored
        public List<EO_FichaExcelenciaOperativaEntidad> ListaFichaEO(string inFilters, DateTime startDate, DateTime endDate, string orderBy = "DESC")
        {
            List<EO_FichaExcelenciaOperativaEntidad> listaFicha = fichaExcelenciaOperativaDAL.ListaFichaEOFilters(inFilters, startDate, endDate, orderBy);

            foreach (EO_FichaExcelenciaOperativaEntidad ficha in listaFicha)
            {
                List<EO_FichaCategoriaEntidad> listaCategoria = new List<EO_FichaCategoriaEntidad>();

                foreach (EO_FichaCategoriaEntidad categoria in fichaCategoriaBL.FichaCategoriaIdFichaObtenerJson(ficha.FichaId))
                {
                    List<EO_FichaItemEntidad> listaItem = new List<EO_FichaItemEntidad>();

                    foreach (EO_FichaItemEntidad item in fichaItemBL.FichaItemIdCategoriaObtenerJson(categoria.CategoriaId))
                    {
                        listaItem.Add(item);
                    }

                    categoria.Items = listaItem;

                    listaCategoria.Add(categoria);
                }

                ficha.Categorias = listaCategoria;
            }

            return listaFicha;
        }

        public EO_FichaExcelenciaOperativaEntidad FichaEOId(long fichaId)
        {
            EO_FichaExcelenciaOperativaEntidad ficha = fichaExcelenciaOperativaDAL.FichaEOId(fichaId);

            List<EO_FichaCategoriaEntidad> listaCategoria = new List<EO_FichaCategoriaEntidad>();

            foreach(EO_FichaCategoriaEntidad categoria in fichaCategoriaBL.FichaCategoriaIdFichaObtenerJson(ficha.FichaId))
            {
                List<EO_FichaItemEntidad> listaItem = new List<EO_FichaItemEntidad>();

                foreach(EO_FichaItemEntidad item in fichaItemBL.FichaItemIdCategoriaObtenerJson(categoria.CategoriaId))
                {
                    listaItem.Add(item);
                }

                categoria.Items = listaItem;

                listaCategoria.Add(categoria);
            }

            ficha.Categorias = listaCategoria;

            return ficha;
        }

        public EO_FichaExcelenciaOperativaEntidad GetOnlyFicha(long fichaId)
        {
            return fichaExcelenciaOperativaDAL.FichaEOId(fichaId);
        }

        public bool EliminarExcelenciaOperativa(long fichaId)
        {
            bool response = false;

            if(fichaExcelenciaOperativaDAL.EliminarEOId(fichaId))
            {
                fichaCategoriaDAL.EliminarEOCategoriasFicha(fichaId);
                fichaItemDAL.EliminarEOItemsFicha(fichaId);

                response = true;
            }

            return response;
        }
    }
}
