using DXA.Modules.Forms.Areas.Forms.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXA.Modules.Forms.Areas.Forms
{
    public interface IFormHandler
    {
        string Name { get; }

        void ProcessForm(NameValueCollection formData, List<FormFieldModel> fields, BaseFormPostActionModel model);
        
    }
}