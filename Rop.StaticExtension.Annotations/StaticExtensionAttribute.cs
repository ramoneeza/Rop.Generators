namespace Rop.StaticExtension.Annotations;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class StaticExtensionAttribute:Attribute
{
    public StaticExtensionAttribute()
    {
    }
}
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class InsertStaticExtensions:Attribute
{
    public InsertStaticExtensions()
    {
    }
}