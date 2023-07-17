using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rop.DerivedFrom.Annotations
{
    public interface IDerivedFrom<out T>
    {
    }
    public interface IFormDerivedFrom<out T>
    {
    }
    public interface IFormFlat<out T>
    {
    }
}
