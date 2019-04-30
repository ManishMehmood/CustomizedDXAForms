using Sdl.Web.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXA.Modules.Forms.Areas.Forms.Models
{
    
 [SemanticEntity(EntityName = "genericFormTile", Prefix = "a", Vocab = CoreVocabulary)]
    public class GenericFormTile : EntityModel
    {
        [SemanticProperty("a:heading")]
        public string Heading { get; set; }

        [SemanticProperty("a:subHeading")]
        public string SubHeading { get; set; }

        [SemanticProperty("a:description")]
        public RichText Description { get; set; }

        [SemanticProperty("a:addnDescription")]
        public RichText AddnDescription { get; set; }

        [SemanticProperty("a:tileCustomCSS")]
        public Tag TileCustomCSS { get; set; }
    }
}