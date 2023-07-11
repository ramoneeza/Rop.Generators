using Rop.StaticExtension.Annotations;

namespace Test.StaticExtensions;

public class BaseForm<A, B>
{
    public B ItemB { get; set; }
    public A SoyDeBaseForm()
    {
        return default;
    }

    [StaticExtension]
    public static A Execute<T>(B item) where T : BaseForm<A, B>,new()
    {
        var t = new T();
        t.ItemB = item;
        return t.SoyDeBaseForm();
    }
}