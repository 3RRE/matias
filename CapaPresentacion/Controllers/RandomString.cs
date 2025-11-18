using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CapaPresentacion.Controllers
{
    public class RandomString
    {
        private readonly HashSet<string> _cache = new HashSet<string>();
        private readonly int _baseLenght = 2;

        public RandomString() { }

        public RandomString(int baseLenght)
        {
            _baseLenght = baseLenght;
        }

        public string GetUniqueString()
        {
            bool status = true;
            string str;
            do
            {
                str = GetString(_baseLenght);
                if (_cache.Add(str))
                {
                    status = false;
                }
            }
            while (status);
            return str;
        }

        public static string[] UniqueStringCollection(int stringLenght, int arraySize)
        {
            if (stringLenght == 0)
            {
                throw new ArgumentException("stringLenght");
            }

            if (arraySize == 0)
            {
                throw new ArgumentException("arraySize");
            }

            var collection = new string[] { };

            for (int i = 0; i < arraySize; i++)
            {
                var str = GetString(stringLenght);

                if (collection.Contains(str))
                {
                    i--;
                }
                else
                {
                    collection[i] = str;
                }
            }

            return collection;
        }

        public static string GetString(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString()
                          .ToLower();
        }
    }
}