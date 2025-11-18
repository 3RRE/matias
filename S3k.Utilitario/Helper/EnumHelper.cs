using S3k.Utilitario.Enum;

namespace S3k.Utilitario.Helper {
    public class EnumHelper {
        public static int GetDocumentTypePlayerTracking(DocumentTypePlayerTracking documentTypePlayerTracking) {
            int value = 0;
            switch(documentTypePlayerTracking) {
                case DocumentTypePlayerTracking.DocumentoIdentidad:
                    value = 1;
                    break;
                case DocumentTypePlayerTracking.Pasarpote:
                    value = 2;
                    break;
                case DocumentTypePlayerTracking.CarneExtranjeria:
                    value = 3;
                    break;
            }
            return value;
        }
    }
}
