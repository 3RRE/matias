namespace CapaEntidad.Cortesias {
    public class CRT_Maquina {
        public string CodMaquina { get; set; }
        public int Zona { get; set; }
        public int Posicion { get; set; }
        public int Isla { get; set; }

        public string Id { get; set; }
        public int CodSala { get; set; }
        public string Sala { get; set; }

        public string NombreZona { get; set; }
        public string NombreIsla { get; set; }
    }

    public class CRT_MaquinaAnfitriona {
        public string CodMaquina { get; set; }
        public int IdUsuario { get; set; }
    }
}
