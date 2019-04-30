using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sdl.Web.Common.Models;
using Navient.Modules.Forms.Areas.Forms.Models;

namespace Navient.Modules.Forms.Areas.Forms.Models
{
    [SemanticEntity(EntityName ="Form", Prefix = "a", Vocab = "http://www.sdl.com/web/schemas/forms")]
    public class AjaxFormModel : BaseFormModel
    {
        public string Heading { get; set; }

        public RichText Subheading { get; set; }

        public Article SuccessRedirect { get; set; }
        public Article ErrorRedirect { get; set; }
    }
}