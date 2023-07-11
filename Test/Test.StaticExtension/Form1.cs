namespace Test.StaticExtensions;
using Rop.StaticExtension.Annotations;

[InsertStaticExtensions]
public partial class Form1 : BaseFormIntBool
{
    public void Hola()
    {
        var x = SoyDeBaseForm();
        Console.WriteLine(x.ToString());
        Execute(true);
    }
}

public class BaseFormIntBool : Test.SubDll.BaseForm<int, bool>
{
}