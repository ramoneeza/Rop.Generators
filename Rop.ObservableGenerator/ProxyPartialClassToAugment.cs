using System.Linq;

namespace Rop.ObservableGenerator
{
    public class ProxyPartialClassToAugment
    {
        public PartialClassToAugment Original { get; }
        public FieldToAugment[] Fields { get; }
        public ProxyPartialClassToAugment(PartialClassToAugment original, FieldToAugment[] fields)
        {
            Original = original;
            Fields = fields;
        }
    }
}