using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Web;

namespace S3k.Utilitario.WhatsApp {
    public class UtilitarioWhatsApp {
        public static StatusImage ValidateImage(HttpPostedFileBase image, int maxSize = 16, int maxLenhtBase64 = 10000000) {
            List<string> allowedTypes = new List<string> { "image/jpeg", "image/jpg", "image/gif", "image/png", "image/webp", "image/bmp" };
            List<string> allowedExtensions = new List<string> { ".jpg", ".jpeg", ".gif", ".png", ".webp", ".bmp" };

            // Verificamos si la imagen es nula o no tiene contenido
            if(image == null || image.ContentLength == 0) {
                return StatusImage.Empty;
            }

            // Verificamos si el tipo MIME de la imagen está en la lista de tipos permitidos
            if(!allowedTypes.Contains(image.ContentType)) {
                return StatusImage.ErrorType;
            }

            // Verificamos si la extensión de la imagen está en la lista de extensiones permitidas
            var extension = Path.GetExtension(image.FileName).ToLower();
            if(!allowedExtensions.Contains(extension)) {
                return StatusImage.ErrorExtension;
            }

            //Verificamos si el tamaño de la imagen no supera el maximo
            if(image.ContentLength > maxSize * 1024 * 1024) {
                return StatusImage.ErrorSize;
            }

            //Verificamos la longitud del BASE64
            if(GetLengthBase64Image(image) > maxLenhtBase64) {
                return StatusImage.ErrorLengthBase64;
            }

            return StatusImage.Ok;
        }

        public static string ImageToBase64(HttpPostedFileBase image) {
            using(MemoryStream memoryStream = new MemoryStream()) {
                Stream inputStream = image.InputStream;
                inputStream.CopyTo(memoryStream);
                inputStream.Seek(0, SeekOrigin.Begin);
                string base64String = Convert.ToBase64String(memoryStream.ToArray());
                return base64String;
            }
        }

        public static int GetLengthBase64Image(HttpPostedFileBase image) {
            using(MemoryStream memoryStream = new MemoryStream()) {
                Stream inputStream = image.InputStream;
                inputStream.CopyTo(memoryStream);
                inputStream.Seek(0, SeekOrigin.Begin);
                string base64String = Convert.ToBase64String(memoryStream.ToArray());
                return base64String.Length;
            }
        }

        public static FormUrlEncodedContent ObjectToFormUrlEncodedContent(object obj) {
            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var keyValuePairs = new List<KeyValuePair<string, string>>();

            foreach(var property in properties) {
                var value = property.GetValue(obj)?.ToString();
                var key = property.Name;

                keyValuePairs.Add(new KeyValuePair<string, string>(key, value));
            }

            return new FormUrlEncodedContent(keyValuePairs);
        }

        public static Dictionary<string, string> ObjectToDictionary(object obj) {
            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var keyValuePairs = new Dictionary<string, string>();

            foreach(var property in properties) {
                var value = property.GetValue(obj);
                var key = property.Name;

                if(value != null) {
                    keyValuePairs[key] = value.ToString();
                } else {
                    keyValuePairs[key] = null;
                }
            }

            return keyValuePairs;
        }
    }
}
