namespace Test.StaticExtensions;

public class BaseForm<A, B>
{
    public B ItemB { get; set; }
    public A SoyDeBaseForm()
    {
        return default;
    }
}