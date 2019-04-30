using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sdl.Web.Common.Models;

namespace DXA.Modules.Forms.Areas.Forms.Models
{
    [SemanticEntity(EntityName = "genericFormCollection", Prefix = "tri", Vocab = CoreVocabulary)]
    public class FormCollectionModel : EntityModel
    {

        [SemanticProperty("tri:title")]
        public string SectionTitle { get; set; }

        [SemanticProperty("tri:description")]
        public RichText Description { get; set; }

        [SemanticProperty("tri:addnDescription")]
        public RichText AdditionalDescription { get; set; }

        [SemanticProperty("tri:forms")]
        public List<FormModel> Forms { get; set; }

        [SemanticProperty("tri:successRedirect")]
        public Link SuccessRedirect { get; set; }
        [SemanticProperty("tri:errorRedirect")]
        public Link ErrorRedirect { get; set; }

        [SemanticProperty("tri:submitButtonLabel")] 
        public string SubmitButtonLabel { get; set; }
        [SemanticProperty("tri:formPostAction")]
        public List<BaseFormPostActionModel> FormPostActions { get; set; }

        [SemanticProperty("tri:customCSS")]
        public Tag CustomCSS { get; set; }

        [SemanticProperty("tri:enableRecaptcha")]
        public string EnableRecaptcha { get; set; }

    }
}