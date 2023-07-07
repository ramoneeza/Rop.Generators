using Rop.DerivedFrom.Annotations;

namespace Test.Derived;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var f=new Form1();
        f.Hola();
    }
}

public class BaseForm<A, B>
{
    public A SoyDeBaseForm()
    {
        return default;
    }
}

public partial class Form1 : IDerivedFrom<BaseForm<int, bool>>
{
    public void Hola()
    {
        var x = SoyDeBaseForm();
        Console.WriteLine(x.ToString());
    }
}