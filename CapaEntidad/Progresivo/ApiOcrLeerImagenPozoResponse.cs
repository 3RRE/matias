using System.Collections.Generic;

namespace CapaEntidad.Progresivo {
    public class ApiOcrLeerImagenPozoResponse {
        public List<OcrNumber> valores;
        public List<float> numeros;
    }

    public class OcrNumber {
        public string nombre;
        public float valor;
    }
}