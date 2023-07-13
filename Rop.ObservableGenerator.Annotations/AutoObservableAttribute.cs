using System;

namespace Rop.ObservableGenerator.Annotations
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple = false)]
    public class AutoObservableAttribute : Attribute
    {
        public AutoObservableAttribute(bool skipmethodeventgenerator=false)
        {
        }
    }
    

}
