namespace IASServiceServer.Domain.Administrativo
{
    public abstract class Etiqueta : Entity
    {
        public string ColorHexa { get; set; }

        public string Sigla { get; set; }
    }
}