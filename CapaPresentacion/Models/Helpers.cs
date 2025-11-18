using System;
using System.Text.RegularExpressions;

namespace CapaPresentacion.Models
{
    public class Helpers
    {
        public const int CHANNEL_WEB = 1;
        public const int CHANNEL_APP = 2;

        public static void APIReniecLog(int channel, string sala, string username, string dni, string message = "")
        {
            Logger logger = new Logger();

            DateTime currentDate = DateTime.Now;
            string month = currentDate.ToString("MM");
            string year = currentDate.ToString("yyyy");
            string channelName = GetChannelName(channel);

            string fileName = $"apireniec-{channelName}";
            string fileDate = $"{month}-{year}";
            string fileExtension = "log";

            string logDate = currentDate.ToString("dd/MM/yyyy HH:mm:ss");
            string logLine = $"{logDate}|{sala}|{username}|{dni}|{message}";
            string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\logs\\{fileName}-{fileDate}.{fileExtension}";

            logger.WriteLine(logLine, filePath);
        }

        public static string GetChannelName(int channel)
        {
            string channelName = "other";

            if (channel == CHANNEL_WEB)
            {
                channelName = "web";
            }

            if (channel == CHANNEL_APP)
            {
                channelName = "app";
            }

            return channelName;
        }

        public static bool IsValidDNI(string dni)
        {
            string pattern = @"^\d{8}$";

            Regex regex = new Regex(pattern);

            return regex.IsMatch(dni);
        }
    }
}