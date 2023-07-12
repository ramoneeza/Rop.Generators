using Rop.StaticExtension.Annotations;

namespace Test.SubDll;

public class BaseForm<A, B>
{
    public B ItemB { get; set; }
    public A SoyDeBaseForm()
    {
        return default;
    }

    [StaticExtension]
    public static Result<A> Execute<T>(B item) where T : BaseForm<A, B>,new()
    {
        var t = new T();
        t.ItemB = item;
        var r=new Result<A>();
        r.Value= t.SoyDeBaseForm();
        return r;
    }
}
public class Result<T>
{
    public T Value { get; set; }
}