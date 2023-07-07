using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.ControllerGenerator.Controllers
{
    public abstract class BaseController<T> where T: Form
    {
        public T ParentForm { get; }
        protected BaseController(T parentForm)
        {
            ParentForm = parentForm;
        }
    }
}
