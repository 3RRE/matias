using IASServiceServer.Massive;

namespace IASServiceServer.DynamicModels.Admistrativo
{
    public class EmpresaDynamicModel : DynamicModel
    {
        //you don't have to specify the connection - Massive will use the first one it finds in your config
        public EmpresaDynamicModel() : base("Administrativo", "Empresa", "CodEmpresa") { }
    }
}
