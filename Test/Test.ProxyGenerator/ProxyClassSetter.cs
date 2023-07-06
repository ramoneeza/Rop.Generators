using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.ProxyGenerator.Annotations;

namespace Test.ProxyGenerator
{

    [ProxyOf(typeof(ISoloGetters), nameof(ProxyClassSetter._soloGetters))]
    [ProxyOfDisableNullable]
    public partial class ProxyClassSetter:ISoloGetters
    {
        private SoloGettersDecorator _soloGetters;

        public int TestIt()
        {
            this.SoloGetter = 1;
            this.SoloGetterString = "1";
            return this.SoloGetter;
        }

        public ProxyClassSetter()
        {
            _soloGetters = new SoloGettersDecorator();
        }

        private class SoloGettersDecorator : ISoloGetters
        {
            public int SoloGetter { get; set; }
            public string SoloGetterString { get; set; }
        }
    }
}
