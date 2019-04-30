using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sdl.Web.Common.Models;

namespace DXA.Modules.Forms.Areas.Forms.Models
{
   [SemanticEntity(EntityName ="Form", Prefix = "b", Vocab = "http://www.sdl.com/web/schemas/forms")]
    public class BaseFormModel : EntityModel
    {
        public string Heading { get; set; }

        public RichText Subheading { get; set; }

        [SemanticProperty("b:formField")]
        public List<FormFieldModel> FormFields { get; set; }

    }
}