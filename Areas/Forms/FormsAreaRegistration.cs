using DXA.Modules.Forms.Areas.Forms.FormHandlers;
using DXA.Modules.Forms.Areas.Forms.Models;
using Sdl.Web.Mvc.Configuration;
using System.Web.Mvc;

namespace DXA.Modules.Forms.Areas.Forms
{
    public class FormsAreaRegistration : BaseAreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Forms";
            }
        }

        protected override void RegisterAllViewModels()
        {
            RegisterViewModel("Form", typeof(FormModel), controllerName: "Forms");
         //   RegisterViewModel("AjaxForm", typeof(AjaxFormModel), controllerName: "Forms");
            RegisterViewModel("FormEmail", typeof(EmailPostActionModel), controllerName: "Forms");
            RegisterViewModel("FormSalesforce", typeof(SalesforcePostActionModel), controllerName: "Forms");
            RegisterViewModel("AudienceManager", typeof(AudienceManagerPostActionModel), controllerName: "Forms");
            RegisterViewModel("GenericFormTile-6-Column",typeof(FormCollectionModel), controllerName: "Forms");


        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            base.RegisterArea(context);

            FormHandlerRegistry.RegisterFormHandler(new EmailFormHandler());
            FormHandlerRegistry.RegisterFormHandler(new SalesforceHandler());

        }
    }
}