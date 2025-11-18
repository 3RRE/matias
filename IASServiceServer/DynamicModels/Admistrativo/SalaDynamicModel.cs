using IASServiceServer.Massive;

namespace IASServiceServer.DynamicModels.Admistrativo
{
    public class SalaDynamicModel : DynamicModel
    {
        //you don't have to specify the connection - Massive will use the first one it finds in your config
        public SalaDynamicModel() : base("Administrativo", "Sala", "CodSala") { }
    }
}
