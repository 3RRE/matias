using System;
using System.Linq;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using Quartz;

namespace IASServiceServer.Jobs
{
    public class SincronizarSalasJob : IJob
    {
        private readonly DynamicModels.Admistrativo.SalaDynamicModel _salaDmAdm;
        private readonly DynamicModels.SeguridadPj.SalaDynamicModel _salaDmSegPj;
        private readonly DynamicModels.Admistrativo.EmpresaDynamicModel _empresaDmAdm;
        private readonly DynamicModels.SeguridadPj.EmpresaDynamicModel _empresaDmSegPj;

        public SincronizarSalasJob()
        {
            _salaDmAdm = new DynamicModels.Admistrativo.SalaDynamicModel();
            _salaDmSegPj = new DynamicModels.SeguridadPj.SalaDynamicModel();
            _empresaDmAdm = new DynamicModels.Admistrativo.EmpresaDynamicModel();
            _empresaDmSegPj = new DynamicModels.SeguridadPj.EmpresaDynamicModel();
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var salasAdm = Utils.FromExpandoList<Domain.Administrativo.Sala>(_salaDmAdm.All().ToList()).ToList();
                var salasSegPj = Utils.FromExpandoList<Domain.SeguridadPj.Sala>(_salaDmSegPj.All().ToList()).ToList();

                foreach (var salaAdm in salasAdm)
                {
                    var salaSegPj = salasSegPj.FirstOrDefault(x =>
                        x.CodSala == salaAdm.CodSala && x.CodEmpresa == salaAdm.CodEmpresa);

                    if (salaSegPj != null)
                    {
                        _salaDmSegPj.Update(new
                        {
                            salaAdm.Nombre,
                            salaAdm.NombreCorto,
                            salaAdm.IpPublica,
                            salaAdm.UrlCuadre,
                            salaAdm.UrlPlayerTracking,
                            salaAdm.UrlProgresivo,
                            salaAdm.UrlSistemaOnline,
                            FechaModificacion = DateTime.Now
                        }, salaSegPj.CodSala);
                    }
                    else
                    {
                        var expEmpresaAdm = _empresaDmSegPj.Single(salaAdm.CodEmpresa);

                        if (expEmpresaAdm == null)
                        {
                            expEmpresaAdm = _empresaDmAdm.Single(salaAdm.CodEmpresa);
                            if (expEmpresaAdm != null)
                            {
                                var empresaAdm = new Domain.Administrativo.Empresa();
                                new FromExpando().Map(expEmpresaAdm, empresaAdm);
                                var empresaSegPj = Mapper.Map<Domain.SeguridadPj.Empresa>(empresaAdm);
                                _empresaDmSegPj.Insert(empresaSegPj);
                            }
                        }

                        var newSalaSegPj = Mapper.Map<Domain.SeguridadPj.Sala>(salaAdm);
                        if (newSalaSegPj.CodigoEstablecimiento == null)
                        {
                            newSalaSegPj.CodigoEstablecimiento = "";
                        }

                        _salaDmSegPj.Insert(newSalaSegPj);
                    }
                }


            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return Task.CompletedTask;
        }
    }
}
