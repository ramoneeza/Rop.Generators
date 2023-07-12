using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.DerivedFrom.Annotations;
using Test.DerivedFrom.SubForms;

namespace Test.DerivedFrom
{
    public partial class Form1:IFormDerivedFrom<SubForm<int,bool>>
    {
        public string Hola()
        {
            return "Hola";
        }
    }
}
