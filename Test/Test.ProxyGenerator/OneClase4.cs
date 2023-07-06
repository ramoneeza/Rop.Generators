using Rop.ProxyGenerator.Annotations;

namespace Test.ProxyGenerator;

[ProxyOf(typeof(IOv1), nameof(OneClase4.Ovi))]
public partial class OneClase4:OneClase3, IOv1
{
    public IOv1 Ovi { get; }
    public OneClase4(IOv1 ovi)
    {
        Ovi = ovi;
    }
}