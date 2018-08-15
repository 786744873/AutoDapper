﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace XDF.DapperLambda.Helper
{
    internal static class ReflectExtension
    {
        internal static PropertyInfo[] GetProperties(this object obj)
        {
            return obj.GetType().GetProperties().Where(f => !Attribute.IsDefined(f, typeof(DatabaseGeneratedAttribute))).ToArray();
        }
    }
}