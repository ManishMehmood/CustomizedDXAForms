using Sdl.Web.Common.Models;
using System.Collections.Generic;

namespace DXA.Modules.Forms.Areas.Forms.Models
{
    [SemanticEntity(EntityName = "EmailPostAction", Prefix="e", Vocab = CoreVocabulary)]
    public class EmailPostActionModel : BaseFormPostActionModel
    {
        
        [SemanticProperty("e:to")]
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public string Subject { get; set; }
        public RichText EmailBody { get; set; }
    }
}