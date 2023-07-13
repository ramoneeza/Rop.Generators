using System;

namespace Rop.ObservableGenerator.Annotations
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple = false)]
    public class AutoNotifyAttribute : Attribute
    {
    }
}