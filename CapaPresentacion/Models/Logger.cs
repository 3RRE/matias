using System;
using System.IO;

namespace CapaPresentacion.Models
{
    public class Logger
    {
        public bool WriteLine(string logLine, string fileName, bool overwrite = false)
        {
            try
            {
                Stream stream = null;
                FileInfo fileInfo = new FileInfo(fileName);
                DirectoryInfo directoryInfo = new DirectoryInfo(fileInfo.DirectoryName);

                if (!directoryInfo.Exists) directoryInfo.Create();

                if (fileInfo.Exists)
                {
                    if (overwrite)
                    {
                        fileInfo.Delete();
                        stream = fileInfo.Create();
                    }
                    else
                    {
                        stream = new FileStream(fileName, FileMode.Append);
                    }
                }
                else
                {
                    stream = fileInfo.Create();
                }

                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.WriteLine(logLine);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return File.Exists(fileName);
        }
    }
}