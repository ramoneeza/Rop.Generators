using Rop.ProxyGenerator.Annotations;

namespace Test.ProxyGenerator;

[ProxyOf(typeof(IDisposable), nameof(MyInterface3))]
public partial class ProxyClass3 : IDisposable
{
    public IDisposable MyInterface3 { get; }

    public ProxyClass3(IDisposable myInterface)
    {
        MyInterface3 = myInterface;
    }
}