namespace S3k.Utilitario {
    public static class FileFormatMimeTypes {
        public static string GetMimeType(string extension) {
            switch(extension) {
                case ".doc":
                    return "application/msword";
                case ".dot":
                    return "application/msword";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".dotx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                case ".docm":
                    return "application/vnd.ms-word.document.macroEnabled.12";
                case ".dotm":
                    return "application/vnd.ms-word.template.macroEnabled.12";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".xlt":
                    return "application/vnd.ms-excel";
                case ".xla":
                    return "application/vnd.ms-excel";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".xltx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
                case ".xlsm":
                    return "application/vnd.ms-excel.sheet.macroEnabled.12";
                case ".xltm":
                    return "application/vnd.ms-excel.template.macroEnabled.12";
                case ".xlam":
                    return "application/vnd.ms-excel.addin.macroEnabled.12";
                case ".xlsb":
                    return "application/vnd.ms-excel.sheet.binary.macroEnabled.12";
                case ".ppt":
                    return "application/vnd.ms-powerpoint";
                case ".pot":
                    return "application/vnd.ms-powerpoint";
                case ".pps":
                    return "application/vnd.ms-powerpoint";
                case ".ppa":
                    return "application/vnd.ms-powerpoint";
                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".potx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.template";
                case ".ppsx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
                case ".ppam":
                    return "application/vnd.ms-powerpoint.addin.macroEnabled.12";
                case ".pptm":
                    return "application/vnd.ms-powerpoint.presentation.macroEnabled.12";
                case ".potm":
                    return "application/vnd.ms-powerpoint.presentation.macroEnabled.12";
                case ".ppsm":
                    return "application/vnd.ms-powerpoint.slideshow.macroEnabled.12";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
