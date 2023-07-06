using Rop.Winforms7.Annotations;

namespace Test.StaticExtension;
 

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Form1.Execute(true);
    }
}

public class BaseForm<A, B>
{
    public B ItemB { get; set; }
    public A SoyDeBaseForm()
    {
        return default;
    }
}

public class MyStaticExtension<T,A,B> where T : BaseForm<A, B>,new()
{
    public static A Execute(B b)
    {
        var t= new T();
        t.ItemB= b;
        return t.SoyDeBaseForm();
    }
}


[StaticExtension<MyStaticExtension<Form1,int,bool>>]
public partial class Form1 : BaseForm<int, bool>
{
    public void Hola()
    {
        var x = SoyDeBaseForm();
        Console.WriteLine(x.ToString());
    }

    public static int Execute2(bool b)=>MyStaticExtension<Form1,int,bool>.Execute(b);
}