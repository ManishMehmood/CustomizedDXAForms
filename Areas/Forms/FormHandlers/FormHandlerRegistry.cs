using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXA.Modules.Forms.Areas.Forms.FormHandlers
{
    public class FormHandlerRegistry
    {
        private static IDictionary<string, IFormHandler> formHandlers = new Dictionary<string, IFormHandler>();

        public static void RegisterFormHandler(IFormHandler formHandler)
        {
            formHandlers.Add(formHandler.Name, formHandler);
        }

        public static IFormHandler Get(string name)
        {
            IFormHandler formHandler = null;
            formHandlers.TryGetValue(name, out formHandler);
            return formHandler;
        }
    }
}