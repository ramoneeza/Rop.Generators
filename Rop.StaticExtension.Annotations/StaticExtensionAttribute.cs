namespace Rop.StaticExtension.Annotations;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class StaticExtensionAttribute<T>:Attribute
{
    public StaticExtensionAttribute()
    {
    }
}