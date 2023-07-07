namespace Test.StaticExtensions;

public class MyStaticExtension<T,A,B> where T : BaseForm<A, B>,new()
{
    public static A Execute(B b)
    {
        var t= new T();
        t.ItemB= b;
        return t.SoyDeBaseForm();
    }
}