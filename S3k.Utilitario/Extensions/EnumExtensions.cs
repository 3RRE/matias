using S3k.Utilitario.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using EnumClass = System.Enum;

namespace S3k.Utilitario.Extensions {
    public static class EnumExtensions {
        public static string GetDisplayText(this EnumClass value) {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }

        public static List<EnumItemDto> ToListEnumItems<TEnum>() where TEnum : EnumClass {
            return EnumClass.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                       .Select(e => new EnumItemDto {
                           Valor = Convert.ToInt32(e),
                           Texto = e.GetDisplayText()
                       })
                       .ToList();
        }
    }
}
