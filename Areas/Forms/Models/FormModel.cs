using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sdl.Web.Common.Models;

namespace DXA.Modules.Forms.Areas.Forms.Models
{
    [SemanticEntity(EntityName ="Form", Prefix = "b", Vocab = "http://www.sdl.com/web/schemas/forms")]
    [SemanticEntity(EntityName = "genericFormTile", Prefix = "a", Vocab = CoreVocabulary)]
    public class FormModel : BaseFormModel
    {
       

        [SemanticProperty("a:description")]
        public RichText Description { get; set; }

        [SemanticProperty("a:addnDescription")]
        public RichText AddnDescription { get; set; }

        [SemanticProperty("b:formCustomCSS")]
        public Tag FormCustomCSS { get; set; }

        [SemanticProperty("a:tileCustomCSS")]
        public Tag TileCustomCSS { get; set; }
    }
}