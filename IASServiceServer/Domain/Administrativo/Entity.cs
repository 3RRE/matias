using System;

namespace IASServiceServer.Domain.Administrativo
{
    public abstract class Entity
    {
        protected Entity()
        {
            Activo = true;
            Estado = 1;
        }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string CodUsuario { get; set; }

        public int Estado { get; set; }

        public bool Activo { get; set; }
    }
}