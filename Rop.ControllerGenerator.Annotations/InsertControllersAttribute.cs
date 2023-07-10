using System;
using System.ComponentModel;

namespace Rop.ControllerGenerator.Annotations
{
    [Description("Use 'InitControllers' into constructor to initialize the controllers")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InsertControllersAttribute : Attribute
    {
        public InsertControllersAttribute(){}
    }
}

