using Sdl.Web.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXA.Modules.Forms.Areas.Forms.Models
{
    [SemanticEntity(EntityName = "AudienceManagerPostAction", Prefix = "a", Vocab = CoreVocabulary)]
    public class AudienceManagerPostActionModel : BaseFormPostActionModel
    {
        public string IdentificationKeyConfig { get; set; }

        public string IdentificationSource { get; set; }

        public string EmailConfirmationIdentifier { get; set; }

        public int AddressBookId { get; set; }

    }
}