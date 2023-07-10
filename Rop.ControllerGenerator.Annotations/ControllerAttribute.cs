using System;

namespace Rop.ControllerGenerator.Annotations
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ControllerAttribute : Attribute
    {
        public ControllerAttribute()
        {
        }
    }
}