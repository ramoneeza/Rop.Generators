using System.Runtime.CompilerServices;
using Rop.ProxyGenerator.Annotations;

namespace Test.ProxyGenerator;

[ProxyOf(typeof(IMyInterface<bool>), nameof(MyInterface), new string[] { "a", nameof(IMyInterface<bool>.Fecha), "b" })]
public partial class ProxyClass: IMyInterface<bool>
{
    public IMyInterface<bool> MyInterface { get; set; }
    //public DateTime Fecha { get; set; }
    
    public object SubExcluido { get; }
    public DateTime Fecha { get; set; }
}