using Sdl.Web.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXA.Modules.Forms.Areas.Forms.Models
{
    //[SemanticEntity(EntityName = "PostActionObject", Prefix = "p", Vocab = "http://www.sdl.com/web/schemas/forms")]
    public abstract class BaseFormPostActionModel : EntityModel
    {
        //[SemanticProperty("p:formHandler")]
        public string FormHandler { get; set; }

    }
}