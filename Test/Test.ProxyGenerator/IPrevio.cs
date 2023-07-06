using Rop.ProxyGenerator.Annotations;

namespace Test.ProxyGenerator;

public interface IPrevio
{
    [OverrideWithPreBase]
    int MyMethod(int a,int b);
}