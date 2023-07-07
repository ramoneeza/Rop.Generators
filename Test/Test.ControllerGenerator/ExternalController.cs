using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.ControllerGenerator.Annotations;
using Test.ControllerGenerator.Controllers;

namespace Test.ControllerGenerator
{
    [Controller]
    public class ExternalController:BaseController<Form1>
    {
        public string Test => "I'm an external controller";
        public ExternalController(Form1 form) : base(form)
        {
        }
    }
}
