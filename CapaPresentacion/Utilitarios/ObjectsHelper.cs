using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CapaPresentacion.Utilitarios
{
    public class ResultadoEquals
    {
        public int Pozo { get; set; }
        public string Campo { get; set; } = string.Empty;
        public string Anterior { get; set; }
        public string Nuevo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }

    public class ObjectsHelper
    {
        public List<ResultadoEquals> CompareObjects<T>(T object1, T object2, List<string> noCompareProperties, int index = 0)
        {
            var type = typeof(T);

            if (object1 == null || object2 == null || object1.GetType() != object2.GetType())
            {
                return new List<ResultadoEquals>();
            }

            List<ResultadoEquals> resultados = new List<ResultadoEquals>();

            foreach (var property in type.GetProperties())
            {
                ResultadoEquals resultado = new ResultadoEquals();

                resultado.Pozo = index;
                resultado.Campo = property.Name;
                resultado.Nuevo = property.GetValue(object1)?.ToString();
                resultado.Anterior = property.GetValue(object2)?.ToString();

                var value1 = property.GetValue(object1);
                var value2 = property.GetValue(object2);

                if (noCompareProperties.Contains(property.Name))
                {
                    resultado.Mensaje = "Se ignora";
                    resultado.Estado = true;
                    resultados.Add(resultado);

                    continue;
                }

                if (value1 == null && value2 == null)
                {
                    resultado.Mensaje = "Ambos son NULL";
                    resultado.Estado = true;

                    continue;
                }
                else if (value1 == null || value2 == null)
                {
                    resultado.Mensaje = "Uno de los valores es null";
                    resultado.Estado = false;
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    DateTime dt1 = (DateTime)value1;
                    DateTime dt2 = (DateTime)value2;

                    resultado.Estado = dt1.ToString("dd/MM/yyyy HH:mm:ss").Equals(dt2.ToString("dd/MM/yyyy HH:mm:ss"));

                    resultado.Mensaje = resultado.Estado ? "Son Iguales" : "Son diferentes";
                }
                else if (IsPrimitiveType(property.PropertyType) || property.PropertyType == typeof(string) || property.PropertyType == typeof(decimal))
                {
                    resultado.Mensaje = value1.Equals(value2) ? "Son Iguales" : "Son diferentes";
                    resultado.Estado = value1.Equals(value2);
                }
                else if (IsListType(property.PropertyType))
                {
                    var list1 = (IEnumerable)value1;
                    var list2 = (IEnumerable)value2;

                    var enumerator1 = list1.GetEnumerator();
                    var enumerator2 = list2.GetEnumerator();

                    List<ResultadoEquals> compracionLista = new List<ResultadoEquals>();

                    int indexPozo = 1;

                    while (enumerator1.MoveNext() && enumerator2.MoveNext())
                    {
                        if (enumerator1.Current == null || enumerator2.Current == null)
                        {
                            continue;
                        }

                        Type listElementType = property.PropertyType.GetGenericArguments().FirstOrDefault();

                        MethodInfo compareObjectsMethod = typeof(ObjectsHelper).GetMethod("CompareObjects").MakeGenericMethod(listElementType);

                        List<ResultadoEquals> sublistResultados = (List<ResultadoEquals>)compareObjectsMethod.Invoke(this, new object[] { enumerator1.Current, enumerator2.Current, noCompareProperties, indexPozo });

                        compracionLista.AddRange(sublistResultados);

                        indexPozo++;
                    }

                    resultados.AddRange(compracionLista);
                    resultado.Mensaje = compracionLista.All(x => x.Estado) ? "Listas iguales" : "Listas diferentes";
                    resultado.Estado = compracionLista.All(x => x.Estado);
                }
                else if (IsObjectType(property.PropertyType))
                {
                    Type listElementType = property.PropertyType;

                    MethodInfo compareObjectsMethod = typeof(ObjectsHelper).GetMethod("CompareObjects").MakeGenericMethod(listElementType);

                    List<ResultadoEquals> sublistResultados = (List<ResultadoEquals>)compareObjectsMethod.Invoke(this, new object[] { value1, value2, index });
                }
                else
                {
                    resultado.Mensaje = "Son Iguales";
                    resultado.Estado = true;
                }

                resultados.Add(resultado);
            }

            return resultados;
        }

        private bool IsPrimitiveType(Type type)
        {
            return type.IsPrimitive || type == typeof(Guid);
        }

        private bool IsListType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        private bool IsObjectType(Type type)
        {
            return !type.IsPrimitive && type != typeof(Guid);
        }
    }
}