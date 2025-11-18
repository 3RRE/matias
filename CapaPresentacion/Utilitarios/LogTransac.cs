using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
namespace CapaPresentacion.Utilitarios
{
    public class LogTransac
    {
        public void escribir_logOK(string ruta,string message)
        {
            var fechacarpeta = DateTime.Now.ToString("dd_MM_yyyy");
            string ubicacion = ruta + "\\"+ fechacarpeta + "\\";
            try
            {
                
                if (Directory.Exists(ubicacion))
                {
                    StreamWriter writeReportFile = File.AppendText(ubicacion+"log.txt");
                    //Write a line of text
                    writeReportFile.WriteLine("Fecha : " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + " || Message :" + message);
                    //Close the file
                    writeReportFile.Close();
                }
                else
                {
                    Directory.CreateDirectory(ubicacion);
                    StreamWriter writeReportFile = File.AppendText(ubicacion + "log.txt");
                    //Write a line of text
                    writeReportFile.WriteLine("Fecha : " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + " || Message :" + message);
                    //Close the file
                    writeReportFile.Close();
                }
            }
            catch (Exception e)
            {
                StreamWriter writeReportFile = File.AppendText(ubicacion + "log.txt");
                //Write a line of text
                writeReportFile.WriteLine("Fecha : " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + " || error :" + e.Message);
                //Close the file
                writeReportFile.Close();
            }

        }
        public void escribir_log(string trace, string source, string message)
        {
            try
            {
                string ruta = "C:\\log\\";
                if (Directory.Exists(ruta))
                {
                    StreamWriter writeReportFile = File.AppendText(@"C:\\log\\asistencia\\Log_Operaciones.txt");
                    //Write a line of text
                    writeReportFile.WriteLine("Fecha : " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + " || trace :" + trace + " || Source :" + source + " || Message :" + message);
                    //Close the file
                    writeReportFile.Close();
                }
                else
                {
                    Directory.CreateDirectory(ruta);
                    StreamWriter writeReportFile = File.AppendText(@"C:\\log\\asistencia\\Log_Operaciones.txt");
                    //Write a line of text
                    writeReportFile.WriteLine("Fecha :" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + " || trace :" + trace + " || Source :" + source + " || Message :" + message);
                    //Close the file
                    writeReportFile.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //throw;
            }

        }
    }
}
