using System;

namespace Rop.ProxyGenerator.Annotations
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public class ProxyOfDisableNullableAttribute : Attribute
    {
    }
}