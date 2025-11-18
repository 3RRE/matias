using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Utilitarios
{
    public class calculosCoordenadas
    {

        public double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            var d1 = latitude * (Math.PI / 180.0);
            var num1 = longitude * (Math.PI / 180.0);
            var d2 = otherLatitude * (Math.PI / 180.0);
            var num2 = otherLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        public int round_up_to_even(double number_to_round)
        {
            int converted_to_int = Convert.ToInt32(number_to_round);
            if (converted_to_int % 2 == 0) { return converted_to_int; }
            double difference = (converted_to_int + 1) - number_to_round;
            if (difference <= 0.5) { return converted_to_int + 1; }
            return converted_to_int - 1;
        }


        ////////////////////////////// metodo 2
        ///
        public double Distancemetodo2(double Latitude1, double Longitude1, double Latitude2, double Longitude2)
        {
            double R = 6371.0;          // R is earth radius.
            double dLat = this.toRadian(Latitude2 - Latitude1);
            double dLon = this.toRadian(Longitude2 - Longitude1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(this.toRadian(Latitude1)) * Math.Cos(this.toRadian(Latitude2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;

            return d;
        }

        private double toRadian(double val)
        {
            return (Math.PI / 180) * val;
        }

        ///////////////////////////////////metodo 3
        ///
        public double GetDistanceBetweenPoints(double lat1, double long1, double lat2, double long2)
        {
            double distance = 0;

            double dLat = (lat2 - lat1) / 180 * Math.PI;
            double dLong = (long2 - long1) / 180 * Math.PI;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                        + Math.Cos(lat2) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            //Calculate radius of earth
            // For this you can assume any of the two points.
            double radiusE = 6378135; // Equatorial radius, in metres
            double radiusP = 6356750; // Polar Radius

            //Numerator part of function
            double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
            //Denominator part of the function
            double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
                            + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
            double radius = Math.Sqrt(nr / dr);

            //Calaculate distance in metres.
            distance = radius * c;
            return distance;
        }

    }

    
}