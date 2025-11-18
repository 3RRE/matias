using System.Data;

namespace S3k.Utilitario.Excel {
    public class ExportExcel {
        public string Title { get; set; } = string.Empty;
        public DataTable Data { get; set; } = new DataTable();
        public string FileName { get; set; } = string.Empty;
        public string SheetName { get; set; } = string.Empty;
        public bool FirstColumNumber { get; set; }
    }
}
