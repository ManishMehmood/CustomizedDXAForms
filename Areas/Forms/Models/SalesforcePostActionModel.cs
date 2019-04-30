using Sdl.Web.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXA.Modules.Forms.Areas.Forms.Models
{
    [SemanticEntity(EntityName = "SalesforcePostAction", Prefix = "s", Vocab = CoreVocabulary)]
    public class SalesforcePostActionModel : BaseFormPostActionModel
    {
        public string OrganizationId { get; set; }

        public string ParameterTwo { get; set; }

    }
}