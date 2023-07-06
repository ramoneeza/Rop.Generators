using Rop.ProxyGenerator.Annotations;

namespace Test.ProxyGenerator;

[ProxyOf("IMyInterface<T>", nameof(MyInterface))]
public partial class ProxyClassGen<T> : IMyInterface<T>
{
    public IMyInterface<T> MyInterface { get; set; }
    public object SubExcluido { get; }
}