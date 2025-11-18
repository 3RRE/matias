using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Runtime.Serialization;

namespace CapaDatos.Utilitarios
{
    public class DynamicDictionary : DynamicObject
    {
        private readonly StreamingContext _ctx;
        // The inner dictionary.
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public DynamicDictionary() { }

        protected DynamicDictionary(SerializationInfo info, StreamingContext ctx)
        {
            _ctx = ctx;
            foreach (var field in info)
            {
                var fieldName = field.Name;
                var fieldValue = field.Value;

                if (string.IsNullOrWhiteSpace(fieldName))
                {
                    continue;
                }

                if (fieldValue == null)
                {
                    continue;
                }

                _dictionary.Add(fieldName, fieldValue);
            }
        }

        // This property returns the number of elements
        // in the inner dictionary.
        public int Count
        {
            get { return _dictionary.Count; }
        }

        public StreamingContext Ctx
        {
            get { return _ctx; }
        }

        // If you try to get a value of a property 
        // not defined in the class, this method is called.
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            string name = binder.Name.ToLower();

            // If the property name is found in a dictionary,
            // set the result parameter to the property value and return true.
            // Otherwise, return false.
            return _dictionary.TryGetValue(name, out result);
        }

        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            _dictionary[binder.Name.ToLower()] = value;

            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }

        public IDictionary<string, object> GetDictionary()
        {
            return _dictionary;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var kvp in _dictionary)
            {
                info.AddValue(kvp.Key, kvp.Value);
            }
        }
    }
}
