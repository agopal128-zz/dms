using System;
using System.ComponentModel;
using System.Reflection;

namespace NDMS.Helpers
{
    public static class EnumUtility
    {
        public static string GetDescription(Enum enumValue)
        {

            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi)
            {
                object[] attrs = fi.GetCustomAttributes
                        (typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return enumValue.ToString();
        }
    }
}
