using Sdl.Web.Common.Models;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using Sdl.Web.Common.Configuration;

namespace DXA.Modules.Forms.Areas.Forms.Models
{
    public class Article : EntityModel
    {
        [SemanticProperty("s:headline")]
        public string Headline { get; set; }
        
        [SemanticProperty("s:dateCreated")]
        public DateTime? Date { get; set; }
        [SemanticProperty("s:about")]
        public string Description { get; set; }
        [SemanticProperty("s:articleBody")]
        public List<Paragraph> ArticleBody { get; set; }
    }
}


