using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Utilitarios
{
    public static class Conversiones
    {
        //Metodo: Convierte a valores de tipo structura
        //Creado por: Oscar Idiaquez
        //Fecha: 04/06/2013
        public static T Valor<T>(object dbValue) where T : struct
        {
            T Item;
            if (dbValue == null)
            {
                return default(T);
            }
            if (dbValue.Equals(DBNull.Value))
            {
                return default(T);
            }

            if (string.IsNullOrWhiteSpace(dbValue.ToString()))
            {
                return default(T);
            }
            try
            {
                TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                Item = (T)conv.ConvertFrom(dbValue.ToString());
            }
            catch
            {
                Item = default(T);
            }


            return Item;
        }


        public static T Valor<T>(object dbValue, System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture) where T : struct
        {
            T Item;
            if (dbValue == null)
            {
                return default(T);
            }
            if (dbValue.Equals(DBNull.Value))
            {
                return default(T);
            }

            if (string.IsNullOrWhiteSpace(dbValue.ToString()))
            {
                return default(T);
            }
            try
            {
                TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                Item = (T)conv.ConvertFrom(context, culture, dbValue.ToString());
            }
            catch
            {
                Item = default(T);
            }


            return Item;
        }




        public static Nullable<T> DbValueToNullable<T>(Object dbValue) where T : struct
        {
            Nullable<T> returnValue = null;
            if (string.IsNullOrWhiteSpace(dbValue.Trim()))
            {
                return returnValue;
            }
            if (dbValue != null && !dbValue.Equals(DBNull.Value))
            {
                TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                returnValue = (T)conv.ConvertFrom(dbValue.ToString());
            }
            return returnValue;
        }

        /// <summary>
        /// Quita espacios en blanco
        /// </summary>        
        public static string Trim(this object dbValue)
        {
            string valor = Convert.ToString(dbValue).Trim();
            return valor;
        }

        /// <summary>
        /// Quita espacios en blanco
        /// </summary>        
        public static string RTrim(this object dbValue)
        {
            string valor = Convert.ToString(dbValue).Trim();
            return valor;
        }

        /// <summary>
        /// Construye DataTable
        /// </summary>
        /// <param name="nombretabla">Nombre de Table</param>
        /// <param name="columnas">Parametros de Tabla Lista("NombreColumna;TipoDato") <example>lista.AddRange("Nombre;String,Edad;Int32")</example></param>
        /// <returns>DataTable</returns>
        public static DataTable InstanciarDataTable(string nombretabla, params string[] columnas)
        {
            DataTable dt = new DataTable();
            dt.TableName = nombretabla;

            foreach (string columna in columnas)
            {
                dt.Columns.Add(CrearColumna(columna));
            }
            return dt;
        }

        /// <summary>
        /// Crea un DataColumn
        /// </summary>
        /// <param name="nombre">es Valor de "NombreColumns" con su TipoDato separados por ";" <example>"Edad;Int32"</example></param>
        /// <returns>DataColumn</returns>
        public static DataColumn CrearColumna(string nombre)
        {

            string[] Datos = nombre.Split(";".ToArray());
            DataColumn columna = new DataColumn();
            columna.DataType = Type.GetType("System." + Datos[1]);
            columna.ColumnName = Datos[0];
            return columna;
        }
        public static string GetTable(string nombre, IEnumerable codigos)
        {
            DataTable table = new DataTable(nombre);
            table.Columns.Add("item", typeof(string));
            //Conversiones.InstanciarDataTable(nombre, "n;Int32");
            foreach (var item in codigos)
            {
                table.Rows.Add(item);
            }
            System.IO.StringWriter ms = new System.IO.StringWriter();
            table.WriteXml(ms);
            string msa = ms.ToString();
            return msa;
        }

        public static String ToHexa(String valor)
        {
            UInt64 uiDecimal = 0;
            uiDecimal = checked((UInt64)System.Convert.ToUInt64(valor));
            return String.Format("{0:x2}", uiDecimal);
        }

        public static T Copia<T>(T origen)
        {
            // Verificamos que sea serializable antes de hacer la copia            
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("La clase " + typeof(T).ToString() + " no es serializable");

            // En caso de ser nulo el objeto, se devuelve tal cual
            if (Object.ReferenceEquals(origen, null))
                return default(T);

            //Creamos un stream en memoria            
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                try
                {
                    formatter.Serialize(stream, origen);
                    stream.Seek(0, SeekOrigin.Begin);
                    //Deserializamos la porcón de memoria en el nuevo objeto                
                    return (T)formatter.Deserialize(stream);
                }
                catch (SerializationException ex)
                { throw new ArgumentException(ex.Message, ex); }
                catch { throw; }
            }
        }


        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            hex = hex.Replace("-", "");
            return hex;
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }


        public static byte[] ToByteArray(String hexString)              // convierte cadena hexadecimal a byte
        {
            byte[] retval = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
                retval[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            return retval;
        }

        public static string ToHexaString(byte[] hexbyte)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hexbyte)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }



        public static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.UTF8.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
        }

        public static string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.UTF8.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }
    }
}
