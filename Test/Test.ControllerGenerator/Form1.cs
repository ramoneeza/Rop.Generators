using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.ControllerGenerator.Annotations;

namespace Test.ControllerGenerator
{
    [InsertControllers]
    public partial class Form1:Form
    {
       
        public Form1()
        {
            InitControllers();
            Console.WriteLine(MainController!.Test);
            Console.WriteLine(ExternalController!.Test);
        }
    }
}
