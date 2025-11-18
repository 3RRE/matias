using System.Collections.Generic;

namespace S3k.Utilitario.Constants {
    public class Constants {
        public static Dictionary<int, string> UrlVerificationClients = new Dictionary<int, string> {
            //--mambos == 65        (9)
            //--keops == 76         (102)
            //--winmeier == 74      (103)

            { 65, "http://132.251.207.86:7575/api/clientes/verificar/mambos" },
            { 9, "http://132.251.207.86:7575/api/clientes/verificar/mambos" },
            { 76, "http://190.187.35.174:7575/api/clientes/verificar/keops" },
            { 102, "http://190.187.35.174:7575/api/clientes/verificar/keops" },
            { 74, "http://200.31.120.6:7575/api/clientes/verificar/winmeier" },
            { 103, "http://200.31.120.6:7575/api/clientes/verificar/winmeier" },
        };

        public static List<int> SalasNoVerificaCliente = new List<int> {
            //--excalibur ==4        (36)
            //--megacasino ==66
            4, 66
        };
    }
}
