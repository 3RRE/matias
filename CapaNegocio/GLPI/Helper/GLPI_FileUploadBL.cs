using CapaEntidad.GLPI.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CapaNegocio.GLPI.Helper {
    public class GLPI_FileUploadBL {
        private readonly string basePath;
        private readonly long maxFileSize;
        private readonly Dictionary<string, List<string>> allowedFileTypes;

        public GLPI_FileUploadBL(long maxFileSizeInMB = 20) {
            basePath = ConfigurationManager.AppSettings["PathArchivos"];
            maxFileSize = maxFileSizeInMB * 1024 * 1024;
            allowedFileTypes = new Dictionary<string, List<string>>
            {
                {".jpg", new List<string> {"image/jpeg"}},
                {".jpeg", new List<string> {"image/jpeg"}},
                {".png", new List<string> {"image/png"}},
                {".pdf", new List<string> {"application/pdf"}},
                {".doc", new List <string> { "application/msword" }},
                {".docx", new List<string>
                    {
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                        "application/vnd.ms-word.document.macroEnabled.12"
                    }
                }
            };
        }

        public async Task<GLPI_FileUpload> UploadFileAsync(HttpPostedFileBase file, List<string> subFolders) {
            //if(file == null || file.ContentLength == 0) {
            //    return new GLPI_FileUpload {
            //        Success = false,
            //        DisplayMessage = "No se ha proporcionado ningún archivo."
            //    };
            //}

            if(file != null && file.ContentLength > 0) {
                if(!IsValidFileSize(file.ContentLength)) {
                    return new GLPI_FileUpload {
                        Success = false,
                        DisplayMessage = $"El archivo excede el tamaño máximo permitido ({maxFileSize}MB)."
                    };
                }
                if(!IsValidExtension(file.FileName)) {
                    return new GLPI_FileUpload {
                        Success = false,
                        DisplayMessage = $"El tipo de archivo no está permitido. {GetAllowedExtensionsMessage()}"
                    };
                }

                if(!IsValidMimeType(file.FileName, file.ContentType)) {
                    return new GLPI_FileUpload {
                        Success = false,
                        DisplayMessage = "El formato del archivo no es válido."
                    };
                }
                try {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                    string fullPath = basePath;
                    string relativePath = string.Empty;
                    if(subFolders != null && subFolders.Any()) {
                        relativePath = Path.Combine(subFolders.ToArray());
                        fullPath = Path.Combine(basePath, relativePath);
                    }
                    Directory.CreateDirectory(fullPath);

                    string fullFilePath = Path.Combine(fullPath, fileName);
                    string relativeFilePath = Path.Combine(relativePath, fileName);

                    using(FileStream stream = new FileStream(fullFilePath, FileMode.Create)) {
                        await file.InputStream.CopyToAsync(stream);
                    }

                    return new GLPI_FileUpload {
                        Success = true,
                        DisplayMessage = "Archivo subido exitosamente.",
                        Path = relativeFilePath
                    };
                } catch(Exception ex) {
                    return new GLPI_FileUpload {
                        Success = false,
                        DisplayMessage = $"Error al subir el archivo: {ex.Message}"
                    };
                }
            }
            return null;

            
        }

        public bool DeleteFile(string filePath) {
            try {
                if(File.Exists(filePath)) {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            } catch {
                return false;
            }
        }

        private bool IsValidFileSize(long fileSize) {
            return fileSize > 0 && fileSize <= maxFileSize;
        }

        private bool IsValidExtension(string fileName) {
            string extension = Path.GetExtension(fileName)?.ToLowerInvariant();
            return !string.IsNullOrEmpty(extension) && allowedFileTypes.ContainsKey(extension);
        }

        private bool IsValidMimeType(string fileName, string mimeType) {
            string extension = Path.GetExtension(fileName)?.ToLowerInvariant();
            if(string.IsNullOrEmpty(extension) || !allowedFileTypes.ContainsKey(extension)) {
                return false;
            }

            return allowedFileTypes[extension].Contains(mimeType.ToLowerInvariant());
        }

        private string GetAllowedExtensionsMessage() {
            return $"Tipos de archivo permitidos: {string.Join(", ", allowedFileTypes.Keys)}";
        }
    }
}
