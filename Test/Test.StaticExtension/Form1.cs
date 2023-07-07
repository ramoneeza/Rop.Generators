namespace Test.StaticExtensions;
using Rop.StaticExtension.Annotations;

[StaticExtension<MyStaticExtension<Form1,int,bool>>()]
public partial class Form1 : BaseForm<int, bool>
{
    public void Hola()
    {
        var x = SoyDeBaseForm();
        Console.WriteLine(x.ToString());
    }

    public static int Execute2(bool b)=>MyStaticExtension<Form1,int,bool>.Execute(b);
}