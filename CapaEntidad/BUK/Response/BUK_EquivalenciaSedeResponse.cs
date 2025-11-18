namespace CapaEntidad.BUK.Response {
    public class BUK_EquivalenciaSedeResponse {
        public int CantidadRegistros { get => RegistrosInsertadosCorrectamente + RegistrosInsertadosIncorrectamente + RegistrosActualizadosCorrectamente + RegistrosActualizadosIncorrectamente + RegistrosExistentes; }
        public int RegistrosInsertadosCorrectamente { get; set; }
        public int RegistrosInsertadosIncorrectamente { get; set; }
        public int RegistrosActualizadosCorrectamente { get; set; }
        public int RegistrosActualizadosIncorrectamente { get; set; }
        public int RegistrosExistentes { get; set; }
    }
}
