using System;
using System.Reflection;

namespace System
{
    public static class AttributeExtension
    {
        public static T GetAttribute<T>(this object obj) where T : class
        {
            return AttributeExtension.GetAttribute<T>(obj.GetType());
        }

        public static T GetAttribute<T>(this Type type) where T : class
        {
            Attribute customAttribute = type.GetCustomAttribute(typeof(T));
            if (!customAttribute.IsStringEmpty())
                return customAttribute as T;
            return default(T);
        }
    }
}