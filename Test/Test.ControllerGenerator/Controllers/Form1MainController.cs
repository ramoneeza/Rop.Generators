using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.ControllerGenerator.Controllers
{
    public class Form1MainController:BaseController<Form1>
    {
        public string Test=> "I'm a main controller";
        public Form1MainController(Form1 form1):base(form1)
        {
        }
    }
}
