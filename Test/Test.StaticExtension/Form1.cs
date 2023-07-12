namespace Test.StaticExtensions;
using Rop.StaticExtension.Annotations;

[InsertStaticExtensions]
public partial class Form1 : BaseFormIntBool
{
    public void Hola()
    {
        var x = SoyDeBaseForm();
        Console.WriteLine(x.ToString());
        
    }
}

public class BaseFormIntBool : Test.SubDll.BaseForm<Nullable<int>, bool>
{
}

[InsertStaticExtensions]
public partial class Form2 : Form1
{
    public void Adios()
    {
        var x = SoyDeBaseForm();
        Console.WriteLine(x.ToString());
        
    }
}
[InsertStaticExtensions]
public partial class Form3:IDerived<BaseFormIntBool>{
    public void Hola()
    {
        Console.WriteLine("Form3");
    }
}

public interface IDerived<T> where T : BaseFormIntBool
{
}

