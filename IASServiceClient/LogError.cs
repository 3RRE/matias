using System;
using System.Globalization;
using System.IO;

namespace IASServiceClient
{
    public class LogError
    {
        public void escribir_log(string nombreMetodo, string message)
        {
            try
            {
                const string ruta = "C:\\log\\";
                if (Directory.Exists(ruta))
                {
                    var writeReportFile = File.AppendText(@"C:\\log\\log_ServidorCentral.txt");
                    //Write a line of text
                    writeReportFile.WriteLine("fecha :" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + " || nombre_metodo :" + nombreMetodo + " || Message :" + message);
                    //Close the file
                    writeReportFile.Close();
                }
                else
                {
                    Directory.CreateDirectory(ruta);
                    StreamWriter writeReportFile = File.AppendText(@"C:\\log\\log_ServidorCentral.txt");
                    //Write a line of text
                    writeReportFile.WriteLine("fecha :" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + " || nombre_metodo :" + nombreMetodo + " || Message :" + message);
                    //Close the file
                    writeReportFile.Close();
                }
            }
            catch (Exception)
            {
                //throw;
            }

        }
        
    }
}
