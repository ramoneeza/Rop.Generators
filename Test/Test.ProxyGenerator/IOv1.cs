using System.ComponentModel;
using Rop.ProxyGenerator.Annotations;

namespace Test.ProxyGenerator;

public interface IOv1:IPrevio
{
    [OverrideWithPostBase]
    int MyMethod2(int a,int b);
    [ExplicitOverrideNoBase]
    int MyMethod3(int a,int b);
    [ExplicitOverrideNoBase]
    int MyMethod4(int a,int b);
    int MyMethod5();
    [IncludeNextAttributes]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    int MyMethod6();
}