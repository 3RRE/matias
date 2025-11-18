using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace S3k.Utilitario {
    public static class DynamicTypeBuilder {
        private static readonly AssemblyName AssemblyName = new AssemblyName { Name = "DynamicLinqTypes" };
        private static readonly ModuleBuilder ModuleBuilder;
        private static readonly Dictionary<string, Tuple<string, Type>> BuiltTypes = new Dictionary<string, Tuple<string, Type>>();
        private static readonly Dictionary<string, Type> Types = new Dictionary<string, Type>();

        static DynamicTypeBuilder() {
            ModuleBuilder = Thread.GetDomain()
                                  .DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run)
                                  .DefineDynamicModule(AssemblyName.Name);
        }

        private static string GetTypeKey(Dictionary<string, Type> fields) {
            return fields.OrderBy(v => v.Key)
                         .ThenBy(v => v.Value.Name)
                         .Aggregate(string.Empty, (current, field) => current + (field.Key + ";" + field.Value.Name + ";"));
        }

        public static string GetTypeKey(IEnumerable<PropertyInfo> fields) {
            return GetTypeKey(fields.ToDictionary(f => f.Name, f => f.PropertyType));
        }

        public static Type GetDynamicType(Dictionary<string, Type> fields, Type basetype, Type[] interfaces) {
            if(null == fields) {
                throw new ArgumentNullException("fields");
            }
            if(0 == fields.Count) {
                throw new ArgumentOutOfRangeException("fields", "fields must have at least 1 field definition");
            }

            try {
                Monitor.Enter(BuiltTypes);
                string typeKey = GetTypeKey(fields);

                if(BuiltTypes.ContainsKey(typeKey)) {
                    return BuiltTypes[typeKey].Item2;
                }

                string typeName = "DynamicLinqType_" + BuiltTypes.Count;

                TypeBuilder typeBuilder = ModuleBuilder.DefineType(typeName,
                                                                   TypeAttributes.Public | TypeAttributes.Class |
                                                                   TypeAttributes.Serializable,
                                                                   null,
                                                                   Type.EmptyTypes);

                foreach(var field in fields) {
                    typeBuilder.DefineField(field.Key, field.Value, FieldAttributes.Public);
                }

                BuiltTypes[typeKey] = new Tuple<string, Type>(typeName, typeBuilder.CreateType());

                return BuiltTypes[typeKey].Item2;
            } finally {
                Monitor.Exit(BuiltTypes);
            }
        }

        public static Type CreateType(IEnumerable<PropertyInfo> fields) {
            return CreateType(fields.ToDictionary(f => f.Name, f => f.PropertyType));
        }

        private static TypeBuilder CreateTypeBuilder(string assemblyName, string moduleName, string typeKey) {
            TypeBuilder typeBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(assemblyName),
                                                                                    AssemblyBuilderAccess.Run)
                                               .DefineDynamicModule(moduleName)
                                               .DefineType(typeKey, TypeAttributes.Public);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            return typeBuilder;
        }

        private static void CreateAutoImplementedProperty(TypeBuilder builder, string propertyName, Type propertyType) {
            const string privateFieldPrefix = "m_";
            const string getterPrefix = "get_";
            const string setterPrefix = "set_";

            // Generate the field.
            FieldBuilder fieldBuilder = builder.DefineField(string.Concat(privateFieldPrefix, propertyName),
                                                            propertyType,
                                                            FieldAttributes.Private);

            // Generate the property
            PropertyBuilder propertyBuilder = builder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

            // Property getter and setter attributes.
            const MethodAttributes propertyMethodAttributes =
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            // Define the getter method.
            MethodBuilder getterMethod = builder.DefineMethod(string.Concat(getterPrefix, propertyName),
                                                              propertyMethodAttributes,
                                                              propertyType,
                                                              Type.EmptyTypes);

            // Emit the IL code.
            // ldarg.0
            // ldfld,_field
            // ret
            ILGenerator getterIlCode = getterMethod.GetILGenerator();
            getterIlCode.Emit(OpCodes.Ldarg_0);
            getterIlCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getterIlCode.Emit(OpCodes.Ret);

            // Define the setter method.
            MethodBuilder setterMethod = builder.DefineMethod(string.Concat(setterPrefix, propertyName),
                                                              propertyMethodAttributes,
                                                              null,
                                                              new[] { propertyType });

            // Emit the IL code.
            // ldarg.0
            // ldarg.1
            // stfld,_field
            // ret
            ILGenerator setterIlCode = setterMethod.GetILGenerator();
            setterIlCode.Emit(OpCodes.Ldarg_0);
            setterIlCode.Emit(OpCodes.Ldarg_1);
            setterIlCode.Emit(OpCodes.Stfld, fieldBuilder);
            setterIlCode.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterMethod);
            propertyBuilder.SetSetMethod(setterMethod);
        }

        public static Type CreateType(Dictionary<string, Type> properties, string typeKey = null) {
            try {
                Monitor.Enter(Types);

                if(typeKey == null) {
                    typeKey = GetTypeKey(properties);
                }

                if(Types.ContainsKey(typeKey)) {
                    return Types[typeKey];
                }

                TypeBuilder typeBuilder = ModuleBuilder.DefineType(typeKey,
                                                                   TypeAttributes.Public | TypeAttributes.Class |
                                                                   TypeAttributes.Serializable);

                //TypeBuilder typeBuilder = CreateTypeBuilder("DynamicAssembly", "DynamicModule", typeKey);

                //foreach (var field in properties){
                //    typeBuilder.DefineField(field.Key, field.Value, FieldAttributes.Public);
                //}

                foreach(var type in properties) {
                    CreateAutoImplementedProperty(typeBuilder, type.Key, type.Value);
                }

                Types[typeKey] = typeBuilder.CreateType();

                return Types[typeKey];
            } finally {
                Monitor.Exit(Types);
            }
        }

        public static Type CreateType(Dictionary<string, string> properties, string typeKey = null) {
            if(properties == null) {
                throw new ArgumentNullException("properties");
            }

            Dictionary<string, Type> propertiesParsed = new Dictionary<string, Type>();

            foreach(var property in properties) {
                propertiesParsed[property.Key] = typeof(string);
            }

            return CreateType(propertiesParsed, typeKey);
        }
    }
}
