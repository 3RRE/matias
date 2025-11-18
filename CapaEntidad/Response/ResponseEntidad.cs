using System.Collections.Generic;

namespace CapaEntidad.Response {
    public class ResponseEntidad<T> where T : new() {
        public bool success { get; set; }
        public string displayMessage { get; set; }
        public T data { get; set; } = new T();
        public List<string> errorMessage { get; set; } = new List<string>();

        public void CreateResponse(bool success, T data, string message) {
            this.success = success;
            this.data = data;
            this.displayMessage = message;
        }
        
        public void CreateNoDataSuccessResponse(string message) {
            success = true;
            displayMessage = message;
        }

        public void CreateSuccessResponse(T data, string message) {
            success = true;
            this.data = data;
            displayMessage = message;
        }

        public void CreateErrorResponse(string message) {
            success = false;
            displayMessage = message;
        }
    }
}
