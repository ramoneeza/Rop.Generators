using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.ProxyGenerator.Annotations;

namespace Test.ProxyGenerator
{
    public interface ISoloGetters
    {
        [AddSetterOnProxy]
        int SoloGetter { get; }
        [AddSetterOnProxy]
        string SoloGetterString { get; }
    }
}
